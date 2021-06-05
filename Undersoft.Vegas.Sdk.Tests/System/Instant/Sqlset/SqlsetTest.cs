using System.Sets;
using Xunit;

namespace System.Instant.Sqlset.Tests
{
    public class SqlsetTest
    {
        private Sqlbase bank;

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

        [Fact]
        public void Sqlset_Accessor_GetFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity", 
                                                 "StockTradingActivity", 
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });
            IFigures figures = im;            
        }

        [Fact]
        public void Sqlset_Accessor_AddFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity", 
                                                 "StockTradingActivity", 
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            IDeck<IDeck<IFigure>> result = bank.Add(im);
        }

        [Fact]
        public void Sqlset_Accessor_PutFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity",
                                                 "StockTradingActivity",
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            IDeck<IDeck<IFigure>> result = bank.Put(im);
        }

        [Fact]
        public void Sqlset_Accessor_DeleteFigures_Test()
        {
            IFigures im = bank.Get("SELECT * From StockTradingActivity",
                                                 "StockTradingActivity",
                                                 new Deck<string>() { "permno", "market_name", "trading_date" });

            IDeck<IDeck<IFigure>> result = bank.Delete(im);
        }
    }
}
