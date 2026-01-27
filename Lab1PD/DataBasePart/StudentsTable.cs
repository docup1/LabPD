using Lab1PD.Core.ManyToMany;

namespace Lab1PD.DataBasePart;

/// <summary>
/// Хэш-таблица для хранения объектов StudentNode.
/// Использует открытую адресацию.
/// </summary>
public class StudentsTable
{
    /// <summary> Фиксированный размер таблицы. </summary>
    private const int Size = 256;
    
    /// <summary> Массив для хранения узлов студентов. </summary>
    private readonly StudentNode?[] _items = new StudentNode[Size];

    /// <summary>
    /// Вставляет студента в таблицу. Если имя уже занято активным узлом — вставка игнорируется.
    /// </summary>
    /// <param name="name">Имя студента (ключ).</param>
    /// <param name="newNode">Объект студента.</param>
    public void Insert(char[] name, StudentNode newNode)
    {
        char[] nameChars = PrepareNameArray(name);
        int hash = Hash(nameChars);
        int currentIndex = hash;
        
        int firstTombstoneIndex = -1;
        bool visitedAll = false;

        while (!visitedAll)
        {
            StudentNode? currentNode = _items[currentIndex];

            // 1. Свободная ячейка
            if (currentNode == null)
            {
                int insertAt = (firstTombstoneIndex != -1) ? firstTombstoneIndex : currentIndex;
                _items[insertAt] = newNode;
                return;
            }

            // 2. Проверка на дубликат (сверяем Name напрямую)
            if (ArraysEqual(currentNode.Name, nameChars) && !IsDeleted(currentNode.Name))
            {
                return; 
            }

            // 3. Нашли "надгробие" (удаленный элемент)
            if (IsDeleted(currentNode.Name))
            {
                if (firstTombstoneIndex == -1) firstTombstoneIndex = currentIndex;
            }

            currentIndex = NextHash(currentIndex);
            if (currentIndex == hash) visitedAll = true;
        }

        if (firstTombstoneIndex != -1)
        {
            _items[firstTombstoneIndex] = newNode;
        }
    }

    /// <summary>
    /// Находит студента по имени.
    /// </summary>
    /// <returns>StudentNode или null, если студент не найден или удален.</returns>
    public StudentNode? Get(char[] name)
    {
        char[] nameChars = PrepareNameArray(name);
        int index = FindIndex(nameChars);
        return index != -1 ? _items[index] : null;
    }

    /// <summary>
    /// "Ленивое" удаление студента из таблицы.
    /// </summary>
    public void Remove(char[] name)
    {
        char[] nameChars = PrepareNameArray(name);
        int index = FindIndex(nameChars);

        if (index != -1 && _items[index] != null)
        {
            // Ставим '\0' в начало массива Name — это признак удаленного элемента (Tombstone)
            _items[index]!.Name[0] = '\0';
        }
        else
        {
            throw new InvalidOperationException($"Студент '{name}' не найден в базе.");
        }
    }

    // ================= ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =================

    private int FindIndex(char[] targetName)
    {
        int initialHash = Hash(targetName);
        int currentIndex = initialHash;
        bool visitedAll = false;

        while (!visitedAll)
        {
            StudentNode? node = _items[currentIndex];
            if (node == null) return -1; // Конец цепочки

            if (ArraysEqual(node.Name, targetName) && !IsDeleted(node.Name))
            {
                return currentIndex;
            }

            currentIndex = NextHash(currentIndex);
            if (currentIndex == initialHash) visitedAll = true;
        }
        return -1;
    }

    private int Hash(char[] name)
    {
        int sum = 0;
        int i = 0;
        while (i < name.Length && name[i] != '\0')
        {
            sum += name[i++];
        }
        return sum % Size;
    }

    private int NextHash(int hash) => (hash + 1) % Size;

    private static bool IsDeleted(char[] name) => name.Length > 0 && name[0] == '\0';

    private static bool ArraysEqual(char[] a, char[] b)
    {
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            if (a[i] != b[i]) return false;
            if (a[i] == '\0') return true; 
        }
        return true;
    }

    private static char[] PrepareNameArray(char[] name)
    {
        char[] arr = new char[StudentNode.NameSize + 1];
        int length = Math.Min(name.Length, StudentNode.NameSize);
        for (int i = 0; i < length; ++i) arr[i] = name[i];
        arr[length] = '\0';
        return arr;
    }
}