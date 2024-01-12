namespace Aoc_2021_csharp;

public class P4
{
    public readonly int A;
    public readonly int B;
    public readonly int C;
    public readonly int D;

    private P4(int a, int b, int c, int d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public override bool Equals(object obj) => obj is P4 p && Equals(p);
    private bool Equals(P4 o) => A == o.A && B == o.B && C == o.C && D == o.D;

    public override int GetHashCode() => HashCode.Combine(A, B, C, D);

    public override string ToString() => $"(a={A}, b={B}, c={C}, d={D})";

    public static P4 Cr(int a, int b, int c, int d) => new(a, b, c, d);

    public static bool operator ==(P4 a, P4 b) => Equals(a, b);
    public static bool operator !=(P4 a, P4 b) => !(a == b);
}