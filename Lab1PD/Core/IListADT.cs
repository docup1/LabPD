using System;

namespace Lab1PD.Core
{
    /// <summary>
    /// Интерфейс абстрактного типа данных (АТД) «Список».
    /// 
    /// Определяет базовые операции для последовательного контейнера,
    /// поддерживающего доступ к элементам по позиции, вставку, удаление,
    /// поиск и навигацию по списку.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в списке.</typeparam>
    public interface IListADT<T>
    {
        /// <summary>
        /// Возвращает позицию, следующую за последним элементом списка.
        /// </summary>
        /// <returns>Позиция, соответствующая "концу" списка.</returns>
        int End();

        /// <summary>
        /// Вставляет элемент <paramref name="x"/> в указанную позицию <paramref name="p"/>.
        /// </summary>
        /// <param name="x">Элемент, который необходимо вставить.</param>
        /// <param name="p">Позиция, в которую выполняется вставка.</param>
        /// <remarks>
        /// Элементы, находящиеся на позиции <paramref name="p"/> и далее, сдвигаются вправо.
        /// </remarks>
        void Insert(T x, int p);

        /// <summary>
        /// Находит позицию первого вхождения элемента <paramref name="x"/>.
        /// </summary>
        /// <param name="x">Элемент для поиска.</param>
        /// <returns>Позиция элемента, если найден; иначе — <see cref="End"/>.</returns>
        int Locate(T x);

        /// <summary>
        /// Возвращает элемент, находящийся в позиции <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Позиция требуемого элемента.</param>
        /// <returns>Элемент типа <typeparamref name="T"/> в позиции <paramref name="p"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если позиция <paramref name="p"/> некорректна.</exception>
        T Retrieve(int p);

        /// <summary>
        /// Удаляет элемент, находящийся в позиции <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Позиция удаляемого элемента.</param>
        /// <exception cref="ArgumentOutOfRangeException">Если позиция <paramref name="p"/> некорректна.</exception>
        void Delete(int p);

        /// <summary>
        /// Возвращает позицию следующего элемента после <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Текущая позиция.</param>
        /// <returns>Позиция следующего элемента или <see cref="End"/>, если достигнут конец списка.</returns>
        int Next(int p);

        /// <summary>
        /// Возвращает позицию предыдущего элемента перед <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Текущая позиция.</param>
        /// <returns>Позиция предыдущего элемента или <see cref="First"/>, если достигнуто начало списка.</returns>
        int Previous(int p);

        /// <summary>
        /// Очищает список, делая его пустым.
        /// </summary>
        /// <returns>Позиция конца списка после очистки (обычно 0 или 1).</returns>
        int Makenull();

        /// <summary>
        /// Возвращает позицию первого элемента списка.
        /// </summary>
        /// <returns>Позиция первого элемента или <see cref="End"/>, если список пуст.</returns>
        int First();

        /// <summary>
        /// Выводит содержимое списка на консоль в читаемом виде.
        /// </summary>
        void PrintList();
    }
}
