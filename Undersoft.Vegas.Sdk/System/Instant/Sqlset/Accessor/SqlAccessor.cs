/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlAccessor.cs
   
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
    using System.Data.SqlClient;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="SqlAccessor" />.
    /// </summary>
    public class SqlAccessor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAccessor"/> class.
        /// </summary>
        public SqlAccessor()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="sqlConnectString">The sqlConnectString<see cref="string"/>.</param>
        /// <param name="sqlQry">The sqlQry<see cref="string"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="keyNames">The keyNames<see cref="IDeck{string}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public IFigures Get(string sqlConnectString,
                                          string sqlQry,
                                          string tableName,
                                          IDeck<string> keyNames)
        {
            try
            {
                if (DbHand.Schema == null || DbHand.Schema.DbTables.Count == 0)
                {
                    Sqlbase sqb = new Sqlbase(sqlConnectString);
                }
                SqlAdapter sqa = new SqlAdapter(sqlConnectString);

                try
                {
                    return sqa.ExecuteInject(sqlQry, tableName, keyNames);
                }
                catch (Exception ex)
                {
                    throw new SqlException(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The GetSqlDataTable.
        /// </summary>
        /// <param name="parameters">The parameters<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetSqlDataTable(object parameters)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>((Dictionary<string, object>)parameters);
                string sqlQry = param["SqlQuery"].ToString();
                string sqlConnectString = param["ConnectionString"].ToString();

                DataTable Table = new DataTable();
                SqlConnection sqlcn = new SqlConnection(sqlConnectString);
                sqlcn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlQry, sqlcn);
                adapter.Fill(Table);
                return Table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetSqlDataTable.
        /// </summary>
        /// <param name="cmd">The cmd<see cref="SqlCommand"/>.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public DataTable GetSqlDataTable(SqlCommand cmd)
        {
            try
            {

                DataTable Table = new DataTable();
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(Table);
                return Table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// The GetSqlDataTable.
        /// </summary>
        /// <param name="qry">The qry<see cref="string"/>.</param>
        /// <param name="cn">The cn<see cref="SqlConnection"/>.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public DataTable GetSqlDataTable(string qry, SqlConnection cn)
        {
            try
            {
                DataTable Table = new DataTable();
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(qry, cn);
                adapter.Fill(Table);
                return Table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
