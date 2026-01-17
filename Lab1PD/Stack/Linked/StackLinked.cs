using Lab1PD.Core;

namespace Lab1PD.Stack.Linked
{
    
// Стек на основе односвязного списка
    public class Stack<T>
    {
        private Node? _head;  // Вершина стека

        // Добавление элемента на вершину
        public void Push(T x)
        {
            Node? prevHead = _head;
            _head = new Node(x, prevHead);  // Новый узел становится головой
        }

        // Извлечение элемента с вершины
        public T Pop()
        {
            T toReturn = _head!.Data;
            _head = _head.Next;  // Перемещаем указатель на следующий узел
            return toReturn;
        }

        // Просмотр вершины без извлечения
        public T Top() => _head!.Data; 
    
        // Проверка пустоты стека
        public bool Empty() => _head is null;

        // Стек на списке никогда не заполняется
        public bool Full() => false; 

        // Очистка стека
        public void MakeNull() => _head = null;
    
        // Узел односвязного списка
        private class Node(T data, Node? next)
        {
            public T Data { get; set; } = data;
            public Node? Next { get; set; } = next;
        }
    }
}
