
using System;
using Lab1PD.Core;

namespace Lab1PD.Stack.Array
{
    // Стек на основе массива
    public class Stack<T>
    {
        private const int Size = 256;        // Максимальный размер стека
        private readonly T[] _array = new T[Size];  // Массив для хранения элементов
        private int _last = -1;              // Указатель на вершину стека

        // Добавление элемента на вершину стека
        public void Push(T x) => _array[++_last] = x;  // Увеличиваем указатель и сохраняем элемент

        // Извлечение элемента с вершины
        public T Pop() => _array[_last--];  // Возвращаем элемент и уменьшаем указатель

        // Просмотр вершины без извлечения
        public T Top() => _array[_last];
    
        // Проверка заполненности стека
        public bool Full() => _last == Size - 1;  // Достигнут конец массива

        // Проверка пустоты стека
        public bool Empty() => _last < 0;  // Указатель ниже начала массива

        // Очистка стека
        public void MakeNull() => _last = -1;  // Сбрасываем указатель
    }
}
