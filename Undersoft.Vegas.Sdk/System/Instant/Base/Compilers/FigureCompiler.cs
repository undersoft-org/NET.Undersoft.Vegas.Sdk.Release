using System.Uniques;
using System.Instant.Treatment;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace System.Instant
{   
    public class FigureCompiler
    {
        public SortedList<short, MemberRubric> identities = new SortedList<short, MemberRubric>();

        public readonly ConstructorInfo dataMemberCtor = typeof(DataMemberAttribute)
                                                            .GetConstructor(Type.EmptyTypes);

        public readonly PropertyInfo[] dataMemberProps = new[] { typeof(DataMemberAttribute).GetProperty("Order"), 
                                                         typeof(DataMemberAttribute).GetProperty("Name") };

        public readonly ConstructorInfo structLayoutCtor = typeof(StructLayoutAttribute)
                                                            .GetConstructor(new Type[] { typeof(LayoutKind) });

        public readonly FieldInfo[] structLayoutFields = new[] { typeof(StructLayoutAttribute).GetField("CharSet"),
                                                         typeof(StructLayoutAttribute).GetField("Pack") };

        public readonly ConstructorInfo marshalAsCtor = typeof(MarshalAsAttribute)
                                                            .GetConstructor(new Type[] { typeof(UnmanagedType) });

        public readonly ConstructorInfo figureKeyCtor = typeof(FigureKeyAttribute)
                                                            .GetConstructor(Type.EmptyTypes);

        public readonly ConstructorInfo figureIdentityCtor = typeof(FigureKeyAttribute)
                                                            .GetConstructor(Type.EmptyTypes);

        public readonly ConstructorInfo figureRequiredCtor = typeof(FigureRequiredAttribute)
                                                            .GetConstructor(Type.EmptyTypes);

        public readonly ConstructorInfo figureDisplayCtor = typeof(FigureDisplayAttribute)
                                                            .GetConstructor(new Type[] { typeof(string) });

        public readonly ConstructorInfo figuresTreatmentCtor = typeof(FigureTreatmentAttribute)
                                                           .GetConstructor(Type.EmptyTypes);

        public FieldBuilder[] fields = null;
        public PropertyBuilder[] props = null;
        public FigureMode mode;
        public int length;
        public int scode = 1;
        public MemberRubrics members => (MemberRubrics)figure.Rubrics;
        public Figure figure;

        public FigureCompiler(Figure instantFigure)
        {
            figure = instantFigure;
            length = members.Count;        
        }

        public void CreateFieldCustomAttributes(FieldBuilder fb, MemberInfo mi, MemberRubric mr)
        {          
            object[] o = mi.GetCustomAttributes(typeof(FigureKeyAttribute), false);
            if (o != null && o.Any())
            {
                FigureKeyAttribute fka = (FigureKeyAttribute)o.FirstOrDefault();
                mr.IsKey = true;
                mr.IsIdentity = true;
                mr.IsAutoincrement = fka.IsAutoincrement;
                if (identities.ContainsKey(fka.Order))
                    fka.Order = (short)(identities.LastOrDefault().Key + 1);
                mr.IdentityOrder = fka.Order;
                identities.Add(mr.IdentityOrder, mr);              
                mr.Required = true;
                CreateFigureKeyAttribute(fb, mi, fka);
            }
            else if (mr.IsKey)
            {
                mr.IsIdentity = true;
                mr.Required = true;
                if (identities.ContainsKey(mr.IdentityOrder))
                    mr.IdentityOrder += (short)(identities.LastOrDefault().Key + 1);
                identities.Add(mr.IdentityOrder, mr);
                CreateFigureKeyAttribute(fb, mi, new FigureKeyAttribute() { IsAutoincrement = mr.IsAutoincrement, Order = mr.IdentityOrder });             
            }

            if (!mr.IsKey)
            {
                o = mi.GetCustomAttributes(typeof(FigureIdentityAttribute), false);
                if (o != null && o.Any())
                {
                    FigureIdentityAttribute fia = (FigureIdentityAttribute)o.FirstOrDefault();
                    mr.IsIdentity = true;
                    mr.IsAutoincrement = fia.IsAutoincrement;
                    if (identities.ContainsKey(fia.Order))
                        fia.Order = (short)(identities.LastOrDefault().Key + 1);
                    mr.IdentityOrder = fia.Order;
                    identities.Add(mr.IdentityOrder, mr);

                    CreateFigureIdentityAttribute(fb, mi, fia);
                }
                else if (((MemberRubric)mi).IsIdentity)
                {
                    if (identities.ContainsKey(mr.IdentityOrder))
                        mr.IdentityOrder += (short)(identities.LastOrDefault().Key + 1);
                    identities.Add(mr.IdentityOrder, mr);
                    CreateFigureIdentityAttribute(fb, mi, new FigureIdentityAttribute() { IsAutoincrement = mr.IsAutoincrement, Order = mr.IdentityOrder });
                }
            }

            o = mi.GetCustomAttributes(typeof(FigureRequiredAttribute), false);
            if (o != null && o.Any())
            {
                mr.Required = true;
                CreateFigureRequiredAttribute(fb, mi);
            }
            else if (mr.Required)
            {
                CreateFigureRequiredAttribute(fb, mi);
            }

            o = mi.GetCustomAttributes(typeof(FigureDisplayAttribute), false);
            if (o != null && o.Any())
            {
                FigureDisplayAttribute fda = (FigureDisplayAttribute)o.FirstOrDefault();
                mr.DisplayName = fda.Name;
                CreateFigureDisplayAttribute(fb, mi, fda);
            }
            else if (mr.DisplayName != null)
            {
                CreateFigureDisplayAttribute(fb, mi, new FigureDisplayAttribute(mr.DisplayName));
            }

            o = mi.GetCustomAttributes(typeof(FigureTreatmentAttribute), false);
            if (o != null && o.Any())
            {
                FigureTreatmentAttribute fta = (FigureTreatmentAttribute)o.FirstOrDefault();
                mr.AggregateOperand = fta.AggregateOperand;
                mr.SummaryOperand = fta.SummaryOperand;
                CreateFigureTreatmentAttribute(fb, mi, fta);
            }
            else if (mr.AggregateOperand != AggregateOperand.None || mr.SummaryOperand != AggregateOperand.None)
            {
                CreateFigureTreatmentAttribute(fb, mi, new FigureTreatmentAttribute() { AggregateOperand = mr.AggregateOperand, SummaryOperand = mr.SummaryOperand } );
            }
        }

        public void CreateMarshalAttribute(FieldBuilder field, MemberInfo member, MarshalAsAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value },
                                                                               new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") },
                                                                               new object[] { attrib.SizeConst }));
        }

        public void CreateFigureAsAttribute(FieldBuilder field, MemberInfo member, FigureAsAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(marshalAsCtor, new object[] { attrib.Value }, 
                                                                               new FieldInfo[] { typeof(MarshalAsAttribute).GetField("SizeConst") }, 
                                                                               new object[] { attrib.SizeConst }));
        }

        public void CreateFigureKeyAttribute(FieldBuilder field, MemberInfo member, FigureKeyAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureKeyCtor, Type.EmptyTypes,
                                                                               new FieldInfo[] { typeof(FigureKeyAttribute).GetField("Order"),
                                                                                                 typeof(FigureKeyAttribute).GetField("IsAutoincrement") },
                                                                               new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        public void CreateFigureIdentityAttribute(FieldBuilder field, MemberInfo member, FigureIdentityAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureIdentityCtor, Type.EmptyTypes, 
                                                                                    new FieldInfo[] { typeof(FigureIdentityAttribute).GetField("Order"),
                                                                                                      typeof(FigureIdentityAttribute).GetField("IsAutoincrement") },
                                                                                    new object[] { attrib.Order, attrib.IsAutoincrement }));
        }

        public void CreateFigureRequiredAttribute(FieldBuilder field, MemberInfo member)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureRequiredCtor, Type.EmptyTypes));
        }

        public void CreateFigureDisplayAttribute(FieldBuilder field, MemberInfo member, FigureDisplayAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figureDisplayCtor, new object[] { attrib.Name }));
        }

        public void CreateFigureTreatmentAttribute(FieldBuilder field, MemberInfo member, FigureTreatmentAttribute attrib)
        {
            field.SetCustomAttribute(new CustomAttributeBuilder(figuresTreatmentCtor,   Type.EmptyTypes, 
                                                                                        new FieldInfo[] { typeof(FigureTreatmentAttribute).GetField("AggregateOperand"),
                                                                                                          typeof(FigureTreatmentAttribute).GetField("SummaryOperand") },
                                                                                        new object[] { attrib.AggregateOperand, attrib.SummaryOperand }));
        }

        public void CreateGetKeyBytesMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetKeyBytes");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetKeyBytes"), null);
            il.Emit(OpCodes.Ret);
        }

        public void CreateGetHashKeyMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetHashKey");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetHashKey"), null);
            il.Emit(OpCodes.Ret);
        }

        public void CreateSetHashKeyMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("SetHashKey");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetHashKey"), null);
            il.Emit(OpCodes.Ret);
        }

        public void CreateGetHashSeedMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("GetHashSeed");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("GetHashSeed"), null);
            il.Emit(OpCodes.Ret);
        }

        public void CreateSetHashSeedMethod(TypeBuilder tb)
        {
            MethodInfo createArray = typeof(IUnique).GetMethod("SetHashSeed");

            ParameterInfo[] args = createArray.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder method = tb.DefineMethod(createArray.Name, createArray.Attributes & ~MethodAttributes.Abstract,
                                                          createArray.CallingConvention, createArray.ReturnType, argTypes);
            tb.DefineMethodOverride(method, createArray);

            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldflda, fields[0]);
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetMethod("SetHashSeed"), null);
            il.Emit(OpCodes.Ret);
        }

        public void CreateEqualsMethod(TypeBuilder tb)
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

        public void CreateCompareToMethod(TypeBuilder tb)
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

        public void CreateGetEmptyProperty(TypeBuilder tb)
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

        public void CreateKeyBlockProperty(TypeBuilder tb)
        {

            PropertyBuilder prop = tb.DefineProperty("KeyBlock", PropertyAttributes.HasDefault,
                                                     typeof(long), new Type[] { typeof(long) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("KeyBlock");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            //tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();           

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("KeyBlock").GetGetMethod(), null);
            il.Emit(OpCodes.Ret); // return

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                          mutator.CallingConvention, mutator.ReturnType, argTypes);
            //tb.DefineMethodOverride(setter, mutator);

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.Emit(OpCodes.Ldarg_1); // value
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("KeyBlock").GetSetMethod(), null);
            il.Emit(OpCodes.Ret); // return

           // return prop;
        }

        public void CreateSeedBlockProperty(TypeBuilder tb)
        {

            PropertyBuilder prop = tb.DefineProperty("SeedBlock", PropertyAttributes.HasDefault,
                                                     typeof(long), new Type[] { typeof(long) });

            PropertyInfo iprop = typeof(IUnique).GetProperty("SeedBlock");

            MethodInfo accessor = iprop.GetGetMethod();

            ParameterInfo[] args = accessor.GetParameters();
            Type[] argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder getter = tb.DefineMethod(accessor.Name, accessor.Attributes & ~MethodAttributes.Abstract,
                                                          accessor.CallingConvention, accessor.ReturnType, argTypes);
            //tb.DefineMethodOverride(getter, accessor);

            prop.SetGetMethod(getter);
            ILGenerator il = getter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("SeedBlock").GetGetMethod(), null);
            il.Emit(OpCodes.Ret); // return

            MethodInfo mutator = iprop.GetSetMethod();

            args = mutator.GetParameters();
            argTypes = Array.ConvertAll(args, a => a.ParameterType);

            MethodBuilder setter = tb.DefineMethod(mutator.Name, mutator.Attributes & ~MethodAttributes.Abstract,
                                                          mutator.CallingConvention, mutator.ReturnType, argTypes);
            //tb.DefineMethodOverride(setter, mutator);

            prop.SetSetMethod(setter);
            il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Ldflda, fields[0]); // load
            il.Emit(OpCodes.Ldarg_1); // value
            il.EmitCall(OpCodes.Call, typeof(Ussn).GetProperty("SeedBlock").GetSetMethod(), null);
            il.Emit(OpCodes.Ret); // return

            // return prop;
        }

        public void CreateGetGenericByIntMethod(TypeBuilder tb)
        {
            string[] typeParameterNames = { "V" };
            GenericTypeParameterBuilder[] typeParameters =
                tb.DefineGenericParameters(typeParameterNames);

            GenericTypeParameterBuilder V = typeParameters[0];

            MethodInfo mi = typeof(IFigure).GetMethod("Get", new Type[] { typeof(int) }).MakeGenericMethod();

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
        ValueType,
        Reference
    }

}