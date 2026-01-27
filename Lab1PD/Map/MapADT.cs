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
        /// </summary>
        /// <param name="key">Ключ для поиска</param>
        /// <returns>
        /// - Если ключ найден: найденный узел
        /// - Если ключ не найден: null
        /// </returns>
        private Node? FindNodeByKey(TKey key)
        {
            Node? currentNode = _head;
            
            while (currentNode != null)
            {
                if (currentNode.Key.Equals(key))
                {
                    return currentNode;
                }
                currentNode = currentNode.Next;
            }
            
            return null;
        }

        /// <summary>
        /// Находит последний узел в списке.
        /// </summary>
        /// <returns>Последний узел списка или null, если список пуст</returns>
        private Node? FindLastNode()
        {
            if (_head == null)
                return null;
                
            Node? currentNode = _head;
            while (currentNode.Next != null)
            {
                currentNode = currentNode.Next;
            }
            return currentNode;
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
            // Ищем узел с таким ключом
            Node? existingNode = FindNodeByKey(key);
            
            if (existingNode != null)
            {
                // Ключ найден - обновляем значение
                existingNode.Value = value;
            }
            else
            {
                // Ключ не найден - добавляем новый узел в КОНЕЦ списка
                Node? lastNode = FindLastNode();
                
                if (lastNode == null)
                {
                    // Список пустой - создаем голову
                    _head = new Node(key, value, null);
                }
                else
                {
                    // Добавляем после последнего узла
                    lastNode.Next = new Node(key, value, null);
                }
            }
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
            Node? foundNode = FindNodeByKey(key);
            
            if (foundNode != null)
            {
                value = foundNode.Value;
                return true;
            }
            
            return false;
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
            bool first = true;

            while (currentNode != null)
            {
                if (!first)
                {
                    Console.Write(", ");
                }
                Console.Write(currentNode);
                first = false;
                currentNode = currentNode.Next;
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