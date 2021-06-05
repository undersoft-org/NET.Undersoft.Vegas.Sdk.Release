using System.Linq;
using System.Uniques;
using System.Sets;

namespace System.Instant.Mathset
{
    public class Computation : IComputation
    {
        private MathRubrics computation;    

        public Computation(IFigures data)
        {
            computation = new MathRubrics(data);
            serialcode.UniqueKey = (ulong)DateTime.Now.ToBinary();
            if (data.Computations == null)
                data.Computations = new Deck<IComputation>();
            data.Computations.Put(this);
        }

        public Mathset this[int id]
        {
            get
            {
                return GetMathset(id);
            }
        }
        public Mathset this[string name]
        {
            get
            {
               return GetMathset(name);
            }
        }
        public Mathset this[MemberRubric rubric]
        {
            get
            {
                return GetMathset(rubric);
            }
        }

        public Mathset GetMathset(int id)
        {
            MemberRubric rubric = computation.Rubrics[id];
            if (rubric != null)
            {
                MathRubric mathrubric = null;
                if (computation.MathsetRubrics.TryGet(rubric.Name, out mathrubric))
                    return mathrubric.GetMathset();
                return computation.Put(rubric.Name, new MathRubric(computation, rubric)).Value.GetMathset();
            }
            return null;
        }
        public Mathset GetMathset(string name)
        {
            MemberRubric rubric = null;
            if (computation.Rubrics.TryGet(name, out rubric))
            {
                MathRubric mathrubric = null;
                if (computation.MathsetRubrics.TryGet(name, out mathrubric))
                    return mathrubric.GetMathset();
                return computation.Put(rubric.Name, new MathRubric(computation, rubric)).Value.GetMathset();
            }
            return null;
        }
        public Mathset GetMathset(MemberRubric rubric)
        {
            return GetMathset(rubric.Name);
        }

        public bool ContainsFirst(MemberRubric rubric)
        {
            return computation.First.Value.RubricName == rubric.Name;
        }
        public bool ContainsFirst(string rubricName)
        {
            return computation.First.Value.RubricName == rubricName;
        }

        public IFigures Compute()
        {
            computation.Combine();
            computation.AsValues().Where(p => !p.PartialMathset).OrderBy(p => p.ComputeOrdinal).Select(p => p.Compute()).ToArray();
            return computation.Data;
        }

        private Ussn serialcode;
        public Ussn SerialCode { get => serialcode; set => serialcode = value; }
        public IUnique Empty => Ussn.Empty;
        public ulong UniqueKey
        { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }
       

        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }
        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }
        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }
        public ulong UniqueSeed
        { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }
    }
}
