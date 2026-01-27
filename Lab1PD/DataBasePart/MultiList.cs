using Lab1PD.Core.ManyToMany;

namespace Lab1PD.DataBasePart;

/// <summary>
/// Реализация структуры "Мультисписок" (Multi-list) для связи "многие-ко-многим".
/// Эта структура позволяет эффективно обходить связи как со стороны Студента, так и со стороны Курса.
/// </summary>
public class MultiList
{
    private readonly StudentsTable _students = new StudentsTable();
    private readonly CoursesTable _courses = new CoursesTable();

    // =================================================================================================
    // 1. ДОБАВЛЕНИЕ ЭЛЕМЕНТОВ
    // =================================================================================================

    /// <summary> Регистрирует нового студента в системе. </summary>
    public void AddNewStudent(char[] name) => _students.Insert(name, new StudentNode(name));

    /// <summary> Регистрирует новый учебный курс в системе. </summary>
    public void AddNewCourse(char[] name) => _courses.Insert(name, new CourseNode(name));

    /// <summary>
    /// Создает связь между студентом и курсом. 
    /// Новое звено LinkNode вставляется в начало обоих кольцевых списков.
    /// </summary>
    public void AddStudentToCourse(char[] studentName, char[] courseName)
    {
        StudentNode? sNode = _students.Get(studentName);
        CourseNode? cNode = _courses.Get(courseName);

        // Проверка на существование обоих сущностей
        if (sNode == null || cNode == null) return;

        // Проверка на дубликат: если связь уже существует, повторно не добавляем
        if (GetPrevNodeStudentAtCourse(sNode, cNode) != null) return;

        LinkNode newLink = new LinkNode();

        // --- Привязка к списку студента ---
        // Если список пуст, указываем на заголовок, иначе на текущий первый элемент
        if (sNode.Course == null) {
            sNode.Course = newLink;
            newLink.Student = sNode; // Кольцо замыкается на студенте
        } else {
            newLink.Student = sNode.Course; // Вставка в начало: новый узел ссылается на старую "голову"
            sNode.Course = newLink;         // Заголовок теперь указывает на новый узел
        }

        // --- Привязка к списку курса (Горизонтальная ось) ---
        if (cNode.Student == null) {
            cNode.Student = newLink;
            newLink.Course = cNode; // Кольцо замыкается на курсе
        } else {
            newLink.Course = cNode.Student; // Вставка в начало
            cNode.Student = newLink;
        }
    }

    // =================================================================================================
    // 2. ВЫВОД ДАННЫХ (НАВИГАЦИЯ)
    // =================================================================================================

