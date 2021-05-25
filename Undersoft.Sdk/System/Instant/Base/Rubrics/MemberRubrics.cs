using System.Collections.Generic;
using System.Multemic;
using System.Uniques;
using System.Linq;

namespace System.Instant
{
    public partial class MemberRubrics : CardBook<MemberRubric>, IRubrics
    {
        public MemberRubrics() 
            : base() { }
        public MemberRubrics(IList<MemberRubric> collection) 
            : base(collection) { }
        public MemberRubrics(IEnumerable<MemberRubric> collection) 
            : base(collection) { }

        public override ICard<MemberRubric> EmptyCard()
        {
            return new RubricCard();
        }

        public override ICard<MemberRubric>[] EmptyCardTable(int size)
        {
            return new RubricCard[size];
        }
        public override ICard<MemberRubric>[] EmptyCardList(int size)
        {          
            return new RubricCard[size];
        }

        public override ICard<MemberRubric> NewCard(object key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        public override ICard<MemberRubric> NewCard(long key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        public override ICard<MemberRubric> NewCard(MemberRubric value)
        {
            return new RubricCard(value.GetHashKey(), value);
        }
        public override ICard<MemberRubric> NewCard(ICard<MemberRubric> value)
        {
            return new RubricCard(value);
        }

        public IRubrics KeyRubrics { get; set; }

        public IFigures Figures { get; set; }

        public FieldMappings Mappings { get; set; }

        private int[] ordinals;
        public int[] Ordinals { get => ordinals; }

        public void Update()
        {
            ordinals = this.AsValues().Select(o => o.FigureFieldId).ToArray();
            if (KeyRubrics != null)
                KeyRubrics.Update();
        }

        #region IFigure

        public object[] ValueArray { get => Figures.ValueArray; set => Figures.ValueArray = value; }
        public Ussn SystemSerialCode { get => Figures.SystemSerialCode; set => Figures.SystemSerialCode = value; }

        public IUnique Empty => Figures.Empty;

        public long KeyBlock { get => Figures.KeyBlock; set => Figures.KeyBlock = value; }
        public uint SeedBlock { get => Figures.SeedBlock; set => Figures.SeedBlock = value; }

        public byte[] GetBytes()
        {
            return Figures.GetBytes();
        }
        public byte[] GetKeyBytes()
        {
            return Figures.GetKeyBytes();
        }    
        public void SetHashKey(long value)
        {
            Figures.SetHashKey(value);
        }
        public long GetHashKey()
        {
            return Figures.GetHashKey();
        }
        public bool Equals(IUnique other)
        {
            return Figures.Equals(other);
        }
        public int CompareTo(IUnique other)
        {
            return Figures.CompareTo(other);
        }

        public void SetHashSeed(uint seed)
        {
            Figures.SetHashSeed(seed);
        }

        public uint GetHashSeed()
        {
            return Figures.GetHashSeed();
        }

        #endregion
    }


}
