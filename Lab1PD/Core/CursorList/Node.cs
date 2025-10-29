namespace Lab1PD.Core.CursorList;

internal class Node<T>
{
    public T Data { get; set; }
    public int Next { get; set; } = -1;

    public Node(int i)
    {
        Next = i;
    }
}