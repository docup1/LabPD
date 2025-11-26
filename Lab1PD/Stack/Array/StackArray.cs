
using System;
using Lab1PD.Core;

namespace Lab1PD.Stack.Array
{
    /// <summary>
    /// Реализация абстрактного типа данных (АТД) «Стек» на массиве.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в стеке.</typeparam>
    public class StackArray<T> : IStack<T>
    {
        private T[] _items;      // массив для хранения элементов стека
        private int _top;        // индекс вершины стека (последнего элемента)
        private int _capacity;   // максимальный размер стека

        /// <summary>
        /// Конструктор. Создаёт пустой стек заданной ёмкости.
        /// </summary>
        /// <param name="capacity">Максимальное количество элементов в стеке.</param>
        public StackArray(int capacity)
        {
            _capacity = capacity;
            _items = new T[capacity];
            _top = -1;
        }

        /// <summary>
        /// Очищает стек.
        /// </summary>
        public void MakeNull()
        {
            _top = -1;
        }

        /// <summary>
        /// Добавляет элемент в вершину стека.
        /// </summary>
        /// <param name="x">Элемент для добавления.</param>
        public void Push(T x)
        {
            if (Full())
                throw new InvalidOperationException("Стек переполнен");

            _items[++_top] = x;
        }

        /// <summary>
        /// Удаляет и возвращает элемент из вершины стека.
        /// </summary>
        /// <returns>Элемент, удалённый из стека.</returns>
        public T Pop()
        {
            if (Empty())
                throw new InvalidOperationException("Стек пуст");

            return _items[_top--];
        }

        /// <summary>
        /// Возвращает элемент, находящийся на вершине стека, без удаления.
        /// </summary>
        public T Top()
        {
            if (Empty())
                throw new InvalidOperationException("Стек пуст");

            return _items[_top];
        }

        /// <summary>
        /// Проверяет, пуст ли стек.
        /// </summary>
        /// <returns>true, если стек пуст; иначе false.</returns>
        public bool Empty()
        {
            return _top == -1;
        }

        /// <summary>
        /// Проверяет, заполнен ли стек.
        /// </summary>
        /// <returns>true, если стек заполнен; иначе false.</returns>
        public bool Full()
        {
            return _top == _capacity - 1;
        }

        /// <summary>
        /// Печатает содержимое стека (для отладки).
        /// </summary>
        public void Print()
        {
            Console.Write("Стек: ");
            for (int i = 0; i <= _top; i++)
                Console.Write($"{_items[i]} ");
            Console.WriteLine();
        }
    }
}
