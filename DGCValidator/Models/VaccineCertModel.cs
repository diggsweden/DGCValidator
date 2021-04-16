using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class VaccineCertModel : BaseModel,ICertModel
    {
        public CertType Type { get; set; }
        string _header;
        string _info;
        public string Adm { get; set; }
        public string Aut { get; set; }
        public string Cou { get; set; }
        public DateTimeOffset Dat { get; set; }
        public string Dis { get; set; }
        public string Lot { get; set; }
        public string Mep { get; set; }
        public long Seq { get; set; }
        public long Tot { get; set; }
        public string Vap { get; set; }

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
            Header = AppResources.DoseText + Seq + "/" + Tot;
            Info = AppResources.DiseaseLabel + Dis + "\n" +
                AppResources.VaccineDescriptionLabel + Vap + "\n" +
                AppResources.VaccineNameLabel + Mep + "\n" +
                AppResources.VaccineAuthLabel + Aut + "\n" +
                AppResources.VaccineLotLabel + Lot + "\n" +
                AppResources.VaccinationDateLabel + Dat.ToString("d") + "\n" +
                AppResources.VaccinationProviderLabel + Adm + "\n" +
                AppResources.CountryLabel + Cou;

        }
    }
}
