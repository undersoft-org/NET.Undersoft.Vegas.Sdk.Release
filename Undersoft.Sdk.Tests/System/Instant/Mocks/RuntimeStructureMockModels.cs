using System.Uniques;
using System.Instant.Treatment;
using System.Runtime.InteropServices;

namespace System.Instant
{

    [StructLayout(LayoutKind.Sequential)]
    public class FieldsOnlyModel
    {
        [FigureKey]
        public int Id = 404;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "ProperSize";

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name = "SizeIsTwoTimesLonger";

        public long Key = long.MaxValue;

        [FigureDisplay("AvgPrice")]
        [FigureTreatment(AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Sum)]
        public double Price;

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray = new byte[10];

        public Ussn SerialCode = Ussn.Empty;

        public bool Status;

        public DateTime Time = DateTime.Now;

        public Guid GlobalId = new Guid();

        public double Factor = 2 * (long)int.MaxValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PropertiesOnlyModel
    {
        [FigureKey(IsAutoincrement = true, Order = 0)]
        public int Id { get; set; } = 405;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "ProperSize";

        [FigureKey(Order = 1)]
        [FigureDisplay("ProductName")]
        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

        [FigureKey]
        private long Key = long.MaxValue;

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U1)]
        public byte[] ByteArray { get; set; }

        [FigureDisplay("AvgPrice")]
        [FigureTreatment( AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Sum )]
        public double Price { get; set; }

        public Usid SerialCode { get; set; } = Usid.Empty;

        public bool Status { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public Guid GlobalId { get; set;} = new Guid();

        public double Factor { get; set; } = 2 * (long)int.MaxValue;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class FieldsAndPropertiesModel
    {
        [FigureKey]
        public int Id { get; set; } = 404;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "ProperSize";

        [FigureDisplay("ProductName")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Name = "SizeIsTwoTimesLonger";

        private long Key = long.MaxValue;

        [FigureDisplay("AvgPrice")]
        [FigureTreatment(AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Sum)]
        public double Price { get; set; } = 12.3;

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray { get; set; } = new byte[10];

        public Usid SerialCode { get; set; } = Usid.Empty;

        public bool Status { get; set; }

        [FigureKey]
        public DateTime Time { get; set; } = DateTime.Now;

        public Guid GlobalId { get; set; } = new Guid();

        public double Factor { get; set; } = 2 * (long)int.MaxValue;
    }

  
}
