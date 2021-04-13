using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class RecoveredCertModel : BaseModel,ICertModel
    {
        public CertEnum Type { get; set; }
        String _header;
        String _info;
        public string Cou { get; set; }
        public DateTimeOffset Dat { get; set; }
        public string Dis { get; set; }

        public RecoveredCertModel()
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
            Info = AppResources.TestDateLabel + Dat + "\n"+
                AppResources.CountryLabel + Cou;
        }
    }
}
