using Lab1PD.Core;
using Lab1PD.Core.DoubleLinkedList;

namespace Lab1PD.ListADT
{
    /// <summary>
    /// Реализация абстрактного типа данных (ADT) «Двусвязный список».
    /// 
    /// Особенности:
    /// - Каждый узел хранит ссылку на следующий и предыдущий элементы.
    /// - Поддерживает операции вставки, удаления, поиска, навигации и очистки.
    /// - Индексация логически начинается с 1 (для пользователя).
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранимых в списке.</typeparam>
    public class DoubleLinkedListAdt<T> : IListAdt<T>
    {
        private Node<T>? _head;
        private Node<T>? _tail;
        private static readonly Position<T> _End = new Position<T>(null!);

        /// <summary>
        /// Создаёт новый пустой двусвязный список.
        /// </summary>
        public DoubleLinkedListAdt()
        {
            _head = null;
            _tail = null;
        }

        /// <summary>
        /// Возвращает фиктивную позицию конца списка (используется как "нулевая" позиция).
        /// </summary>
        public IPosition End() => _End;

        /// <summary>
        /// Возвращает позицию первого элемента в списке.
        /// Если список пуст, возвращает End().
        /// </summary>
        public IPosition First() => new Position<T>(_head);

        // ======================== ОПЕРАЦИИ МОДИФИКАЦИИ ========================

        /// <summary>
        /// Вставляет элемент <paramref name="x"/> перед позицией <paramref name="p"/>.
        /// Если p = End(), элемент добавляется в конец списка.
        /// </summary>
        /// <param name="x">Добавляемое значение.</param>
        /// <param name="p">Позиция, перед которой выполняется вставка.</param>
        /// <exception cref="ArgumentNullException">Если позиция равна null.</exception>
        /// <exception cref="InvalidOperationException">Если произошла ошибка при вставке.</exception>
        public void Insert(T x, IPosition p)
        {
            // --- Проверяем корректность позиции ---
            if (!ValidatePosition(p))
                throw new ArgumentException("Позиция не принадлежит этому списку.", nameof(p));
            
            if (p == null)
                throw new ArgumentNullException(nameof(p), "Позиция не может быть null.");

            if (p is not Position<T> pos)
                throw new ArgumentException("Переданная позиция имеет неверный тип.", nameof(p));

            Node<T>? target = pos.Node;
            Node<T> newNode = new Node<T>(x);

            // =====================================================================
            //                        1. ВСТАВКА В ПУСТОЙ СПИСОК
            // =====================================================================
            if (_head == null)
            {
                _head = _tail = newNode;
                return;
            }

            // =====================================================================
            //            2. target == null → это End() → вставка В КОНЕЦ
            // =====================================================================
            if (target == null)
            {
                _tail!.Next = newNode;
                newNode.Previous = _tail;
                _tail = newNode;
                return;
            }

            // =====================================================================
            //            3. ВСТАВКА ПЕРЕД ПЕРВЫМ ЭЛЕМЕНТОМ (_head)
            // =====================================================================
            if (target == _head)
            {
                newNode.Next = _head;
                _head.Previous = newNode;
                _head = newNode;
                return;
            }

            // =====================================================================
            //                       4. ВСТАВКА В СЕРЕДИНУ
            // =====================================================================
            Node<T>? prev = target.Previous;

            newNode.Next = target;
            newNode.Previous = prev;

            prev!.Next = newNode;
            target.Previous = newNode;
        }

        /// <summary>
        /// Вставляет элемент <paramref name="p"/> в конец списка./>.
        /// </summary>
        /// <param name="p">Добавляемое значение.</param>
        public void Add(T p) => Insert(p, _End);
        
        
        public IPosition Delete(IPosition p)
        {
            // --- Проверяем корректность позиции ---
            // End() допускается (добавление в конец)
            if (!ValidatePosition(p))
                throw new ArgumentException("Позиция не принадлежит этому списку.", nameof(p));
            
            if (p == null)
                throw new ArgumentNullException(nameof(p), "Позиция не может быть null.");

            // Проверка, что позиция принадлежит этому списку и имеет корректный тип
            if (p is not Position<T> pos)
                throw new ArgumentException("Позиция не принадлежит списку.", nameof(p));

            Node<T>? node = pos.Node;

            // ---- НАЧИНАЕТСЯ БЛОК УДАЛЕНИЯ ----

            // Удаление единственного элемента
            if (node == _head && node == _tail)
            {
                _head = _tail = null;
            }
            // Удаление первого
            else if (node == _head)
            {
                _head = _head!.Next;
                _head!.Previous = null;
            }
            // Удаление последнего
            else if (node == _tail)
            {
                _tail = _tail!.Previous;
                _tail!.Next = null;
            }
            // Удаление из середины
            else
            {
                Node<T>? prev = node.Previous;
                Node<T>? next = node.Next;
                prev!.Next = next;
                next!.Previous = prev;
            }
            
            // Вернуть позицию следующего узла (или End(), если конец)
            // Нужно получить следующий узел ДО удаления
            Node<T>? nextNode = pos.Node.Next;
            return new Position<T>(nextNode); // Если nextNode == null, будет End()

        }


        // ======================== ОПЕРАЦИИ ДОСТУПА ========================

