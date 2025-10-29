using Lab1PD.Core;
using Lab1PD.Core.DoubleLinkedList;

namespace Lab1PD.ListADT;

/// <summary>
/// Класс <c>DoubleLinkedListADT&lt;T&gt;</c>
/// =======================================
/// Реализация абстрактного типа данных (ADT) «Двусвязный список».
/// 
/// Каждый узел хранит:
///  - данные типа T;
///  - ссылку на следующий элемент;
///  - ссылку на предыдущий элемент.
/// 
/// Поддерживаются операции вставки, удаления, поиска, перемещения по списку и очистки.
/// 
/// Индексация позиций начинается с 1 (а не с 0), в соответствии с формальными определениями ADT-списков.
/// </summary>
public class DoubleLinkedListAdt<T> : IListADT<T>
{
    // -------------------------------
    // Поля
    // -------------------------------

    private Node<T>? _head; // первый элемент списка
    private Node<T>? _tail; // последний элемент списка
    private int _count; // количество элементов в списке

    // -------------------------------
    // Конструктор
    // -------------------------------

    /// <summary>
    /// Создаёт пустой двусвязный список.
    /// </summary>
    public DoubleLinkedListAdt()
    {
        _head = null;
        _tail = null;
        _count = 0;
    }

    // -------------------------------
    // Основные методы интерфейса IListADT
    // -------------------------------

    /// <summary>
    /// Возвращает позицию "конца" списка — фиктивную позицию после последнего элемента.
    /// Используется для обозначения отсутствия элемента (аналог NULL-позиции).
    /// </summary>
    public int End() => _count + 1;

    /// <summary>
    /// Вставляет элемент <paramref name="x"/> на позицию <paramref name="p"/>.
    /// 
    /// Возможные случаи:
    ///  - вставка в начало (p == 1);
    ///  - вставка в конец (p == count + 1);
    ///  - вставка в середину списка.
    /// </summary>
    public void Insert(T x, int p)
    {
        if (p < 1 || p > _count + 1)
            throw new ArgumentException("Недопустимая позиция для вставки");

        Node<T> newNode = new Node<T>(x);

        // Вставка в начало
        if (p == 1)
        {
            if (_head == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Next = _head;
                _head.Previous = newNode;
                _head = newNode;
            }
        }
        // Вставка в конец
        else if (p == _count + 1)
        {
            if (_tail == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                _tail.Next = newNode;
                newNode.Previous = _tail;
                _tail = newNode;
            }
        }
        // Вставка в середину списка
        else
        {
            var current = GetNodeAt(p);
            if (current == null) return;

            var prev = current.Previous;
            if (prev != null)
                prev.Next = newNode;

            newNode.Previous = prev;
            newNode.Next = current;
            current.Previous = newNode;
        }

        _count++;
    }

    /// <summary>
    /// Находит позицию первого вхождения элемента <paramref name="x"/>.
    /// Если элемент не найден — возвращает End().
    /// </summary>
    public int Locate(T x)
    {
        var current = _head;
        int pos = 1;

        while (current != null)
        {
            if (Equals(current.Data, x))
                return pos;
            current = current.Next;
            pos++;
        }

        return End();
    }

    /// <summary>
    /// Возвращает значение элемента, находящегося на позиции <paramref name="p"/>.
    /// </summary>
    public T Retrieve(int p)
    {
        if (p < 1 || p > _count)
            throw new ArgumentException("Недопустимая позиция");

        var node = GetNodeAt(p);
        return node != null ? node.Data! : default!;
    }

    /// <summary>
    /// Удаляет элемент, находящийся на позиции <paramref name="p"/>.
    /// После удаления связи корректируются.
    /// </summary>
    public void Delete(int p)
    {
        if (p < 1 || p > _count)
            throw new ArgumentException("Недопустимая позиция");

        var current = GetNodeAt(p);
        if (current == null) return;

        // Случай 1: в списке один элемент
        if (_head == _tail && current == _head)
        {
            _head = _tail = null;
        }
        // Случай 2: удаление головы
        else if (current == _head)
        {
            _head = _head.Next;
            if (_head != null)
                _head.Previous = null;
        }
        // Случай 3: удаление хвоста
        else if (current == _tail)
        {
            _tail = _tail.Previous;
            if (_tail != null)
                _tail.Next = null;
        }
        // Случай 4: удаление из середины
        else
        {
            var prev = current.Previous;
            var next = current.Next;
            if (prev != null) prev.Next = next;
            if (next != null) next.Previous = prev;
        }

        _count--;
    }

    /// <summary>
    /// Возвращает позицию следующего элемента.
    /// Если элемент последний, возвращает End().
    /// </summary>
    public int Next(int p)
    {
        if (p < 1 || p > _count)
            throw new ArgumentException("Неправильная позиция");

        return (p == _count) ? End() : p + 1;
    }

    /// <summary>
    /// Возвращает позицию предыдущего элемента.
    /// Если элемент первый — выбрасывает исключение.
    /// </summary>
    public int Previous(int p)
    {
        if (p < 1 || p > _count)
            throw new ArgumentException("Неправильная позиция");

        if (p == 1)
            throw new InvalidOperationException("Перед первой позицией ничего нет");

        return p - 1;
    }

    /// <summary>
    /// Полностью очищает список.
    /// Возвращает фиктивную позицию конца (End()).
    /// </summary>
    public int Makenull()
    {
        _head = null;
        _tail = null;
        _count = 0;
        return End();
    }

    /// <summary>
    /// Возвращает позицию первого элемента списка.
    /// Если список пуст — возвращает End().
    /// </summary>
    public int First()
    {
        return (_count > 0) ? 1 : End();
    }

    /// <summary>
    /// Печатает содержимое списка в консоль.
    /// Каждый элемент выводится с индексом.
    /// </summary>
    public void PrintList()
    {
        if (_head == null)
        {
            Console.WriteLine("Список пуст.");
            return;
        }

        var current = _head;
        int i = 1;

        while (current != null)
        {
            Console.WriteLine($"{i}. {current.Data}");
            current = current.Next;
            i++;
        }
    }

    // -------------------------------
    // Вспомогательные методы
    // -------------------------------

    /// <summary>
    /// Возвращает ссылку на узел, находящийся на заданной позиции.
    /// Если позиция некорректна — возвращает null.
    /// </summary>
    private Node<T>? GetNodeAt(int position)
    {
        if (position < 1 || position > _count)
            return null;

        var current = _head;
        for (int i = 1; i < position; i++)
            current = current?.Next;

        return current;
    }
}