using System.Collections.Generic;
using System.Uniques;
using System.Multemic;
using System.Data;
using System.Linq;

namespace System.Instant.Sqlbank
{
    public interface IDataReader<T> where T : class
    {
        IFigures InjectRead(string tableName, IDeck<string> keyNames = null);

        IDeck<IDeck<IFigure>> UpdateRead(IFigures toUpdateCards);

        IDeck<IDeck<IFigure>> InsertRead(IFigures toInsertCards);

        IDeck<IDeck<IFigure>> DeleteRead(IFigures toInsertCards);
    }

    public class SqlReader<T> : IDataReader<T> where T : class
    {
        private IDataReader dr;

        public SqlReader(IDataReader _dr)
        {
            dr = _dr;
        }

        public IFigures        InjectRead(string tableName, IDeck<string> keyNames = null)
        {
            DataTable schema = dr.GetSchemaTable();
            List<MemberRubric> columns = new List<MemberRubric>(schema.Rows.Cast<DataRow>().AsEnumerable().AsQueryable()
                                                .Where(n => n["ColumnName"].ToString() != "SystemSerialCode").Select(c =>
                                                new MemberRubric(new FieldRubric(Type.GetType(c["DataType"].ToString()),
                                                                        c["ColumnName"].ToString(),
                                                                        Convert.ToInt32(c["ColumnSize"]),
                                                                        Convert.ToInt32(c["ColumnOrdinal"]))
                                                {
                                                    RubricSize = Convert.ToInt32(c["ColumnSize"])
                                                })
                                                {
                                                    FigureFieldId = Convert.ToInt32(c["ColumnOrdinal"]),
                                                    IsIdentity = Convert.ToBoolean(c["IsIdentity"]),
                                                    IsAutoincrement = Convert.ToBoolean(c["IsAutoincrement"]),
                                                    IsDBNull = Convert.ToBoolean(c["AllowDBNull"]),                                                                               
                                                }
                                                ).ToList());

         
         
            bool takeDbKeys = false;
            if (keyNames != null)
                if (keyNames.Count > 0)
                    foreach (var k in keyNames)
                    {
                        columns.Where(c => c.Name == k).Select(ck => ck.IsKey = true).ToArray();
                    }
                else
                    takeDbKeys = true;
            else
                takeDbKeys = true;

            if (takeDbKeys &&
                 DbHand.Schema != null &&
                    DbHand.Schema.DataDbTables.List.Count > 0)
            {
                List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
                MemberRubric[] pKeys = columns
                        .Where(c => dbtabs.SelectMany(t =>
                         t.GetKeyForDataTable.Select(d => d.RubricName))
                        .Contains(c.RubricName)).ToArray();
                if (pKeys.Length > 0)
                {
                    pKeys.Select(pk => pk.IsKey = true);
                }
            }

            Figure rt = new Figure(columns.ToArray(), tableName);
            Figures deck = new Figures(rt, tableName + "_Figures");
            IFigures tab = deck.Generate();

            if (dr.Read())
            {
                int columnsCount = dr.FieldCount;
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = tab.Rubrics.KeyRubrics.Ordinals;

                do
                {
                    IFigure figure = tab.NewFigure();

                    dr.GetValues(itemArray);

                    figure.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    //figure.SystemSerialCode = new Ussn(keyOrder.Select(ko => itemArray[ko]).ToArray());

                    tab.Add(figure);
                }
                while (dr.Read());
                itemArray = null;
            }
            dr.Dispose();
            return tab;
        }            

        public IDeck<IDeck<IFigure>> UpdateRead(IFigures toUpdateCards)
        {
            IFigures deck = toUpdateCards;                      
            IDeck<IFigure> updatedList = new Album<IFigure>();
            IDeck<IFigure> toInsertList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics != null ? deck.Rubrics.Count : 0;

                if (i == 0 && columnsCount == 0)
                {
                    IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics, true);
                    deck = tab;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = deck.Rubrics.KeyRubrics.Ordinals;
                while (dr.Read())
                {
                    if ((columnsCount - 1) != (int)(dr.FieldCount / 2))
                    {
                        IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics, true);
                        deck = tab;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();
                    }

                    dr.GetValues(itemArray);
               
                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    //row.SystemSerialCode = new Ussn(keyOrder.Select(ko => itemArray[ko]).ToArray());

                    updatedList.Add(row);
                }

                foreach (IFigure ir in toUpdateCards)
                    if (!updatedList.ContainsKey(ir))
                        toInsertList.Add(ir);

            } while (dr.NextResult());

