namespace Lab1PD.Hashing;

public class Dictionary
{
    private const int Size = 10;                           // Фиксированный размер хеш-таблицы
    private readonly Node?[] _nodes = new Node[Size];      // Массив цепочек (списков)
    
    // Проверка принадлежности элемента множеству
    public bool Member(char[] x)
    {
        int hash = Hash(x);
        return Contains(x, hash);
    }

    // Поиск элемента в цепочке по хешу
    private bool Contains(char[] x, int hash)
    {
        Node? cur = _nodes[hash];                          // Начало цепочки
        while (cur != null)
        {
            if (ArraysEqual(cur.Data, x)) return true;     // Элемент найден
            cur = cur.Next;                                // Переход к следующему узлу
        }

        return false;                                      // Элемент не найден в цепочке
    }
    
    // Очистка хеш-таблицы (разрыв всех цепочек)
    public void MakeNull()
    {
        for (int i = 0; i < Size; ++i)
        {
            _nodes[i] = null;                              // Обнуляем все головы списков
        }
    }

    // Вставка элемента в хеш-таблицу
    public void Insert(char[] x)
    {
        int hash = Hash(x);
        if (Contains(x, hash)) return;                     // Элемент уже существует
        
        Node? head = _nodes[hash];                         // Текущая голова цепочки
        _nodes[hash] = new Node(x, head);                  // Вставка в начало цепочки
    }

    // Удаление элемента из хеш-таблицы
    public void Delete(char[] x)
    {
        int hash = Hash(x);
        Node? cur = _nodes[hash];                          // Начало цепочки

        if (cur == null) return;                           // Цепочка пуста

        // Проверка первого узла в цепочке
        if (ArraysEqual(cur.Data, x))
        {
            _nodes[hash] = cur.Next;                       // Удаление головы цепочки
            return;
        }
        
        // Поиск в остальной части цепочки
        Node prev = cur;                                  // Предыдущий узел
        cur = cur.Next;                                    // Текущий узел
        while (cur != null)
        {
            if (ArraysEqual(cur.Data, x))
            {
                prev.Next = cur.Next;                      // Пропускаем удаляемый узел
                return;
            }
            prev = cur;
            cur = cur.Next;
        }
    }

    // Вывод всех элементов хеш-таблицы
    public void Print()
    {
        for (int i = 0; i < Size; ++i)                    // Обход всех цепочек
        {
            Node? node = _nodes[i];                        // Глава текущей цепочки
            while (node != null)
            {
                Console.Write($"{new string(node.Data)} "); // Вывод элемента
                node = node.Next;                          // Переход к следующему узлу
            }
        }
        Console.WriteLine();
    }

    // Хеш-функция: сумма кодов символов до '\0' по модулю Size
    private static int Hash(char[] obj)
    {
        int sum = 0;
        int i = 0;
        while (i < obj.Length && obj[i] != '\0')          // Суммируем до нулевого символа
        {
            sum += obj[i++];
        }

        return sum % Size;                                // Приведение к диапазону таблицы
    }

    // Сравнение двух массивов символов (с учетом null)
    private static bool ArraysEqual(char[]? a, char[]? b)
    {
        if (a == null || b == null) return a == b;        // Оба null или один null
    
        if (a.Length != b.Length) return false;           // Разная длина
        
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i]) return false;               // Найден несовпадающий символ
        }
        
        return true;                                      // Массивы идентичны
    }
    
    // Узел связного списка для хранения элементов цепочки
    private class Node(char[] data, Node? next = null)
    {
        public char[] Data { get; set; } = data;          // Данные узла (массив символов)
        public Node? Next { get; set; } = next;           // Ссылка на следующий узел
    }
}