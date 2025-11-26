
namespace Lab1PD.Queue.Array
{
    /// <summary>
    /// Реализация абстрактного типа данных (АТД) «Очередь» на массиве.
    /// 
    /// Элементы добавляются в конец очереди (Enqueue)
    /// и извлекаются из начала (Dequeue).
    /// Используется циклический буфер для эффективного использования памяти.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в очереди.</typeparam>
    public class QueueArrayAdt<T>
    {
        private T[] _items;       // Массив для хранения элементов очереди
        private int _front;       // Индекс начала очереди
        private int _rear;        // Индекс конца очереди
        private int _count;       // Текущее количество элементов
        private int _capacity;    // Максимальный размер очереди

        /// <summary>
        /// Конструктор. Создает пустую очередь заданной ёмкости.
        /// </summary>
        /// <param name="capacity">Максимальное количество элементов в очереди.</param>
        public QueueArrayAdt(int capacity)
        {
            _capacity = capacity;
            _items = new T[capacity];
            _front = 0;
            _rear = -1;
            _count = 0;
        }

        /// <summary>
        /// Очищает очередь, делая её пустой.
        /// </summary>
        public void MakeNull()
        {
            _front = 0;
            _rear = -1;
            _count = 0;
        }

        /// <summary>
        /// Добавляет элемент в конец очереди.
        /// </summary>
        /// <param name="x">Элемент, который нужно добавить.</param>
        public void Enqueue(T x)
        {
            if (Full())
                throw new InvalidOperationException("Очередь переполнена");

            // Циклическое смещение конца
            _rear = (_rear + 1) % _capacity;
            _items[_rear] = x;
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

            T value = _items[_front];
            _front = (_front + 1) % _capacity;
            _count--;
            return value;
        }

        /// <summary>
        /// Возвращает первый элемент очереди, не удаляя его.
        /// </summary>
        /// <returns>Первый элемент очереди.</returns>
        public T Front()
        {
            if (Empty())
                throw new InvalidOperationException("Очередь пуста");

            return _items[_front];
        }

        /// <summary>
        /// Проверяет, пуста ли очередь.
        /// </summary>
        /// <returns>true, если очередь пуста; иначе false.</returns>
        public bool Empty()
        {
            return _count == 0;
        }

        /// <summary>
        /// Проверяет, заполнена ли очередь.
        /// </summary>
        /// <returns>true, если очередь заполнена; иначе false.</returns>
        public bool Full()
        {
            return _count == _capacity;
        }

        /// <summary>
        /// Печатает текущее содержимое очереди (для отладки).
        /// </summary>
        public void Print()
        {
            Console.Write("Очередь: ");
            for (int i = 0; i < _count; i++)
            {
                int index = (_front + i) % _capacity;
                Console.Write($"{_items[index]} ");
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
