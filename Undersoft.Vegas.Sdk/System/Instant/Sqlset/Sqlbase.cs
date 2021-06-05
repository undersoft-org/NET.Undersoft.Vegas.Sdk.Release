using System.Sets;
using System.Data.SqlClient;

namespace System.Instant.Sqlset
{
    public class Sqlbase
    {
        private SqlIdentity identity;
        private string cnString
        {
            get => identity.ConnectionString;
            set => identity.ConnectionString = value;
        }
        private SqlConnection sqlcn;       
        private SqlUpdate update;
        private SqlInsert insert;
        private SqlDelete delete; 
        private SqlAccessor accessor;
        private SqlMutator mutator; 
        private SqlMapper mapper;

        public Sqlbase(string SqlConnectionString)
        {
            identity = new SqlIdentity();
            cnString = SqlConnectionString;
            sqlcn = new SqlConnection(cnString);
            Initialization();
        }
        public Sqlbase(SqlConnection SqlDbConnection) 
            : this(SqlDbConnection.ConnectionString)
        {
        }
        public Sqlbase(SqlIdentity sqlIdentity)          
        {
            identity = sqlIdentity;
            sqlcn = new SqlConnection(cnString);
            Initialization();
        }

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

        public IFigures        Get(string sqlQry, string tableName, IDeck<string> keyNames = null)
        {
            return accessor.Get(cnString, sqlQry, tableName, keyNames);
        }

        public IDeck<IDeck<IFigure>> Add(IFigures cards)
        {
            return mutator.Set(cnString, cards, false);
        }

        public IDeck<IDeck<IFigure>> Put(IFigures cards)
        {
            return mutator.Set(cnString, cards, true);
        }

        public IDeck<IDeck<IFigure>> Delete(IFigures cards)
        {
            return mutator.Delete(cnString, cards);
        }

        public IDeck<IDeck<IFigure>> Update(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkUpdate(cards, keysFromDeck, buildMapping, updateKeys, updateExcept, tempType);
        }

        public IDeck<IDeck<IFigure>> BulkUpdate(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            if(update == null)
                update = new SqlUpdate(sqlcn);
            return update.BulkUpdate(cards, keysFromDeck, buildMapping, updateKeys, updateExcept, tempType);
        }
        public IDeck<IDeck<IFigure>> BatchUpdate(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null)
        {
            if (update == null)
                update = new SqlUpdate(sqlcn);
            return update.BatchUpdate(cards, keysFromDeck, buildMapping, updateKeys, updateExcept);
        }
        public int                   SimpleUpdate(IFigures cards, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null)
        {
            if (update == null)
                update = new SqlUpdate(sqlcn);
            return update.SimpleUpdate(cards, buildMapping, updateKeys, updateExcept);
        }

        public IDeck<IDeck<IFigure>> Insert(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkInsert(cards, keysFromDeck, buildMapping, tempType);
        }

        public IDeck<IDeck<IFigure>> BulkInsert(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.Insert(cards, keysFromDeck, buildMapping, false, null, tempType);
        }
        public IDeck<IDeck<IFigure>> BatchInsert(IFigures cards, bool buildMapping)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.BatchInsert(cards, buildMapping);
        }
        public int                   SimpleInsert(IFigures cards, bool buildMapping)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.SimpleInsert(cards, buildMapping);
        }
        public int                   SimpleInsert(IFigures cards)
        {
            if (insert == null)
                insert = new SqlInsert(sqlcn);
            return insert.SimpleInsert(cards);
        }

        public IDeck<IDeck<IFigure>> Delete(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkDelete(cards, keysFromDeck, buildMapping, tempType);
        }

        public IDeck<IDeck<IFigure>> BulkDelete(IFigures cards, bool keysFromDeck = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.Delete(cards, keysFromDeck, buildMapping, tempType);
        }
        public IDeck<IDeck<IFigure>> BatchDelete(IFigures cards, bool buildMapping)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.BatchDelete(cards, buildMapping);
        }
        public int SimpleDelete(IFigures cards, bool buildMapping)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.SimpleDelete(cards, buildMapping);
        }
        public int SimpleDelete(IFigures cards)
        {
            if (delete == null)
                delete = new SqlDelete(sqlcn);
            return delete.SimpleDelete(cards);
        }

        public int Execute(string query)
        {
            SqlCommand cmd = sqlcn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
        }

        public IFigures Mapper(IFigures cards, bool keysFromDeck = false, string[] dbTableNames = null, string tablePrefix = "")
        {
            mapper = new SqlMapper(cards, keysFromDeck, dbTableNames, tablePrefix);
            return mapper.CardsMapped;
        }
    }
 
    public class SqlException : Exception
    {
        public SqlException(string message)
            : base(message)
        {

        }
    }
}
