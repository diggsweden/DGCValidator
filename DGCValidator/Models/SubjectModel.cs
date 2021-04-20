using System;
using System.Collections.Generic;
using System.ComponentModel;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class SubjectModel : BaseModel
    {
        string _name="";
        string _dateOfBirth="";
        string _identifier = "";
        List<SubjectIdentity> _identities = new List<SubjectIdentity>();

        public SubjectModel()
        {
        }
        public SubjectModel(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return (_name!=null&&_name.Length>0?AppResources.NameLabel + _name:null); }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string DateOfBirth
        {
            get { return (_dateOfBirth != null && _dateOfBirth.Length > 0 ? AppResources.BirthDateLabel + _dateOfBirth:null); }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }
        public DateTimeOffset? ConvertDateOfBirth
        {
            set{
                if (value != null)
                {
                    DateOfBirth = ((DateTimeOffset)value).ToString("d");
                }
            }
        }
        public string Identifier
        {
            get { return (_identifier != null && _identifier.Length > 0 ? AppResources.PinLabel + _identifier:null); }
            set
            {
                _identifier = value;
                OnPropertyChanged();
            }
        }
        public void AddIdentity(SubjectIdentity identity)
        {
            _identities.Add(identity);
            OnPropertyChanged("Identities");
        }
        public void Clear()
        {
            Name = "";
            DateOfBirth = "";
            Identifier = "";
            _identities.Clear();
        }
    }

    public class SubjectIdentity
    {
        string Type { get; set; }
        string Identifier { get; set; }
    }
}
