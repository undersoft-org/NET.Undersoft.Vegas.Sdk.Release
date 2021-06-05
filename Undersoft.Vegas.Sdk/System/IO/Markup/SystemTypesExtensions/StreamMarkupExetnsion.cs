/*************************************************
   Copyright (c) 2021 Undersoft

   System.StreamMarkupExetnsion.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.IO
{
    /// <summary>
    /// Defines the <see cref="StreamMarkupExtension" />.
    /// </summary>
    public static class StreamMarkupExtension
    {
        #region Methods

        /// <summary>
        /// The Markup.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="blocksize">The blocksize<see cref="int"/>.</param>
        /// <param name="bytenoise">The bytenoise<see cref="MarkupType"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public static Stream Markup(this Stream stream, int blocksize, MarkupType bytenoise)
        {
            int blockSize = blocksize;
            long blockLeft = stream.Length % blockSize;
            long resize = (blockSize - blockLeft >= 28) ? blockSize - blockLeft - 16 : blockSize + (blockSize - blockLeft) - 16;
            byte[] byteMarkup = new byte[resize].Initialize((byte)bytenoise);
            stream.Write(byteMarkup, 0, (int)resize);
            return stream;
        }

        /// <summary>
        /// The Markup.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="blocksize">The blocksize<see cref="long"/>.</param>
        /// <param name="bytenoise">The bytenoise<see cref="MarkupType"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public static Stream Markup(this Stream stream, long blocksize, MarkupType bytenoise)
        {
            long blockSize = blocksize;
            long blockLeft = stream.Length % blockSize;
            long resize = (blockSize - blockLeft >= 28) ? blockSize - blockLeft - 16 : blockSize + (blockSize - blockLeft) - 16;
            byte[] byteMarkup = new byte[resize].Initialize((byte)bytenoise);
            stream.Write(byteMarkup, 0, (int)resize);
            return stream;
        }

        /// <summary>
        /// The SeekMarkup.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="seekorigin">The seekorigin<see cref="SeekOrigin"/>.</param>
        /// <param name="direction">The direction<see cref="SeekDirection"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="_length">The _length<see cref="int"/>.</param>
        /// <returns>The <see cref="MarkupType"/>.</returns>
        public static MarkupType SeekMarkup(this Stream stream, SeekOrigin seekorigin = SeekOrigin.Begin, SeekDirection direction = SeekDirection.Forward, int offset = 0, int _length = -1)
        {
            bool isFwd = (direction != SeekDirection.Forward) ? false : true;
            short noiseFlag = 0;
            MarkupType noiseKind = MarkupType.None;
            MarkupType lastKind = MarkupType.None;
            if (stream.Length > 0)
            {
                long length = (_length <= 0) ? stream.Length : _length;
                long saveposition = stream.Position;
                offset += (!isFwd) ? 1 : 0;
                length -= ((!isFwd) ? 0 : 1);

                for (int i = offset; i < length; i++)
                {
                    if (!isFwd)
                        stream.Seek(-i, seekorigin);
                    else
                        stream.Seek(i, seekorigin);

                    byte checknoise = (byte)stream.ReadByte();

                    MarkupType tempKind = MarkupType.None;
                    if (checknoise.IsMarkup(out tempKind))
                    {
                        lastKind = tempKind;
                        noiseFlag++;
                    }
                    else if (noiseFlag >= 16)
                    {
                        noiseKind = lastKind;
                        return noiseKind;
                    }
                    else
                    {
                        lastKind = tempKind;
                        noiseFlag = 0;
                    }
                }
                stream.Position = saveposition;
            }
            return lastKind;
        }

        #endregion
    }
}
