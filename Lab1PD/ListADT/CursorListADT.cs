// ===============================
// Реализация через курсоры
// ===============================

using Lab1PD.Core;

namespace Lab1PD.ListADT
{
    public class CursorListADT<T> : IListADT<T>
    {
        private CursorNode<T>[] nodes;
        private int head;
        private int tail;
        private int count;
        private int capacity;

        public CursorListADT(int capacity = 100)
        {
            this.capacity = capacity;
            nodes = new CursorNode<T>[capacity];
            head = -1;
            tail = -1;
            count = 0;
        }

        public int End() => count + 1;

        public void Insert(T x, int p)
        {
            if (p < 1 || p > count + 1) return;

            int index = FindFree();
            if (index == -1) throw new Exception("No space");

            nodes[index].Data = x;
            nodes[index].Used = true;

            if (count == 0) // список пуст
            {
                head = tail = index;
                nodes[index].Next = -1;
                nodes[index].Prev = -1;
            }
            else if (p == 1) // вставка в начало
            {
                nodes[index].Next = head;
                nodes[index].Prev = -1;
                nodes[head].Prev = index;
                head = index;
            }
            else if (p == count + 1) // вставка в конец
            {
                nodes[index].Prev = tail;
                nodes[index].Next = -1;
                nodes[tail].Next = index;
                tail = index;
            }
            else // вставка в середину
            {
                int current = GetNodeIndexAt(p);
                int prev = nodes[current].Prev;

                nodes[index].Prev = prev;
                nodes[index].Next = current;
                nodes[prev].Next = index;
                nodes[current].Prev = index;
            }
            count++;
        }

        public int Locate(T x)
        {
            int current = head;
            int pos = 1;
            while (current != -1)
            {
                if (Equals(nodes[current].Data, x)) return pos;
                current = nodes[current].Next;
                pos++;
            }
            return End();
        }

        public T Retrieve(int p)
        {
            if (p < 1 || p > count) throw new ArgumentException("Invalid position");
            int current = GetNodeIndexAt(p);
            return nodes[current].Data;
        }

        public void Delete(int p)
        {
            if (p < 1 || p > count) return;

            int current = GetNodeIndexAt(p);
            int prev = nodes[current].Prev;
            int next = nodes[current].Next;

            if (prev != -1) nodes[prev].Next = next; else head = next;
            if (next != -1) nodes[next].Prev = prev; else tail = prev;

            nodes[current].Used = false;
            count--;
        }

        public int Next(int p) => (p >= 1 && p < count) ? p + 1 : End();

        public int Previous(int p) => (p > 1 && p <= count) ? p - 1 : throw new ArgumentException("Invalid position");

        public int Makenull()
        {
            head = -1;
            tail = -1;
            count = 0;
            nodes = new CursorNode<T>[capacity];
            return End();
        }

        public int First() => (count > 0) ? 1 : End();

        public void PrintList()
        {
            int current = head;
            while (current != -1)
            {
                Console.WriteLine(nodes[current].Data);
                current = nodes[current].Next;
            }
        }

        private int FindFree()
        {
            for (int i = 0; i < capacity; i++)
                if (!nodes[i].Used) return i;
            return -1;
        }
        
        private int GetNodeIndexAt(int p)
        {
            if (p < 1 || p > count) throw new ArgumentException("Invalid position");
            int current = head;
            for (int i = 1; i < p; i++)
                current = nodes[current].Next;
            return current;
        }
    }
}

