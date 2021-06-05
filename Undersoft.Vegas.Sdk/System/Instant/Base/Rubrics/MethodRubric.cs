/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.MethodRubric.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="MethodRubric" />.
    /// </summary>
    public class MethodRubric : MethodInfo, IMemberRubric
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRubric"/> class.
        /// </summary>
        public MethodRubric()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRubric"/> class.
        /// </summary>
        /// <param name="method">The method<see cref="MethodInfo"/>.</param>
        /// <param name="propertyId">The propertyId<see cref="int"/>.</param>
        public MethodRubric(MethodInfo method, int propertyId = -1) :
            this(method.DeclaringType, method.Name, method.ReturnType, method.GetParameters(), method.Module, propertyId)
        {
            RubricInfo = method;
            RubricType = method.DeclaringType;
            RubricName = method.Name;
            RubricParameterInfo = method.GetParameters();
            RubricReturnType = method.ReturnType;
            RubricModule = method.Module;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodRubric"/> class.
        /// </summary>
        /// <param name="declaringType">The declaringType<see cref="Type"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <param name="returnType">The returnType<see cref="Type"/>.</param>
        /// <param name="parameterTypes">The parameterTypes<see cref="ParameterInfo[]"/>.</param>
        /// <param name="module">The module<see cref="Module"/>.</param>
        /// <param name="propertyId">The propertyId<see cref="int"/>.</param>
        public MethodRubric(Type declaringType, string propertyName, Type returnType, ParameterInfo[] parameterTypes, Module module, int propertyId = -1)
        {
            RubricType = declaringType;
            RubricName = propertyName;
            RubricId = propertyId;
            RubricParameterInfo = parameterTypes;
            RubricReturnType = returnType;
            RubricModule = module;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Attributes.
        /// </summary>
        public override MethodAttributes Attributes => RubricInfo.Attributes;

        /// <summary>
        /// Gets the DeclaringType.
        /// </summary>
        public override Type DeclaringType => RubricInfo != null ? RubricInfo.DeclaringType : null;

        /// <summary>
        /// Gets or sets a value indicating whether Editable.
        /// </summary>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// Gets the MethodHandle.
        /// </summary>
        public override RuntimeMethodHandle MethodHandle => RubricInfo.MethodHandle;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public override string Name => RubricName;

        /// <summary>
        /// Gets the ReflectedType.
        /// </summary>
        public override Type ReflectedType => RubricInfo != null ? RubricInfo.ReflectedType : null;

        /// <summary>
        /// Gets the ReturnTypeCustomAttributes.
        /// </summary>
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => RubricInfo.ReturnTypeCustomAttributes;

        /// <summary>
        /// Gets or sets the RubricAttributes.
        /// </summary>
        public object[] RubricAttributes { get; set; } = null;

        /// <summary>
        /// Gets or sets the RubricId.
        /// </summary>
        public int RubricId { get; set; } = -1;

        /// <summary>
        /// Gets or sets the RubricInfo.
        /// </summary>
        public MethodInfo RubricInfo { get; set; }

        /// <summary>
        /// Gets or sets the RubricModule.
        /// </summary>
        public Module RubricModule { get; set; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        public string RubricName { get; set; }

        /// <summary>
        /// Gets or sets the RubricOffset.
        /// </summary>
        public int RubricOffset { get; set; } = -1;

        /// <summary>
        /// Gets or sets the RubricParameterInfo.
        /// </summary>
        public ParameterInfo[] RubricParameterInfo { get; set; }

        /// <summary>
        /// Gets or sets the RubricReturnType.
        /// </summary>
        public Type RubricReturnType { get; set; }

        /// <summary>
        /// Gets or sets the RubricSize.
        /// </summary>
        public int RubricSize { get; set; } = -1;

        /// <summary>
        /// Gets or sets the RubricType.
        /// </summary>
        public Type RubricType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// The GetBaseDefinition.
        /// </summary>
        /// <returns>The <see cref="MethodInfo"/>.</returns>
        public override MethodInfo GetBaseDefinition()
        {
            return RubricInfo.GetBaseDefinition();
        }

        /// <summary>
        /// The GetCustomAttributes.
        /// </summary>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public override object[] GetCustomAttributes(bool inherit)
        {
            return RubricInfo.GetCustomAttributes(inherit);
        }

        /// <summary>
        /// The GetCustomAttributes.
        /// </summary>
        /// <param name="attributeType">The attributeType<see cref="Type"/>.</param>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return RubricInfo.GetCustomAttributes(attributeType, inherit);
        }

        /// <summary>
        /// The GetMethodImplementationFlags.
        /// </summary>
        /// <returns>The <see cref="MethodImplAttributes"/>.</returns>
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return RubricInfo.GetMethodImplementationFlags();
        }

        /// <summary>
        /// The GetParameters.
        /// </summary>
        /// <returns>The <see cref="ParameterInfo[]"/>.</returns>
        public override ParameterInfo[] GetParameters()
        {
            return RubricInfo.GetParameters();
        }

        /// <summary>
        /// The Invoke.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="invokeAttr">The invokeAttr<see cref="BindingFlags"/>.</param>
        /// <param name="binder">The binder<see cref="Binder"/>.</param>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return RubricInfo.Invoke(obj, invokeAttr, binder, parameters, culture);
        }

        /// <summary>
        /// The IsDefined.
        /// </summary>
        /// <param name="attributeType">The attributeType<see cref="Type"/>.</param>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if (this.GetCustomAttributes(attributeType, inherit) != null)
                return true;
            return false;
        }

        #endregion
    }
}
