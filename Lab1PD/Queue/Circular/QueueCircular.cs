namespace Lab1PD.Queue.Circular
{
    /// <summary>
    /// Реализация абстрактного типа данных (АТД) «Очередь»
    /// на кольцевом связном списке.
    /// 
    /// Каждый элемент представлен узлом (Node), содержащим значение
    /// и ссылку на следующий элемент. Хвост (_rear) указывает на последний элемент,
    /// а его Next — на первый (начало очереди).
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в очереди.</typeparam>
    public class QueueCircularAdt<T>
    {
        /// <summary>
        /// Внутренний класс, представляющий элемент очереди (узел).
        /// </summary>
        private class Node
        {
            public T Value;     // Хранимое значение
            public Node Next;   // Ссылка на следующий элемент

            public Node(T value)
            {
                Value = value;
                Next = null;
            }
        }

        private Node _rear;      // Указатель на последний элемент (хвост)
        private int _count;      // Текущее количество элементов
        private int _capacity;   // Максимальное количество элементов

        /// <summary>
        /// Конструктор. Создает пустую очередь заданной ёмкости.
        /// </summary>
        /// <param name="capacity">Максимальное количество элементов.</param>
        public QueueCircularAdt(int capacity)
        {
            _capacity = capacity;
            _rear = null;
            _count = 0;
        }

        /// <summary>
        /// Очищает очередь, делая её пустой.
        /// </summary>
        public void MakeNull()
        {
            _rear = null;
            _count = 0;
        }

        /// <summary>
        /// Добавляет элемент в конец очереди.
        /// </summary>
        /// <param name="x">Элемент для добавления.</param>
        public void Enqueue(T x)
        {
            if (Full())
                throw new InvalidOperationException("Очередь переполнена");

            Node newNode = new Node(x); // Новый узел
            if (_rear == null)
            {
                // Первая вставка — элемент указывает на самого себя
                _rear = newNode;
                _rear.Next = _rear;
            }
            else
            {
                // Вставка после хвоста
                newNode.Next = _rear.Next; // Новый узел указывает на первый
                _rear.Next = newNode;      // Старый хвост указывает на новый
                _rear = newNode;           // Новый узел становится хвостом
            }

            _count++;
        }

        /// <summary>
        /// Удаляет и возвращает первый элемент очереди.
        /// </summary>
        /// <returns>Удалённый элемент.</returns>
        public T Dequeue()
        {
            if (Empty())
                throw new InvalidOperationException("Очередь пуста");

            Node frontNode = _rear.Next; // Первый элемент (после хвоста)
            T value = frontNode.Value;

            if (_rear == frontNode)
            {
                // В очереди один элемент
                _rear = null;
            }
            else
            {
                // Удаляем первый узел, сдвигая ссылку хвоста
                _rear.Next = frontNode.Next;
            }

            _count--;
            return value;
        }

        /// <summary>
        /// Возвращает первый элемент очереди без удаления.
        /// </summary>
        /// <returns>Первый элемент очереди.</returns>
        public T Front()
        {
            if (Empty())
                throw new InvalidOperationException("Очередь пуста");

            return _rear.Next.Value;
        }

        /// <summary>
        /// Проверяет, пуста ли очередь.
        /// </summary>
        /// <returns>true, если очередь пуста; иначе false.</returns>
        public bool Empty()
        {
            return _rear == null;
        }

        /// <summary>
        /// Проверяет, заполнена ли очередь.
        /// </summary>
        /// <returns>true, если очередь заполнена; иначе false.</returns>
        public bool Full()
        {
            return _count >= _capacity;
        }

        /// <summary>
        /// Печатает текущее содержимое очереди (для отладки).
        /// </summary>
        public void Print()
        {
            if (Empty())
            {
                Console.WriteLine("Очередь пуста.");
                return;
            }

            Console.Write("Очередь: ");
            Node current = _rear.Next; // Начинаем с головы
            for (int i = 0; i < _count; i++)
            {
                Console.Write($"{current.Value} ");
                current = current.Next;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Возвращает текущее количество элементов в очереди.
        /// </summary>
        public int Count()
        {
            return _count;
        }
    }
}
