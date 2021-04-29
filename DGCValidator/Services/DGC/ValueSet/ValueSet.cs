namespace DGCValidator.Services.DGC.ValueSet
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ValueSet
    {
        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("lang")]
        public Lang Lang { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("system")]
        public Uri System { get; set; }

        [JsonProperty("version")]
        public VersionUnion Version { get; set; }

        [JsonProperty("valueSetId")]
        public string ValueSetId { get; set; }
    }

    public enum Lang { En };

    public enum VersionEnum { Empty, The10, The202101, The269 };

    public partial struct VersionUnion
    {
        public VersionEnum? Enum;
        public Uri PurpleUri;

        public static implicit operator VersionUnion(VersionEnum Enum) => new VersionUnion { Enum = Enum };
        public static implicit operator VersionUnion(Uri PurpleUri) => new VersionUnion { PurpleUri = PurpleUri };
    }

    public partial class ValueSet
    {
        public static Dictionary<string, ValueSet> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, ValueSet>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, ValueSet> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                LangConverter.Singleton,
                VersionUnionConverter.Singleton,
                VersionEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class LangConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Lang) || t == typeof(Lang?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "en")
            {
                return Lang.En;
            }
            throw new Exception("Cannot unmarshal type Lang");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Lang)untypedValue;
            if (value == Lang.En)
            {
                serializer.Serialize(writer, "en");
                return;
            }
            throw new Exception("Cannot marshal type Lang");
        }

        public static readonly LangConverter Singleton = new LangConverter();
    }

    internal class VersionUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(VersionUnion) || t == typeof(VersionUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    switch (stringValue)
                    {
                        case "":
                            return new VersionUnion { Enum = VersionEnum.Empty };
                        case "1.0":
                            return new VersionUnion { Enum = VersionEnum.The10 };
                        case "2.69":
                            return new VersionUnion { Enum = VersionEnum.The269 };
                        case "2021-01":
                            return new VersionUnion { Enum = VersionEnum.The202101 };
                    }
                    try
                    {
                        var uri = new Uri(stringValue);
                        return new VersionUnion { PurpleUri = uri };
                    }
                    catch (UriFormatException) {}
                    break;
            }
            throw new Exception("Cannot unmarshal type VersionUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (VersionUnion)untypedValue;
            if (value.Enum != null)
            {
                switch (value.Enum)
                {
                    case VersionEnum.Empty:
                        serializer.Serialize(writer, "");
                        return;
                    case VersionEnum.The10:
                        serializer.Serialize(writer, "1.0");
                        return;
                    case VersionEnum.The269:
                        serializer.Serialize(writer, "2.69");
                        return;
                    case VersionEnum.The202101:
                        serializer.Serialize(writer, "2021-01");
                        return;
                }
            }
            if (value.PurpleUri != null)
            {
                serializer.Serialize(writer, value.PurpleUri.ToString());
                return;
            }
            throw new Exception("Cannot marshal type VersionUnion");
        }

        public static readonly VersionUnionConverter Singleton = new VersionUnionConverter();
    }

    internal class VersionEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(VersionEnum) || t == typeof(VersionEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return VersionEnum.Empty;
                case "1.0":
                    return VersionEnum.The10;
                case "2.69":
                    return VersionEnum.The269;
                case "2021-01":
                    return VersionEnum.The202101;
            }
            throw new Exception("Cannot unmarshal type VersionEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (VersionEnum)untypedValue;
            switch (value)
            {
                case VersionEnum.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case VersionEnum.The10:
                    serializer.Serialize(writer, "1.0");
                    return;
                case VersionEnum.The269:
                    serializer.Serialize(writer, "2.69");
                    return;
                case VersionEnum.The202101:
                    serializer.Serialize(writer, "2021-01");
                    return;
            }
            throw new Exception("Cannot marshal type VersionEnum");
        }

        public static readonly VersionEnumConverter Singleton = new VersionEnumConverter();
    }
}
