using System.Runtime.InteropServices;
using System.Instant.Treatments;

namespace System.Instant
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureAttribute : Attribute
    {
        public FigureAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureAsAttribute : FigureAttribute
    {
        public UnmanagedType Value;

        public FigureAsAttribute(UnmanagedType unmanaged)
        {
            Value = unmanaged;
        }        

        public int SizeConst;     
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureSizeAttribute : FigureAttribute
    {
        public UnmanagedType Value;

        public FigureSizeAttribute(int size)
        {
            SizeConst = size;
        }

        public int SizeConst;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureLinkAttribute : FigureAttribute
    {
        public Type TargetType;
        public string TargetName;


        public FigureLinkAttribute(Type targetType, string linkRubric)
        {
            TargetType = targetType;
            TargetName = targetType.Name;
            LinkRubric = linkRubric;
        }
        public FigureLinkAttribute(string targetName, string linkRubric)
        {
            TargetName = targetName;
            LinkRubric = linkRubric;
        }

        public string LinkRubric;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureIdentityAttribute : FigureAttribute
    {      
        public FigureIdentityAttribute()
        {
        }

        public bool IsAutoincrement = false;

        public short Order = 0;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureKeyAttribute : FigureIdentityAttribute
    {
        public FigureKeyAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureRequiredAttribute : FigureAttribute
    {
        public FigureRequiredAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureDisplayAttribute : FigureAttribute
    {
        public string Name;

        public FigureDisplayAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureTreatmentAttribute : FigureAttribute
    {
        public FigureTreatmentAttribute()
        {
        }

        public AggregateOperand AggregateOperand = AggregateOperand.None;

        public AggregateOperand SummaryOperand = AggregateOperand.None;
    }
}
