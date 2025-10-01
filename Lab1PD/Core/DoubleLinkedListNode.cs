namespace Lab1PD.Core;

internal class DoubleLinkedListNode<T>
{
    public T Data;
    public DoubleLinkedListNode<T>? Next;
    public DoubleLinkedListNode<T>? Prev;
    public DoubleLinkedListNode(T data)
    {
        Data = data;
        Next = null;
        Prev = null;
    }
}