using Lab1PD.Core.ManyToMany;

namespace Lab1PD.DataBasePart;

/// <summary>
/// Класс для реализации связи "многие ко многим" между студентами и курсами
/// с использованием двух хэш-таблиц и связных списков
/// </summary>
public class MultiList
{
    /// <summary>
    /// Размер хэш-таблиц для студентов и курсов
    /// </summary>
    private const int Size = 3;
    
    /// <summary>
    /// Хэш-таблица для хранения узлов студентов
    /// </summary>
    private readonly StudentNode?[] _studentNodes = new StudentNode[Size];
    
    /// <summary>
    /// Хэш-таблица для хранения узлов курсов
    /// </summary>
    private readonly CourseNode?[] _courseNodes = new CourseNode[Size];

    /// <summary>
    /// Добавляет нового студента в коллекцию
    /// </summary>
    /// <param name="name">Имя студента</param>
    public void AddNewStudent(string name)
    {
        // Вычисляем хэш для имени студента
        int hash = Hash(name.ToCharArray());

        // Используем линейное пробирование для разрешения коллизий
        while (_studentNodes[hash] != null)
        {
            hash = NextHash(hash);
        }
        
        // Создаем и сохраняем новый узел студента
        _studentNodes[hash] = new StudentNode(name);
    }
    
    /// <summary>
    /// Добавляет новый курс в коллекцию
    /// </summary>
    /// <param name="name">Название курса</param>
    public void AddNewCourse(string name)
    {
        // Вычисляем хэш для названия курса
        int hash = Hash(name.ToCharArray());

        // Используем линейное пробирование для разрешения коллизий
        while (_courseNodes[hash] != null)
        {
            hash = NextHash(hash);
        }
        
        // Создаем и сохраняем новый узел курса
        _courseNodes[hash] = new CourseNode(name);
    }

    /// <summary>
    /// Связывает студента с курсом (добавляет студента на курс)
    /// </summary>
    /// <param name="studentName">Имя студента</param>
    /// <param name="courseName">Название курса</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если студент или курс не найдены
    /// </exception>
    public void AddStudentToCourse(string studentName, string courseName)
    {
        // Получаем узлы студента и курса
        StudentNode? studentNode = GetStudentNode(studentName.ToCharArray());
        if (studentNode == null) throw new InvalidOperationException("Студента не существует в коллекции.");
        CourseNode? courseNode = GetCourseNode(courseName.ToCharArray());
        if (courseNode == null) throw new InvalidOperationException("Курс не существует в коллекции.");

        LinkNode link;
        
        // Если у студента еще нет курсов
        if (studentNode.Course == null)
        {
            link = new LinkNode();
            studentNode.Course = link;
            link.Student = studentNode;
            
            // Если на курсе еще нет студентов
            if (courseNode.Student == null)
            {
                link.Course = courseNode;
                courseNode.Student = link;
                return;
            }
            
            // У студента нет курсов, но на курсе студенты уже есть
            link.Course = courseNode.Student;
            courseNode.Student = link;
            return;
        }

        // Проверяем, не записан ли студент уже на этот курс
        if (GetPrevNodeStudentAtCourse(studentNode, courseNode) != null) return;
        
        // Создаем новую связь
        link = new LinkNode();
        link.Student = studentNode.Course;
        studentNode.Course = link;

        // У студента есть другие курсы, но на курсе нет студентов
        if (courseNode.Student == null)
        {
            link.Course = courseNode;
            courseNode.Student = link;
            return;
        }
        
        // И у студента есть курсы, и у курса есть студенты
        link.Course = courseNode.Student;
        courseNode.Student = link;
    }

    /// <summary>
    /// Удаляет связь между студентом и курсом (отчисляет студента с курса)
    /// </summary>
    /// <param name="studentName">Имя студента</param>
    /// <param name="courseName">Название курса</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если студент или курс не найдены
    /// </exception>
    public void RemoveStudentFromCourse(string studentName, string courseName)
    {
        // Получаем узлы студента и курса
        StudentNode? studentNode = GetStudentNode(studentName.ToCharArray());
        if (studentNode == null) throw new InvalidOperationException("Студента не существует в коллекции.");
        CourseNode? courseNode = GetCourseNode(courseName.ToCharArray());
        if (courseNode == null) throw new InvalidOperationException("Курс не существует в коллекции.");

        // Ищем предыдущий узел в цепочке студента
        IDefaultNode? prevStudent = GetPrevNodeStudentAtCourse(studentNode, courseNode);

        // Если связь не найдена, ничего не делаем
        if (prevStudent == null) return;

        // Удаляем связь со стороны студента
        RemoveRelFromStudent(prevStudent);
        
        // Ищем предыдущий узел в цепочке курса
        IDefaultNode prevCourse = GetPrevNodeCourseAtStudent(studentNode, courseNode)!;

        // Удаляем связь со стороны курса
        RemoveRelFromCourse(prevCourse);
    }

