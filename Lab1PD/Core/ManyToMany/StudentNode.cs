namespace Lab1PD.Core.ManyToMany;

public class StudentNode : Base
{
    public char[] Name = new char[NameSize + 1];
    public const int NameSize = 15;
    public LinkNode? Course { get; set; }

    public StudentNode(char[] name)
    {
        int length = int.Min(name.Length, NameSize);
        
        for (int i = 0; i < length; ++i)
        {
            Name[i] = name[i];
        }

        Name[NameSize] = '\0';
    }
    
    public override string ToString()
    {
        return string.Join("", Name);
    }
}