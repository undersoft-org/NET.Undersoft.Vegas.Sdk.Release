/*************************************************
   Copyright (c) 2021 Undersoft

   System.ArrayListExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Collections
{
    /// <summary>
    /// Defines the <see cref="ArrayListExtensions" />.
    /// </summary>
    public static class ArrayListExtensions
    {
        #region Methods

        /// <summary>
        /// The Resize.
        /// </summary>
        /// <param name="array">The array<see cref="ArrayList"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ArrayList"/>.</returns>
        public static ArrayList Resize(this ArrayList array, int size)
        {
            int resize = size - array.Count;
            ArrayList fill = ArrayList.Repeat(null, resize);
            (array).Capacity = size;
            (array).AddRange(fill);
            return array;
        }

        #endregion
    }
}
