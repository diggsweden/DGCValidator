
namespace DGCValidator.Services.DGC.V1
{
    using System;

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
    /// "VS-2021-04-08" - https://webgate.ec.europa.eu/fpfis/wikis/x/05PuKg
    /// </summary>
    public partial class EU_DGC
    {
        /// <summary>
        /// Certificate metadata
        /// </summary>
        [JsonProperty("cert", NullValueHandling = NullValueHandling.Ignore)]
        public Cert Cert { get; set; }

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
        /// Vaccination/prophylaxis information
        /// </summary>
        [JsonProperty("vac", NullValueHandling = NullValueHandling.Ignore)]
        public Vac[] Vac { get; set; }
    }

    /// <summary>
    /// Certificate metadata
    /// </summary>
    public partial class Cert
    {
        /// <summary>
        /// Country of the issuing authority (optional for vaccination and testing, viz. annex)
        /// </summary>
        [JsonProperty("co")]
        public string Co { get; set; }

        /// <summary>
        /// Identifier of the DGC, called UVID (V for vaccinations), will maybe be renamed to DGCI?
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Issuer of the DGC
        /// </summary>
        [JsonProperty("is")]
        public string Is { get; set; }

        /// <summary>
        /// Define schema type for easier detection for applicatione (e.g. smartphone wallets). Can
        /// also be used to refer to schemas with less data fields e.g. for privacy issues.
        /// </summary>
        [JsonProperty("ty")]
        public string Ty { get; set; }

        /// <summary>
        /// Certificate valid from (optional for vaccination and testing, viz. annex)
        /// </summary>
        [JsonProperty("vf")]
        public DateTimeOffset Vf { get; set; }

        /// <summary>
        /// Version of the schema (optional, viz. annex)
        /// </summary>
        [JsonProperty("vr")]
        public string Vr { get; set; }

        /// <summary>
        /// Certificate valid until (optional for vaccination and testing, viz. annex)
        /// </summary>
        [JsonProperty("vu")]
        public DateTimeOffset Vu { get; set; }
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
        /// Mandatory if no Person identifier is provided
        /// </summary>
        [JsonProperty("dob", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Dob { get; set; }

        /// <summary>
        /// The family name(s) of the person addressed in the certificate
        /// </summary>
        [JsonProperty("fn", NullValueHandling = NullValueHandling.Ignore)]
        public string Fn { get; set; }

        /// <summary>
        /// Administrative gender (optional, viz. VS-2021-04-08)
        /// </summary>
        [JsonProperty("gen", NullValueHandling = NullValueHandling.Ignore)]
        public string Gen { get; set; }

        /// <summary>
        /// The given name(s) of the person addressed in the certificate
        /// </summary>
        [JsonProperty("gn")]
        public string Gn { get; set; }

        /// <summary>
        /// Identifiers of the vaccinated person, according to the policies applicable in each country
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public PersonIdentifier[] Id { get; set; }
    }

    public partial class PersonIdentifier
    {
        [JsonProperty("i")]
        public string I { get; set; }

        /// <summary>
        /// The type of identifier (viz. VS-2021-04-08) PPN = Passport NNxxx = national Person
        /// Identifier (ISO 3166-1 alpha-3 country code) CZ = Citizenship card HC = Health Card
        /// number etc.
        /// </summary>
        [JsonProperty("t")]
        public string T { get; set; }
    }

    public partial class Tst
    {
        /// <summary>
        /// Country (member state) of test (ISO 3166-1 alpha-2 Country Code)
        /// </summary>
        [JsonProperty("cou")]
        public string Cou { get; set; }

        /// <summary>
        /// Disease or agent targeted
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
        /// event. (not specified in VS-2021-04-08)
        /// </summary>
        [JsonProperty("fac")]
        public string Fac { get; set; }

        /// <summary>
        /// Origin of sample that was taken (e.g. nasopharyngeal swab, oropharyngeal swab etc.), viz.
        /// VS-2021-04-08, optional
        /// </summary>
        [JsonProperty("ori", NullValueHandling = NullValueHandling.Ignore)]
        public string Ori { get; set; }

        /// <summary>
        /// Result of the test according to SNOMED CT (viz. VS-2021-04-08)
        /// </summary>
        [JsonProperty("res")]
        public string Res { get; set; }

        /// <summary>
        /// Manufacturer of the test, optional for NAAT test (work in progress in VS-2021-04-08)
        /// </summary>
        [JsonProperty("tma")]
        public string Tma { get; set; }

        /// <summary>
        /// Commercial or brand name of the RT-PCR or rapid antigen test (work in progress in
        /// VS-2021-04-08)
        /// </summary>
        [JsonProperty("tna")]
        public string Tna { get; set; }

        /// <summary>
        /// Code of the type of test that was conducted (viz. VS-2021-04-08)
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
        /// Code as defined in EMA SPOR - Organisations Management System (viz. VS-2021-04-08)
        /// </summary>
        [JsonProperty("aut")]
        public string Aut { get; set; }

        /// <summary>
        /// Country (member state) of vaccination (ISO 3166-1 alpha-2 Country Code)
        /// </summary>
        [JsonProperty("cou")]
        public string Cou { get; set; }

        /// <summary>
        /// The date of the vaccination event
        /// </summary>
        [JsonProperty("dat")]
        public DateTimeOffset Dat { get; set; }

        /// <summary>
        /// Disease or agent targeted (viz. VS-2021-04-08)
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
        /// Code of the medicinal product (viz. VS-2021-04-08)
        /// </summary>
        [JsonProperty("mep")]
        public string Mep { get; set; }

        /// <summary>
        /// Number of dose administered in a cycle
        /// </summary>
        [JsonProperty("seq")]
        public long Seq { get; set; }

        /// <summary>
        /// Number of expected doses for a complete cycle (specific for a person at the time of
        /// administration)
        /// </summary>
        [JsonProperty("tot")]
        public long Tot { get; set; }

        /// <summary>
        /// Generic description of the vaccine/prophylaxis or its component(s), (viz. VS-2021-04-08)
        /// </summary>
        [JsonProperty("vap")]
        public string Vap { get; set; }
    }

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
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
