/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.JsonAttributes.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System;

    #region Enums

    public enum JsonModes
    {
        All,
        KeyValue,
        Array
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="JsonArrayAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonArrayAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArrayAttribute"/> class.
        /// </summary>
        public JsonArrayAttribute()
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="JsonIgnoreAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonIgnoreAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonIgnoreAttribute"/> class.
        /// </summary>
        public JsonIgnoreAttribute()
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="JsonMemberAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonMemberAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMemberAttribute"/> class.
        /// </summary>
        public JsonMemberAttribute()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SerialMode.
        /// </summary>
        public JsonModes SerialMode { get; set; } = JsonModes.All;

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="JsonObjectAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonObjectAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObjectAttribute"/> class.
        /// </summary>
        public JsonObjectAttribute()
        {
        }

        #endregion
    }
}
