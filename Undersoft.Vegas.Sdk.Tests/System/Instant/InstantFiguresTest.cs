using System.Reflection;
using Xunit;

namespace System.Instant.Tests
{   
    public class FiguresTest
    {
        private Figure str;
        private Figures rtsq;
        private IFigures iRtseq;
        private IFigure iRts; 
        private IFigure Figure_Compilation_Helper_Test(Figure str, FieldsAndPropertiesModel fom)
        {
            IFigure rts = str.Combine();          

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
            return rts;           
        }

        [Fact]
        public void Figures_NewFigure_Test()
        {
            str = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            iRts = Figure_Compilation_Helper_Test(str, fom);

            rtsq = new Figures(str, "InstantSequence_Compilation_Test");

            iRtseq = rtsq.Combine();

            IFigure rcst = iRtseq.NewFigure();

            Assert.NotNull(rcst);
        }

        [Fact]
        public void Figures_MutatorAndAccessorById_Test()
        {
            str = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            iRts = Figure_Compilation_Helper_Test(str, fom);

            rtsq = new Figures(str, "InstantSequence_Compilation_Test");

            iRtseq = rtsq.Combine();

            iRtseq.Add(iRtseq.NewFigure());
            iRtseq[0, 4] = iRts[4];

            Assert.Equal(iRts[4], iRtseq[0, 4]);
            
        }

        [Fact]
        public void Figures_MutatorAndAccessorByName_Test()
        {
            str = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            iRts = Figure_Compilation_Helper_Test(str, fom);

            rtsq = new Figures(str, "InstantSequence_Compilation_Test");

            iRtseq = rtsq.Combine();

            iRtseq.Add(iRtseq.NewFigure());
            iRtseq[0, nameof(fom.Name)] = iRts[nameof(fom.Name)];

            Assert.Equal(iRts[nameof(fom.Name)], iRtseq[0, nameof(fom.Name)]);

        }

        [Fact]
        public void Figures_SetRubrics_Test()
        {
            str = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            iRts = Figure_Compilation_Helper_Test(str, fom);

            rtsq = new Figures(str, "InstantSequence_Compilation_Test");

            var rttab = rtsq.Combine();

            Assert.Equal(rttab.Rubrics, rtsq.Rubrics);
          
        }

        [Fact]
        public void Figures_Compile_Test()
        {
            str = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            iRts = Figure_Compilation_Helper_Test(str, fom);

            rtsq = new Figures(str, "InstantSequence_Compilation_Test");

            var rttab = rtsq.Combine();

            for (int i = 0; i < 10000; i++)
            {      
                rttab.Add((long)int.MaxValue + i, rttab.NewFigure());
            }

            for (int i = 9999; i > -1; i--)
            {                
                rttab[i] = rttab.Get(i + (long)int.MaxValue); 
            }

        }

    }
}
