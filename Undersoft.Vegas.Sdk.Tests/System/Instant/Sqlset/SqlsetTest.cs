/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Sqlset.SqlsetTest.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Sqlset.Tests
{
    using System.Sets;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="SqlsetTest" />.
    /// </summary>
    public class SqlsetTest
    {
        #region Fields

        private Sqlbase bank;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlsetTest"/> class.
        /// </summary>
        public SqlsetTest()
        {
            SqlIdentity identity = new SqlIdentity()
            {
                Name = "UndersoftStockAdmin",
                UserId = "sa",
                Password = "$t0kkk3",
                Database = "UndersoftStock",
                Server = "127.0.0.1",
                Security = true,
                Provider = SqlProvider.MsSql,
                Port = 0
            };
            bank = new Sqlbase(identity);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Sqlset_Accessor_AddFigures_Test.
        /// </summary>
        [Fact]
        public void Sqlset_Accessor_AddFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity",
                                                 "StockTradingActivity",
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            IDeck<IDeck<IFigure>> result = bank.Add(im);
        }

        /// <summary>
        /// The Sqlset_Accessor_DeleteFigures_Test.
        /// </summary>
        [Fact]
        public void Sqlset_Accessor_DeleteFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity",
                                                 "StockTradingActivity",
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            IDeck<IDeck<IFigure>> result = bank.Delete(im);
        }

        /// <summary>
        /// The Sqlset_Accessor_GetFigures_Test.
        /// </summary>
        [Fact]
        public void Sqlset_Accessor_GetFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity",
                                                 "StockTradingActivity",
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });
            IFigures figures = im;
        }

        /// <summary>
        /// The Sqlset_Accessor_PutFigures_Test.
        /// </summary>
        [Fact]
        public void Sqlset_Accessor_PutFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity",
                                                 "StockTradingActivity",
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            IDeck<IDeck<IFigure>> result = bank.Put(im);
        }

        #endregion
    }
}
