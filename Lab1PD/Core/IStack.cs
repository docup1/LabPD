namespace Lab1PD.Core
{
    /// <summary>
    /// Интерфейс абстрактного типа данных "Стек" (Stack ADT).
    /// 
    /// Стек работает по принципу LIFO (Last In – First Out):
    /// элемент, добавленный последним, извлекается первым.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранящихся в стеке.</typeparam>
    public interface IStack<T>
    {
        /// <summary>
        /// Делает стек пустым, удаляя все элементы.
        /// </summary>
        void MakeNull();

        /// <summary>
        /// Возвращает элемент, находящийся на вершине стека,
        /// без его удаления.
        /// </summary>
        /// <returns>Копия верхнего элемента стека.</returns>
        /// <exception cref="InvalidOperationException">
        /// Если стек пуст.
        /// </exception>
        T Top();

        /// <summary>
        /// Удаляет элемент с вершины стека и возвращает его.
        /// </summary>
        /// <returns>Удалённый элемент.</returns>
        /// <exception cref="InvalidOperationException">
        /// Если стек пуст.
        /// </exception>
        T Pop();

        /// <summary>
        /// Добавляет элемент <paramref name="x"/> в вершину стека.
        /// </summary>
        /// <param name="x">Элемент для добавления.</param>
        void Push(T x);

        /// <summary>
        /// Проверяет, пуст ли стек.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если стек пустой;
        /// иначе <c>false</c>.
        /// </returns>
        bool Empty();

        /// <summary>
        /// Проверяет, полон ли стек.
        /// 
        /// Для реализаций на списках обычно всегда возвращает <c>false</c>.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если стек полон;
        /// иначе <c>false</c>.
        /// </returns>
        bool Full();
    }
}