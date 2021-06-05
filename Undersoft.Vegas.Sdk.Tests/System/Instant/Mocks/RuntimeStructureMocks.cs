/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Tests.RuntimeStructureMocks.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Tests
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="FigureMocks" />.
    /// </summary>
    public static class FigureMocks
    {
        #region Methods

        /// <summary>
        /// The Figure_Memberinfo_FieldsAndPropertiesModel.
        /// </summary>
        /// <returns>The <see cref="MemberInfo[]"/>.</returns>
        public static MemberInfo[] Figure_Memberinfo_FieldsAndPropertiesModel()
        {
            return typeof(FieldsAndPropertiesModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? m :
                                                             m.MemberType == MemberTypes.Property ? m :
                                                             null).Where(p => p != null).ToArray();
        }

        /// <summary>
        /// The Figure_Memberinfo_FieldsOnlyModel.
        /// </summary>
        /// <returns>The <see cref="MemberInfo[]"/>.</returns>
        public static MemberInfo[] Figure_Memberinfo_FieldsOnlyModel()
        {
            return typeof(FieldsOnlyModel).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                            .Where(p => p != null).ToArray();
        }

        /// <summary>
        /// The Figure_Memberinfo_PropertiesOnlyModel.
        /// </summary>
        /// <returns>The <see cref="MemberInfo[]"/>.</returns>
        public static MemberInfo[] Figure_Memberinfo_PropertiesOnlyModel()
        {
            return typeof(PropertiesOnlyModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? m :
                                                             m.MemberType == MemberTypes.Property ? m :
                                                             null).Where(p => p != null).ToArray();
        }

        /// <summary>
        /// The Figure_MemberRubric_FieldsAndPropertiesModel.
        /// </summary>
        /// <returns>The <see cref="MemberInfo[]"/>.</returns>
        public static MemberInfo[] Figure_MemberRubric_FieldsAndPropertiesModel()
        {
            return typeof(FieldsAndPropertiesModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                             m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                             null).Where(p => p != null).ToArray();
        }

        /// <summary>
        /// The Figure_MemberRubric_FieldsOnlyModel.
        /// </summary>
        /// <returns>The <see cref="MemberInfo[]"/>.</returns>
        public static MemberInfo[] Figure_MemberRubric_FieldsOnlyModel()
        {
            return typeof(FieldsOnlyModel).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                            .Select(m => new MemberRubric(m)).Where(p => p != null).ToArray();
        }

        /// <summary>
        /// The Figure_MemberRubric_PropertiesOnlyModel.
        /// </summary>
        /// <returns>The <see cref="MemberInfo[]"/>.</returns>
        public static MemberInfo[] Figure_MemberRubric_PropertiesOnlyModel()
        {
            return typeof(PropertiesOnlyModel).GetMembers().Select(m => m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                             m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                             null).Where(p => p != null).ToArray();
        }

        #endregion
    }
}
