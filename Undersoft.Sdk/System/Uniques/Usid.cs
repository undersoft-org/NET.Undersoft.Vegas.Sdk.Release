using System.Extract;
using System.Runtime.InteropServices;
using System;

namespace System.Uniques
{
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public unsafe struct Usid : IFormattable, IComparable
        , IComparable<IUnique>, IEquatable<IUnique>, IUnique
    {
        private fixed byte bytes[8];       

        private long _KeyBlock
        {
            get
            {
                long block = KeyBlock;
                return (block << 32) | ((block >> 16) & 0xffff0000) | (block >> 48);
            }
            set
            {
                KeyBlock = (value >> 32) | (((value & 0x0ffff0000) << 16)) | (value << 48);
            }
        }

        public Usid(long l)
        {
            KeyBlock = l;
        }
        public Usid(string ca)
        {
            this.FromHexTetraChars(ca.ToCharArray());
        }
        public Usid(byte[] b)
        {
            if (b != null)
            {
                int l = b.Length;
                if (l > 8)
                    l = 8;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }
        public Usid(ushort z, ushort y, uint x)
        {
            fixed(byte* pbytes = bytes)
            {
                *((uint*)pbytes) = x;
                *((uint*)(pbytes + 4)) = y;
                *((uint*)(pbytes + 6)) = z;
            }
        }
        public Usid(object key)
        {
            fixed (byte* n = bytes)
                *((long*)n) = key.GetHashKey64();            
        }

        public byte[] this[int offset]
        {
            get
            {
                if (offset > 0 && offset < 8)
                {                   
                    int l = (8 - offset);
                    byte[] r = new byte[l];
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = r)
                        Extractor.CopyBlock(rbyte, pbyte + offset, l);
                    return r;
                }
                return GetBytes();
            }
            set
            {
                int l = value.Length;
                if (offset > 0 || l < 8)
                {
                    int count = 8 - offset;
                    if (l < count)
                        count = l;
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, l);
                    }
                }
                else
                {
                    fixed (byte* v = value)
                    fixed (byte* b = bytes)
                        *(long*)b = *(long*)v;
                }
            }
        }

        public byte[] GetBytes()
        {
            byte[] r = new byte[8];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = bytes)
            {
                *((long*)rbyte) = *((long*)pbyte);
            }
            return r;
        }

        public byte[] GetKeyBytes()
        {          
            return GetBytes();
        }

        public long KeyBlock
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

        public ushort BlockZ
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ushort*)(pbyte + 6));
            }
            set
            {
                fixed (byte* pbyte = bytes)
                    *((ushort*)(pbyte + 6)) = value;
            }
        }

        public ushort BlockY
        {
            get
            {
             
                fixed (byte* pbyte = bytes)
                    return *((ushort*)(pbyte + 4));
            }
            set
            {
                fixed (byte* pbyte = bytes)
                    *((ushort*)(pbyte + 4)) = value;
            }
        }

        public ushort BlockX
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ushort*)pbyte);
            }
            set
            {
                fixed (byte* pbyte = bytes)
                    *((ushort*)pbyte) = value;
            }
        }

        public bool IsNotEmpty
        {
            get { return (KeyBlock > 0); }
        }

        public bool IsEmpty
        {
            get { return (KeyBlock == 0); }
        }             

        public override int GetHashCode()
        {
            fixed (byte* pbyte = bytes)
            {
                return (int)HashHandle32.ComputeHashKey(pbyte, 8);
            }
        }

        public void SetHashKey(long value)
        {
            KeyBlock = value;
        }

        public long GetHashKey()
        {
            return KeyBlock;
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return -1;
            if (!(value is Usid))
                throw new Exception();

            return (int)(KeyBlock - value.GetHashKey64());
        }

        public int CompareTo(Usid g)
        {
            return (int)(KeyBlock - g.KeyBlock);
        }

        public int CompareTo(IUnique g)
        {
            return (int)(KeyBlock - g.GetHashKey());
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;
            if ((value is string))
                return new Usid(value.ToString()).KeyBlock == KeyBlock;

            return (KeyBlock == ((Usid)value).KeyBlock);
        }

        public bool Equals(long g)
        {
            return (KeyBlock == g);
        }
        public bool Equals(Usid g)
        {
            return (KeyBlock == g.KeyBlock);
        }
        public bool Equals(IUnique g)
        {
            return (KeyBlock == g.KeyBlock);
        }
        public bool Equals(string g)
        {
            return (KeyBlock == new Usid(g).KeyBlock);
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
            return new string(this.ToHexTetraChars());
        }

        public static bool operator ==(Usid a, Usid b)
        {
            return (a.KeyBlock == b.KeyBlock);
        }
        public static bool operator !=(Usid a, Usid b)
        {
            return (a.KeyBlock != b.KeyBlock);
        }

        public static explicit operator Usid(String s)
        {
            return new Usid(s);
        }
        public static implicit operator String(Usid s)
        {
            return s.ToString();
        }

        public static explicit operator Usid(byte[] l)
        {
            return new Usid(l);
        }
        public static implicit operator byte[] (Usid s)
        {
            return s.GetBytes();
        }

        public static Usid Empty
        {
            get { return new Usid(); }
        }

        IUnique IUnique.Empty
        {
            get
            {
                return new Usid();
            }
        }        

        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[10];
            long pchlong;
            byte pchbyte;
            int pchlength = 0;
            long _longValue = _KeyBlock;
            //56-bit representation: //A [max 2^30] //B [max 2^16] //C [max 2^10]
            //i.e., bits: [A: 55..26][B: 25..10][C: 9..0]
            pchlong = ((_longValue & 0x3fffffff00000000L) >> 6) | ((_longValue & 0xffff0000L) >> 6) | (_longValue & 0x03ffL);
            for (int i = 0; i < 5; i++)
            {
                pchbyte = (byte)(pchlong & 0x003fL);
                pchchar[i] = (pchbyte).ToHexTetraChar();
                pchlong = pchlong >> 6;
            }

            pchlength = 5;

            //Trim PrimeId
            for (int i = 5; i < 10; i++)
            {
                pchbyte = (byte)(pchlong & 0x003fL);
                if (pchbyte != 0x00) pchlength = i + 1;
                pchchar[i] = (pchbyte).ToHexTetraChar();
                pchlong = pchlong >> 6;
            }

            char[] pchchartrim = new char[pchlength];
            Array.Copy(pchchar, 0, pchchartrim, 0, pchlength);

            return pchchartrim;
        }

        public void FromHexTetraChars(char[] pchchar)
        {
            long pchlong = 0;
            byte pchbyte;
            int pchlength = 0;

            //bits: [A: 55..26][B: 25..10][C: 9..0]
            pchlength = pchchar.Length;
            pchbyte = (pchchar[pchlength - 1]).ToHexTetraByte();
            pchlong = pchbyte & 0x3fL;
            for (int i = pchlength - 2; i >= 0; i--)
            {
                pchbyte = (pchchar[i]).ToHexTetraByte();
                pchlong = pchlong << 6;
                pchlong = pchlong | (pchbyte & 0x3fL);
            }
            _KeyBlock = ((pchlong << 6) & 0x3fffffff00000000L) | ((pchlong << 6) & 0xffff0000L) | (pchlong & 0x03ffL);
        }

        public uint SeedBlock
        {
            get => 0;
            set => throw new NotImplementedException();
        }
        public void SetHashSeed(uint seed)
        {
            throw new NotImplementedException();
        }
        public uint GetHashSeed()
        {
            return 0;
        }
    }
}