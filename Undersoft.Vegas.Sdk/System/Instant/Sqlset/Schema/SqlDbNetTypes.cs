/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlDbNetTypes.cs
   
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
    using System.Globalization;
    using System.Linq;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="DbNetTypes" />.
    /// </summary>
    public static class DbNetTypes
    {
        #region Fields

        private static Dictionary<Type, object> sqlNetDefaults =
            new Dictionary<Type, object>()
            {
                {typeof(int), 0},
                {typeof(string), ""},
                {typeof(DateTime), DateTime.Now},
                {typeof(bool), false},
                {typeof(float), 0},
                {typeof(decimal), 0},
                {typeof(Guid), Guid.Empty},
                {typeof(Usid), Usid.Empty},
                {typeof(Ussn), Ussn.Empty}
            };
        private static Dictionary<Type, string> sqlNetTypes =
            new Dictionary<Type, string>()
            {
                { typeof(byte)    ,   "tinyint"                   },
                { typeof(short)    ,   "smallint"                   },
                { typeof(int)      ,   "int"                        },
                { typeof(string)   ,   "nvarchar"                   },
                { typeof(DateTime) ,   "datetime"                   },
                { typeof(bool)     ,   "bit"                        },
                { typeof(double)   ,   "float"                      },
                { typeof(float)    ,   "numeric"                    },
                { typeof(decimal)  ,   "decimal"                    },
                { typeof(Guid)     ,   "uniqueidentifier"           },
                { typeof(long)     ,   "bigint"                     },
                { typeof(byte[])   ,   "varbinary"                  },
                { typeof(Usid)     ,   "bigint"                     },
                { typeof(Ussn)     ,   "varbinary"                  },
            };

        #endregion

        #region Properties

        /// <summary>
        /// Gets the SqlNetDefaults.
        /// </summary>
        public static Dictionary<Type, object> SqlNetDefaults
        {
            get { return sqlNetDefaults; }
        }

        /// <summary>
        /// Gets the SqlNetTypes.
        /// </summary>
        public static Dictionary<Type, string> SqlNetTypes
        {
            get { return sqlNetTypes; }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SqlNetType" />.
    /// </summary>
    public static class SqlNetType
    {
        #region Methods

        /// <summary>
        /// The NetTypeToSql.
        /// </summary>
        /// <param name="netType">The netType<see cref="Type"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string NetTypeToSql(Type netType)
        {
            if (DbNetTypes.SqlNetTypes.ContainsKey(netType))
                return DbNetTypes.SqlNetTypes[netType];
            else
                return "varbinary";
        }

        /// <summary>
        /// The SqlNetVal.
        /// </summary>
        /// <param name="fieldRow">The fieldRow<see cref="IFigure"/>.</param>
        /// <param name="fieldName">The fieldName<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object SqlNetVal(IFigure fieldRow, string fieldName, string prefix = "", string tableName = null)
        {
            object sqlNetVal = new object();
            try
            {
                CultureInfo cci = CultureInfo.CurrentCulture;
                string decRep = (cci.NumberFormat.NumberDecimalSeparator == ".") ? "," : ".";
                string decSep = cci.NumberFormat.NumberDecimalSeparator, _tableName = "";
                if (tableName != null) _tableName = tableName; else _tableName = fieldRow.GetType().BaseType.Name;
                if (!DbHand.Schema.DataDbTables.Have(_tableName)) _tableName = prefix + _tableName;
                if (DbHand.Schema.DataDbTables.Have(_tableName))
                {
                    Type ft = DbHand.Schema.DataDbTables[_tableName].DataDbColumns[fieldName + "#"].RubricType;

                    if (DBNull.Value != fieldRow[fieldName])
                    {
                        if (ft == typeof(decimal) || ft == typeof(float) || ft == typeof(double))
                            sqlNetVal = Convert.ChangeType(fieldRow[fieldName].ToString().Replace(decRep, decSep), ft);
                        else if (ft == typeof(string))
                        {
                            int maxLength = DbHand.Schema.DataDbTables[_tableName].DataDbColumns[fieldName + "#"].MaxLength;
                            if (fieldRow[fieldName].ToString().Length > maxLength)
                                sqlNetVal = Convert.ChangeType(fieldRow[fieldName].ToString().Substring(0, maxLength), ft);
                            else
                                sqlNetVal = Convert.ChangeType(fieldRow[fieldName], ft);
                        }
                        else if (ft == typeof(long) && fieldRow[fieldName] is Usid)
                            sqlNetVal = ((Usid)fieldRow[fieldName]).UniqueKey;
                        else if (ft == typeof(byte[]) && fieldRow[fieldName] is Ussn)
                            sqlNetVal = ((Ussn)fieldRow[fieldName]).GetBytes();
                        else
                            sqlNetVal = Convert.ChangeType(fieldRow[fieldName], ft);
                    }
                    else
                    {
                        fieldRow[fieldName] = DbNetTypes.SqlNetDefaults[ft];
                        sqlNetVal = Convert.ChangeType(fieldRow[fieldName], ft);
                    }
                }
                else
                {
                    sqlNetVal = fieldRow[fieldName];
                }
            }
            catch (Exception ex)
            {
                //  throw new Nnvalidnamespace System.Instant.Sqlset.Sq("\n" + ex + " - fieldName - " + fieldName);
            }
            return sqlNetVal;
        }

        /// <summary>
        /// The SqlTypeToNet.
        /// </summary>
        /// <param name="sqlType">The sqlType<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static Type SqlTypeToNet(string sqlType)
        {
            if (DbNetTypes.SqlNetTypes.ContainsValue(sqlType))
                return DbNetTypes.SqlNetTypes.Where(v => v.Value == sqlType).First().Key;
            else
                return typeof(object);
        }

        #endregion
    }
}
