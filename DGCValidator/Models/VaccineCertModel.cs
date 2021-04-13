using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class VaccineCertModel : BaseModel,ICertModel
    {
        public CertEnum Type { get; set; }
        String _header;
        String _info;
        public string Adm { get; set; }
        public string Aut { get; set; }
        public DateTimeOffset Dat { get; set; }
        public string Des { get; set; }
        public string Dis { get; set; }
        public string Lot { get; set; }
        public string Nam { get; set; }
        public long Seq { get; set; }
        public long Tot { get; set; }

        public VaccineCertModel()
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
            Header = AppResources.DoseText + Seq + "/" + Tot;
            Info = AppResources.DiseaseLabel + Dis + "\n" +
                AppResources.VaccineNameLabel + Nam + "\n" +
                AppResources.VaccineDescriptionLabel + Des + "\n" +
                AppResources.VaccineAuthLabel + Aut + "\n" +
                AppResources.VaccineLotLabel + Lot + "\n" +
                AppResources.VaccinationDateLabel + Dat.ToString("d") + "\n" +
                AppResources.VaccinationProviderLabel + Adm; ;
        }
    }
}
