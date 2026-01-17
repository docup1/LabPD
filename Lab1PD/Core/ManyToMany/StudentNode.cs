namespace Lab1PD.Core.ManyToMany;

internal class StudentNode : IDefaultNode
{
    public bool HasNext { get; } = false;
    public char[] Name = new char[NameSize + 1];
    public const int NameSize = 15;
    public LinkNode? Course { get; set; }

    public StudentNode(string name)
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