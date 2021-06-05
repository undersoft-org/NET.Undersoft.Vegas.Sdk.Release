/*************************************************
  Copyright (c) 2021 Undersoft

  System.Sets.vEBTreeNode.cs

  @project: Undersoft.Vegas.Sdk
  @stage: Development
  @author: PhD Radoslaw Rudek, Dariusz Hanc
  @date: (30.05.2021) 
  @licence MIT
*************************************************/

namespace System.Sets.Spectrum
{
    /// <summary>
    /// Defines the <see cref="vEBTreeNode" />.
    /// </summary>
    public class vEBTreeNode
    {
        #region Properties

        /// <summary>
        /// Gets or sets the IndexOffset.
        /// </summary>
        public int IndexOffset { get; set; }

        /// <summary>
        /// Gets or sets the NodeCounter.
        /// </summary>
        public int NodeCounter { get; set; }

        /// <summary>
        /// Gets or sets the NodeSize.
        /// </summary>
        public int NodeSize { get; set; }

        #endregion
    }
}
