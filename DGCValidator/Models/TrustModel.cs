using System;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class TrustModel : BaseModel
    {
        string _expirationDate;
        string _issuedDate;
        string _issuer = "";

        public TrustModel()
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
        public string Issuer
        {
            get { return _issuer; }
            set
            {
                _issuer = (value!=null?value.ToLower():"");
                OnPropertyChanged();
            }
        }
        public void Clear()
        {
            ExpirationDateString = "";
            IssuedDateString = "";
            Issuer = "";
        }
    }
}
