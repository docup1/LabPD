namespace Lab1PD.Queue.List
{
    /// <summary>
    /// Очередь на связном списке.
    /// </summary>
    public class QueueList<T>
    {
        /// <summary>
        /// Узел связного списка.
        /// </summary>
        private class Node
        {
            public T Data;
            public Node Next;

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node _front; // первый элемент (начало очереди)
        private Node _rear;  // последний элемент (конец очереди)
        private int _count;  // количество элементов в очереди

        public QueueList()
        {
            MakeNull();
        }

        /// <summary>
        /// Очищает очередь (делает её пустой).
        /// </summary>
        public void MakeNull()
        {
            _front = null;
            _rear = null;
            _count = 0;
        }

        /// <summary>
        /// Возвращает первый элемент очереди (без удаления).
        /// </summary>
        public T Front()
        {
            if (Empty())
                throw new InvalidOperationException("Очередь пуста");
            return _front.Data;
        }

        /// <summary>
        /// Удаляет и возвращает первый элемент очереди.
        /// </summary>
        public T Dequeue()
        {
            if (Empty())
                throw new InvalidOperationException("Очередь пуста");

            T value = _front.Data;
            _front = _front.Next;
            _count--;

            if (_front == null)
                _rear = null; // если очередь опустела

            return value;
        }

        /// <summary>
        /// Добавляет элемент в конец очереди.
        /// </summary>
        public void Enqueue(T x)
        {
            Node node = new Node(x);

            if (Empty())
            {
                _front = node;
                _rear = node;
            }
            else
            {
                _rear.Next = node;
                _rear = node;
            }

            _count++;
        }

        /// <summary>
        /// Проверяет, пуста ли очередь.
        /// </summary>
        public bool Empty()
        {
            return _front == null;
        }

        /// <summary>
        /// Проверяет, полна ли очередь (для списка — никогда).
        /// </summary>
        public bool Full()
        {
            return false; // список неограничен
        }

        /// <summary>
        /// Возвращает количество элементов в очереди.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Возвращает строковое представление очереди.
        /// </summary>
        public override string ToString()
        {
            var current = _front;
            string result = "Front -> ";

            while (current != null)
            {
                result += current.Data + " ";
                current = current.Next;
            }

            return result + "<- Rear";
        }
    }
}
