/*************************************************
   Copyright (c) 2021 Undersoft

   System.ByteMarkupExetnsion.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.IO
{
    /// <summary>
    /// Defines the <see cref="ByteMarkupExtension" />.
    /// </summary>
    public static class ByteMarkupExtension
    {
        #region Methods

        /// <summary>
        /// The IsMarkup.
        /// </summary>
        /// <param name="checknoise">The checknoise<see cref="byte"/>.</param>
        /// <param name="noisekind">The noisekind<see cref="MarkupType"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsMarkup(this byte checknoise, out MarkupType noisekind)
        {
            switch (checknoise)
            {
                case (byte)MarkupType.Block:
                    noisekind = MarkupType.Block;
                    return true;
                case (byte)MarkupType.End:
                    noisekind = MarkupType.End;
                    return true;
                case (byte)MarkupType.Empty:
                    noisekind = MarkupType.Empty;
                    return false;
                default:
                    noisekind = MarkupType.None;
                    return false;
            }
        }

        /// <summary>
        /// The IsSpliter.
        /// </summary>
        /// <param name="checknoise">The checknoise<see cref="byte"/>.</param>
        /// <param name="spliterkind">The spliterkind<see cref="MarkupType"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsSpliter(this byte checknoise, out MarkupType spliterkind)
        {
            switch (checknoise)
            {
                case (byte)MarkupType.Empty:
                    spliterkind = MarkupType.Empty;
                    return true;
                case (byte)MarkupType.Line:
                    spliterkind = MarkupType.Line;
                    return true;
                case (byte)MarkupType.Space:
                    spliterkind = MarkupType.Space;
                    return true;
                case (byte)MarkupType.Semi:
                    spliterkind = MarkupType.Semi;
                    return true;
                case (byte)MarkupType.Coma:
                    spliterkind = MarkupType.Coma;
                    return true;
                case (byte)MarkupType.Colon:
                    spliterkind = MarkupType.Colon;
                    return true;
                case (byte)MarkupType.Dot:
                    spliterkind = MarkupType.Dot;
                    return true;
                default:
                    spliterkind = MarkupType.None;
                    return false;
            }
        }

        #endregion
    }
}
