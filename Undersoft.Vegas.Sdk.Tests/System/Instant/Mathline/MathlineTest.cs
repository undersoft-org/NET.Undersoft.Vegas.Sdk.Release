using System.Reflection;
using Xunit;

namespace System.Instant.Mathline
{   
    public class MathlineTest
    {
        private Figure instFig;
        private Figures instMtic;
        private Computation rck;
        private IFigures spcMtic;

        private IFigure Mathline_Figure_Helper_Test(IFigures str, MathlineMockModel fom)
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

        public MathlineTest()
        {
            instFig = new Figure(typeof(MathlineMockModel));
           
            instMtic = new Figures(instFig, "Figures_Mathline_Test");

            spcMtic = instMtic.Generate();

            MathlineMockModel fom = new MathlineMockModel();

            for (int i = 0; i < 1000 * 1000; i++)
            {
                var f = Mathline_Figure_Helper_Test(spcMtic, fom);
                f["NetPrice"] = (double)f["NetPrice"] + i;
                f["NetCost"] = (double)f["NetPrice"] / 2;
                spcMtic.Add(i, f);
            }            

        }

        [Fact]
        public void Mathline_Computation_Formula_Test()
        {
            rck = new Computation(spcMtic);

            Mathline ml = rck.GetMathline("SellNetPrice");

            ml.Formula = (ml["NetPrice"] * (ml["SellFeeRate"] / 100)) + ml["NetPrice"];

            Mathline ml2 = rck.GetMathline("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck.Compute(); // first with formula compilation

            rck.Compute(); // second when formula compiled
        }

        [Fact]
        public void Mathline_Computation_LogicOnStack_Formula_Test()
        {
            rck = new Computation(spcMtic);

            Mathline ml = rck.GetMathline("SellNetPrice");

            ml.Formula = (((ml["NetCost"] < 10) | (ml["NetCost"] > 50)) * (ml["NetPrice"] * (ml["SellFeeRate"] / 100)) + ml["NetPrice"]) +
                          ((ml["NetCost"] > 10) & (ml["NetCost"] < 50)) * ml["NetPrice"];

            Mathline ml2 = rck.GetMathline("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck.Compute(); // first with formula compilation

            rck.Compute(); // second when formula compiled
        }
    }
}
