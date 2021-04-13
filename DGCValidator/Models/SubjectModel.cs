using System;
using System.ComponentModel;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class SubjectModel : BaseModel
    {
        String _name="";
        String _dateOfBirth="";
        String _identifier = "";

        public SubjectModel()
        {
        }
        public SubjectModel(string name)
        {
            _name = name;
        }

        public String Name
        {
            get { return (_name!=null&&_name.Length>0?AppResources.NameLabel + _name:null); }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public String DateOfBirth
        {
            get { return (_dateOfBirth != null && _dateOfBirth.Length > 0 ? AppResources.BirthDateLabel + _dateOfBirth:null); }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }
        public String Identifier
        {
            get { return (_identifier != null && _identifier.Length > 0 ? AppResources.PinLabel + _identifier:null); }
            set
            {
                _identifier = value;
                OnPropertyChanged();
            }
        }

        public void Clear()
        {
            Name = "";
            DateOfBirth = "";
            Identifier = "";
        }
    }
}
