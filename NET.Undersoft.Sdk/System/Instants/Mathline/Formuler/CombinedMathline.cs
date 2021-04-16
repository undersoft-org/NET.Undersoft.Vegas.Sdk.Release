
namespace System.Instants.Mathline
{
	/// Class for the Generated Code
	public abstract class CombinedReckoner
    { 
        public int ParametersCount = 0;

        public IFigures[] DataParameters = new IFigures[1];

        public abstract void Reckon();

        public void SetParams(IFigures[] p, int paramCount)
        {
            DataParameters = p;
            ParametersCount = paramCount;
        }
        public bool SetParams(IFigures p, int index)
        {
            if (index < ParametersCount)
            {
                if (ReferenceEquals(DataParameters[index], p))
                    return false;
                else
                    DataParameters[index] = p;
            }
            return false;
        }
        public void SetParams(IFigures p)
        {
            Put(p);
        }

        public int Put(IFigures v)
        {
            int index = GetIndexOf(v);
            if (index < 0)
            {
                DataParameters[ParametersCount] = v;
                return 1 + ParametersCount++;
            }
            else
            {
                DataParameters[index] = v;
            }
            return index;
        }

        public int GetIndexOf(IFigures v)
        {
            for (int i = 0; i < ParametersCount; i++)
                if (DataParameters[i] == v) return 1 + i;
            return -1;
        }
    
        public int GetRowCount(int paramid)
        {            
            return DataParameters[paramid].Count;
        }

        public int GetColumnCount(int paramid)
        {
            return DataParameters[paramid].Rubrics.Count;
        }

    }
}
