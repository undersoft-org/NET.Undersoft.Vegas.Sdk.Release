using System.Linq;
using System.Uniques;
using System.Sets;

namespace System.Instant.Mathset
{
    public class MathRubrics : BaseAlbum<MathRubric>
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
            MathsetRubrics = new MathRubrics(Rubrics);
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

        public MathRubrics MathsetRubrics
        { get; set; } 
        public MathRubrics FormulaRubrics
        { get; set; } 

        public bool Combine(IFigures table)
        {
            if (!ReferenceEquals(Data, table))
            {
                Data = table;
                CombinedMathset[] evs = CombineEvaluators();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineEvaluators();
            return false;
        }
        public bool Combine()
        {
            if (!ReferenceEquals(Data, null))
            {
                CombinedMathset[] evs = CombineEvaluators();
                bool[] b = evs.Select(e => e.SetParams(Data, 0)).ToArray();
                return true;
            }
            CombineEvaluators();
            return false;

        }

        public CombinedMathset[] CombineEvaluators()
        {
            return this.AsValues().Select(m => m.CombineEvaluator()).ToArray();
        }

        public override  ICard<MathRubric> EmptyCard()
        {
            return new MathRubricCard();
        }
        public override  ICard<MathRubric>[] EmptyCardTable(int size)
        {
            return new MathRubricCard[size];
        }
        public override  ICard<MathRubric>[] EmptyBaseDeck(int size)
        {
            return new MathRubricCard[size];
        }
                         
        public override  ICard<MathRubric> NewCard(object key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }
        public override  ICard<MathRubric> NewCard(ulong key, MathRubric value)
        {
            return new MathRubricCard(key, value);
        }
        public override  ICard<MathRubric> NewCard(ICard<MathRubric> value)
        {
            return new MathRubricCard(value);
        }
        public override  ICard<MathRubric> NewCard(MathRubric value)
        {
            return new MathRubricCard(value);
        }

    }


}
