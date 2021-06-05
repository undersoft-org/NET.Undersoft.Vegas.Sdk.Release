/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlReader.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Sets;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="IDataReader{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public interface IDataReader<T> where T : class
    {
        #region Methods

        /// <summary>
        /// The DeleteRead.
        /// </summary>
        /// <param name="toInsertCards">The toInsertCards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        IDeck<IDeck<IFigure>> DeleteRead(IFigures toInsertCards);

        /// <summary>
        /// The InjectRead.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="keyNames">The keyNames<see cref="IDeck{string}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        IFigures InjectRead(string tableName, IDeck<string> keyNames = null);

        /// <summary>
        /// The InsertRead.
        /// </summary>
        /// <param name="toInsertCards">The toInsertCards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        IDeck<IDeck<IFigure>> InsertRead(IFigures toInsertCards);

        /// <summary>
        /// The UpdateRead.
        /// </summary>
        /// <param name="toUpdateCards">The toUpdateCards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        IDeck<IDeck<IFigure>> UpdateRead(IFigures toUpdateCards);

        #endregion
    }
    /// <summary>
    /// Defines the <see cref="SqlReader{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class SqlReader<T> : IDataReader<T> where T : class
    {
        #region Fields

        private IDataReader dr;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlReader{T}"/> class.
        /// </summary>
        /// <param name="_dr">The _dr<see cref="IDataReader"/>.</param>
        public SqlReader(IDataReader _dr)
        {
            dr = _dr;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The DeckFromSchema.
        /// </summary>
        /// <param name="schema">The schema<see cref="DataTable"/>.</param>
        /// <param name="operColumns">The operColumns<see cref="IDeck{MemberRubric}"/>.</param>
        /// <param name="insAndDel">The insAndDel<see cref="bool"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public IFigures DeckFromSchema(DataTable schema, IDeck<MemberRubric> operColumns, bool insAndDel = false)
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
                                                    FieldId = Convert.ToInt32(c["ColumnOrdinal"]),
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
            IFigures deck = tab.Combine();

            List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
            MemberRubric[] pKeys = columns.Where(c => dbtabs.SelectMany(t => t.GetKeyForDataTable.Select(d => d.RubricName)).Contains(c.RubricName) && operColumns.Select(o => o.RubricName).Contains(c.RubricName)).ToArray();
            if (pKeys.Length > 0)
                deck.Rubrics.KeyRubrics = new MemberRubrics(pKeys);
            deck.Rubrics.Update();
            return deck;
        }

        /// <summary>
        /// The DeleteRead.
        /// </summary>
        /// <param name="toDeleteCards">The toDeleteCards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
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

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    //row.SerialCode = new Ussn(keyIndexes.Select(ko => itemArray[ko]).ToArray());

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

        /// <summary>
        /// The InjectRead.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="keyNames">The keyNames<see cref="IDeck{string}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public IFigures InjectRead(string tableName, IDeck<string> keyNames = null)
        {
            DataTable schema = dr.GetSchemaTable();
            List<MemberRubric> columns = new List<MemberRubric>(schema.Rows.Cast<DataRow>().AsEnumerable().AsQueryable()
                                                .Where(n => n["ColumnName"].ToString() != "SerialCode").Select(c =>
                                                new MemberRubric(new FieldRubric(Type.GetType(c["DataType"].ToString()),
                                                                        c["ColumnName"].ToString(),
                                                                        Convert.ToInt32(c["ColumnSize"]),
                                                                        Convert.ToInt32(c["ColumnOrdinal"]))
                                                {
                                                    RubricSize = Convert.ToInt32(c["ColumnSize"])
                                                })
                                                {
                                                    FieldId = Convert.ToInt32(c["ColumnOrdinal"]),
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
            IFigures tab = deck.Combine();

            if (dr.Read())
            {
                int columnsCount = dr.FieldCount;
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = tab.Rubrics.KeyRubrics.Ordinals;

                do
                {
                    IFigure figure = tab.NewFigure();

                    dr.GetValues(itemArray);

                    figure.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    figure.UniqueKey = keyOrder.Select(i => itemArray[i]).ToArray().UniqueKey64();

                    tab.Put(figure);
                }
                while (dr.Read());
                itemArray = null;
            }
            dr.Dispose();
            return tab;
        }

        /// <summary>
        /// The InsertRead.
        /// </summary>
        /// <param name="toInsertCards">The toInsertCards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
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
                        keyIndexes = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FieldId).ToArray();
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    //row.SerialCode = new Ussn(keyIndexes.Select(ko => itemArray[ko]).ToArray());

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

        /// <summary>
        /// The UpdateRead.
        /// </summary>
        /// <param name="toUpdateCards">The toUpdateCards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
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
                        keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FieldId).ToArray();
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    //row.SerialCode = new Ussn(keyOrder.Select(ko => itemArray[ko]).ToArray());

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

        #endregion
    }
}
