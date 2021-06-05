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
    public abstract class FigureCompiler : FigureCompilerConstructors
    {
        public SortedList<short, MemberRubric> Identities
            = new SortedList<short, MemberRubric>();

        protected MemberRubrics fieldRubrics;
        protected MemberRubrics propertyRubrics;
        public FieldRubric[] derivedFields;
        protected FieldBuilder[] fields = null;
        protected PropertyBuilder[] props = null;
        protected FigureMode mode;
        protected int length;
        protected int scode = 1;
        protected Figure figure;
        protected bool IsDerived => figure.IsDerived;

        public FigureCompiler(Figure instantFigure, MemberRubrics fieldRubrics, MemberRubrics propertyRubrics)
        {
            this.fieldRubrics = fieldRubrics;
            this.propertyRubrics = propertyRubrics;
            figure = instantFigure;
            length = fieldRubrics.Count;
        }

        public abstract Type CompileFigureType(string typeName);

        public abstract TypeBuilder GetTypeBuilder(string typeName);

        public abstract void CreateSerialCodeProperty(TypeBuilder tb, Type type, string name);

        public abstract FieldBuilder[] CreateFieldsAndProperties(TypeBuilder tb);

        public abstract void CreateValueArrayProperty(TypeBuilder tb);

        public abstract void CreateItemByIntProperty(TypeBuilder tb);

        public abstract void CreateItemByStringProperty(TypeBuilder tb);

        public abstract void CreateGetBytesMethod(TypeBuilder tb);

        public void ResolveFigureAttributes(FieldBuilder fb, MemberRubric mrwa, MemberRubric mr)
        {
            MemberInfo mi = mrwa.RubricInfo;

            resolveFigureKeyAttributes(fb, mi, mr);

            resolveFigureIdentityAttributes(fb, mi, mr);

            resolveFigureRquiredAttributes(fb, mi, mr);

            resolveFigureDisplayAttributes(fb, mi, mr);

            resolveFigureTreatmentAttributes(fb, mi, mr);
        }

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

        public void CreateMarshaAslAttribute(FieldBuilder field, MarshalAsAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value },
                                                                               new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                                                                               new object[] { attrib.SizeConst }));
        }

        public void CreateFigureAsAttribute(FieldBuilder field, FigureAsAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value },
                                                                               new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                                                                               new object[] { attrib.SizeConst }));
        }

        public void CreateFigureKeyAttribute(FieldBuilder field, FigureKeyAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureKeyCtor, Type.EmptyTypes,
                                                                               new FieldInfo[] { typeof(FigureKeyAttribute).GetField("Order"),
                                                                                                 typeof(FigureKeyAttribute).GetField("IsAutoincrement") },
                                                                               new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        public void CreateFigureIdentityAttribute(FieldBuilder field, FigureIdentityAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureIdentityCtor, Type.EmptyTypes,
                                                                                    new FieldInfo[] { typeof(FigureIdentityAttribute).GetField("Order"),
                                                                                                      typeof(FigureIdentityAttribute).GetField("IsAutoincrement") },
                                                                                    new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        public void CreateFigureRequiredAttribute(FieldBuilder field)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureRequiredCtor, Type.EmptyTypes));
        }

        public void CreateFigureDisplayAttribute(FieldBuilder field, FigureDisplayAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureDisplayCtor, new object[] { attrib.Name }));
        }

        public void CreateFigureTreatmentAttribute(FieldBuilder field, FigureTreatmentAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figuresTreatmentCtor, Type.EmptyTypes,
                                                                                        new FieldInfo[] { typeof(FigureTreatmentAttribute).GetField("AggregateOperand"),
                                                                                                          typeof(FigureTreatmentAttribute).GetField("SummaryOperand") },
                                                                                        new object[] { attrib.AggregateOperand, attrib.SummaryOperand }));
        }

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

            // return prop;
        }

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

            // return prop;
        }

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

    }

    public enum FigureMode
    {
        Derived,
        ValueType,
        Reference
    }

}