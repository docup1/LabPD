
namespace Lab1PD.Core
{
    public class Person
    {
        private const int NameSize = 20;
        private const int AdrSize = 50;
        
        private char[] _name; 
        private char[] _adr; 

        // Конструктор с копированием массивов и ограничением длины
        public Person(char[] name, char[] adr)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (adr == null) throw new ArgumentNullException(nameof(adr));

            if (name.Length > NameSize)
                throw new ArgumentException("Имя не может быть длиннее 20 символов", nameof(name));
            if (adr.Length > AdrSize)
                throw new ArgumentException("Адрес не может быть длиннее 50 символов", nameof(adr));

            // Копируем массив name вручную
            _name = new char[NameSize];
            for (int i = 0; i < name.Length && i < NameSize; i++)
            {
                _name[i] = name[i];
            }
            _adr = new char[AdrSize];
            for (int i = 0; i < adr.Length && i < AdrSize; i++)
            {
                _adr[i] = adr[i];
            }
        }

        // Переопределяем Equals для сравнения содержимого массивов
        public override bool Equals(object? obj)
        {
            if (obj is not Person other)
                return false;

            // Сравниваем name
            for (int i = 0; i < NameSize; i++)
            {
                if (_name[i] != other._name[i])
                    return false;
            }

            // Сравниваем adr
            for (int i = 0; i < AdrSize; i++)
            {
                if (_adr[i] != other._adr[i])
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"Name: {new string(_name).TrimEnd('\0')}, Address: {new string(_adr).TrimEnd('\0')}";
        }
    }
}
