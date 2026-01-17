using System;

namespace Lab1PD.Hashing
{
    /// <summary>
    /// Реализация словаря на основе хеш-таблицы с закрытой адресацией и линейным пробированием.
    /// Использует "ленивое удаление" (пометка ячейки как удаленной символом '\0').
    /// </summary>
    public class CloseHashedDictionary
    {
        private const int TableSize = 256;
        
        // Основной массив для хранения данных (массивов символов)
        private readonly char[]?[] _buckets = new char[TableSize][];

        /// <summary>
        /// Проверяет наличие элемента в множестве.
        /// </summary>
        /// <param name="targetArray">Искомый массив символов.</param>
        /// <returns>True, если элемент найден.</returns>
        public bool Contains(char[] targetArray)
        {
            if (targetArray == null) return false;
            
            int initialHash = CalculateHash(targetArray);
            return FindIndex(targetArray, initialHash) != -1;
        }

        /// <summary>
        /// Вставляет элемент в таблицу, используя линейное пробирование.
        /// Если найдена "удаленная" ячейка, она сохраняется для возможной вставки.
        /// </summary>
        public void Insert(char[] newArray)
        {
            if (newArray == null) return;

            int initialHash = CalculateHash(newArray);
            int currentIndex = initialHash;
            int firstTombstoneIndex = -1; // Индекс первой встреченной удаленной ячейки

            // Проходим по таблице, пока не вернемся в начало или не найдем пустую ячейку
            bool visitedAll = false;
            while (!visitedAll)
            {
                char[]? currentData = _buckets[currentIndex];

                // 1. Нашли абсолютно пустую ячейку (никогда не заполнялась)
                if (currentData == null)
                {
                    int insertAt = (firstTombstoneIndex == -1) ? currentIndex : firstTombstoneIndex;
                    _buckets[insertAt] = newArray;
                    return;
                }

                // 2. Нашли дубликат (алгоритм множества)
                if (AreArraysEqual(currentData, newArray))
                {
                    return;
                }

                // 3. Нашли удаленную ячейку (tombstone)
                if (IsDeleted(currentIndex) && firstTombstoneIndex == -1)
                {
                    firstTombstoneIndex = currentIndex;
                }

                currentIndex = GetNextIndex(currentIndex);
                if (currentIndex == initialHash) visitedAll = true;
            }

            // Если прошли круг и нашли удаленное место — вставляем туда
            if (firstTombstoneIndex != -1)
            {
                _buckets[firstTombstoneIndex] = newArray;
            }
        }

        /// <summary>
        /// Выполняет "ленивое удаление": помечает первый символ массива как '\0'.
        /// </summary>
        public void Remove(char[] targetArray)
        {
            if (targetArray == null) return;

            int initialHash = CalculateHash(targetArray);
            int index = FindIndex(targetArray, initialHash);

            if (index != -1)
            {
                // Устанавливаем маркер удаления (первый символ = 0)
                _buckets[index]![0] = '\0';
            }
        }

        /// <summary>
        /// Очищает хеш-таблицу.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < TableSize; ++i)
            {
                _buckets[i] = null;
            }
        }

        /// <summary>
        /// Печатает все активные элементы (пропуская null и удаленные).
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < TableSize; ++i)
            {
                if (_buckets[i] != null && !IsDeleted(i))
                {
                    Console.Write($"{new string(_buckets[i]!)} ");
                }
            }
            Console.WriteLine();
        }

        // -----------------------------------------------------------------------
        // Вспомогательные методы
        // -----------------------------------------------------------------------

        /// <summary>
        /// Ищет индекс элемента в таблице с учетом линейного пробирования.
        /// </summary>
        private int FindIndex(char[] targetArray, int initialHash)
        {
            int currentIndex = initialHash;
            
            bool visitedAll = false;
            while (!visitedAll)
            {
                if (AreArraysEqual(_buckets[currentIndex], targetArray))
                {
                    return currentIndex;
                }

                currentIndex = GetNextIndex(currentIndex);
                if (currentIndex == initialHash) visitedAll = true;
            }

            return -1;
        }

        /// <summary>
        /// Хеш-функция (сумма кодов символов до терминатора).
        /// </summary>
        private static int CalculateHash(char[] array)
        {
            int sum = 0;
            int i = 0;
            while (i < array.Length && array[i] != '\0')
            {
                sum += array[i++];
            }
            return sum % TableSize;
        }

        /// <summary>
        /// Линейное пробирование: переход к следующему индексу по кругу.
        /// </summary>
        private static int GetNextIndex(int currentIndex)
        {
            return (currentIndex + 1) % TableSize;
        }

        /// <summary>
        /// Проверяет, является ли ячейка помеченной как "удаленная".
        /// </summary>
        private bool IsDeleted(int index)
        {
            // Ячейка считается удаленной, если массив не null, но его первый символ '\0'
            return _buckets[index] != null && _buckets[index]![0] == '\0';
        }

        /// <summary>
        /// Сравнение содержимого двух массивов.
        /// </summary>
        private static bool AreArraysEqual(char[]? arrayA, char[]? arrayB)
        {
            if (ReferenceEquals(arrayA, arrayB)) return true;
            if (arrayA == null || arrayB == null) return false;
            if (arrayA.Length != arrayB.Length) return false;

            for (int i = 0; i < arrayA.Length; i++)
            {
                if (arrayA[i] != arrayB[i]) return false;
            }
            return true;
        }
    }
}