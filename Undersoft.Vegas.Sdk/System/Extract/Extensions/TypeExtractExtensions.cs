/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.TypeExtractExtensions.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    /// <summary>
    /// Defines the <see cref="TypeExtractExtenstion" />.
    /// </summary>
    public static class TypeExtractExtenstion
    {
        #region Methods

        /// <summary>
        /// The NewStructure.
        /// </summary>
        /// <param name="structure">The structure<see cref="Type"/>.</param>
        /// <param name="binary">The binary<see cref="byte*"/>.</param>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object NewStructure(this Type structure, byte* binary, long offset = 0)
        {
            // return _copier.PtrToStruct(binary, structure);
            // object o = Activator.CreateInstance(structure);
            return Extractor.PointerToStructure(binary, structure, offset);
        }

        /// <summary>
        /// The NewStructure.
        /// </summary>
        /// <param name="structure">The structure<see cref="Type"/>.</param>
        /// <param name="binary">The binary<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object NewStructure(this Type structure, byte[] binary, long offset = 0)
        {
            //return _copier.PtrToStruct(binary, structure);

            // object o = Activator.CreateInstance(structure);
            return Extractor.BytesToStructure(binary, structure, offset);
        }

        #endregion
    }
}
