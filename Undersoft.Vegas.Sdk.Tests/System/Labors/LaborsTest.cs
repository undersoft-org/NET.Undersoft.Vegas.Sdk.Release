/*************************************************
   Copyright (c) 2021 Undersoft

   System.Labors.LaborsTest.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Labors.Tests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Instant;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="ComputeCurrency" />.
    /// </summary>
    public class ComputeCurrency
    {
        #region Methods

        /// <summary>
        /// The Compute.
        /// </summary>
        /// <param name="currency1">The currency1<see cref="string"/>.</param>
        /// <param name="rate1">The rate1<see cref="double"/>.</param>
        /// <param name="currency2">The currency2<see cref="string"/>.</param>
        /// <param name="rate2">The rate2<see cref="double"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Compute(string currency1, double rate1, string currency2, double rate2)
        {
            try
            {
                double _rate1 = rate1;
                double _rate2 = rate2;
                double result = _rate2 / _rate1;
                Debug.WriteLine("Result : " + result.ToString());

                return new object[] { _rate1, _rate2, result };
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FirstCurrency" />.
    /// </summary>
    public class FirstCurrency
    {
        #region Methods

        /// <summary>
        /// The GetFirstCurrency.
        /// </summary>
        /// <param name="currency">The currency<see cref="string"/>.</param>
        /// <param name="days">The days<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetFirstCurrency(string currency, int days)
        {
            //Thread.Sleep(2000);

            NBPSource kurKraju = new NBPSource(days);

            try
            {
                double rate = kurKraju.LoadRate(currency);
                Debug.WriteLine("Rate 1 : " + currency + "  days past: " + days.ToString() + " amount : " + rate.ToString("#.####"));

                return new object[] { currency, rate };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="LaborsTest" />.
    /// </summary>
    public class LaborsTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LaborsTest"/> class.
        /// </summary>
        public LaborsTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Labors_Inductive_MultiThreading_Integration_Test.
        /// </summary>
        [Fact]
        public void Labors_Inductive_MultiThreading_Integration_Test()
        {

            Laboratory lab = new Laboratory();

            Subject download = lab.Expanse(new Deputy(typeof(FirstCurrency).FullName, nameof(FirstCurrency.GetFirstCurrency)), "Download");
            download.Add(new Deputy(typeof(SecondCurrency).FullName, nameof(SecondCurrency.GetSecondCurrency)));
            download.Visor.CreateLaborers(8); /// e.g. 8 workers/consumers for downloading 2 different currency rates

            Subject compute = lab.Expanse(new Deputy(typeof(ComputeCurrency).FullName, nameof(ComputeCurrency.Compute)), "Compute");
            compute.Add(new Deputy(typeof(PresentResult).FullName, nameof(PresentResult.Present)));
            compute.Visor.CreateLaborers(4); /// e.g. 4 workers/consumers for computing and presenting results


            List<Labor> ComputeResultRequirements = new List<Labor>()
            {
                download[nameof(FirstCurrency.GetFirstCurrency)],
                download[nameof(SecondCurrency.GetSecondCurrency)]
            };

            download[nameof(FirstCurrency.GetFirstCurrency)]
                .Laborer
                    .AddEvoker(compute[nameof(ComputeCurrency.Compute)], ComputeResultRequirements);

            download[nameof(SecondCurrency.GetSecondCurrency)]
                .Laborer
                    .AddEvoker(compute[nameof(ComputeCurrency.Compute)], ComputeResultRequirements);

            compute[nameof(ComputeCurrency.Compute)]
                .Laborer
                    .AddEvoker(compute[nameof(PresentResult.Present)]);

            for (int i = 1; i < 15; i++)
            {
                download[nameof(FirstCurrency.GetFirstCurrency)].Elaborate("EUR", i);
                download[nameof(SecondCurrency.GetSecondCurrency)].Elaborate("USD", i);
            }

            Thread.Sleep(10000);

            download.Visor.Close(true);
            compute.Visor.Close(true);
        }

        /// <summary>
        /// The Labors_QuickLabor_Integration_Test.
        /// </summary>
        [Fact]
        public void Labors_QuickLabor_Integration_Test()
        {

            QuickLabor ql0 = new QuickLabor(typeof(FirstCurrency).FullName, nameof(FirstCurrency.GetFirstCurrency), "EUR", 1);
            QuickLabor ql1 = new QuickLabor(typeof(SecondCurrency).FullName, nameof(SecondCurrency.GetSecondCurrency), "USD", 1);

            Thread.Sleep(5000);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="NBPSource" />.
    /// </summary>
    public class NBPSource
    {
        #region Constants

        private const string file_dir = "http://www.nbp.pl/Kursy/xml/dir.txt";
        private const string xml_url = "http://www.nbp.pl/kursy/xml/";

        #endregion

        #region Fields

        public string file_name;
        public DateTime rate_date;
        private int start_int = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NBPSource"/> class.
        /// </summary>
        /// <param name="daysbefore">The daysbefore<see cref="int"/>.</param>
        public NBPSource(int daysbefore)
        {
            GetFileName(daysbefore);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The GetCurrenciesRate.
        /// </summary>
        /// <param name="currency_names">The currency_names<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="Dictionary{string, double}"/>.</returns>
        public Dictionary<string, double> GetCurrenciesRate(List<string> currency_names)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach (var item in currency_names)
            {
                result.Add(item, LoadRate(item));
            }
            return result;
        }

        /// <summary>
        /// The LoadRate.
        /// </summary>
        /// <param name="currency_name">The currency_name<see cref="string"/>.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public double LoadRate(string currency_name)
        {

            try
            {
                string file = xml_url + file_name + ".xml";
                DataSet ds = new DataSet(); ds.ReadXml(file);
                var tabledate = ds.Tables["tabela_kursow"].Rows.Cast<DataRow>().AsEnumerable();
                var before_rate_date = (from k in tabledate select new { Data = k["data_publikacji"].ToString() }).First();
                var tabela = ds.Tables["pozycja"].Rows.Cast<DataRow>().AsEnumerable();
                var rate = (from k in tabela where k["kod_waluty"].ToString() == currency_name select new { Kurs = k["kurs_sredni"].ToString() }).First();
                rate_date = Convert.ToDateTime(before_rate_date.Data);
                return Convert.ToDouble(rate.Kurs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        /// <summary>
        /// The GetFileName.
        /// </summary>
        /// <param name="daysbefore">The daysbefore<see cref="int"/>.</param>
        private void GetFileName(int daysbefore)
        {

            try
            {
                int minusdays = daysbefore * -1;
                WebClient client = new WebClient();
                Stream strm = client.OpenRead(file_dir);
                StreamReader sr = new StreamReader(strm);
                string file_list = sr.ReadToEnd();
                string date_str = string.Empty;
                bool has_this_rate = false;
                DateTime date_of_rate = DateTime.Now.AddDays(minusdays);
                while (!has_this_rate)
                {
                    date_str = "a" + start_int.ToString().PadLeft(3, '0') + "z" + date_of_rate.ToString("yyMMdd");
                    if (file_list.Contains(date_str))
                    {
                        has_this_rate = true;
                    }

                    start_int++;

                    if (start_int > 365)
                    {
                        start_int = 1;
                        date_of_rate = date_of_rate.AddDays(-1);
                    }
                }
                file_name = date_str;
                rate_date = date_of_rate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="PresentResult" />.
    /// </summary>
    public class PresentResult
    {
        #region Methods

        /// <summary>
        /// The Present.
        /// </summary>
        /// <param name="rate1">The rate1<see cref="double"/>.</param>
        /// <param name="rate2">The rate2<see cref="double"/>.</param>
        /// <param name="result">The result<see cref="double"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Present(double rate1, double rate2, double result)
        {

            try
            {
                string present = "Rate USD : " + rate1.ToString() + " EUR : " + rate2.ToString() + " EUR->USD : " + result.ToString();
                Debug.WriteLine(present);
                return present;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SecondCurrency" />.
    /// </summary>
    public class SecondCurrency
    {
        #region Methods

        /// <summary>
        /// The GetSecondCurrency.
        /// </summary>
        /// <param name="currency">The currency<see cref="string"/>.</param>
        /// <param name="days">The days<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetSecondCurrency(string currency, int days)
        {
            NBPSource kurKraju = new NBPSource(days);

            try
            {
                double rate = kurKraju.LoadRate(currency);
                Debug.WriteLine("Rate 2 : " + currency + " days past : " + days.ToString() + " amount : " + rate.ToString("#.####"));

                return new object[] { currency, rate };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new InvalidLaborException(ex.ToString());
            }
        }

        #endregion
    }
}
