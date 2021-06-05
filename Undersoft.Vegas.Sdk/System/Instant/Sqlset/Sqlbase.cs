/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.Sqlbase.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset
{
    using System.Data.SqlClient;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="Sqlbase" />.
    /// </summary>
    public class Sqlbase
    {
        #region Fields

        private SqlAccessor accessor;
        private SqlDelete delete;
        private SqlIdentity identity;
        private SqlInsert insert;
        private SqlMapper mapper;
        private SqlMutator mutator;
        private SqlConnection sqlcn;
        private SqlUpdate update;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sqlbase"/> class.
        /// </summary>
        /// <param name="SqlDbConnection">The SqlDbConnection<see cref="SqlConnection"/>.</param>
        public Sqlbase(SqlConnection SqlDbConnection)
            : this(SqlDbConnection.ConnectionString)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Sqlbase"/> class.
        /// </summary>
        /// <param name="sqlIdentity">The sqlIdentity<see cref="SqlIdentity"/>.</param>
        public Sqlbase(SqlIdentity sqlIdentity)
        {
            identity = sqlIdentity;
            sqlcn = new SqlConnection(cnString);
            Initialization();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Sqlbase"/> class.
        /// </summary>
        /// <param name="SqlConnectionString">The SqlConnectionString<see cref="string"/>.</param>
        public Sqlbase(string SqlConnectionString)
        {
            identity = new SqlIdentity();
            cnString = SqlConnectionString;
            sqlcn = new SqlConnection(cnString);
            Initialization();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cnString.
        /// </summary>
        private string cnString { get => identity.ConnectionString; set => identity.ConnectionString = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Add(IFigures cards)
        {
            return mutator.Set(cnString, cards, false);
        }

        /// <summary>
        /// The BatchDelete.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchDelete(IFigures cards, bool buildMapping)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.BatchDelete(cards, buildMapping);
        }

        /// <summary>
        /// The BatchInsert.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchInsert(IFigures cards, bool buildMapping)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.BatchInsert(cards, buildMapping);
        }

        /// <summary>
        /// The BatchUpdate.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BatchUpdate(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null)
        {
            if (update == null)
                update = new SqlUpdate(sqlcn);
            return update.BatchUpdate(cards, keysFromDeck, buildMapping, updateKeys, updateExcept);
        }

        /// <summary>
        /// The BulkDelete.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BulkDelete(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.Delete(cards, keysFromDeck, buildMapping, tempType);
        }

        /// <summary>
        /// The BulkInsert.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BulkInsert(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.Insert(cards, keysFromDeck, buildMapping, false, null, tempType);
        }

        /// <summary>
        /// The BulkUpdate.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> BulkUpdate(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            if (update == null)
                update = new SqlUpdate(sqlcn);
            return update.BulkUpdate(cards, keysFromDeck, buildMapping, updateKeys, updateExcept, tempType);
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Delete(IFigures cards)
        {
            return mutator.Delete(cnString, cards);
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Delete(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkDelete(cards, keysFromDeck, buildMapping, tempType);
        }

        /// <summary>
        /// The Execute.
        /// </summary>
        /// <param name="query">The query<see cref="string"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Execute(string query)
        {
            SqlCommand cmd = sqlcn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="sqlQry">The sqlQry<see cref="string"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="keyNames">The keyNames<see cref="IDeck{string}"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public IFigures Get(string sqlQry, string tableName, IDeck<string> keyNames = null)
        {
            return accessor.Get(cnString, sqlQry, tableName, keyNames);
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Insert(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkInsert(cards, keysFromDeck, buildMapping, tempType);
        }

        /// <summary>
        /// The Mapper.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="dbTableNames">The dbTableNames<see cref="string[]"/>.</param>
        /// <param name="tablePrefix">The tablePrefix<see cref="string"/>.</param>
        /// <returns>The <see cref="IFigures"/>.</returns>
        public IFigures Mapper(IFigures cards, bool keysFromDeck = false, string[] dbTableNames = null, string tablePrefix = "")
        {
            mapper = new SqlMapper(cards, keysFromDeck, dbTableNames, tablePrefix);
            return mapper.CardsMapped;
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Put(IFigures cards)
        {
            return mutator.Set(cnString, cards, true);
        }

        /// <summary>
        /// The SimpleDelete.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleDelete(IFigures cards)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.SimpleDelete(cards);
        }

        /// <summary>
        /// The SimpleDelete.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleDelete(IFigures cards, bool buildMapping)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.SimpleDelete(cards, buildMapping);
        }

        /// <summary>
        /// The SimpleInsert.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleInsert(IFigures cards)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.SimpleInsert(cards);
        }

        /// <summary>
        /// The SimpleInsert.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleInsert(IFigures cards, bool buildMapping)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.SimpleInsert(cards, buildMapping);
        }

        /// <summary>
        /// The SimpleUpdate.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SimpleUpdate(IFigures cards, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null)
        {
            if (update == null)
                update = new SqlUpdate(sqlcn);
            return update.SimpleUpdate(cards, buildMapping, updateKeys, updateExcept);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="cards">The cards<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="buildMapping">The buildMapping<see cref="bool"/>.</param>
        /// <param name="updateKeys">The updateKeys<see cref="bool"/>.</param>
        /// <param name="updateExcept">The updateExcept<see cref="string[]"/>.</param>
        /// <param name="tempType">The tempType<see cref="BulkPrepareType"/>.</param>
        /// <returns>The <see cref="IDeck{IDeck{IFigure}}"/>.</returns>
        public IDeck<IDeck<IFigure>> Update(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkUpdate(cards, keysFromDeck, buildMapping, updateKeys, updateExcept, tempType);
        }

        /// <summary>
        /// The Initialization.
        /// </summary>
        private void Initialization()
        {
            string dbName = sqlcn.Database;
            SqlSchemaBuild SchemaBuild = new SqlSchemaBuild(sqlcn);
            SchemaBuild.SchemaPrepare(BuildDbSchemaType.Schema);
            sqlcn.ChangeDatabase("tempdb");
            SchemaBuild.SchemaPrepare(BuildDbSchemaType.Temp);
            sqlcn.ChangeDatabase(dbName);
            accessor = new SqlAccessor();
            mutator = new SqlMutator(this);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SqlException" />.
    /// </summary>
    public class SqlException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SqlException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
