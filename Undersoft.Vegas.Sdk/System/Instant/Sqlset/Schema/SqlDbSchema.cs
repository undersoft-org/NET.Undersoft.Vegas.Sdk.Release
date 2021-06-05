/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlDbSchema.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    /// <summary>
    /// Defines the <see cref="DataDbSchema" />.
    /// </summary>
    public class DataDbSchema
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataDbSchema"/> class.
        /// </summary>
        /// <param name="sqlDbConnection">The sqlDbConnection<see cref="SqlConnection"/>.</param>
        public DataDbSchema(SqlConnection sqlDbConnection)
        {
            DataDbTables = new DbTables();
            DbConfig = new DbConfig(sqlDbConnection.ConnectionString);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataDbSchema"/> class.
        /// </summary>
        /// <param name="dbConnectionString">The dbConnectionString<see cref="string"/>.</param>
        public DataDbSchema(string dbConnectionString)
        {
            DataDbTables = new DbTables();
            DbConfig = new DbConfig(dbConnectionString);
            SqlDbConnection = new SqlConnection(dbConnectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the DataDbTables.
        /// </summary>
        public DbTables DataDbTables { get; set; }

        /// <summary>
        /// Gets or sets the DbConfig.
        /// </summary>
        public DbConfig DbConfig { get; set; }

        /// <summary>
        /// Gets or sets the DbName.
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// Gets or sets the DbTables.
        /// </summary>
        public List<DbTable> DbTables
        {
            get { return DataDbTables.List; }
            set { DataDbTables.List = value; }
        }

        /// <summary>
        /// Gets or sets the SqlDbConnection.
        /// </summary>
        public SqlConnection SqlDbConnection { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="DbConfig" />.
    /// </summary>
    public class DbConfig
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConfig"/> class.
        /// </summary>
        public DbConfig()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbConfig"/> class.
        /// </summary>
        /// <param name="_User">The _User<see cref="string"/>.</param>
        /// <param name="_Password">The _Password<see cref="string"/>.</param>
        /// <param name="_Source">The _Source<see cref="string"/>.</param>
        /// <param name="_Catalog">The _Catalog<see cref="string"/>.</param>
        /// <param name="_Provider">The _Provider<see cref="string"/>.</param>
        public DbConfig(string _User, string _Password, string _Source, string _Catalog, string _Provider = "SQLNCLI11")
        {
            User = _User;
            Password = _Password;
            Provider = _Provider;
            Source = _Source;
            Album = _Catalog;
            DbConnectionString = string.Format("Provider={0};Data Source = {1}; Persist Security Info=True;Password={2};User ID = {3}; Initial Album = {4}", Provider, Source, Password, User, Album);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbConfig"/> class.
        /// </summary>
        /// <param name="dbConnectionString">The dbConnectionString<see cref="string"/>.</param>
        public DbConfig(string dbConnectionString)
        {
            DbConnectionString = dbConnectionString;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Album.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the DbConnectionString.
        /// </summary>
        public string DbConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public string User { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="DbHand" />.
    /// </summary>
    public static class DbHand
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Schema.
        /// </summary>
        public static DataDbSchema Schema { get; set; }

        /// <summary>
        /// Gets or sets the Temp.
        /// </summary>
        public static DataDbSchema Temp { get; set; }

        #endregion
    }
}
