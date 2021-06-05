/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.MathlineMockModels.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="MathsetMockModel" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class MathsetMockModel
    {
        #region Fields

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "StockAlias";
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Name = "StockFullName";
        public int Quantity = 86;
        private long Key = long.MaxValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the BuyFeeRate.
        /// </summary>
        public double BuyFeeRate { get; set; } = 8;

        /// <summary>
        /// Gets or sets the BuyNetCost.
        /// </summary>
        public double BuyNetCost { get; set; } = 0.85;

        /// <summary>
        /// Gets or sets the BuyNetTotal.
        /// </summary>
        public double BuyNetTotal { get; set; } = 0.85;

        /// <summary>
        /// Gets or sets the ByteArray.
        /// </summary>
        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray { get; set; } = new byte[10];

        /// <summary>
        /// Gets or sets the CurrencyRate.
        /// </summary>
        public double CurrencyRate { get; set; } = 1.23;

        /// <summary>
        /// Gets or sets the CurrentMarkupRate.
        /// </summary>
        public double CurrentMarkupRate { get; set; } = 0;

        /// <summary>
        /// Gets or sets the GlobalId.
        /// </summary>
        public Guid GlobalId { get; set; } = new Guid();

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; } = 404;

        /// <summary>
        /// Gets or sets the NetCost.
        /// </summary>
        public double NetCost { get; set; } = 1.00;

        /// <summary>
        /// Gets or sets the NetPrice.
        /// </summary>
        public double NetPrice { get; set; } = 1.00;

        /// <summary>
        /// Gets or sets the SellFeeRate.
        /// </summary>
        public double SellFeeRate { get; set; } = 8;

        /// <summary>
        /// Gets or sets the SellGrossPrice.
        /// </summary>
        public double SellGrossPrice { get; set; } = 0;

        /// <summary>
        /// Gets or sets the SellGrossTotal.
        /// </summary>
        public double SellGrossTotal { get; set; } = 0;

        /// <summary>
        /// Gets or sets the SellNetPrice.
        /// </summary>
        public double SellNetPrice { get; set; } = 1.00;

        /// <summary>
        /// Gets or sets the SellNetTotal.
        /// </summary>
        public double SellNetTotal { get; set; } = 1.00;

        /// <summary>
        /// Gets or sets a value indicating whether Status.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the SystemKey.
        /// </summary>
        public Ussn SystemKey { get; set; } = Ussn.Empty;

        /// <summary>
        /// Gets or sets the TargetMarkupRate.
        /// </summary>
        public double TargetMarkupRate { get; set; } = 0;

        /// <summary>
        /// Gets or sets the TaxRate.
        /// </summary>
        public double TaxRate { get; set; } = 1.23;

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

        #endregion
    }
}
