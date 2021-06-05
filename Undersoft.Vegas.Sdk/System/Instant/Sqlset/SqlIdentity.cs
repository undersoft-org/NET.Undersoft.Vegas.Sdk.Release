/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlIdentity.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    #region Enums

    public enum SqlProvider
    {
        MsSql,
        MySql,
        Postgres,
        Oracle,
        SqlLite
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="SqlIdentity" />.
    /// </summary>
    [Serializable]
    public class SqlIdentity
    {
        #region Fields

        public string AuthId;
        public string Database;
        public int Id;
        public string Name;
        public string Password;
        public int Port;
        public SqlProvider Provider;
        public bool Security;
        public string Server;
        public string UserId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlIdentity"/> class.
        /// </summary>
        public SqlIdentity()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlIdentity"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString<see cref="string"/>.</param>
        public SqlIdentity(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ConnectionString.
        /// </summary>
        public string ConnectionString
        {
            get
            {

                string cn = string.Format("server={0}{1};Persist Security Info={2};password={3};User ID={4};database={5}",
                                           Server,
                                           (Port != 0) ? ":" + Port.ToString() : "",
                                           Security.ToString(),
                                           Password,
                                           UserId,
                                           Database);
                return cn;
            }
            set
            {
                string cn = value;
                string[] opts = cn.Split(';');
                foreach (string opt in opts)
                {
                    string name = opt.Split('=')[0].ToLower();
                    string val = opt.Split('=')[1];
                    switch (name)
                    {
                        case "server":
                            Server = val;
                            break;
                        case "persist security info":
                            Security = Boolean.Parse(val);
                            break;
                        case "password":
                            Password = val;
                            break;
                        case "user id":
                            UserId = val;
                            break;
                        case "database":
                            Database = val;
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
