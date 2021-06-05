using System.Multemic;

namespace System.Instant.Sqlbank
{
    public class SqlMutator
    {
        private Sqlbase sqaf;

        public SqlMutator()
        {
        }
        public SqlMutator(Sqlbase insql)
        {
            sqaf = insql;
        }

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
    }
}
