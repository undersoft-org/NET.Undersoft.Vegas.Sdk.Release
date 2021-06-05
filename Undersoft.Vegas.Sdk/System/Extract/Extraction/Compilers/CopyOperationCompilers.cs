/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.CopyOperationCompilers.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Defines the <see cref="Extraction" />.
    /// </summary>
    public static partial class Extraction
    {
        #region Methods

        /// <summary>
        /// The CompileCopyByteArrayBlockUInt32.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public static void CompileCopyByteArrayBlockUInt32(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte[]), typeof(uint), typeof(byte[]), typeof(uint), typeof(uint) });
            var code = copyMethod.GetILGenerator();

            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);

            //updated by Darek
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_0);
            code.Emit(OpCodes.Ldloc_0);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_1);
            code.Emit(OpCodes.Ldloc_1);
            code.Emit(OpCodes.Ldarg, 5);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte[]), typeof(uint), typeof(byte[]), typeof(uint), typeof(uint) }));
        }

        /// <summary>
        /// The CompileCopyByteArrayBlockUInt64.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public static void CompileCopyByteArrayBlockUInt64(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte[]), typeof(ulong), typeof(byte[]), typeof(ulong), typeof(ulong) });
            var code = copyMethod.GetILGenerator();

            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);
            code.DeclareLocal(typeof(byte[]).MakePointerType(), pinned: true);

            //updated by Darek
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_0);
            code.Emit(OpCodes.Ldloc_0);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Ldelema, typeof(byte));
            code.Emit(OpCodes.Stloc_1);
            code.Emit(OpCodes.Ldloc_1);
            code.Emit(OpCodes.Ldarg, 5);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte[]), typeof(ulong), typeof(byte[]), typeof(ulong), typeof(ulong) }));
        }

        /// <summary>
        /// The CompileCopyPointerBlockUInt32.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public static void CompileCopyPointerBlockUInt32(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte*), typeof(uint), typeof(byte*), typeof(uint), typeof(uint) });
            var code = copyMethod.GetILGenerator();

            //updated by Darek
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldarg, 5);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte*), typeof(uint), typeof(byte*), typeof(uint), typeof(uint) }));
        }

        /// <summary>
        /// The CompileCopyPointerBlockUInt64.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public static void CompileCopyPointerBlockUInt64(TypeBuilder tb)
        {
            var copyMethod = tb.DefineMethod("CopyBlock",
                                             MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                             typeof(void),
                                             new Type[] { typeof(byte*), typeof(ulong), typeof(byte*), typeof(ulong), typeof(ulong) });
            var code = copyMethod.GetILGenerator();

            //updated by Darek
            code.Emit(OpCodes.Ldarg_2);
            code.Emit(OpCodes.Ldarg_1);
            code.Emit(OpCodes.Ldarg_3);
            code.Emit(OpCodes.Add);
            code.Emit(OpCodes.Ldarg, 4);
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Cpblk);
            code.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(copyMethod, typeof(IExtractor).GetMethod("CopyBlock", new Type[] { typeof(byte*), typeof(ulong), typeof(byte*), typeof(ulong), typeof(ulong) }));
        }

        #endregion
    }
}
