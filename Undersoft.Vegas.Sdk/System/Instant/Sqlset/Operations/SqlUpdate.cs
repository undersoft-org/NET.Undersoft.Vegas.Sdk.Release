/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlUpdate.cs
   
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
    /// Defines the <see cref="SqlUpdate" />.
    /// </summary>
    public class SqlUpdate
    {
        #region Fields

        private SqlConnection _cn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUpdate"/> class.
        /// </summary>
        /// <param name="cn">The cn<see cref="SqlConnection"/>.</param>
        public SqlUpdate(SqlConnection cn)
        {
            _cn = cn;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUpdate"/> class.
        /// </summary>
        /// <param name="cnstring">The cnstring<see cref="string"/>.</param>
        public SqlUpdate(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The BatchUpdate.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchUpdate(IFigures table, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, int batchSize = 250)
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
                foreach (IFigure ir in table)
                {
                    if (ir.GetType().DeclaringType != tab.FigureType)
                    {
                        if (buildMapping)
                        {
                            SqlMapper imapper = new SqlMapper(tab, keysFromDeck);
                        }
                        nMaps = tab.Rubrics.Mappings;
                    }

                    foreach (FieldMapping nMap in nMaps)
                    {
                        IDeck<int> co = nMap.ColumnOrdinal;
                        IDeck<int> ko = nMap.KeyOrdinal;
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();
                        if (updateExcept != null)
                        {
                            ic = ic.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                            ik = ik.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                        }

                        string qry = BatchUpdateQuery(ir, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        IDeck<IDeck<IFigure>> bIFigures = afad.ExecuteUpdate(sb.ToString(), tab);
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
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                IDeck<IDeck<IFigure>> rIFigures = afad.ExecuteUpdate(sb.ToString(), tab, true);

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
                throw new SqlUpdateException(ex.ToString());
            }
        }

        /// <summary>
        /// The BatchUpdateQuery.
        /// </summary>
        /// <param name="card">The card<see cref="IFigure"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="columns">The columns<see cref="MemberRubric[]"/>.</param>
        /// <param name="keys">The keys<see cref="MemberRubric[]"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public StringBuilder BatchUpdateQuery(IFigure card, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sb = new StringBuilder();
            string tName = tableName;
            IFigure ir = card;
            object[] ia = ir.ValueArray;
            MemberRubric[] ic = columns;
            MemberRubric[] ik = keys;

            sb.AppendLine(@"  /* ---- DATA BANK START ITEM CMD ------ */  ");
            sb.Append("UPDATE " + tName + " SET ");
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;
                if (ia[columns[i].FieldId] != DBNull.Value)
                {
                    if (c > 0)
                        delim = ",";
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                                    @"{0}[{1}] = {2}{3}{2}",
                                    delim,
                                    columns[i].RubricName,
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
                sb.AppendFormat(CultureInfo.InvariantCulture, ", [updated] = '{0}'", DateTime.Now);

            if (updateKeys)
            {
                if (columns.Length > 0)
                    delim = ",";
                else
                    delim = "";
                c = 0;
                for (int i = 0; i < keys.Length; i++)
                {

                    if (ia[keys[i].FieldId] != DBNull.Value)
                    {
                        if (c > 0)
                            delim = ",";
                        sb.AppendFormat(CultureInfo.InvariantCulture,
                                        @"{0}[{1}] = {2}{3}{2}",
                                        delim,
                                        keys[i].RubricName,
                                        (keys[i].RubricType == typeof(string) ||
                                        keys[i].RubricType == typeof(DateTime)) ? "'" : "",
                                        (keys[i].RubricType != typeof(string)) ?
                                        Convert.ChangeType(ia[keys[i].FieldId], keys[i].RubricType) :
                                        ia[keys[i].FieldId].ToString().Replace("'", "''")
                                        );
                        c++;
                    }
                }
            }
            sb.AppendLine(" OUTPUT inserted.*, deleted.* ");
            delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i > 0)
                    delim = " AND ";
                else
                    delim = " WHERE ";

                if (ia[keys[i].FieldId] != DBNull.Value)
                {
                    if (c > 0)
                        delim = " AND ";
                    else
                        delim = " WHERE ";

                    sb.AppendFormat(CultureInfo.InvariantCulture,
                                    @"{0} [{1}] = {2}{3}{2}",
                                    delim,
                                    keys[i].RubricName,
                                    (keys[i].RubricType == typeof(string) ||
                                    keys[i].RubricType == typeof(DateTime)) ? "'" : "",
                                    (ia[keys[i].FieldId] != DBNull.Value) ?
                                    (keys[i].RubricType != typeof(string)) ?
                                    Convert.ChangeType(ia[keys[i].FieldId], keys[i].RubricType) :
                                    ia[keys[i].FieldId].ToString().Replace("'", "''") : ""
                                    );
                    c++;
                }
            }
            sb.AppendLine("");
            sb.AppendLine(@"  /* ----  SQL BANK END ITEM CMD ------ */  ");
            return sb;
        }

        /// <summary>
        /// The BulkUpdate.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BulkUpdate(IFigures table, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            try
            {
                IFigures tab = table;
                if (tab.Count > 0)
                {
                    IList<FieldMapping> nMaps = new List<FieldMapping>();
                    if (buildMapping)
                    {
                        SqlMapper imapper = new SqlMapper(tab, keysFromDeck);
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

                        string qry = BulkUpdateQuery(dbName, tab.FigureType.Name, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        sb.AppendLine(@"  /* ----  TABLE BULK END CMD ------ */  ");
                    }
                    sb.AppendLine(@"  /* ----  SQL BANK END CMD ------ */  ");

                    var bIFigures = afad.ExecuteUpdate(sb.ToString(), tab, true);

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
                throw new SqlUpdateException(ex.ToString());
            }
        }

        /// <summary>
        /// The BulkUpdateQuery.
        /// </summary>
        /// <param name="DBName">The DBName<see cref="string"/>.</param>
        /// <param name="buforName">The buforName<see cref="string"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="columns">The columns<see cref="MemberRubric[]"/>.</param>
        /// <param name="keys">The keys<see cref="MemberRubric[]"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public StringBuilder BulkUpdateQuery(string DBName, string buforName, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sb = new StringBuilder();
            string bName = buforName;
            string tName = tableName;
            MemberRubric[] ic = columns;
            MemberRubric[] ik = keys;
            string dbName = DBName;
            sb.AppendLine(@"  /* ---- DATA BANK START ITEM CMD ------ */  ");
            sb.AppendFormat(@"UPDATE [{0}].[dbo].[" + tName + "] SET ", dbName);
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < columns.Length; i++)
            {

                if (columns[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;

                if (c > 0)
                    delim = ",";
                sb.AppendFormat(CultureInfo.InvariantCulture,
                                @"{0}[{1}] =[S].[{2}]",
                                delim,
                                columns[i].RubricName,
                                columns[i].RubricName
                                );
                c++;
            }

            if (updateKeys)
            {
                if (columns.Length > 0)
                    delim = ",";
                else
                    delim = "";
                c = 0;
                for (int i = 0; i < keys.Length; i++)
                {
                    if (c > 0)
                        delim = ",";
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                                    @"{0}[{1}] = [S].[{2}]",
                                    delim,
                                    keys[i].RubricName,
                                    keys[i].RubricName
                                    );
                    c++;
                }
            }
            sb.AppendLine(" OUTPUT inserted.*, deleted.* ");
            sb.AppendFormat(" FROM [tempdb].[dbo].[{0}] AS S INNER JOIN [{1}].[dbo].[{2}] AS T ", bName, dbName, tName);
            delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i > 0)
                    delim = " AND ";
                else
                    delim = " ON ";

                sb.AppendFormat(CultureInfo.InvariantCulture,
                                @"{0} [T].[{1}] = [S].[{2}]",
                                delim,
                                keys[i].RubricName,
                                keys[i].RubricName
                                );
                c++;
            }
            sb.AppendLine("");
            sb.AppendLine(@"  /* ----  SQL BANK END ITEM CMD ------ */  ");
            return sb;
        }

        /// <summary>
        /// The SimpleUpdate.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleUpdate(IFigures table, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, int batchSize = 500)
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
                foreach (IFigure ir in table)
                {
                    if (ir.GetType().DeclaringType != tab.FigureType)
                    {
                        if (buildMapping)
                        {
                            SqlMapper imapper = new SqlMapper(tab);
                        }
                        nMaps = tab.Rubrics.Mappings;
                    }

                    foreach (FieldMapping nMap in nMaps)
                    {
                        IDeck<int> co = nMap.ColumnOrdinal;
                        IDeck<int> ko = nMap.KeyOrdinal;
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();
                        if (updateExcept != null)
                        {
                            ic = ic.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                            ik = ik.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                        }

                        string qry = BatchUpdateQuery(ir, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH EDataD CMD ------ */  ");
                        intSqlset += afad.ExecuteUpdate(sb.ToString());
                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                intSqlset += afad.ExecuteUpdate(sb.ToString());
                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlUpdateException(ex.ToString());
            }
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Update(IFigures table, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkUpdate(table, keysFromDeck, buildMapping, updateKeys, updateExcept, tempType);
        }

        #endregion

        /// <summary>
        /// Defines the <see cref="SqlUpdateException" />.
        /// </summary>
        public class SqlUpdateException : Exception
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlUpdateException"/> class.
            /// </summary>
            /// <param name="message">The message<see cref="string"/>.</param>
            public SqlUpdateException(string message)
                : base(message)
            {
            }

            #endregion
        }
    }
}
