using Lab1PD.Core;
using Lab1PD.Core.CursorList;

namespace Lab1PD.ListADT
{
    /// <summary>
    /// Реализация абстрактного типа данных «Список на курсорах».
    /// Список работает в статическом массиве узлов, связи — через индексы.
    /// </summary>
    public class CursorListAdt<T> : IListAdt<T>
    {
        private const int MaxSize = 100;

        private Node<T>[] _nodes;  // Пул узлов
        private int _space;        // Текущая свободная ячейка
        private int _head = -1;    // Первый элемент списка
        private readonly Position _End = new Position(-1);

        public CursorListAdt()
        {
            _nodes = new Node<T>[MaxSize];

            // создаём цепочку свободных узлов: 0 → 1 → 2 → ... → 99 → -1
            for (int i = 0; i < MaxSize - 1; i++)
                _nodes[i] = new Node<T> { Next = i + 1 };

            _nodes[MaxSize - 1] = new Node<T> { Next = -1 };
            _space = 0;
        }

        // ============================================================
        // Вспомогательные методы
        // ============================================================

        /// <summary>Возвращает индекс последнего элемента.</summary>
        private int Last()
        {
            int current = _head;
            int prev = -1;

            while (current != -1)
            {
                prev = current;
                current = _nodes[current].Next;
            }
            return prev;
        }

        /// <summary>
        /// Находит предыдущий узел.
        /// </summary>
        private int GetPrev(int index)
        {
            if (_head == -1) return -2;

            int current = _head;
            int prev = -1;

            while (current != -1)
            {
                if (current == index)
                    return prev;

                prev = current;
                current = _nodes[current].Next;
            }

            return -2;
        }

        // ============================================================
        // Операции ADT
        // ============================================================

        public IPosition End() => _End;
        public IPosition First() => _head == -1 ? _End : new Position(_head);

        /// <summary>
        /// Вставка элемента перед позицией p.
        /// </summary>
        public void Insert(T x, IPosition p)
        {
            if (_space == -1)
                throw new Exception("Память пула исчерпана");

            int newIndex = _space;            // берём свободный узел
            _space = _nodes[_space].Next;     // обновляем голову списка свободных
            _nodes[newIndex].Data = x;

            Position pos = (Position)p;

            // -----------------------------
            // Вставка в конец
            // -----------------------------

            if (pos.N == -1)
            {
                if (_head == -1)
                {
                    // список пуст → новый элемент становится первым
                    _nodes[newIndex].Next = -1;
                    _head = newIndex;
                }
                else
                {
                    // найти последний элемент
                    int last = Last();
                    _nodes[newIndex].Next = -1;
                    _nodes[last].Next = newIndex;
                }
                return;
            }

            // -----------------------------
            // Вставка перед первым элементом
            // -----------------------------
            if (pos.N == _head)
            {
                _nodes[newIndex].Next = _head;
                _head = newIndex;
                return;
            }

            // -----------------------------
            // Вставка в середину
            // -----------------------------

            int prev = GetPrev(pos.N);
            if (prev < -1)
                throw new ArgumentException("Недопустимая позиция");

            _nodes[newIndex].Next = pos.N;
            _nodes[prev].Next = newIndex;
        }

        public void Add(T p) => Insert(p, _End);

        /// <summary>Удаляет элемент в позиции p.</summary>
        public IPosition Delete(IPosition p)
        {
            if (_head == -1)
                throw new ArgumentException("Список пуст");

            Position pos = (Position)p;
            if (pos.N == -1)
                throw new ArgumentException("Удаление по End() невозможно");

            int next = _nodes[pos.N].Next;
            int freed = pos.N;

            // Удаление головы
            if (pos.N == _head)
            {
                _head = next;
            }
            else
            {
                int prev = GetPrev(pos.N);
                if (prev < 0)
                    throw new ArgumentException("Элемент не найден");

                _nodes[prev].Next = next;
            }

            // Возвращаем узел в пул свободных
            _nodes[freed].Next = _space;
            _space = freed;

            return next == -1 ? _End : new Position(next);
        }

        /// <summary>Ищет элемент x.</summary>
        public IPosition Locate(T x)
        {
            int cur = _head;
            while (cur != -1)
            {
                if (_nodes[cur].Data != null &&
                    _nodes[cur].Data.Equals(x))
                    return new Position(cur);

                cur = _nodes[cur].Next;
            }
            return _End;
        }

        /// <summary>Возвращает данные по позиции.</summary>
        public T Retrieve(IPosition p)
        {
            Position pos = (Position)p;

            if (pos.N == -1)
                throw new ArgumentException("End() не содержит данных");

            return _nodes[pos.N].Data;
        }

        public IPosition Next(IPosition p)
        {
            Position pos = (Position)p;
            if (pos.N == -1) return _End;

            int next = _nodes[pos.N].Next;
            return next == -1 ? _End : new Position(next);
        }

        public IPosition Previous(IPosition p)
        {
            Position pos = (Position)p;

            if (pos.N == _head)
                throw new ArgumentException("У первого элемента нет предыдущего");

            int prev = GetPrev(pos.N);
            if (prev < 0)
                throw new ArgumentException("Позиция недействительна");

            return new Position(prev);
        }

        /// <summary>Очищает весь список.</summary>
        public IPosition Makenull()
        {
            if (_head == -1) return _End;

            int last = Last();
            _nodes[last].Next = _space;
            _space = _head;
            _head = -1;

            return _End;
        }

        /// <summary>Печатает список.</summary>
        public void PrintList()
        {
            if (_head == -1)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            int cur = _head;
            int i = 1;

            while (cur != -1)
            {
                Console.WriteLine($"{i++}. {_nodes[cur].Data}");
                cur = _nodes[cur].Next;
            }
        }
    }
}
