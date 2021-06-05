using System.Reflection;
using System.Linq;
using System.Uniques;
using System.Instant.Linkmap;
using System.Instant.Treatment;

namespace System.Instant
{
    public class MemberRubric : MemberInfo, IRubric
    {                       
        public MemberRubric(IMemberRubric member)
        {
            RubricInfo = ((MemberInfo)member);
            RubricName = member.RubricName;
            RubricId = member.RubricId;
            Visible = member.Visible;
            Editable = member.Editable;
            if (RubricInfo.MemberType == MemberTypes.Method)
                SystemSerialCode = new Ussn((new String(RubricParameterInfo
                                            .SelectMany(p => p.ParameterType.Name)
                                                .ToArray()) + "_" + RubricName).GetHashKey64());
            else
                SystemSerialCode = new Ussn(RubricName.GetHashKey64());


        }
        public MemberRubric(MemberRubric member) : this(member.RubricInfo != null ? (IMemberRubric)member.RubricInfo : (IMemberRubric)member)
        {
            FigureType = member.FigureType;
            FigureField = member.FigureField;
            FigureFieldId = member.FigureFieldId;
            RubricOffset = member.RubricOffset;
            IsKey = member.IsKey;
            IsIdentity = member.IsIdentity;
            IsAutoincrement = member.IsAutoincrement;
            IdentityOrder = member.IdentityOrder;
            Required = member.Required;
            DisplayName = member.DisplayName;
        }
        public MemberRubric(MethodRubric method) : this((IMemberRubric)method)
        {
        }
        public MemberRubric(FieldRubric field) : this((IMemberRubric)field)
        {
        }
        public MemberRubric(PropertyRubric property) : this((IMemberRubric)property)
        {
        }
        public MemberRubric(MethodInfo method) : this((IMemberRubric)new MethodRubric(method))
        {
        }
        public MemberRubric(PropertyInfo property) : this((IMemberRubric)new PropertyRubric(property))
        {
        }
        public MemberRubric(FieldInfo field) : this((IMemberRubric)new FieldRubric(field))
        {
        }

        public MemberRubrics Rubrics { get; set; }

        public Type FigureType { get; set; }
        public FieldInfo FigureField { get; set; }
        public int FigureFieldId { get; set; }
        public MemberInfo RubricInfo { get; set; }
        public IMemberRubric VirtualInfo => (IMemberRubric)RubricInfo;

        public Type RubricReturnType { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricReturnType : null; }
        public ParameterInfo[] RubricParameterInfo { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricParameterInfo : null; }
        public Module RubricModule { get => MemberType == MemberTypes.Method ? ((MethodRubric)RubricInfo).RubricModule : null; }
        public string RubricName { get; set; }
        public string DisplayName { get; set; }
        public Type RubricType { get { return VirtualInfo.RubricType; } set { VirtualInfo.RubricType = value; } }

        public int RubricId { get; set; }
        public int RubricSize { get { return VirtualInfo.RubricSize; } set { VirtualInfo.RubricSize = value; } }
        public int RubricOffset { get; set; }
        public short IdentityOrder { get; set; }

        public bool Visible { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public bool IsKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsAutoincrement { get; set; }
        public bool IsDBNull { get; set; }
        public bool IsColossus { get; set; }

        public IRubric          AggregatePattern { get; set; }
        public AggregateOperand AggregateOperand { get; set; }
        public int[]            AggregateIndex   { get; set; }
        public int[]            AggregateOrdinal { get; set; }

        public FigureLinks     AggregateLinks { get; set; }

        public int              SummaryOrdinal { get; set; }
        public IRubric          SummaryPattern { get; set; }
        public AggregateOperand SummaryOperand { get; set; }        

        public object[] RubricAttributes
        { get { return VirtualInfo.RubricAttributes; } set { VirtualInfo.RubricAttributes = value; } }

        public override Type DeclaringType => FigureType != null ? FigureType : RubricInfo.DeclaringType;
        public override MemberTypes MemberType => RubricInfo.MemberType;
        public override string Name => RubricInfo.Name;
        public override Type ReflectedType => RubricInfo.ReflectedType;

        public IUnique Empty => Ussn.Empty;

        public long KeyBlock { get => systemSerialCode.KeyBlock; set => systemSerialCode.KeyBlock = value; }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return RubricInfo.GetCustomAttributes(inherit);
        }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return RubricInfo.GetCustomAttributes(attributeType, inherit);
        }
        public override bool     IsDefined(Type attributeType, bool inherit)
        {
            return RubricInfo.IsDefined(attributeType, inherit);
        }

        public byte[] GetBytes()
        {
            return systemSerialCode.GetBytes();
        }
        public byte[] GetKeyBytes()
        {
            return systemSerialCode.GetKeyBytes();
        }
        public void   SetHashKey(long value)
        {
            systemSerialCode.KeyBlock = value;
        }
        public long   GetHashKey()
        {
            return systemSerialCode.KeyBlock;
        }

        public bool Equals(IUnique other)
        {
           return KeyBlock == other.KeyBlock;
        }
        public int CompareTo(IUnique other)
        {
            return (int)(KeyBlock - other.KeyBlock);
        }

        public void SetHashSeed(uint seed)
        {
            systemSerialCode.SetHashSeed(seed);
        }

        public uint GetHashSeed()
        {
            return systemSerialCode.GetHashSeed();
        }

        private Ussn systemSerialCode;
        public Ussn SystemSerialCode { get => systemSerialCode; set => systemSerialCode = value; }
        public uint SeedBlock { get => systemSerialCode.SeedBlock; set => systemSerialCode.SeedBlock = value; }
    }
}
