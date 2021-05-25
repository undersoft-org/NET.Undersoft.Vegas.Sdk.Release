using System.Collections.Specialized;
using System.Extract;
using System.Runtime.InteropServices;

namespace System.Uniques
{
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 24)]  
    public unsafe struct Ussn : IFormattable, IComparable 
        , IComparable<Ussn>, IEquatable<Ussn>, IUnique, ISerialNumber       
    {
        private fixed byte bytes[24];              

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

        public ushort  BlockZ
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ushort*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((ushort*)(b + 8)) = value;
            }
        }
        public ushort  BlockY
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ushort*)(pbyte + 10));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((ushort*)(b + 10)) = value;
            }
        }
        public ushort  BlockX
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ushort*)(pbyte + 12));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((ushort*)(b + 12)) = value;
            }
        }

        public ushort  FlagsBlock
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ushort*)(pbyte + 14));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((ushort*)(b + 14)) = value;
            }
        }

        public long    TimeBlock
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((long*)(pbyte + 16));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((long*)(b + 16)) = value;
            }
        }

        public Ussn(long l)
        {
            fixed (byte* b = bytes)
            {
                *((long*)b) = l;
                *((long*)(b + 16)) = DateTime.Now.ToBinary();
            }
        }
        public Ussn(string s)
        {          
            this.FromHexTetraChars(s.ToCharArray());    //RR
        }
        public Ussn(byte[] b)
        {
            if (b != null)
            {
                int l = b.Length;
                if (l > 24)
                    l = 24;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }

        public Ussn(long key, uint seed)
        {
            fixed (byte* n = bytes)
            {
                *((long*)n) = key;
                *((uint*)n + 8) = seed;
            }
        }
        public Ussn(byte[] key, uint seed)
        {
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((long*)n) = *((long*)s);
                *((uint*)(n + 8)) = seed;
            }
        }
        public Ussn(object key, uint seed)
        {
            byte[] shah = key.GetHashBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((long*)n) = *((long*)s);
                *((uint*)(n + 8)) = seed;
            }
        }

        public Ussn(long key, short z, short y, short x, short flags, long time)
        {
            fixed (byte* n = bytes)
            {
                *((long*)n) = key;
                *((short*)&n[8]) = z;
                *((short*)&n[10]) = y;
                *((short*)&n[12]) = x;
                *((short*)&n[14]) = flags;
                *((long*)&n[16]) = time;
            }
        }
        public Ussn(byte[] key, short z, short y, short x, short flags, long time)
        {          
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((long*)n) = *((long*)s);
                *((short*)(n + 8)) = z;            
                *((short*)(n + 10)) = y;
                *((short*)(n + 12)) = x;
                *((short*)(n + 14)) = flags;
                *((long*)(n + 16)) = time;
            }
        }
        public Ussn(object key, short z, short y, short x, BitVector32 flags, DateTime time)
        {
            byte[] shah = key.GetHashBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((long*)n) = *((long*)s);
                *((short*)(n + 8)) = z;
                *((short*)(n + 10)) = y;
                *((short*)(n + 12)) = x;
                *((short*)(n + 14)) = *((short*)&flags);
                *((long*)(n + 16)) = time.ToBinary();    //TODO: f.Tick - rok 2018.01.01 w tikach
            }
        }
        public Ussn(object key)
        {
            fixed (byte* n = bytes)
            {
                *((long*)n) = key.GetHashKey64();
               // *((long*)(n + 16)) = DateTime.Now.ToBinary();    //TODO: f.Tick - rok 2018.01.01 w tikach
            }
        }

        public byte[] this[int offset]
        {
            get
            {
                if (offset != 0)                   
                {
                    int l = 24 - offset;
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
                if (offset > 0 && l < 24)
                {
                    int count = 24 - offset;
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
                        Extractor.CopyBlock(pbyte, rbyte, 24);
                    }
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
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;
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
            byte[] r = new byte[24];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = bytes)
            {
                Extractor.CopyBlock(rbyte, pbyte, 24);
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

        public long    ValueFromXYZ(int vectorZ, int vectorY)
        {
            return (BlockZ * vectorZ * vectorY) + (BlockY * vectorY) + BlockX;
        }
        public ushort[] ValueToXYZ(long vectorZ, long vectorY, long value)
        {
            if (value > 0)
            {
                long vectorYZ = (vectorY * vectorZ);
                long blockZdiv = (value / vectorYZ);
                long blockYsub = value - (blockZdiv * vectorYZ);
                long blockYdiv = blockYsub / vectorY;
                long blockZ = (blockZdiv > 0 && (value % vectorYZ) > 0) ? blockZdiv + 1 : blockZdiv;
                long blockY = (blockYdiv > 0 && (value % vectorY) > 0) ? blockYdiv + 1 : blockYdiv;
                long blockX = value % vectorY;
                return new ushort[] { (ushort)blockZ, (ushort)blockY, (ushort)blockX };
            }
            return null;
        }

        public short    GetFlags()
        {
            fixed (byte* pbyte = bytes)
                return *((short*)(pbyte + 14));
        }
        public BitVector32 GetFlagsBits()
        {
            fixed (byte* pbyte = bytes)
                return new BitVector32(*((short*)(pbyte + 14)));
        }
        public void SetFlagsBits(BitVector32 bits)
        {
            fixed (byte* pbyte = bytes)
                *((short*)(pbyte + 14)) = *((short*)&bits);
        }

        public long     GetDateLong()
        {
            fixed (byte* pbyte = bytes)
                return *((long*)(pbyte + 16));
        }
        public DateTime GetDateTime()
        {
            fixed (byte* pbyte = bytes)
                return DateTime.FromBinary(*((long*)(pbyte + 16)));
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
            if (!(value is Ussn))
                throw new Exception();

            return (int)(KeyBlock - ((Ussn)value).KeyBlock);
        }

        public int CompareTo(Ussn g)
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
                return new Ussn(value.ToString()).KeyBlock == KeyBlock;

            return (KeyBlock == ((Ussn)value).KeyBlock);
        }

        public bool Equals(Ussn g)
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

        public static bool operator ==(Ussn a, Ussn b)
        {
            return (a.KeyBlock == b.KeyBlock);
        }

        public static bool operator !=(Ussn a, Ussn b)
        {
            return (a.KeyBlock != b.KeyBlock);
        }

        public static explicit operator Ussn(String s)
        {
            return new Ussn(s);
        }
        public static implicit operator String(Ussn s)
        {
            return s.ToString();
        }

        public static explicit operator Ussn(byte[] l)
        {
            return new Ussn(l);
        }
        public static implicit operator byte[](Ussn s)
        {
            return s.GetBytes();
        }

        public static Ussn Empty
        {
            get { return new Ussn(); }
        }      

        IUnique IUnique.Empty
        {
            get
            {
                return new Ussn();
            }
        }       

        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[32];
            long pchblock;  
            int pchlength = 32;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 4; j++)
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

            for (int j = 0; j < 4; j++)
            {
                pchblock = 0x00L;
                blocklength = Math.Min(8, Math.Max(0, pchlength - 8 * j));        //required if trimmed zeros, length < 32
                idx = Math.Min(pchlength, 8*(j+1)) - 1;                           //required if trimmed zeros, length <32

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
                        pchblock_int = (int)(pchblock >> 16);
                        *((long*)&pbyte[18]) = pchblock_short;
                        *((long*)&pbyte[20]) = pchblock_int;
                        break;
                    }
                    *((long*)&pbyte[j * 6]) = pchblock;

                }
            }                                    
        }

        public bool EqualsContent(Ussn g)
        {
            long pchblockA, pchblockB, pchblockC;
            bool result;

            if (g == null) return false;
            fixed (byte* pbyte = bytes)
            {
                pchblockA = *((long*)&pbyte[0]);
                pchblockB = *((long*)&pbyte[8]);
                pchblockC = *((long*)&pbyte[16]);
            }

                result = (pchblockA  == * ((long*)&g.bytes[0]))
                && (pchblockB == *((long*)&g.bytes[8]))
                && (pchblockC == *((long*)&g.bytes[16]));
            
            
            return result;
        }

        public bool Equals(BitVector32 other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DateTime other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISerialNumber other)
        {
            throw new NotImplementedException();
        }     
    }
}
