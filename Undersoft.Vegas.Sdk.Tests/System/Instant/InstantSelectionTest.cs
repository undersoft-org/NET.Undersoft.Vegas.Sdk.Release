using System;
using System.Uniques;
using System.Extract;
using System.Reflection;
using Xunit;

namespace System.Instant.Tests
{   
    public class SleeveTest
    {
        private Figures rtsq;

        private IFigures iRtseq;
        private IFigure iRts; 
        private IFigure Sleeve_Compilation_Helper_Test(IFigures str, FieldsAndPropertiesModel fom)
        {
            IFigure rts = str.NewFigure();          

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
        public void Sleeve_SelectionFromFiguresMultiNesting_Test()
        {
            rtsq = new Figures(typeof(FieldsAndPropertiesModel), "InstantSequence_Compilation_Test", true);
            iRtseq = rtsq.Combine();
           
            iRts = Sleeve_Compilation_Helper_Test(iRtseq, new FieldsAndPropertiesModel());
            
            int idSeed = (int)iRts["Id"];
            DateTime now = DateTime.Now;
            for (int i = 0; i < 250000; i++)
            {
                IFigure _iRts = iRtseq.NewFigure();
                _iRts.ValueArray = iRts.ValueArray;              
                _iRts["Id"] = idSeed + i;
                _iRts["Time"] = now;
                iRtseq.Add(_iRts);
            }

            ulong[] keyarray = new ulong[60 * 1000];
            for (   int i = 0; i < 60000; i++)
            {
                keyarray[i] = Unique.NewKey;               
            }

            iRtseq.Add(iRtseq.NewFigure());
            iRtseq[0, 4] = iRts[4];

            IFigures isel1 = new Sleeves(iRtseq).Combine();
            IFigures isel2 = new Sleeves(isel1).Combine();

            foreach (var card in iRtseq)
                isel2.Add(card);

            iRts = Sleeve_Compilation_Helper_Test(iRtseq, new FieldsAndPropertiesModel());

            isel2.Add(iRts);
            isel2[0, 4] = iRts[4];

            Assert.Equal(iRts[4], isel2[0, 4]);
            Assert.Equal(iRtseq.Count, isel1.Count);
            Assert.Equal(isel1.Count, isel2.Count);

        }
      
    }
}
