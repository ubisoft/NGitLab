using System;
using Newtonsoft.Json;
using NGitLab.Json;

namespace NGitLab {
    /// <summary>
    ///     Sha1 hash value representation.
    /// </summary>
    [JsonConverter(typeof(Sha1Converter))]
    public struct Sha1 {
        readonly ulong p1; // 8
        readonly ulong p2; // 16
        readonly uint p3; // 20

        [JsonConstructor]
        public Sha1(string value) {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Cannot be null or empty", "value");

            if (value.Length != 40)
                throw new ArgumentException("Sha1 is 20 bytes long, which is 40 chars");

            var index = 0;

            p1 = GetLong(value, ref index);
            p2 = GetLong(value, ref index);
            p3 = GetInt(value, ref index);
        }

        public bool Equals(Sha1 other) {
            return p1 == other.p1 && p2 == other.p2 && p3 == other.p3;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Sha1 && Equals((Sha1)obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = p1.GetHashCode();
                hashCode = (hashCode * 397) ^ p2.GetHashCode();
                hashCode = (hashCode * 397) ^ p3.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() {
            return p1.ToString("X16") +
                   p2.ToString("X16") +
                   p3.ToString("X8");
        }

        static ulong GetLong(string value, ref int i) {
            unchecked {
                return ((ulong)GetInt(value, ref i) << 32) + GetInt(value, ref i);
            }
        }

        static uint GetInt(string value, ref int i) {
            unchecked {
                return (GetByte(value[i++], value[i++]) << 24) +
                       (GetByte(value[i++], value[i++]) << 16) +
                       (GetByte(value[i++], value[i++]) << 8) +
                       GetByte(value[i++], value[i++]);
            }
        }

        static uint GetByte(char c1, char c2) {
            unchecked {
                return (GetHexVal(c1) << 4) + GetHexVal(c2);
            }
        }

        static uint GetHexVal(char hex) {
            var val = (int)hex;

            //lower or upper
            return (uint)(val - (val < 58 ? 48 : (val < 97 ? 55 : 87)));
        }
    }
}