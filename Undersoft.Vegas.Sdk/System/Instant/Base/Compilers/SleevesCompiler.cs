/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.SleevesCompiler.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="SleevesCompiler" />.
    /// </summary>
    public class SleevesCompiler
    {
        #region Fields

        private readonly ConstructorInfo
            marshalAsCtor = typeof(MarshalAsAttribute)
            .GetConstructor(new Type[] { typeof(UnmanagedType) });
        private FieldBuilder multemicField = null;
        private FieldBuilder selectiveField = null;
        private Sleeves sleeve;
        private Type SleeveType = typeof(FigureSleeves);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SleevesCompiler"/> class.
        /// </summary>
        /// <param name="instantSleeve">The instantSleeve<see cref="Sleeves"/>.</param>
        public SleevesCompiler(Sleeves instantSleeve)
        {
            sleeve = instantSleeve;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CompileFigureType.
        /// </summary>
        /// <param name="typeName">The typeName<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public Type CompileFigureType(string typeName)
        {
            TypeBuilder tb = GetTypeBuilder(typeName);

            CreateSleeveField(tb);

            CreateFiguresField(tb);

            CreateElementByIntProperty(tb);

            CreateItemByIntProperty(tb);

            CreateItemByStringProperty(tb);

            // CreateArrayLengthField(tb); 

            return tb.CreateTypeInfo();
        }

        /// <summary>
        /// The CreateArrayLengthField.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateArrayLengthField(TypeBuilder tb)
        {
            PropertyInfo iprop = SleeveType.GetProperty("Length");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            //prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldfld, selectiveField); // load
            il.Emit(OpCodes.Ldlen); // load
            il.Emit(OpCodes.Ret); // return
        }

        /// <summary>
        /// The CreateElementByIntProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateElementByIntProperty(TypeBuilder tb)
        {

            PropertyInfo prop = typeof(FigureSleeves).GetProperty("Item", new Type[] { typeof(int) });
            MethodInfo accessor = prop.GetGetMethod();
            if (accessor != null)
            {
                ParameterInfo[] args = accessor.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                  accessor.CallingConvention, accessor.ReturnType, argTypes);
                tb.DefineMethodOverride(method, accessor);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, selectiveField); // foo load   
                il.Emit(OpCodes.Ldarg_1); // rowid
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int) }), null);
                il.Emit(OpCodes.Ret); // end
            }

            MethodInfo mutator = prop.GetSetMethod();
            if (mutator != null)
            {
                ParameterInfo[] args = mutator.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                   mutator.CallingConvention, mutator.ReturnType, argTypes);
                tb.DefineMethodOverride(method, mutator);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, selectiveField); // foo load   
                il.Emit(OpCodes.Ldarg_1); // rowid
                il.Emit(OpCodes.Ldarg_2); // value
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("set_Item", new Type[] { typeof(int), typeof(IFigure) }), null);
                il.Emit(OpCodes.Ret); // end
            }
        }

        /// <summary>
        /// The CreateField.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="FieldBuilder"/>.</returns>
        private FieldBuilder CreateField(TypeBuilder tb, Type type, string name)
        {
            return tb.DefineField("_" + name, type, FieldAttributes.Private);
        }

        /// <summary>
        /// The CreateFiguresField.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateFiguresField(TypeBuilder tb)
        {
            FieldBuilder fb = CreateField(tb, typeof(object).MakeArrayType(), "Figures");
            multemicField = fb;

            PropertyInfo iprop = SleeveType.GetProperty("Figures");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            //getter.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldfld, fb); // load
            il.Emit(OpCodes.Ret); // return

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            //prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldarg_1); // value
            il.Emit(OpCodes.Stfld, fb); // assign
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateItemByIntProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateItemByIntProperty(TypeBuilder tb)
        {
            PropertyInfo prop = typeof(FigureSleeves).GetProperty("Item", new Type[] { typeof(int), typeof(int) });
            MethodInfo accessor = prop.GetGetMethod();
            if (accessor != null)
            {
                ParameterInfo[] args = accessor.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                  accessor.CallingConvention, accessor.ReturnType, argTypes);
                tb.DefineMethodOverride(method, accessor);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, selectiveField); // foo load   
                il.Emit(OpCodes.Ldarg_1); // rowid
                il.Emit(OpCodes.Ldarg_2); // fieldid
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int), typeof(int) }), null);
                il.Emit(OpCodes.Ret); // end
            }

            MethodInfo mutator = prop.GetSetMethod();
            if (mutator != null)
            {
                ParameterInfo[] args = mutator.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                   mutator.CallingConvention, mutator.ReturnType, argTypes);
                tb.DefineMethodOverride(method, mutator);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, selectiveField); // foo load   
                il.Emit(OpCodes.Ldarg_1); // rowid
                il.Emit(OpCodes.Ldarg_2); // fieldid
                il.Emit(OpCodes.Ldarg_3); // value
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("set_Item", new Type[] { typeof(int), typeof(int), typeof(object) }), null);
                il.Emit(OpCodes.Ret); // end
            }
        }

        /// <summary>
        /// The CreateItemByStringProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateItemByStringProperty(TypeBuilder tb)
        {
            PropertyInfo prop = typeof(FigureSleeves).GetProperty("Item", new Type[] { typeof(int), typeof(string) });
            MethodInfo accessor = prop.GetGetMethod();
            if (accessor != null)
            {
                ParameterInfo[] args = accessor.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                   accessor.CallingConvention, accessor.ReturnType, argTypes);
                tb.DefineMethodOverride(method, accessor);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, selectiveField); // foo load   
                il.Emit(OpCodes.Ldarg_1); // rowid
                il.Emit(OpCodes.Ldarg_2); // fieldid
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("get_Item", new Type[] { typeof(int), typeof(string) }), null);
                il.Emit(OpCodes.Ret); // end
            }

            MethodInfo mutator = prop.GetSetMethod();
            if (mutator != null)
            {
                ParameterInfo[] args = mutator.GetParameters();
                Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);
                MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                   mutator.CallingConvention, mutator.ReturnType, argTypes);
                tb.DefineMethodOverride(method, mutator);
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, selectiveField); // foo load   
                il.Emit(OpCodes.Ldarg_1); // rowid
                il.Emit(OpCodes.Ldarg_2); // fieldid
                il.Emit(OpCodes.Ldarg_3); // value
                il.EmitCall(OpCodes.Callvirt, typeof(IFigures).GetMethod("set_Item", new Type[] { typeof(int), typeof(string), typeof(object) }), null);
                il.Emit(OpCodes.Ret); // end
            }
        }

        /// <summary>
        /// The CreateMarshalAttribue.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="MarshalAsAttribute"/>.</param>
        private void CreateMarshalAttribue(FieldBuilder field, MarshalAsAttribute attrib)
        {
            List<object> attribValues = new List<object>(1);
            List<FieldInfo> attribFields = new List<FieldInfo>(1);
            attribValues.Add(attrib.SizeConst);
            attribFields.Add(attrib.GetType().GetField("SizeConst"));
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value }, attribFields.ToArray(), attribValues.ToArray()));
        }

        /// <summary>
        /// The CreateNewSleeveObject.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateNewSleeveObject(TypeBuilder tb)
        {
            MethodInfo createArray = SleeveType.GetMethod("NewSleeves");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(typeof(IFigure).MakeArrayType());

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Newarr, typeof(IFigure));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Stfld, selectiveField); //    
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="PropertyBuilder"/>.</returns>
        private PropertyBuilder CreateProperty(TypeBuilder tb, FieldBuilder field, Type type, string name)
        {

            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            MethodBuilder getter = tb.DefineMethod("get_" + name, MethodAttributes.Public |
                                                            MethodAttributes.HideBySig, type,
                                                            Type.EmptyTypes);
            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldfld, field); // load
            il.Emit(OpCodes.Ret); // return

            MethodBuilder setter = tb.DefineMethod("set_" + name, MethodAttributes.Public |
                                                            MethodAttributes.HideBySig, typeof(void),
                                                            new Type[] { type });
            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldarg_1); // value
            il.Emit(OpCodes.Stfld, field); // assign
            il.Emit(OpCodes.Ret);

            return prop;
        }

        /// <summary>
        /// The CreateSleeveField.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        private void CreateSleeveField(TypeBuilder tb)
        {
            FieldBuilder fb = CreateField(tb, typeof(object).MakeArrayType(), "Sleeves");
            selectiveField = fb;

            PropertyInfo iprop = SleeveType.GetProperty("Sleeves");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            //getter.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldfld, fb); // load
            il.Emit(OpCodes.Ret); // return

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(setter, mutator);

            //prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldarg_1); // value
            il.Emit(OpCodes.Stfld, fb); // assign
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The GetTypeBuilder.
        /// </summary>
        /// <param name="typeName">The typeName<see cref="string"/>.</param>
        /// <returns>The <see cref="TypeBuilder"/>.</returns>
        private TypeBuilder GetTypeBuilder(string typeName)
        {
            string typeSignature = typeName;
            AssemblyName an = new AssemblyName(typeSignature);

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(typeSignature + "Module");
            TypeBuilder tb = null;

            tb = moduleBuilder.DefineType(typeSignature, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Serializable |
                                                         TypeAttributes.AnsiClass);

            tb.SetCustomAttribute(new CustomAttributeBuilder(typeof(DataContractAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
            tb.SetParent(typeof(FigureSleeves));
            return tb;
        }

        #endregion
    }
}
