using System.Collections.Generic;
using System.Uniques;
using System.Multemic;
using System.Data;
using System.Linq;

namespace System.Instants.Sqlbank
{
    public interface IDataReader<T> where T : class
    {
        InstantFigures InjectRead(string tableName,
                       IDeck<string> keyNames = null);

        Album<Album<IFigure>> UpdateRead(IFigures toUpdateCards);

        Album<Album<IFigure>> InsertRead(IFigures toInsertCards);
    }

    public class SqlReader<T> : IDataReader<T> where T : class
    {
        private IDataReader dr;

        public SqlReader(IDataReader _dr)
        {
            dr = _dr;
        }

        public InstantFigures InjectRead(string tableName,
                                    IDeck<string> keyNames = null)
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

            InstantFigure rt = new InstantFigure(columns.ToArray(), tableName);
            InstantFigures deck = new InstantFigures(rt, tableName + "_Collection");
            IFigures tab = deck.Collection;
         
            bool takeDbKeys = false;
            if (keyNames != null)
                if (keyNames.Count > 0)         
                    foreach (var k in keyNames)
                        tab.Rubrics.KeyRubrics.Add(tab.Rubrics[k]);                
                else
                    takeDbKeys = true;
            else
                takeDbKeys = true;

            if (takeDbKeys &&
                 DbHand.Schema != null &&
                    DbHand.Schema.DataDbTables.List.Count > 0)
                    {
                        List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
                        MemberRubric[] pKeys = tab.Rubrics.AsValues()
                                .Where(c => dbtabs.SelectMany(t =>
                                 t.GetKeyForDataTable.Select(d => d.RubricName))
                                .Contains(c.RubricName)).ToArray();
                        if (pKeys.Length > 0)
                        {
                            foreach (var k in pKeys)
                                tab.Rubrics.KeyRubrics.Add(k);
                        }
                    }


            if (dr.Read())
            {
                int columnsCount = dr.FieldCount;
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = tab.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();

                do
                {
                    IFigure figure = tab.NewFigure();

                    dr.GetValues(itemArray);

                    figure.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    figure.SystemSerialCode = new Ussn(keyOrder.Select(ko => itemArray[ko]).ToArray());

                    tab.Add(figure);
                }
                while (dr.Read());
                itemArray = null;
            }
            dr.Dispose();
            return deck;
        }            

        public Album<Album<IFigure>> UpdateRead(IFigures toUpdateCards)
        {
            //InstantFigure instfig = new InstantFigure(toUpdateCards.Rubrics, "UpdateFigure");
            //new InstantFigures(instfig, "Update");

            IFigures deck = toUpdateCards;                      
            Album<IFigure> updatedList = new Album<IFigure>();
            Album<IFigure> toInsertList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics != null ? deck.Rubrics.Count : 0;

                if (i == 0 && columnsCount == 0)
                {
                    InstantFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics, true);
                    deck = tab.Collection;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();
                while (dr.Read())
                {
                    if ((columnsCount - 1) != (int)(dr.FieldCount / 2))
                    {
                        InstantFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics, true);
                        deck = tab.Collection;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();
                    }

                    dr.GetValues(itemArray);
               
                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    row.SystemSerialCode = new Ussn(keyOrder.Select(ko => itemArray[ko]).ToArray());

                    updatedList.Add(row);
                }

                foreach (IFigure ir in toUpdateCards)
                    if (!updatedList.ContainsKey(ir))
                        toInsertList.Add(ir);

            } while (dr.NextResult());

            Album<Album<IFigure>> iSet = new Album<Album<IFigure>>();

            iSet.Add("Failed", toInsertList);

            iSet.Add("Updated", updatedList);

            return iSet;
        }

        public Album<Album<IFigure>> InsertRead(IFigures toInsertCards)
        {
            IFigures deck = toInsertCards;
            Album<IFigure> insertedList = new Album<IFigure>();
            Album<IFigure> brokenList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics.Count;

                if (i == 0 && deck.Rubrics.Count == 0)
                {
                    InstantFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                    deck = tab.Collection;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();
                while (dr.Read())
                {
                    if ((columnsCount - 1) != dr.FieldCount)
                    {
                        InstantFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                        deck = tab.Collection;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FigureFieldId).ToArray();
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().GetDefault() : a).ToArray();

                    row.SystemSerialCode = new Ussn(keyOrder.Select(ko => itemArray[ko]).ToArray());

                    insertedList.Add(row);
                }

                foreach (IFigure ir in toInsertCards)
                   if (!insertedList.ContainsKey(ir))
                        brokenList.Add(ir);

            } while (dr.NextResult());         

            Album<Album<IFigure>> iSet = new Album<Album<IFigure>>();

            iSet.Add("Failed", brokenList);

            iSet.Add("Inserted", insertedList);

            return iSet;
        }

        public InstantFigures DeckFromSchema(DataTable schema,
                                        IDeck<MemberRubric> operColumns,
                                        bool insAndDel = false)
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

            InstantFigure rt = new InstantFigure(_columns.ToArray(), "SchemaFigure");
            InstantFigures tab = new InstantFigures(rt, "Schema");
            IFigures deck = tab.Collection;
          
            List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
            MemberRubric[] pKeys = columns.Where(c => dbtabs.SelectMany(t => t.GetKeyForDataTable.Select(d => d.RubricName)).Contains(c.RubricName) && operColumns.Select(o => o.RubricName).Contains(c.RubricName)).ToArray();
            if (pKeys.Length > 0)
                deck.Rubrics.KeyRubrics = new MemberRubrics(pKeys);

            return tab;
        }

    }
   
}



