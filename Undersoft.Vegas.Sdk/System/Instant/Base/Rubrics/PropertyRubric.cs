/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.PropertyRubric.cs
   
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
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="PropertyRubric" />.
    /// </summary>
    public class PropertyRubric : PropertyInfo, IMemberRubric
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRubric"/> class.
        /// </summary>
        public PropertyRubric()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRubric"/> class.
        /// </summary>
        /// <param name="property">The property<see cref="PropertyInfo"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="propertyId">The propertyId<see cref="int"/>.</param>
        public PropertyRubric(PropertyInfo property, int size = -1, int propertyId = -1) : this(property.PropertyType, property.Name, propertyId)
        {
            RubricInfo = property;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRubric"/> class.
        /// </summary>
        /// <param name="propertyType">The propertyType<see cref="Type"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="propertyId">The propertyId<see cref="int"/>.</param>
        public PropertyRubric(Type propertyType, string propertyName, int size = -1, int propertyId = -1)
        {
            RubricType = propertyType;
            RubricName = propertyName;
            RubricId = propertyId;
            if (propertyType.IsValueType)
            {
                if (propertyType == typeof(DateTime))
                    RubricSize = 8;
                else
                    RubricSize = Marshal.SizeOf(propertyType);
            }
            if (size > 0)
                RubricSize = size;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Attributes.
        /// </summary>
        public override PropertyAttributes Attributes => RubricInfo != null ? RubricInfo.Attributes : PropertyAttributes.HasDefault;

        /// <summary>
        /// Gets a value indicating whether CanRead.
        /// </summary>
        public override bool CanRead => Visible;

        /// <summary>
        /// Gets a value indicating whether CanWrite.
        /// </summary>
        public override bool CanWrite => Editable;

        /// <summary>
        /// Gets the DeclaringType.
        /// </summary>
        public override Type DeclaringType => RubricInfo != null ? RubricInfo.DeclaringType : null;

        /// <summary>
        /// Gets or sets a value indicating whether Editable.
        /// </summary>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public override string Name => RubricName;

        /// <summary>
        /// Gets the PropertyType.
        /// </summary>
        public override Type PropertyType => RubricType;

        /// <summary>
        /// Gets the ReflectedType.
        /// </summary>
        public override Type ReflectedType => RubricInfo != null ? RubricInfo.ReflectedType : null;

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
        public PropertyInfo RubricInfo { get; set; }

        /// <summary>
        /// Gets or sets the RubricModule.
        /// </summary>
        public Module RubricModule { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        public PropertyInfo[] RubricParameterInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the RubricReturnType.
        /// </summary>
        public Type RubricReturnType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        /// The GetAccessors.
        /// </summary>
        /// <param name="nonPublic">The nonPublic<see cref="bool"/>.</param>
        /// <returns>The <see cref="MethodInfo[]"/>.</returns>
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            if (RubricInfo != null)
                return RubricInfo.GetAccessors(nonPublic);
            return null;
        }

        /// <summary>
        /// The GetCustomAttributes.
        /// </summary>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public override object[] GetCustomAttributes(bool inherit)
        {
            if (RubricAttributes != null)
                return RubricAttributes;

            RubricAttributes = new object[0];
            if (RubricInfo != null)
            {
                var attrib = RubricInfo.GetCustomAttributes(inherit);
                if (attrib != null)
                {
                    if (RubricType.IsArray || RubricType == typeof(string))
                    {
                        var _attrib = attrib.Where(r => r is FigureAsAttribute).ToArray();
                        if (_attrib.Any())
                        {
                            _attrib.Cast<FigureAsAttribute>().Select(a => RubricSize = a.SizeConst).ToArray();
                            return RubricAttributes = attrib;
                        }
                        else
                            RubricAttributes.Concat(attrib).ToArray();
                    }
                    else
                        return RubricAttributes.Concat(attrib).ToArray();
                }
            }

            if (RubricType == typeof(string))
            {
                var _attrib = RubricAttributes.Where(r => r is MarshalAsAttribute).ToArray();
                if (_attrib.Any())
                {
                    _attrib.Cast<MarshalAsAttribute>().Select(a => RubricSize = a.SizeConst).ToArray();
                    return RubricAttributes;
                }

                return new[] { new MarshalAsAttribute(UnmanagedType.ByValTStr) { SizeConst = RubricSize } };
            }
            else if (RubricType.IsArray)
            {
                if (RubricType == typeof(byte[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize, ArraySubType = UnmanagedType.U1 } }).ToArray();
                if (RubricType == typeof(char[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize, ArraySubType = UnmanagedType.U1 } }).ToArray();
                if (RubricType == typeof(int[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 4, ArraySubType = UnmanagedType.I4 } }).ToArray();
                if (RubricType == typeof(long[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 8, ArraySubType = UnmanagedType.I8 } }).ToArray();
                if (RubricType == typeof(float[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 4, ArraySubType = UnmanagedType.R4 } }).ToArray();
                if (RubricType == typeof(double[]))
                    return RubricAttributes.Concat(new[] { new MarshalAsAttribute(UnmanagedType.ByValArray) { SizeConst = RubricSize / 8, ArraySubType = UnmanagedType.R8 } }).ToArray();
            }
            return null;
        }

        /// <summary>
        /// The GetCustomAttributes.
        /// </summary>
        /// <param name="attributeType">The attributeType<see cref="Type"/>.</param>
        /// <param name="inherit">The inherit<see cref="bool"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            var attribs = this.GetCustomAttributes(inherit);
            if (attribs != null)
                attribs = attribs.Where(r => r.GetType() == attributeType).ToArray();
            if (!attribs.Any())
                return null;
            return attribs;
        }

        /// <summary>
        /// The GetGetMethod.
        /// </summary>
        /// <param name="nonPublic">The nonPublic<see cref="bool"/>.</param>
        /// <returns>The <see cref="MethodInfo"/>.</returns>
        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            if (RubricInfo != null)
                return RubricInfo.GetGetMethod(nonPublic);
            return null;
        }

        /// <summary>
        /// The GetIndexParameters.
        /// </summary>
        /// <returns>The <see cref="ParameterInfo[]"/>.</returns>
        public override ParameterInfo[] GetIndexParameters()
        {
            if (RubricInfo != null)
                return RubricInfo.GetIndexParameters();
            return null;
        }

        /// <summary>
        /// The GetSetMethod.
        /// </summary>
        /// <param name="nonPublic">The nonPublic<see cref="bool"/>.</param>
        /// <returns>The <see cref="MethodInfo"/>.</returns>
        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            if (RubricInfo != null)
                return RubricInfo.GetSetMethod(nonPublic);
            return null;
        }

        /// <summary>
        /// The GetValue.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="invokeAttr">The invokeAttr<see cref="BindingFlags"/>.</param>
        /// <param name="binder">The binder<see cref="Binder"/>.</param>
        /// <param name="index">The index<see cref="object[]"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            if (RubricId < 0)
                return ((IFigure)obj)[RubricName];
            return ((IFigure)obj)[RubricId];
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

        /// <summary>
        /// The SetValue.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="invokeAttr">The invokeAttr<see cref="BindingFlags"/>.</param>
        /// <param name="binder">The binder<see cref="Binder"/>.</param>
        /// <param name="index">The index<see cref="object[]"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            if (RubricId < 0)
                ((IFigure)obj)[RubricName] = value;
            ((IFigure)obj)[RubricId] = value;
        }

        #endregion
    }
}
