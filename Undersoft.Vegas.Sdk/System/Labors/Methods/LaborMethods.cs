/*************************************************
   Copyright (c) 2021 Undersoft

   LaborMethods.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Instant;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="LaborMethods" />.
    /// </summary>
    public class LaborMethods : Catalog<IDeputy>
    {
        #region Methods

        /// <summary>
        /// The EmptyBaseDeck.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{IDeputy}[]"/>.</returns>
        public override ICard<IDeputy>[] EmptyBaseDeck(int size)
        {
            return new LaborMethod[size];
        }

        /// <summary>
        /// The EmptyCard.
        /// </summary>
        /// <returns>The <see cref="ICard{IDeputy}"/>.</returns>
        public override ICard<IDeputy> EmptyCard()
        {
            return new LaborMethod();
        }

        /// <summary>
        /// The EmptyCardTable.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <returns>The <see cref="ICard{IDeputy}[]"/>.</returns>
        public override ICard<IDeputy>[] EmptyCardTable(int size)
        {
            return new LaborMethod[size];
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        /// <returns>The <see cref="ICard{IDeputy}"/>.</returns>
        public override ICard<IDeputy> NewCard(IDeputy value)
        {
            return new LaborMethod(value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        /// <returns>The <see cref="ICard{IDeputy}"/>.</returns>
        public override ICard<IDeputy> NewCard(object key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }

        /// <summary>
        /// The NewCard.
        /// </summary>
        /// <param name="key">The key<see cref="long"/>.</param>
        /// <param name="value">The value<see cref="IDeputy"/>.</param>
        /// <returns>The <see cref="ICard{IDeputy}"/>.</returns>
        public override ICard<IDeputy> NewCard(ulong key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }

        #endregion
    }
}
