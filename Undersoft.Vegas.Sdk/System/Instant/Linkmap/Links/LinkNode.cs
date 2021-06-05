using System.Uniques;
using System.Multemic;
using System.Collections.Generic;
using System.Linq;

namespace System.Instant.Linkmap
{
    public class LinkNode : IUnique
    {
        public LinkMember Member { get; set; }
        public LinkBranch Branch { get; set; }

        public Ussn SystemSerialCode { get; set; }

        public IUnique Empty => Ussn.Empty;

        public long KeyBlock
        {
            get => SystemSerialCode.KeyBlock;
            set => SystemSerialCode.SetHashKey(value);
        }

        public int CompareTo(IUnique other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IUnique other)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public long   GetHashKey()
        {
            throw new NotImplementedException();
        }
        public byte[] GetKeyBytes()
        {
            throw new NotImplementedException();
        }
        public void   SetHashKey(long value)
        {
            throw new NotImplementedException();
        }

        public uint SeedBlock
        {
            get => SystemSerialCode.SeedBlock;
            set => SystemSerialCode.SetHashSeed(value);
        }
        public void SetHashSeed(uint seed)
        {
            SystemSerialCode.SetHashSeed(seed);
        }
        public uint GetHashSeed()
        {
            return SystemSerialCode.GetHashSeed();
        }
    }
}
