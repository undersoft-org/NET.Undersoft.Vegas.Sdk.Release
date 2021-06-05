/*************************************************
   Copyright (c) 2021 Undersoft

   System.Uniques.HexTetraEncodingExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines the <see cref="HexTetraEncodingExtension" />.
    /// </summary>
    public static class HexTetraEncodingExtension
    {
        #region Fields

        // Character spectrum - below array is not used in algorithm - informational only
        private static readonly char[] _base64 = new[]{
            '0','1','2','3','4','5','6','7','8','9','A',
            'B','C','D','E','F','G','H','I','J','K','a',
            'b','c','d','e','f','g','h','i','j','k','L',
            'M','N','O','P','Q','R','S','T','U','V','W',
            'X','Y','Z','l','m','n','o','p','q','r','s',
            't','u','v','w','x','y','z','-','.'};

        #endregion

        #region Methods

        /// <summary>
        /// The ToHexTetraByte.
        /// </summary>
        /// <param name="phchar">The phchar<see cref="char"/>.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToHexTetraByte(this char phchar)
        {
            if (phchar <= '.')
                return (byte)(phchar + 17); //0-9
            else if (phchar <= '9')
                return (byte)(phchar - 48); //A-Z
            else if (phchar <= 'Z')
                return (byte)(phchar - 55); //a-z
            return (byte)(phchar - 61);      //- or .
        }

        /// <summary>
        /// The ToHexTetraChar.
        /// </summary>
        /// <param name="phbyte">The phbyte<see cref="byte"/>.</param>
        /// <returns>The <see cref="char"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToHexTetraChar(this byte phbyte)
        {
            if (phbyte <= 9)
                return (char)(phbyte + 48); //0-9
            else if (phbyte <= 35)
                return (char)(phbyte + 55); //A-Z
            else if (phbyte <= 61)
                return (char)(phbyte + 61); //a-z
            return (char)(phbyte - 17);      //- or .
        }

        #endregion
    }
}
