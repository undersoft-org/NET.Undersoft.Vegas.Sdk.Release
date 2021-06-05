/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlMapper.cs
   
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
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="SqlMapper" />.
    /// </summary>
    public class SqlMapper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlMapper"/> class.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        /// <param name="keysFromDeck">The keysFromDeck<see cref="bool"/>.</param>
        /// <param name="dbTableNames">The dbTableNames<see cref="string[]"/>.</param>
        /// <param name="tablePrefix">The tablePrefix<see cref="string"/>.</param>
        public SqlMapper(IFigures table, bool keysFromDeck = false, string[] dbTableNames = null, string tablePrefix = "")
        {
            try
            {
                bool mixedMode = false;
                string tName = "", dbtName = "", prefix = tablePrefix;
                List<string> dbtNameMixList = new List<string>();
                if (dbTableNames != null)
                {
                    foreach (string dbTableName in dbTableNames)
                        if (DbHand.Schema.DataDbTables.Have(dbTableName))
                            dbtNameMixList.Add(dbTableName);
                    if (dbtNameMixList.Count > 0)
                        mixedMode = true;
                }
                IFigures t = table;
                tName = t.FigureType.Name;
                if (!mixedMode)
                {
                    if (!DbHand.Schema.DataDbTables.Have(tName))
                    {
                        if (DbHand.Schema.DataDbTables.Have(prefix + tName))
                            dbtName = prefix + tName;
                    }
                    else
                        dbtName = tName;
                    if (!string.IsNullOrEmpty(dbtName))
                    {
                        if (!keysFromDeck)
                        {
                            Album<int> colOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName) &&
                                                                                                    !DbHand.Schema.DataDbTables[dbtName].DbPrimaryKey.Select(pk => pk.ColumnName)
                                                                                                         .Contains(c.RubricName)).Select(o => o.FieldId));
                            Album<int> keyOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DbPrimaryKey.Select(pk => pk.ColumnName)
                                                                                                         .Contains(c.RubricName)).Select(o => o.FieldId));
                            FieldMapping iSqlsetMap = new FieldMapping(dbtName, keyOrdinal, colOrdinal);
                            if (t.Rubrics.Mappings == null)
                                t.Rubrics.Mappings = new FieldMappings();
                            t.Rubrics.Mappings.Add(iSqlsetMap.DbTableName, iSqlsetMap);
                        }
                        else
                        {
                            Album<int> colOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName) &&
                                                                                                          !c.IsKey).Select(o => o.FieldId));
                            Album<int> keyOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName) &&
                                                                                                          c.IsKey).Select(o => o.FieldId));
                            FieldMapping iSqlsetMap = new FieldMapping(dbtName, keyOrdinal, colOrdinal);
                            if (t.Rubrics.Mappings == null)
                                t.Rubrics.Mappings = new FieldMappings();
                            t.Rubrics.Mappings.Add(iSqlsetMap.DbTableName, iSqlsetMap);
                        }
                    }
                }
                else
                {
                    if (!keysFromDeck)
                    {
                        foreach (string dbtNameMix in dbtNameMixList)
                        {
                            dbtName = dbtNameMix;
                            Album<int> colOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName) &&
                                                                                                          !DbHand.Schema.DataDbTables[dbtName].DbPrimaryKey.Select(pk => pk.ColumnName)
                                                                                                        .Contains(c.RubricName)).Select(o => o.FieldId));
                            Album<int> keyOrdinal = new Album<int>((t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DbPrimaryKey.Select(pk => pk.ColumnName)
                                                                                                         .Contains(c.RubricName)).Select(o => o.FieldId)));
                            if (keyOrdinal.Count == 0)
                                keyOrdinal = new Album<int>(t.Rubrics.KeyRubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName)).Select(o => o.FieldId));
                            FieldMapping iSqlsetMap = new FieldMapping(dbtName, keyOrdinal, colOrdinal);
                            if (t.Rubrics.Mappings == null)
                                t.Rubrics.Mappings = new FieldMappings();
                            t.Rubrics.Mappings.Add(iSqlsetMap.DbTableName, iSqlsetMap);
                        }
                    }
                    else
                    {
                        foreach (string dbtNameMix in dbtNameMixList)
                        {
                            dbtName = dbtNameMix;
                            Album<int> colOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName) &&
                                                                                                          !c.IsKey).Select(o => o.FieldId));
                            Album<int> keyOrdinal = new Album<int>(t.Rubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName) &&
                                                                                                          c.IsKey).Select(o => o.FieldId));
                            if (keyOrdinal.Count == 0)
                                keyOrdinal = new Album<int>(t.Rubrics.KeyRubrics.AsValues().Where(c => DbHand.Schema.DataDbTables[dbtName].DataDbColumns.Have(c.RubricName)).Select(o => o.FieldId));
                            FieldMapping iSqlsetMap = new FieldMapping(dbtName, keyOrdinal, colOrdinal);
                            if (t.Rubrics.Mappings == null)
                                t.Rubrics.Mappings = new FieldMappings();
                            t.Rubrics.Mappings.Add(iSqlsetMap.DbTableName, iSqlsetMap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SqlMapperException(ex.ToString());
            }
            CardsMapped = table;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CardsMapped.
        /// </summary>
        public IFigures CardsMapped { get; set; }

        #endregion

        /// <summary>
        /// Defines the <see cref="SqlMapperException" />.
        /// </summary>
        public class SqlMapperException : Exception
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlMapperException"/> class.
            /// </summary>
            /// <param name="message">The message<see cref="string"/>.</param>
            public SqlMapperException(string message)
                : base(message)
            {
            }

            #endregion
        }
    }
}