    /// <summary>
    /// Удаляет студента и все его связи с курсами
    /// </summary>
    /// <param name="studentName">Имя студента</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если студент не найден
    /// </exception>
    public void RemoveStudent(string studentName)
    {
        // Получаем узел студента
        StudentNode? studentNode = GetStudentNode(studentName.ToCharArray());
        if (studentNode == null) throw new InvalidOperationException("Студента не существует в коллекции.");

        // Проходим по всем курсам студента и удаляем связи
        IDefaultNode? cur = studentNode.Course;
        if (cur == null) return;
        
        while (cur != studentNode)
        {
            IDefaultNode curNode = cur;
            // Ищем конечный узел в цепочке (курс)
            while (curNode.HasNext)
            {
                curNode = ((LinkNode)curNode).Course;
            }

            // Находим предыдущий узел для удаления связи со стороны курса
            IDefaultNode prevCourse = GetPrevNodeFromCourseWay((CourseNode)curNode, (LinkNode)cur)!;
            RemoveRelFromCourse(prevCourse);
            cur = ((LinkNode)cur).Student;
        }

        // Обнуляем ссылку на курсы у студента
        studentNode.Course = null;
    }

    /// <summary>
    /// Удаляет курс и все его связи со студентами
    /// </summary>
    /// <param name="courseName">Название курса</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если курс не найден
    /// </exception>
    public void RemoveCourse(string courseName)
    {
        // Получаем узел курса
        CourseNode? courseNode = GetCourseNode(courseName.ToCharArray());
        if (courseNode == null) throw new InvalidOperationException("Курс не существует в коллекции.");
        
        // Проходим по всем студентам курса и удаляем связи
        IDefaultNode? cur = courseNode.Student;
        if (cur == null) return;
        
        while (cur != courseNode)
        {
            IDefaultNode curNode = cur;
            // Ищем конечный узел в цепочке (студента)
            while (curNode.HasNext)
            {
                curNode = ((LinkNode)curNode).Student;
            }

            // Находим предыдущий узел для удаления связи со стороны студента
            IDefaultNode prevCourse = GetPrevNodeFromStudentWay((StudentNode)curNode, (LinkNode)cur)!;
            RemoveRelFromStudent(prevCourse);
            cur = ((LinkNode)cur).Course;
        }

        // Обнуляем ссылку на студентов у курса
        courseNode.Student = null;
    }

    /// <summary>
    /// Выводит список студентов, записанных на курс
    /// </summary>
    /// <param name="courseName">Название курса</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если курс не найден
    /// </exception>
    public void PrintStudentsOfCourse(string courseName)
    {
        // Получаем узел курса
        CourseNode? courseNode = GetCourseNode(courseName.ToCharArray());
        if (courseNode == null) throw new InvalidOperationException("Курс не существует в коллекции.");

        Console.Write($"{courseName}: ");
        IDefaultNode? cur = courseNode.Student;
        
        // Если на курсе нет студентов
        if (cur == null)
        {
            Console.WriteLine();
            return;
        }

        // Проходим по всем студентам курса
        while (cur != courseNode)
        {
            IDefaultNode curNode = cur;
            // Ищем конечный узел (студента) в цепочке
            while (curNode.HasNext)
            {
                curNode = ((LinkNode)curNode).Student;
            }
            
            // Выводим имя студента
            Console.Write($"{curNode}  ");
            cur = ((LinkNode)cur).Course;
        }
        
        Console.WriteLine();
    }
    
