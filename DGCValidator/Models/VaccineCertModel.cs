using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class VaccineCertModel : BaseModel,ICertModel
    {
        public CertType Type { get; set; }
        string _header;
        string _info;
//        public string Adm { get; set; }//Adm
//        public string Cou { get; set; }//Cou
        public string Tg { get; set; }//Dis Disease or agent target
//        public string Lot { get; set; }//Lot
        public string Vp { get; set; }//Vap Vaccine or prophylaxis
        public string Mp { get; set; }//Mep Medical product
        public string Ma { get; set; }//Aut Marketing Authorization
        public long Dn { get; set; }//Seq Dose number
        public long Sd { get; set; }//Tot Total Doses
        public DateTimeOffset Dt { get; set; }//Dat Date
        //public string Dt { get; set; }//Dat Date
        public string Co { get; set; } // Country
        public string Is { get; set; } // Issuer
        public string Ci { get; set; } // Certificate Identifier

        public VaccineCertModel()
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
            Header = AppResources.DoseText + Dn + "/" + Sd;
            Info = AppResources.DiseaseLabel + Tg + "\n" +
                AppResources.VaccineDescriptionLabel + Vp + "\n" +
                AppResources.VaccineNameLabel + Mp + "\n" +
                AppResources.VaccineAuthLabel + Ma + "\n" +
                AppResources.VaccinationDateLabel + Dt.ToString("d") + "\n" +
                AppResources.CountryLabel + Co + "\n" +
                AppResources.IssuerLabel + Is + "\n" +
                AppResources.CertificateIdentifierLabel + Ci;

        }
    }
}
