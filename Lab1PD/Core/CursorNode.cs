namespace Lab1PD.Core;

internal struct CursorNode<T>
{
    public T Data;
    public int Next;
    public int Prev;
    public bool Used;
}