using Lab1PD.Core;
using Lab1PD.Core.CursorList;

namespace Lab1PD.ListADT;

/// <summary>
/// Класс <c>CursorListAdt<T></c>
/// =====================================
/// Реализация абстрактного типа данных (ADT) "список" с использованием курсоров вместо указателей.
///
/// Каждый элемент хранится в статическом массиве <c>_memoryPool</c> фиксированной длины.
/// Узлы связаны между собой индексами (int), а не ссылками.
///
/// Поля:
/// - <c>_head</c> — индекс первого элемента списка (или -1, если список пуст);
/// - <c>_space</c> — индекс первой свободной ячейки в пуле памяти;
/// - <c>_memoryPool</c> — статический массив узлов (Node<T>), представляющий всю "память";
/// - <c>Length</c> — размер пула памяти.
///
/// Принцип:
/// - Свободные ячейки соединены в "список свободных элементов".
/// - Занятые ячейки соединены в пользовательский список.
/// </summary>
public class CursorListAdt<T> : IListADT<T>
{
// -------------------------------
// Поля
// -------------------------------
    private int _head = -1; // индекс головы списка (-1, если список пуст)
    private int _space = 0; // индекс первой свободной ячейки
    private const int Length = 10; // максимальное количество узлов в пуле
    private static Node<T>[] _memoryPool; // пул памяти (хранилище всех узлов)

    // -------------------------------
    // Статический конструктор
    // -------------------------------

    /// <summary>
    /// Инициализация статического пула памяти.
    /// Все элементы соединяются в цепочку свободных ячеек.
    /// Последний элемент ссылается на 0 (как конец списка свободных).
    /// </summary>
    static CursorListAdt()
    {
        _memoryPool = new Node<T>[Length];
        for (int i = 0; i < Length; i++)
            _memoryPool[i] = new Node<T>(i + 1);
        _memoryPool[Length - 1] = new Node<T>(0);
    }

    // -------------------------------
    // Вспомогательные методы
    // -------------------------------

    /// <summary>
    /// Возвращает индекс узла, который находится **перед** узлом с индексом <paramref name="n"/>.
    /// Если элемент не найден — возвращает -1.
    /// </summary>
    private int GetPrevious(int n)
    {
        int curr = _head;
        int tmp = -1;

        while (curr != -1)
        {
            if (n == curr)
                return tmp;

            tmp = curr;
            curr = _memoryPool[curr].Next;
        }

        return -1;
    }

    /// <summary>
    /// Возвращает индекс **предпоследнего** элемента (перед End()).
    /// Используется для вставки в конец списка.
    /// </summary>
    private int GetPreEnd()
    {
        int curr = _head;
        int tmp = -1;

        while (curr != -1)
        {
            tmp = curr;
            curr = _memoryPool[curr].Next;
        }

        return tmp;
    }

    /// <summary>
    /// Поиск узла, содержащего значение <paramref name="x"/>.
    /// Возвращает объект Position с индексом найденного элемента или -1, если элемент отсутствует.
    /// </summary>
    private Position FindElement(T x)
    {
        int current = _head;

        while (current != -1)
        {
            if (Equals(_memoryPool[current].Data, x))
                return new Position(current);

            current = _memoryPool[current].Next;
        }

        return new Position(-1);
    }

    // -------------------------------
    // Методы интерфейса IListADT
    // -------------------------------

    /// <summary>
    /// Вставляет новый элемент <paramref name="x"/> на позицию <paramref name="p"/>.
    /// 
    /// Возможные случаи:
    ///  - <paramref name="p"/> == -1: вставка в конец (или создание списка);
    ///  - <paramref name="p"/> == _head: вставка в начало;
    ///  - вставка в середину списка.
    /// 
    /// Использует пул свободных ячеек (<c>_space</c>).
    /// </summary>
    public void Insert(T x, int p)
    {
        if (_space == -1)
            throw new Exception("Список полон");

        // Вставка в конец или создание нового списка
        if (p == -1)
        {
            if (_head == -1) // если список пуст
            {
                _head = 0;
                _memoryPool[_head].Data = x;
                _memoryPool[_head].Next = -1;
                _space++;
            }
            else
            {
                int last = GetPreEnd();
                int nextSpace = _memoryPool[_space].Next;
                _memoryPool[_space].Data = x;
                _memoryPool[_space].Next = -1;
                _memoryPool[last].Next = _space;
                _space = nextSpace;
            }

            return;
        }

        // Вставка перед первым элементом
        if (p == _head)
        {
            int nextSpace = _memoryPool[_space].Next;
            _memoryPool[_space].Data = x;
            _memoryPool[_space].Next = _head;
            _head = _space;
            _space = nextSpace;
            return;
        }

        // Вставка в середину
        int prev = GetPrevious(p);
        if (prev == -1)
            return;

        int nextFree = _memoryPool[_space].Next;
        _memoryPool[_space].Data = _memoryPool[p].Data;
        _memoryPool[_space].Next = -1;
        _memoryPool[p].Data = x;
        _memoryPool[p].Next = _space;
        _space = nextFree;
    }

