using System.Linq;
using System.Uniques;
using System.Multemic;

namespace System.Instant.Mathline
{
    public class Computation : IComputation
    {
        private MathRubrics computation;    

        public Computation(IFigures data)
        {
            computation = new MathRubrics(data);
            systemSerialCode.KeyBlock = DateTime.Now.ToBinary();
            if (data.Computations == null)
                data.Computations = new Deck<IComputation>();
            data.Computations.Put(this);
        }

        public Mathline this[int id]
        {
            get
            {
                return GetMathline(id);
            }
        }
        public Mathline this[string name]
        {
            get
            {
               return GetMathline(name);
            }
        }
        public Mathline this[MemberRubric rubric]
        {
            get
            {
                return GetMathline(rubric);
            }
        }

        public Mathline GetMathline(int id)
        {
            MemberRubric rubric = computation.Rubrics[id];
            if (rubric != null)
            {
                MathRubric mathrubric = null;
                if (computation.MathlineRubrics.TryGet(rubric.Name, out mathrubric))
                    return mathrubric.GetMathline();
                return computation.Put(rubric.Name, new MathRubric(computation, rubric)).Value.GetMathline();
            }
            return null;
        }
        public Mathline GetMathline(string name)
        {
            MemberRubric rubric = null;
            if (computation.Rubrics.TryGet(name, out rubric))
            {
                MathRubric mathrubric = null;
                if (computation.MathlineRubrics.TryGet(name, out mathrubric))
                    return mathrubric.GetMathline();
                return computation.Put(rubric.Name, new MathRubric(computation, rubric)).Value.GetMathline();
            }
            return null;
        }
        public Mathline GetMathline(MemberRubric rubric)
        {
            return GetMathline(rubric.Name);
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
            computation.AsValues().Where(p => !p.PartialMathline).OrderBy(p => p.ComputeOrdinal).Select(p => p.Compute()).ToArray();
            return computation.Data;
        }

        private Ussn systemSerialCode;
        public Ussn SystemSerialCode { get => systemSerialCode; set => systemSerialCode = value; }
        public IUnique Empty => Ussn.Empty;
        public long KeyBlock
        { get => SystemSerialCode.KeyBlock; set => systemSerialCode.KeyBlock = value; }
       

        public int CompareTo(IUnique other)
        {
            return systemSerialCode.CompareTo(other);
        }
        public bool Equals(IUnique other)
        {
            return systemSerialCode.Equals(other);
        }
        public byte[] GetBytes()
        {
            return systemSerialCode.GetBytes();
        }
        public long GetHashKey()
        {
            return systemSerialCode.GetHashKey();
        }
        public byte[] GetKeyBytes()
        {
            return systemSerialCode.GetKeyBytes();
        }
        public void SetHashKey(long value)
        {
            systemSerialCode.SetHashKey(value);
        }

        public uint SeedBlock
        { get => systemSerialCode.SeedBlock; set => systemSerialCode.SeedBlock = value; }

        public void SetHashSeed(uint seed)
        {
            systemSerialCode.SetHashSeed(seed);
        }

        public uint GetHashSeed()
        {
            return systemSerialCode.GetHashSeed();
        }
    }
}
