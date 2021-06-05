/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlInsert.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Sets;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="SqlInsert" />.
    /// </summary>
    public class SqlInsert
    {
        #region Fields

        private SqlConnection _cn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInsert"/> class.
        /// </summary>
        /// <param name="cn">The cn<see cref="SqlConnection"/>.</param>
        public SqlInsert(SqlConnection cn)
        {
            _cn = cn;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInsert"/> class.
        /// </summary>
        /// <param name="cnstring">The cnstring<see cref="string"/>.</param>
        public SqlInsert(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The BatchInsert.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchInsert(IFigures table, bool buildMapping, int batchSize = 1000)
        {
            try
            {
                IFigures tab = table;
                IList<FieldMapping> nMaps = new List<FieldMapping>();
                SqlAdapter afad = new SqlAdapter(_cn);
                StringBuilder sb = new StringBuilder();
                IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();
                sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                int count = 0;
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        var bIFigures = afad.ExecuteInsert(sb.ToString(), tab);
                        if (nSet.Count == 0)
                            nSet = bIFigures;
                        else
                            foreach (Album<IFigure> its in bIFigures.AsValues())
                            {
                                if (nSet.Contains(its))
                                {
                                    nSet[its].Put(its.AsValues());
                                }
                                else
                                    nSet.Add(its);
                            }
                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                var rIFigures = afad.ExecuteInsert(sb.ToString(), tab);

                if (nSet.Count == 0)
                    nSet = rIFigures;
                else
                    foreach (IDeck<IFigure> its in rIFigures.AsValues())
                    {
                        if (nSet.Contains(its))
                        {
                            nSet[its].Put(its.AsValues());
                        }
                        else
                            nSet.Add(its);
                    }

                return nSet;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        /// <summary>
        /// The BatchInsert.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchInsert(IFigures table, int batchSize = 1000)
        {
            try
            {
                IFigures tab = table;
                IList<FieldMapping> nMaps = new List<FieldMapping>();
                SqlAdapter afad = new SqlAdapter(_cn);
                StringBuilder sb = new StringBuilder();
                IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();
                sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                int count = 0;
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        var bIFigures = afad.ExecuteInsert(sb.ToString(), tab);
                        if (nSet.Count == 0)
                            nSet = bIFigures;
                        else
                            foreach (Album<IFigure> its in bIFigures.AsValues())
                            {
                                if (nSet.Contains(its))
                                {
                                    nSet[its].Put(its.AsValues());
                                }
                                else
                                    nSet.Add(its);
                            }
                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                var rIFigures = afad.ExecuteInsert(sb.ToString(), tab);

                if (nSet.Count == 0)
                    nSet = rIFigures;
                else
                    foreach (IDeck<IFigure> its in rIFigures.AsValues())
                    {
                        if (nSet.Contains(its))
                        {
                            nSet[its].Put(its.AsValues());
                        }
                        else
                            nSet.Add(its);
                    }

                return nSet;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        /// <summary>
        /// The BatchInsertQuery.
        /// </summary>
        /// <param name="card">The card<see cref="IFigure"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="columns">The columns<see cref="MemberRubric[]"/>.</param>
        /// <param name="keys">The keys<see cref="MemberRubric[]"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public StringBuilder BatchInsertQuery(IFigure card, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sbCols = new StringBuilder(), sbVals = new StringBuilder(), sbQry = new StringBuilder();
            string tName = tableName;
            IFigure ir = card;
            object[] ia = ir.ValueArray;
            MemberRubric[] ic = columns;
            MemberRubric[] ik = keys;

            sbCols.AppendLine(@"  /* ---- DATA BANK START ITEM CMD ------ */  ");
            sbCols.Append("INSERT INTO " + tableName + " (");
            sbVals.Append(@") OUTPUT inserted.* VALUES (");
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < columns.Length; i++)
            {


                if (columns[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;
                if (ia[columns[i].FieldId] != DBNull.Value && !columns[i].IsIdentity)
                {
                    if (c > 0)
                        delim = ",";
                    sbCols.AppendFormat(CultureInfo.InvariantCulture, @"{0}[{1}]", delim,
                                                                                   columns[i].RubricName
                                                                                   );
                    sbVals.AppendFormat(CultureInfo.InvariantCulture, @"{0} {1}{2}{1}", delim,
                                                                                        (columns[i].RubricType == typeof(string) ||
                                                                                        columns[i].RubricType == typeof(DateTime)) ? "'" : "",
                                                                                        (columns[i].RubricType != typeof(string)) ?
                                                                                        Convert.ChangeType(ia[columns[i].FieldId], columns[i].RubricType) :
                                                                                        ia[columns[i].FieldId].ToString().Replace("'", "''")
                                                                                        );
                    c++;
                }
            }

            if (DbHand.Schema.DataDbTables[tableName].DataDbColumns.Have("updated") && !isUpdateCol)
            {
                sbCols.AppendFormat(CultureInfo.InvariantCulture, ", [updated]", DateTime.Now);
                sbVals.AppendFormat(CultureInfo.InvariantCulture, ", '{0}'", DateTime.Now);
            }
            if (columns.Length > 0)
                delim = ",";
            else
                delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {

                if (ia[keys[i].FieldId] != DBNull.Value && !keys[i].IsIdentity)
                {
                    if (c > 0)
                        delim = ",";
                    sbCols.AppendFormat(CultureInfo.InvariantCulture, @"{0}[{1}]", delim,
                                                                                    keys[i].RubricName
                                                                                    );
                    sbVals.AppendFormat(CultureInfo.InvariantCulture, @"{0} {1}{2}{1}", delim,
                                                                                        (keys[i].RubricType == typeof(string) ||
                                                                                        keys[i].RubricType == typeof(DateTime)) ? "'" : "",
                                                                                        (keys[i].RubricType != typeof(string)) ?
                                                                                        Convert.ChangeType(ia[keys[i].FieldId], keys[i].RubricType) :
                                                                                        ia[keys[i].FieldId].ToString().Replace("'", "''")
                                                                                        );
                    c++;
                }
            }
            sbQry.Append(sbCols.ToString() + sbVals.ToString() + ") ");
            sbQry.AppendLine(@"  /* ----  SQL BANK END ITEM CMD ------ */  ");
            return sbQry;
        }

        /// <summary>
        /// The BulkInsert.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeckis">The keysFromDeckis<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BulkInsert(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            try
            {
                IFigures tab = table;
                if (tab.Any())
                {
                    IList<FieldMapping> nMaps = new List<FieldMapping>();
                    if (buildMapping)
                    {
                        SqlMapper imapper = new SqlMapper(tab, keysFromDeckis);
                    }
                    nMaps = tab.Rubrics.Mappings;
                    string dbName = _cn.Database;
                    SqlAdapter afad = new SqlAdapter(_cn);
                    afad.DataBulk(tab, tab.FigureType.Name, tempType, BulkDbType.TempDB);
                    _cn.ChangeDatabase(dbName);
                    IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                    foreach (FieldMapping nMap in nMaps)
                    {
                        sb.AppendLine(@"  /* ----  TABLE BULK START CMD ------ */  ");

                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        if (updateExcept != null)
                        {
                            ic = ic.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                            ik = ik.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                        }

                        string qry = BulkInsertQuery(dbName, tab.FigureType.Name, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        sb.AppendLine(@"  /* ----  TABLE BULK END CMD ------ */  ");
                    }
                    sb.AppendLine(@"  /* ----  SQL BANK END CMD ------ */  ");

                    IDeck<IDeck<IFigure>> bIFigures = afad.ExecuteInsert(sb.ToString(), tab, true);


                    if (nSet.Count == 0)
                        nSet = bIFigures;
                    else
                        foreach (IDeck<IFigure> its in bIFigures.AsValues())
                        {
                            if (nSet.Contains(its))
                            {
                                nSet[its].Put(its.AsValues());
                            }
                            else
                                nSet.Add(its);
                        }
                    sb.Clear();

                    return nSet;
                }
                else
                    return null;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        /// <summary>
        /// The BulkInsertQuery.
        /// </summary>
        /// <param name="DBName">The DBName<see cref="string"/>.</param>
        /// <param name="buforName">The buforName<see cref="string"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="columns">The columns<see cref="MemberRubric[]"/>.</param>
        /// <param name="keys">The keys<see cref="MemberRubric[]"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public StringBuilder BulkInsertQuery(string DBName, string buforName, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbv = new StringBuilder();
            string bName = buforName;
            string tName = tableName;
            MemberRubric[] rubrics = keys.Concat(columns).ToArray();
            string dbName = DBName;
            sb.AppendLine(@"  /* ---- DATA BANK START ITEM CMD ------ */");
            sb.AppendFormat(@"INSERT INTO [{0}].[dbo].[" + tName + "] (", dbName);
            sbv.Append(@"SELECT ");
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < rubrics.Length; i++)
            {

                if (rubrics[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;

                if (c > 0)
                    delim = ",";
                sb.AppendFormat(CultureInfo.InvariantCulture, @"{0}[{1}]", delim, rubrics[i].RubricName);
                sbv.AppendFormat(CultureInfo.InvariantCulture, @"{0}[S].[{1}]", delim, rubrics[i].RubricName);
                c++;
            }
            sb.AppendFormat(CultureInfo.InvariantCulture, @") OUTPUT inserted.* {0}", sbv.ToString());
            sb.AppendFormat(" FROM [tempdb].[dbo].[{0}] AS S ", bName, dbName, tName);
            sb.AppendLine("");
            sb.AppendLine(@"  /* ----  SQL BANK END ITEM CMD ------ */  ");
            sbv.Clear();
            return sb;
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeckis">The keysFromDeckis<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Insert(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkInsert(table, keysFromDeckis, buildMapping, updateKeys, updateExcept, tempType);
        }

        /// <summary>
        /// The SimpleInsert.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleInsert(IFigures table, bool buildMapping, int batchSize = 1000)
        {
            try
            {
                IFigures tab = table;
                IList<FieldMapping> nMaps = new List<FieldMapping>();
                SqlAdapter afad = new SqlAdapter(_cn);
                StringBuilder sb = new StringBuilder();
                int intSqlset = 0;
                sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                int count = 0;
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        ;
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");

                        intSqlset += afad.ExecuteInsert(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                intSqlset += afad.ExecuteInsert(sb.ToString());
                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        /// <summary>
        /// The SimpleInsert.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleInsert(IFigures table, int batchSize = 1000)
        {
            try
            {
                IFigures tab = table;
                IList<FieldMapping> nMaps = new List<FieldMapping>();
                SqlAdapter afad = new SqlAdapter(_cn);
                StringBuilder sb = new StringBuilder();
                int intSqlset = 0;
                sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                int count = 0;
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        intSqlset += afad.ExecuteInsert(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                intSqlset += afad.ExecuteInsert(sb.ToString());

                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SqlInsertException" />.
    /// </summary>
    public class SqlInsertException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInsertException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SqlInsertException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
