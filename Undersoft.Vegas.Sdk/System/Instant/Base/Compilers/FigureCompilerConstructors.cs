using System.Uniques;
using System.Instant.Treatments;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace System.Instant
{
    public class FigureCompilerConstructors
    {
        protected readonly PropertyInfo[] dataMemberProps = new[] { typeof(DataMemberAttribute).GetProperty("Order"),
                                                                        typeof(DataMemberAttribute).GetProperty("Name") };
        protected readonly FieldInfo[] structLayoutFields = new[] { typeof(StructLayoutAttribute).GetField("CharSet"),
                                                                        typeof(StructLayoutAttribute).GetField("Pack") };
        protected readonly ConstructorInfo dataMemberCtor = typeof(DataMemberAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly ConstructorInfo structLayoutCtor = typeof(StructLayoutAttribute).GetConstructor(new Type[] { typeof(LayoutKind) });
        protected readonly ConstructorInfo marshalAsCtor = typeof(MarshalAsAttribute).GetConstructor(new Type[] { typeof(UnmanagedType) });
        protected readonly ConstructorInfo figureKeyCtor = typeof(FigureKeyAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly ConstructorInfo figureLinkCtor = typeof(FigureLinkAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly ConstructorInfo figureIdentityCtor = typeof(FigureIdentityAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly ConstructorInfo figureRequiredCtor = typeof(FigureRequiredAttribute).GetConstructor(Type.EmptyTypes);
        protected readonly ConstructorInfo figureDisplayCtor = typeof(FigureDisplayAttribute).GetConstructor(new Type[] { typeof(string) });
        protected readonly ConstructorInfo figuresTreatmentCtor = typeof(FigureTreatmentAttribute).GetConstructor(Type.EmptyTypes);
    }
      
}