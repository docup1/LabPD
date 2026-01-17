using Lab1PD.Core;

namespace Lab1PD.Map
{
    /// <summary>
    /// Реализация словаря (ассоциативного массива) на основе односвязного списка.
    /// Предоставляет операции добавления, поиска и удаления элементов по ключу.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа элемента словаря</typeparam>
    /// <typeparam name="TValue">Тип значения элемента словаря</typeparam>
    public class MapADT<TKey, TValue> 
    {
        // Головной элемент односвязного списка
        private Node? _head;

        /// <summary>
        /// Находит узел с заданным ключом в списке.
        /// Если узел не найден, возвращает последний узел для оптимизации вставки.
        /// </summary>
        /// <param name="key">Ключ для поиска</param>
        /// <param name="foundNode">
        /// Выходной параметр:
        /// - Если ключ найден: ссылка на найденный узел
        /// - Если ключ не найден: ссылка на последний узел списка (или null для пустого списка)
        /// </param>
        /// <returns>
        /// true - если узел с заданным ключом найден,
        /// false - если узел не найден
        /// </returns>
        private bool FindNodeByKey(TKey key, ref Node? foundNode)
        {
            Node? currentNode = _head;
            Node? previousNode = null;
            
            // Проход по всем узлам списка
            while (currentNode != null)
            {
                // Сравнение ключей (использует метод Equals)
                if (currentNode.Key.Equals(key))
                {
                    foundNode = currentNode;  // Узел найден
                    return true;
                }

                // Переход к следующему узлу
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            // Ключ не найден - возвращаем последний узел
            foundNode = previousNode;
            return false;
        }

        /// <summary>
        /// Проверяет, пуст ли словарь.
        /// </summary>
        /// <returns>true - если словарь пуст, false - в противном случае</returns>
        private bool IsEmpty()
        {
            return _head is null;
        }
        
        /// <summary>
        /// Добавляет или обновляет значение по указанному ключу.
        /// Если ключ уже существует, обновляет его значение.
        /// Если ключ не существует, добавляет новую пару ключ-значение в конец списка.
        /// </summary>
        /// <param name="key">Ключ для добавления/обновления</param>
        /// <param name="value">Значение, ассоциированное с ключом</param>
        public void Assign(TKey key, TValue value)
        {
            // Обработка случая пустого списка
            if (IsEmpty())
            {
                _head = new Node(key, value, null);
                return;
            }

            Node? targetNode = null;
            
            // Поиск узла с заданным ключом
            if (FindNodeByKey(key, ref targetNode))
            {
                // Ключ найден - обновляем значение
                targetNode!.Value = value;
                return;
            }

            // Ключ не найден - добавляем новый узел в конец списка
            targetNode!.Next = new Node(key, value, null);
        }

        /// <summary>
        /// Получает значение по указанному ключу.
        /// </summary>
        /// <param name="key">Ключ для поиска значения</param>
        /// <param name="value">
        /// Выходной параметр для получения значения.
        /// Если ключ найден, содержит ассоциированное значение.
        /// </param>
        /// <returns>
        /// true - если ключ найден и значение записано в параметр value,
        /// false - если ключ не найден
        /// </returns>
        public bool Compute(TKey key, ref TValue value)
        {
            Node? targetNode = null;
            
            // Поиск узла с заданным ключом
            if (!FindNodeByKey(key, ref targetNode))
                return false;
            
            // Возврат значения через выходной параметр
            value = targetNode!.Value;
            return true;
        }

        /// <summary>
        /// Очищает словарь, удаляя все элементы.
        /// </summary>
        public void MakeNull()
        {
            _head = null;
        }

        /// <summary>
        /// Выводит все элементы словаря в консоль в формате JSON-подобного объекта.
        /// Формат вывода: {key1: value1, key2: value2, ...}
        /// </summary>
        public void PrintList()
        {
            Console.Write("{");
            
            Node? currentNode = _head;

            // Вывод первого элемента (без запятой)
            if (currentNode != null)
            {
                Console.Write(currentNode);
                currentNode = currentNode.Next;
                
                // Вывод остальных элементов (с запятыми)
                while (currentNode != null)
                {
                    Console.Write($", {currentNode}");
                    currentNode = currentNode.Next;
                }
            }

            Console.WriteLine("}");
        }

        /// <summary>
        /// Внутренний класс, представляющий узел односвязного списка.
        /// Хранит пару ключ-значение и ссылку на следующий узел.
        /// </summary>
        private class Node
        {
            /// <summary>
            /// Ключ элемента словаря.
            /// </summary>
            public TKey Key { get; set; }
            
            /// <summary>
            /// Значение, ассоциированное с ключом.
            /// </summary>
            public TValue Value { get; set; }
            
            /// <summary>
            /// Ссылка на следующий узел в списке.
            /// </summary>
            public Node? Next { get; set; }

            /// <summary>
            /// Создает новый узел с указанными параметрами.
            /// </summary>
            /// <param name="key">Ключ узла</param>
            /// <param name="value">Значение узла</param>
            /// <param name="next">Ссылка на следующий узел</param>
            public Node(TKey key, TValue value, Node? next)
            {
                Key = key;
                Value = value;
                Next = next;
            }

            /// <summary>
            /// Возвращает строковое представление узла в формате "ключ: значение".
            /// </summary>
            /// <returns>Строковое представление узла</returns>
            public override string ToString()
            {
                return $"{Key}: {Value}";
            }
        }
    }
}