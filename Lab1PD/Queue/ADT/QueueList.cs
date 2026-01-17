using System;
using Lab1PD.Core;
using Lab1PD.ListADT;

namespace Lab1PD.Queue.List
{
    /// <summary>Очередь на основе АТД списка</summary>
    /// <typeparam name="T">Тип элементов очереди</typeparam>
    /// <remarks>Использует двусвязный список как внутреннюю структуру</remarks>
    public class Queue<T> 
    {
        private IListAdt<T> _list = new DoubleLinkedListAdt<T>();  // Основа очереди - АТД список
    
        /// <summary>Добавляет элемент в конец очереди</summary>
        /// <param name="x">Элемент для добавления</param>
        public void Enqueue(T x) => _list.Add(x);

        /// <summary>Удаляет и возвращает первый элемент очереди</summary>
        /// <returns>Первый элемент очереди</returns>
        /// <exception cref="InvalidOperationException">Очередь пуста</exception>
        public T Dequeue()
        {
            T toReturn = _list.Retrieve(_list.First());
            _list.Delete(_list.First());
            return toReturn;
        }

        /// <summary>Возвращает первый элемент без удаления</summary>
        /// <returns>Первый элемент очереди</returns>
        /// <exception cref="InvalidOperationException">Очередь пуста</exception>
        public T Front() => _list.Retrieve(_list.First());

        /// <summary>Проверяет, заполнена ли очередь (всегда false)</summary>
        public bool Full() => false;

        /// <summary>Проверяет, пуста ли очередь</summary>
        public bool Empty() => _list.First() == _list.End();

        /// <summary>Очищает очередь</summary>
        public void MakeNull() => _list.Makenull();
    }
}