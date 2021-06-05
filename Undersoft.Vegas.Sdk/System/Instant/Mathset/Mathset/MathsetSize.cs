using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Instant.Mathset
{
    [Serializable]
    public class MathsetSize
    {
        public MathsetSize(int i, int j)
        {
            rows = i;
            cols = j;
        }

        public int rows;
        public int cols;

        public static MathsetSize Scalar = new MathsetSize(1, 1);

        public override bool Equals(object o)
        {
            if (o is MathsetSize) return ((MathsetSize)o) == this;
            return false;
        }

        public static bool operator !=(MathsetSize o1, MathsetSize o2)
        {
            return o1.rows != o2.rows || o1.cols != o2.cols;
        }

        public static bool operator ==(MathsetSize o1, MathsetSize o2)
        {
            return o1.rows == o2.rows && o1.cols == o2.cols;
        }

        public override int GetHashCode()
        {
            return rows * cols;
        }

        public override string ToString()
        {
            return "" + rows + " " + cols;
        }
    }
}
