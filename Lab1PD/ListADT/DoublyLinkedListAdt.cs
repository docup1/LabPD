using Lab1PD.Core;
using Lab1PD.Core.DoubleLinkedList;

namespace Lab1PD.ListADT
{
    /// <summary>
    /// Реализация ADT «Двусвязный список» с использованием логики перестановки данных при вставке.
    /// </summary>
    /// <typeparam name="T">Тип хранимых данных.</typeparam>
    public class DoubleLinkedListAdt<T> : IListAdt<T>
    {
        private Node<T>? _head;
        private Node<T>? _tail;
        private static readonly Position<T> _end = new Position<T>(null!);

        public DoubleLinkedListAdt()
        {
            _head = null;
            _tail = null;
        }

        /// <summary>
        /// 1. Возвращает маркер конца списка.
        /// </summary>
        public IPosition End() => _end;

        /// <summary>
        /// 2. Проверяет валидность позиции: принадлежность текущему списку и отличие от End.
        /// </summary>
        private bool ValidatePosition(IPosition p)
        {
            // 2.1. Если позиция — конец списка, возвращаем false
            if (p == _end || p is not Position<T> pos) return false;
            
            // 2.2. Обход списка с головы
            Node<T>? current = _head;
            while (current != null)
            {
                // 2.3-2.4. Сравнение узлов
                if (current == pos.Node) return true;
                current = current.Next;
            }
            // 2.5. Совпадений не найдено
            return false;
        }

        /// <summary>
        /// 3. Устанавливает двустороннюю связь между двумя узлами.
        /// </summary>
        private void LinkNodes(Node<T>? first, Node<T>? second)
        {
            if (first != null) first.Next = second;
            if (second != null) second.Previous = first;
        }

        /// <summary>
        /// 4. Вставляет элемент obj в позицию p.
        /// Если p == End, вставка производится в хвост.
        /// В остальных случаях данные текущего узла вытесняются в новый узел.
        /// </summary>
        public void Insert(T obj, IPosition p)
        {
            // --- Сценарий A: Вставка в конец (p == _end) ---
            if (p == _end)
            {
                Node<T> newNode = new Node<T>(obj);
                // 4.1. Если список пуст
                if (_head == null)
                {
                    _head = newNode;
                    _tail = newNode;
                }
                else
                {
                    // 4.2. Связываем хвост с новым узлом и обновляем хвост
                    LinkNodes(_tail, newNode);
                    _tail = newNode;
                }
                return;
            }

            // --- Сценарий B: Вставка в существующую позицию ---
            // 4.1. Валидация
            if (!ValidatePosition(p))
                throw new ArgumentException("Передана неверная позиция для вставки.");

            Position<T> pos = (Position<T>)p;
            Node<T> cur = pos.Node!;
            
            // 4.3. Создаем новый узел, в который копируем текущие данные из cur
            Node<T> newNodeForOldData = new Node<T>(cur.Data!);
            
            // 4.4. Перестраиваем связи: ставим новый узел сразу после текущего
            newNodeForOldData.Next = cur.Next;
            if (cur.Next != null) cur.Next.Previous = newNodeForOldData;
            
            cur.Next = newNodeForOldData;
            newNodeForOldData.Previous = cur;
            
            // 4.5. Заменяем данные в текущем узле на новые
            cur.Data = obj;

            // 4.6. Если вставляли в хвост, теперь хвостом стал новый узел с "вытесненными" данными
            if (cur == _tail)
            {
                _tail = newNodeForOldData;
            }
        }

        /// <summary>
        /// 5. Находит позицию первого вхождения объекта obj.
        /// </summary>
        public IPosition Locate(T obj)
        {
            Node<T>? current = _head;
            while (current != null)
            {
                if (Equals(current.Data, obj))
                    return new Position<T>(current);
                current = current.Next;
            }
            return _end;
        }

        /// <summary>
        /// 6. Возвращает данные, хранящиеся в позиции p.
        /// </summary>
        public T Retrieve(IPosition p)
        {
            if (!ValidatePosition(p))
                throw new ArgumentException("Позиция не принадлежит списку или является End().");
            
            return ((Position<T>)p).Node!.Data!;
        }

        /// <summary>
        /// 7. Удаляет узел в позиции p и возвращает позицию следующего за ним элемента.
        /// </summary>
        public IPosition Delete(IPosition p)
        {
            if (!ValidatePosition(p))
                throw new ArgumentException("Невозможно удалить: невалидная позиция.");

            Node<T> nodeToDelete = ((Position<T>)p).Node!;
            Node<T>? nextNode = nodeToDelete.Next;

            // --- Сценарий A: Удаление головы ---
            if (nodeToDelete == _head)
            {
                if (_head == _tail)
                {
                    _head = _tail = null;
                }
                else
                {
                    _head = _head!.Next;
                    _head!.Previous = null;
                }
            }
            // --- Сценарий B: Удаление хвоста ---
            else if (nodeToDelete == _tail)
            {
                _tail = _tail!.Previous;
                _tail!.Next = null;
            }
            // --- Сценарий C: Удаление из середины ---
            else
            {
                LinkNodes(nodeToDelete.Previous, nodeToDelete.Next);
            }

            return nextNode != null ? new Position<T>(nextNode) : _end;
        }

        /// <summary>
        /// 8. Возвращает следующую позицию.
        /// </summary>
        public IPosition Next(IPosition p)
        {
            if (!ValidatePosition(p))
                throw new ArgumentException("Невалидная позиция.");

            Node<T> node = ((Position<T>)p).Node!;
            return node == _tail ? _end : new Position<T>(node.Next);
        }

        /// <summary>
        /// 9. Возвращает предыдущую позицию.
        /// </summary>
        public IPosition Previous(IPosition p)
        {
            if (!ValidatePosition(p))
                throw new ArgumentException("Невалидная позиция.");

            Node<T> node = ((Position<T>)p).Node!;
            if (node == _head)
                throw new InvalidOperationException("Попытка получить Previous для головы списка.");

            return new Position<T>(node.Previous);
        }

        /// <summary>
        /// 10. Возвращает позицию первого элемента.
        /// </summary>
        public IPosition First()
        {
            return _head == null ? _end : new Position<T>(_head);
        }

        /// <summary>
        /// 11. Очищает список.
        /// </summary>
        public IPosition Makenull()
        {
            _head = null;
            _tail = null;
            return _end;
        }

        /// <summary>
        /// 12. Выводит содержимое списка в формате эл1,\nэл2.
        /// </summary>
        public void PrintList()
        {
            Node<T>? current = _head;
            if (current != null)
            {
                Console.Write(current.Data);
                current = current.Next;
                while (current != null)
                {
                    Console.Write($",\n{current.Data}");
                    current = current.Next;
                }
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// 13. Проверяет, пуст ли список.
        /// </summary>
        public bool IsEmpty() => _head == null;

        /// <summary>
        /// Обертка для вставки в конец списка.
        /// </summary>
        public void Add(T obj) => Insert(obj, _end);
    }
}