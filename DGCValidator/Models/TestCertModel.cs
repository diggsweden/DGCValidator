using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class TestCertModel : BaseModel,ICertModel
    {
        public CertEnum Type { get; set; }
        String _header;
        String _info;
        public string Cou { get; set; }
        public DateTimeOffset Dat { get; set; }
        public string Dis { get; set; }
        public string Fac { get; set; }
        public string Ori { get; set; }
        public bool? Res { get; set; }
        public string Tma { get; set; }
        public string Tna { get; set; }
        public string Typ { get; set; }

        public TestCertModel()
        {
        }
        public String Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }
        public String Info
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
                AppResources.TestDateLabel + Dat + "\n" +
                AppResources.TestResultLabel + Res + "\n" +
                AppResources.TestingCentreLabel + Fac + "\n" +
                AppResources.CountryLabel + Cou;
        }
    }
}
