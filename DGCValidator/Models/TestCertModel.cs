using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class TestCertModel : BaseModel, ICertModel
    {
        public CertType Type { get; set; }
        string _header;
        string _info;
        public string Tg { get; set; }//Dis Disease
        public string Tt { get; set; }//Typ Type of test
        public string Nm { get; set; }//Ori NAA Test name
        public string Ma { get; set; }// Tma RAT Test name and manufacturer
        public DateTimeOffset Sc { get; set; }// Test date
        public DateTimeOffset? Dr { get; set; }// Test result date
        //public string Sc { get; set; }// Test date
        //public string Dr { get; set; }// Test result date
        public string Tr { get; set; }//Res Test result
        public string Tc { get; set; }//Fac Testing centre
        public string Co { get; set; }//Cou Country
        public string Is { get; set; } // Issuer
        public string Ci { get; set; } // Certificate Identifier

        public TestCertModel()
        {
        }
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged();
            }
        }
        public void CreateHeaderAndInfo()
        {
            Header = AppResources.DiseaseLabel + Tg;
            Info = AppResources.TestTypeLabel + Tt + "\n" +
                AppResources.TestSampleOriginLabel + Nm + "\n" +
                AppResources.TestManifacturerLabel + Ma + "\n" +
                AppResources.TestDateLabel + Sc + "\n" +
                AppResources.TestResultDateLabel + Dr + "\n" +
                AppResources.TestResultLabel + Tr + "\n" +
                AppResources.TestingCentreLabel + Tc + "\n" +
                AppResources.CountryLabel + Co + "\n" +
                AppResources.IssuerLabel + Is + "\n" +
                AppResources.CertificateIdentifierLabel + Ci;
        }

        public static DateTimeOffset ConvertFromSecondsEpoc(long seconds)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(seconds).ToLocalTime();
            return dtDateTime;
        }
    }
}
