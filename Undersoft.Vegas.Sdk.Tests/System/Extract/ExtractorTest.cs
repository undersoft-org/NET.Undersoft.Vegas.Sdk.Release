using System.Uniques;
using System.Runtime.InteropServices;
using System.Instant;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.Extract
{   
    public class ExtractorTest
    {
        #region Parameters
        byte[] source = new byte[ushort.MaxValue];
        byte[] dest = new byte[ushort.MaxValue];
        byte[] structBytes = null;
        byte[] structBytes2 = null;
        Figure str = null;
        Figures table = null;
        IFigures rctab = null;
        IFigure rcobj = null;
        #endregion

        public ExtractorTest()
        {
            Random r = new Random();
            r.NextBytes(source);

            str = new Figure(FigureMocks.Figure_MemberRubric_FieldsAndPropertiesModel(),
                                                      "Figure_MemberRubric_FieldsAndPropertiesModel_ValueType");
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            table = new Figures(str, "Figures_Compilation_Test");
            rctab = table.Combine();

            rcobj = rctab.NewFigure();

            foreach (var rubric in str.Rubrics.AsValues())
            {
                if (rubric.FieldId > -1)
                {                   
                    var field = fom.GetType().GetField(rubric.FigureField.Name,
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field == null)
                        field = fom.GetType().GetField(rubric.RubricName);
                    if (field == null)
                    {
                        var prop = fom.GetType().GetProperty(rubric.RubricName);
                        if(prop != null)
                            rcobj[rubric.FieldId] = prop.GetValue(fom);
                    }
                    else
                        rcobj[rubric.FieldId] = field.GetValue(fom);   
                    
                }
            }

            for (int i = 0; i < 1000; i++)
            {
                IFigure nrcstr = rctab.NewFigure();
                nrcstr.ValueArray = rcobj.ValueArray;
                rctab.Add(i, nrcstr);
            }

            structBytes = new byte[rctab.FigureSize];
            structBytes2 = new byte[rctab.FigureSize];

            rcobj.StructureTo(ref structBytes, 0);
        }

        private IFigure Figure_Compilation_Helper_Test(IInstant str, FieldsAndPropertiesModel fom)
        {
            IFigure rts = (IFigure)str.New();
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

        [Fact] public unsafe void Extractor_CopyBlock_ByteArray_UInt_Test()
        {
            Random r = new Random();
            r.NextBytes(source);
            dest.Initialize();

            Extractor.CopyBlock(dest, 0, source, 0, source.Length);
            bool equal = dest.BlockEqual(source);
            Assert.True(equal);
        }
        [Fact] public unsafe void Extractor_CopyBlock_Pointer_UInt_Test()
        {
            Random r = new Random();
            r.NextBytes(source);
            dest.Initialize();

            fixed (byte* psrc = source, pdst = dest)
            {
                Extractor.CopyBlock(pdst, 0, psrc, 0, source.Length);
                bool equal = dest.BlockEqual(source);
                Assert.True(equal);
            }
        }
        [Fact] public unsafe void Extractor_CopyBlock_ByteArray_Ulong_Test()
        {
            Random r = new Random();
            r.NextBytes(source);
            dest.Initialize();

            Extractor.CopyBlock(dest, (ulong)0, source, (ulong)0, (ulong)source.Length);
            bool equal = dest.BlockEqual(source);
            Assert.True(equal);
        }
        [Fact] public unsafe void Extractor_CopyBlock_Pointer_Ulong_Test()
        {
            Random r = new Random();
            r.NextBytes(source);
            dest.Initialize();

            Extractor.CopyBlock(dest, 0, source, 0, source.Length);
            bool equal = dest.BlockEqual(source);
            Assert.True(equal);
        }

        [Fact] public unsafe void Extractor_BytesToStruct_FromType_Test()
        {
            object os = rctab.NewFigure();
            Extractor.BytesToStructure(structBytes, os, 0);
            bool equal = rcobj.StructureEqual(os);
            Assert.True(equal);
        }
        [Fact] public unsafe void Extractor_PointerToNewStruct_Type_Test()
        {
            fixed (byte* b = structBytes)
            {
                object os = Extractor.PointerToStructure(b, rctab.FigureType, 0);
                bool equal = rcobj.StructureEqual(os);
                Assert.True(equal);
            }
        }
        [Fact] public unsafe void Extractor_PointerToStruct_Test()
        {
            fixed (byte* b = structBytes)
            {
                object os = rctab.NewFigure();
                Extractor.PointerToStructure(b, os);
                bool equal = rcobj.StructureEqual(os);
                Assert.True(equal);
            }
        }
        [Fact] public unsafe void Extractor_StructToBytesArray_Test()
        {
            byte[] b = rcobj.GetStructureBytes();
            bool equal = b.BlockEqual(structBytes);           
            Assert.True(equal);
            object ro = rcobj;
            structBytes2 = new byte[rcobj.GetSize()];
            Extractor.StructureToBytes(ro, ref structBytes2, 0);
            bool equal2 = structBytes2.BlockEqual(structBytes);
            Assert.True(equal2);

        }
        [Fact] public unsafe void Extractor_StructToPointer_Test()
        {          

            GCHandle gcptr = GCHandle.Alloc(structBytes, GCHandleType.Pinned);
            byte* ptr = (byte*)gcptr.AddrOfPinnedObject();

            rcobj.StructureTo(ptr);

            rcobj["Id"] = 88888;
            rcobj["Name"] = "Zmiany";

            rcobj.StructureTo(ptr);

            rcobj["Id"] = 5555555;
            rcobj["Name"] = "Zm342";

            rcobj.StructureFrom(ptr);

            Assert.Equal(88888, rcobj["Id"]);


        }
        [Fact] public unsafe void Extractor_StructModel_Test()
        {
            Assemblies.ResolveExecuting();

            StructModel[] structure = new StructModel[] { new StructModel(83948930), new StructModel(45453), new StructModel(5435332) };
            structure[0].Alias = "FirstAlias";
            structure[0].Name = "FirstName";
            structure[1].Alias = "SecondAlia";
            structure[1].Name = "SecondName";
            structure[2].Alias = "ThirdAlias";
            structure[2].Name = "ThirdName";

            StructModels structures = new StructModels(structure);

            int size = Marshal.SizeOf(structure[0]);

            byte* pserial = Extraction.ValueStructureToPointer(structure[0]);

            StructModel structure2 = new StructModel();
            ValueType o = structure2;

            Extraction.PointerToValueStructure(pserial, o, 0);

            structure2 = (StructModel)o;

            structure2.Alias = "FirstChange";

        }

        [Fact] public unsafe void Extractor_FigureExtracts_Test()
        {
            Figure refFigure = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fapm = new FieldsAndPropertiesModel();
            IFigure refFigureFilled = Figure_Compilation_Helper_Test(refFigure, fapm);

            IntPtr refFigureFilled_UnmngPtr = refFigureFilled.GetStructureIntPtr();
            IFigure refFigureNew_A = refFigure.Combine();
            refFigureFilled_UnmngPtr.ToStructure(refFigureNew_A);

            byte[] refFigureNew_A_Bytes = refFigureNew_A.GetBytes();
            IFigure refFigureNew_B = refFigure.Combine();
            refFigureNew_A_Bytes.ToStructure(refFigureNew_B);

            IFigure refFigureNew_C = refFigure.Combine();
            refFigureNew_C.StructureFrom(refFigureNew_A_Bytes);

            Figure valtypeFigure = new Figure(typeof(FieldsAndPropertiesModel), FigureMode.ValueType);
            fapm = new FieldsAndPropertiesModel();
            IFigure valtypeFigureFilled = Figure_Compilation_Helper_Test(valtypeFigure, fapm);
            ValueType valtypeFigureFilledAndCasted = (ValueType)valtypeFigureFilled;

            IntPtr valtypeFigureFilledAndCasted_UnmngPtr = valtypeFigureFilledAndCasted.GetStructureIntPtr();

            IFigure valtypeFigureNew_A = valtypeFigure.Combine();
            ValueType valtypeFigureNew_A_Casted = (ValueType)valtypeFigureNew_A;
            valtypeFigureNew_A = (IFigure)(valtypeFigureFilledAndCasted_UnmngPtr.ToStructure(valtypeFigureNew_A_Casted));

            byte[] valtypeFigureNew_A_Bytes = valtypeFigureNew_A.GetBytes();
            IFigure valtypeFigureNew_B = valtypeFigure.Combine();
            valtypeFigureNew_B = (IFigure)(valtypeFigureFilledAndCasted_UnmngPtr.ToStructure(valtypeFigureNew_B));
            fixed (byte* b = valtypeFigureNew_A_Bytes)
                valtypeFigureNew_B = (IFigure)(Extractor.PointerToStructure(b, valtypeFigureNew_B));

            IFigure valtypeFigureNew_C = valtypeFigure.Combine();
            valtypeFigureNew_C = (IFigure)(valtypeFigureNew_C.StructureFrom(valtypeFigureFilledAndCasted_UnmngPtr));

            Marshal.FreeHGlobal((IntPtr)valtypeFigureFilledAndCasted_UnmngPtr);

        }

    }
}
