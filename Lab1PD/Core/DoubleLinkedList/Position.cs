namespace Lab1PD.Core.DoubleLinkedList;

public class Position<T> : IPosition
{
    public Node<T> Node { get;  init; }
    
    public Position(Node<T> node)
    {
        Node = node;
    }
}