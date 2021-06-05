/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlDbColumns.cs
   
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
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="DbColumn" />.
    /// </summary>
    public class DbColumn
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumn"/> class.
        /// </summary>
        public DbColumn()
        {
            isDBNull = false;
            isIdentity = false;
            isKey = false;
            isAutoincrement = false;
            MaxLength = -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ColumnName.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the DbColumnSize.
        /// </summary>
        public int DbColumnSize { get; set; }

        /// <summary>
        /// Gets or sets the DbOrdinal.
        /// </summary>
        public int DbOrdinal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isAutoincrement.
        /// </summary>
        public bool isAutoincrement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isDBNull.
        /// </summary>
        public bool isDBNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isIdentity.
        /// </summary>
        public bool isIdentity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isKey.
        /// </summary>
        public bool isKey { get; set; }

        /// <summary>
        /// Gets or sets the MaxLength.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the Rubrics.
        /// </summary>
        public List<MemberRubric> Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the RubricType.
        /// </summary>
        public Type RubricType { get; set; }

        #endregion
    }

    public class DbColumns
    {
        public DbColumns()
        {
            list = new List<DbColumn>();
        }
        private List<DbColumn> list;
        public List<DbColumn> List
        {
            get
            {
                return list;
            }
            set
            {
                list.AddRange(value.Where(c => !this.Have(c.ColumnName)).ToList());
            }
        }


        public void Add(DbColumn column) { if (!this.Have(column.ColumnName)) List.Add(column); }
        public void AddRange(List<DbColumn> _columns) { list.AddRange(_columns.Where(c => !this.Have(c.ColumnName)).ToList()); }
        public void Remove(DbColumn column) { list.Remove(column); }
        public void RemoveAt(int index) { list.RemoveAt(index); }
        public void Clear() { List.Clear(); }
        public bool Have(string ColumnName)
        {
            return list.Where(c => c.ColumnName == ColumnName).Any();
        }

        public DbColumn this[string ColumnName]
        {
            get
            {
                return list.Where(c => ColumnName == c.ColumnName).First();
            }
        }
        public DbColumn this[int Ordinal]
        {
            get
            {
                return list.Where(c => Ordinal == c.DbOrdinal).First();
            }
        }
        public DbColumn GetDbColumn(string ColumnName)
        {
            return list.Where(c => c.ColumnName == ColumnName).First();
        }
        public DbColumn[] GetDbColumns(List<string> ColumnNames)
        {
            return list.Where(c => ColumnNames.Contains(c.ColumnName)).ToArray();
        }
        public List<MemberRubric> GetRubrics(string ColumnNames)
        {
            return list.Where(c => ColumnNames == c.ColumnName).SelectMany(r => r.Rubrics).ToList();
        }
        public List<MemberRubric> GetRubrics(List<string> ColumnNames)
        {
            return list.Where(c => ColumnNames.Contains(c.ColumnName)).SelectMany(r => r.Rubrics).ToList();
        }
    }
}
