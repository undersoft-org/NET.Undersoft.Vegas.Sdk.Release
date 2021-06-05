/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Deputy.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading.Tasks;
    using System.Uniques;

    #region Delegates

    /// <summary>
    /// The InstantDelegate.
    /// </summary>
    /// <param name="target">The target<see cref="object"/>.</param>
    /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
    /// <returns>The <see cref="object"/>.</returns>
    public delegate object InstantDelegate(object target, params object[] parameters);

    #endregion

    #region Enums

    public enum ChangesType
    {
        Added,
        Removed,
        Replaced,
        Cleared
    };

    #endregion



    public class Deputy : IDeputy
    {
        private Ussn serialcode;

        public Deputy(Delegate TargetMethod)
        {
            TargetObject = TargetMethod.Target;
            Type t = TargetObject.GetType();
            MethodInfo m = TargetMethod.Method;

            Method = invoking(m);

            NumberOfArguments = m.GetParameters().Length;
            Info = m;
            Parameters = m.GetParameters();
            long time = DateTime.Now.ToBinary();
            string decription = $"{Info.DeclaringType.FullName}." +
                                $"{Info.Name}" +
                                $"{new String(Parameters.SelectMany(p => "." + p.ParameterType.Name).ToArray())}";

            ulong seed = Unique.NewKey;
            serialcode.UniqueKey = decription.UniqueKey64(seed);
            serialcode.UniqueSeed = seed;
            serialcode.TimeBlock = time;
        }
        public Deputy(Object TargetObject, String MethodName) : this(TargetObject, MethodName, null)
        {
        }
        public Deputy(Object TargetObject, String MethodName, Type[] parameters)
        {
            this.TargetObject = TargetObject;
            Type t = TargetObject.GetType();

            MethodInfo m = parameters != null ? t.GetMethod(MethodName, parameters) : t.GetMethod(MethodName);
            Method = invoking(m);
            NumberOfArguments = m.GetParameters().Length;
            Info = m;
            Parameters = m.GetParameters();
            long time = DateTime.Now.ToBinary();
            string decription = $"{Info.DeclaringType.FullName}." +
                                $"{Info.Name}" +
                                $"{new String(Parameters.SelectMany(p => "." + p.ParameterType.Name).ToArray())}";

            ulong seed = Unique.NewKey;
            serialcode.UniqueKey = decription.UniqueKey64(seed);
            serialcode.UniqueSeed = seed;
            serialcode.TimeBlock = time;
        }
        public Deputy(Type TargetType, String MethodName) : this(Summon.New(TargetType), MethodName, null)
        {
        }
        public Deputy(Type TargetType, String MethodName, Type[] parameters) : this(Summon.New(TargetType), MethodName, parameters)
        {
        }
        public Deputy(String TargetName, String MethodName) : this(Summon.New(TargetName), MethodName, null)
        {
        }
        public Deputy(String TargetName, String MethodName, Type[] parameters) : this(Summon.New(TargetName), MethodName, parameters)
        {
        }
        public Deputy(MethodInfo MethodInvokeInfo) : this(MethodInvokeInfo.DeclaringType.New(), MethodInvokeInfo.Name, MethodInvokeInfo.GetParameters().Select(p => p.ParameterType).ToArray())
        {
        }

        public object this[int fieldId]
        {
            get => ParameterValues[fieldId];
            set => ParameterValues[fieldId] = value;
        }
        public object this[string propertyName]
        {
            get
            {
                for (int i = 0; i < Parameters.Length; i++)
                    if (Parameters[i].Name == propertyName)
                        return ParameterValues[i];
                return null;
            }
            set
            {
                for (int i = 0; i < Parameters.Length; i++)
                    if (Parameters[i].Name == propertyName)
                        ParameterValues[i] = value;
            }
        }

        public Object TargetObject { get; set; }

        public Delegate Method { get; set; }

        public MethodInfo Info { get; set; }

        public IUnique Empty => Ussn.Empty;

        public ulong UniqueKey
        {
            get => serialcode.UniqueKey;
            set => serialcode.UniqueKey = value;
        }

        public ulong UniqueSeed
        {
            get => serialcode.UniqueSeed;
            set => serialcode.UniqueSeed = value;
        }

        public ParameterInfo[] Parameters { get; set; }

        public object[] ParameterValues { get; set; }

        public object[] ValueArray { get => ParameterValues; set => ParameterValues = value; }

        public Ussn SerialCode { get => serialcode; set => serialcode = value; }

        public int NumberOfArguments { get; set; }

        private Delegate invoking(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty,
                          typeof(object), new Type[] { typeof(object),
                          typeof(object[]) },
                          methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                directint(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                casting(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                boxing(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    directint(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            Delegate invoder = (InstantDelegate)
               dynamicMethod.CreateDelegate(typeof(InstantDelegate));
            return invoder;
        }

        private static void casting(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private static void boxing(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void directint(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        public object Execute(params object[] FunctionParameters)
        {
            try
            {
                return Method.DynamicInvoke(TargetObject, FunctionParameters);
            }
            catch (Exception e)
            {
                throw new TargetInvocationException(e);
            }
        }

        public T Execute<T>(params object[] FunctionParameters)
        {
            try
            {
                return (T)Method.DynamicInvoke(TargetObject, FunctionParameters);
            }
            catch (Exception e)
            {
                throw new TargetInvocationException(e);
            }
        }

        public async Task<object> ExecuteAsync(params object[] FunctionParameters)
        {
            try
            {
                return await Task.Run<object>(() => Execute(FunctionParameters)).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                throw new TargetInvocationException(e);
            }
        }

        public async Task<T> ExecuteAsync<T>(params object[] FunctionParameters)
        {
            try
            {
                return await Task.Run<T>(() => Execute<T>(FunctionParameters)).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                throw new TargetInvocationException(e);
            }
        }

        public object TypeConvert(object source, Type DestType)
        {

            object NewObject = System.Convert.ChangeType(source, DestType);
            return (NewObject);
        }
    }

    /// <summary>
    /// Defines the <see cref="ItemChangedEventArgs{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ItemChangedEventArgs<T> : EventArgs
    {
        #region Fields

        public readonly T ChangedItem;
        public readonly ChangesType ChangesType;
        public readonly T ReplacedWith;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="changesType">The changesType<see cref="ChangesType"/>.</param>
        /// <param name="changedItem">The changedItem<see cref="T"/>.</param>
        /// <param name="replacement">The replacement<see cref="T"/>.</param>
        public ItemChangedEventArgs(ChangesType changesType, T changedItem,
            T replacement)
        {
            ChangesType = changesType;
            ChangedItem = changedItem;
            ReplacedWith = replacement;
        }

        #endregion
    }
}
