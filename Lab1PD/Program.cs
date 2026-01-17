using Lab1PD.Core;
using Lab1PD.ListADT;
using Lab1PD.Map;
using Lab1PD.DataBasePart;
using Lab1PD.Hashing;

namespace Lab1PD
{
    internal class Program
    {
        // Метод удаления дубликатов для задания 1
        private static void RemoveDuplicates(IListAdt<Person> list)
        {
            IPosition p = list.First();

            while (p != list.End())
            {
                Person currentValue = list.Retrieve(p);
                IPosition q = list.Next(p);

                while (q != list.End())
                {
                    IPosition nextQ = list.Next(q);

                    if (currentValue.Equals(list.Retrieve(q)))
                    {
                        list.Delete(q);
                    }

                    q = nextQ;
                }

                p = list.Next(p);
            }
        }

        // Метод конвертации строки в char[]
        static char[] ToCharArray(string str)
        {
            char[] result = new char[Math.Min(str.Length + 1, 50)]; // Ограничение длины
            int copyLength = Math.Min(str.Length, result.Length - 1);
            str.CopyTo(0, result, 0, copyLength);
            if (copyLength < result.Length)
                result[copyLength] = '\0';
            return result;
        }

        public static void Main()
        {
            Console.WriteLine("Лабораторная работа 1 - Объединение всех заданий");
            Console.WriteLine("================================================");

            while (true)
            {
                Console.WriteLine("\nВыберите задание для демонстрации:");
                Console.WriteLine("1 - ADT List (удаление дубликатов)");
                Console.WriteLine("2 - Stack, Queue, Map (коллекции)");
                Console.WriteLine("3 - Hashing (открытое/закрытое хеширование)");
                Console.WriteLine("4 - M2M (многокурсовая система)");

                Console.WriteLine("5 - Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DemoListADT();
                        break;

                    case "2":
                        DemoCollections();
                        break;

                    case "3":
                        DemoHashing();
                        break;

                    case "4":
                        DemoMultiList();
                        break;

                    case "5":
                        Console.WriteLine("Выход из программы...");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        // Демонстрация задания 1 - ADT List
        private static void DemoListADT()
        {
            Console.WriteLine("\n=== Задание 1: ADT List - Удаление дубликатов ===");

            // Можно менять тип списка для тестирования
            IListAdt<Person> dlList = new DoubleLinkedListAdt<Person>();
            IListAdt<Person> curList = new CursorListAdt<Person>();

            // Создаём объекты Person
            Person person1 = new Person(ToCharArray("Alice"), ToCharArray("123 Main St"));
            Person person2 = new Person(ToCharArray("Bob"), ToCharArray("456 Elm St"));
            Person person3 = new Person(ToCharArray("Charlie"), ToCharArray("789 Oak St"));

            // Добавляем в список (с дубликатами)
            dlList.Insert(person1, dlList.End());
            dlList.Insert(person2, dlList.End());
            dlList.Insert(person1, dlList.End()); // дубликат
            dlList.Insert(person3, dlList.End());
            dlList.Insert(person2, dlList.End()); // дубликат

            curList.Insert(person1, curList.End());
            curList.Insert(person2, curList.End());
            curList.Insert(person1, curList.End()); // дубликат
            curList.Insert(person3, curList.End());
            curList.Insert(person2, curList.End());

            Console.WriteLine("\nДо удаления дубликатов:");
            Console.WriteLine("DoublyLinkedList");
            dlList.PrintList();
            Console.WriteLine("CursorList");
            curList.PrintList();

            RemoveDuplicates(dlList);
            RemoveDuplicates(curList);

            Console.WriteLine("\nПосле удаления дубликатов:");
            Console.WriteLine("DoublyLinkedList");
            dlList.PrintList();
            Console.WriteLine("CursorList");
            curList.PrintList();
        }

        // Демонстрация задания 2 - Stack, Queue, Map
        private static void DemoCollections()
        {
            Console.WriteLine("\n=== Задание 2: Stack, Queue, Map ===");

            MapADT<char, int> map = new MapADT<char, int>();
            Lab1PD.Queue.Array.Queue<char> arrQueue = new Lab1PD.Queue.Array.Queue<char>();
            Lab1PD.Stack.Array.Stack<char> arrStack = new Lab1PD.Stack.Array.Stack<char>();

            Lab1PD.Queue.Linked.Queue<char> linkQueue = new Lab1PD.Queue.Linked.Queue<char>();
            Lab1PD.Stack.Linked.Stack<char> linkStack = new Lab1PD.Stack.Linked.Stack<char>();

            Lab1PD.Queue.List.Queue<char> listQueue = new Lab1PD.Queue.List.Queue<char>();
            Lab1PD.Stack.ADT.Stack<char> adtStack = new Lab1PD.Stack.ADT.Stack<char>();

            string testString = "Hello World";
            char[] arr = testString.ToCharArray();

            Console.WriteLine($"Исходная строка: {testString}");
            Console.WriteLine($"Длина строки: {testString.Length} символов");

            int cur = 0;

            // Заносим все символы в коллекции
            foreach (var ch in arr)
            {
                map.Assign(ch, cur);
                arrQueue.Enqueue(ch);
                arrStack.Push(ch);
                linkQueue.Enqueue(ch);
                linkStack.Push(ch);
                listQueue.Enqueue(ch);
                adtStack.Push(ch);
                cur++;
            }

            Console.WriteLine("\n1. Отображение (частотный словарь символов):");
            map.PrintList();

            Console.WriteLine("\n2.1. Очередь (На массиве):");
            Console.Write("   ");
            while (!arrQueue.Empty())
            {
                Console.Write(arrQueue.Dequeue());
            }

            Console.WriteLine("\n2.2. Очередь (На односвязном списке):");
            Console.Write("   ");
            while (!linkQueue.Empty())
            {
                Console.Write(linkQueue.Dequeue());
            }
            Console.WriteLine("\n2.3. Очередь (На ATD списке):");
            Console.Write("   ");
            while (!listQueue.Empty())
            {
                Console.Write(listQueue.Dequeue());
            }

            Console.WriteLine("\n\n3.1. Стек (На массиве):");
            Console.Write("   ");
            while (!arrStack.Empty())
            {
                Console.Write(arrStack.Pop());
            }
            Console.WriteLine("\n3.2. Стек (На односвязном списке):");
            Console.Write("   ");
            while (!linkStack.Empty())
            {
                Console.Write(linkStack.Pop());
            }

            Console.WriteLine("\n3.3. Стек (На ATD списке):");
            Console.Write("   ");
            while (!adtStack.Empty())
            {
                Console.Write(adtStack.Pop());
            }
            Console.WriteLine();
        }

        // Демонстрация задания 3 - M2M (MultiList)
        private static void DemoMultiList()
        {
            Console.WriteLine("\n=== Задание 3: M2M - Многокурсовая система ===");

            var db = new MultiList();

            Console.WriteLine("\n1. Добавление студентов и курсов");
            db.AddNewStudent("Vlad");
            db.AddNewStudent("Alina");
            db.AddNewStudent("John");
            db.AddNewCourse("Math");
            db.AddNewCourse("Programming");
            db.AddNewCourse("Physics");

            Console.WriteLine("\n2. Запись студентов на курсы");
            db.AddStudentToCourse("Vlad", "Math");
            db.AddStudentToCourse("Vlad", "Programming");
            db.AddStudentToCourse("Alina", "Math");
            db.AddStudentToCourse("John", "Physics");
            db.AddStudentToCourse("John", "Programming");

            Console.WriteLine("\n3. Курсы студента Vlad:");
            db.PrintCoursesOfStudent("Vlad");

            Console.WriteLine("\n4. Курсы студента Alina:");
            db.PrintCoursesOfStudent("Alina");

            Console.WriteLine("\n5. Курсы студента John:");
            db.PrintCoursesOfStudent("John");

            Console.WriteLine("\n6. Студенты на курсе Math:");
            db.PrintStudentsOfCourse("Math");

            Console.WriteLine("\n7. Студенты на курсе Programming:");
            db.PrintStudentsOfCourse("Programming");

            Console.WriteLine("\n8. Удаление студента Vlad из курса Programming:");
            db.RemoveStudentFromCourse("Vlad", "Programming");
            db.PrintCoursesOfStudent("Vlad");

            Console.WriteLine("\n9. Студенты на курсе Programming после удаления:");
            db.PrintStudentsOfCourse("Programming");
        }

        // Демонстрация задания 4 - Hashing
        private static void DemoHashing()
        {
            Console.WriteLine("\n=== Задание 4: Hashing - Система классификации ===");

            OpenHashedDictionary goodGuys = new OpenHashedDictionary();
            CloseHashedDictionary badGuys = new CloseHashedDictionary();

            Console.WriteLine("Команды имитируются автоматически...\n");

            // Тестовые команды
            string[] commands =
            {
                "F Batman",
                "F Superman",
                "U Joker",
                "U LexLuthor",
                "? Batman",
                "? Joker",
                "F Joker",
                "P",
                "? Penguin",
                "U Batman",
                "P",
                "E"
            };

            foreach (string input in commands)
            {
                if (string.IsNullOrEmpty(input)) continue;

                char cmd = input[0];

                if (cmd == 'E') break;

                if (cmd == 'P')
                {
                    Console.WriteLine("\n--- Текущее состояние словарей ---");
                    Console.Write("Хорошие парни: ");
                    goodGuys.Print();
                    Console.WriteLine();
                    Console.Write("Плохие парни: ");
                    badGuys.Print();
                    Console.WriteLine("\n" + new string('-', 40));
                    continue;
                }

                // Парсинг имени
                string nameStr = input.Length > 2 ? input.Substring(2) : "";
                char[] nameArr = new char[10];
                for (int i = 0; i < nameStr.Length && i < 10; i++)
                {
                    nameArr[i] = nameStr[i];
                }

                switch (cmd)
                {
                    case 'F':
                        Console.WriteLine($"Добавляем {nameStr} к хорошим парням...");
                        goodGuys.Insert(nameArr);
                        badGuys.Remove(nameArr);
                        break;
                    case 'U':
                        Console.WriteLine($"Добавляем {nameStr} к плохим парням...");
                        badGuys.Insert(nameArr);
                        goodGuys.Remove(nameArr);
                        break;
                    case '?':
                        if (goodGuys.Contains(nameArr))
                            Console.WriteLine($"Результат: {nameStr} - хороший парень");
                        else if (badGuys.Contains(nameArr))
                            Console.WriteLine($"Результат: {nameStr} - плохой парень");
                        else
                            Console.WriteLine($"Результат: {nameStr} не найден");
                        break;
                }
            }
        }
    }
}
