/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.MemberRubrics.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Collections.Generic;
    using System.Extract;
    using System.Linq;
    using System.Sets;
    using System.Uniques;

    public partial class MemberRubrics : BaseAlbum<MemberRubric>, IRubrics
    {
        #region Fields

        private int[] ordinals;
        private int[] binarySizes;
        private int   binarySize;

        #endregion

        #region Constructors

        public MemberRubrics()
            : base()
        {
        }
        public MemberRubrics(IEnumerable<MemberRubric> collection)
            : base(collection)
        {
        }
        public MemberRubrics(IList<MemberRubric> collection)
            : base(collection)
        {
        }

        #endregion

        #region Properties

        public IUnique Empty => Figures.Empty;

        public IFigures Figures { get; set; }

        public IRubrics KeyRubrics { get; set; }

        public FieldMappings Mappings { get; set; }

        public int[] Ordinals { get => ordinals; }

        public int[] BinarySizes { get => binarySizes; }

        public int BinarySize { get => binarySize; }

        public Ussn SerialCode { get => Figures.SerialCode; set => Figures.SerialCode = value; }

        public ulong UniqueKey { get => Figures.UniqueKey; set => Figures.UniqueKey = value; }

        public ulong UniqueSeed { get => Figures.UniqueSeed; set => Figures.UniqueSeed = value; }

        public object[] ValueArray { get => Figures.ValueArray; set => Figures.ValueArray = value; }

        #endregion

        #region Methods

        public int CompareTo(IUnique other)
        {
            return Figures.CompareTo(other);
        }

        public override ICard<MemberRubric> EmptyCard()
        {
            return new RubricCard();
        }

        public override ICard<MemberRubric>[] EmptyBaseDeck(int size)
        {
            return new RubricCard[size];
        }

        public override ICard<MemberRubric>[] EmptyCardTable(int size)
        {
            return new RubricCard[size];
        }

        public bool Equals(IUnique other)
        {
            return Figures.Equals(other);
        }

        public byte[] GetBytes()
        {
            return Figures.GetBytes();
        }

        public unsafe byte[] GetBytes(IFigure figure)
        {
            int size = Figures.FigureSize;
            byte* figurePtr = stackalloc byte[size * 2];
            byte* bufferPtr = figurePtr + size;
            figure.StructureTo(figurePtr);
            int destOffset = 0;
            foreach (var rubric in AsValues())
            {
                int l = rubric.RubricSize;
                Extractor.CopyBlock(bufferPtr, destOffset, figurePtr, rubric.RubricOffset, l);
                destOffset += l;
            }
            byte[] b = new byte[destOffset];
            fixed (byte* bp = b)
                Extractor.CopyBlock(bp, bufferPtr, destOffset);
            return b;
        }

        public byte[] GetUniqueBytes()
        {
            return Figures.GetUniqueBytes();
        }

        public unsafe byte[] GetUniqueBytes(IFigure figure, uint seed = 0)
        {
            //return KeyRubrics.Ordinals.Select(x => figure[x]).ToArray().UniqueKey64();
            int size = Figures.FigureSize;
            byte* figurePtr = stackalloc byte[size * 2];
            byte* bufferPtr = figurePtr + size;
            figure.StructureTo(figurePtr);
            int destOffset = 0;
            foreach (var rubric in AsValues())
            {
                int l = rubric.RubricSize;
                Extractor.CopyBlock(bufferPtr, destOffset, figurePtr, rubric.RubricOffset, l);
                destOffset += l;
            }
            ulong hash = Hasher64.ComputeKey(bufferPtr, destOffset, seed);
            byte[] b = new byte[8];
            fixed (byte* bp = b)
                *((ulong*)bp) = hash;
            return b;
        }

        public unsafe ulong GetUniqueKey(IFigure figure, uint seed = 0)
        {
            int size = Figures.FigureSize;
            byte* figurePtr = stackalloc byte[size * 2];
            byte* bufferPtr = figurePtr + size;
            figure.StructureTo(figurePtr);
            int destOffset = 0;
            foreach (var rubric in AsValues())
            {
                int l = rubric.RubricSize;
                Extractor.CopyBlock(bufferPtr, destOffset, figurePtr, rubric.RubricOffset, l);
                destOffset += l;
            }
            return Hasher64.ComputeKey(bufferPtr, destOffset, seed);
        }

        public override ICard<MemberRubric> NewCard(ICard<MemberRubric> value)
        {
            return new RubricCard(value);
        }

        public override ICard<MemberRubric> NewCard(ulong  key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        public override ICard<MemberRubric> NewCard(MemberRubric value)
        {
            return new RubricCard(value);
        }

        public override ICard<MemberRubric> NewCard(object key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        public void SetUniqueKey(IFigure figure, uint seed = 0)
        {
            figure.UniqueKey = GetUniqueKey(figure, seed);
        }

        public void Update()
        {
            ordinals = this.AsValues().Select(o => o.FieldId).ToArray();
            binarySizes = this.AsValues().Select(o => o.RubricSize).ToArray();
            binarySize = this.AsValues().Sum(b => b.RubricSize);
            if (KeyRubrics != null)
                KeyRubrics.Update();
        }

        #endregion
    }
}
