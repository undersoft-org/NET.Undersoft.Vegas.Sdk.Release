/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Tests.InstantFiguresTest.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Tests
{
    using System.Reflection;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="FiguresTest" />.
    /// </summary>
    public class FiguresTest
    {
        #region Fields

        private IFigure iRts;
        private IFigures iRtseq;
        private Figures rtsq;
        private Figure str;

        #endregion

        #region Methods

        /// <summary>
        /// The Figures_Compile_Test.
        /// </summary>
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

        /// <summary>
        /// The Figures_MutatorAndAccessorById_Test.
        /// </summary>
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

        /// <summary>
        /// The Figures_MutatorAndAccessorByName_Test.
        /// </summary>
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

        /// <summary>
        /// The Figures_NewFigure_Test.
        /// </summary>
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

        /// <summary>
        /// The Figures_SetRubrics_Test.
        /// </summary>
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

        /// <summary>
        /// The Figure_Compilation_Helper_Test.
        /// </summary>
        /// <param name="str">The str<see cref="Figure"/>.</param>
        /// <param name="fom">The fom<see cref="FieldsAndPropertiesModel"/>.</param>
        /// <returns>The <see cref="IFigure"/>.</returns>
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

        #endregion
    }
}
