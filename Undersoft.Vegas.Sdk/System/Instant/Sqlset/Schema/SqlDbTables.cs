/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlDbTables.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="DbTable" />.
    /// </summary>
    public class DbTable
    {
        #region Fields

        private DbColumn[] dbPrimaryKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable"/> class.
        /// </summary>
        public DbTable()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable"/> class.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        public DbTable(string tableName)
        {
            TableName = tableName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the DataDbColumns.
        /// </summary>
        public DbColumns DataDbColumns { get; set; }

        /// <summary>
        /// Gets or sets the DbPrimaryKey.
        /// </summary>
        public DbColumn[] DbPrimaryKey
        {
            get
            {
                return dbPrimaryKey;
            }
            set
            {
                dbPrimaryKey = value;
            }
        }

        /// <summary>
        /// Gets the GetColumnsForDataTable.
        /// </summary>
        public List<MemberRubric> GetColumnsForDataTable
        {
            get
            {
                return DataDbColumns.List.Select(c =>
                    new MemberRubric(new FieldRubric(c.RubricType, c.ColumnName, c.DbColumnSize, c.DbOrdinal) { RubricSize = c.DbColumnSize })
                    {
                        FieldId = c.DbOrdinal - 1,
                        IsAutoincrement = c.isAutoincrement,
                        IsDBNull = c.isDBNull,
                        IsIdentity = c.isIdentity
                    }).ToList();
            }
        }

        /// <summary>
        /// Gets the GetKeyForDataTable.
        /// </summary>
        public MemberRubric[] GetKeyForDataTable
        {
            get
            {
                return DbPrimaryKey.Select(c =>
                                                 new MemberRubric(new FieldRubric(c.RubricType, c.ColumnName, c.DbColumnSize, c.DbOrdinal) { RubricSize = c.DbColumnSize })
                                                 {
                                                     FieldId = c.DbOrdinal - 1,
                                                     IsAutoincrement = c.isAutoincrement,
                                                     IsDBNull = c.isDBNull,
                                                     IsIdentity = c.isIdentity
                                                 }).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the TableName.
        /// </summary>
        public string TableName { get; set; }

        #endregion
    }

    public class DbTables
    {
        private object holder = new object();
        public object Holder
        {
            get
            {
                return holder;
            }
        }

        public DbTables()
        {
            tables = new List<DbTable>();
        }
        private List<DbTable> tables;
        public List<DbTable> List
        {
            get
            {
                return
                    tables;
            }
            set
            {
                tables.AddRange(value.Where(c => !this.Have(c.TableName)).ToList());
            }
        }

        public void Add(DbTable table) { if (!this.Have(table.TableName)) { tables.Add(table); } }
        public void AddRange(List<DbTable> _tables) { tables.AddRange(_tables.Where(c => !this.Have(c.TableName)).ToList()); }
        public void Remove(DbTable table) { tables.Remove(table); }
        public void RemoveAt(int index) { tables.RemoveAt(index); }
        public bool Have(string TableName)
        {
            return tables.Where(t => t.TableName == TableName).Any();
        }
        public void Clear() { tables.Clear(); }

        public DbTable this[string TableName]
        {
            get
            {
                return tables.Where(c => TableName == c.TableName).First();
            }
        }
        public DbTable this[int TableIndex]
        {
            get
            {
                return tables[TableIndex];
            }
        }
        public DbTable GetDbTable(string TableName)
        {
            return tables.Where(c => TableName == c.TableName).First();
        }
        public List<DbTable> GetDbTables(List<string> TableNames)
        {
            return tables.Where(c => TableNames.Contains(c.TableName)).ToList();
        }

    }
}
