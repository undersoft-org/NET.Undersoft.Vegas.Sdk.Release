/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlMutator.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset
{
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="SqlMutator" />.
    /// </summary>
    public class SqlMutator
    {
        #region Fields

        private Sqlbase sqaf;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlMutator"/> class.
        /// </summary>
        public SqlMutator()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlMutator"/> class.
        /// </summary>
        /// <param name="insql">The insql<see cref="Sqlbase"/>.</param>
        public SqlMutator(Sqlbase insql)
        {
            sqaf = insql;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="SqlConnectString">The SqlConnectString<see cref="string"/>.</param>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Delete(string SqlConnectString, IFigures cards)
        {
            try
            {
                if (sqaf == null)
                    sqaf = new Sqlbase(SqlConnectString);
                try
                {
                    bool buildmap = true;
                    if (cards.Count > 0)
                    {
                        BulkPrepareType prepareType = BulkPrepareType.Drop;
                        return sqaf.Delete(cards, true, buildmap, prepareType);
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="SqlConnectString">The SqlConnectString<see cref="string"/>.</param>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="Renew">The Renew<see cref="bool"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Set(string SqlConnectString, IFigures cards, bool Renew)
        {
            try
            {
                if (sqaf == null)
                    sqaf = new Sqlbase(SqlConnectString);
                try
                {
                    bool buildmap = true;
                    if (cards.Count > 0)
                    {
                        BulkPrepareType prepareType = BulkPrepareType.Drop;

                        if (Renew)
                            prepareType = BulkPrepareType.Trunc;

                        var ds = sqaf.Update(cards, true, buildmap, true, null, prepareType);
                        if (ds != null)
                        {
                            IFigures im = (IFigures)Summon.New(cards.GetType());
                            im.Rubrics = cards.Rubrics;
                            im.FigureType = cards.FigureType;
                            im.FigureSize = cards.FigureSize;
                            im.Add(ds["Failed"].AsValues());
                            return sqaf.Insert(im, true, false, prepareType);
                        }
                        else
                            return null;
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
