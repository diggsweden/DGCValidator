using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class SignatureModel : BaseModel
    {
        DateTime? _expirationDate;
        DateTime? _issuedDate;
        String _issuerCountry = "";

        public SignatureModel()
        {
        }
        public DateTime? ExpriationDate
        {
            get { return _expirationDate; }
            set
            {
                _expirationDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime? IssuedDate
        {
            get { return _issuedDate; }
            set
            {
                _issuedDate = value;
                OnPropertyChanged();
            }
        }
        public String IssuerCountry
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
            ExpriationDate = null;
            IssuedDate = null;
            IssuerCountry = null;
        }
    }
}
