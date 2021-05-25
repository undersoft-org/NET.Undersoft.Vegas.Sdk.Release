using System.Multemic;
using System.Instant.Linkmap;
using System.Reflection;
using Xunit;

namespace System.Instant
{   
    public class FiguresLinkmapTest
    {
        private DateTime seedKeyTick = DateTime.Now;

        private IFigure FiguresLinkmap_SetValues_Helper_Test(IFigures str, object fom)
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

        private IFigures FiguresLinkmap_AddFigures_Helper_Test(IFigures figures)
        {
            IFigures _figures = figures;
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            IFigure figureMock = FiguresLinkmap_SetValues_Helper_Test(_figures, fom);
            int idSeed = (int)figureMock["Id"];
            for (int i = 0; i < 100000; i++)
            {
                IFigure figure = _figures.NewFigure();               
                figure.ValueArray = figureMock.ValueArray;
                figure["Id"] = idSeed + i;
                figure["Time"] = seedKeyTick;              
                _figures.Put(figure);               
            }
            return _figures;
        }

        [Fact]
        public void FiguresLinkmap__Test()
        {
            IFigures figuresA = new Figures(typeof(FieldsAndPropertiesModel), "Figures_A_Test").Generate();

            IFigures figuresB = new Figures(typeof(FieldsAndPropertiesModel), "Figures_B_Test").Generate();

            FiguresLinkmap_AddFigures_Helper_Test(figuresA);
     
            FiguresLinkmap_AddFigures_Helper_Test(figuresB);

            FigureLink fl = new FigureLink(figuresA, figuresB);
            fl.OriginKeys.Put(figuresA.Rubrics.KeyRubrics.AsValues());
            fl.TargetKeys.Put(figuresB.Rubrics.KeyRubrics.AsValues());

            // LinkBranches targetsA = figuresA.Linkmap.CreateTargetLinks();

            // LinkBranches originsB = figuresB.Linkmap.LinkOrigins();
        }

     
    }
}