    /// <summary>
    /// Выводит список курсов, на которые записан студент
    /// </summary>
    /// <param name="studentName">Имя студента</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если студент не найден
    /// </exception>
    public void PrintCoursesOfStudent(string studentName)
    {
        // Получаем узел студента
        StudentNode? studentNode = GetStudentNode(studentName.ToCharArray());
        if (studentNode == null) throw new InvalidOperationException("Студента не существует в коллекции.");

        Console.Write($"{studentName}: ");
        IDefaultNode? cur = studentNode.Course;
        
        // Если у студента нет курсов
        if (cur == null)
        {
            Console.WriteLine();
            return;
        }

        // Проходим по всем курсам студента
        while (cur != studentNode)
        {
            IDefaultNode curNode = cur;
            // Ищем конечный узел (курс) в цепочке
            while (curNode.HasNext)
            {
                curNode = ((LinkNode)curNode).Course;
            }
            
            // Выводим название курса
            Console.Write($"{curNode}  ");
            cur = ((LinkNode)cur).Student;
        }
        
        Console.WriteLine();
    }
    
    /// <summary>
    /// Находит узел студента по имени
    /// </summary>
    /// <param name="name">Имя студента в виде массива символов</param>
    /// <returns>Узел студента или null если не найден</returns>
    private StudentNode? GetStudentNode(char[] name)
    {
        int hash = Hash(name);
        int cur = hash;
        // Обходим хэш-таблицу с использованием линейного пробирования
        do
        {
            if (ArraysEqual(_studentNodes[cur]?.Name, name)) return _studentNodes[cur];
            cur = NextHash(cur);
        } while (cur != hash);
        
        return null;
    }

    /// <summary>
    /// Находит узел курса по названию
    /// </summary>
    /// <param name="name">Название курса в виде массива символов</param>
    /// <returns>Узел курса или null если не найден</returns>
    private CourseNode? GetCourseNode(char[] name)
    {
        int hash = Hash(name);
        int cur = hash;
        // Обходим хэш-таблицу с использованием линейного пробирования
        do
        {
            if (ArraysEqual(_courseNodes[cur]?.Name, name)) return _courseNodes[cur];
            cur = NextHash(cur);
        } while (cur != hash);
        
        return null;
    }

    /// <summary>
    /// Удаляет связь со стороны студента
    /// </summary>
    /// <param name="prevStudent">Предыдущий узел в цепочке студента</param>
    private static void RemoveRelFromStudent(IDefaultNode prevStudent)
    {
        // Если предыдущий узел - это связующий узел (LinkNode)
        if (prevStudent.HasNext)
        {
            LinkNode prevLink = (LinkNode)prevStudent;
            prevLink.Student = ((LinkNode)prevLink.Student).Student;
        }
        else // Если предыдущий узел - это сам студент (StudentNode)
        {
            StudentNode prevNode = (StudentNode)prevStudent;
            IDefaultNode newNode = prevNode.Course!.Student;
            prevNode.Course = newNode.HasNext ? (LinkNode)newNode : null;
        }
    }

    /// <summary>
    /// Удаляет связь со стороны курса
    /// </summary>
    /// <param name="prevCourse">Предыдущий узел в цепочке курса</param>
    private static void RemoveRelFromCourse(IDefaultNode prevCourse)
    {
        // Если предыдущий узел - это связующий узел (LinkNode)
        if (prevCourse.HasNext)
        {
            LinkNode prevLink = (LinkNode)prevCourse;
            prevLink.Course = ((LinkNode)prevLink.Course).Course;
        }
        else // Если предыдущий узел - это сам курс (CourseNode)
        {
            CourseNode prevNode = (CourseNode)prevCourse;
            IDefaultNode newNode = prevNode.Student!.Course;
            prevNode.Student = newNode.HasNext ? (LinkNode)newNode : null;
        }
    }
    
    /// <summary>
    /// Находит предыдущий узел в цепочке курсов студента для заданного курса
    /// </summary>
    /// <param name="studentNode">Узел студента</param>
    /// <param name="courseNode">Узел курса</param>
    /// <returns>Предыдущий узел или null если связь не найдена</returns>
    private static IDefaultNode? GetPrevNodeStudentAtCourse(StudentNode studentNode, CourseNode courseNode)
    {
        IDefaultNode prev = studentNode;
        IDefaultNode? cur = studentNode.Course;
        
        if (cur == null) return null;
        
        // Обходим цепочку курсов студента
        while (cur != studentNode)
        {
            LinkNode curLink = (LinkNode)cur;
            IDefaultNode curNode = curLink.Course;
            
            // Идем до конца цепочки (до узла курса)
            while (curNode.HasNext)
            {
                curNode = ((LinkNode)curNode).Course;
            }

            // Если нашли нужный курс
            if (curNode == courseNode)
            {
                return prev;
            }

            prev = cur;
            cur = curLink.Student;
        }
        
        return null;
    }

