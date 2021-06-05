using System.Collections.Specialized;
using System.Extract;
using System.Runtime.InteropServices;

namespace System.Uniques
{
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 16)]  
    public unsafe struct Ussc : IFormattable, IComparable 
        , IComparable<Ussc>, IEquatable<Ussc>, IUnique       
    {
        private fixed byte bytes[16];              

        public ulong    UniqueKey
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ulong*)pbyte);

            }
            set
            {
                fixed (byte* b = bytes)
                    *((ulong*)b) = value;
            }
        }

        public ulong    UniqueSeed
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ulong*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((ulong*)(b + 8)) = value;
            }
        }

        public Ussc(ulong l)
        {
            fixed (byte* b = bytes)
            {
                *((ulong*)b) = l;
            }
        }
        public Ussc(string s)
        {          
            this.FromHexTetraChars(s.ToCharArray());    //RR
        }
        public Ussc(byte[] b)
        {
            if (b != null)
            {
                int l = b.Length;
                if (l > 16)
                    l = 16;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }

        public Ussc(ulong key, uint seed)
        {
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key;
                *((uint*)n + 8) = seed;
            }
        }
        public Ussc(byte[] key, ulong seed)
        {          
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((ulong*)n) = *((ulong*)s);
                *((ulong*)(n + 8)) = seed;            
            }
        }
        public Ussc(object key, ulong seed)
        {
            byte[] shah = key.UniqueBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((ulong*)n) = *((ulong*)s);
                *((ulong*)(n + 8)) = seed;
            }
        }
        public Ussc(object key)
        {
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key.UniqueKey64();
            }
        }

        public byte[] this[int offset]
        {
            get
            {
                if (offset != 0)                   
                {
                    int l = 16 - offset;
                    byte[] r = new byte[l];
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte + offset, l);
                    }
                    return r;
                }
                return null;
            }
            set
            {
                int l = value.Length;
                if (offset > 0 && l < 16)
                {
                    int count = 16 - offset;
                    if (l < count)
                        count = l;
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, count);
                    }
                }
                else
                {
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, 16);
                    }
                }               
            }
        }
        public byte[] this[int offset, int length]
        {
            get
            {
                if (offset < 16)
                {
                    if ((16 - offset) > length)
                    length = 16 - offset;
               
                    byte[] r = new byte[length];
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte + offset, length);
                    }
                    return r;
                }
                return null;
               
            }
            set
            {
                if (offset < 16)
                {
                    if ((16 - offset) > length)
                        length = 16 - offset;
                    if (value.Length < length)
                        length = value.Length;
                   
                    fixed (byte* rbyte = value)
                    fixed (byte* pbyte = bytes)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, length);
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
            byte[] r = new byte[16];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = bytes)
            {
                Extractor.CopyBlock(rbyte, pbyte, 16);
            }
            return r;
        }

        public byte[] GetUniqueBytes()
        {            
            byte[] kbytes = new byte[8];
            fixed (byte* b = bytes)
            fixed (byte* k = kbytes)
                *((ulong*)k) = *((ulong*)b);
            return kbytes;
        }

        public void SetUniqueKey(ulong value)
        {
            UniqueKey = value;
        }

        public ulong GetUniqueKey()
        {
            return UniqueKey;
        }

        public void SetUniqueSeed(ulong seed)
        {
            UniqueSeed = seed;
        }

        public ulong GetUniqueSeed()
        {
            return UniqueSeed;
        }

        public bool IsNotEmpty
        {
            get { return (UniqueKey != 0); }
        }

        public bool IsEmpty
        {
            get { return (UniqueKey == 0); }
        }      

        public override int GetHashCode()
        {
            fixed (byte* pbyte = &this[0,8].BitAggregate64to32()[0])
                return *((int*)pbyte);
        }      

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is Ussc))
                throw new Exception();

            return (int)(UniqueKey - ((Ussc)value).UniqueKey);
        }

        public int CompareTo(Ussc g)
        {
            return (int)(UniqueKey - g.UniqueKey);
        }

        public int CompareTo(IUnique g)
        {
            return (int)(UniqueKey - g.UniqueKey());
        }

        public bool Equals(ulong g)
        {
            return (UniqueKey == g);
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;
            if ((value is string))
                return new Ussc(value.ToString()).UniqueKey == UniqueKey;

            return (UniqueKey == ((Ussc)value).UniqueKey);
        }

        public bool Equals(Ussc g)
        {
            return (UniqueKey == g.UniqueKey);
        }
        public bool Equals(IUnique g)
        {
            return (UniqueKey == g.UniqueKey());
        }

        public override String ToString()
        {           
            return new string(this.ToHexTetraChars());  
        }

        public String ToString(String format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {         
            return new string(this.ToHexTetraChars());  //RR
        }

        public static bool operator ==(Ussc a, Ussc b)
        {
            return (a.UniqueKey == b.UniqueKey);
        }

        public static bool operator !=(Ussc a, Ussc b)
        {
            return (a.UniqueKey != b.UniqueKey);
        }

        public static explicit operator Ussc(String s)
        {
            return new Ussc(s);
        }
        public static implicit operator String(Ussc s)
        {
            return s.ToString();
        }

        public static explicit operator Ussc(byte[] l)
        {
            return new Ussc(l);
        }
        public static implicit operator byte[](Ussc s)
        {
            return s.GetBytes();
        }

        public static Ussc Empty
        {
            get { return new Ussc(new byte[24]); }
        }      

        IUnique IUnique.Empty
        {
            get
            {
                return new Ussc();
            }
        }       

        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[16];
            ulong pchblock;  
            int pchlength = 16;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 2; j++)
            {
                fixed (byte* pbyte = bytes)
                {
                    pchblock = *((ulong*)(pbyte + (j * 6)));
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

            for (int j = 0; j < 2; j++)
            {
                pchblock = 0x00L;
                blocklength = Math.Min(8, Math.Max(0, pchlength - 8 * j));        //required if trimmed zeros, length < 16
                idx = Math.Min(pchlength, 8*(j+1)) - 1;                           //required if trimmed zeros, length < 16

                for (int i = 0; i < blocklength; i++)     //8 chars per block, each 6 bits
                {
                    pchbyte = (pchchar[idx]).ToHexTetraByte();
                    pchblock = pchblock << 6;
                    pchblock = pchblock | (pchbyte & 0x3fUL);
                    idx--;
                }
                fixed (byte* pbyte = bytes)
                {
                    if (j == 1) //ostatnie nalozenie - block3 przekracza o 2 bajty rozmiar bytes!!!! tych 2 bajty sa 0, ale uniknac ewentualne wejscia w pamiec poza bytes
                    {
                        pchblock_short = (ushort)(pchblock & 0x00ffffUL);
                        pchblock_int = (uint)(pchblock >> 8);
                        *((ulong*)&pbyte[6]) = pchblock_short;
                        *((ulong*)&pbyte[8]) = pchblock_int;
                        break;
                    }
                    *((ulong*)&pbyte[j * 6]) = pchblock;

                }
            }                                    
        }

        public bool EqualsContent(Ussc g)
        {
            ulong pchblockA, pchblockB;
            bool result;

            if (g == null) return false;
            fixed (byte* pbyte = bytes)
            {
                pchblockA = *((ulong*)&pbyte[0]);
                pchblockB = *((uint*)&pbyte[8]);
            }

            result = (pchblockA == *((ulong*)&g.bytes[0]))
            && (pchblockB == *((uint*)&g.bytes[8]));
            
            
            return result;
        }


    }
}
