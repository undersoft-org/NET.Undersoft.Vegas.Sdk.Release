/*************************************************
   Copyright (c) 2021 Undersoft

   System.TypeExtension.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="TypeExtension" />.
    /// </summary>
    public static class TypeExtension
    {
        #region Methods

        /// <summary>
        /// The Default.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object Default(this Type type)
        {
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> is not a publicly-visible type, so the default value cannot be retrieved");
        }

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="queue">The queue<see cref="Queue{object}"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        /// <returns>The <see cref="Queue{object}"/>.</returns>
        public static Queue<object>
                               New(this Queue<object> queue, Type type, int count)
        {
            if (type.IsInterface)
                type = type.GetType();
            for (int i = 0; i < count; i++)
            {
                queue.Enqueue(Activator.CreateInstance(type));
            }
            return queue;
        }

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object New(this Type type)
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
        public static object New(this Type type, params object[] ctorArguments)
        {
            if (type.IsInterface)
                type = type.GetType();
            return Activator.CreateInstance(type, ctorArguments);
        }

        /// <summary>
        /// The New.
        /// </summary>
        /// <param name="types">The types<see cref="Type[]"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public static object[] New(this Type[] types)
        {
            object[] models = new object[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.IsInterface)
                    type = type.GetType();
                models[i] = Activator.CreateInstance(type);
            }
            return models;
        }

        #endregion
    }
}
