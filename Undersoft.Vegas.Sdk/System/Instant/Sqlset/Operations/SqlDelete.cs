using System;
using System.Collections.Generic;
using System.Linq;
using System.Sets;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace System.Instant.Sqlset
{
    class SqlDelete
    {
        private SqlConnection _cn;

        public SqlDelete(SqlConnection cn)
        {
            _cn = cn;
        }
        public SqlDelete(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        public IDeck<IDeck<IFigure>> Delete(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkDelete(table, keysFromDeckis, buildMapping, tempType);
        }

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
    }
    public class SqlDeleteException : Exception
    {
        public SqlDeleteException(string message)
            : base(message)
        {

        }
    }
}
