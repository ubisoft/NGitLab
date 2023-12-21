using System;

namespace NGitLab;

/// <summary>
/// Sha1 hash value representation.
/// </summary>
public readonly struct Sha1 : IEquatable<Sha1>
{
    private readonly ulong _p1; // 8
    private readonly ulong _p2; // 16
    private readonly uint _p3; // 20

    public Sha1(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Cannot be null or empty", nameof(value));

        if (value.Length != 40)
            throw new ArgumentException("Sha1 is 20 bytes long, which is 40 chars");

        var index = 0;

        _p1 = GetLong(value, ref index);
        _p2 = GetLong(value, ref index);
        _p3 = GetInt(value, ref index);
    }

    public static bool operator ==(Sha1 left, Sha1 right) => left.Equals(right);

    public static bool operator !=(Sha1 left, Sha1 right) => !(left == right);

    public bool Equals(Sha1 other)
    {
        return _p1 == other._p1 && _p2 == other._p2 && _p3 == other._p3;
    }

    public override bool Equals(object obj)
    {
        return obj is Sha1 sha1 && Equals(sha1);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = _p1.GetHashCode();
            hashCode = (hashCode * 397) ^ _p2.GetHashCode();
            hashCode = (hashCode * 397) ^ _p3.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString()
    {
        return _p1.ToString("X16") +
               _p2.ToString("X16") +
               _p3.ToString("X8");
    }

    private static ulong GetLong(string value, ref int i)
    {
        unchecked
        {
            return ((ulong)GetInt(value, ref i) << 32) + GetInt(value, ref i);
        }
    }

    private static uint GetInt(string value, ref int i)
    {
        unchecked
        {
            return (GetByte(value[i++], value[i++]) << 24) +
                   (GetByte(value[i++], value[i++]) << 16) +
                   (GetByte(value[i++], value[i++]) << 8) +
                   GetByte(value[i++], value[i++]);
        }
    }

    private static uint GetByte(char c1, char c2)
    {
        unchecked
        {
            return (GetHexVal(c1) << 4) + GetHexVal(c2);
        }
    }

    private static uint GetHexVal(char hex)
    {
        var val = (int)hex;

        // lower or upper
        return (uint)(val - (val < 58 ? 48 : (val < 97 ? 55 : 87)));
    }
}
