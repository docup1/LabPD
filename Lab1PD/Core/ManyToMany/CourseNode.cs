namespace Lab1PD.Core.ManyToMany;

internal class CourseNode : IDefaultNode
{
    public bool HasNext { get; } = false;
    public char[] Name = new char[NameSize + 1];
    public const int NameSize = 23;
    public LinkNode? Student { get; set; }
    
    public CourseNode(string name)
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