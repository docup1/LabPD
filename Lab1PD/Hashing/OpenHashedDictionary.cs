using System;
using Lab1PD.Hashing;

namespace Lab1PD.Hashing
{
    /// <summary>
    /// Реализация словаря (множества) на основе хеш-таблицы с методом цепочек (Open Hashing / Separate Chaining).
    /// </summary>
    public class OpenHashedDictionary
    {
        // Константа размера таблицы (фиксирована по условию задачи)
        private const int TableSize = 256;

        // Массив "корзин" (buckets). Каждый элемент — это голова связного списка.
        private readonly Node?[] _buckets = new Node[TableSize];

        /// <summary>
        /// Проверяет, содержится ли указанный массив символов в таблице.
        /// </summary>
        /// <param name="targetArray">Искомый массив символов.</param>
        /// <returns>True, если элемент найден, иначе False.</returns>
        public bool Contains(char[] targetArray)
        {
            if (targetArray == null) return false;

            int hashIndex = CalculateHashIndex(targetArray);
            return FindInBucket(targetArray, hashIndex);
        }

        /// <summary>
        /// Вставляет новый элемент в хеш-таблицу.
        /// Если элемент уже существует, вставка игнорируется.
        /// </summary>
        /// <param name="newArray">Массив символов для вставки.</param>
        public void Insert(char[] newArray)
        {
            if (newArray == null) return;

            int hashIndex = CalculateHashIndex(newArray);

            // Если элемент уже есть в цепочке, ничего не делаем
            if (FindInBucket(newArray, hashIndex)) return;

            // Вставка в начало цепочки (метод цепочек)
            Node? currentHead = _buckets[hashIndex];
            _buckets[hashIndex] = new Node(newArray, currentHead);
        }

        /// <summary>
        /// Удаляет указанный элемент из хеш-таблицы.
        /// </summary>
        /// <param name="targetArray">Массив символов для удаления.</param>
        public void Remove(char[] targetArray)
        {
            if (targetArray == null) return;

            int hashIndex = CalculateHashIndex(targetArray);
            Node? currentNode = _buckets[hashIndex];

            if (currentNode == null) return; // Цепочка пуста

            // Случай 1: Удаляемый элемент находится в голове цепочки
            if (AreArraysEqual(currentNode.Data, targetArray))
            {
                _buckets[hashIndex] = currentNode.Next;
                return;
            }

            // Случай 2: Поиск элемента в середине или конце цепочки
            Node previousNode = currentNode;
            currentNode = currentNode.Next;

            while (currentNode != null)
            {
                if (AreArraysEqual(currentNode.Data, targetArray))
                {
                    // Исключаем узел из списка, перекидывая ссылку
                    previousNode.Next = currentNode.Next;
                    return;
                }

                previousNode = currentNode;
                currentNode = currentNode.Next;
            }
        }

        /// <summary>
        /// Очищает хеш-таблицу, удаляя все элементы.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < TableSize; ++i)
            {
                _buckets[i] = null; // Сборщик мусора удалит отвязанные узлы
            }
        }

        /// <summary>
        /// Выводит содержимое хеш-таблицы в консоль.
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < TableSize; ++i)
            {
                Node? currentNode = _buckets[i];
                
                // Проходим по всей цепочке в текущей корзине
                while (currentNode != null)
                {
                    Console.Write($"{new string(currentNode.Data)} ");
                    currentNode = currentNode.Next;
                }
            }
            Console.WriteLine();
        }

        // -----------------------------------------------------------------------
        // Private Helpers (Внутренняя логика)
        // -----------------------------------------------------------------------

        /// <summary>
        /// Ищет элемент в конкретной цепочке (bucket) по хешу.
        /// </summary>
        private bool FindInBucket(char[] targetArray, int hashIndex)
        {
            Node? currentNode = _buckets[hashIndex];
            
            while (currentNode != null)
            {
                if (AreArraysEqual(currentNode.Data, targetArray))
                {
                    return true;
                }
                currentNode = currentNode.Next;
            }

            return false;
        }

        /// <summary>
        /// Хеш-функция: вычисляет индекс на основе суммы кодов символов.
        /// Учитывает символы до '\0' (алгоритм сохранен).
        /// </summary>
        private static int CalculateHashIndex(char[] array)
        {
            int sum = 0;
            int i = 0;

            // Алгоритм суммирует коды символов до конца массива или до терминатора '\0'
            while (i < array.Length && array[i] != '\0')
            {
                sum += array[i];
                i++;
            }

            return sum % TableSize;
        }

        /// <summary>
        /// Поэлементное сравнение двух массивов символов.
        /// </summary>
        private static bool AreArraysEqual(char[]? arrayA, char[]? arrayB)
        {
            // Ссылка на один и тот же объект или оба null
            if (ReferenceEquals(arrayA, arrayB)) return true;
            
            // Если один из них null (а второй нет, т.к. проверка выше прошла)
            if (arrayA == null || arrayB == null) return false;

            if (arrayA.Length != arrayB.Length) return false;

            for (int i = 0; i < arrayA.Length; i++)
            {
                if (arrayA[i] != arrayB[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Внутренний класс узла связного списка.
        /// </summary>
        private class Node
        {
            public char[] Data { get; }
            public Node? Next { get; set; }

            public Node(char[] data, Node? next = null)
            {
                Data = data;
                Next = next;
            }
        }
    }
}

