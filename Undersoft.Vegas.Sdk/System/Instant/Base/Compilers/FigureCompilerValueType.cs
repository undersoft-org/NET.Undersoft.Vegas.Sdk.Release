/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureCompilerValueType.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Extract;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="FigureCompilerValueType" />.
    /// </summary>
    public class FigureCompilerValueType : FigureCompiler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCompilerValueType"/> class.
        /// </summary>
        /// <param name="instantFigure">The instantFigure<see cref="Figure"/>.</param>
        /// <param name="fieldRubrics">The fieldRubrics<see cref="MemberRubrics"/>.</param>
        /// <param name="propertyRubrics">The propertyRubrics<see cref="MemberRubrics"/>.</param>
        public FigureCompilerValueType(Figure instantFigure, MemberRubrics fieldRubrics, MemberRubrics propertyRubrics) :
            base(instantFigure, fieldRubrics, propertyRubrics)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CompileFigureType.
        /// </summary>
        /// <param name="typeName">The typeName<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public override Type CompileFigureType(string typeName)
        {
            fields = new FieldBuilder[length + scode];
            props = new PropertyBuilder[length + scode];

            TypeBuilder tb = GetTypeBuilder(typeName);

            CreateSerialCodeProperty(tb, typeof(Ussn), "SerialCode");

            CreateFieldsAndProperties(tb);

            CreateValueArrayProperty(tb);

            CreateItemByIntProperty(tb);

            CreateItemByStringProperty(tb);

            CreateUniqueKeyProperty(tb);

            CreateUniqueSeedProperty(tb);

            CreateGetUniqueBytesMethod(tb);

            CreateGetBytesMethod(tb);

            CreateGetEmptyProperty(tb);

            CreateEqualsMethod(tb);

            CreateCompareToMethod(tb);

            return tb.CreateTypeInfo();
        }

        /// <summary>
        /// The CreateFieldsAndProperties.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <returns>The <see cref="FieldBuilder[]"/>.</returns>
        public override FieldBuilder[] CreateFieldsAndProperties(TypeBuilder tb)
        {
            for (int i = scode; i < length + scode; i++)
            {
                MemberRubric mr = fieldRubrics[i - scode];
                Type type = null;
                string name = mr.RubricName;
                string fieldName = "_" + mr.RubricName;
                bool isBackingField = false;
                if (mr.MemberType == MemberTypes.Field)
                {
                    var fieldRubric = (FieldRubric)mr.RubricInfo;
                    isBackingField = fieldRubric.IsBackingField;
                    fieldRubric.FieldName = fieldName;
                    type = mr.RubricType;

                    if (type == null)
                        type = fieldRubric.FieldType;
                }
                else if (mr.MemberType == MemberTypes.Property)
                {
                    type = mr.RubricType;
                    if (type == null)
                        type = ((PropertyRubric)mr.RubricInfo).PropertyType;
                }

                if ((type.IsArray && !type.GetElementType().IsValueType) ||
                    (!type.IsArray && !type.IsValueType && type != typeof(string)))
                {
                    type = null;
                }

                if (type != null)
                {
                    var _mr = mr;
                    if (isBackingField)
                    {
                        var __mr = propertyRubrics[mr.RubricName];
                        if (__mr != null)
                            _mr = __mr;
                    }

                    FieldBuilder fb = createField(tb, _mr, type, fieldName);

                    if (fb != null)
                    {

                        ResolveFigureAttributes(fb, _mr, mr);

                        PropertyBuilder pb = (type != typeof(string)) ?
                                              createProperty(tb, fb, type, name) :
                                              createStringProperty(tb, fb, type, name);
                        fields[i] = fb;
                        props[i] = pb;
                        pb.SetCustomAttribute(new CustomAttributeBuilder(dataMemberCtor, new object[0],
                                                                         dataMemberProps, new object[2] { i - scode, name }));
                    }
                }
            }

            return fields;
        }

        /// <summary>
        /// The CreateGetBytesMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateGetBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Box, tb.UnderlyingSystemType); // box
            il.EmitCall(OpCodes.Call, typeof(ObjectExtractExtenstion).GetMethod("GetValueStructureBytes", new Type[] { typeof(object) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateItemByIntProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateItemByIntProperty(TypeBuilder tb)
        {
            foreach (PropertyInfo prop in typeof(IFigure).GetProperties())
            {
                MethodInfo accessor = prop.GetGetMethod();
                if (accessor != null)
                {
                    ParameterInfo[] args = accessor.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 1 && argTypes[0] == typeof(int))
                    {
                        MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                               accessor.CallingConvention, accessor.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, accessor);
                        ILGenerator il = method.GetILGenerator();

                        Label[] branches = new Label[length];
                        for (int i = scode; i < length + scode; i++)
                        {
                            if (fields[i].FieldType != null)
                                branches[i - scode] = il.DefineLabel();
                        }
                        il.Emit(OpCodes.Ldarg_1); // key

                        il.Emit(OpCodes.Switch, branches); // switch
                                                           // default:
                        il.ThrowException(typeof(ArgumentOutOfRangeException));
                        for (int i = scode; i < length + scode; i++)
                        {
                            if (fields[i].FieldType != null)
                            {
                                il.MarkLabel(branches[i - scode]);
                                il.Emit(OpCodes.Ldarg_0); // this
                                il.Emit(OpCodes.Ldfld, fields[i]); // foo load
                                if (fields[i].FieldType.IsValueType)
                                {
                                    il.Emit(OpCodes.Box, fields[i].FieldType); // box
                                }
                                else if (fields[i].FieldType == typeof(char[]))
                                {
                                    il.Emit(OpCodes.Newobj, typeof(string).GetConstructor(new Type[] { typeof(char[]) })); // load
                                }
                                il.Emit(OpCodes.Ret); // end
                            }
                        }
                    }
                }


                MethodInfo mutator = prop.GetSetMethod();
                if (mutator != null)
                {
                    ParameterInfo[] args = mutator.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 2 && argTypes[0] == typeof(int) && argTypes[1] == typeof(object))
                    {
                        MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                           mutator.CallingConvention, mutator.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, mutator);
                        ILGenerator il = method.GetILGenerator();

                        Label[] branches = new Label[length];
                        for (int i = scode; i < length + scode; i++)
                        {
                            if (fields[i].FieldType != null)
                                branches[i - scode] = il.DefineLabel();
                        }
                        il.Emit(OpCodes.Ldarg_1); // key

                        il.Emit(OpCodes.Switch, branches); // switch
                                                           // default:
                        il.ThrowException(typeof(ArgumentOutOfRangeException));
                        for (int i = scode; i < length + scode; i++)
                        {
                            if (fields[i].FieldType != null)
                            {
                                il.MarkLabel(branches[i - scode]);
                                il.Emit(OpCodes.Ldarg_0); // this
                                il.Emit(OpCodes.Ldarg_2); // value
                                if (fields[i].FieldType.IsValueType)
                                {
                                    il.Emit(OpCodes.Unbox_Any, fields[i].FieldType);
                                }
                                else if (fields[i].FieldType == typeof(char[]))
                                {
                                    il.Emit(OpCodes.Castclass, typeof(string));
                                    il.EmitCall(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes), null);
                                }
                                else
                                    il.Emit(OpCodes.Castclass, fields[i].FieldType);
                                il.Emit(OpCodes.Stfld, fields[i]);
                                il.Emit(OpCodes.Ret); // end
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// The CreateItemByStringProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateItemByStringProperty(TypeBuilder tb)
        {
            foreach (PropertyInfo prop in typeof(IFigure).GetProperties())
            {
                MethodInfo accessor = prop.GetGetMethod();
                if (accessor != null)
                {
                    ParameterInfo[] args = accessor.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 1 && argTypes[0] == typeof(string))
                    {
                        MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                               accessor.CallingConvention, accessor.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, accessor);
                        ILGenerator il = method.GetILGenerator();

                        il.DeclareLocal(typeof(string));

                        Label[] branches = new Label[length + scode];

                        for (int i = 0; i < length + scode; i++)
                        {
                            branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); // key
                        il.Emit(OpCodes.Stloc_0);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (props[i].Name != null)
                            {
                                il.Emit(OpCodes.Ldloc_0);
                                il.Emit(OpCodes.Ldstr, props[i].Name);
                                il.EmitCall(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }), null);
                                il.Emit(OpCodes.Brtrue, branches[i]);
                            }
                        }

                        il.Emit(OpCodes.Ldnull); // this
                        il.Emit(OpCodes.Ret);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (props[i].Name != null)
                            {
                                il.MarkLabel(branches[i]);
                                il.Emit(OpCodes.Ldarg_0); // this
                                il.Emit(OpCodes.Ldfld, fields[i]); // foo load
                                if (fields[i].FieldType.IsValueType)
                                {
                                    il.Emit(OpCodes.Box, fields[i].FieldType); // box
                                }
                                else if (fields[i].FieldType == typeof(char[]))
                                {
                                    il.Emit(OpCodes.Newobj, typeof(string).GetConstructor(new Type[] { typeof(char[]) })); // load
                                }
                                il.Emit(OpCodes.Ret); // end
                            }
                        }
                    }
                }


                MethodInfo mutator = prop.GetSetMethod();
                if (mutator != null)
                {
                    ParameterInfo[] args = mutator.GetParameters();
                    Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

                    if (args.Length == 2 && argTypes[0] == typeof(string) && argTypes[1] == typeof(object))
                    {
                        MethodBuilder method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                           mutator.CallingConvention, mutator.ReturnType, argTypes);
                        tb.DefineMethodOverride(method, mutator);
                        ILGenerator il = method.GetILGenerator();

                        il.DeclareLocal(typeof(string));

                        Label[] branches = new Label[length + scode];
                        for (int i = 0; i < length + scode; i++)
                        {
                            if (props[i].Name != null)
                                branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); // key
                        il.Emit(OpCodes.Stloc_0);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (props[i].Name != null)
                            {
                                il.Emit(OpCodes.Ldloc_0);
                                il.Emit(OpCodes.Ldstr, props[i].Name);
                                il.EmitCall(OpCodes.Call, typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }), null);
                                il.Emit(OpCodes.Brtrue, branches[i]);
                            }
                        }

                        il.Emit(OpCodes.Ret);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (props[i].Name != null)
                            {
                                il.MarkLabel(branches[i]);
                                il.Emit(OpCodes.Ldarg_0); // this
                                il.Emit(OpCodes.Ldarg_2); // value
                                if (fields[i].FieldType.IsValueType)
                                {
                                    il.Emit(OpCodes.Unbox_Any, fields[i].FieldType);
                                }
                                else if (fields[i].FieldType == typeof(char[]))
                                {
                                    il.Emit(OpCodes.Castclass, typeof(string));
                                    il.EmitCall(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes), null);
                                }
                                else
                                    il.Emit(OpCodes.Castclass, fields[i].FieldType);
                                il.Emit(OpCodes.Stfld, fields[i]); // 
                                il.Emit(OpCodes.Ret); // end
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// The CreateSerialCodeProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        public override void CreateSerialCodeProperty(TypeBuilder tb, Type type, string name)
        {
            FieldBuilder fb = createField(tb, null, type, name.ToLower());
            fields[0] = fb;

            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            PropertyInfo iprop = typeof(IFigure).GetProperty(name);

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                   accessor.CallingConvention, accessor.ReturnType, argTypes);

            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
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

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldarg_1); // value
            il.Emit(OpCodes.Stfld, fb); // assign
            il.Emit(OpCodes.Ret);

            prop.SetCustomAttribute(new CustomAttributeBuilder(
                                       dataMemberCtor, new object[0],
                                       dataMemberProps, new object[2] { 0, name.ToUpper() }));

            props[0] = prop;
        }

        /// <summary>
        /// The CreateValueArrayProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateValueArrayProperty(TypeBuilder tb)
        {
            PropertyInfo prop = typeof(IFigure).GetProperty("ValueArray");

            MethodInfo accessor = prop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                   accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(method, accessor);

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(typeof(object[]));

            il.Emit(OpCodes.Ldc_I4, length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc_0);

            for (int i = scode; i < length + scode; i++)
            {
                il.Emit(OpCodes.Ldloc_0); // this
                il.Emit(OpCodes.Ldc_I4, i - scode);
                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldfld, fields[i]); // foo load
                if (fields[i].FieldType.IsValueType)
                {
                    il.Emit(OpCodes.Box, fields[i].FieldType); // box
                }
                else if (fields[i].FieldType == typeof(char[]))
                {
                    il.Emit(OpCodes.Newobj, typeof(string).GetConstructor(new Type[] { typeof(char[]) })); // load
                }
                il.Emit(OpCodes.Stelem, typeof(object)); // this
            }
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret); // return

            MethodInfo mutator = prop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            method = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                               mutator.CallingConvention, mutator.ReturnType, argTypes);
            tb.DefineMethodOverride(method, mutator);
            il = method.GetILGenerator();
            il.DeclareLocal(typeof(object[]));

            il.Emit(OpCodes.Ldarg_1); // value
            il.Emit(OpCodes.Stloc_0);
            for (int i = scode; i < length + scode; i++)
            {
                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldc_I4, i - scode);
                il.Emit(OpCodes.Ldelem, typeof(object));
                if (fields[i].FieldType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, fields[i].FieldType);
                }
                else if (fields[i].FieldType == typeof(char[]))
                {
                    il.Emit(OpCodes.Castclass, typeof(string));
                    il.EmitCall(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes), null);
                }
                else
                    il.Emit(OpCodes.Castclass, fields[i].FieldType);
                il.Emit(OpCodes.Stfld, fields[i]); // 
            }
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The GetTypeBuilder.
        /// </summary>
        /// <param name="typeName">The typeName<see cref="string"/>.</param>
        /// <returns>The <see cref="TypeBuilder"/>.</returns>
        public override TypeBuilder GetTypeBuilder(string typeName)
        {
            string typeSignature = (typeName != null && typeName != "") ? typeName : Unique.NewKey.ToString();
            AssemblyName an = new AssemblyName(typeSignature);

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(typeSignature + "Module");

            TypeBuilder tb;

            tb = moduleBuilder.DefineType(typeSignature, TypeAttributes.Public | TypeAttributes.Serializable |
                                                         TypeAttributes.BeforeFieldInit | TypeAttributes.Class |
                                                         TypeAttributes.AnsiClass | TypeAttributes.SequentialLayout, typeof(ValueType));

            //tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Any, Type.EmptyTypes);

            tb.SetCustomAttribute(new CustomAttributeBuilder(structLayoutCtor, new object[] { LayoutKind.Sequential },
                                                             structLayoutFields, new object[] { CharSet.Ansi, 1 }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(typeof(DataContractAttribute)
                                                                .GetConstructor(Type.EmptyTypes), new object[0]));

            tb.AddInterfaceImplementation(typeof(IFigure));

            return tb;
        }

        /// <summary>
        /// The createField.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="fieldName">The fieldName<see cref="string"/>.</param>
        /// <returns>The <see cref="FieldBuilder"/>.</returns>
        private FieldBuilder createField(TypeBuilder tb, MemberRubric mr, Type type, string fieldName)
        {
            if (type == typeof(string) || type.IsArray)
            {
                Type _type = type;

                FieldBuilder fb;

                if (type == typeof(string))
                    _type = typeof(char[]);

                fb = tb.DefineField(fieldName, _type, FieldAttributes.Private | FieldAttributes.HasDefault | FieldAttributes.HasFieldMarshal);

                ResolveMarshalAsAttributeForArray(fb, mr, _type);

                return fb;
            }
            else
            {
                return tb.DefineField(fieldName, type, FieldAttributes.Private);
            }
        }

        /// <summary>
        /// The createProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="PropertyBuilder"/>.</returns>
        private PropertyBuilder createProperty(TypeBuilder tb, FieldBuilder field, Type type, string name)
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
        /// The createStringProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="PropertyBuilder"/>.</returns>
        private PropertyBuilder createStringProperty(TypeBuilder tb, FieldBuilder field, Type type, string name)
        {
            PropertyBuilder prop = tb.DefineProperty(name, PropertyAttributes.HasDefault,
                                                     type, new Type[] { type });

            MethodBuilder getter = tb.DefineMethod("get_" + name, MethodAttributes.Public |
                                                            MethodAttributes.HideBySig, type,
                                                            Type.EmptyTypes);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            if (type == typeof(string))
            {
                il.Emit(OpCodes.Ldfld, field); // load
                il.Emit(OpCodes.Newobj, typeof(string).GetConstructor(new Type[] { typeof(char[]) })); // load
            }
            else
                il.Emit(OpCodes.Ldfld, field); // load
            il.Emit(OpCodes.Ret); // return

            MethodBuilder setter = tb.DefineMethod("set_" + name, MethodAttributes.Public |
                                                            MethodAttributes.HideBySig, typeof(void),
                                                            new Type[] { type });

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            if (type == typeof(string))
            {
                il.Emit(OpCodes.Ldarg_1); // value
                il.EmitCall(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes), null);
            }
            il.Emit(OpCodes.Stfld, field); // assign
            il.Emit(OpCodes.Ret);

            return prop;
        }

        #endregion
    }
}
