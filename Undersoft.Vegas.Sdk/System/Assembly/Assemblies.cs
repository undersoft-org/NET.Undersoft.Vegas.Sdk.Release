/*************************************************
   Copyright (c) 2021 Undersoft

   System.Assemblies.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="Assemblies" />.
    /// </summary>
    public static class Assemblies
    {
        #region Fields

        public static bool resolveAssigned;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="Assemblies"/> class.
        /// </summary>
        static Assemblies()
        {
            resolveAssigned = ResolveExecuting();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the AssemblyCode.
        /// </summary>
        public static string AssemblyCode
        {
            get
            {
                object[] attributes;
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly is null)
                    attributes = Assembly.GetCallingAssembly()
                        .GetCustomAttributes(typeof(GuidAttribute), false);
                else
                    attributes = entryAssembly
                        .GetCustomAttributes(typeof(GuidAttribute), false);
                if (attributes.Length == 0)
                    return String.Empty;
                return ((GuidAttribute)attributes[0]).Value.ToUpper();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The FindType.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="nameSpace">The nameSpace<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static Type FindType(string name, string nameSpace = null)
        {
            Type type = Type.GetType(name);
            if (type != null)
                return type;
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms)
            {

                type = asm.GetType(name);
                if (type != null)
                    if (nameSpace == null || type.Namespace == nameSpace)
                        return type;
            }
            return null;
        }

        /// <summary>
        /// The FindType.
        /// </summary>
        /// <param name="argumentType">The argumentType<see cref="Type"/>.</param>
        /// <param name="argumentValue">The argumentValue<see cref="object"/>.</param>
        /// <param name="attributeType">The attributeType<see cref="Type"/>.</param>
        /// <param name="nameSpace">The nameSpace<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static Type FindType(Type argumentType, object argumentValue, Type attributeType = null, string nameSpace = null)
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var asm in asms)
            {
                Type[] types = nameSpace != null ?
                                    asm.GetTypes().Where(n => n.Namespace == nameSpace).ToArray() :
                                        asm.GetTypes();
                if (attributeType != null)
                {
                    foreach (var type in types)
                        if (type.GetCustomAttributesData().Where(a => a.AttributeType == attributeType).Where(s => s.ConstructorArguments.Where(o => o.ArgumentType == argumentType &&
                                                        o.Value.Equals(argumentValue)).Any()).Any())
                            return type;
                }
                else
                    foreach (var type in types)
                        if (type.GetCustomAttributesData().Where(s => s.ConstructorArguments.Where(o => o.ArgumentType == argumentType &&
                                                        o.Value.Equals(argumentValue)).Any()).Any())
                            return type;
            }
            return null;
        }

        /// <summary>
        /// The ResolveExecuting.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ResolveExecuting()
        {
            if (!resolveAssigned)
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    String resourceName = "AssemblyLoadingAndReflection." + new AssemblyName(args.Name).Name + ".dll";
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        Byte[] assemblyData = new Byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        return Assembly.Load(assemblyData);
                    }
                };
            }
            return true;
        }

        #endregion
    }
}
