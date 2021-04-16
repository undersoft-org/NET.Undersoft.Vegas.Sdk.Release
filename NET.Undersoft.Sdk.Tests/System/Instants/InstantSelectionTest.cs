using System.Uniques;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.Instants
{   
    public class InstantSelectionTest
    {
        private InstantFigure str;
        private InstantFigures rtsq;
        private InstantSelection isel;

        private IFigures iRtseq;
        private IFigure iRts; 
        private IFigure InstantSelection_Compilation_Helper_Test(IFigures str, FieldsAndPropertiesModel fom)
        {
            IFigure rts = str.NewFigure();          

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)((MemberRubric)r).RubricInfo).Name);
                    if (fi != null)
                        rts[r.Name] = fi.GetValue(fom);
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)((MemberRubric)r).RubricInfo).Name);
                    if (pi != null)
                        rts[r.Name] = pi.GetValue(fom);
                }
            }

            return rts;
           
        }        

        [Fact]
        public void InstantSelection_SelectionFromFiguresMultiNesting_Test()
        {
            str = new InstantFigure(InstantFigureMocks.InstantFigure_MemberRubric_FieldsAndPropertiesModel(),
                                                                 "InstantFigure_MemberRubric_FieldsAndPropertiesModel");

            rtsq = new InstantFigures(str, "InstantSequence_Compilation_Test");
            iRtseq = rtsq.New();
            iRtseq.Rubrics.KeyRubrics.Add(new[] { iRtseq.Rubrics["Id"], iRtseq.Rubrics["Time"] });
           

            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            iRts = InstantSelection_Compilation_Helper_Test(iRtseq, fom);

            /// prepare local keyids
            int[] keyids = iRtseq.Rubrics.KeyRubrics.AsValues().Select(ki => ki.FigureFieldId).ToArray();
            for (int i = 0; i < 10000; i++)
            {
                var _iRts = iRtseq.NewFigure();
                _iRts.ValueArray = iRts.ValueArray;

                iRtseq.Add(keyids.Select(ki => _iRts[ki]).ToArray(), _iRts);

                iRts["Id"] = (int)iRts["Id"] + i;
                iRts["Time"] = DateTime.Now;
            }

            /// lazy spaghetti - each time get from keyrubrics - obviously 20% slower
            for (int i = 10000; i < 20000; i++)
            {
                var _iRts = iRtseq.NewFigure();
                _iRts.ValueArray = iRts.ValueArray;
                var c = iRtseq.NewCard(_iRts);
                c.SetHashKey(c.GetKeyIdValues().GetHashKey());
                iRtseq.Add(c);
                iRts["Id"] = (int)iRts["Id"] + i;
                iRts["Time"] = DateTime.Now;
            }


            iRtseq.Add(iRtseq.NewFigure());
            iRtseq[0, 4] = iRts[4];

            isel = new InstantSelection(iRtseq);

            IFigures ifsel = isel.New();

            IFigures isel2 = new InstantSelection(ifsel).New();

            foreach (var card in iRtseq)
                isel2.Add(card);

            iRts = InstantSelection_Compilation_Helper_Test(iRtseq, fom);
            var c1 = iRtseq.NewCard(iRts);
            c1.SetHashKey(c1.GetKeyIdValues().GetHashKey());

            isel2.Add(c1);
            isel2[0, 4] = iRts[4];

            Assert.Equal(iRts[4], isel2[0, 4]);
            Assert.Equal(iRtseq.Count, ifsel.Count);
            Assert.Equal(ifsel.Count, isel2.Count);

        }
      
    }
}
