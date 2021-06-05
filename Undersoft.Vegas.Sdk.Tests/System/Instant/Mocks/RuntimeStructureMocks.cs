using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using Xunit;
using System.Globalization;

namespace System.Instant.Tests
{

    public static class FigureMocks
    {
        public static MemberInfo[] Figure_Memberinfo_FieldsOnlyModel()
        {
            return typeof(FieldsOnlyModel).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                            .Where(p => p != null).ToArray();
        }


        public static MemberInfo[] Figure_MemberRubric_FieldsOnlyModel()
        {
            return typeof(FieldsOnlyModel).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                            .Select(m => new MemberRubric(m)).Where(p => p != null).ToArray();
        }

        public static MemberInfo[] Figure_Memberinfo_PropertiesOnlyModel()
        {
            return typeof(PropertiesOnlyModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? m :
                                                             m.MemberType == MemberTypes.Property ? m :
                                                             null).Where(p => p != null).ToArray();
        }


        public static MemberInfo[] Figure_MemberRubric_PropertiesOnlyModel()
        {
            return typeof(PropertiesOnlyModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                             m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                             null).Where(p => p != null).ToArray();
        }

        public static MemberInfo[] Figure_Memberinfo_FieldsAndPropertiesModel()
        {
            return typeof(FieldsAndPropertiesModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? m :
                                                             m.MemberType == MemberTypes.Property ? m :
                                                             null).Where(p => p != null).ToArray();
        }


        public static MemberInfo[] Figure_MemberRubric_FieldsAndPropertiesModel()
        {
            return typeof(FieldsAndPropertiesModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                             m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                             null).Where(p => p != null).ToArray();
        }

    }
}
