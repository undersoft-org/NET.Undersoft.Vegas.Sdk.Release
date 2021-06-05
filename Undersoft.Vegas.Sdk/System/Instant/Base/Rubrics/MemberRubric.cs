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

    public class MemberRubric : MemberInfo, IRubric
    {
        #region Fields

        private Ussn serialcode;

        #endregion

        #region Constructors

        public MemberRubric(FieldInfo field) : this((IMemberRubric)new FieldRubric(field))
        {
        }
        public MemberRubric(FieldRubric field) : this((IMemberRubric)field)
        {
        }
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
        public MemberRubric(MethodInfo method) : this((IMemberRubric)new MethodRubric(method))
        {
        }
        public MemberRubric(MethodRubric method) : this((IMemberRubric)method)
        {
        }
        public MemberRubric(PropertyInfo property) : this((IMemberRubric)new PropertyRubric(property))
        {
        }
        public MemberRubric(PropertyRubric property) : this((IMemberRubric)property)
        {
        }

        #endregion

        #region Properties

        public int AggregateLinkId { get; set; }

        public Links AggregateLinks { get; set; }

        public AggregateOperand AggregateOperand { get; set; }

        public int AggregateOrdinal { get; set; }

        public IRubric AggregateRubric { get; set; }

        public override Type DeclaringType => FigureType != null ? FigureType : RubricInfo.DeclaringType;

        public string DisplayName { get; set; }

        public bool Editable { get; set; }

        public IUnique Empty => Ussn.Empty;

        public FieldInfo FigureField { get; set; }

        public int FieldId { get; set; }

        public Type FigureType { get; set; }

        public short IdentityOrder { get; set; }

        public bool IsAutoincrement { get; set; }

        public bool IsColossus { get; set; }

        public bool IsDBNull { get; set; }

        public bool IsIdentity { get; set; }

        public bool IsKey { get; set; }

        public override MemberTypes MemberType => RubricInfo.MemberType;

        public override string Name => RubricInfo.Name;

        public override Type ReflectedType => RubricInfo.ReflectedType;

        public bool Required { get; set; }

        public object[] RubricAttributes
        {
            get { return VirtualInfo.RubricAttributes; }
            set { VirtualInfo.RubricAttributes = value; }
        }

        public int RubricId { get; set; }

        public MemberInfo RubricInfo { get; set; }

        public Module RubricModule { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricModule : null; }

        public string RubricName { get; set; }

        public int RubricOffset { get; set; }

        public ParameterInfo[] RubricParameterInfo { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricParameterInfo : null; }

        public Type RubricReturnType { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricReturnType : null; }

        public MemberRubrics Rubrics { get; set; }

        public int RubricSize
        {
            get { return VirtualInfo.RubricSize; }
            set { VirtualInfo.RubricSize = value; }
        }

        public Type RubricType
        {
            get { return VirtualInfo.RubricType; }
            set { VirtualInfo.RubricType = value; }
        }

        public Ussn SerialCode { get => serialcode; set => serialcode = value; }

        public AggregateOperand SummaryOperand { get; set; }

        public int SummaryOrdinal { get; set; }

        public IRubric SummaryRubric { get; set; }

        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.SetUniqueKey(value); }

        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.SetUniqueSeed(value); }

        public IMemberRubric VirtualInfo => (IMemberRubric)RubricInfo;

        public bool Visible { get; set; }

        #endregion

        #region Methods

        public int CompareTo(IUnique other)
        {
            return (int)(UniqueKey - other.UniqueKey);
        }

        public bool Equals(IUnique other)
        {
            return UniqueKey == other.UniqueKey;
        }

        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return RubricInfo.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return RubricInfo.GetCustomAttributes(attributeType, inherit);
        }

        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return RubricInfo.IsDefined(attributeType, inherit);
        }

        #endregion
    }
}
