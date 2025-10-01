using System;

// ===============================
// Общие типы и интерфейс
// ===============================
namespace Lab1PD.Core
{ 
    /// <summary>
    /// Интерфейс АТД «Список»
    /// </summary>
    public interface IListADT<T>
    {
        // Возвращает позицию после последнего
        int End();

        // Вставляет элемент x в позицию p
        void Insert(T x, int p);

        // Находит позицию элемента x
        int Locate(T x);

        // Возвращает элемент в позиции p
        T Retrieve(int p);

        // Удаляет элемент в позиции p
        void Delete(int p);

        // Возвращает следующую позицию после p
        int Next(int p);

        // Возвращает предыдущую позицию перед p
        int Previous(int p);

        // Делает список пустым
        int Makenull();

        // Возвращает первую позицию
        int First();

        // Печать списка
        void PrintList();
    }
}