/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FieldRubric.cs
   
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
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="FieldRubric" />.
    /// </summary>
    public class FieldRubric : FieldInfo, IMemberRubric
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRubric"/> class.
        /// </summary>
        public FieldRubric()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRubric"/> class.
        /// </summary>
        /// <param name="field">The field<see cref="FieldInfo"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="fieldId">The fieldId<see cref="int"/>.</param>
        public FieldRubric(FieldInfo field, int size = -1, int fieldId = -1) : this(field.FieldType, field.Name, size, fieldId)
        {
            if (field.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) != null)
            {
                string name = field.Name;
                int end = name.LastIndexOf('>'), start = (name.IndexOf('<') + 1), length = end - start;
                RubricName = new String(field.Name.ToCharArray(start, length));
                IsBackingField = true;
            }
            RubricInfo = field;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRubric"/> class.
        /// </summary>
        /// <param name="fieldType">The fieldType<see cref="Type"/>.</param>
        /// <param name="fieldName">The fieldName<see cref="string"/>.</param>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="fieldId">The fieldId<see cref="int"/>.</param>
        public FieldRubric(Type fieldType, string fieldName, int size = -1, int fieldId = -1)
        {
            RubricType = fieldType;
            RubricName = fieldName;
            FieldName = fieldName;
            RubricId = fieldId;
            if (size > 0)
                RubricSize = size;
            else
            {
                if (fieldType.IsValueType)
                {
                    if (fieldType == typeof(DateTime))
                        RubricSize = 8;
                    else if (fieldType == typeof(Enum))
                        RubricSize = 4;
                    else
                        RubricSize = Marshal.SizeOf(fieldType);
                }
                else
                {
                    RubricSize = Marshal.SizeOf(typeof(IntPtr));
                }
                if (size > 0)
                    RubricSize = size;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Attributes.
        /// </summary>
        public override FieldAttributes Attributes => RubricInfo != null ? RubricInfo.Attributes : FieldAttributes.HasDefault;

        /// <summary>
        /// Gets the DeclaringType.
        /// </summary>
        public override Type DeclaringType => RubricInfo != null ? RubricInfo.DeclaringType : null;

        /// <summary>
        /// Gets or sets a value indicating whether Editable.
        /// </summary>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// Gets the FieldHandle.
        /// </summary>
        public override RuntimeFieldHandle FieldHandle => RubricInfo != null ? RubricInfo.FieldHandle : throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the FieldName.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets the FieldType.
        /// </summary>
        public override Type FieldType => RubricType;

        /// <summary>
        /// Gets or sets a value indicating whether IsBackingField.
        /// </summary>
        public bool IsBackingField { get; set; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public override string Name => FieldName;

        /// <summary>
        /// Gets the ReflectedType.
        /// </summary>
        public override Type ReflectedType => RubricInfo != null ? RubricInfo.ReflectedType : null;

        /// <summary>
        /// Gets or sets the RubricAttributes.
        /// </summary>
        public object[] RubricAttributes { get; set; }

        /// <summary>
        /// Gets or sets the RubricId.
        /// </summary>
        public int RubricId { get; set; }

        /// <summary>
        /// Gets or sets the RubricInfo.
        /// </summary>
        public FieldInfo RubricInfo { get; set; }

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
        public int RubricOffset { get; set; }

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
        public int RubricSize { get; set; }

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
                        if (attrib.Where(r => r is MarshalAsAttribute).Any())
                        {
                            attrib.Where(r => r is MarshalAsAttribute).Cast<MarshalAsAttribute>().Select(a => RubricSize = a.SizeConst).ToArray();
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
                if (RubricSize < 1)
                    RubricSize = 16;
                return new[] { new MarshalAsAttribute(UnmanagedType.ByValTStr) { SizeConst = RubricSize } };
            }
            else if (RubricType.IsArray)
            {
                if (RubricSize < 1)
                    RubricSize = 8;

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
            return attribs;
        }

        /// <summary>
        /// The GetValue.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public override object GetValue(object obj)
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
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            if (RubricId < 0)
                ((IFigure)obj)[RubricName] = value;
            ((IFigure)obj)[RubricId] = value;
        }

        #endregion
    }
}
