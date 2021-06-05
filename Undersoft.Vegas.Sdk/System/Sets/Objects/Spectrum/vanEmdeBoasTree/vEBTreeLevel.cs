/*************************************************
   Copyright (c) 2021 Undersoft

   System.Sets.vEBTreeLevel.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: PhD Radoslaw Rudek, Dariusz Hanc
   @date: (30.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Sets.Spectrum
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="vEBTreeLevel" />.
    /// </summary>
    public class vEBTreeLevel
    {
        #region Constructors

        public vEBTreeLevel()
        {
            Level = 0;
            BaseOffset = 0;
            Scopes = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the BaseOffset.
        /// </summary>
        public int BaseOffset { get; set; }

        /// <summary>
        /// Gets or sets the Count.
        /// </summary>
        public byte Count { get; set; }

        /// <summary>
        /// Gets or sets the Level.
        /// </summary>
        public byte Level { get; set; }

        /// <summary>
        /// Gets or sets the Scopes.
        /// </summary>
        public IList<vEBTreeNode> Scopes { get; set; }

        #endregion
    }
}