    /// <summary>
    /// Находит предыдущий узел в цепочке студентов курса для заданного студента
    /// </summary>
    /// <param name="studentNode">Узел студента</param>
    /// <param name="courseNode">Узел курса</param>
    /// <returns>Предыдущий узел или null если связь не найдена</returns>
    private static IDefaultNode? GetPrevNodeCourseAtStudent(StudentNode studentNode, CourseNode courseNode)
    {
        IDefaultNode prev = courseNode;
        IDefaultNode? cur = courseNode.Student;

        if (cur == null) return null;
        
        // Обходим цепочку студентов курса
        while (cur != courseNode)
        {
            LinkNode curLink = (LinkNode)cur;
            IDefaultNode curNode = curLink.Student;
            
            // Идем до конца цепочки (до узла студента)
            while (curNode.HasNext)
            {
                curNode = ((LinkNode)curNode).Student;
            }

            // Если нашли нужного студента
            if (curNode == studentNode)
            {
                return prev;
            }

            prev = cur;
            cur = curLink.Course;
        }
        
        return null;
    }

    /// <summary>
    /// Находит предыдущий узел в цепочке курса для заданного связующего узла
    /// </summary>
    /// <param name="courseNode">Узел курса</param>
    /// <param name="targetNode">Целевой связующий узел</param>
    /// <returns>Предыдущий узел или null если не найден</returns>
    private static IDefaultNode? GetPrevNodeFromCourseWay(CourseNode courseNode, LinkNode targetNode)
    {
        IDefaultNode prev = courseNode;
        IDefaultNode? cur = courseNode.Student;
        
        if (cur == null) return null;

        // Обходим цепочку студентов курса
        while (cur != courseNode)
        {
            if (cur == targetNode) return prev;
            
            prev = cur;
            cur = ((LinkNode)cur).Course;
        } 
        
        return null;
    }

    /// <summary>
    /// Находит предыдущий узел в цепочке студента для заданного связующего узла
    /// </summary>
    /// <param name="studentNode">Узел студента</param>
    /// <param name="targetNode">Целевой связующий узел</param>
    /// <returns>Предыдущий узел или null если не найден</returns>
    private static IDefaultNode? GetPrevNodeFromStudentWay(StudentNode studentNode, LinkNode targetNode)
    {
        IDefaultNode prev = studentNode;
        IDefaultNode? cur = studentNode.Course;
        
        if (cur == null) return null;

        // Обходим цепочку курсов студента
        while (cur != studentNode)
        {
            if (cur == targetNode) return prev;
            
            prev = cur;
            cur = ((LinkNode)cur).Student;
        } 
        
        return null;
    }
    
    /// <summary>
    /// Вычисляет хэш-значение для массива символов
    /// </summary>
    /// <param name="name">Массив символов (имя)</param>
    /// <returns>Хэш-значение в диапазоне [0, Size-1]</returns>
    private int Hash(char[] name)
    {
        int sum = 0;
        int i = 0;
        // Суммируем коды символов до нулевого символа
        while (i < name.Length && name[i] != '\0')
        {
            sum += name[i++];
        }

        // Используем деление по модулю для получения индекса
        return sum % Size;
    }
    
    /// <summary>
    /// Вычисляет следующий хэш при линейном пробировании
    /// </summary>
    /// <param name="hash">Текущий хэш</param>
    /// <returns>Следующий хэш</returns>
    private int NextHash(int hash)
    {
        return (hash + 1) % Size;
    }
    
    /// <summary>
    /// Сравнивает два массива символов
    /// </summary>
    /// <param name="a">Первый массив символов</param>
    /// <param name="b">Второй массив символов</param>
    /// <returns>true если массивы равны, иначе false</returns>
    private static bool ArraysEqual(char[]? a, char[]? b)
    {
        // Проверка на null
        if (a == null || b == null) return a == b;
    
        // Поэлементное сравнение массивов
        for (int i = 0; i < a.Length && a[i] != '\0' && b[i] != '\0'; i++)
        {
            if (a[i] != b[i]) return false;
        }
        
        return true;
    }
}