using Lab1PD.Core.ManyToMany;

namespace Lab1PD.DataBasePart;

/// <summary>
/// Хэш-таблица для объектов CourseNode. 
/// Использует открытую адресацию.
/// </summary>
public class CoursesTable
{
    /// <summary> Фиксированный размер таблицы. </summary>
    private const int Size = 256;
    
    /// <summary> Массив для хранения узлов курсов. </summary>
    private readonly CourseNode?[] _items = new CourseNode[Size];

    /// <summary>
    /// Вставляет новый курс в таблицу. 
    /// Если курс с таким именем уже существует — вставка игнорируется.
    /// </summary>
    /// <param name="name">Название курса.</param>
    /// <param name="newNode">Объект узла курса.</param>
    public void Insert(char[] name, CourseNode newNode)
    {
        char[] nameChars = PrepareNameArray(name);
        int hash = Hash(nameChars);
        int currentIndex = hash;
        
        int firstTombstoneIndex = -1;
        bool visitedAll = false;

        while (!visitedAll)
        {
            CourseNode? currentNode = _items[currentIndex];

            // 1. Нашли пустую ячейку
            if (currentNode == null)
            {
                int insertAt = (firstTombstoneIndex != -1) ? firstTombstoneIndex : currentIndex;
                _items[insertAt] = newNode;
                return;
            }

            // 2. Нашли существующий элемент (сверяем имена)
            if (ArraysEqual(currentNode.Name, nameChars) && !IsDeleted(currentNode.Name))
            {
                return; // Дубликаты не вставляем
            }

            // 3. Нашли "надгробие" (удаленный элемент)
            if (IsDeleted(currentNode.Name))
            {
                if (firstTombstoneIndex == -1) firstTombstoneIndex = currentIndex;
            }

            currentIndex = NextHash(currentIndex);
            if (currentIndex == hash) visitedAll = true;
        }

        // Если таблица была полна "надгробий", вставляем в первое найденное
        if (firstTombstoneIndex != -1)
        {
            _items[firstTombstoneIndex] = newNode;
        }
    }

    /// <summary>
    /// Ищет курс по названию.
    /// </summary>
    /// <returns>Узел курса или null, если ничего не найдено.</returns>
    public CourseNode? Get(char[] name)
    {
        char[] nameChars = PrepareNameArray(name);
        int index = FindIndex(nameChars);
        return index != -1 ? _items[index] : null;
    }

    /// <summary>
    /// Удаляет курс из таблицы (маркирует как удаленный).
    /// </summary>
    public void Remove(char[] name)
    {
        char[] nameChars = PrepareNameArray(name);
        int index = FindIndex(nameChars);

        if (index != -1 && _items[index] != null)
        {
            // Ставим '\0' в начало массива Name — это помечает ячейку как Tombstone.
            _items[index]!.Name[0] = '\0';
        }
        else
        {
            throw new InvalidOperationException($"Курс '{name}' не найден.");
        }
    }

    // --- Вспомогательные механизмы ---

    private int FindIndex(char[] targetName)
    {
        int initialHash = Hash(targetName);
        int currentIndex = initialHash;
        bool visitedAll = false;

        while (!visitedAll)
        {
            CourseNode? node = _items[currentIndex];
            if (node == null) return -1; // Цепочка коллизий прервана

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
        // Используем константу напрямую из CourseNode
        char[] arr = new char[CourseNode.NameSize + 1];
        int length = Math.Min(name.Length, CourseNode.NameSize);
        for (int i = 0; i < length; ++i) arr[i] = name[i];
        arr[length] = '\0';
        return arr;
    }
}