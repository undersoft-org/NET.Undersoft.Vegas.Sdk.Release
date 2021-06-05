/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureCompilerDerivedType.cs
   
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
    /// Defines the <see cref="FigureCompilerDerivedType" />.
    /// </summary>
    public class FigureCompilerDerivedType : FigureCompiler
    {
        #region Fields

        protected FieldBuilder scodeField;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCompilerDerivedType"/> class.
        /// </summary>
        /// <param name="instantFigure">The instantFigure<see cref="Figure"/>.</param>
        /// <param name="memberRubrics">The memberRubrics<see cref="MemberRubrics"/>.</param>
        /// <param name="fieldRubrics">The fieldRubrics<see cref="MemberRubrics"/>.</param>
        public FigureCompilerDerivedType(Figure instantFigure, MemberRubrics memberRubrics, MemberRubrics fieldRubrics) : base(instantFigure, memberRubrics, fieldRubrics)
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
            derivedFields = new FieldRubric[length + scode];

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
        /// The CreateCompareToMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateCompareToMethod(TypeBuilder tb)
        {
            MethodInfo mi = typeof(IComparable<IUnique>).GetMethod("CompareTo");

            ParameterInfo[] args = mi.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(mi.Name, mi.Attributes & ~MethodAttributes.Abstract,
                                                          mi.CallingConvention, mi.ReturnType, argTypes);
            tb.DefineMethodOverride(method, mi);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, scodeField);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("CompareTo", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateEqualsMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateEqualsMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IEquatable<IUnique>).GetMethod("Equals");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, scodeField);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("Equals", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
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

                    ResolveFigureAttributes(null, _mr, mr);

                    derivedFields[i] = (FieldRubric)mr.RubricInfo;
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
        /// The CreateGetEmptyProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateGetEmptyProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty("Empty", PropertyAttributes.HasDefault,
                                                     typeof(IUnique), Type.EmptyTypes);

            PropertyInfo iprop = typeof(IUnique).GetProperty("Empty");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, scodeField); // load
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("get_Empty"), null);
            il.Emit(OpCodes.Ret); // return
        }

        /// <summary>
        /// The CreateGetUniqueBytesMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateGetUniqueBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, scodeField);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueBytes"), null);
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
                            if (derivedFields[i].FieldType != null)
                                branches[i - scode] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); // key

                        il.Emit(OpCodes.Switch, branches); // switch
                                                           // default:
                        il.ThrowException(typeof(ArgumentOutOfRangeException));

                        for (int i = scode; i < length + scode; i++)
                        {
                            if (derivedFields[i].FieldType != null)
                            {
                                il.MarkLabel(branches[i - scode]);
                                il.Emit(OpCodes.Ldarg_0); // this

                                if (derivedFields[i].IsBackingField)
                                    il.EmitCall(OpCodes.Call, figure.BaseType.GetMethod("get_" + derivedFields[i].RubricName), null);
                                else
                                    il.Emit(OpCodes.Ldfld, derivedFields[i].RubricInfo); // foo load

                                if (derivedFields[i].FieldType.IsValueType)
                                {
                                    il.Emit(OpCodes.Box, derivedFields[i].FieldType); // box
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
                            if (derivedFields[i].FieldType != null)
                                branches[i - scode] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); // key

                        il.Emit(OpCodes.Switch, branches); // switch
                                                           // default:
                        il.ThrowException(typeof(ArgumentOutOfRangeException));

                        for (int i = scode; i < length + scode; i++)
                        {
                            if (derivedFields[i].FieldType != null)
                            {
                                il.MarkLabel(branches[i - scode]);
                                il.Emit(OpCodes.Ldarg_0); // this
                                il.Emit(OpCodes.Ldarg_2); // value
                                il.Emit(derivedFields[i].FieldType.IsValueType ?
                                                             OpCodes.Unbox_Any :
                                                             OpCodes.Castclass,
                                                             derivedFields[i].FieldType); // type
                                if (derivedFields[i].IsBackingField)
                                {
                                    il.EmitCall(OpCodes.Call, figure.BaseType.GetMethod("set_" + derivedFields[i].RubricName,
                                                                        new Type[] { derivedFields[i].FieldType }), null); // foo load
                                }
                                else
                                    il.Emit(OpCodes.Stfld, derivedFields[i].RubricInfo); // 
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
                            if (derivedFields[i].RubricName != null)
                            {
                                il.Emit(OpCodes.Ldloc_0);
                                il.Emit(OpCodes.Ldstr, derivedFields[i].RubricName);
                                il.EmitCall(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }), null);
                                il.Emit(OpCodes.Brtrue, branches[i]);
                            }
                        }

                        il.Emit(OpCodes.Ldnull); // this
                        il.Emit(OpCodes.Ret);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (derivedFields[i].RubricName != null)
                            {
                                il.MarkLabel(branches[i]);
                                il.Emit(OpCodes.Ldarg_0); // this

                                if (derivedFields[i].IsBackingField)
                                    il.EmitCall(OpCodes.Call, figure.BaseType.GetMethod("get_" + derivedFields[i].RubricName), null);
                                else
                                    il.Emit(OpCodes.Ldfld, derivedFields[i].RubricInfo); // foo load

                                if (derivedFields[i].FieldType.IsValueType)
                                {
                                    il.Emit(OpCodes.Box, derivedFields[i].FieldType); // box
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
                            if (derivedFields[i].RubricName != null)
                                branches[i] = il.DefineLabel();
                        }

                        il.Emit(OpCodes.Ldarg_1); // key
                        il.Emit(OpCodes.Stloc_0);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (derivedFields[i].RubricName != null)
                            {
                                il.Emit(OpCodes.Ldloc_0);
                                il.Emit(OpCodes.Ldstr, derivedFields[i].RubricName);
                                il.EmitCall(OpCodes.Call, typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }), null);
                                il.Emit(OpCodes.Brtrue, branches[i]);
                            }
                        }

                        il.Emit(OpCodes.Ret);

                        for (int i = 0; i < length + scode; i++)
                        {
                            if (derivedFields[i].RubricName != null)
                            {
                                il.MarkLabel(branches[i]);
                                il.Emit(OpCodes.Ldarg_0); // this
                                il.Emit(OpCodes.Ldarg_2); // value
                                il.Emit(derivedFields[i].FieldType.IsValueType ?
                                                             OpCodes.Unbox_Any :
                                                             OpCodes.Castclass,
                                                             derivedFields[i].FieldType); // type
                                if (derivedFields[i].IsBackingField)
                                {
                                    il.EmitCall(OpCodes.Call, figure.BaseType.GetMethod("set_" + derivedFields[i].RubricName,
                                                                        new Type[] { derivedFields[i].FieldType }), null); // foo load
                                }
                                else
                                    il.Emit(OpCodes.Stfld, derivedFields[i].RubricInfo); // 
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
            scodeField = tb.DefineField(name.ToLower(), type, FieldAttributes.Private);

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
            il.Emit(OpCodes.Ldfld, scodeField); // load
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
            il.Emit(OpCodes.Stfld, scodeField); // assign
            il.Emit(OpCodes.Ret);

            prop.SetCustomAttribute(new CustomAttributeBuilder(
                                       dataMemberCtor, new object[0],
                                       dataMemberProps, new object[2] { 0, name.ToUpper() }));

            derivedFields[0] = new FieldRubric(scodeField.FieldType, name);
            derivedFields[0].RubricInfo = scodeField;
        }

        /// <summary>
        /// The CreateUniqueKeyProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateUniqueKeyProperty(TypeBuilder tb)
        {
            PropertyBuilder prop = tb.DefineProperty("UniqueKey", PropertyAttributes.HasDefault,
                                                     typeof(ulong), new Type[] { typeof(ulong) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("UniqueKey");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, scodeField); // load
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueKey").GetGetMethod(), null);
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
            il.Emit(OpCodes.Ldflda, scodeField); // load
            il.Emit(OpCodes.Ldarg_1); // value
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueKey").GetSetMethod(), null);
            il.Emit(OpCodes.Ret); // return
        }

        /// <summary>
        /// The CreateUniqueSeedProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public override void CreateUniqueSeedProperty(TypeBuilder tb)
        {

            PropertyBuilder prop = tb.DefineProperty("UniqueSeed", PropertyAttributes.HasDefault,
                                                     typeof(ulong), new Type[] { typeof(ulong) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("UniqueSeed");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, scodeField); // load
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueSeed").GetGetMethod(), null);
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
            il.Emit(OpCodes.Ldflda, scodeField); // load
            il.Emit(OpCodes.Ldarg_1); // value
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueSeed").GetSetMethod(), null);
            il.Emit(OpCodes.Ret); // return
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

                if (derivedFields[i].IsBackingField)
                    il.EmitCall(OpCodes.Call, figure.BaseType.GetMethod("get_" + derivedFields[i].RubricName), null); // foo load
                else
                    il.Emit(OpCodes.Ldfld, derivedFields[i].RubricInfo); // foo load

                if (derivedFields[i].FieldType.IsValueType)
                {
                    il.Emit(OpCodes.Box, derivedFields[i].FieldType); // box
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
                il.Emit(derivedFields[i].FieldType.IsValueType ?
                                             OpCodes.Unbox_Any :
                                             OpCodes.Castclass,
                                             derivedFields[i].FieldType); // type
                if (derivedFields[i].IsBackingField)
                {
                    il.EmitCall(OpCodes.Call, figure.BaseType.GetMethod("set_" + derivedFields[i].RubricName,
                                                        new Type[] { derivedFields[i].FieldType }), null); // foo load
                }
                else
                    il.Emit(OpCodes.Stfld, derivedFields[i].RubricInfo); // 
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
            TypeBuilder tb = null;

            tb = moduleBuilder.DefineType(typeSignature, TypeAttributes.Class | TypeAttributes.Public |
                                                         TypeAttributes.Serializable | TypeAttributes.AnsiClass |
                                                         TypeAttributes.SequentialLayout);

            tb.SetCustomAttribute(new CustomAttributeBuilder(structLayoutCtor, new object[] { LayoutKind.Sequential },
                                                             structLayoutFields, new object[] { CharSet.Ansi, 1 }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(typeof(DataContractAttribute)
                                                                .GetConstructor(Type.EmptyTypes), new object[0]));

            tb.AddInterfaceImplementation(typeof(IFigure));

            tb.SetParent(figure.BaseType);

            return tb;
        }

        #endregion
    }
}
