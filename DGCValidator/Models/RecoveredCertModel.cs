using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class RecoveredCertModel : BaseModel,ICertModel
    {
        public CertType Type { get; set; }
        string _header;
        string _info;
        public string Tg { get; set; }//Dis Disease
        //public string Fr { get; set; }//Dat First positive test result
        public DateTimeOffset Fr { get; set; }//Dat First positive test result
        public string Co { get; set; }//Cou Country
        public string Is { get; set; } // Issuer
        public DateTimeOffset Df { get; set; } // Certificate valid from
        public DateTimeOffset Du { get; set; } // Certificate valid until
        //public string Df { get; set; } // Certificate valid from
        //public string Du { get; set; } // Certificate valid until
        public string Ci { get; set; } // Certificate Identifier


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
            Header = AppResources.DiseaseLabel + Tg;
            Info = AppResources.RecTestDateLabel + Fr + "\n"+
                AppResources.CountryLabel + Co + "\n" +
                AppResources.IssuerLabel + Is + "\n" +
                AppResources.ValidFromLabel + Df + "\n" +
                AppResources.ValidUntilLabel + Du + "\n" +
                AppResources.CertificateIdentifierLabel + Ci;
        }
    }
}
