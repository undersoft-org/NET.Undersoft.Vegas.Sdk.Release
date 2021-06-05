/*************************************************
   Copyright (c) 2021 Undersoft

   System.Deal.JsonParser.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Deal
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Instant;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Uniques;

    #region Enums

    public enum JsonToken
    {
        Unknown,
        LeftBrace,
        RightBrace,
        Colon,
        Comma,
        LeftBracket,
        RightBracket,
        String,
        Number,
        True,
        False,
        Null
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="IJson" />.
    /// </summary>
    public interface IJson
    {
    }
    ///Possible JSON tokens in parsed input.
    /// <summary>
    /// Defines the <see cref="InvalidJsonException" />.
    /// </summary>
    public class InvalidJsonException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidJsonException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public InvalidJsonException(string message)
            : base(message)
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="JsonParser" />.
    /// </summary>
    public class JsonParser
    {
        #region Fields

        private static readonly char[] _base16 = new[]
                             {
                                 '0', '1', '2', '3',
                                 '4', '5', '6', '7',
                                 '8', '9', 'A', 'B',
                                 'C', 'D', 'E', 'F'
                             };
        private static readonly
            IDictionary<string,
                IDictionary<Type,
                    PropertyInfo[]>> _cache;
        private static readonly string[][] _unichar = new string[][]
                             {
                               new string[] { new string('"', 1),  @"\""", },
                               new string[] { new string('/', 1),  @"\/", },
                               new string[] { new string('\\', 1), @"\\", },
                               new string[] { new string('\b', 1), @"\b", },
                               new string[] { new string('\f', 1), @"\f", },
                               new string[] { new string('\n', 1), @"\n", },
                               new string[] { new string('\r', 1), @"\r", },
                               new string[] { new string('\t', 1), @"\t", }
                             };
        private static NumberStyles _numbers = NumberStyles.Float;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="JsonParser"/> class.
        /// </summary>
        static JsonParser()
        {
            _cache = new Dictionary<string, IDictionary<Type, PropertyInfo[]>>();
            foreach (string cmplx in Enum.GetNames(typeof(DealComplexity)))
                _cache.Add(cmplx, new Dictionary<Type, PropertyInfo[]>());
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Deserialize.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="json">The json<see cref="string"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public static T Deserialize<T>(string json)
        {
            T instance;
            var map = PrepareInstance(out instance);
            var bag = FromJson(json);

            DeserializeImpl(map, bag, instance);
            return instance;
        }

        /// <summary>
        /// The Deserialize.
        /// </summary>
        /// <param name="json">The json<see cref="string"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object Deserialize(string json, Type type)
        {
            object instance;
            var map = PrepareInstance(out instance, type);
            var bag = FromJson(json);

            DeserializeImpl(map, bag, instance);
            return instance;
        }

        /// <summary>
        /// The DeserializeType.
        /// </summary>
        /// <param name="map">The map<see cref="IEnumerable{PropertyInfo}"/>.</param>
        /// <param name="bag">The bag<see cref="IDictionary{string, object}"/>.</param>
        /// <param name="instance">The instance<see cref="object"/>.</param>
        public static void DeserializeType(IEnumerable<PropertyInfo> map, IDictionary<string, object> bag, object instance)
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            foreach (PropertyInfo info in map)
            {
                bool mutated = false;
                string key = info.Name;
                if (!bag.ContainsKey(key))
                {
                    key = info.Name.Replace("_", "");
                    if (!bag.ContainsKey(key))
                    {
                        key = info.Name.Replace("-", "");
                        if (!bag.ContainsKey(key))
                            continue;
                    }
                }

                object value = bag[key];

                if (value != null && value.GetType() == typeof(String))
                    if (value.Equals("null"))
                        value = null;

                if (value != null)
                {
                    if (info.PropertyType == typeof(byte[]))
                        if (value.GetType() == typeof(List<object>)) value = ((List<object>)value).Select(symbol => Convert.ToByte(symbol)).ToArray();
                        else value = ((object[])value).Select(symbol => Convert.ToByte(symbol)).ToArray();
                    else if (info.PropertyType == typeof(Ussn)) value = new Ussn(value.ToString());
                    else if (info.PropertyType == typeof(Single)) value = Convert.ToSingle(value, nfi);
                    else if (info.PropertyType == typeof(DateTime)) value = Convert.ToDateTime(value);
                    else if (info.PropertyType == typeof(double)) value = Convert.ToDouble(value, nfi);
                    else if (info.PropertyType == typeof(decimal)) value = Convert.ToDecimal(value, nfi);
                    else if (info.PropertyType == typeof(int)) value = Convert.ToInt32(value);
                    else if (info.PropertyType == typeof(long)) value = Convert.ToInt64(value);
                    else if (info.PropertyType == typeof(short)) value = Convert.ToInt16(value);
                    else if (info.PropertyType == typeof(bool)) value = Convert.ToBoolean(value);

                    else if (info.PropertyType == typeof(IFigure))
                    {
                        object n = info.GetValue(instance, null);
                        DeserializeType(n.GetType().GetProperties(),
                                        (Dictionary<string, object>)value, n);
                        mutated = true;
                    }
                    else if (info.PropertyType == typeof(DealContext))
                    {
                        object inst = new object();
                        Dictionary<string, object> subbag = (Dictionary<string, object>)value;
                        IEnumerable<PropertyInfo> submap = PrepareInstance(out inst, typeof(DealContext));
                        DeserializeType(submap, subbag, inst);
                        info.SetValue(instance, inst, null);
                        mutated = true;
                    }
                    else if (info.PropertyType == typeof(MemberIdentity))
                    {
                        object inst = new object();
                        Dictionary<string, object> subbag = (Dictionary<string, object>)value;
                        IEnumerable<PropertyInfo> submap = PrepareInstance(out inst, typeof(MemberIdentity));
                        DeserializeType(submap, subbag, inst);
                        info.SetValue(instance, inst, null);
                        mutated = true;
                    }
                    else if (info.PropertyType == typeof(Type))
                    {
                        object typevalue = info.GetValue(instance, null);
                        if (value != null)
                            typevalue = Type.GetType(value.ToString());
                        value = typevalue;
                    }
                    else if (info.PropertyType.IsEnum)
                    {
                        object enumvalue = info.GetValue(instance, null);
                        enumvalue = Enum.Parse(info.PropertyType, value.ToString());
                        value = enumvalue;
                    }
                }

                if (!mutated)
                    info.SetValue(instance, value, null);
            }
        }

        /// <summary>
        /// The FromJson.
        /// </summary>
        /// <param name="json">The json<see cref="string"/>.</param>
        /// <returns>The <see cref="IDictionary{string, object}"/>.</returns>
        public static IDictionary<string, object> FromJson(string json)
        {

            JsonToken type;

            var result = FromJson(json, out type);

            switch (type)
            {
                case JsonToken.LeftBrace:
                    var @object = (IDictionary<string, object>)result.Single().Value;
                    return @object;
            }

            return result;
        }

        /// <summary>
        /// The FromJson.
        /// </summary>
        /// <param name="json">The json<see cref="string"/>.</param>
        /// <param name="type">The type<see cref="JsonToken"/>.</param>
        /// <returns>The <see cref="IDictionary{string, object}"/>.</returns>
        public static IDictionary<string, object> FromJson(string json, out JsonToken type)
        {

            var data = json.ToCharArray();
            var index = 0;

            // Rewind index for first token
            var token = NextToken(data, ref index);
            switch (token)
            {
                case JsonToken.LeftBrace:   // Start Object
                case JsonToken.LeftBracket: // Start Array
                    index--;
                    type = token;
                    break;
                default:
                    throw new InvalidJsonException("JSON must begin with an object or array");
            }

            return ParseObject(data, ref index);
        }

        /// <summary>
        /// The PrepareInstance.
        /// </summary>
        /// <param name="instance">The instance<see cref="object"/>.</param>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="IEnumerable{PropertyInfo}"/>.</returns>
        public static IEnumerable<PropertyInfo> PrepareInstance(out object instance, Type type)
        {
            instance = Activator.CreateInstance(type);

            CacheReflection(type);

            return _cache["Standard"][type];
        }

        /// <summary>
        /// The PrepareInstance.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="instance">The instance<see cref="T"/>.</param>
        /// <returns>The <see cref="IEnumerable{PropertyInfo}"/>.</returns>
        public static IEnumerable<PropertyInfo> PrepareInstance<T>(out T instance)
        {
            instance = Activator.CreateInstance<T>();
            Type item = typeof(T);

            CacheReflection(item);

            return _cache["Standard"][item];
        }

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <param name="instance">The instance<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object Serialize(object instance)
        {
            Type type = instance.GetType();
            IDictionary<string, object> bag = GetBagForObject(type, instance);

            return ToJson(bag);
        }

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="instance">The instance<see cref="T"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Serialize<T>(T instance)
        {
            IDictionary<string, object> bag = GetBagForObject(instance);

            return ToJson(bag);
        }

        /// <summary>
        /// The ToJson.
        /// </summary>
        /// <param name="bag">The bag<see cref="IDictionary{int, object}"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToJson(IDictionary<int, object> bag, DealComplexity complexity = DealComplexity.Standard)
        {

            var sb = new StringBuilder(0);

            SerializeItem(sb, bag, null, complexity);

            return sb.ToString();
        }

        /// <summary>
        /// The ToJson.
        /// </summary>
        /// <param name="bag">The bag<see cref="IDictionary{string, object}"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToJson(IDictionary<string, object> bag, DealComplexity complexity = DealComplexity.Standard)
        {
            var sb = new StringBuilder(4096);

            SerializeItem(sb, bag, null, complexity);

            return sb.ToString();
        }

        /// <summary>
        /// The BaseConvert.
        /// </summary>
        /// <param name="input">The input<see cref="int"/>.</param>
        /// <param name="charSet">The charSet<see cref="char[]"/>.</param>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string BaseConvert(int input, char[] charSet, int minLength)
        {
            var sb = new StringBuilder();
            var @base = charSet.Length;

            while (input > 0)
            {
                var index = input % @base;
                sb.Insert(0, new[] { charSet[index] });
                input = input / @base;
            }

            while (sb.Length < minLength)
            {
                sb.Insert(0, "0");
            }

            return sb.ToString();
        }

        /// <summary>
        /// The CacheReflection.
        /// </summary>
        /// <param name="item">The item<see cref="Type"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        internal static void CacheReflection(Type item, DealComplexity complexity = DealComplexity.Standard)
        {
            if (_cache[complexity.ToString()].ContainsKey(item))
                return;

            PropertyInfo[] verified = new PropertyInfo[0];
            PropertyInfo[] prepare = item.GetJsonProperties(complexity);
            if (prepare != null)
                verified = prepare;

            _cache[complexity.ToString()].Add(item, verified);
        }

        /// <summary>
        /// The GetBagForObject.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="instance">The instance<see cref="T"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        /// <returns>The <see cref="IDictionary{string, object}"/>.</returns>
        internal static IDictionary<string, object> GetBagForObject<T>(T instance, DealComplexity complexity = DealComplexity.Standard)
        {
            return GetBagForObject(typeof(T), instance, complexity);
        }

        /// <summary>
        /// The GetBagForObject.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="instance">The instance<see cref="object"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        /// <returns>The <see cref="IDictionary{string, object}"/>.</returns>
        internal static IDictionary<string, object> GetBagForObject(Type type, object instance, DealComplexity complexity = DealComplexity.Standard)
        {
            CacheReflection(type, complexity);

            if (type.FullName == null)
            {
                return null;
            }

            bool anonymous = type.FullName.Contains("__AnonymousType");
            PropertyInfo[] map = _cache[complexity.ToString()][type];

            IDictionary<string, object> bag = InitializeBag();
            foreach (PropertyInfo info in map)
            {
                if (info != null)
                {
                    var readWrite = (info.CanWrite && info.CanRead);
                    if (!readWrite && !anonymous)
                    {
                        continue;
                    }
                    object value = null;
                    try
                    {
                        value = info.GetValue(instance, null);
                    }
                    catch (Exception ex)
                    {
                        //ex.();
                    }
                    bag.Add(info.Name, value);
                }
            }

            return bag;
        }

        /// <summary>
        /// The GetKeyword.
        /// </summary>
        /// <param name="word">The word<see cref="string"/>.</param>
        /// <param name="target">The target<see cref="JsonToken"/>.</param>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="result">The result<see cref="JsonToken"/>.</param>
        internal static void GetKeyword(string word, JsonToken target, IList<char> data, ref int index, ref JsonToken result)
        {
            int buffer = data.Count - index;
            if (buffer < word.Length)
            {
                return;
            }

            for (var i = 0; i < word.Length; i++)
            {
                if (data[index + i] != word[i])
                {
                    return;
                }
            }

            result = target;
            index += word.Length;
        }

        /// <summary>
        /// The GetTokenFromSymbol.
        /// </summary>
        /// <param name="symbol">The symbol<see cref="char"/>.</param>
        /// <returns>The <see cref="JsonToken"/>.</returns>
        internal static JsonToken GetTokenFromSymbol(char symbol)
        {
            return GetTokenFromSymbol(symbol, JsonToken.Unknown);
        }

        /// <summary>
        /// The GetTokenFromSymbol.
        /// </summary>
        /// <param name="symbol">The symbol<see cref="char"/>.</param>
        /// <param name="token">The token<see cref="JsonToken"/>.</param>
        /// <returns>The <see cref="JsonToken"/>.</returns>
        internal static JsonToken GetTokenFromSymbol(char symbol, JsonToken token)
        {
            switch (symbol)
            {
                case '{':
                    token = JsonToken.LeftBrace;
                    break;
                case '}':
                    token = JsonToken.RightBrace;
                    break;
                case ':':
                    token = JsonToken.Colon;
                    break;
                case ',':
                    token = JsonToken.Comma;
                    break;
                case '[':
                    token = JsonToken.LeftBracket;
                    break;
                case ']':
                    token = JsonToken.RightBracket;
                    break;
                case '"':
                    token = JsonToken.String;
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                case 'e':
                case 'E':
                case '+':
                case '-':
                    token = JsonToken.Number;
                    break;
            }
            return token;
        }

        /// <summary>
        /// The GetUnicode.
        /// </summary>
        /// <param name="character">The character<see cref="char"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetUnicode(char character)
        {
            switch (character)
            {
                case '"': return @"\""";
                case '/': return @"\/";
                case '\\': return @"\\";
                case '\b': return @"\b";
                case '\f': return @"\f";
                case '\n': return @"\n";
                case '\r': return @"\r";
                case '\t': return @"\t";
            }
            return new string(character, 1);
        }

        /// <summary>
        /// The IgnoreWhitespace.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="symbol">The symbol<see cref="char"/>.</param>
        internal static void IgnoreWhitespace(IList<char> data, ref int index, char symbol)
        {
            var token = JsonToken.Unknown;
            IgnoreWhitespace(data, ref index, ref token, symbol);
            return;
        }

        /// <summary>
        /// The IgnoreWhitespace.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="token">The token<see cref="JsonToken"/>.</param>
        /// <param name="symbol">The symbol<see cref="char"/>.</param>
        /// <returns>The <see cref="JsonToken"/>.</returns>
        internal static JsonToken IgnoreWhitespace(IList<char> data, ref int index, ref JsonToken token, char symbol)
        {
            switch (symbol)
            {
                case ' ':
                case '\\':
                case '/':
                case '\b':
                case '\f':
                case '\n':
                case '\r':
                case '\t':
                    index++;
                    token = NextToken(data, ref index);
                    break;
            }
            return token;
        }

        /// <summary>
        /// The InitializeBag.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, object}"/>.</returns>
        internal static Dictionary<string, object> InitializeBag()
        {
            return new Dictionary<string, object>(
                0, StringComparer.OrdinalIgnoreCase
                );
        }

        /// <summary>
        /// The NextToken.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="JsonToken"/>.</returns>
        internal static JsonToken NextToken(IList<char> data, ref int index)
        {

            var symbol = data[index];
            var token = GetTokenFromSymbol(symbol);
            token = IgnoreWhitespace(data, ref index, ref token, symbol);

            GetKeyword("true", JsonToken.True, data, ref index, ref token);
            GetKeyword("false", JsonToken.False, data, ref index, ref token);
            GetKeyword("null", JsonToken.Null, data, ref index, ref token);

            return token;
        }

        /// <summary>
        /// The ParseArray.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="IEnumerable{object}"/>.</returns>
        internal static IEnumerable<object>
                               ParseArray(IList<char> data, ref int index)
        {
            var result = new List<object>();

            index++; // Skip first bracket
            while (index < data.Count - 1)
            {
                var token = NextToken(data, ref index);
                switch (token)
                {
                    // End Tokens
                    case JsonToken.Unknown:             // Bad Data
                        throw new InvalidJsonException(string.Format(
                            "Invalid JSON found while parsing an array at index {0}.", index
                            ));
                    case JsonToken.RightBracket:        // End Array
                        index++;
                        return result;
                    // Skip Tokens
                    case JsonToken.Comma:               // Separator
                    case JsonToken.RightBrace:          // End Object
                    case JsonToken.Colon:               // Separator
                        index++;
                        break;
                    // Value Tokens
                    case JsonToken.LeftBrace:           // Start Object
                        var nested = ParseObject(data, ref index);
                        result.Add(nested);
                        break;
                    case JsonToken.LeftBracket:         // Start Array
                    case JsonToken.String:
                    case JsonToken.Number:
                    case JsonToken.True:
                    case JsonToken.False:
                    case JsonToken.Null:
                        var value = ParseValue(data, ref index);
                        result.Add(value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }

        /// <summary>
        /// The ParseNumber.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        internal static object ParseNumber(IList<char> data, ref int index)
        {
            var symbol = data[index];
            IgnoreWhitespace(data, ref index, symbol);

            var start = index;
            var length = 0;
            while (ParseToken(JsonToken.Number, data, ref index))
            {
                length++;
                index++;
            }

            double result;
            var buffer = new string(data.Skip(start).Take(length).ToArray());
            if (!double.TryParse(buffer, _numbers, CultureInfo.InvariantCulture, out result))
            {
                throw new InvalidJsonException(
                    string.Format("Value '{0}' was not a valid JSON number", buffer)
                    );
            }
            return result;
        }

        /// <summary>
        /// The ParseObject.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="IDictionary{string, object}"/>.</returns>
        internal static IDictionary<string, object>
                               ParseObject(IList<char> data, ref int index)
        {
            var result = InitializeBag();

            index++; // Skip first token

            while (index < data.Count - 1)
            {
                var token = NextToken(data, ref index);
                switch (token)
                {
                    // End Tokens
                    case JsonToken.Unknown:             // Bad Data
                    case JsonToken.True:
                    case JsonToken.False:
                    case JsonToken.Null:
                    case JsonToken.Colon:
                    case JsonToken.RightBracket:
                    case JsonToken.Number:
                        throw new InvalidJsonException(string.Format(
                            "Invalid JSON found while parsing an object at index {0}.", index
                            ));
                    case JsonToken.RightBrace:          // End Object
                        index++;
                        return result;
                    // Skip Tokens
                    case JsonToken.Comma:
                        index++;
                        break;
                    // Start Tokens
                    case JsonToken.LeftBrace:           // Start Object
                        var @object = ParseObject(data, ref index);
                        if (@object != null)
                        {
                            result.Add(string.Concat("object", result.Count), @object);
                        }
                        index++;
                        break;
                    case JsonToken.LeftBracket:         // Start Array
                        var @array = ParseArray(data, ref index);
                        if (@array != null)
                        {
                            result.Add(string.Concat("array", result.Count), @array);
                        }
                        index++;
                        break;
                    case JsonToken.String:
                        var pair = ParsePair(data, ref index);
                        result.Add(pair.Key, pair.Value);
                        break;
                    default:
                        throw new NotSupportedException("Invalid token expected.");
                }
            }

            return result;
        }

        /// <summary>
        /// The ParsePair.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="KeyValuePair{string, object}"/>.</returns>
        internal static KeyValuePair<string, object>
                               ParsePair(IList<char> data, ref int index)
        {
            var valid = true;

            var name = ParseString(data, ref index);
            if (name == null)
            {
                valid = false;
            }

            if (!ParseToken(JsonToken.Colon, data, ref index))
            {
                valid = false;
            }

            if (!valid)
            {
                throw new InvalidJsonException(string.Format(
                            "Invalid JSON found while parsing a value pair at index {0}.", index
                            ));
            }

            index++;
            var value = ParseValue(data, ref index);
            return new KeyValuePair<string, object>(name, value);
        }

        /// <summary>
        /// The ParseString.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string ParseString(IList<char> data, ref int index)
        {
            var symbol = data[index];
            IgnoreWhitespace(data, ref index, symbol);
            symbol = data[++index]; // Skip first quotation

            var sb = new StringBuilder();
            while (true)
            {
                if (index >= data.Count - 1)
                {
                    return null;
                }
                switch (symbol)
                {
                    case '"':  // End String
                        index++;
                        return sb.ToString();
                    case '\\': // Control Character
                        symbol = data[++index];
                        switch (symbol)
                        {
                            case '/':
                            case '\\':
                            case '"':
                                sb.Append(symbol);
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case 'u': // Unicode literals
                                if (index < data.Count - 5)
                                {
                                    var array = data.ToArray();
                                    var buffer = new char[4];
                                    Array.Copy(array, index + 1, buffer, 0, 4);

                                    // http://msdn.microsoft.com/en-us/library/aa664669%28VS.71%29.aspx
                                    // http://www.yoda.arachsys.com/csharp/unicode.html
                                    // http://en.wikipedia.org/wiki/UTF-32/UCS-4
                                    var hex = new string(buffer);
                                    var unicode = (char)Convert.ToInt32(hex, 16);
                                    sb.Append(unicode);
                                    index += 4;
                                }
                                else
                                {
                                    break;
                                }
                                break;
                        }
                        break;
                    default:
                        sb.Append(symbol);
                        break;
                }
                symbol = data[++index];
            }
        }

        /// <summary>
        /// The ParseToken.
        /// </summary>
        /// <param name="token">The token<see cref="JsonToken"/>.</param>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool ParseToken(JsonToken token, IList<char> data, ref int index)
        {
            var nextToken = NextToken(data, ref index);
            return token == nextToken;
        }

        /// <summary>
        /// The ParseValue.
        /// </summary>
        /// <param name="data">The data<see cref="IList{char}"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        internal static object ParseValue(IList<char> data, ref int index)
        {

            var token = NextToken(data, ref index);
            switch (token)
            {
                // End Tokens
                case JsonToken.RightBracket:    // Bad Data
                case JsonToken.RightBrace:
                case JsonToken.Unknown:
                case JsonToken.Colon:
                case JsonToken.Comma:
                    throw new InvalidJsonException(string.Format(
                            "Invalid JSON found while parsing a value at index {0}.", index
                            ));
                // Value Tokens
                case JsonToken.LeftBrace:
                    return ParseObject(data, ref index);
                case JsonToken.LeftBracket:
                    return ParseArray(data, ref index);
                case JsonToken.String:
                    return ParseString(data, ref index);
                case JsonToken.Number:
                    return ParseNumber(data, ref index);
                case JsonToken.True:
                    return true;
                case JsonToken.False:
                    return false;
                case JsonToken.Null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// The SerializeArray.
        /// </summary>
        /// <param name="item">The item<see cref="object"/>.</param>
        /// <param name="sb">The sb<see cref="StringBuilder"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        internal static void SerializeArray(object item, StringBuilder sb, DealComplexity complexity = DealComplexity.Standard)
        {
            Type type = item.GetType();
            if (type.IsDefined(typeof(JsonObjectAttribute), false))
            {
                var bag = GetBagForObject(item.GetType(), item, complexity);
                SerializeItem(sb, bag, null, complexity);
            }
            else
            {
                ICollection array = (ICollection)item;

                sb.Append("[");
                var count = 0;

                var total = array.Cast<object>().Count();
                foreach (object element in array)
                {
                    SerializeItem(sb, element, null, complexity);
                    count++;
                    if (count < total)
                        sb.Append(",");
                }
                sb.Append("]");
            }
        }

        /// <summary>
        /// The SerializeDateTime.
        /// </summary>
        /// <param name="sb">The sb<see cref="StringBuilder"/>.</param>
        /// <param name="item">The item<see cref="object"/>.</param>
        internal static void SerializeDateTime(StringBuilder sb, object item = null)
        {
            sb.Append("\"" + ((DateTime)item).ToString("yyyy-MM-dd HH:mm:dd") + "\"");
        }

        /// <summary>
        /// The SerializeItem.
        /// </summary>
        /// <param name="sb">The sb<see cref="StringBuilder"/>.</param>
        /// <param name="item">The item<see cref="object"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        internal static void SerializeItem(StringBuilder sb, object item, string key = null, DealComplexity complexity = DealComplexity.Standard)
        {
            if (item == null)
            {
                sb.Append("null");
                return;
            }

            if (item is IDictionary)
            {
                SerializeObject(item, sb, false, complexity);
                return;
            }

            if (item is ICollection && !(item is string))
            {
                SerializeArray(item, sb, complexity);
                return;
            }

            if (item is Usid)
            {
                sb.Append("\"" + ((Usid)item).ToString() + "\"");
                return;
            }

            if (item is Ussn)
            {
                sb.Append("\"" + ((Ussn)item).ToString() + "\"");
                return;
            }

            if (item is DateTime)
            {
                sb.Append("\"" + ((DateTime)item).ToString("yyyy-MM-dd HH:mm:dd") + "\"");
                return;
            }

            if (item is Enum)
            {
                sb.Append("\"" + item.ToString() + "\"");
                return;
            }

            if (item is Type)
            {
                sb.Append("\"" + ((Type)item).FullName + "\"");
                return;
            }

            if (item is bool)
            {
                sb.Append(((bool)item).ToString().ToLower());
                return;
            }

            if (item is ValueType)
            {
                sb.Append(item.ToString().Replace(',', '.'));
                return;
            }

            IDictionary<string, object>
                bag = GetBagForObject(item.GetType(), item, complexity);

            SerializeItem(sb, bag, key, complexity);
        }

        /// <summary>
        /// The SerializeObject.
        /// </summary>
        /// <param name="item">The item<see cref="object"/>.</param>
        /// <param name="sb">The sb<see cref="StringBuilder"/>.</param>
        /// <param name="intAsKey">The intAsKey<see cref="bool"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        internal static void SerializeObject(object item, StringBuilder sb, bool intAsKey = false, DealComplexity complexity = DealComplexity.Standard)
        {
            sb.Append("{");

            IDictionary nested = (IDictionary)item;
            int i = 0;
            int count = nested.Count;
            foreach (DictionaryEntry entry in nested)
            {
                sb.Append("\"" + entry.Key + "\"");
                sb.Append(":");

                object value = entry.Value;
                if (value is string)
                {
                    SerializeString(sb, value);
                }
                else
                {
                    SerializeItem(sb, entry.Value, entry.Key.ToString(), complexity);
                }
                if (i < count - 1)
                {
                    sb.Append(",");
                }
                i++;
            }

            sb.Append("}");
        }

        /// <summary>
        /// The SerializeString.
        /// </summary>
        /// <param name="sb">The sb<see cref="StringBuilder"/>.</param>
        /// <param name="item">The item<see cref="object"/>.</param>
        internal static void SerializeString(StringBuilder sb, object item)
        {
            char[] symbols = ((string)item).ToCharArray();
            SerializeUnicode(sb, symbols);
        }

        /// <summary>
        /// The SerializeUnicode.
        /// </summary>
        /// <param name="sb">The sb<see cref="StringBuilder"/>.</param>
        /// <param name="symbols">The symbols<see cref="char[]"/>.</param>
        internal static void SerializeUnicode(StringBuilder sb, char[] symbols)
        {
            sb.Append("\"");

            string[] unicodes = symbols.Select(symbol => GetUnicode(symbol)).ToArray();

            foreach (var unicode in unicodes)
                sb.Append(unicode);

            sb.Append("\"");
        }

        /// <summary>
        /// The DeserializeImpl.
        /// </summary>
        /// <param name="map">The map<see cref="IEnumerable{PropertyInfo}"/>.</param>
        /// <param name="bag">The bag<see cref="IDictionary{string, object}"/>.</param>
        /// <param name="instance">The instance<see cref="object"/>.</param>
        private static void DeserializeImpl(IEnumerable<PropertyInfo> map, IDictionary<string, object> bag, object instance)
        {
            DeserializeType(map, bag, instance);
        }

        /// <summary>
        /// The DeserializeImpl.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="map">The map<see cref="IEnumerable{PropertyInfo}"/>.</param>
        /// <param name="bag">The bag<see cref="IDictionary{string, object}"/>.</param>
        /// <param name="instance">The instance<see cref="T"/>.</param>
        private static void DeserializeImpl<T>(IEnumerable<PropertyInfo> map, IDictionary<string, object> bag, T instance)
        {
            DeserializeType(map, bag, instance);
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="JsonParserProperties" />.
    /// </summary>
    public static class JsonParserProperties
    {
        #region Fields

        public static Dictionary<string, string[]> Deck = new Dictionary<string, string[]>()
        {
               {
                "System.Deal.DealHeader",
                new string[]
                {
                    "Context",   "Content"
                }
             },
            {
                "System.Deal.DealMessage",
                new string[]
                {
                    "Notice",   "Content"
                }
             },
            {
                "System.Deal.DealContext",
                new string[]
                {
                    "Identity",   "ContentType",  "Complexity",
                    "Echo"
                }
             },
             {
                "System.Deal.DealIdentity_Advanced",
                new string[]
                {
                    "Id",     "Name",   "Key",  "Token", "UserId",
                    "DeptId", "Ip", "DataPlace"
                }
             },
              {
                "System.DealIdentity_Basic",
                new string[]
                {
                    "UserId", "DeptId", "Token", "DataPlace",
                }
             },
               {
                "System.Core.NetdealIdentity",
                new string[]
                {
                    "Name",   "Key",  "Token", "UserId",
                    "DeptId", "DataPlace",
                }
             },
                {
                "System.Core.NetdealIdentity_Guide",
                new string[]
                {
                    "Token", "UserId", "DeptId", "DataPlace"
                }
             },
            {
                "System.FigureAs.DataArea",
                new string[]
                {
                    "SpaceName",   "DisplayName",  "Config",  "State",
                    "Areas"
                }
             },
              {
                "System.FigureAs.DataArea_Guide",
                new string[]
                {
                    "SpaceName", "Areas", "Config"
                }
             },
             {
                "System.FigureAs.LogicCatalogs",
                new string[]
                {
                    "CatalogsId",   "DisplayName",  "Config",  "State",
                    "Catalogs",     "CatalogsIn"
                }
             },
              {
                "System.FigureAs.LogicCatalogs_Guide",
                new string[]
                {
                    "CatalogsId", "Catalogs", "CatalogsIn", "Config"
                }
             },
             {
                "System.FigureAs.LogicCatalog",
                new string[]
                {
                    "CatalogId",   "DisplayName",  "Config",     "State",
                    "Relations",     "Deckises",   "CatalogsIn"
                }
            },
                {
                "System.FigureAs.LogicCatalog_Guide",
                new string[]
                {
                    "CatalogId", "Deckises", "CatalogsIn", "Config"
                }
            },
               {
                "System.FigureAs.Deck64",
                new string[]
                {
                    "DeckName",  "DisplayName", "Mode",       "Config",  "State",
                    "Checked",    "Edited",      "Synced",     "Saved",
                    "Quered",     "CountView",   "Count",        "PagingDetails",
                    "Rubrics",     "PrimeKey",    "EditLevel",    "SimLevel",
                    "Filter",     "Sort",        "Favorites",    "Relationing",
                    "CardsTotal", "SimsTotal"
                }
            },
            {
                "System.FigureAs.Deck_Advanced",
                new string[]
                {
                    "DeckName",     "DisplayName",  "Visible",       "Mode",
                    "Checked",       "Edited",       "Synced",        "Saved",
                    "Quered",        "CountView",    "Count",        "Config",
                    "IsPrime",       "State",        "Rubrics",       "PrimeKey",
                    "EditLevel",     "SimLevel",     "Filter",       "Sort",
                    "PagingDetails", "Favorites",    "Relationing",     "CardsTotal",
                    "SimsTotal",     "EditLength",   "SimLength",    "MappingName",
                    "Relations"
                }
            },
             {
                "System.FigureAs.Deck_Basic",
                new string[]
                {
                    "DeckName",    "DisplayName",   "CountView",   "Config",
                    "State",        "Checked",      "Edited",        "Synced",
                     "Saved",       "Quered",       "EditLevel",     "SimLevel",
                     "Mode",        "Filter",      "Sort",          "PagingDetails",
                    "Favorites",    "CardsTotal", "SimsTotal"
                }
            },
               {
                "System.FigureAs.Deck_Guide",
                new string[]
                {
                    "DeckName", "Config"
                }
            },
            {
                "System.FigureAs.Card",
                new string[]
                {
                    "Checked",   "Index",      "Page",     "PageIndex",
                    "ViewIndex", "NoId",       "Edited",   "Deleted",
                    "Added",     "Synced",     "Saved",
                    "Fields",    "ChildJoins", "ParentJoins"
                }
            },
             {
                "System.FigureAs.DataField",
                new string[]
                {
                    "Value"
                }
            },
            {
                "System.FigureAs.VirtualRubric",
                new string[]
                {
                    "RubricId",     "Ordinal",           "Visible",
                    "RubricName",   "DisplayName",       "DataType",
                    "Default",     "isKey",             "Editable",
                    "Revalue",     "RevalOperand",      "RevalType",
                    "TotalOperand"
                }
            },

             {
                "System.FigureAs.FilterTerm",
                new string[]
                {
                    "Index",     "RubricName",    "Operand",
                    "Value",     "Logic",        "Stage",
                }
            },
               {
                "System.FigureAs.SortTerm",
                new string[]
                {
                    "Index",     "RubricName",    "Direction"
                }
            },
                 {
                "System.Core.Settings",
                new string[]
                {
                    "Place",  "DataIdx",  "Path", "NetdealIdx", "DataType"
                }
            },
                     {
                "System.Core.Settings_Guide",
                new string[]
                {
                    "Place",  "DataIdx", "DataType"
                }
            },
                    {
                "System.Core.Settings_Basic",
                new string[]
                {
                    "Place", "DataIdx", "NetdealIdx", "DataType"
                }
            },
              {
                "System.Core.Settings_Advanced",
                new string[]
                {
                    "Place", "DataIdx", "Path", "Disk", "File", "NetdealIdx", "DataType"
                }
            },
                   {
                "System.Core.StructState",
                new string[]
                {
                    "Edited",     "Deleted",
                    "Added",      "Synced",     "Canceled",
                    "Saved",      "Quered",
                }
            },
            {
                "System.FigureAs.PageDetails",
                new string[]
                {
                    "Page",        "PageActive",  "CachedPages",
                    "PageCount",   "PageSize",
                }
            },
             {
                "System.FigureAs.Relatings",
                new string[]
                {
                    "ChildRelations",  "ParentRelations"
                }
            },
              {
                "System.FigureAs.Relation",
                new string[]
                {
                    "RelationName",  "Parent",  "Child",
                }
            },
            {
                "System.FigureAs.RelationMember",
                new string[]
                {
                    "DeckName",  "RelationKeys"
                }
            }
        };

        #endregion

        #region Methods

        /// <summary>
        /// The GetJsonProperties.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        /// <returns>The <see cref="PropertyInfo[]"/>.</returns>
        public static PropertyInfo[] GetJsonProperties<T>(DealComplexity complexity = DealComplexity.Standard)
        {
            Type type = typeof(T);
            string name = type.FullName;
            string cname = "";
            if (complexity != DealComplexity.Standard)
                cname = name + "_" + complexity.ToString();
            else
                cname = name;

            if (Deck.ContainsKey(cname))
                return Deck[cname].Select(t => type.GetProperty(t)).ToArray();
            else if (Deck.ContainsKey(name))
                return Deck[name].Select(t => type.GetProperty(t)).ToArray();
            else
                return null;
        }

        /// <summary>
        /// The GetJsonProperties.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="complexity">The complexity<see cref="DealComplexity"/>.</param>
        /// <returns>The <see cref="PropertyInfo[]"/>.</returns>
        public static PropertyInfo[] GetJsonProperties(this Type type, DealComplexity complexity = DealComplexity.Standard)
        {
            string name = type.FullName;
            string cname = "";
            if (complexity != DealComplexity.Standard)
                cname = name + "_" + complexity.ToString();
            else
                cname = name;

            if (Deck.ContainsKey(cname))
                return Deck[cname].Select(t => type.GetProperty(t)).ToArray();
            else if (Deck.ContainsKey(name))
                return Deck[name].Select(t => type.GetProperty(t)).ToArray();
            else
                return null;
        }

        /// <summary>
        /// The JsonNumberInfo.
        /// </summary>
        /// <returns>The <see cref="NumberFormatInfo"/>.</returns>
        public static NumberFormatInfo JsonNumberInfo()
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return nfi;
        }

        #endregion
    }
}
