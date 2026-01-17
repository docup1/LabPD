namespace Lab1PD.Core
{
    public class Person
    {
        private const int NameSize = 20;
        private const int AdrSize = 50;
        private const char TerminalElement = '\0';
        
        private readonly char[] _name;
        private readonly char[] _adr;

        public Person(char[] name, char[] adr)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (adr == null) throw new ArgumentNullException(nameof(adr));

            int nameLen = name.Length;
            int adrLen = adr.Length;
            
            if (nameLen > NameSize)
                throw new ArgumentException("Имя не может быть длиннее 20 символов", nameof(name));

            if (adrLen > AdrSize)
                throw new ArgumentException("Адрес не может быть длиннее 50 символов", nameof(adr));

            _name = new char[nameLen + 1];
            _adr = new char[adrLen + 1];

            // Копируем имя
            Array.Copy(name, _name, nameLen);
            _name[nameLen] = TerminalElement;

            // Копируем адрес
            Array.Copy(adr, _adr, adrLen);
            _adr[adrLen] = TerminalElement;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Person other)
                return false;

            // Сначала проверяем размеры массивов
            if (_name.Length != other._name.Length || _adr.Length != other._adr.Length)
                return false;

            // Сравниваем содержимое имени
            for (int i = 0; i < _name.Length; i++)
                if (_name[i] != other._name[i])
                    return false;

            // Сравниваем содержимое адреса
            for (int i = 0; i < _adr.Length; i++)
                if (_adr[i] != other._adr[i])
                    return false;

            return true;
        }

        public override string ToString()
        {
            string nameStr = new string(_name).TrimEnd(TerminalElement);
            string adrStr  = new string(_adr).TrimEnd(TerminalElement);
            return $"Name: {nameStr}, Address: {adrStr}";
        }
    }
}
