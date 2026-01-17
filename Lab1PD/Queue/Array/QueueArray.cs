using Lab1PD.Core;

namespace Lab1PD.Queue.Array
{
    /// <summary>Кольцевая очередь на фиксированном массиве</summary>
    /// <typeparam name="T">Тип элементов очереди</typeparam>
    /// <remarks>Использует кольцевой буфер размером 256 элементов</remarks>
    public class Queue<T>
    {
        private const int Size = 256;               // Фиксированный размер очереди
        private readonly T[] _array = new T[Size];  // Кольцевой буфер
        private int _front = 0;                     // Индекс начала очереди
        private int _rear = Size - 1;               // Индекс конца очереди (предыдущий от первого свободного)
    
        /// <summary>Добавляет элемент в конец очереди</summary>
        /// <param name="x">Элемент для добавления</param>
        /// <exception cref="InvalidOperationException">Очередь переполнена</exception>
        public void Enqueue(T x)
        {
            _rear = Next(_rear);  // Перемещаем указатель конца
            _array[_rear] = x;    // Сохраняем элемент
        }

        /// <summary>Удаляет и возвращает первый элемент очереди</summary>
        /// <returns>Первый элемент очереди</returns>
        /// <exception cref="InvalidOperationException">Очередь пуста</exception>
        public T Dequeue()
        {
            T toReturn = _array[_front];
            _front = Next(_front);  // Перемещаем указатель начала
            return toReturn;
        }

        /// <summary>Возвращает первый элемент без удаления</summary>
        /// <returns>Первый элемент очереди</returns>
        /// <exception cref="InvalidOperationException">Очередь пуста</exception>
        public T Front() => _array[_front];

        /// <summary>Проверяет, заполнена ли очередь</summary>
        /// <remarks>Очередь считается заполненной, если свободна только одна ячейка</remarks>
        public bool Full() => Next(Next(_rear)) == _front;

        /// <summary>Проверяет, пуста ли очередь</summary>
        public bool Empty() => Next(_rear) == _front;

        /// <summary>Очищает очередь</summary>
        public void MakeNull() => _rear = _front - 1;  // Сбрасываем указатель конца

        /// <summary>Вычисляет следующий индекс в кольцевом буфере</summary>
        /// <param name="pos">Текущий индекс</param>
        /// <returns>Следующий индекс (с зацикливанием)</returns>
        private int Next(int pos) => (pos + 1) % Size;
    }
}