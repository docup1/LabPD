using System;
using Lab1PD.Core;
using Lab1PD.ListADT;

namespace Lab1PD.Stack.ADT
{
    public class Stack<T> 
    {
        private readonly IListAdt<T> _list = new DoubleLinkedListAdt<T> ();  // Основа стека

        // Добавление элемента на вершину стека
        public void Push(T x) => _list.Insert(x, _list.First());  // Вставляем в начало списка

        // Извлечение элемента с вершины
        public T Pop()
        {
            T toReturn = _list.Retrieve(_list.First());
            _list.Delete(_list.First());  // Удаляем первый элемент
            return toReturn;
        }

        // Просмотр вершины без извлечения
        public T Top() => _list.Retrieve(_list.First());

        // Проверка пустоты стека
        public bool Empty() => _list.First() == _list.End();

        // Стек на списке никогда не заполняется
        public bool Full() => false;

        // Очистка стека
        public void MakeNull() => _list.Makenull();
    }
}