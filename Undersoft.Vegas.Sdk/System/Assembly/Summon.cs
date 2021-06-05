/*************************************************
   Copyright (c) 2021 Undersoft

   System.Summon.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    /// <summary>
    /// Defines the <see cref="Summon" />.
    /// </summary>
    public static class Summon
    {
        #region Methods

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="strFullyQualifiedName">The strFullyQualifiedName<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
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

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="strFullyQualifiedName">The strFullyQualifiedName<see cref="string"/>.</param>
        /// <param name="constructorParams">The constructorParams<see cref="object[]"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
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

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object New(Type type)
        {
            if (type.IsInterface)
                type = type.GetType();
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="ctorArguments">The ctorArguments<see cref="object[]"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object New(Type type, params object[] ctorArguments)
        {
            if (type.IsInterface)
                type = type.GetType();
            return Activator.CreateInstance(type, ctorArguments);
        }

        #endregion
    }
}
