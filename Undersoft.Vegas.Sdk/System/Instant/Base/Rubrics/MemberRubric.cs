/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.MemberRubric.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (30.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Instant.Linking;
    using System.Instant.Treatments;
    using System.Linq;
    using System.Reflection;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="MemberRubric" />.
    /// </summary>
    public class MemberRubric : MemberInfo, IRubric
    {
        #region Fields

        private Ussn serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="field">The field<see cref="FieldInfo"/>.</param>
        public MemberRubric(FieldInfo field) : this((IMemberRubric)new FieldRubric(field))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="field">The field<see cref="FieldRubric"/>.</param>
        public MemberRubric(FieldRubric field) : this((IMemberRubric)field)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="IMemberRubric"/>.</param>
        public MemberRubric(IMemberRubric member)
        {
            RubricInfo = ((MemberInfo)member);
            RubricName = member.RubricName;
            RubricId = member.RubricId;
            Visible = member.Visible;
            Editable = member.Editable;
            if (RubricInfo.MemberType == MemberTypes.Method)
                serialcode.UniqueKey = (RubricName + "_" + new String(RubricParameterInfo
                                                    .SelectMany(p => p.ParameterType.Name)
                                                        .ToArray())).UniqueKey64();
            else
                serialcode.UniqueKey = RubricName.UniqueKey64();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="member">The member<see cref="MemberRubric"/>.</param>
        public MemberRubric(MemberRubric member) : this(member.RubricInfo != null ? (IMemberRubric)member.RubricInfo : member)
        {
            FigureType = member.FigureType;
            FigureField = member.FigureField;
            FieldId = member.FieldId;
            RubricOffset = member.RubricOffset;
            IsKey = member.IsKey;
            IsIdentity = member.IsIdentity;
            IsAutoincrement = member.IsAutoincrement;
            IdentityOrder = member.IdentityOrder;
            Required = member.Required;
            DisplayName = member.DisplayName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="method">The method<see cref="MethodInfo"/>.</param>
        public MemberRubric(MethodInfo method) : this((IMemberRubric)new MethodRubric(method))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="method">The method<see cref="MethodRubric"/>.</param>
        public MemberRubric(MethodRubric method) : this((IMemberRubric)method)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="property">The property<see cref="PropertyInfo"/>.</param>
        public MemberRubric(PropertyInfo property) : this((IMemberRubric)new PropertyRubric(property))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRubric"/> class.
        /// </summary>
        /// <param name="property">The property<see cref="PropertyRubric"/>.</param>
        public MemberRubric(PropertyRubric property) : this((IMemberRubric)property)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the AggregateLinkId.
        /// </summary>
        public int AggregateLinkId { get; set; }

        /// <summary>
        /// Gets or sets the AggregateLinks.
        /// </summary>
        public Links AggregateLinks { get; set; }

        /// <summary>
        /// Gets or sets the AggregateOperand.
        /// </summary>
        public AggregateOperand AggregateOperand { get; set; }

        /// <summary>
        /// Gets or sets the AggregateOrdinal.
        /// </summary>
        public int AggregateOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the AggregateRubric.
        /// </summary>
        public IRubric AggregateRubric { get; set; }

        /// <summary>
        /// Gets the DeclaringType.
        /// </summary>
        public override Type DeclaringType => FigureType != null ? FigureType : RubricInfo.DeclaringType;

        /// <summary>
        /// Gets or sets the DisplayName.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Editable.
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => Ussn.Empty;

        /// <summary>
        /// Gets or sets the FieldId.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Gets or sets the FigureField.
        /// </summary>
        public FieldInfo FigureField { get; set; }

        /// <summary>
        /// Gets or sets the FigureType.
        /// </summary>
        public Type FigureType { get; set; }

        /// <summary>
        /// Gets or sets the IdentityOrder.
        /// </summary>
        public short IdentityOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsAutoincrement.
        /// </summary>
        public bool IsAutoincrement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsColossus.
        /// </summary>
        public bool IsColossus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDBNull.
        /// </summary>
        public bool IsDBNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsIdentity.
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsKey.
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// Gets the MemberType.
        /// </summary>
        public override MemberTypes MemberType => RubricInfo.MemberType;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public override string Name => RubricInfo.Name;

        /// <summary>
        /// Gets the ReflectedType.
        /// </summary>
        public override Type ReflectedType => RubricInfo.ReflectedType;

        /// <summary>
        /// Gets or sets a value indicating whether Required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the RubricAttributes.
        /// </summary>
        public object[] RubricAttributes
        {
            get { return VirtualInfo.RubricAttributes; }
            set { VirtualInfo.RubricAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the RubricId.
        /// </summary>
        public int RubricId { get; set; }

        /// <summary>
        /// Gets or sets the RubricInfo.
        /// </summary>
        public MemberInfo RubricInfo { get; set; }

        /// <summary>
        /// Gets the RubricModule.
        /// </summary>
        public Module RubricModule { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricModule : null; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        public string RubricName { get; set; }

        /// <summary>
        /// Gets or sets the RubricOffset.
        /// </summary>
        public int RubricOffset { get; set; }

        /// <summary>
        /// Gets the RubricParameterInfo.
        /// </summary>
        public ParameterInfo[] RubricParameterInfo { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricParameterInfo : null; }

        /// <summary>
        /// Gets the RubricReturnType.
        /// </summary>
        public Type RubricReturnType { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricReturnType : null; }

        /// <summary>
        /// Gets or sets the Rubrics.
        /// </summary>
        public MemberRubrics Rubrics { get; set; }

        /// <summary>
        /// Gets or sets the RubricSize.
        /// </summary>
        public int RubricSize
        {
            get { return VirtualInfo.RubricSize; }
            set { VirtualInfo.RubricSize = value; }
        }

        /// <summary>
        /// Gets or sets the RubricType.
        /// </summary>
        public Type RubricType
        {
            get { return VirtualInfo.RubricType; }
            set { VirtualInfo.RubricType = value; }
        }

        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Ussn SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the SummaryOperand.
        /// </summary>
        public AggregateOperand SummaryOperand { get; set; }

        /// <summary>
        /// Gets or sets the SummaryOrdinal.
        /// </summary>
        public int SummaryOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the SummaryRubric.
        /// </summary>
        public IRubric SummaryRubric { get; set; }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.SetUniqueKey(value); }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.SetUniqueSeed(value); }

        /// <summary>
        /// Gets the VirtualInfo.
        /// </summary>
        public IMemberRubric VirtualInfo => (IMemberRubric)RubricInfo;

        /// <summary>
        /// Gets or sets a value indicating whether Visible.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return (int)(UniqueKey - other.UniqueKey);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return UniqueKey == other.UniqueKey;
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        /// <summary>
        /// The GetCustomAttributes.
        /// </summary>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public override object[] GetCustomAttributes(bool inherit)
        {
            return RubricInfo.GetCustomAttributes(inherit);
        }

        /// <summary>
        /// The GetCustomAttributes.
        /// </summary>
        /// <param name="attributeType">The attributeType<see cref="Type"/>.</param>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return RubricInfo.GetCustomAttributes(attributeType, inherit);
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        /// <summary>
        /// The IsDefined.
        /// </summary>
        /// <param name="attributeType">The attributeType<see cref="Type"/>.</param>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return RubricInfo.IsDefined(attributeType, inherit);
        }

        #endregion
    }
}
