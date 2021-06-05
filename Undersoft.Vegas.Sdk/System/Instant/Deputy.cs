using System.Reflection;
using System.Reflection.Emit;
using System.Uniques;
using System.Linq;
using System.Threading.Tasks;
using System.Extract;
using System.Multemic;

namespace System.Instant
{
    public delegate object InstantDelegate(object target, object[] parameters);

    public class Deputy: IDeputy
    {
        public   Object TargetObject;
        public Delegate Method;

        public     MethodInfo Info { get; set; }

        public IUnique Empty => Ussn.Empty; 

        public long KeyBlock { get => systemSerialCode.KeyBlock; set => systemSerialCode.KeyBlock = value; }

        public ParameterInfo[] Parameters { get; set; }
        public object[] ParameterValues { get; set; }
        public object[] ValueArray { get => ParameterValues; set => ParameterValues = value; }

        private Ussn systemSerialCode;
        public Ussn SystemSerialCode { get => systemSerialCode; set => systemSerialCode = value; }

        public object this[int fieldId] { get => ParameterValues[fieldId]; set => ParameterValues[fieldId] = value; }
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

        public int NumberOfArguments;

        public Deputy(Delegate TargetMethod)
        {
            TargetObject = TargetMethod.Target;
            Type t2 = TargetObject.GetType();
            MethodInfo m2 = TargetMethod.Method;
            Method = invoking(m2);
            NumberOfArguments = m2.GetParameters().Length;
            Info = m2;
            Parameters = m2.GetParameters();
            long time = DateTime.Now.ToBinary();
            string decription = $"{Info.DeclaringType.FullName}." +
                                   $"{Info.Name}" +
                                   $"{new String(Parameters.SelectMany(p => "." + p.ParameterType.Name).ToArray())}";

            systemSerialCode.KeyBlock = new object[] { decription, DateTime.Now }.GetHashKey();
            systemSerialCode.TimeBlock = time;
        }
        public Deputy(Object TargetClassObject, String MethodName, Type[] parameters)
        {
            TargetObject = TargetClassObject;
            Type t2 = TargetClassObject.GetType();

            MethodInfo m2 = parameters != null ? t2.GetMethod(MethodName, parameters) : t2.GetMethod(MethodName);
            Method = invoking(m2);
            NumberOfArguments = m2.GetParameters().Length;
            Info = m2;
            Parameters = m2.GetParameters();           
            long time = DateTime.Now.ToBinary();
            string decription = $"{Info.DeclaringType.FullName}." + 
                                   $"{Info.Name}" +
                                   $"{new String(Parameters.SelectMany(p => "." + p.ParameterType.Name).ToArray())}";

            systemSerialCode.KeyBlock = new object[] { decription, DateTime.Now }.GetHashKey();
            systemSerialCode.TimeBlock = time;
        }
        public Deputy(String TargetClassName, String MethodName) : this(Summon.New(TargetClassName), MethodName, null)
        {
        }
        public Deputy(String TargetClassName, String MethodName, Type[] parameters) : this(Summon.New(TargetClassName), MethodName, parameters)
        {
        }
        public Deputy(Object TargetClassObject, String MethodName) : this(TargetClassObject, MethodName, null)
        {
        }
        public Deputy(MethodInfo MethodInvokeInfo) : this(MethodInvokeInfo.DeclaringType.FullName, MethodInvokeInfo.Name, MethodInvokeInfo.GetParameters().Select(p => p.ParameterType).ToArray())
        {
        }

        public byte[]   GetBytes()
        {
            return SystemSerialCode.GetBytes();
        }
        public byte[]   GetKeyBytes()
        {
            return SystemSerialCode.GetKeyBytes();
        }
        public void     SetHashKey(long value)
        {
            SystemSerialCode.SetHashKey(value);
        }
        public long     GetHashKey()
        {
            return SystemSerialCode.GetHashKey();
        }
        public bool     Equals(IUnique other)
        {
            return SystemSerialCode.Equals(other);
        }
        public int      CompareTo(IUnique other)
        {
            return SystemSerialCode.CompareTo(other);
        }

        public uint SeedBlock
        {
            get => systemSerialCode.SeedBlock;
            set => systemSerialCode.SeedBlock = value;
        }
        public void SetHashSeed(uint seed)
        {
            systemSerialCode.SetHashSeed(seed);
        }
        public uint GetHashSeed()
        {
            return systemSerialCode.GetHashSeed();
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

        private    Delegate invoking(MethodInfo methodInfo)
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
        private static void boxing(ILGenerator il, System.Type type)
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
    }   

    public static class Summon
    {
        public static object New(Type type)
        {          
            if (type != null)
                return Activator.CreateInstance(type);
            string strFullyQualifiedName = type.FullName;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }
        public static object New(Type type, params object[] constructorParams)
        {
            if (type != null)
                return Activator.CreateInstance(type, constructorParams);

            string strFullyQualifiedName = type.FullName;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type, constructorParams);
            }
            return null;
        }
        public static object New(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }
        public static object New(string strFullyQualifiedName, params object[] constructorParams)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type, constructorParams);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type, constructorParams);
            }
            return null;
        }
    }

    public class ItemChangedEventArgs<T> : EventArgs
    {
        public readonly T ChangedItem;
        public readonly ChangesType ChangesType;
        public readonly T ReplacedWith;

        public ItemChangedEventArgs(ChangesType changesType, T changedItem,
            T replacement)
        {
            ChangesType = changesType;
            ChangedItem = changedItem;
            ReplacedWith = replacement;
        }
    }

    public enum ChangesType
    {
        Added,
        Removed,
        Replaced,
        Cleared
    };
}
