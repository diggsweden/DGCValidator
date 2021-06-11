using System;
using System.Collections.Generic;
using System.IO;
using DGCValidator.Services.DGC.V1;
namespace DGCValidator.Services.DGC
{
    public class CodeMapperUtil
    {
        //private static readonly string FileName = "ValueSets/ValueSet.json";

        //static CodeMapperUtil()
        //{
        //    if( File.Exists(FileName))
        //    {
        //        Dict = ValueSet.ValueSet.FromJson(File.ReadAllText(FileName));
        //    }
        //    else
        //    {
        //        Dict = new Dictionary<string, ValueSet.ValueSet>();
        //    }
        //}

        internal static string GetDiseaseAgentTargeted(string tg)
        {
            if (tg == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.Disesase).ValueSetValues.TryGetValue(tg, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : tg;
        }

        internal static string GetVaccineOrProphylaxis(string vp)
        {
            if (vp == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.VaccineProphylaxis).ValueSetValues.TryGetValue(vp, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : vp;
        }

        internal static string GetVaccineMedicalProduct(string mp)
        {
            if (mp == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.VaccineProduct).ValueSetValues.TryGetValue(mp, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : mp;
        }

        internal static string GetVaccineMarketingAuthHolder(string ma)
        {
            if (ma == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.VaccineManufacturer).ValueSetValues.TryGetValue(ma, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : ma;
        }

        internal static string GetTestResult(string tr)
        {
            if (tr == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.TestResult).ValueSetValues.TryGetValue(tr, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : tr;
        }

        internal static string GetTestMarketingAuthHolder(string ma)
        {
            if (ma == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.TestManufacturer).ValueSetValues.TryGetValue(ma, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : ma;
        }

        internal static string GetTestType(string tt)
        {
            if (tt == null)
            {
                return "";
            }
            _ = App.CertificateManager.ValueSets.GetValueOrDefault(Constants.TestType).ValueSetValues.TryGetValue(tt, out ValueSet.ValueSetValue value);
            return value != null ? value.Display : tt;
        }

    }
}
