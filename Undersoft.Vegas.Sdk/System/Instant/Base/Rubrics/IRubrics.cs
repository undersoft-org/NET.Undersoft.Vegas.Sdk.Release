/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IRubrics.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="IRubrics" />.
    /// </summary>
    public interface IRubrics : IUnique, IDeck<MemberRubric>
    {
        #region Properties

        /// <summary>
        /// Gets the BinarySize.
        /// </summary>
        int BinarySize { get; }

        /// <summary>
        /// Gets the BinarySizes.
        /// </summary>
        int[] BinarySizes { get; }

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        IFigures Figures { get; set; }

        /// <summary>
        /// Gets or sets the KeyRubrics.
        /// </summary>
        IRubrics KeyRubrics { get; set; }

        /// <summary>
        /// Gets or sets the Mappings.
        /// </summary>
        FieldMappings Mappings { get; set; }

        /// <summary>
        /// Gets the Ordinals.
        /// </summary>
        int[] Ordinals { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] GetBytes(IFigure figure);

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="seed">The seed<see cref="uint"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] GetUniqueBytes(IFigure figure, uint seed = 0);

        /// <summary>
        /// The GetUniqueKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="seed">The seed<see cref="uint"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        ulong GetUniqueKey(IFigure figure, uint seed = 0);

        /// <summary>
        /// The SetUniqueKey.
        /// </summary>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        /// <param name="seed">The seed<see cref="uint"/>.</param>
        void SetUniqueKey(IFigure figure, uint seed = 0);

        /// <summary>
        /// The Update.
        /// </summary>
        void Update();

        #endregion
    }
}
