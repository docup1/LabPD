namespace Lab1PD.Core.CursorList;

public class Position : IPosition
{
    public int N { get; }

    public Position(int n)
    {
        N = n;
    }
}