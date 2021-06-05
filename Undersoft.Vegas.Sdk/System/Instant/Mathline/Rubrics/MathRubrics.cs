using System.Linq;
using System.Uniques;
using System.Multemic;

namespace System.Instant.Mathline
{
    public class MathRubrics : CardBook<MathRubric>
    {
        public int RubricsCount
        {
            get
            {
                return Rubrics.Count;
            }
        }
        public int RowsCount
        {
            get
            {
                return Data.Count;
            }
        }

        public IFigures Data { get; set; }

        public MathRubrics(IFigures data)
        {
            Rubrics = data.Rubrics;
            FormulaRubrics = new MathRubrics(Rubrics);
            MathlineRubrics = new MathRubrics(Rubrics);
            Data = data;
        }
        public MathRubrics(IRubrics rubrics)
        {
            Rubrics = rubrics;
            Data = rubrics.Figures;
        }
        public MathRubrics(MathRubrics rubrics)
        {
            Rubrics = rubrics.Rubrics;
            Data = rubrics.Data;
        }      

        public IRubrics Rubrics
        { get; set; }

        public MathRubrics MathlineRubrics
        { get; set; } 
        public MathRubrics FormulaRubrics
        { get; set; } 

        public bool Combine(IFigures table)
        {
            if (!ReferenceEquals(Data, table))
            {
                Data = table;
                CombinedComputer[] evs = CombineComputers();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineComputers();
            return false;
        }
        public bool Combine()
        {
            if (!ReferenceEquals(Data, null))
            {
                CombinedComputer[] evs = CombineComputers();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineComputers();
            return false;

        }

        public CombinedComputer[] CombineComputers()
        {
            return this.AsValues().Select(m => m.CombineComputer()).ToArray();
        }

        public override  ICard<MathRubric> EmptyCard()
        {
            return new MathRubricCard();
        }
        public override  ICard<MathRubric>[] EmptyCardTable(int size)
        {
            return new MathRubricCard[size];
        }
        public override  ICard<MathRubric>[] EmptyCardList(int size)
        {
            return new MathRubricCard[size];
        }
                         
        public override  ICard<MathRubric> NewCard(object key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }
        public override  ICard<MathRubric> NewCard(long key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }
        public override  ICard<MathRubric> NewCard(ICard<MathRubric> value)
        {
            return new MathRubricCard(value);
        }
        public override  ICard<MathRubric> NewCard(MathRubric value)
        {
            return new MathRubricCard(value.GetHashKey(), value);
        }

    }


}
