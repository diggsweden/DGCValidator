using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class TestCertModel : BaseModel, ICertModel
    {
        public CertType Type { get; set; }
        string _header;
        string _info;
        public string Cou { get; set; }
        public DateTimeOffset Dts { get; set; }
        public DateTimeOffset Dtr { get; set; }
        public string Dis { get; set; }
        public string Fac { get; set; }
        public string Ori { get; set; }
        public string Res { get; set; }
        public string Tma { get; set; }
        public string Tna { get; set; }
        public string Typ { get; set; }

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
            Header = AppResources.DiseaseLabel + Dis;
            Info = AppResources.TestTypeLabel + Typ + "\n" +
                AppResources.TestNameLabel + Tna + "\n" +
                AppResources.TestManifacturerLabel + Tma + "\n" +
                AppResources.TestSampleOriginLabel + Ori + "\n" +
                AppResources.TestDateLabel + Dts + "\n" +
                AppResources.TestResultDateLabel + Dtr + "\n" +
                AppResources.TestResultLabel + Res + "\n" +
                AppResources.TestingCentreLabel + Fac + "\n" +
                AppResources.CountryLabel + Cou;
        }

        public static DateTimeOffset ConvertFromSecondsEpoc(long seconds)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(seconds).ToLocalTime();
            return dtDateTime;
        }
    }
}
