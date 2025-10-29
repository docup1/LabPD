namespace Lab1PD.Core.CursorList;

public class Position
{
    public int N { get; }

    public Position(int n)
    {
        N = n;
    }

    public bool Equals(Position p) => N == p.N;
}