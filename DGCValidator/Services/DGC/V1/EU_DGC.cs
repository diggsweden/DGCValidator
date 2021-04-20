
namespace DGCValidator.Services.DGC.V1
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Proof of vaccination, test results or recovery according to EU eHN, version 1.0,
    /// including certificate metadata; According to 1) REGULATION OF THE EUROPEAN PARLIAMENT AND
    /// OF THE COUNCIL on a framework for the issuance, verification and acceptance of
    /// interoperable certificates on vaccination, testing and recovery to facilitate free
    /// movement during the COVID-19 pandemic (Digital Green Certificate) -
    /// https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX%3A52021PC0130 2) Document
    /// "Value Sets for the digital green certificate as stated in the Annex ...", abbr.
    /// "VS-2021-04-14" - https://webgate.ec.europa.eu/fpfis/wikis/x/05PuKg 3) Guidelines on
    /// verifiable vaccination certificates - basic interoperability elements - Release 2 -
    /// 2021-03-12, abbr. "guidelines"
    /// </summary>
    public partial class EU_DGC
    {
        /// <summary>
        /// Unique identifier of the DGC (initially called UVCI (V for vaccination), later renamed to
        /// DGCI), format and composizion viz. guidelines
        /// </summary>
        [JsonProperty("dgcid")]
        public string Dgcid { get; set; }

        /// <summary>
        /// Recovery statement
        /// </summary>
        [JsonProperty("rec", NullValueHandling = NullValueHandling.Ignore)]
        public Rec[] Rec { get; set; }

        /// <summary>
        /// Subject
        /// </summary>
        [JsonProperty("sub")]
        public Sub Sub { get; set; }

        /// <summary>
        /// Test result statement
        /// </summary>
        [JsonProperty("tst", NullValueHandling = NullValueHandling.Ignore)]
        public Tst[] Tst { get; set; }

        /// <summary>
        /// Version of the schema, according to Semantic versioning (ISO, https://semver.org/ version
        /// 2.0.0 or newer) (viz. guidelines)
        /// </summary>
        [JsonProperty("v")]
        public string V { get; set; }

        /// <summary>
        /// Vaccination/prophylaxis information
        /// </summary>
        [JsonProperty("vac", NullValueHandling = NullValueHandling.Ignore)]
        public Vac[] Vac { get; set; }
    }

    public partial class Rec
    {
        /// <summary>
        /// Country (member state) in which the first positive test was performed (ISO 3166-1 alpha-2
        /// Country Code)
        /// </summary>
        [JsonProperty("cou")]
        public string Cou { get; set; }

        /// <summary>
        /// The date when the sample for the test was collected that led to a positive test result
        /// </summary>
        [JsonProperty("dat")]
        public DateTimeOffset Dat { get; set; }

        /// <summary>
        /// Disease or agent the citizen has recovered from
        /// </summary>
        [JsonProperty("dis")]
        public string Dis { get; set; }
    }

    /// <summary>
    /// Subject
    /// </summary>
    public partial class Sub
    {
        /// <summary>
        /// The date of birth of the person addressed in the certificate
        /// </summary>
        [JsonProperty("dob")]
        public DateTimeOffset Dob { get; set; }

        /// <summary>
        /// The family name(s) of the person addressed in the certificate
        /// </summary>
        [JsonProperty("fn", NullValueHandling = NullValueHandling.Ignore)]
        public string Fn { get; set; }

        /// <summary>
        /// The family name(s) of the person addressed in the certificate transliterated into the
        /// OCR-B Characters from ISO 1073-2 according to the ICAO Doc 9303 part 3.
        /// </summary>
        [JsonProperty("fnt", NullValueHandling = NullValueHandling.Ignore)]
        public string Fnt { get; set; }

        /// <summary>
        /// The given name(s) of the person addressed in the certificate
        /// </summary>
        [JsonProperty("gn")]
        public string Gn { get; set; }

        /// <summary>
        /// The given name(s) of the person addressed in the certificate transliterated into the
        /// OCR-B Characters from ISO 1073-2 according to the ICAO Doc 9303 part 3.
        /// </summary>
        [JsonProperty("gnt", NullValueHandling = NullValueHandling.Ignore)]
        public string Gnt { get; set; }

        /// <summary>
        /// Identifiers of the vaccinated person, according to the policies applicable in each country
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public PersonIdentifier[] Id { get; set; }
    }

    public partial class PersonIdentifier
    {
        /// <summary>
        /// Issuing country (ISO 3166-1 alpha-2 country code) of identifier
        /// </summary>
        [JsonProperty("c")]
        public string C { get; set; }

        [JsonProperty("i")]
        public string I { get; set; }

        /// <summary>
        /// The type of identifier (viz. VS-2021-04-08) PP = Passport Number NN = National Person
        /// Identifier (country specified in the 'c' parameter) CZ = Citizenship Card Number HC =
        /// Health Card Number
        /// </summary>
        [JsonProperty("t")]
        public IdentifierType T { get; set; }
    }

    public partial class Tst
    {
        /// <summary>
        /// Country (member state) of test (ISO 3166-1 alpha-2 Country Code)
        /// </summary>
        [JsonProperty("cou")]
        public string Cou { get; set; }

        /// <summary>
        /// Disease or agent targeted (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("dis")]
        public string Dis { get; set; }

        /// <summary>
        /// Date and time when the test result was produced (seconds since epoch)
        /// </summary>
        [JsonProperty("dtr")]
        public long Dtr { get; set; }

        /// <summary>
        /// Date and time when the sample for the test was collected (seconds since epoch)
        /// </summary>
        [JsonProperty("dts")]
        public long Dts { get; set; }

        /// <summary>
        /// Name/code of testing centre, facility or a health authority responsible for the testing
        /// event.
        /// </summary>
        [JsonProperty("fac")]
        public string Fac { get; set; }

        /// <summary>
        /// Origin of sample that was taken (e.g. nasopharyngeal swab, oropharyngeal swab etc.) (viz.
        /// VS-2021-04-14) optional
        /// </summary>
        [JsonProperty("ori", NullValueHandling = NullValueHandling.Ignore)]
        public string Ori { get; set; }

        /// <summary>
        /// Result of the test according to SNOMED CT (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("res")]
        public string Res { get; set; }

        /// <summary>
        /// Manufacturer and commercial name of the test used (optional for NAAT test) (viz.
        /// VS-2021-04-14)
        /// </summary>
        [JsonProperty("tma", NullValueHandling = NullValueHandling.Ignore)]
        public string Tma { get; set; }

        /// <summary>
        /// Code of the type of test that was conducted
        /// </summary>
        [JsonProperty("typ")]
        public string Typ { get; set; }
    }

    public partial class Vac
    {
        /// <summary>
        /// Name/code of administering centre or a health authority responsible for the vaccination
        /// event, optional
        /// </summary>
        [JsonProperty("adm", NullValueHandling = NullValueHandling.Ignore)]
        public string Adm { get; set; }

        /// <summary>
        /// Code as defined in EMA SPOR - Organisations Management System (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("aut")]
        public string Aut { get; set; }

        /// <summary>
        /// Country (member state) of vaccination (ISO 3166-1 alpha-2 Country Code) (viz.
        /// VS-2021-04-14)
        /// </summary>
        [JsonProperty("cou")]
        public string Cou { get; set; }

        /// <summary>
        /// The date of the vaccination event
        /// </summary>
        [JsonProperty("dat")]
        public DateTimeOffset Dat { get; set; }

        /// <summary>
        /// Disease or agent targeted (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("dis")]
        public string Dis { get; set; }

        /// <summary>
        /// A distinctive combination of numbers and/or letters which specifically identifies a
        /// batch, optional
        /// </summary>
        [JsonProperty("lot", NullValueHandling = NullValueHandling.Ignore)]
        public string Lot { get; set; }

        /// <summary>
        /// Code of the medicinal product (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("mep")]
        public string Mep { get; set; }

        /// <summary>
        /// Number of dose administered in a cycle  (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("seq")]
        public long Seq { get; set; }

        /// <summary>
        /// Number of expected doses for a complete cycle (specific for a person at the time of
        /// administration) (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("tot")]
        public long Tot { get; set; }

        /// <summary>
        /// Generic description of the vaccine/prophylaxis or its component(s), (viz. VS-2021-04-14)
        /// </summary>
        [JsonProperty("vap")]
        public string Vap { get; set; }
    }

    /// <summary>
    /// The type of identifier (viz. VS-2021-04-08) PP = Passport Number NN = National Person
    /// Identifier (country specified in the 'c' parameter) CZ = Citizenship Card Number HC =
    /// Health Card Number
    /// </summary>
    public enum IdentifierType { Cz, Hc, Nn, Pp };

    public partial class EU_DGC
    {
        public static EU_DGC FromJson(string json) => JsonConvert.DeserializeObject<EU_DGC>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this EU_DGC self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                IdentifierTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class IdentifierTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(IdentifierType) || t == typeof(IdentifierType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "CZ":
                    return IdentifierType.Cz;
                case "HC":
                    return IdentifierType.Hc;
                case "NN":
                case "pin":
                    return IdentifierType.Nn;
                case "PP":
                case "pas":
                case "nid":
                    return IdentifierType.Pp;
            }
            throw new Exception("Cannot unmarshal type IdentifierType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (IdentifierType)untypedValue;
            switch (value)
            {
                case IdentifierType.Cz:
                    serializer.Serialize(writer, "CZ");
                    return;
                case IdentifierType.Hc:
                    serializer.Serialize(writer, "HC");
                    return;
                case IdentifierType.Nn:
                    serializer.Serialize(writer, "NN");
                    return;
                case IdentifierType.Pp:
                    serializer.Serialize(writer, "PP");
                    return;
            }
            throw new Exception("Cannot marshal type IdentifierType");
        }

        public static readonly IdentifierTypeConverter Singleton = new IdentifierTypeConverter();
    }
}
