/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.MathsetTest.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System.Reflection;

    using Xunit;

    /// <summary>
    /// Defines the <see cref="MathsetTest" />.
    /// </summary>
    public class MathsetTest
    {
        #region Fields

        private Figure instFig;
        private Figures instMtic;
        private Computation rck;
        private IFigures spcMtic;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MathsetTest"/> class.
        /// </summary>
        public MathsetTest()
        {
            instFig = new Figure(typeof(MathsetMockModel));

            instMtic = new Figures(instFig, "Figures_Mathset_Test");

            spcMtic = instMtic.Combine();

            MathsetMockModel fom = new MathsetMockModel();

            for (int i = 0; i < 1000 * 1000; i++)
            {
                var f = Mathset_Figure_Helper_Test(spcMtic, fom);
                f["NetPrice"] = (double)f["NetPrice"] + i;
                f["NetCost"] = (double)f["NetPrice"] / 2;
                spcMtic.Add(i, f);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Mathset_Computation_Formula_Test.
        /// </summary>
        [Fact]
        public void Mathset_Computation_Formula_Test()
        {
            rck = new Computation(spcMtic);

            Mathset ml = rck.GetMathset("SellNetPrice");

            ml.Formula = (ml["NetPrice"] * (ml["SellFeeRate"] / 100)) + ml["NetPrice"];

            Mathset ml2 = rck.GetMathset("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck.Compute(); // first with formula compilation

            rck.Compute(); // second when formula compiled
        }

        /// <summary>
        /// The Mathset_Computation_LogicOnStack_Formula_Test.
        /// </summary>
        [Fact]
        public void Mathset_Computation_LogicOnStack_Formula_Test()
        {
            rck = new Computation(spcMtic);

            Mathset ml = rck.GetMathset("SellNetPrice");

            ml.Formula = (((ml["NetCost"] < 10) | (ml["NetCost"] > 50)) * (ml["NetPrice"] * (ml["SellFeeRate"] / 100)) + ml["NetPrice"]) +
                          ((ml["NetCost"] > 10) & (ml["NetCost"] < 50)) * ml["NetPrice"];

            Mathset ml2 = rck.GetMathset("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck.Compute(); // first with formula compilation

            rck.Compute(); // second when formula compiled
        }

        /// <summary>
        /// The Mathset_Figure_Helper_Test.
        /// </summary>
        /// <param name="str">The str<see cref="IFigures"/>.</param>
        /// <param name="fom">The fom<see cref="MathsetMockModel"/>.</param>
        /// <returns>The <see cref="IFigure"/>.</returns>
        private IFigure Mathset_Figure_Helper_Test(IFigures str, MathsetMockModel fom)
        {
            IFigure rts = str.NewFigure();

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                MemberInfo r = str.Rubrics[i].RubricInfo;
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
