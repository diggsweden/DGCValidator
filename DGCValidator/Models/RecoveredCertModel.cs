using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class RecoveredCertModel : BaseModel,ICertModel
    {
        public CertType Type { get; set; }
        string _header;
        string _info;
        public string Cou { get; set; }
        public DateTimeOffset Dat { get; set; }
        public string Dis { get; set; }

        public RecoveredCertModel()
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
            Info = AppResources.RecTestDateLabel + Dat + "\n"+
                AppResources.CountryLabel + Cou;
        }
    }
}
