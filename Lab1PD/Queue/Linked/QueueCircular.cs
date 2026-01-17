using Lab1PD.Core;

namespace Lab1PD.Queue.Linked
{
    /// <summary>Кольцевая очередь на односвязном списке</summary>
    /// <typeparam name="T">Тип элементов очереди</typeparam>
    /// <remarks>_tail хранит последний элемент, _tail.Next указывает на голову</remarks>
    public class Queue<T>
    {
        private Node? _tail;  // Хвост (последний элемент). null = очередь пуста
    
        /// <summary>Добавляет элемент в конец очереди</summary>
        public void Enqueue(T x)
        {
            if (_tail is null)
            {
                _tail = new Node(x, null);
                _tail.Next = _tail;  // Единственный элемент ссылается сам на себя
                return;
            }

            Node cur = new Node(x, _tail!.Next);  // Новый элемент ссылается на голову
            _tail.Next = cur;  // Старый хвост ссылается на новый элемент
            _tail = cur;       // Новый элемент становится хвостом
        }

        /// <summary>Удаляет и возвращает первый элемент очереди</summary>
        /// <exception cref="NullReferenceException">Очередь пуста</exception>
        public T Dequeue()
        {
            T toReturn = _tail!.Next!.Data;  // Данные головы
            
            if (_tail!.Next == _tail) 
                _tail = null;           // Удаляем единственный элемент
            else 
                _tail.Next = _tail.Next.Next;  // Исключаем голову из кольца
                
            return toReturn;
        }
    
        /// <summary>Возвращает первый элемент без удаления</summary>
        /// <exception cref="NullReferenceException">Очередь пуста</exception>
        public T Front() => _tail!.Next!.Data;

        /// <summary>Проверяет, заполнена ли очередь (всегда false)</summary>
        public bool Full() => false;

        /// <summary>Проверяет, пуста ли очередь</summary>
        public bool Empty() => _tail is null;

        /// <summary>Очищает очередь</summary>
        public void MakeNull() => _tail = null;
    
        /// <summary>Узел кольцевого списка</summary>
        private class Node(T data, Node? next)
        {
            public T Data { get; set; } = data;      // Хранимые данные
            public Node? Next { get; set; } = next;  // Ссылка на следующий узел
        }
    }
}