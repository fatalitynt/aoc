namespace Aoc_2021_csharp;

public class P3
{
    public readonly int X;
    public readonly int Y;
    public readonly int Z;

    private P3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals(object obj) => obj is P3 p && Equals(p);
    private bool Equals(P3 other) => X == other.X && Y == other.Y && Z == other.Z;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"(y={Y} x={X} z={Z})";

    public static P3 C(int x, int y, int z) => new(x, y, z);

    public static bool operator ==(P3 a, P3 b) => Equals(a, b);
    public static bool operator !=(P3 a, P3 b) => !(a == b);
}