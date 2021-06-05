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

    /// <summary>
    /// Defines the <see cref="MemberRubrics" />.
    /// </summary>
    public partial class MemberRubrics : BaseAlbum<MemberRubric>, IRubrics
    {
        #region Fields

        private int binarySize;
        private int[] binarySizes;
        private int[] ordinals;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubrics"/> class.
        /// </summary>
        public MemberRubrics()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubrics"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{MemberRubric}"/>.</param>
        public MemberRubrics(IEnumerable<MemberRubric> collection)
            : base(collection)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubrics"/> class.
        /// </summary>
        /// <param name="collection">The collection<see cref="IList{MemberRubric}"/>.</param>
        public MemberRubrics(IList<MemberRubric> collection)
            : base(collection)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the BinarySize.
        /// </summary>
        public int BinarySize { get => binarySize; }

        /// <summary>
        /// Gets the BinarySizes.
        /// </summary>
        public int[] BinarySizes { get => binarySizes; }

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => Figures.Empty;

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures { get; set; }

        /// <summary>
        /// Gets or sets the KeyRubrics.
        /// </summary>
        public IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the Mappings.
        /// </summary>
        public FieldMappings Mappings { get; set; }

        /// <summary>
        /// Gets the Ordinals.
        /// </summary>
        public int[] Ordinals { get => ordinals; }

        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Ussn SerialCode { get => Figures.SerialCode; set => Figures.SerialCode = value; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => Figures.UniqueKey; set => Figures.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => Figures.UniqueSeed; set => Figures.UniqueSeed = value; }

        /// <summary>
        /// Gets or sets the ValueArray.
        /// </summary>
        public object[] ValueArray { get => Figures.ValueArray; set => Figures.ValueArray = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return Figures.CompareTo(other);
        }

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{MemberRubric}[]"/>.</returns>
        public override ICard<MemberRubric>[] EmptyBaseDeck(int size)
        {
            return new RubricCard[size];
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{MemberRubric}"/>.</returns>
        public override ICard<MemberRubric> EmptyCard()
        {
            return new RubricCard();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{MemberRubric}[]"/>.</returns>
        public override ICard<MemberRubric>[] EmptyCardTable(int size)
        {
            return new RubricCard[size];
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return Figures.Equals(other);
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes()
        {
            return Figures.GetBytes();
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
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

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetUniqueBytes()
        {
            return Figures.GetUniqueBytes();
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="seed">The seed<see cref="uint"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
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

        /// <summary>
        /// The GetUniqueKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="seed">The seed<see cref="uint"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
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

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="ICard{MemberRubric}"/>.</param>
        /// <returns>The <see cref="ICard{MemberRubric}"/>.</returns>
        public override ICard<MemberRubric> NewCard(ICard<MemberRubric> value)
        {
            return new RubricCard(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        /// <returns>The <see cref="ICard{MemberRubric}"/>.</returns>
        public override ICard<MemberRubric> NewCard(MemberRubric value)
        {
            return new RubricCard(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        /// <returns>The <see cref="ICard{MemberRubric}"/>.</returns>
        public override ICard<MemberRubric> NewCard(object key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="ulong"/>.</param>
        /// <param name="value">The value<see cref="MemberRubric"/>.</param>
        /// <returns>The <see cref="ICard{MemberRubric}"/>.</returns>
        public override ICard<MemberRubric> NewCard(ulong key, MemberRubric value)
        {
            return new RubricCard(key, value);
        }

        /// <summary>
        /// The SetUniqueKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="seed">The seed<see cref="uint"/>.</param>
        public void SetUniqueKey(IFigure figure, uint seed = 0)
        {
            figure.UniqueKey = GetUniqueKey(figure, seed);
        }

        /// <summary>
        /// The Update.
        /// </summary>
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
