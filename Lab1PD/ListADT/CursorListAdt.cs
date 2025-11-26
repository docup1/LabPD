using Lab1PD.Core;
using Lab1PD.Core.CursorList;

namespace Lab1PD.ListADT
{
    /// <summary>
    /// Реализация абстрактного типа данных (ADT) <b>«Список на курсорах»</b>.
    /// 
    /// В отличие от обычных связных списков, здесь не используются указатели.
    /// Вместо этого список хранится в <see cref="_nodes"/> — статическом массиве узлов, 
    /// а связи реализованы через индексы массива.
    /// 
    /// Принцип:
    /// - Каждый элемент хранит значение <typeparamref name="T"/> и индекс следующего.
    /// - Свободные ячейки образуют список свободных позиций, управляемый через <see cref="_space"/>.
    /// - Список может содержать максимум <see cref="MaxSize"/> элементов.
    /// - Конец списка обозначается индексом <c>-1</c>.
    /// 
    /// Такой подход имитирует указатели и часто применяется в языках без поддержки ссылок или в системах ограниченной памяти.
    /// </summary>
    public class CursorListAdt<T> : IListAdt<T> 
    {
        // --------------------------------------------------------------------
        // Поля
        // --------------------------------------------------------------------

        private const int MaxSize = 100;                   // Максимальное количество узлов в списке
        private Node<T>[] _nodes;                   // Основной массив узлов
        private int _space;                         // Индекс первой свободной ячейки ("список свободных")
        private int _head = -1;                            // Индекс первого элемента (-1 = пусто)
        private static readonly Position _End = new Position(-1); // Фиктивная позиция конца списка (используется как маркер)

        // --------------------------------------------------------------------
        // Статический конструктор
        // --------------------------------------------------------------------

        /// <summary>
        /// Инициализация памяти под список (пул узлов).
        /// 
        /// При запуске приложения формируется "цепочка" свободных узлов:
        /// каждый узел ссылается на следующий по индексу, а последний — на <c>-1</c>.
        /// </summary>

        public CursorListAdt()
        {
            _nodes = new Node<T>[MaxSize];

            // Инициализация цепочки свободных узлов
            for (int i = 0; i < MaxSize - 1; i++)
                _nodes[i] = new Node<T> { Next = i + 1 };

            _nodes[MaxSize - 1] = new Node<T> { Next = -1 };
            _space = 0;
        }


        // --------------------------------------------------------------------
        // Вспомогательные методы
        // --------------------------------------------------------------------

        /// <summary>
        /// Возвращает индекс последнего элемента в списке.
        /// Если список пуст — возвращает -1.
        /// </summary>
        private int Last()
        {
            int current = _head;
            int previous = -1;
            while (current != -1)
            {
                previous = current;
                current = _nodes[current].Next;
            }
            return previous;
        }

        /// <summary>
        /// Возвращает индекс предыдущего узла для элемента с указанным индексом.
        /// 
        /// <list type="bullet">
        /// <item><description>Если элемент первый → возвращает -1.</description></item>
        /// <item><description>Если элемент не найден → возвращает -2.</description></item>
        /// </list>
        /// </summary>
        /// <summary>
        /// Возвращает индекс предыдущего узла для элемента с указанным индексом.
        /// </summary>
        /// <param name="index">Индекс элемента в массиве узлов.</param>
        /// <returns>
        /// - индекс предыдущего узла, если элемент найден и не является головой;
        /// - -1, если элемент является первым (головой);
        /// - -2, если элемент не найден в списке.
        /// </returns>
        private int GetPrev(int index)
        {
            if (_head == -1) // список пуст
                return -2;

            if (index < 0 || index >= MaxSize) // недопустимый индекс
                return -2;

            int current = _head;
            int previous = -1;

            while (current != -1)
            {
                if (current == index)
                    return previous; // нашли элемент, возвращаем предыдущий

                previous = current;
                current = _nodes[current].Next;
            }

            // если дошли до конца списка и не нашли индекс
            return -2;
        }


        // --------------------------------------------------------------------
        // Методы интерфейса IListAdt<T>
        // --------------------------------------------------------------------

        /// <summary> Возвращает фиктивную позицию конца списка. </summary>
        public IPosition End() => _End;

        /// <summary> Возвращает позицию первого элемента списка. Если список пуст, возвращает <see cref="End()"/>. </summary>
        public IPosition First() => new Position(_head);

