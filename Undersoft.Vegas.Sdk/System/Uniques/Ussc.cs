using System.Collections.Specialized;
using System.Extract;
using System.Runtime.InteropServices;

namespace System.Uniques
{
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 12)]  
    public unsafe struct Ussc : IFormattable, IComparable 
        , IComparable<Ussc>, IEquatable<Ussc>, IUnique       
    {
        private fixed byte bytes[12];              

        public long    KeyBlock
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((long*)pbyte);

            }
            set
            {
                fixed (byte* b = bytes)
                    *((long*)b) = value;
            }
        }

        public uint    SeedBlock
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((uint*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((uint*)(b + 8)) = value;
            }
        }

        public Ussc(long l)
        {
            fixed (byte* b = bytes)
            {
                *((long*)b) = l;
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
                if (l > 12)
                    l = 12;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }

        public Ussc(long key, uint seed)
        {
            fixed (byte* n = bytes)
            {
                *((long*)n) = key;
                *((uint*)n + 8) = seed;
            }
        }
        public Ussc(byte[] key, uint seed)
        {          
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((long*)n) = *((long*)s);
                *((uint*)(n + 8)) = seed;            
            }
        }
        public Ussc(object key, uint seed)
        {
            byte[] shah = key.GetHashBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((long*)n) = *((long*)s);
                *((uint*)(n + 8)) = seed;
            }
        }
        public Ussc(object key)
        {
            fixed (byte* n = bytes)
            {
                *((long*)n) = key.GetHashKey64();
            }
        }

        public byte[] this[int offset]
        {
            get
            {
                if (offset != 0)                   
                {
                    int l = 12 - offset;
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
                if (offset > 0 && l < 12)
                {
                    int count = 12 - offset;
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
                        Extractor.CopyBlock(pbyte, rbyte, 12);
                    }
                }               
            }
        }
        public byte[] this[int offset, int length]
        {
            get
            {
                if (offset < 12)
                {
                    if ((12 - offset) > length)
                    length = 12 - offset;
               
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
                if (offset < 12)
                {
                    if ((12 - offset) > length)
                        length = 12 - offset;
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
            byte[] r = new byte[12];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = bytes)
            {
                Extractor.CopyBlock(rbyte, pbyte, 12);
            }
            return r;
        }

        public byte[] GetKeyBytes()
        {            
            byte[] kbytes = new byte[8];
            fixed (byte* b = bytes)
            fixed (byte* k = kbytes)
                *((long*)k) = *((long*)b);
            return kbytes;
        }

        public void SetHashKey(long value)
        {
            KeyBlock = value;
        }

        public long GetHashKey()
        {
            return KeyBlock;
        }

        public void SetHashSeed(uint seed)
        {
            SeedBlock = seed;
        }

        public uint GetHashSeed()
        {
            return SeedBlock;
        }

        public bool IsNotEmpty
        {
            get { return (KeyBlock != 0); }
        }

        public bool IsEmpty
        {
            get { return (KeyBlock == 0); }
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

            return (int)(KeyBlock - ((Ussc)value).KeyBlock);
        }

        public int CompareTo(Ussc g)
        {
            return (int)(KeyBlock - g.KeyBlock);
        }

        public int CompareTo(IUnique g)
        {
            return (int)(GetHashKey() - g.GetHashKey());
        }

        public bool Equals(long g)
        {
            return (KeyBlock == g);
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;
            if ((value is string))
                return new Ussc(value.ToString()).KeyBlock == KeyBlock;

            return (KeyBlock == ((Ussc)value).KeyBlock);
        }

        public bool Equals(Ussc g)
        {
            return (KeyBlock == g.KeyBlock);
        }
        public bool Equals(IUnique g)
        {
            return (GetHashKey() == g.GetHashKey());
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
            return (a.KeyBlock == b.KeyBlock);
        }

        public static bool operator !=(Ussc a, Ussc b)
        {
            return (a.KeyBlock != b.KeyBlock);
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
            long pchblock;  
            int pchlength = 16;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 2; j++)
            {
                fixed (byte* pbyte = bytes)
                {
                    pchblock = *((long*)(pbyte + (j * 6)));
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
            long pchblock = 0;
            int blocklength = 8;
            int pchblock_int;
            short pchblock_short;

            for (int j = 0; j < 2; j++)
            {
                pchblock = 0x00L;
                blocklength = Math.Min(8, Math.Max(0, pchlength - 8 * j));        //required if trimmed zeros, length < 16
                idx = Math.Min(pchlength, 8*(j+1)) - 1;                           //required if trimmed zeros, length < 16

                for (int i = 0; i < blocklength; i++)     //8 chars per block, each 6 bits
                {
                    pchbyte = (pchchar[idx]).ToHexTetraByte();
                    pchblock = pchblock << 6;
                    pchblock = pchblock | (pchbyte & 0x3fL);
                    idx--;
                }
                fixed (byte* pbyte = bytes)
                {
                    if (j == 3) //ostatnie nalozenie - block3 przekracza o 2 bajty rozmiar bytes!!!! tych 2 bajty sa 0, ale uniknac ewentualne wejscia w pamiec poza bytes
                    {
                        pchblock_short = (short)(pchblock & 0x00ffffL);
                        pchblock_int = (int)(pchblock >> 8);
                        *((long*)&pbyte[6]) = pchblock_short;
                        *((long*)&pbyte[8]) = pchblock_int;
                        break;
                    }
                    *((long*)&pbyte[j * 6]) = pchblock;

                }
            }                                    
        }

        public bool EqualsContent(Ussc g)
        {
            long pchblockA, pchblockB;
            bool result;

            if (g == null) return false;
            fixed (byte* pbyte = bytes)
            {
                pchblockA = *((long*)&pbyte[0]);
                pchblockB = *((uint*)&pbyte[8]);
            }

            result = (pchblockA == *((long*)&g.bytes[0]))
            && (pchblockB == *((uint*)&g.bytes[8]));
            
            
            return result;
        }


    }
}
