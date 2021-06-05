using System.Runtime.InteropServices;
using System.Instant.Treatment;

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

        public UnmanagedType ArraySubType;       
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