            IDeck<IDeck<IFigure>> iSet = new Album<IDeck<IFigure>>();

            iSet.Add("Failed", toInsertList);

            iSet.Add("Updated", updatedList);

            return iSet;
        }

        public IDeck<IDeck<IFigure>> InsertRead(IFigures toInsertCards)
        {
            IFigures deck = toInsertCards;
            IDeck<IFigure> insertedList = new Album<IFigure>();
            IDeck<IFigure> brokenList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics.Count;

                if (i == 0 && deck.Rubrics.Count == 0)
                {
                    IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                    deck = tab;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyIndexes = deck.Rubrics.KeyRubrics.Ordinals;
                while (dr.Read())
                {
                    if ((columnsCount - 1) != dr.FieldCount)
                    {
                        IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                        deck = tab;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyIndexes = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    //row.SystemSerialCode = new Ussn(keyIndexes.Select(ko => itemArray[ko]).ToArray());

                    insertedList.Add(row);
                }

                foreach (IFigure ir in toInsertCards)
                   if (!insertedList.ContainsKey(ir))
                        brokenList.Add(ir);

            } while (dr.NextResult());         

            IDeck<IDeck<IFigure>> iSet = new Album<IDeck<IFigure>>();

            iSet.Add("Failed", brokenList);

            iSet.Add("Inserted", insertedList);

            return iSet;
        }

        public IDeck<IDeck<IFigure>> DeleteRead(IFigures toDeleteCards)
        {
            IFigures deck = toDeleteCards;
            IDeck<IFigure> deletedList = new Album<IFigure>();
            IDeck<IFigure> brokenList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics.Count;

                if (i == 0 && deck.Rubrics.Count == 0)
                {
                    IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                    deck = tab;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyIndexes = deck.Rubrics.KeyRubrics.Ordinals;
                while (dr.Read())
                {
                    if ((columnsCount - 1) != dr.FieldCount)
                    {
                        IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                        deck = tab;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyIndexes = deck.Rubrics.KeyRubrics.Ordinals;
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    //row.SystemSerialCode = new Ussn(keyIndexes.Select(ko => itemArray[ko]).ToArray());

                    deletedList.Add(row);
                }

                foreach (IFigure ir in toDeleteCards)
                    if (!deletedList.ContainsKey(ir))
                        brokenList.Add(ir);

            } while (dr.NextResult());

            IDeck<IDeck<IFigure>> iSet = new Album<IDeck<IFigure>>();

            iSet.Add("Failed", brokenList);

            iSet.Add("Deleted", deletedList);

            return iSet;
        }

        public IFigures        DeckFromSchema(DataTable schema, IDeck<MemberRubric> operColumns, bool insAndDel = false)
        {

            List<MemberRubric> columns = new List<MemberRubric>(schema.Rows.Cast<DataRow>().AsEnumerable().AsQueryable()
                                                .Select(c => new MemberRubric(new FieldRubric(Type.GetType(c["DataType"].ToString()),
                                                                        c["ColumnName"].ToString(),
                                                                        Convert.ToInt32(c["ColumnSize"]),
                                                                        Convert.ToInt32(c["ColumnOrdinal"]))
                                                {
                                                    RubricSize = Convert.ToInt32(c["ColumnSize"])
                                                })
                                                {
                                                    FigureFieldId = Convert.ToInt32(c["ColumnOrdinal"]),
                                                    IsIdentity = Convert.ToBoolean(c["IsIdentity"]),
                                                    IsAutoincrement = Convert.ToBoolean(c["IsAutoincrement"]),
                                                    IsDBNull = Convert.ToBoolean(c["AllowDBNull"])

                                                }).ToList());
            List<MemberRubric> _columns = new List<MemberRubric>();

            if (insAndDel)
                for (int i = 0; i < (int)(columns.Count / 2); i++)
                    _columns.Add(columns[i]);
            else
                _columns.AddRange(columns);

            Figure rt = new Figure(_columns.ToArray(), "SchemaFigure");
            Figures tab = new Figures(rt, "Schema");
            IFigures deck = tab.Generate();
          
            List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
            MemberRubric[] pKeys = columns.Where(c => dbtabs.SelectMany(t => t.GetKeyForDataTable.Select(d => d.RubricName)).Contains(c.RubricName) && operColumns.Select(o => o.RubricName).Contains(c.RubricName)).ToArray();
            if (pKeys.Length > 0)
                deck.Rubrics.KeyRubrics = new MemberRubrics(pKeys);
            deck.Rubrics.Update();
            return deck;
        }

    }
   
}



