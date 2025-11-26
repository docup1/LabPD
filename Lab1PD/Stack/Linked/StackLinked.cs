using Lab1PD.Core;

namespace Lab1PD.Stack.Linked
{
    /// <summary>
    /// Реализация абстрактного типа данных (АТД) «Стек» на связном списке.
    /// 
    /// Каждый элемент представлен узлом (Node),
    /// который содержит значение и ссылку на следующий узел.
    /// Вершина (_top) указывает на последний добавленный элемент.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в стеке.</typeparam>
    public class StackLinked<T> : IStack<T>
    {
        /// <summary>
        /// Вложенный класс, представляющий элемент (узел) стека.
        /// </summary>
        private class Node
        {
            public T Value;       // Значение элемента
            public Node Next;     // Ссылка на следующий элемент

            public Node(T value, Node next)
            {
                Value = value;
                Next = next;
            }
        }

        private Node _top;       // Ссылка на вершину стека
        private int _count;      // Количество элементов
        private int _capacity;   // Максимальное количество элементов

        /// <summary>
        /// Конструктор. Создает пустой стек с заданной максимальной ёмкостью.
        /// </summary>
        /// <param name="capacity">Максимальное количество элементов в стеке.</param>
        public StackLinked(int capacity)
        {
            _capacity = capacity;
            _top = null;
            _count = 0;
        }

        /// <summary>
        /// Очищает стек, делая его пустым.
        /// </summary>
        public void MakeNull()
        {
            _top = null;
            _count = 0;
        }

        /// <summary>
        /// Добавляет элемент в вершину стека.
        /// </summary>
        /// <param name="x">Элемент для добавления.</param>
        public void Push(T x)
        {
            // Проверка на переполнение
            if (Full())
                throw new InvalidOperationException("Стек переполнен");

            // Создаём новый узел, указывающий на предыдущую вершину
            _top = new Node(x, _top);
            _count++;
        }

        /// <summary>
        /// Удаляет и возвращает элемент из вершины стека.
        /// </summary>
        /// <returns>Элемент, удалённый из стека.</returns>
        public T Pop()
        {
            if (Empty())
                throw new InvalidOperationException("Стек пуст");

            // Сохраняем значение вершины
            T value = _top.Value;

            // Смещаем вершину на следующий узел
            _top = _top.Next;
            _count--;

            return value;
        }

        /// <summary>
        /// Возвращает элемент на вершине стека, не удаляя его.
        /// </summary>
        /// <returns>Элемент, находящийся на вершине стека.</returns>
        public T Top()
        {
            if (Empty())
                throw new InvalidOperationException("Стек пуст");

            return _top.Value;
        }

        /// <summary>
        /// Проверяет, пуст ли стек.
        /// </summary>
        /// <returns>true, если стек пуст; иначе false.</returns>
        public bool Empty()
        {
            return _top == null;
        }

        /// <summary>
        /// Проверяет, заполнен ли стек.
        /// </summary>
        /// <returns>true, если стек заполнен; иначе false.</returns>
        public bool Full()
        {
            return _count >= _capacity;
        }

        /// <summary>
        /// Печатает все элементы стека (для наглядности).
        /// </summary>
        public void Print()
        {
            Console.Write("Стек: ");
            Node current = _top;
            while (current != null)
            {
                Console.Write($"{current.Value} ");
                current = current.Next;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Возвращает текущее количество элементов в стеке.
        /// </summary>
        public int Count()
        {
            return _count;
        }
    }
}
