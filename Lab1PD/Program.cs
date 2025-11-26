using System;
using Lab1PD.Core;
using Lab1PD.ListADT;

namespace Lab1PD
{
    internal class Program
    {
        // Метод удаления дубликатов
        private static void RemoveDuplicates(IListAdt<Person> list)
        {
            IPosition p = list.First();

            while (p != list.End())
            {
                Person currentValue = list.Retrieve(p);
                IPosition q = list.Next(p);

                while (q != list.End())
                {
                    // Сохраняем следующую позицию заранее
                    IPosition nextQ = list.Next(q);

                    if (currentValue.Equals(list.Retrieve(q)))
                    {
                        list.Delete(q); // удаляем дубликат
                        // не делаем q = nextQ здесь
                    }

                    q = nextQ; // переходим к следующей позиции
                }

                p = list.Next(p);
            }
        }
        
        public static void Main()
        {
            IListAdt<Person> list = new DoubleLinkedListAdt<Person>();
            // IListAdt<Person> list = new CursorListAdt<Person>();

            // Создаём объекты Person (имя до 20 символов, адрес до 50)
            char[] name1 = "Alice".ToCharArray();
            char[] adr1 = "123 Main St".ToCharArray();
            Person person1 = new Person(name1, adr1);

            char[] name2 = "Bob".ToCharArray();
            char[] adr2 = "456 Elm St".ToCharArray();
            Person person2 = new Person(name2, adr2);

            char[] name3 = "Charlie".ToCharArray();
            char[] adr3 = "789 Oak St".ToCharArray();
            Person person3 = new Person(name3, adr3);

            // Добавляем в список (дубликаты тоже)
            list.Add(person1);
            list.Add(person2);
            list.Add(person1); // дубликат
            list.Add(person3);
            list.Add(person2); // дубликат

            Console.WriteLine("Before:");
            list.PrintList();

            RemoveDuplicates(list);

            Console.WriteLine("After:");
            list.PrintList();
        }
    }
}