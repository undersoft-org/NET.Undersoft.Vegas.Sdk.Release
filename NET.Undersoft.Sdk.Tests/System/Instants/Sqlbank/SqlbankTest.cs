using System;
using System.Multemic;
using System.Instants.Sqlbank;
using System.Instants;
using Xunit;

namespace System.Instants.Sqlbank.Tests
{
    public class SqlbankTest
    {
        private InstantSql bank;

        public SqlbankTest()
        {
            SqlIdentity identity = new SqlIdentity()
            {
                Name = "UnireconStockAdmin",
                UserId = "sa",
                Password = "$t0kkk3",
                Database = "UnireconStock",
                Server = "127.0.0.1",
                Security = true,
                Provider = SqlProvider.MsSql,
                Port = 0
            };
            bank = new InstantSql(identity);
        }


        [Fact]
        public void Sqlbank_Accessor_GetInstantFigures_Test()
        {
            InstantFigures im = bank.GetInstant("SELECT * From StockTradingActivity", 
                                                 "StockTradingActivity", 
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

        }

        [Fact]
        public void Sqlbank_Accessor_SetInstantFigures_Test()
        {
            InstantFigures im = bank.GetInstant("SELECT * From StockTradingActivity", 
                                                 "StockTradingActivity", 
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            var result = bank.SaveInstant(im.Collection, false);

        }
    }
}
