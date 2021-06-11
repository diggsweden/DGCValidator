using System;
using DGCValidator.Resources;
using Xamarin.Forms;

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
            get { return _expirationDate; }
            set
            {
                _expirationDate = value;
                OnPropertyChanged();
            }
        }
        public string IssuedDateString
        {
            get { return _issuedDate; }
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
                _expirationDate = ((DateTime)value).ToShortDateString();
                OnPropertyChanged();
            }
        }
        public DateTime? IssuedDate
        {
            set
            {
                _issuedDate = ((DateTime)value).ToShortDateString();
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
                    if(Device.RuntimePlatform.Equals(Device.Android))
                    {
                        return _issuerCountry + ".png";
                    }
                    else
                    {
                        return "Flags/" + _issuerCountry + ".png";
                    }
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
