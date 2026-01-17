namespace Lab1PD.Core.ManyToMany;

internal class LinkNode : IDefaultNode
{
    public bool HasNext { get; } = true;
    public IDefaultNode Student { get; set; }
    public IDefaultNode Course { get; set; }
    public IDefaultNode Next { get; set; }
}