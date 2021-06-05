/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.Ursn.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Uniques
{
    using System.Extract;
    using System.Runtime.InteropServices;

    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public unsafe struct Ursn : IFormattable, IComparable
        , IComparable<Ursn>, IEquatable<Ursn>, IUnique
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        private byte[] bytes;

        public ulong UniqueKey
        {
            get
            {
                if (IsNull) return 0;
                fixed (byte* pbyte = bytes)
                    return *((ulong*)pbyte);

            }
            set
            {
                fixed (byte* b = SureBytes)
                    *((ulong*)b) = value;
            }
        }

        public ulong KeyBlockX
        {
            get
            {
                if (IsNull) return 0;
                fixed (byte* pbyte = &bytes[8])
                    return *((ulong*)pbyte);
            }
            set
            {
                fixed (byte* b = &SureBytes[8])
                    *((ulong*)b) = value;
            }
        }

        public ulong KeyBlockY
        {
            get
            {
                if (IsNull) return 0;
                fixed (byte* pbyte = &bytes[16])
                    return *((ulong*)pbyte);
            }
            set
            {
                fixed (byte* b = &SureBytes[16])
                    *((ulong*)b) = value;
            }
        }

        public Ursn(ulong l)
        {
            bytes = new byte[24];
            fixed (byte* b = bytes)
            {
                *((ulong*)b) = l;
            }
        }
        public Ursn(string s)
        {
            bytes = new byte[24];
            this.FromHexTetraChars(s.ToCharArray());    //RR
        }
        public Ursn(byte[] b)
        {
            bytes = new byte[24];
            if (b != null)
            {
                int l = b.Length;
                if (l > 24)
                    l = 24;
                b.CopyBlock(bytes, (uint)l);
            }

        }

        public Ursn(ulong x, ulong y, ulong z)
        {
            bytes = new byte[24];

            fixed (byte* n = bytes)
            {
                *((ulong*)n) = x;
                *((ulong*)&n[8]) = y;
                *((ulong*)&n[16]) = z;
            }
        }
        public Ursn(byte[] x, byte[] y, byte[] z)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                fixed (byte* s = x)
                    *((ulong*)n) = *((ulong*)s);
                fixed (byte* s = y)
                    *((ulong*)(n + 8)) = *((ulong*)s);
                fixed (byte* s = z)
                    *((ulong*)(n + 16)) = *((ulong*)s);
            }
        }
        public Ursn(object x, object[] y, object[] z)
        {
            bytes = new byte[24];

            fixed (byte* n = bytes)
            {
                fixed (byte* s = x.UniqueBytes64())
                    *((ulong*)n) = *((ulong*)s);
                fixed (byte* s = y.UniqueBytes64())
                    *((ulong*)(n + 12)) = *((ulong*)s);
                fixed (byte* s = z.UniqueBytes64())
                    *((ulong*)(n + 16)) = *((ulong*)s);
            }
        }

        public byte[] this[int offset]
        {
            get
            {
                if (offset != 0)
                {
                    byte[] r = new byte[24 - offset];
                    fixed (byte* pbyte = &NotSureBytes[offset])
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte, (uint)(24 - offset));
                    }
                    return r;
                }
                return NotSureBytes;
            }
            set
            {
                int l = value.Length;
                if (offset != 0 || l < 24)
                {
                    int count = 24 - offset;
                    if (l < count)
                        count = l;
                    value.CopyBlock(SureBytes, (uint)offset, (uint)count);
                }
                else
                {
                    value.CopyBlock(SureBytes, 0, 24);
                }
            }
        }
        public byte[] this[int offset, int length]
        {
            get
            {
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;

                    byte[] r = new byte[length];
                    fixed (byte* pbyte = &NotSureBytes[offset])
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte, (uint)length);
                    }
                    return r;
                }
                return null;

            }
            set
            {
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;
                    if (value.Length < length)
                        length = value.Length;

                    byte[] r = new byte[length];
                    fixed (byte* pbyte = value)
                    fixed (byte* rbyte = &SureBytes[offset])
                    {
                        Extractor.CopyBlock(rbyte, pbyte, (uint)length);
                    }
                }
            }
        }

        public void SetBytes(byte[] value, int offset)
        {
            this[offset] = value;
        }

        public byte[] GetBytes(int offset, int length)
        {
            return this[offset, length];
        }

        public byte[] GetBytes()
        {
            return SureBytes;
        }

        public byte[] GetUniqueBytes()
        {
            byte[] kbytes = new byte[8];
            fixed (byte* b = SureBytes)
            fixed (byte* k = kbytes)
                *((ulong*)k) = *((ulong*)b);
            return kbytes;
        }

        public bool IsNull
        {
            get
            {
                if (bytes == null)
                    return true;
                return false;
            }
            set
            {
                if (value) bytes = null;
            }
        }

        public bool IsNotEmpty
        {
            get { return (!IsNull && UniqueKey != 0); }
        }

        public bool IsEmpty
        {
            get { return (IsNull || UniqueKey == 0); }
        }

        public byte[] SureBytes
        {
            get => (bytes == null) ? bytes = new byte[24] : bytes;
        }

        public byte[] NotSureBytes
        {
            get => (bytes == null) ? new byte[24] : bytes;
        }

        public override int GetHashCode()
        {
            fixed (byte* pbyte = &this[0, 8].BitAggregate64to32()[0])
                return *((int*)pbyte);
        }

        public void SetUniqueKey(ulong value)
        {
            UniqueKey = value;
        }

        public ulong GetUniqueKey()
        {
            return UniqueKey;
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is Ursn))
                throw new Exception();

            return (int)(UniqueKey - ((Ursn)value).UniqueKey);
        }

        public int CompareTo(Ursn g)
        {
            return (int)(UniqueKey - g.UniqueKey);
        }

        public int CompareTo(IUnique g)
        {
            return (int)(UniqueKey - g.UniqueKey);
        }

        public bool Equals(ulong g)
        {
            return (UniqueKey == g);
        }

        public override bool Equals(object value)
        {
            if (value == null || bytes == null)
                return false;
            if ((value is string))
                return new Ursn(value.ToString()).UniqueKey == UniqueKey;

            return (UniqueKey == ((Ursn)value).UniqueKey);
        }

        public bool Equals(Ursn g)
        {
            return (UniqueKey == g.UniqueKey);
        }
        public bool Equals(IUnique g)
        {
            return (UniqueKey == g.UniqueKey());
        }

        public override String ToString()
        {
            if (bytes == null)
                bytes = new byte[24];
            return new string(this.ToHexTetraChars());
        }

        public String ToString(String format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (bytes == null)
                bytes = new byte[24];
            return new string(this.ToHexTetraChars());  //RR
        }

        public static bool operator ==(Ursn a, Ursn b)
        {
            return (a.UniqueKey == b.UniqueKey);
        }

        public static bool operator !=(Ursn a, Ursn b)
        {
            return (a.UniqueKey != b.UniqueKey);
        }

        public static explicit operator Ursn(String s)
        {
            return new Ursn(s);
        }
        public static implicit operator String(Ursn s)
        {
            return s.ToString();
        }


        public static explicit operator Ursn(byte[] l)
        {
            return new Ursn(l);
        }
        public static implicit operator byte[] (Ursn s)
        {
            return s.GetBytes();
        }

        public static Ursn Empty
        {
            get { return new Ursn(new byte[24]); }
        }

        IUnique IUnique.Empty
        {
            get
            {
                return new Ursn(new byte[24]);
            }
        }

        public ulong UniqueSeed { get => KeyBlockX; set => KeyBlockX = value; }

        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[32];
            ulong pchblock;
            int pchlength = 32;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 4; j++)
            {
                fixed (byte* pbyte = &bytes[j * 6])
                {
                    pchblock = *((ulong*)pbyte);
                }
                pchblock = pchblock & 0x0000ffffffffffffL;  //each block has 6 bytes
                for (int i = 0; i < 8; i++)
                {
                    pchbyte = (byte)(pchblock & 0x3fL);
                    pchchar[idx] = (pchbyte).ToHexTetraChar();
                    idx++;
                    pchblock = pchblock >> 6;
                    if (pchbyte != 0x00) pchlength = idx;
                }
            }

            char[] pchchartrim = new char[pchlength];
            Array.Copy(pchchar, 0, pchchartrim, 0, pchlength);

            return pchchartrim;
        }

        public void FromHexTetraChars(char[] pchchar)
        {
            int pchlength = pchchar.Length;
            int idx = 0;
            byte pchbyte;
            ulong pchblock = 0;
            int blocklength = 8;
            uint pchblock_int;
            ushort pchblock_short;

            for (int j = 0; j < 4; j++)
            {
                pchblock = 0x00L;
                blocklength = Math.Min(8, Math.Max(0, pchlength - 8 * j));        //required if trimmed zeros, length < 32
                idx = Math.Min(pchlength, 8 * (j + 1)) - 1;                           //required if trimmed zeros, length <32

                for (int i = 0; i < blocklength; i++)     //8 chars per block, each 6 bits
                {
                    pchbyte = (pchchar[idx]).ToHexTetraByte();
                    pchblock = pchblock << 6;
                    pchblock = pchblock | (pchbyte & 0x3fUL);
                    idx--;
                }
                fixed (byte* pbyte = bytes)
                {
                    if (j == 3) //ostatnie nalozenie - block3 przekracza o 2 bajty rozmiar bytes!!!! tych 2 bajty sa 0, ale uniknac ewentualne wejscia w pamiec poza bytes
                    {
                        pchblock_short = (ushort)(pchblock & 0x00ffffUL);
                        pchblock_int = (uint)(pchblock >> 16);
                        *((ulong*)&pbyte[18]) = pchblock_short;
                        *((ulong*)&pbyte[20]) = pchblock_int;
                        break;
                    }
                    *((ulong*)&pbyte[j * 6]) = pchblock;

                }
            }
        }

        public bool EqualsContent(Ursn g)
        {
            if (g == null) return false;
            fixed (byte* gbyte = g.bytes)
            fixed (byte* pbyte = bytes)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (*((ulong*)&pbyte[i * 8]) != *((ulong*)&pbyte[i * 8]))
                        return false;
                }
            }
            return true;
        }

        public void SetUniqueSeed(ulong seed)
        {
            KeyBlockX = seed;
        }

        public ulong GetUniqueSeed()
        {
            return KeyBlockX;
        }
    }
}
