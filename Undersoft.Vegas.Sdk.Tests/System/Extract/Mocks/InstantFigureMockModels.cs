/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extract.InstantFigureMockModels.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    using System.Instant;
    using System.Runtime.InteropServices;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="StructModel" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StructModel
    {
        #region Fields

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] _alias;
        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray;
        public int Id;
        public long Key;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] name;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        public StructModel(int id = 0)
        {
            Id = id;
            _alias = new char[10];
            name = new char[10];
            ByteArray = new byte[10];
            Key = 0;
            SerialCode = Ussn.Empty;
            Status = false;
            Time = DateTime.Now;
            GlobalId = Guid.Empty;
            Factor = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Alias.
        /// </summary>
        public unsafe string Alias
        {
            get
            {
                return new string(_alias);
            }
            set
            {
                if (_alias == null)
                    _alias = new char[10];
                int al = _alias.Length;
                int l = value.Length > _alias.Length ? _alias.Length : value.Length;
                int s = sizeof(char);
                fixed (char* v = value, a = _alias)
                    Extractor.Cpblk((byte*)a, (byte*)v, (uint)(l * s));
            }
        }

        /// <summary>
        /// Gets or sets the Factor.
        /// </summary>
        public double Factor { get; set; }

        /// <summary>
        /// Gets or sets the GlobalId.
        /// </summary>
        public Guid GlobalId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public unsafe string Name
        {
            get
            {
                return new string(name);
            }
            set
            {
                if (name == null)
                    name = new char[10];
                int al = name.Length;
                int l = value.Length > name.Length ? name.Length : value.Length;
                int s = sizeof(char);
                fixed (char* v = value, a = name)
                    Extractor.Cpblk((byte*)a, (byte*)v, (uint)(l * s));
            }
        }

        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Ussn SerialCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Status.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="StructModels" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StructModels
    {
        #region Fields

        [FigureAs(UnmanagedType.LPArray, SizeConst = 3)]
        public StructModel[] Structs;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="structs">The structs<see cref="StructModel[]"/>.</param>
        public StructModels(StructModel[] structs)
        {
            Structs = structs;
        }

        #endregion
    }

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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string Alias = "ProperSize";

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
        public string Name = "SizeIsTwoTimesLonger";

        /// <summary>
        /// Gets or sets the ByteArray.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray;

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
    }

    /// <summary>
    /// Defines the <see cref="FieldsOnlyModel" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class FieldsOnlyModel
    {
        #region Fields

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
        public int Id { get; set; } = 404;

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
        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "ProperSize";
        private long Key = long.MaxValue;

        #endregion      
    }
}
