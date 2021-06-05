/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureCompiler.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Collections.Generic;
    using System.Instant.Treatments;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Uniques;

    #region Enums

    public enum FigureMode
    {
        Derived,
        ValueType,
        Reference
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="FigureCompiler" />.
    /// </summary>
    public abstract class FigureCompiler : FigureCompilerConstructors
    {
        #region Fields

        public FieldRubric[] derivedFields;
        public SortedList<short, MemberRubric> Identities
            = new SortedList<short, MemberRubric>();
        protected MemberRubrics fieldRubrics;
        protected FieldBuilder[] fields = null;
        protected Figure figure;
        protected int length;
        protected FigureMode mode;
        protected MemberRubrics propertyRubrics;
        protected PropertyBuilder[] props = null;
        protected int scode = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCompiler"/> class.
        /// </summary>
        /// <param name="instantFigure">The instantFigure<see cref="Figure"/>.</param>
        /// <param name="fieldRubrics">The fieldRubrics<see cref="MemberRubrics"/>.</param>
        /// <param name="propertyRubrics">The propertyRubrics<see cref="MemberRubrics"/>.</param>
        public FigureCompiler(Figure instantFigure, MemberRubrics fieldRubrics, MemberRubrics propertyRubrics)
        {
            this.fieldRubrics = fieldRubrics;
            this.propertyRubrics = propertyRubrics;
            figure = instantFigure;
            length = fieldRubrics.Count;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether IsDerived.
        /// </summary>
        protected bool IsDerived => figure.IsDerived;

        #endregion

        #region Methods

        /// <summary>
        /// The CompileFigureType.
        /// </summary>
        /// <param name="typeName">The typeName<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public abstract Type CompileFigureType(string typeName);

        /// <summary>
        /// The CreateCompareToMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateCompareToMethod(TypeBuilder tb)
        {
            MethodInfo mi = typeof(IComparable<IUnique>).GetMethod("CompareTo");

            ParameterInfo[] args = mi.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(mi.Name, mi.Attributes & ~MethodAttributes.Abstract,
                                                          mi.CallingConvention, mi.ReturnType, argTypes);
            tb.DefineMethodOverride(method, mi);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("CompareTo", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateEqualsMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateEqualsMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IEquatable<IUnique>).GetMethod("Equals");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("Equals", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateFieldsAndProperties.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <returns>The <see cref="FieldBuilder[]"/>.</returns>
        public abstract FieldBuilder[] CreateFieldsAndProperties(TypeBuilder tb);

        /// <summary>
        /// The CreateFigureAsAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="FigureAsAttribute"/>.</param>
        public void CreateFigureAsAttribute(FieldBuilder field, FigureAsAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value },
                                                                               new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                                                                               new object[] { attrib.SizeConst }));
        }

        /// <summary>
        /// The CreateFigureDisplayAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="FigureDisplayAttribute"/>.</param>
        public void CreateFigureDisplayAttribute(FieldBuilder field, FigureDisplayAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureDisplayCtor, new object[] { attrib.Name }));
        }

        /// <summary>
        /// The CreateFigureIdentityAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="FigureIdentityAttribute"/>.</param>
        public void CreateFigureIdentityAttribute(FieldBuilder field, FigureIdentityAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureIdentityCtor, Type.EmptyTypes,
                                                                                    new FieldInfo[] { typeof(FigureIdentityAttribute).GetField("Order"),
                                                                                                      typeof(FigureIdentityAttribute).GetField("IsAutoincrement") },
                                                                                    new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        /// <summary>
        /// The CreateFigureKeyAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="FigureKeyAttribute"/>.</param>
        public void CreateFigureKeyAttribute(FieldBuilder field, FigureKeyAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureKeyCtor, Type.EmptyTypes,
                                                                               new FieldInfo[] { typeof(FigureKeyAttribute).GetField("Order"),
                                                                                                 typeof(FigureKeyAttribute).GetField("IsAutoincrement") },
                                                                               new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        /// <summary>
        /// The CreateFigureRequiredAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        public void CreateFigureRequiredAttribute(FieldBuilder field)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureRequiredCtor, Type.EmptyTypes));
        }

        /// <summary>
        /// The CreateFigureTreatmentAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="FigureTreatmentAttribute"/>.</param>
        public void CreateFigureTreatmentAttribute(FieldBuilder field, FigureTreatmentAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figuresTreatmentCtor, Type.EmptyTypes,
                                                                                        new FieldInfo[] { typeof(FigureTreatmentAttribute).GetField("AggregateOperand"),
                                                                                                          typeof(FigureTreatmentAttribute).GetField("SummaryOperand") },
                                                                                        new object[] { attrib.AggregateOperand, attrib.SummaryOperand }));
        }

        /// <summary>
        /// The CreateGetBytesMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public abstract void CreateGetBytesMethod(TypeBuilder tb);

        /// <summary>
        /// The CreateGetEmptyProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateGetEmptyProperty(TypeBuilder tb)
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
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("get_Empty"), null);
            il.Emit(OpCodes.Ret); // return
        }

        /// <summary>
        /// The CreateGetGenericByIntMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateGetGenericByIntMethod(TypeBuilder tb)
        {
            string[] typeParameterNames = { "V" };
            GenericTypeParameterBuilder[] typeParameters =
                tb.DefineGenericParameters(typeParameterNames);

            GenericTypeParameterBuilder V = typeParameters[0];

            MethodInfo mi = typeof(IFigure).GetMethod("Get", new Type[] { typeof(int) }).MakeGenericMethod(typeParameters);

            ParameterInfo[] args = mi.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(mi.Name, mi.Attributes & ~MethodAttributes.Abstract,
                                                          mi.CallingConvention, mi.ReturnType, argTypes);
            tb.DefineMethodOverride(method, mi);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("CompareTo", new Type[] { typeof(IUnique) }), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateGetUniqueBytesMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateGetUniqueBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueBytes"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateGetUniqueKeyMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateGetUniqueKeyMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueKey");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueKey"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateGetUniqueSeedMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateGetUniqueSeedMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetUniqueSeed");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetUniqueSeed"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateItemByIntProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public abstract void CreateItemByIntProperty(TypeBuilder tb);

        /// <summary>
        /// The CreateItemByStringProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public abstract void CreateItemByStringProperty(TypeBuilder tb);

        /// <summary>
        /// The CreateMarshaAslAttribute.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="attrib">The attrib<see cref="MarshalAsAttribute"/>.</param>
        public void CreateMarshaAslAttribute(FieldBuilder field, MarshalAsAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value },
                                                                               new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                                                                               new object[] { attrib.SizeConst }));
        }

        /// <summary>
        /// The CreateSerialCodeProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        public abstract void CreateSerialCodeProperty(TypeBuilder tb, Type type, string name);

        /// <summary>
        /// The CreateSetUniqueKeyMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateSetUniqueKeyMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("SetUniqueKey");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetUniqueKey"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateSetUniqueSeedMethod.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateSetUniqueSeedMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("SetUniqueSeed");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetUniqueSeed"), null);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// The CreateUniqueKeyProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateUniqueKeyProperty(TypeBuilder tb)
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
            il.Emit(OpCodes.Ldflda, fields[0]); // load
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
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.Emit(OpCodes.Ldarg_1); // value
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueKey").GetSetMethod(), null);
            il.Emit(OpCodes.Ret); // return
        }

        /// <summary>
        /// The CreateUniqueSeedProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public virtual void CreateUniqueSeedProperty(TypeBuilder tb)
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
            il.Emit(OpCodes.Ldflda, fields[0]); // load
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
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.Emit(OpCodes.Ldarg_1); // value
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("UniqueSeed").GetSetMethod(), null);
            il.Emit(OpCodes.Ret); // return
        }

        /// <summary>
        /// The CreateValueArrayProperty.
        /// </summary>
        /// <param name="tb">The tb<see cref="TypeBuilder"/>.</param>
        public abstract void CreateValueArrayProperty(TypeBuilder tb);

        /// <summary>
        /// The GetTypeBuilder.
        /// </summary>
        /// <param name="typeName">The typeName<see cref="string"/>.</param>
        /// <returns>The <see cref="TypeBuilder"/>.</returns>
        public abstract TypeBuilder GetTypeBuilder(string typeName);

        /// <summary>
        /// The ResolveFigureAttributes.
        /// </summary>
        /// <param name="fb">The fb<see cref="FieldBuilder"/>.</param>
        /// <param name="mrwa">The mrwa<see cref="MemberRubric"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        public void ResolveFigureAttributes(FieldBuilder fb, MemberRubric mrwa, MemberRubric mr)
        {
            MemberInfo mi = mrwa.RubricInfo;

            resolveFigureKeyAttributes(fb, mi, mr);

            resolveFigureIdentityAttributes(fb, mi, mr);

            resolveFigureRquiredAttributes(fb, mi, mr);

            resolveFigureDisplayAttributes(fb, mi, mr);

            resolveFigureTreatmentAttributes(fb, mi, mr);
        }

        /// <summary>
        /// The ResolveMarshalAsAttributeForArray.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="member">The member<see cref="MemberRubric"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        public void ResolveMarshalAsAttributeForArray(FieldBuilder field, MemberRubric member, Type type)
        {
            MemberInfo _member = member.RubricInfo;
            if (member is MemberRubric && ((MemberRubric)member).FigureField != null)
            {
                _member = ((MemberRubric)member).FigureField;
            }

            object[] o = _member.GetCustomAttributes(typeof(MarshalAsAttribute), false);
            if (o == null || !o.Any())
            {
                o = _member.GetCustomAttributes(typeof(FigureAsAttribute), false);
                if (o != null && o.Any())
                {
                    FigureAsAttribute faa = (FigureAsAttribute)o.First();
                    CreateFigureAsAttribute(field, new FigureAsAttribute(UnmanagedType.ByValArray) { SizeConst = faa.SizeConst });
                }
                else
                {
                    int size = 64;
                    if (member.RubricSize > 0)
                        size = member.RubricSize;
                    CreateFigureAsAttribute(field, new FigureAsAttribute(UnmanagedType.ByValArray) { SizeConst = size });
                }
            }
            else
            {
                MarshalAsAttribute maa = (MarshalAsAttribute)o.First();
                CreateMarshaAslAttribute(field, new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = maa.SizeConst });
            }
        }

        /// <summary>
        /// The ResolveMarshalAsAttributeForString.
        /// </summary>
        /// <param name="field">The field<see cref="FieldBuilder"/>.</param>
        /// <param name="member">The member<see cref="MemberRubric"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        public void ResolveMarshalAsAttributeForString(FieldBuilder field, MemberRubric member, Type type)
        {
            MemberInfo _member = member.RubricInfo;
            if (member is MemberRubric && ((MemberRubric)member).FigureField != null)
            {
                _member = ((MemberRubric)member).FigureField;
            }

            object[] o = _member.GetCustomAttributes(typeof(MarshalAsAttribute), false);
            if (o == null || !o.Any())
            {
                o = _member.GetCustomAttributes(typeof(FigureAsAttribute), false);
                if (o != null && o.Any())
                {
                    FigureAsAttribute maa = (FigureAsAttribute)o.First();
                    CreateFigureAsAttribute(field, new FigureAsAttribute(UnmanagedType.ByValTStr) { SizeConst = maa.SizeConst });
                }
                else
                {
                    int size = 64;
                    if (member.RubricSize > 0)
                        size = member.RubricSize;
                    CreateFigureAsAttribute(field, new FigureAsAttribute(UnmanagedType.ByValTStr) { SizeConst = size });
                }
            }
            else
            {
                MarshalAsAttribute maa = (MarshalAsAttribute)o.First();
                CreateMarshaAslAttribute(field, new MarshalAsAttribute(UnmanagedType.ByValTStr) { SizeConst = maa.SizeConst });
            }
        }

        /// <summary>
        /// The resolveFigureDisplayAttributes.
        /// </summary>
        /// <param name="fb">The fb<see cref="FieldBuilder"/>.</param>
        /// <param name="mi">The mi<see cref="MemberInfo"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        private void resolveFigureDisplayAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureDisplayAttribute), false);
            if (o != null && o.Any())
            {
                FigureDisplayAttribute fda = (FigureDisplayAttribute)o.First(); ;
                mr.DisplayName = fda.Name;

                if (fb != null)
                    CreateFigureDisplayAttribute(fb, fda);
            }
            else if (mr.DisplayName != null)
            {
                CreateFigureDisplayAttribute(fb, new FigureDisplayAttribute(mr.DisplayName));
            }
        }

        /// <summary>
        /// The resolveFigureIdentityAttributes.
        /// </summary>
        /// <param name="fb">The fb<see cref="FieldBuilder"/>.</param>
        /// <param name="mi">The mi<see cref="MemberInfo"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        private void resolveFigureIdentityAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            if (!mr.IsKey)
            {
                object[] o = mi.GetCustomAttributes(typeof(FigureIdentityAttribute), false);
                if (o != null && o.Any())
                {
                    FigureIdentityAttribute fia = (FigureIdentityAttribute)o.First();
                    mr.IsIdentity = true;
                    mr.IsAutoincrement = fia.IsAutoincrement;

                    if (Identities.ContainsKey(fia.Order))
                        fia.Order = (short)(Identities.LastOrDefault().Key + 1);

                    mr.IdentityOrder = fia.Order;
                    Identities.Add(mr.IdentityOrder, mr);

                    if (fb != null)
                        CreateFigureIdentityAttribute(fb, fia);
                }
                else if (mr.IsIdentity)
                {
                    if (Identities.ContainsKey(mr.IdentityOrder))
                        mr.IdentityOrder += (short)(Identities.LastOrDefault().Key + 1);

                    Identities.Add(mr.IdentityOrder, mr);

                    if (fb != null)
                        CreateFigureIdentityAttribute(fb, new FigureIdentityAttribute() { IsAutoincrement = mr.IsAutoincrement, Order = mr.IdentityOrder });
                }
            }
        }

        /// <summary>
        /// The resolveFigureKeyAttributes.
        /// </summary>
        /// <param name="fb">The fb<see cref="FieldBuilder"/>.</param>
        /// <param name="mi">The mi<see cref="MemberInfo"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        private void resolveFigureKeyAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureKeyAttribute), false);
            if (o != null && o.Any())
            {
                FigureKeyAttribute fka = (FigureKeyAttribute)o.First();
                mr.IsKey = true;
                mr.IsIdentity = true;
                mr.IsAutoincrement = fka.IsAutoincrement;

                if (Identities.ContainsKey(fka.Order))
                    fka.Order = (short)(Identities.LastOrDefault().Key + 1);

                mr.IdentityOrder = fka.Order;
                Identities.Add(mr.IdentityOrder, mr);
                mr.Required = true;

                if (fb != null)
                    CreateFigureKeyAttribute(fb, fka);
            }
            else if (mr.IsKey)
            {
                mr.IsIdentity = true;
                mr.Required = true;

                if (Identities.ContainsKey(mr.IdentityOrder))
                    mr.IdentityOrder += (short)(Identities.LastOrDefault().Key + 1);

                Identities.Add(mr.IdentityOrder, mr);

                if (fb != null)
                    CreateFigureKeyAttribute(fb, new FigureKeyAttribute() { IsAutoincrement = mr.IsAutoincrement, Order = mr.IdentityOrder });
            }
        }

        /// <summary>
        /// The resolveFigureRquiredAttributes.
        /// </summary>
        /// <param name="fb">The fb<see cref="FieldBuilder"/>.</param>
        /// <param name="mi">The mi<see cref="MemberInfo"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        private void resolveFigureRquiredAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureRequiredAttribute), false);
            if (o != null && o.Any())
            {
                mr.Required = true;

                if (fb != null)
                    CreateFigureRequiredAttribute(fb);
            }
            else if (mr.Required)
            {
                if (fb != null)
                    CreateFigureRequiredAttribute(fb);
            }
        }

        /// <summary>
        /// The resolveFigureTreatmentAttributes.
        /// </summary>
        /// <param name="fb">The fb<see cref="FieldBuilder"/>.</param>
        /// <param name="mi">The mi<see cref="MemberInfo"/>.</param>
        /// <param name="mr">The mr<see cref="MemberRubric"/>.</param>
        private void resolveFigureTreatmentAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {
            object[] o = mi.GetCustomAttributes(typeof(FigureTreatmentAttribute), false);
            if (o != null && o.Any())
            {
                FigureTreatmentAttribute fta = (FigureTreatmentAttribute)o.First(); ;
                mr.AggregateOperand = fta.AggregateOperand;
                mr.SummaryOperand = fta.SummaryOperand;

                if (fb != null)
                    CreateFigureTreatmentAttribute(fb, fta);
            }
            else if (mr.AggregateOperand != AggregateOperand.None || mr.SummaryOperand != AggregateOperand.None)
            {
                CreateFigureTreatmentAttribute(fb, new FigureTreatmentAttribute() { AggregateOperand = mr.AggregateOperand, SummaryOperand = mr.SummaryOperand });
            }
        }

        #endregion
    }
}
