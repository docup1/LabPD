namespace Lab1PD.Core.ManyToMany;

public class LinkNode : Base
{
    public override bool HasNext { get; } = true;
    public Base Student { get; set; }
    public Base Course { get; set; }
}