// ===============================
// Реализация через двусвязный список
// ===============================

using Lab1PD.Core;

namespace Lab1PD.ListADT
{
    using Lab1PD.ListADT;
    
    public class DoubleLinkedListADT<T> : IListADT<T>
    {
        private DoubleLinkedListNode<T>? head;
        private DoubleLinkedListNode<T>? tail;
        private int count;

        public DoubleLinkedListADT()
        {
            head = null;
            tail = null;
            count = 0;
        }

        // Возвращает позицию после последнего
        public int End() => count + 1;

        // Вставка элемента
        public void Insert(T x, int p)
        {
            ValidatePosition(p, allowEnd: true);
            DoubleLinkedListNode<T> newNode = new DoubleLinkedListNode<T>(x);

            if (p == 1) // вставка в начало
            {
                LinkNodes(newNode, head);
                head = newNode;
                if (tail == null) tail = newNode;
            }
            else if (p == count + 1) // вставка в конец
            {
                LinkNodes(tail, newNode);
                tail = newNode;
            }
            else // вставка в середину
            {
                var current = GetNodeAt(p);
                LinkNodes(current!.Prev, newNode);
                LinkNodes(newNode, current);
            }
            count++;
        }

        // Найти позицию
        public int Locate(T x)
        {
            DoubleLinkedListNode<T>? current = head;
            int pos = 1;
            while (current != null)
            {
                if (Equals(current.Data, x)) return pos;
                current = current.Next;
                pos++;
            }
            return End();
        }

        // Получить элемент
        public T Retrieve(int p) => GetNodeAt(p)!.Data;

        // Удалить элемент
        public void Delete(int p)
        {
            var current = GetNodeAt(p);
            LinkNodes(current!.Prev, current.Next);
            if (current == head) head = current.Next;
            if (current == tail) tail = current.Prev;
            count--;
        }



        // Следующая позиция
        public int Next(int p) => (p >= 1 && p < count) ? p + 1 : End();

        // Предыдущая позиция
        public int Previous(int p) => (p > 1 && p <= count) ? p - 1 : throw new ArgumentException("Invalid position");

        // Очистка
        public int Makenull()
        {
            head = null;
            tail = null;
            count = 0;
            return End();
        }

        // Первая позиция
        public int First() => (count > 0) ? 1 : End();

        // Печать
        public void PrintList()
        {
            DoubleLinkedListNode<T>? current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data);
                current = current.Next;
            }
        }
        
        // Поиск
        private DoubleLinkedListNode<T>? GetNodeAt(int position)
        {
            if (position < 1 || position > count) 
                throw new ArgumentException("Invalid position");

            DoubleLinkedListNode<T>? current = head;
            for (int i = 1; i < position; i++)
                current = current!.Next;

            return current;
        }
        
        private void LinkNodes(DoubleLinkedListNode<T>? left, DoubleLinkedListNode<T>? right)
        {
            if (left != null) left.Next = right;
            if (right != null) right.Prev = left;
        }

        private void ValidatePosition(int position, bool allowEnd = false)
        {
            int max = allowEnd ? count + 1 : count;
            if (position < 1 || position > max)
                throw new ArgumentException("Invalid position");
        }


    }
}

