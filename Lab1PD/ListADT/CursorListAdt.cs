using System;
using Lab1PD.Core;
using Lab1PD.Core.CursorList;

namespace Lab1PD.ListADT
{
    /// <summary>
    /// Реализация абстрактного типа данных (ADT) <b>«Список на курсорах»</b>.
    /// </summary>
    /// <remarks>
    /// Данная структура имитирует динамическую память, используя статический массив узлов.
    /// Связи между элементами осуществляются через индексы массива вместо указателей.
    /// 
    /// <b>Особенности реализации:</b>
    /// <list type="bullet">
    /// <item>Используется общий для всех экземпляров пул свободных узлов (<c>_space</c>).</item>
    /// <item>При вставке в существующую позицию применяется алгоритм перемещения данных для сохранения стабильности индексов.</item>
    /// <item>Максимальная емкость ограничена <see cref="MaxSize"/>.</item>
    /// </list>
    /// </remarks>
    /// <typeparam name="T">Тип хранимых данных.</typeparam>
    public class CursorListAdt<T> : IListAdt<T>
    {
        // --------------------------------------------------------------------
        // Поля
        // --------------------------------------------------------------------

        private const int MaxSize = 100;
        private static Node<T>[] _nodes; 
        private static int _space = 0;   // Индекс начала списка свободных ячеек
        private int _head = -1;          // Индекс начала текущего списка (start)
        private static readonly Position _end = new Position(-1);

        // --------------------------------------------------------------------
        // Инициализация
        // --------------------------------------------------------------------

        /// <summary>
        /// Статическая инициализация пула узлов.
        /// Формирует начальную цепочку свободных ячеек, где каждая ссылается на следующую.
        /// </summary>
        static CursorListAdt()
        {
            _nodes = new Node<T>[MaxSize];
            for (int i = 0; i < MaxSize - 1; i++)
            {
                _nodes[i] = new Node<T> { Next = i + 1 };
            }
            _nodes[MaxSize - 1] = new Node<T> { Next = -1 };
            _space = 0;
        }

        // --------------------------------------------------------------------
        // Вспомогательные методы
        // --------------------------------------------------------------------

        /// <summary> Возвращает фиктивную позицию, обозначающую конец списка. </summary>
        public IPosition End() => _end;

        /// <summary> Возвращает индекс последнего значимого узла в текущем списке. </summary>
        /// <returns>Индекс последнего узла или -1, если список пуст.</returns>
        private int Last()
        {
            if (_head == -1) return -1;
            int current = _head;
            while (_nodes[current].Next != -1)
            {
                current = _nodes[current].Next;
            }
            return current;
        }

        /// <summary> Находит индекс узла, который ссылается на узел с индексом <paramref name="index"/>. </summary>
        /// <param name="index">Индекс целевого узла.</param>
        /// <returns>Индекс предыдущего узла или -2, если узел не найден.</returns>
        private int GetPrevious(int index)
        {
            int current = _head;
            int prev = -1;
            while (current != -1)
            {
                if (current == index) return prev;
                prev = current;
                current = _nodes[current].Next;
            }
            return -2;
        }

        /// <summary> Проверяет валидность позиции в контексте текущего списка. </summary>
        /// <param name="index">Индекс для проверки.</param>
        private bool ValidatePosition(int index)
        {
            if (index < -1 || index >= MaxSize) return false;
            if (index == -1) return true; // Позиция End() валидна
            return (index == _head) || (GetPrevious(index) != -2);
        }

        /// <summary> Проверяет, не содержит ли список ни одного элемента. </summary>
        public bool IsEmpty() => _head == -1;

        /// <summary> Возвращает позицию первого элемента списка. Если список пуст, возвращает <see cref="End"/>. </summary>
        public IPosition First() => IsEmpty() ? _end : new Position(_head);

        // --------------------------------------------------------------------
        // Основные операции ADT
        // --------------------------------------------------------------------

