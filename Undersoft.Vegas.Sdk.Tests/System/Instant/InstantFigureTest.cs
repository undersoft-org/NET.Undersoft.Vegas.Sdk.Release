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

    public class FigureTest
    {
        #region Constructors

        public FigureTest()
        {
        }

        #endregion

        #region Methods

        //[Fact]
        //public unsafe void Figure_Extractions_Test()
        //{

        //    Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel));
        //    FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
        //    object rts = Figure_Compilation_Helper_Test(referenceType, fom);

        //    IntPtr pserial = rts.GetStructureIntPtr();
        //    object rts2 = referenceType.New();
        //    pserial.ToStructure(rts2);

        //    byte[] bserial = rts2.GetBytes();
        //    object rts3 = referenceType.New();
        //    bserial.ToStructure(rts3);

        //    object rts4 = referenceType.New();
        //    rts4.StructureFrom(bserial);

        //    Figure valueType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.ValueType);
        //    fom = new FieldsAndPropertiesModel();
        //    IFigure vts = Figure_Compilation_Helper_Test(valueType, fom);
        //    ValueType v = (ValueType)vts;

        //    IntPtr pserial2 = v.GetStructureIntPtr();

        //    IFigure vts2 = valueType.Combine();
        //    ValueType v2 = (ValueType)vts2;
        //    vts2 = (IFigure)(pserial2.ToStructure(v2));

        //    byte[] bserial2 = vts.GetBytes();
        //    IFigure vts3 = valueType.Combine();
        //    ValueType v3 = (ValueType)vts3;
        //    vts3 = (IFigure)(bserial2.ToStructure(v3));
        //    fixed (byte* b = bserial2)
        //        vts3 = (IFigure)(Extractor.PointerToStructure(b, vts3));

        //    IFigure vts4 = valueType.Combine();
        //    vts4 = (IFigure)(vts4.StructureFrom(pserial2));

        //    Marshal.FreeHGlobal((IntPtr)pserial2);
        //}

        [Fact]
        public void Figure_Memberinfo_FieldsAndPropertiesModel_Compilation_Test()
        {
            Figure derivedType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.Derived);
            IFigure figureA = Figure_Compilation_Helper_Test(derivedType, new FieldsAndPropertiesModel());

            Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.Reference);
            IFigure figureB = Figure_Compilation_Helper_Test(referenceType, new FieldsAndPropertiesModel());

            Figure valueType = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.ValueType);
            IFigure figureC = Figure_Compilation_Helper_Test(valueType, new FieldsAndPropertiesModel());
        }

        [Fact]
        public void Figure_Memberinfo_FieldsOnlyModel_Compilation_Test()
        {
            Figure derivedType = new Figure(typeof(FieldsOnlyModel), FigureMode.Derived);
            IFigure figureA = Figure_Compilation_Helper_Test(derivedType, new FieldsOnlyModel());

            Figure referenceType = new Figure(typeof(FieldsOnlyModel), FigureMode.Reference);
            IFigure figureB = Figure_Compilation_Helper_Test(referenceType, new FieldsOnlyModel());

            Figure valueType = new Figure(typeof(FieldsOnlyModel), FigureMode.ValueType);
            IFigure figureC = Figure_Compilation_Helper_Test(valueType, new FieldsOnlyModel());        
        }

        [Fact]
        public void Figure_Memberinfo_PropertiesOnlyModel_Compilation_Test()
        {
            Figure derivedType = new Figure(typeof(PropertiesOnlyModel), FigureMode.Derived);
            IFigure figureA = Figure_Compilation_Helper_Test(derivedType, new PropertiesOnlyModel());

            Figure referenceType = new Figure(typeof(PropertiesOnlyModel), FigureMode.Reference);
            IFigure figureB = Figure_Compilation_Helper_Test(referenceType, new PropertiesOnlyModel());

            Figure valueType = new Figure(typeof(PropertiesOnlyModel), FigureMode.ValueType);
            IFigure figureC = Figure_Compilation_Helper_Test(valueType, new PropertiesOnlyModel());
        }

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

        [Fact]
        public void Figure_MemberRubric_FieldsOnlyModel_Compilation_Test()
        {
            Figure referenceType = new Figure(FigureMocks.Figure_MemberRubric_FieldsOnlyModel(),
                                                                "Figure_MemberRubric_FieldsOnlyModel_Reference");
            FieldsOnlyModel fom = new FieldsOnlyModel();
            IFigure figureA = Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(FigureMocks.Figure_Memberinfo_FieldsOnlyModel(),
                                                             "Figure_MemberRubric_FieldsOnlyModel_ValueType", FigureMode.ValueType);
            fom = new FieldsOnlyModel();
            IFigure figureB = Figure_Compilation_Helper_Test(valueType, fom);
        }

        [Fact]
        public void Figure_MemberRubric_PropertiesOnlyModel_Compilation_Test()
        {
            Figure referenceType = new Figure(FigureMocks.Figure_MemberRubric_PropertiesOnlyModel(),
                                                                "Figure_MemberRubric_PropertiesOnlyModel_Reference");
            PropertiesOnlyModel fom = new PropertiesOnlyModel();
            IFigure figureA = Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(FigureMocks.Figure_MemberRubric_PropertiesOnlyModel(),
                                                        "Figure_MemberRubric_PropertiesOnlyModel_ValueType", FigureMode.ValueType);
            fom = new PropertiesOnlyModel();
            IFigure figureB = Figure_Compilation_Helper_Test(valueType, fom);
        }

        [Fact]
        public void Figure_ValueArray_GetSet_Test()
        {
            Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel));

            Figure_Compilation_Helper_Test(referenceType, Figure_Compilation_Helper_Test(referenceType, new FieldsAndPropertiesModel()));

            Figure valueType = new Figure(typeof(PropertiesOnlyModel), FigureMode.ValueType);

            Figure_Compilation_Helper_Test(valueType, Figure_Compilation_Helper_Test(valueType, new FieldsAndPropertiesModel()));
        }

        private IFigure Figure_Compilation_Helper_Test(Figure str, FieldsAndPropertiesModel fom)
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
                var ri = str.Rubrics[i].RubricInfo;
                if (ri.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)ri).Name);
                    if (fi != null)
                        rts[ri.Name] = fi.GetValue(fom);
                }
                if (ri.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)ri).Name);
                    if (pi != null)
                        (rts)[ri.Name] = pi.GetValue(fom);
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

        private void Figure_Compilation_Helper_Test(Figure str, IFigure figure)
        {
            IFigure rts = str.Combine();
            object[] values = rts.ValueArray;
            rts.ValueArray = figure.ValueArray;
            for (int i = 0; i < values.Length; i++)
                Assert.Equal(figure[i], rts.ValueArray[i]);
        }

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
