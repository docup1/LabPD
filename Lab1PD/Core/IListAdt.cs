namespace Lab1PD.Core
{
    /// <summary>
    /// Интерфейс абстрактного типа данных (АТД) «Список».
    /// 
    /// Определяет основные операции для линейной коллекции,
    /// где элементы расположены последовательно и доступны
    /// через абстрактные позиции (<see cref="IPosition"/>).
    /// 
    /// Поддерживаются операции вставки, удаления, поиска,
    /// перемещения по списку и очистки.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в списке.</typeparam>
    public interface IListAdt<T>
    {
        /// <summary>
        /// Возвращает фиктивную позицию, соответствующую «концу» списка.
        /// 
        /// Эта позиция не содержит данных и используется как маркер
        /// конца при итерации по списку.
        /// </summary>
        IPosition End();

        /// <summary>
        /// Вставляет элемент <paramref name="x"/> перед позицией <paramref name="p"/>.
        /// 
        /// Если <paramref name="p"/> указывает на <see cref="End()"/>,
        /// элемент добавляется в конец списка.
        /// </summary>
        /// <param name="x">Элемент, который необходимо вставить.</param>
        /// <param name="p">Позиция, перед которой выполняется вставка.</param>
        /// <exception cref="ArgumentException">Если позиция <paramref name="p"/> недействительна.</exception>
        void Insert(T x, IPosition p);

        /// <summary>
        /// Вставляет элемент <paramref name="p"/> в конец списка./>.
        /// </summary>
        /// <param name="p">Добавляемое значение.</param>
        // void Add(T p);
        
        /// <summary>
        /// Удаляет элемент, находящийся в указанной позиции <paramref name="p"/>.
        /// 
        /// После удаления возвращает позицию следующего элемента,
        /// либо <see cref="End()"/>, если элемент был последним.
        /// </summary>
        /// <param name="p">Позиция удаляемого элемента.</param>
        /// <returns>
        /// Позиция следующего элемента после удалённого или <see cref="End()"/>,
        /// если удалённый элемент был последним.
        /// </returns>
        /// <exception cref="ArgumentException">Если позиция <paramref name="p"/> недействительна.</exception>
        IPosition Delete(IPosition p);

        /// <summary>
        /// Находит позицию первого вхождения элемента <paramref name="x"/>.
        /// </summary>
        /// <param name="x">Элемент для поиска.</param>
        /// <returns>
        /// Позиция первого найденного элемента, если элемент существует;
        /// иначе возвращается <see cref="End()"/>.
        /// </returns>
        IPosition Locate(T x);

        /// <summary>
        /// Возвращает элемент, находящийся в позиции <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Позиция требуемого элемента.</param>
        /// <returns>Элемент типа <typeparamref name="T"/>, расположенный в данной позиции.</returns>
        /// <exception cref="ArgumentException">Если позиция <paramref name="p"/> недействительна.</exception>
        T Retrieve(IPosition p);

        /// <summary>
        /// Возвращает позицию следующего элемента после <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Текущая позиция.</param>
        /// <returns>
        /// Позиция следующего элемента;
        /// если текущий элемент последний — <see cref="End()"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Если позиция <paramref name="p"/> недействительна.</exception>
        IPosition Next(IPosition p);

        /// <summary>
        /// Возвращает позицию предыдущего элемента перед <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Текущая позиция.</param>
        /// <returns>
        /// Позиция предыдущего элемента;
        /// если текущий элемент первый — <see cref="End()"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Если позиция <paramref name="p"/> недействительна.</exception>
        IPosition Previous(IPosition p);

        /// <summary>
        /// Очищает список, удаляя все элементы.
        /// </summary>
        /// <returns>
        /// Позиция конца списка (<see cref="End()"/>), указывающая на пустой список.
        /// </returns>
        IPosition Makenull();

        /// <summary>
        /// Возвращает позицию первого элемента списка.
        /// </summary>
        /// <returns>
        /// Позиция первого элемента или <see cref="End()"/>, если список пуст.
        /// </returns>
        IPosition First();

        /// <summary>
        /// Выводит содержимое списка в консоль.
        /// 
        /// Используется для отладки и наглядного представления структуры данных.
        /// </summary>
        void PrintList();
        
    }
}
