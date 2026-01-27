using Lab1PD.Core;
using Lab1PD.ListADT;
using Lab1PD.Map;
using Lab1PD.DataBasePart;
using Lab1PD.Hashing;

namespace Lab1PD
{
    internal class Program
    {
        /// <summary>
        /// Метод удаления дубликатов из списка.
        /// </summary>
        /// <param name="list">Список, из которого нужно удалить дубликаты.</param>
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

        /// <summary>
        /// Метод конвертации строки в массив символов.
        /// </summary>
        /// <param name="str">Строка для конвертации.</param>
        /// <returns>Массив символов.</returns>
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
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Демонстрация задания 1 - ADT List.
        /// </summary>
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

        /// <summary>
/// Демонстрация задания 2 - Stack, Queue, Map.
/// </summary>
private static void DemoCollections()
{
    Console.WriteLine("\n=== Задание 2: Stack, Queue, Map ===");

    MapADT<char, int> map = new MapADT<char, int>();
    
    // Создаем все коллекции
    var arrQueue = new Lab1PD.Queue.Array.Queue<char>();
    var arrStack = new Lab1PD.Stack.Array.Stack<char>();
    var linkQueue = new Lab1PD.Queue.Linked.Queue<char>();
    var linkStack = new Lab1PD.Stack.Linked.Stack<char>();
    var listQueue = new Lab1PD.Queue.List.Queue<char>();
    var adtStack = new Lab1PD.Stack.ADT.Stack<char>();

    string testString = "Hello World";
    Console.WriteLine($"Исходная строка: {testString}");
    Console.WriteLine($"Длина строки: {testString.Length} символов");

    // Заполняем все коллекции и строим частотный словарь
    int position = 0;
    foreach (char ch in testString)
    {
        // ДЛЯ ЧАСТОТНОГО СЛОВАРЯ (подсчет количества)
        int currentCount = 0;
        if (map.Compute(ch, ref currentCount))
        {
            // Если символ уже есть в словаре, увеличиваем счетчик
            map.Assign(ch, currentCount + 1);
        }
        else
        {
            // Если символа нет в словаре, добавляем с начальным счетчиком 1
            map.Assign(ch, 1);
        }

        // ДЛЯ СЛОВАРЯ ПОЗИЦИЙ (первая позиция) - раскомментировать, если нужно
        // int currentValue = 0;
        // if (!map.Compute(ch, ref currentValue)) // Если ключ не найден
        // {
        //     map.Assign(ch, position); // Только первая позиция
        // }

        // Добавляем во все очереди
        arrQueue.Enqueue(ch);
        linkQueue.Enqueue(ch);
        listQueue.Enqueue(ch);
        
        // Добавляем во все стеки
        arrStack.Push(ch);
        linkStack.Push(ch);
        adtStack.Push(ch);
        
        position++;
    }

    // Выводим Map в текстовом представлении
    Console.WriteLine("\n1. Отображение (частотный словарь символов):");
    Console.Write("   ");
    map.PrintList();  // Вызовет метод PrintList из класса MapADT

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

        /// <summary>
        /// Демонстрация задания 3 - M2M (MultiList).
        /// </summary>
        private static void DemoMultiList()
        {
            Console.WriteLine("\n=== Задание 3: M2M - Многокурсовая система ===");

            var db = new MultiList();

            Console.WriteLine("\n1. Добавление студентов и курсов");
            db.AddNewStudent(ToCharArray("Vlad"));
            db.AddNewStudent(ToCharArray("Alina"));
            db.AddNewStudent(ToCharArray("John"));
            db.AddNewCourse(ToCharArray("Math"));
            db.AddNewCourse(ToCharArray("Programming"));
            db.AddNewCourse(ToCharArray("Physics"));

            Console.WriteLine("\n2. Запись студентов на курсы");
            db.AddStudentToCourse(ToCharArray("Vlad"), ToCharArray("Math"));
            db.AddStudentToCourse(ToCharArray("Vlad"), ToCharArray("Programming"));
            db.AddStudentToCourse(ToCharArray("Alina"), ToCharArray("Math"));
            db.AddStudentToCourse(ToCharArray("John"), ToCharArray("Physics"));
            db.AddStudentToCourse(ToCharArray("John"), ToCharArray("Programming"));

            Console.WriteLine("\n3. Курсы студента Vlad:");
            db.PrintCoursesOfStudent(ToCharArray("Vlad"));

            Console.WriteLine("\n4. Курсы студента Alina:");
            db.PrintCoursesOfStudent(ToCharArray("Alina"));

            Console.WriteLine("\n5. Курсы студента John:");
            db.PrintCoursesOfStudent(ToCharArray("John"));

            Console.WriteLine("\n6. Студенты на курсе Math:");
            db.PrintStudentsOfCourse(ToCharArray("Math"));

            Console.WriteLine("\n7. Студенты на курсе Programming:");
            db.PrintStudentsOfCourse(ToCharArray("Programming"));

            Console.WriteLine("\n8. Удаление студента John из курса Programming:");
            db.RemoveStudentFromCourse(ToCharArray("John"), ToCharArray("Programming"));
            db.PrintCoursesOfStudent(ToCharArray("John"));

            Console.WriteLine("\n9. Студенты на курсе Programming после удаления:");
            db.PrintStudentsOfCourse(ToCharArray("Programming"));
            
            
            Console.WriteLine("\n10. Удаление студента со всех курсов:");
            db.RemoveStudentFromCourse(ToCharArray("John"), ToCharArray("Physics"));

            db.PrintCoursesOfStudent(ToCharArray("John"));
            
            Console.WriteLine("\n11. Студенты на курсе Programming после удаления:");
            db.PrintStudentsOfCourse(ToCharArray("Programming"));
        }

        /// <summary>
        /// Демонстрация задания 4 - Hashing.
        /// </summary>
        private static void DemoHashing()
        {
            Console.WriteLine("\n=== Задание 4: Hashing - Система классификации ===");

            CloseHashedDictionary goodGuys = new CloseHashedDictionary();
            CloseHashedDictionary badGuys = new CloseHashedDictionary();

            Console.WriteLine("Команды имитируются автоматически...\n");

            // Тестовые команды
            string[] commands =
            {
                "F Batman",
                "F Btaman",
                "P",
                "U Btaman",
                "U Batman",

                "P"

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
