namespace Aoc_2021_csharp;

public class P3
{
    public int X;
    public int Y;
    public int Z;

    private P3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public P3 Clone() => P3.C(X, Y, Z);

    public override bool Equals(object obj) => obj is P3 p && Equals(p);
    private bool Equals(P3 other) => X == other.X && Y == other.Y && Z == other.Z;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"(x={X} y={Y} z={Z})";

    public static P3 C(int x, int y, int z) => new(x, y, z);

    public static bool operator ==(P3 a, P3 b) => Equals(a, b);
    public static bool operator !=(P3 a, P3 b) => !(a == b);
}