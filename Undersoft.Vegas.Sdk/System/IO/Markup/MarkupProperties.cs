/*************************************************
   Copyright (c) 2021 Undersoft

   System.MarkupProperties.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.IO
{
    #region Enums

    public enum MarkupType
    {
        None = (byte)0xFF,
        Block = (byte)0x17,
        End = (byte)0x04,
        Empty = (byte)0x00,
        Line = (byte)0x10,
        Space = (byte)0x32,
        Semi = (byte)0x59,
        Coma = (byte)0x44,
        Colon = (byte)0x58,
        Dot = (byte)0x46,
        Cancel = (byte)0x18,
    }
    public enum SeekDirection
    {
        Forward,
        Backward
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="MarkedSegment" />.
    /// </summary>
    public struct MarkedSegment
    {
        #region Fields

        public int ItemSize;
        public long Length;
        public long Offset;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Count.
        /// </summary>
        public int Count => (int)(Length / ItemSize);

        #endregion

        #region Methods

        /// <summary>
        /// The ItemOffset.
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public long ItemOffset(int index)
        {
            return (Offset + (index * ItemSize));
        }

        #endregion
    }
}
