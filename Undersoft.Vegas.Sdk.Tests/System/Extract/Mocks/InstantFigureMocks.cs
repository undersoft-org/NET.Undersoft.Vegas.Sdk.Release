/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extract.InstantFigureMocks.cs.Tests
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    using System.Instant;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="FigureMocks" />.
    /// </summary>
    public static class FigureMocks
    {
        #region Methods

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

        #endregion
    }
}