    /// <summary>
    /// Проходит по цепочке связей студента и выводит названия всех его курсов.
    /// </summary>
    public void PrintCoursesOfStudent(char[] studentName)
    {
        StudentNode? sNode = _students.Get(studentName);
        if (sNode == null) { Console.WriteLine($"Студент {new string(studentName)} не найден."); return; }

        Console.Write($"{new string(studentName)}: ");
        Base? cur = sNode.Course;
        // Итерируемся, пока не вернемся по кольцу обратно к заголовочному узлу StudentNode
        while (cur != null && cur != sNode)
        {
            // Для каждой связи (LinkNode) ищем заголовок курса, чтобы узнать его имя
            Console.Write($"{FindHeaderCourse((LinkNode)cur)}  ");
            cur = ((LinkNode)cur).Student; // Переход по "вертикальной" линии студента
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Проходит по цепочке связей курса и выводит имена всех записанных студентов.
    /// </summary>
    public void PrintStudentsOfCourse(char[] courseName)
    {
        CourseNode? cNode = _courses.Get(courseName);
        if (cNode == null) { Console.WriteLine($"Курс {new string(courseName)} не найден."); return; }

        Console.Write($"{new string(courseName)}: ");
        Base? cur = cNode.Student;
        // Итерируемся, пока не вернемся по кольцу обратно к заголовочному узлу CourseNode
        while (cur != null && cur != cNode)
        {
            // Для каждой связи (LinkNode) ищем заголовок студента, чтобы узнать имя
            Console.Write($"{FindHeaderStudent((LinkNode)cur)}  ");
            cur = ((LinkNode)cur).Course; // Переход по "горизонтальной" линии курса
        }
        Console.WriteLine();
    }

    // =================================================================================================
    // 3. УДАЛЕНИЕ (ПЕРЕПРИВЯЗКА УКАЗАТЕЛЕЙ)
    // =================================================================================================

    /// <summary>
    /// Удаляет связь между студентом и курсом, исключая узел LinkNode из обоих списков.
    /// </summary>
    public void RemoveStudentFromCourse(char[] studentName, char[] courseName)
    {
        StudentNode? sNode = _students.Get(studentName);
        CourseNode? cNode = _courses.Get(courseName);

        if (sNode == null || cNode == null) return;

        // Находим предыдущие узлы в обоих списках, чтобы "перепрыгнуть" через удаляемый узел
        Base? prevInS = GetPrevNodeStudentAtCourse(sNode, cNode);
        Base? prevInC = GetPrevNodeCourseAtStudent(sNode, cNode);

        if (prevInS != null && prevInC != null)
        {
            RemoveRelFromStudent(prevInS); 
            RemoveRelFromCourse(prevInC);  
        }
    }

    // =================================================================================================
    // 4. ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
    // =================================================================================================

    /// <summary> Находит заголовочный узел Студента </summary>
    private static StudentNode FindHeaderStudent(LinkNode link)
    {
        Base cur = link;
        while (cur.HasNext) 
        {
            cur = ((LinkNode)cur).Student;
        }
        return (StudentNode)cur;
    }

    /// <summary> Находит заголовочный узел Курса </summary>
    private static CourseNode FindHeaderCourse(LinkNode link)
    {
        Base cur = link;
        while (cur.HasNext) 
        {
            cur = ((LinkNode)cur).Course;
        }
        return (CourseNode)cur;
    }

    /// <summary> Исключает узел из списка студента. Принимает ПРЕДЫДУЩИЙ узел. </summary>
    private static void RemoveRelFromStudent(Base prev)
    {
        if (!prev.HasNext) 
        {
            // Если prev — это StudentNode (заголовок)
            StudentNode node = (StudentNode)prev;
            LinkNode target = node.Course!;
            Base next = target.Student;

            // Если следующий узел имеет HasNext=true, значит это LinkNode, иначе — заголовок (null)
            node.Course = next.HasNext ? (LinkNode)next : null;
        } 
        else 
        {
            // Если prev — это LinkNode
            LinkNode link = (LinkNode)prev;
            // Берем целевой узел (который удаляем)
            LinkNode target = (LinkNode)link.Student;
            // Перепрыгиваем через него к следующему (который может быть как LinkNode, так и StudentNode)
            link.Student = target.Student;
        }
    }

    /// <summary> Исключает узел из списка курса. Принимает ПРЕДЫДУЩИЙ узел. </summary>
    private static void RemoveRelFromCourse(Base prev)
    {
        if (!prev.HasNext) 
        {
            // Если prev — это CourseNode (заголовок)
            CourseNode node = (CourseNode)prev;
            LinkNode target = node.Student!;
            Base next = target.Course;

            node.Student = next.HasNext ? (LinkNode)next : null;
        } 
        else 
        {
            // Если prev — это LinkNode
            LinkNode link = (LinkNode)prev;
            LinkNode target = (LinkNode)link.Course;
            link.Course = target.Course;
        }
    }

    /// <summary> Поиск узла, который стоит ПЕРЕД связью со стороны студента. </summary>
    private static Base? GetPrevNodeStudentAtCourse(StudentNode sNode, CourseNode cNode)
    {
        Base prev = sNode; 
        Base? cur = sNode.Course;
        while (cur != null && cur != sNode) {
            if (FindHeaderCourse((LinkNode)cur) == cNode) return prev;
            prev = cur; 
            cur = ((LinkNode)cur).Student;
        }
        return null;
    }

    /// <summary> Поиск узла, который стоит ПЕРЕД связью со стороны курса. </summary>
    private static Base? GetPrevNodeCourseAtStudent(StudentNode sNode, CourseNode cNode)
    {
        Base prev = cNode; 
        Base? cur = cNode.Student;
        while (cur != null && cur != cNode) {
            if (FindHeaderStudent((LinkNode)cur) == sNode) return prev;
            prev = cur; 
            cur = ((LinkNode)cur).Course;
        }
        return null;
    }
}