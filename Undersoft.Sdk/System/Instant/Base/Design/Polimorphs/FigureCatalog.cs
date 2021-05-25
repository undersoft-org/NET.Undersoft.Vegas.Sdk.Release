using System.Uniques;
using System.Instant.Linkmap;
using System.Instant.Treatment;
using System.IO;
using System.Multemic;
using System.Runtime.InteropServices;

namespace System.Instant
{
    public abstract class FigureCatalog : SharedAlbum<IFigure>, IFigures
    {
        public IInstant Instant { get; set; }

        public abstract bool Prime { get; set; }

        public abstract object this[int index, string propertyName] { get; set; }

        public abstract object this[int index, int fieldId] { get; set; }       

        public abstract IRubrics Rubrics { get; set; }      

        public abstract IRubrics KeyRubrics { get; set; }

        public abstract IFigure NewFigure();

        public abstract  Type FigureType { get; set; }

        public abstract int FigureSize { get; set; }

        public abstract Ussn SystemSerialCode { get; set; }

        public int Length { get; }

        public override ICard<IFigure> EmptyCard()
        {
            return new FigureCard(this);
        }

        public override ICard<IFigure> NewCard(long key, IFigure value)
        {
            return new FigureCard(key, value, this);
        }
        public override ICard<IFigure> NewCard(object key, IFigure value)
        {
            return new FigureCard(key, value, this);
        }
        public override ICard<IFigure> NewCard(IFigure value)
        {
            return new FigureCard(value, this);
        }
        public override ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return new FigureCard(value, this);
        }

        public override ICard<IFigure>[] EmptyCardTable(int size)
        {
            return new FigureCard[size];
        }

        public override ICard<IFigure>[] EmptyCardList(int size)
        {
            cards = new FigureCard[size];
            return cards;
        }

        private ICard<IFigure>[] cards;
        public ICard<IFigure>[] Cards { get => cards; }

        protected override bool InnerAdd(IFigure value)
        {
            return InnerAdd(NewCard(value));
        }

        protected override ICard<IFigure> InnerPut(IFigure value)
        {
            return InnerPut(NewCard(value));
        }

        public object[] ValueArray { get => ToObjectArray(); set => Put(value); }

        public Type Type { get; set; }

        public IFigures Picked { get; set; }

        public IFigure Summary { get; set; }

        public FigureFilter Filter { get; set; }

        public FigureSort Sort { get; set; }

        public Func<IFigure, bool> Picker { get; set; }

        public FigureLinks Links { get; set; } = new FigureLinks();

        private FigureLinkmap linkmap;
        public  FigureLinkmap Linkmap { get => linkmap == null ? linkmap = new FigureLinkmap(this) : linkmap;
                                        set => linkmap = value; }

        private FigureTreatment treatment;
        public  FigureTreatment Treatment
        {
            get => treatment == null ? treatment = new FigureTreatment(this) : treatment;
            set => treatment = value;
        }

        public IDeck<IComputation> Computations { get; set; }

        #region Uniques

        public IUnique Empty => Ussn.Empty;

        public long KeyBlock { get => SystemSerialCode.KeyBlock; set => SystemSerialCode.SetHashKey(value); }

        object IFigure.this[int fieldId] { get => this[fieldId]; set => this[fieldId] = (IFigure)value; }
        public object this[string propertyName] { get => this[propertyName]; set => this[propertyName] = (IFigure)value; }

        public byte[] GetBytes()
        {
            return SystemSerialCode.GetBytes();
        }

        public byte[] GetKeyBytes()
        {
            return SystemSerialCode.GetKeyBytes();
        }

        public void SetHashKey(long value)
        {
            SystemSerialCode.SetHashKey(value);
        }

        public long GetHashKey()
        {
            return SystemSerialCode.GetHashKey();
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

        public bool Equals(IUnique other)
        {
            return SystemSerialCode.Equals(other);
        }

        public int CompareTo(IUnique other)
        {
            return SystemSerialCode.CompareTo(other);
        }
    
        #endregion       

        #region Formatter

        public int SerialCount { get; set; }
        public int DeserialCount { get; set; }
        public int ProgressCount { get; set; }

        public int Serialize(Stream stream, int offset, int batchSize, FigureFormat serialFormat = FigureFormat.Binary)
        {
            throw new NotImplementedException();
        }
        public int Serialize(IFigurePacket buffor, int offset, int batchSize, FigureFormat serialFormat = FigureFormat.Binary)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Stream stream, FigureFormat serialFormat = FigureFormat.Binary)
        {
            throw new NotImplementedException();
        }
        public object Deserialize(ref object block, FigureFormat serialFormat = FigureFormat.Binary)
        {
            throw new NotImplementedException();
        }

        public object[] GetMessage()
        {
            return new[] { (IFigures)this };
        }

        public object GetHeader()
        {
            return this;
        }       

        public int ItemsCount => Count;      

        #endregion

    }
}