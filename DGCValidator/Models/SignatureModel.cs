using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class SignatureModel : BaseModel
    {
        string _expirationDate;
        string _issuedDate;
        string _issuerCountry = "";

        public SignatureModel()
        {
        }
        public string ExpirationDateString
        {
            get { return (_expirationDate!=null&&_expirationDate.Length>0?AppResources.ExpireDateLabel + _expirationDate.ToString():""); }
            set
            {
                _expirationDate = value;
                OnPropertyChanged();
            }
        }
        public string IssuedDateString
        {
            get { return (_issuedDate!=null&&_issuedDate.Length>0? AppResources.IssuedDateLabel + _issuedDate:""); }
            set
            {
                _issuedDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? ExpirationDate
        {
            set
            {
                _expirationDate = ((DateTime)value).ToString();
                OnPropertyChanged();
            }
        }
        public DateTime? IssuedDate
        {
            set
            {
                _issuedDate = ((DateTime)value).ToString();
                OnPropertyChanged();
            }
        }
        public string IssuerCountry
        {
            get { return _issuerCountry; }
            set
            {
                _issuerCountry = (value!=null?value.ToLower():"");
                OnPropertyChanged();
                OnPropertyChanged("IssuerCountryImage");

            }
        }
        public String IssuerCountryImage
        {
            get
            {
                if( _issuerCountry != null)
                {
                    return _issuerCountry + ".png";
                }
                return "";
            }
        }
        public void Clear()
        {
            ExpirationDateString = "";
            IssuedDateString = "";
            IssuerCountry = "";
        }
    }
}
