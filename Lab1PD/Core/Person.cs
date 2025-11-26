using System;

namespace Lab1PD.Core
{
    public class Person
    {
        private char[] name; // 20
        private char[] adr;  // 50

        // Конструктор с копированием массивов и ограничением длины
        public Person(char[] name, char[] adr)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (adr == null) throw new ArgumentNullException(nameof(adr));

            if (name.Length > 20)
                throw new ArgumentException("Имя не может быть длиннее 20 символов", nameof(name));
            if (adr.Length > 50)
                throw new ArgumentException("Адрес не может быть длиннее 50 символов", nameof(adr));

            // Копируем массивы в новые массивы фиксированной длины
            this.name = new char[20];
            Array.Copy(name, this.name, name.Length);

            this.adr = new char[50];
            Array.Copy(adr, this.adr, adr.Length);
        }

        // Переопределяем Equals для сравнения содержимого массивов
        public override bool Equals(object? obj)
        {
            if (obj is not Person other)
                return false;

            // Сравниваем name
            for (int i = 0; i < 20; i++)
            {
                if (this.name[i] != other.name[i])
                    return false;
            }

            // Сравниваем adr
            for (int i = 0; i < 50; i++)
            {
                if (this.adr[i] != other.adr[i])
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"Name: {new string(name).TrimEnd('\0')}, Address: {new string(adr).TrimEnd('\0')}";
        }
    }
}