        /// <summary>
        /// Вставляет элемент в список по указанной позиции.
        /// </summary>
        /// <param name="obj">Значение для вставки.</param>
        /// <param name="p">Позиция, перед которой произойдет вставка.</param>
        /// <exception cref="Exception">Генерируется при нехватке памяти в пуле.</exception>
        /// <exception cref="ArgumentException">Генерируется при невалидной позиции.</exception>
        public void Insert(T obj, IPosition p)
        {
            if (_space == -1) throw new Exception("Память пула исчерпана");
            Position pos = (Position)p;

            // Сценарий A: Вставка в конец (p == _end)
            if (pos.N == -1)
            {
                int newIdx = _space;
                _space = _nodes[_space].Next;
                
                _nodes[newIdx].Data = obj;
                _nodes[newIdx].Next = -1;

                if (_head == -1) _head = newIdx;
                else _nodes[Last()].Next = newIdx;
            }
            // Сценарий B: Вставка в существующую позицию (сдвиг данных)
            else
            {
                if (!ValidatePosition(pos.N)) throw new ArgumentException("Невалидная позиция");

                int tmp = _space;
                _space = _nodes[_space].Next;

                // Перенос текущих данных из p в новую ячейку
                _nodes[tmp].Data = _nodes[pos.N].Data;
                _nodes[tmp].Next = _nodes[pos.N].Next;

                // Запись новых данных в ячейку p
                _nodes[pos.N].Data = obj;
                _nodes[pos.N].Next = tmp;
            }
        }

        /// <summary> Добавляет элемент в самый конец списка. </summary>
        public void Add(T obj) => Insert(obj, _end);

        /// <summary> Ищет первую позицию элемента с заданным значением. </summary>
        /// <returns>Позиция элемента или <see cref="End"/>, если не найден.</returns>
        public IPosition Locate(T obj)
        {
            int cur = _head;
            while (cur != -1)
            {
                if (Equals(_nodes[cur].Data, obj)) return new Position(cur);
                cur = _nodes[cur].Next;
            }
            return _end;
        }

        /// <summary> Извлекает значение из указанной позиции. </summary>
        public T Retrieve(IPosition p)
        {
            Position pos = (Position)p;
            if (!ValidatePosition(pos.N) || pos.N == -1) throw new ArgumentException("Невалидная позиция");
            return _nodes[pos.N].Data;
        }

        /// <summary>
        /// Удаляет элемент в указанной позиции и возвращает позицию следующего за ним элемента.
        /// </summary>
        public IPosition Delete(IPosition p)
        {
            Position pos = (Position)p;
            if (!ValidatePosition(pos.N) || pos.N == -1) throw new ArgumentException("Невалидная позиция");

            int nextIdx;
            if (pos.N == _head)
            {
                nextIdx = _nodes[_head].Next;
                int tmp = _space;
                _space = _head;
                _head = nextIdx;
                _nodes[_space].Next = tmp;
            }
            else
            {
                int prev = GetPrevious(pos.N);
                int cur = _nodes[prev].Next;
                nextIdx = _nodes[cur].Next;
                
                _nodes[prev].Next = nextIdx;

                int tmp = _space;
                _space = cur;
                _nodes[_space].Next = tmp;
            }
            return nextIdx == -1 ? _end : new Position(nextIdx);
        }

        /// <summary> Возвращает позицию следующего элемента. </summary>
        public IPosition Next(IPosition p)
        {
            Position pos = (Position)p;
            if (!ValidatePosition(pos.N)) throw new ArgumentException("Невалидная позиция");
            if (pos.N == -1 || _nodes[pos.N].Next == -1) return _end;
            return new Position(_nodes[pos.N].Next);
        }

        /// <summary> Возвращает позицию предыдущего элемента. </summary>
        public IPosition Previous(IPosition p)
        {
            Position pos = (Position)p;
            int prev = GetPrevious(pos.N);
            if (pos.N < 0 || prev < 0) throw new ArgumentException("Предыдущего элемента не существует");
            return new Position(prev);
        }

        /// <summary>
        /// Очищает список, возвращая все занятые узлы в пул свободной памяти.
        /// </summary>
        public IPosition Makenull()
        {
            if (IsEmpty()) return _end;
            int last = Last();
            _nodes[last].Next = _space;
            _space = _head;
            _head = -1;
            return _end;
        }

        /// <summary> Выводит содержимое списка в консоль в формате эл1,\nэл2. </summary>
        public void PrintList()
        {
            int cur = _head;
            while (cur != -1)
            {
                Console.Write(_nodes[cur].Data);
                cur = _nodes[cur].Next;
                if (cur != -1) Console.WriteLine(", ");
            }
            Console.WriteLine("");
        }
    }
}