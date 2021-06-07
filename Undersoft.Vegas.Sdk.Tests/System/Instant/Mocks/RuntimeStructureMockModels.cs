/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Tests.RuntimeStructureMockModels.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Tests
{
    using System.Instant.Treatments;
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="FieldsAndPropertiesModel" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class FieldsAndPropertiesModel
    {

        #region Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; } = 404;

        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias { get; set; } = "ProperSize";

        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

        /// <summary>
        /// Gets or sets the ByteArray.
        /// </summary>
        [FigureAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] ByteArray { get; set; }

        /// <summary>
        /// Gets or sets the Factor.
        /// </summary>
        public double Factor { get; set; } = 2 * (long)int.MaxValue;

        /// <summary>
        /// Gets or sets the GlobalId.
        /// </summary>
        public Guid GlobalId { get; set; } = new Guid();

        /// <summary>
        /// Gets or sets a value indicating whether Status.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the SystemKey.
        /// </summary>
        public Ussn SystemKey { get; set; } = Ussn.Empty;

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

        #endregion

        #region Fields
      
        public long Key = long.MaxValue;

        #endregion     
        //#region Properties

        ///// <summary>
        ///// Gets or sets the Id.
        ///// </summary>
        //[FigureKey]
        //public int Id { get; set; } = 404;

        ///// <summary>
        ///// Gets or sets the Key.
        ///// </summary>
        //public long Key { get; set; } = long.MaxValue;

        //#endregion

        //#region Fields

        //[FigureAs(UnmanagedType.ByValArray, SizeConst = 32)]
        //public string Alias { get; set; } = "ProperSize";

        //[FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        //public byte[] ByteArray { get; set; } = new byte[10];

        //public double Factor = 2 * (long)int.MaxValue;

        //public Guid GlobalId = new Guid();

        //[FigureDisplay("ProductName")]
        //[FigureAs(UnmanagedType.ByValArray, SizeConst = 64)]
        //public string Name { get; set; } = "SizeIsTwoTimesLonger";

        //[FigureDisplay("AvgPrice")]
        //[FigureTreatment(AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Sum)]
        //public double Price = 12.3;

        //public bool Status;

        //public Usid SystemKey = Usid.Empty;

        //[FigureKey]
        //public DateTime Time = DateTime.Now;

        //[FigureAs(UnmanagedType.ByValArray, SizeConst = 16)]
        //public string Token { get; set; } = "AFH54345";

        //#endregion
    }

    /// <summary>
    /// Defines the <see cref="FieldsOnlyModel" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class FieldsOnlyModel
    {
        #region Fields

        [FigureKey]
        public int Id = 404;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "ProperSize";
        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray = new byte[10];
        public double Factor = 2 * (long)int.MaxValue;
        public Guid GlobalId = new Guid();
        public long Key = long.MaxValue;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name = "SizeIsTwoTimesLonger";
        [FigureDisplay("AvgPrice")]
        [FigureTreatment(AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Sum)]
        public double Price;
        public bool Status;
        public Ussn SystemKey = Ussn.Empty;
        public DateTime Time = DateTime.Now;

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="PropertiesOnlyModel" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PropertiesOnlyModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [FigureKey(IsAutoincrement = true, Order = 0)]
        public int Id { get; set; } = 405;

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        [FigureKey]
        public long Key { get; set; } = long.MaxValue;

        /// <summary>
        /// Gets or sets the Alias.
        /// </summary>
        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias { get; set; } = "ProperSize";

        /// <summary>
        /// Gets or sets the ByteArray.
        /// </summary>
        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray { get; set; }

        /// <summary>
        /// Gets or sets the Factor.
        /// </summary>
        public double Factor { get; set; } = 2 * (long)int.MaxValue;

        /// <summary>
        /// Gets or sets the GlobalId.
        /// </summary>
        public Guid GlobalId { get; set; } = new Guid();

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [FigureKey(Order = 1)]
        [FigureDisplay("ProductName")]
        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

        /// <summary>
        /// Gets or sets the Price.
        /// </summary>
        [FigureDisplay("AvgPrice")]
        [FigureTreatment(AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Avg)]
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Status.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the SystemKey.
        /// </summary>
        public Usid SystemKey { get; set; } = Usid.Empty;

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

        #endregion
    }
}