        /// <summary>
        /// Возвращает значение элемента в позиции <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Позиция, откуда нужно получить значение.</param>
        /// <returns>Значение элемента типа T.</returns>
        /// <exception cref="ArgumentNullException">Если позиция равна null.</exception>
        /// <exception cref="ArgumentException">Если позиция недопустима или указывает на конец списка.</exception>
        public T Retrieve(IPosition p)
        {
            // --- Проверяем корректность позиции ---
            // End() допускается (добавление в конец)
            if (!ValidatePosition(p))
                throw new ArgumentException("Позиция не принадлежит этому списку.", nameof(p));
            
            if (p == null)
                throw new ArgumentNullException(nameof(p), "Позиция не может быть null.");

            if (p is not Position<T> pos)
                throw new ArgumentException("Переданная позиция имеет неверный тип.", nameof(p));

            if (pos.Node == null)
                throw new ArgumentException("Невозможно получить значение из позиции End() или недействительной позиции.", nameof(p));

            return pos.Node.Data!;
        }



        /// <summary>
        /// Находит позицию первого элемента, значение которого равно <paramref name="x"/>.
        /// </summary>
        /// <param name="x">Искомое значение.</param>
        /// <returns>Позиция найденного элемента или End(), если элемент не найден.</returns>
        public IPosition Locate(T x)
        {
            if (_head == null)
            {
                // Список пустой, сразу возвращаем End()
                return End();
            }

            Node<T>? current = _head;

            // Проходим по всем узлам
            while (current != null)
            {
                if (Equals(current.Data, x))
                {
                    // Элемент найден, возвращаем его позицию
                    return new Position<T>(current);
                }

                current = current.Next;
            }

            // Элемент не найден
            return End();
        }


        // ======================== НАВИГАЦИЯ ========================

        /// <summary>
        /// Возвращает позицию следующего элемента после <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Текущая позиция.</param>
        /// <returns>Следующая позиция или End(), если достигнут конец списка.</returns>
        public IPosition Next(IPosition p)
        {
            // --- Проверяем корректность позиции ---
            // End() допускается (добавление в конец)
            if (!ValidatePosition(p))
                throw new ArgumentException("Позиция не принадлежит этому списку.", nameof(p));
            
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            if (p is not Position<T> pos)
                throw new ArgumentException("Позиция имеет неверный тип.", nameof(p));

            // P == End() -> Next() = End()
            if (pos.Node == null)
                return End();

            // Если следующий узел отсутствует → возвращаем End()
            if (pos.Node.Next == null)
                return End();

            return new Position<T>(pos.Node.Next);
        }



        /// <summary>
        /// Возвращает позицию предыдущего элемента перед <paramref name="p"/>.
        /// </summary>
        /// <param name="p">Текущая позиция.</param>
        /// <returns>Предыдущая позиция или End(), если элемент первый.</returns>
        public IPosition Previous(IPosition p)
        {
            // --- Проверяем корректность позиции ---
            if (!ValidatePosition(p))
                throw new ArgumentException("Позиция не принадлежит этому списку.", nameof(p));
            
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            Position<T>? pos = p as Position<T>;
            if (pos == null)
                throw new ArgumentException("Позиция имеет неверный тип.", nameof(p));

            // Если позиция = End(), возвращаем хвост (последний элемент)
            if (pos.Node == null)
                return _tail == null ? End() : new Position<T>(_tail);
    
            return new Position<T>(pos.Node.Previous); 
        }

        // ======================== ОБСЛУЖИВАНИЕ ========================

        /// <summary>
        /// Полностью очищает список, удаляя все элементы.
        /// </summary>
        /// <returns>Фиктивную позицию End().</returns>
        public IPosition Makenull()
        {
            // Если список уже пуст, возвращаем End()
            if (_head == null && _tail == null)
                return End();

            // Очищаем список
            _head = null;
            _tail = null;

            // Возвращаем фиктивную позицию End()
            return End();
        }


        /// <summary>
        /// Печатает содержимое списка в консоль.
        /// Используется для отладки и визуальной проверки.
        /// </summary>
        public void PrintList()
        {
            try
            {
                if (_head == null)
                {
                    Console.WriteLine("Список пуст.");
                    return;
                }

                Node<T>? current = _head;
                int i = 1;
                while (current != null)
                {
                    Console.WriteLine($"{i}. {current.Data}");
                    current = current.Next;
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выводе списка: {ex.Message}");
            }
        }
        /// <summary>
        /// Проверяет, принадлежит ли указанная позиция текущему списку.
        /// Проходит по всем узлам и ищет совпадение.
        /// </summary>
        /// <param name="p">Позиция, которую необходимо проверить.</param>
        /// <returns>
        /// true — если позиция указывает на существующий узел в списке; 
        /// false — если позиция недействительна или не принадлежит списку.
        /// </returns>
        private bool ValidatePosition(IPosition p)
        {
            // Проверка типа
            if (p is not Position<T> pos)
                return false;

            Node<T>? target = pos.Node;
    
            // End() всегда допустимая позиция для этого списка
            if (target == null && p == _End)
                return true;

            // Проход по списку
            Node<T>? current = _head;
            while (current != null)
            {
                if (current == target)
                    return true;

                current = current.Next;
            }

            return false;
        }

    }
}