        /// <summary>
        /// Вставляет элемент <paramref name="x"/> перед позицией <paramref name="p"/>.
        /// Если позиция равна <see cref="End()"/>, элемент вставляется в конец списка.
        /// </summary>
        /// <param name="x">Элемент для вставки.</param>
        /// <param name="p">Позиция, перед которой нужно вставить.</param>
        /// <exception cref="Exception">Если память пула исчерпана.</exception>
        public void Insert(T x, IPosition p)
        {
            if (_space == -1)
                throw new Exception("Память пула исчерпана");

            Position pos = (Position)p;
            int newIndex = _space;
            _space = _nodes[_space].Next;

            _nodes[newIndex].Data = x;
            _nodes[newIndex].Next = -1;

            // Вставка в пустой список
            if (_head == -1)
            {
                _head = newIndex;
                return;
            }

            // Вставка перед End() -> в конец 
            if (pos.N == -1)
            {
                int last = Last();
                _nodes[last].Next = newIndex;
                return;
            }

            // Вставка перед головой
            if (pos.N == _head)
            {
                _nodes[newIndex].Next = _head;
                _head = newIndex;
                return;
            }

            // Вставка перед произвольным элементом
            int prev = GetPrev(pos.N);
            if (prev < 0)
                throw new ArgumentException("Недопустимая позиция для вставки");

            _nodes[newIndex].Next = pos.N;
            _nodes[prev].Next = newIndex;
        }


        /// <summary>
        /// Вставляет элемент <paramref name="p"/> в конец списка./>.
        /// </summary>
        /// <param name="p">Добавляемое значение.</param>
        public void Add(T p) => Insert(p, _End);
        
        
        /// <summary>
        /// Удаляет элемент, расположенный в позиции <paramref name="p"/>.
        /// Возвращает позицию следующего элемента.
        /// </summary>
        public IPosition Delete(IPosition p)
        {
            Position pos = (Position)p;

            if (_head == -1)
                throw new ArgumentException("Список пуст");
            if (pos.N == -1)
                throw new ArgumentException("Позиция указывает на конец списка");

            int current = pos.N;
            IPosition nextPos = _nodes[current].Next != -1 ? new Position(_nodes[current].Next) : _End; //

            // Удаление головы
            if (current == _head)
            {
                _head = _nodes[_head].Next;
            }
            else
            {
                int prev = GetPrev(current);
                if (prev < 0)
                    throw new ArgumentException("Элемент не найден");
                _nodes[prev].Next = _nodes[current].Next;
            }

            // Возврат узла в пул
            _nodes[current].Next = _space;
            _space = current;

            return nextPos;
        }

        /// <summary>
        /// Находит первую позицию, где хранится элемент <paramref name="x"/>.
        /// Если не найден — возвращает <see cref="End()"/>.
        /// </summary>
        public IPosition Locate(T x)
        {
            int current = _head;
            while (current != -1)
            {
                if (_nodes[current].Data!.Equals(x))
                    return new Position(current);

                current = _nodes[current].Next;
            }
            return _End;
        }

        /// <summary>
        /// Возвращает элемент, расположенный в позиции <paramref name="p"/>.
        /// </summary>
        public T Retrieve(IPosition p)
        {
            Position pos = (Position)p;

            if (_head == -1)
                throw new ArgumentException("Список пуст");

            if (pos.N == -1)
                throw new ArgumentException("Позиция указывает на конец списка");

            if (pos.N != _head && GetPrev(pos.N) < 0)
                throw new ArgumentException("Недопустимая позиция");

            return _nodes[pos.N].Data;
        }

        public IPosition Next(IPosition p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            Position pos = (Position)p;

            // Если позиция указывает на конец списка
            if (pos.N == -1)
                return _End;

            // Берём узел по позиции
            Node<T> currentNode = _nodes[pos.N]; // если массив или список узлов
            if (currentNode == null)
                throw new ArgumentException("Элемент не найден");

            // Если следующего узла нет, возвращаем End()
            return currentNode.Next == -1 ? _End : new Position(currentNode.Next);
        }




        /// <summary>
        /// Возвращает позицию предыдущего элемента относительно позиции <paramref name="p"/>.
        /// </summary>
        public IPosition Previous(IPosition p)
        {
            Position pos = (Position)p;

            if (_head == -1)
                throw new ArgumentException("Список пуст");

            if (pos.N == -1)
                throw new ArgumentException("Позиция указывает на конец списка");

            if (pos.N == _head)
                throw new ArgumentException("Первый элемент не имеет предыдущего");

            int prev = GetPrev(pos.N);
            if (prev < 0)
                throw new ArgumentException("Позиция не найдена");

            return new Position(prev);
        }

        /// <summary>
        /// Полностью очищает список и возвращает все узлы обратно в пул свободных.
        /// </summary>
        public IPosition Makenull()
        {
            if (_head == -1)
                return _End;

            // Найти последний элемент
            int last = Last();

            // Соединить конец списка с текущим пулом
            _nodes[last].Next = _space;

            // Голова возвращается в пул
            _space = _head;
            _head = -1;

            return _End;
        }


        /// <summary>
        /// Выводит содержимое списка в консоль с порядковыми номерами элементов.
        /// </summary>
        public void PrintList()
        {
            if (_head == -1)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            int current = _head;
            int index = 1;
            while (current != -1)
            {
                Console.WriteLine($"{index++}. {_nodes[current].Data}");
                current = _nodes[current].Next;
            }
        }

    }
}
