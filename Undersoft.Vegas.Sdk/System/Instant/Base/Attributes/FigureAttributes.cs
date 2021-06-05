/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureAttributes.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Instant.Treatments;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="FigureAsAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureAsAttribute : FigureAttribute
    {
        #region Fields

        public int SizeConst;
        public UnmanagedType Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureAsAttribute"/> class.
        /// </summary>
        /// <param name="unmanaged">The unmanaged<see cref="UnmanagedType"/>.</param>
        public FigureAsAttribute(UnmanagedType unmanaged)
        {
            Value = unmanaged;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureAttribute"/> class.
        /// </summary>
        public FigureAttribute()
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureDisplayAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureDisplayAttribute : FigureAttribute
    {
        #region Fields

        public string Name;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureDisplayAttribute"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        public FigureDisplayAttribute(string name)
        {
            Name = name;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureIdentityAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureIdentityAttribute : FigureAttribute
    {
        #region Fields

        public bool IsAutoincrement = false;
        public short Order = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureIdentityAttribute"/> class.
        /// </summary>
        public FigureIdentityAttribute()
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureKeyAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureKeyAttribute : FigureIdentityAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureKeyAttribute"/> class.
        /// </summary>
        public FigureKeyAttribute()
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureLinkAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureLinkAttribute : FigureAttribute
    {
        #region Fields

        public string LinkRubric;
        public string TargetName;
        public Type TargetType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureLinkAttribute"/> class.
        /// </summary>
        /// <param name="targetName">The targetName<see cref="string"/>.</param>
        /// <param name="linkRubric">The linkRubric<see cref="string"/>.</param>
        public FigureLinkAttribute(string targetName, string linkRubric)
        {
            TargetName = targetName;
            LinkRubric = linkRubric;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureLinkAttribute"/> class.
        /// </summary>
        /// <param name="targetType">The targetType<see cref="Type"/>.</param>
        /// <param name="linkRubric">The linkRubric<see cref="string"/>.</param>
        public FigureLinkAttribute(Type targetType, string linkRubric)
        {
            TargetType = targetType;
            TargetName = targetType.Name;
            LinkRubric = linkRubric;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureRequiredAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureRequiredAttribute : FigureAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureRequiredAttribute"/> class.
        /// </summary>
        public FigureRequiredAttribute()
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureSizeAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureSizeAttribute : FigureAttribute
    {
        #region Fields

        public int SizeConst;
        public UnmanagedType Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureSizeAttribute"/> class.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        public FigureSizeAttribute(int size)
        {
            SizeConst = size;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FigureTreatmentAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureTreatmentAttribute : FigureAttribute
    {
        #region Fields

        public AggregateOperand AggregateOperand = AggregateOperand.None;
        public AggregateOperand SummaryOperand = AggregateOperand.None;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureTreatmentAttribute"/> class.
        /// </summary>
        public FigureTreatmentAttribute()
        {
        }

        #endregion
    }
}
