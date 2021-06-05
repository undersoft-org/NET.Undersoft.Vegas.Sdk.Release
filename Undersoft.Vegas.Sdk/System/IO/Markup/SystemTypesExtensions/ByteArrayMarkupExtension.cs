/*************************************************
   Copyright (c) 2021 Undersoft

   System.ByteArrayMarkupExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.IO
{
    /// <summary>
    /// Defines the <see cref="ByteArrayMarkupExtension" />.
    /// </summary>
    public static class ByteArrayMarkupExtension
    {
        #region Methods

        /// <summary>
        /// The Initialize.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="defaultValue">The defaultValue<see cref="byte"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Initialize(this byte[] array, byte defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }
            return array;
        }

        /// <summary>
        /// The Initialize.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="defaultValue">The defaultValue<see cref="char"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Initialize(this byte[] array, char defaultValue)
        {
            byte[] charbyte = BitConverter.GetBytes(defaultValue);
            int charlength = charbyte.Length;
            int length = (array.Length % charlength) + array.Length;
            int counter = 0;
            for (int i = 0; i < length; i++)
            {
                array[i] = charbyte[counter];
                counter = (charlength > counter) ? counter++ : 0;
            }
            return array;
        }

        /// <summary>
        /// The Markup.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="blocksize">The blocksize<see cref="int"/>.</param>
        /// <param name="bytenoise">The bytenoise<see cref="MarkupType"/>.</param>
        /// <param name="written">The written<see cref="int"/>.</param>
        /// <param name="byteprefix">The byteprefix<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Markup(this byte[] array, int blocksize, MarkupType bytenoise, int written = 0, int byteprefix = 0)
        {
            int arrayLength = (written > 0) ? written : array.Length;
            int blockSize = blocksize;
            long blockLeft = arrayLength % blockSize;
            long resize = (blockSize - blockLeft >= 28) ? blockSize - blockLeft - byteprefix : blockSize + (blockSize - blockLeft) - byteprefix;
            byte[] byteMarkup = new byte[resize].Initialize((byte)bytenoise);
            Array.Resize<byte>(ref array, arrayLength + (int)resize);
            byteMarkup.CopyTo(array, arrayLength);
            byteMarkup = null;
            return array;
        }

        /// <summary>
        /// The Markup.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="blocksize">The blocksize<see cref="long"/>.</param>
        /// <param name="bytenoise">The bytenoise<see cref="MarkupType"/>.</param>
        /// <param name="written">The written<see cref="int"/>.</param>
        /// <param name="byteprefix">The byteprefix<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Markup(this byte[] array, long blocksize, MarkupType bytenoise, int written = 0, int byteprefix = 0)
        {
            int arrayLength = (written > 0) ? written : array.Length;
            long blockSize = blocksize;
            long blockLeft = arrayLength % blockSize;
            long resize = (blockSize - blockLeft >= 28) ? blockSize - blockLeft - byteprefix : blockSize + (blockSize - blockLeft) - byteprefix;
            byte[] byteMarkup = new byte[resize].Initialize((byte)bytenoise);
            Array.Resize<byte>(ref array, arrayLength + (int)resize);
            byteMarkup.CopyTo(array, arrayLength);
            byteMarkup = null;
            return array;
        }

        /// <summary>
        /// The SeekMarkup.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="position">The position<see cref="long"/>.</param>
        /// <param name="direction">The direction<see cref="SeekDirection"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="_length">The _length<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public static MarkupType SeekMarkup(this byte[] array, out long position, SeekDirection direction = SeekDirection.Forward, int offset = 0, int _length = -1)
        {
            bool isFwd = (direction != SeekDirection.Forward) ? false : true;
            short noiseFlag = 0;
            MarkupType noiseKind = MarkupType.None;
            MarkupType lastKind = MarkupType.None;
            if (array.Length > 0)
            {
                long length = (_length <= 0 || _length > array.Length) ? array.Length : _length;
                int arraylength = array.Length;
                offset += (!isFwd) ? 1 : 0;
                length -= ((!isFwd) ? 0 : 1);

                for (int i = offset; i < length; i++)
                {
                    byte checknoise = 0;
                    if (!isFwd)
                        checknoise = array[arraylength - i];
                    else
                        checknoise = array[i];

                    MarkupType tempKind = MarkupType.None;
                    if (checknoise.IsMarkup(out tempKind))
                    {
                        lastKind = tempKind;
                        noiseFlag++;
                    }
                    else if (noiseFlag >= 16)
                    {
                        noiseKind = lastKind;
                        position = (!isFwd) ? arraylength - i + 1 : i;
                        return noiseKind;
                    }
                    else
                    {
                        lastKind = tempKind;
                        noiseFlag = 0;
                    }
                }
            }
            position = (!isFwd && noiseFlag != 0) ? array.Length - noiseFlag + 1 : 0;
            return lastKind;
        }

        /// <summary>
        /// The SeekSegment.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="itemSize">The itemSize<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkedSegment"/>.</returns>
        public static MarkedSegment SeekSegment(this byte[] array, int itemSize = 0)
        {
            int noiseCount = (array.Length > 4096) ? 1024 : 4096;

            long endPosition = array.Length;
            long endTempPosition = 0;

            MarkupType endNoise = array.SeekMarkup(out endTempPosition, SeekDirection.Backward, 0, noiseCount);
            if (endNoise != MarkupType.None)
                endPosition = endTempPosition;

            long startPosition = 0;
            long startTempPosition = 0;

            MarkupType startNoise = array.SeekMarkup(out startTempPosition, SeekDirection.Forward, 0, noiseCount);
            if (startNoise != MarkupType.None)
                startPosition = startTempPosition;

            return new MarkedSegment() { Offset = startPosition, Length = endPosition - startPosition, ItemSize = itemSize };
        }

        #endregion
    }
}
