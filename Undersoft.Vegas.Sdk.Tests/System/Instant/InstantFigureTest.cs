/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Tests.InstantFigureTest.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Tests
{
    using System.Extract;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Uniques;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="FigureTest" />.
    /// </summary>
    public class FigureTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureTest"/> class.
        /// </summary>
        public FigureTest()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Figure_Extractions_Test.
        /// </summary>
        [Fact]
        public unsafe void Figure_Extractions_Test()
        {

            Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            object rts = Figure_Compilation_Helper_Test(referenceType, fom);

            IntPtr pserial = rts.GetStructureIntPtr();
            object rts2 = referenceType.New();
            pserial.ToStructure(rts2);

            byte[] bserial = rts2.GetBytes();
            object rts3 = referenceType.New();
            bserial.ToStructure(rts3);

            object rts4 = referenceType.New();
            rts4.StructureFrom(bserial);

            Figure valueType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.ValueType);
            fom = new FieldsAndPropertiesModel();
            object vts = Figure_Compilation_Helper_Test(valueType, fom);
            ValueType v = (ValueType)vts;

            IntPtr pserial2 = vts.GetStructureIntPtr();

            object vts2 = valueType.New();
            ValueType v2 = (ValueType)vts2;
            vts2 = pserial2.ToStructure(vts2);

            byte[] bserial2 = vts.GetBytes();
            object vts3 = valueType.New();
            vts3 = bserial2.ToStructure(vts3);
            fixed (byte* b = bserial2)
                vts3 = Extractor.PointerToStructure(b, vts3);

            object vts4 = valueType.New();
            vts4 = vts4.StructureFrom(pserial2);

            Marshal.FreeHGlobal((IntPtr)pserial2);
        }

        /// <summary>
        /// The Figure_Memberinfo_FieldsAndPropertiesModel_Compilation_Test.
        /// </summary>
        [Fact]
        public void Figure_Memberinfo_FieldsAndPropertiesModel_Compilation_Test()
        {
            Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.Derived);
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.ValueType);
            fom = new FieldsAndPropertiesModel();
            Figure_Compilation_Helper_Test(valueType, fom);
        }

        /// <summary>
        /// The Figure_Memberinfo_FieldsOnlyModel_Compilation_Test.
        /// </summary>
        [Fact]
        public void Figure_Memberinfo_FieldsOnlyModel_Compilation_Test()
        {
            Figure referenceType = new Figure(typeof(FieldsOnlyModel));

            FieldsOnlyModel fom = new FieldsOnlyModel();
            Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(typeof(FieldsOnlyModel), FigureMode.ValueType);
            fom = new FieldsOnlyModel();
            Figure_Compilation_Helper_Test(valueType, fom);
        }

        /// <summary>
        /// The Figure_Memberinfo_PropertiesOnlyModel_Compilation_Test.
        /// </summary>
        [Fact]
        public void Figure_Memberinfo_PropertiesOnlyModel_Compilation_Test()
        {
            Figure str = new Figure(typeof(PropertiesOnlyModel));
            PropertiesOnlyModel fom = new PropertiesOnlyModel();
            Figure_Compilation_Helper_Test(str, fom);

            Figure valueType = new Figure(typeof(PropertiesOnlyModel), FigureMode.ValueType);

            fom = new PropertiesOnlyModel();
            Figure_Compilation_Helper_Test(valueType, fom);
        }

        /// <summary>
        /// The Figure_MemberRubric_FieldsAndPropertiesModel_Compilation_Test.
        /// </summary>
        [Fact]
        public void Figure_MemberRubric_FieldsAndPropertiesModel_Compilation_Test()
        {
            Figure referenceType = new Figure(FigureMocks.Figure_MemberRubric_FieldsAndPropertiesModel(),
                                                                "Figure_MemberRubric_FieldsAndPropertiesModel_Reference");

            Figure_Compilation_Helper_Test(referenceType, new FieldsAndPropertiesModel());

            Figure valueType = new Figure(FigureMocks.Figure_MemberRubric_FieldsAndPropertiesModel(),
                                               "Figure_MemberRubric_FieldsAndPropertiesModel_ValueType", FigureMode.ValueType);

            Figure_Compilation_Helper_Test(valueType, new FieldsAndPropertiesModel());
        }

        /// <summary>
        /// The Figure_MemberRubric_FieldsOnlyModel_Compilation_Test.
        /// </summary>
        [Fact]
        public void Figure_MemberRubric_FieldsOnlyModel_Compilation_Test()
        {
            Figure referenceType = new Figure(FigureMocks.Figure_MemberRubric_FieldsOnlyModel(),
                                                                "Figure_MemberRubric_FieldsOnlyModel_Reference");
            FieldsOnlyModel fom = new FieldsOnlyModel();
            Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(FigureMocks.Figure_Memberinfo_FieldsOnlyModel(),
                                                             "Figure_MemberRubric_FieldsOnlyModel_ValueType", FigureMode.ValueType);
            fom = new FieldsOnlyModel();
            Figure_Compilation_Helper_Test(valueType, fom);
        }

        /// <summary>
        /// The Figure_MemberRubric_PropertiesOnlyModel_Compilation_Test.
        /// </summary>
        [Fact]
        public void Figure_MemberRubric_PropertiesOnlyModel_Compilation_Test()
        {
            Figure referenceType = new Figure(FigureMocks.Figure_MemberRubric_PropertiesOnlyModel(),
                                                                "Figure_MemberRubric_PropertiesOnlyModel_Reference");
            PropertiesOnlyModel fom = new PropertiesOnlyModel();
            Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(FigureMocks.Figure_MemberRubric_PropertiesOnlyModel(),
                                                        "Figure_MemberRubric_PropertiesOnlyModel_ValueType", FigureMode.ValueType);
            fom = new PropertiesOnlyModel();
            Figure_Compilation_Helper_Test(valueType, fom);
        }

        /// <summary>
        /// The Figure_ValueArray_GetSet_Test.
        /// </summary>
        [Fact]
        public void Figure_ValueArray_GetSet_Test()
        {
            Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel));

            Figure_Compilation_Helper_Test(referenceType, Figure_Compilation_Helper_Test(referenceType, new FieldsAndPropertiesModel()));

            Figure valueType = new Figure(typeof(PropertiesOnlyModel), FigureMode.ValueType);

            Figure_Compilation_Helper_Test(valueType, Figure_Compilation_Helper_Test(valueType, new FieldsAndPropertiesModel()));
        }

        /// <summary>
        /// The Figure_Compilation_Helper_Test.
        /// </summary>
        /// <param name="str">The str<see cref="Figure"/>.</param>
        /// <param name="fom">The fom<see cref="FieldsAndPropertiesModel"/>.</param>
        /// <returns>The <see cref="IFigure"/>.</returns>
        private IFigure Figure_Compilation_Helper_Test(Figure str, FieldsAndPropertiesModel fom)
        {
            IFigure rts = str.Combine();
            fom.Id = 202;
            rts[0] = 202;
            Assert.Equal(fom.Id, (rts)[0]);
            (rts)["Id"] = 404;
            Assert.NotEqual(fom.Id, (rts)[nameof(fom.Id)]);
            (rts)[nameof(fom.Name)] = fom.Name;
            Assert.Equal(fom.Name, (rts)[nameof(fom.Name)]);
            (rts).SerialCode = new Ussn(DateTime.Now.ToBinary());
            string hexTetra = (rts).SerialCode.ToString();
            Ussn ssn = new Ussn(hexTetra);
            Assert.Equal(ssn, (rts).SerialCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        (rts)[r.Name] = fi.GetValue(fom);
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        (rts)[r.Name] = pi.GetValue(fom);
                }
            }

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal((rts)[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        Assert.Equal((rts)[r.Name], pi.GetValue(fom));
                }
            }
            return rts;
        }

        /// <summary>
        /// The Figure_Compilation_Helper_Test.
        /// </summary>
        /// <param name="str">The str<see cref="Figure"/>.</param>
        /// <param name="fom">The fom<see cref="FieldsOnlyModel"/>.</param>
        /// <returns>The <see cref="IFigure"/>.</returns>
        private IFigure Figure_Compilation_Helper_Test(Figure str, FieldsOnlyModel fom)
        {
            IFigure rts = str.Combine();
            fom.Id = 202;
            rts[0] = 202;
            Assert.Equal(fom.Id, rts[0]);
            rts["Id"] = 404;
            Assert.NotEqual(fom.Id, rts[nameof(fom.Id)]);
            rts[nameof(fom.Name)] = fom.Name;
            Assert.Equal(fom.Name, rts[nameof(fom.Name)]);

            rts.SerialCode = new Ussn(DateTime.Now.ToBinary());
            string hexTetra = rts.SerialCode.ToString();
            Ussn ssn = new Ussn(hexTetra);
            Assert.Equal(ssn, rts.SerialCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        rts[r.Name] = fi.GetValue(fom);
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        rts[r.Name] = pi.GetValue(fom);
                }
            }

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal(rts[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        Assert.Equal(rts[r.Name], pi.GetValue(fom));
                }
            }
            return rts;
        }

        /// <summary>
        /// The Figure_Compilation_Helper_Test.
        /// </summary>
        /// <param name="str">The str<see cref="Figure"/>.</param>
        /// <param name="figure">The figure<see cref="IFigure"/>.</param>
        private void Figure_Compilation_Helper_Test(Figure str, IFigure figure)
        {
            IFigure rts = str.Combine();
            object[] values = rts.ValueArray;
            rts.ValueArray = figure.ValueArray;
            for (int i = 0; i < values.Length; i++)
                Assert.Equal(figure[i], rts.ValueArray[i]);
        }

        /// <summary>
        /// The Figure_Compilation_Helper_Test.
        /// </summary>
        /// <param name="str">The str<see cref="Figure"/>.</param>
        /// <param name="fom">The fom<see cref="PropertiesOnlyModel"/>.</param>
        /// <returns>The <see cref="IFigure"/>.</returns>
        private IFigure Figure_Compilation_Helper_Test(Figure str, PropertiesOnlyModel fom)
        {
            IFigure rts = str.Combine();
            fom.Id = 202;
            rts[0] = 202;
            Assert.Equal(fom.Id, rts[0]);
            rts["Id"] = 404;
            Assert.NotEqual(fom.Id, rts[nameof(fom.Id)]);
            rts[nameof(fom.Name)] = fom.Name;
            Assert.Equal(fom.Name, rts[nameof(fom.Name)]);
            rts.SerialCode = new Ussn(DateTime.Now.ToBinary());
            string hexTetra = rts.SerialCode.ToString();
            Ussn ssn = new Ussn(hexTetra);
            Assert.Equal(ssn, rts.SerialCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        rts[r.Name] = fi.GetValue(fom);
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        rts[r.Name] = pi.GetValue(fom);
                }
            }

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal(rts[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        Assert.Equal(rts[r.Name], pi.GetValue(fom));
                }
            }
            return rts;
        }

        #endregion
    }
}
