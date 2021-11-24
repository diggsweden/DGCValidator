namespace DGCValidator.Services.Vaccinregler.ValueSet
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class VaccinRules
    {
        [JsonProperty("valueSetId")]
        public string ValueSetId { get; set; }

        [JsonProperty("valueSetDate")]
        public DateTimeOffset ValueSetDate { get; set; }

        [JsonProperty("validVaccines")]
        public Dictionary<string, ValidVaccineValue> ValidVaccines { get; set; }
    }

    public partial class VaccinRules
    {
        public static VaccinRules FromJson(string json) => JsonConvert.DeserializeObject<VaccinRules>(json, VaccinRulesConverter.Settings);
    }

    public static class VaccinRulesSerialize
    {
        public static string ToJson(this VaccinRules self) => JsonConvert.SerializeObject(self, VaccinRulesConverter.Settings);
    }

    internal static class VaccinRulesConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    public partial class ValidVaccineValue
    {
        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("mindose")]
        public int MinDose { get; set; }

        [JsonProperty("dayssincemindose")]
        public int DaysSinceMinDose { get; set; }
    }

    public partial class ValidVaccineValue
    {
        public static Dictionary<string, ValidVaccineValue> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, ValidVaccineValue>>(json, ValidVaccineValueConverter.Settings);
    }

    public static class ValidVaccineValueSerialize
    {
        public static string ToJson(this Dictionary<string, ValidVaccineValue> self) => JsonConvert.SerializeObject(self, ValidVaccineValueConverter.Settings);
    }

    internal static class ValidVaccineValueConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

}
