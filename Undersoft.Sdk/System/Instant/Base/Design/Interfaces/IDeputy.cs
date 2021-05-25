using System.Reflection;
using System.Multemic;
using System.Threading.Tasks;

namespace System.Instant
{ 
    public interface IDeputy : IFigure
    {
        MethodInfo Info { get; set; }
        ParameterInfo[] Parameters { get; set; }
        object[] ParameterValues { get; set; }

        object Execute(params object[] parameters);
    }
}