    /// <summary>
    /// Удаляет элемент с индексом <paramref name="p"/>.
    /// Возвращает ячейку в пул свободных элементов.
    /// </summary>
    public void Delete(int p)
    {
        if (_head == -1)
            return;

        // Удаление головы списка
        if (p == _head)
        {
            int next = _memoryPool[p].Next;
            _memoryPool[p].Next = _space;
            _space = p;
            _head = next;
            return;
        }

        // Удаление в середине
        int prev = GetPrevious(p);
        if (prev != -1)
        {
            int current = _memoryPool[prev].Next;
            _memoryPool[prev].Next = _memoryPool[current].Next;
            _memoryPool[current].Next = _space;
            _space = current;
        }
    }

    /// <summary>
    /// Возвращает индекс узла, в котором хранится значение <paramref name="x"/>.
    /// Если элемент не найден, возвращает -1.
    /// </summary>
    public int Locate(T x)
    {
        var pos = FindElement(x);
        return pos.N;
    }

    /// <summary>
    /// Возвращает значение, хранящееся в узле с индексом <paramref name="p"/>.
    /// Проверяет корректность позиции.
    /// </summary>
    public T Retrieve(int p)
    {
        if (p >= _memoryPool.Length)
            throw new Exception("Неверно выбрана позиция");

        if (GetPrevious(p) != -1 || p == _head)
            return _memoryPool[p].Data;
        else
            throw new Exception("Такой позиции в списке нет");
    }

    /// <summary>
    /// Возвращает индекс следующего элемента относительно позиции <paramref name="p"/>.
    /// </summary>
    public int Next(int p)
    {
        if (p == _head)
            return _memoryPool[_head].Next;

        int tmpPrev = GetPrevious(p);
        if (tmpPrev == -1)
            throw new Exception("Неверно выбрана позиция");

        tmpPrev = _memoryPool[tmpPrev].Next;
        if (tmpPrev == -1)
            throw new Exception("Неверно выбрана позиция");

        return _memoryPool[tmpPrev].Next;
    }

    /// <summary>
    /// Возвращает индекс предыдущего элемента относительно позиции <paramref name="p"/>.
    /// </summary>
    public int Previous(int p)
    {
        if (p == -1 || p == _head || p >= _memoryPool.Length)
            throw new Exception("Неверно выбрана позиция");

        int tmp = GetPrevious(p);
        if (tmp == -1)
            throw new Exception("Неверно выбрана позиция");

        return tmp;
    }

    /// <summary>
    /// Возвращает фиктивную позицию конца списка (всегда -1).
    /// </summary>
    public int End() => -1;

    /// <summary>
    /// Возвращает индекс первого элемента списка (или -1, если список пуст).
    /// </summary>
    public int First() => _head;

    /// <summary>
    /// Полностью очищает список, возвращая все элементы в пул свободных.
    /// </summary>
    public int Makenull()
    {
        int temp = GetPreEnd();
        if (temp != -1)
        {
            _memoryPool[temp].Next = _space;
            _space = _head;
        }

        _head = -1;
        return End();
    }

    /// <summary>
    /// Печатает содержимое списка в консоль.
    /// </summary>
    public void PrintList()
    {
        if (_head == -1)
        {
            Console.WriteLine("Список пуст");
            return;
        }

        int temp = _head;
        int counter = 0;

        while (temp != -1)
        {
            counter++;
            Console.Write($"{counter}. ");
            Console.WriteLine(_memoryPool[temp].Data);
            temp = _memoryPool[temp].Next;
        }
    }
}