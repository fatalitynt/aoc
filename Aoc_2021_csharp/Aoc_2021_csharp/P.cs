namespace Aoc_2021_csharp;

public class P
{
    public readonly int X;
    public readonly int Y;
    public P Up => C(X, Y - 1);
    public P Down => C(X, Y + 1);
    public P Left => C(X - 1, Y);
    public P Right => C(X + 1, Y);

    private P(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj) => obj is P p && Equals(p);

    private bool Equals(P other) => X == other.X && Y == other.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"(y={Y} x={X})";

    public static P C(int x, int y) => new(x, y);

    public IEnumerable<P> Get4ValidNeighbors<T>(T[][] a)
    {
        if (Y > 0) yield return Up;
        if (Y + 1 < a.Length) yield return Down;
        if (X > 0) yield return Left;
        if (X + 1 < a[0].Length) yield return Right;
    }

    public IEnumerable<P> Get8ValidNeighbors<T>(T[][] a)
    {
        if (Y > 0) yield return Up;
        if (Y > 0 && X + 1 < a[0].Length) yield return Up.Right;
        if (Y > 0 && X > 0) yield return Up.Left;

        if (Y + 1 < a.Length) yield return Down;
        if (Y + 1 < a.Length && X + 1 < a[0].Length) yield return Down.Right;
        if (Y + 1 < a.Length && X > 0) yield return Down.Left;

        if (X + 1 < a[0].Length) yield return Right;
        if (X > 0) yield return Left;
    }

    public IEnumerable<P> Get4Neighbors()
    {
        yield return Up;
        yield return Down;
        yield return Left;
        yield return Right;
    }

    public bool IsInside<T>(T[][] a) =>
        a.Length > 0 && Y >= 0 && Y < a.Length && X >= 0 && X < a[0].Length;

    public bool TryRead<T>(T[][] a, out T result)
    {
        result = default;
        if (!IsInside(a)) return false;
        result = a[Y][X];
        return true;
    }

    public void Write<T>(T[][] a, T val)
    {
        if (IsInside(a)) a[Y][X] = val;
        else throw new Exception($"Point {this} is outside of 2d array");
    }

    public void Update<T>(T[][] a, Func<T, T> update)
    {
        if (IsInside(a)) a[Y][X] = update(a[Y][X]);
        else throw new Exception($"Point {this} is outside of 2d array");
    }

    public T Read<T>(T[][] a)
    {
        if (IsInside(a)) return a[Y][X];
        throw new Exception($"Point {this} is outside of 2d array");
    }

    public static IEnumerable<P> GetAllPoints<T>(T[][] a)
    {
        if (a.Length == 0) yield break;
        for (var y = 0; y < a.Length; y++)
        for (var x = 0; x < a[0].Length; x++)
            yield return C(x, y);
    }

    public long Dist(P oth) => Math.Abs((long)X - oth.X) + Math.Abs((long)Y - oth.Y);

    public static bool operator ==(P a, P b) => Equals(a, b);
    public static bool operator !=(P a, P b) => !(a == b);
}