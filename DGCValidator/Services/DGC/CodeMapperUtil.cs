using System;
using System.Collections.Generic;
using System.IO;
using DGCValidator.Services.DGC.V1;
namespace DGCValidator.Services.DGC
{
    public class CodeMapperUtil
    {
        private static readonly string FileName = "ValueSets/ValueSet.json";

        static CodeMapperUtil()
        {
            if( File.Exists(FileName))
            {
                Dict = ValueSet.ValueSet.FromJson(File.ReadAllText(FileName));
            }
            else
            {
                Dict = new Dictionary<string, ValueSet.ValueSet>();
            }
        }

        public static Dictionary<string, ValueSet.ValueSet> Dict { get; private set; }

        internal static string GetDiseaseAgentTargeted(string tg)
        {
            ValueSet.ValueSet value;
            Dict.TryGetValue(tg, out value);
            return (value!=null?value.Display:tg);
        }

        internal static string GetTestResult(string tr)
        {
            ValueSet.ValueSet value;
            Dict.TryGetValue(tr, out value);
            return (value != null ? value.Display : tr);
        }

        internal static string GetVaccineOrProphylaxis(string vp)
        {
            ValueSet.ValueSet value;
            Dict.TryGetValue(vp, out value);
            return (value != null ? value.Display : vp);
        }

        internal static string GetVaccineMedicalProduct(string mp)
        {
            ValueSet.ValueSet value;
            Dict.TryGetValue(mp, out value);
            return (value != null ? value.Display : mp);
        }

        internal static string GetMarketingAuthHolder(string ma)
        {
            ValueSet.ValueSet value;
            Dict.TryGetValue(ma, out value);
            return (value != null ? value.Display : ma);
        }
    }
}
