/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlDelete.cs
   
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
    /// Defines the <see cref="SqlDeleteException" />.
    /// </summary>
    public class SqlDeleteException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDeleteException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SqlDeleteException(string message)
            : base(message)
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SqlDelete" />.
    /// </summary>
    internal class SqlDelete
    {
        #region Fields

        private SqlConnection _cn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDelete"/> class.
        /// </summary>
        /// <param name="cn">The cn<see cref="SqlConnection"/>.</param>
        public SqlDelete(SqlConnection cn)
        {
            _cn = cn;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDelete"/> class.
        /// </summary>
        /// <param name="cnstring">The cnstring<see cref="string"/>.</param>
        public SqlDelete(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The BatchDelete.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchDelete(IFigures table, bool buildMapping, int batchSize = 1000)
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
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        var bIFigures = afad.ExecuteDelete(sb.ToString(), tab);
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

                var rIFigures = afad.ExecuteDelete(sb.ToString(), tab);

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
                throw new SqlDeleteException(ex.ToString());
            }
        }

        /// <summary>
        /// The BatchDelete.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchDelete(IFigures table, int batchSize = 1000)
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
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        var bIFigures = afad.ExecuteDelete(sb.ToString(), tab);
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

                var rIFigures = afad.ExecuteDelete(sb.ToString(), tab);

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
                throw new SqlDeleteException(ex.ToString());
            }
        }

        /// <summary>
        /// The BatchDeleteQuery.
        /// </summary>
        /// <param name="card">The card<see cref="IFigure"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="keys">The keys<see cref="MemberRubric[]"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public StringBuilder BatchDeleteQuery(IFigure card, string tableName, MemberRubric[] keys)
        {
            StringBuilder sb = new StringBuilder();
            string tName = tableName;
            IFigure ir = card;
            object[] ia = ir.ValueArray;
            MemberRubric[] ik = keys;

            sb.AppendLine(@"  /* ---- DATA BANK START ITEM CMD ------ */  ");
            sb.Append("DELETE FROM " + tableName + " OUTPUT deleted.* ");
            string delim = "";
            int c = 0;

            delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {
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
        /// The BulkDelete.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeckis">The keysFromDeckis<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BulkDelete(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
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
                    SqlAdapter adapter = new SqlAdapter(_cn);
                    adapter.DataBulk(tab, tab.FigureType.Name, tempType, BulkDbType.TempDB);
                    _cn.ChangeDatabase(dbName);
                    IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                    foreach (FieldMapping nMap in nMaps)
                    {
                        sb.AppendLine(@"  /* ----  TABLE BULK START CMD ------ */  ");

                        string qry = BulkDeleteQuery(dbName, tab.FigureType.Name, nMap.DbTableName).ToString();
                        sb.Append(qry);

                        sb.AppendLine(@"  /* ----  TABLE BULK END CMD ------ */  ");
                    }
                    sb.AppendLine(@"  /* ----  SQL BANK END CMD ------ */  ");

                    IDeck<IDeck<IFigure>> bIFigures = adapter.ExecuteDelete(sb.ToString(), tab, true);


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
                throw new SqlDeleteException(ex.ToString());
            }
        }

        /// <summary>
        /// The BulkDeleteQuery.
        /// </summary>
        /// <param name="DBName">The DBName<see cref="string"/>.</param>
        /// <param name="buforName">The buforName<see cref="string"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        public StringBuilder BulkDeleteQuery(string DBName, string buforName, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            string bName = buforName;
            string tName = tableName;
            string dbName = DBName;
            sb.AppendLine(@"  /* ---- DATA BANK START ITEM CMD ------ */");
            sb.AppendFormat(@"DELETE FROM [{0}].[dbo].[" + tName + "] OUTPUT deleted.* WHERE EXISTS (", dbName);
            sb.AppendFormat("SELECT * FROM [tempdb].[dbo].[{0}] AS S)", bName);
            sb.AppendLine("");
            sb.AppendLine(@"  /* ----  SQL BANK END ITEM CMD ------ */  ");
            return sb;
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeckis">The keysFromDeckis<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Delete(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkDelete(table, keysFromDeckis, buildMapping, tempType);
        }

        /// <summary>
        /// The SimpleDelete.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleDelete(IFigures table, bool buildMapping, int batchSize = 1000)
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
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");

                        intSqlset += afad.ExecuteDelete(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                intSqlset += afad.ExecuteDelete(sb.ToString());
                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlDeleteException(ex.ToString());
            }
        }

        /// <summary>
        /// The SimpleDelete.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleDelete(IFigures table, int batchSize = 1000)
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
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"  /* ----  DATA BATCH END CMD ------ */  ");
                        intSqlset += afad.ExecuteDelete(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"  /* ----  SQL BANK START CMD ------ */  ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"  /* ----  DATA BANK END CMD ------ */  ");

                intSqlset += afad.ExecuteDelete(sb.ToString());

                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlDeleteException(ex.ToString());
            }
        }

        #endregion
    }
}
