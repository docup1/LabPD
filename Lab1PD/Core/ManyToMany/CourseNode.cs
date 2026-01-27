namespace Lab1PD.Core.ManyToMany;

public class CourseNode : Base
{
    public char[] Name = new char[NameSize + 1];
    public const int NameSize = 23;
    public LinkNode? Student { get; set; }
    
    public CourseNode(char[] name)
    {
        int length = int.Min(name.Length, NameSize);
        
        for (int i = 0; i < length; ++i)
        {
            Name[i] = name[i];
        }

        Name[length] = '\0';
    }

    public override string ToString()
    {
        return string.Join("", Name);
    }
}