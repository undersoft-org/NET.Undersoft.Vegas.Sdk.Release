using System.Linq;

namespace System.Instant
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class JsonAttribute : Attribute
    {
        public JsonAttribute()
        { }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonIgnoreAttribute : JsonAttribute
    {
        public JsonIgnoreAttribute()
        { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonMemberAttribute : JsonAttribute
    {
        public JsonModes SerialMode { get; set; } = JsonModes.All;

        public JsonMemberAttribute()
        { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonArrayAttribute : JsonAttribute
    {
        public JsonArrayAttribute()
        { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonObjectAttribute : JsonAttribute
    {
        public JsonObjectAttribute()
        { }
    }

    public enum JsonModes
    {
        All,
        KeyValue,
        Array        
    }

}
