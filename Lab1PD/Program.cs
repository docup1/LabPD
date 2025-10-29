// ===============================
// Главный класс
// ===============================

using Lab1PD.Core;
using Lab1PD.ListADT;
using System;

namespace LabMain
{
    // Для выбора реализации:
    // using ListImpl = CursorListAdt<string>;

    using ListImpl = DoubleLinkedListAdt<string>;

    internal class Program
    {
        private static void RemoveDuplicates<T>(IListADT<T> list)
        {
            int p = list.First();
            while (p != list.End())
            {
                T value = list.Retrieve(p);
                int q = list.Next(p);
                while (q != list.End())
                {
                    if (Equals(list.Retrieve(q), value))
                    {
                        list.Delete(q);
                        q = list.Next(q - 1);
                    }
                    else
                    {
                        q = list.Next(q);
                    }
                }

                p = list.Next(p);
            }
        }

        public static void Main()
        {
            IListADT<string> list = new ListImpl();
            list.Insert("Alice", list.End());
            list.Insert("Bob", list.End());
            list.Insert("Alice", list.End());
            list.Insert("Charlie", list.End());
            list.Insert("Bob", list.End());

            Console.WriteLine("Before:");
            list.PrintList();

            RemoveDuplicates(list);

            Console.WriteLine("After:");
            list.PrintList();
        }
    }
}