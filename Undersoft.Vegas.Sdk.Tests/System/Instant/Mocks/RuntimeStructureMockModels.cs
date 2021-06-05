using System.Uniques;
using System.Instant.Treatments;
using System.Runtime.InteropServices;

namespace System.Instant.Tests
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

        public Ussn SystemKey = Ussn.Empty;

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

        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias { get; set; } = "ProperSize";

        [FigureKey(Order = 1)]
        [FigureDisplay("ProductName")]
        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

        [FigureKey]
        public long Key { get; set; } = long.MaxValue;

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray { get; set; }

        [FigureDisplay("AvgPrice")]
        [FigureTreatment( AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Avg )]
        public double Price { get; set; }

        public Usid SystemKey { get; set; } = Usid.Empty;

        public bool Status { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public Guid GlobalId { get; set;} = new Guid();

        public double Factor { get; set; } = 2 * (long)int.MaxValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FieldsAndPropertiesModel
    {
        [FigureKey]
        public int Id { get; set; } = 404;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string token = "AFH54345";

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Alias = "ProperSize";

        [FigureDisplay("ProductName")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name = "SizeIsTwoTimesLonger";

        public long Key { get; set; } = long.MaxValue;

        [FigureDisplay("AvgPrice")]
        [FigureTreatment(AggregateOperand = AggregateOperand.Avg, SummaryOperand = AggregateOperand.Sum)]
        public double Price = 12.3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray  = new byte[10];

        public Usid SystemKey = Usid.Empty;

        public bool Status;

        [FigureKey]
        public DateTime Time = DateTime.Now;

        public Guid GlobalId = new Guid();

        public double Factor = 2 * (long)int.MaxValue;
    }

    public class FieldsAndProperties : FieldsAndPropertiesModel
    {
        public new int Id { get; set; }

    }



}
