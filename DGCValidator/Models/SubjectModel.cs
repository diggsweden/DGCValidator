using System;
using System.Collections.Generic;
using System.ComponentModel;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class SubjectModel : BaseModel
    {
        string _firstname = "";
        string _familyname = "";
        string _tranName = "";
        string _dateOfBirth ="";

        public SubjectModel()
        {
        }
        public SubjectModel(string firstname, string familyname)
        {
            _firstname = firstname;
            _familyname = familyname;
        }

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                OnPropertyChanged();
            }
        }
        public string Familyname
        {
            get { return _familyname; }
            set
            {
                _familyname = value;
                OnPropertyChanged();
            }
        }
        public string TranName
        {
            get { return (_tranName != null && _tranName.Length > 0 ? _tranName : null); }
            set
            {
                _tranName = value;
                OnPropertyChanged();
            }
        }
        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }
        //public DateTimeOffset? ConvertDateOfBirth
        //{
        //    set{
        //        if (value != null)
        //        {
        //            DateOfBirth = ((DateTimeOffset)value).ToString("d");
        //        }
        //    }
        //}
        public void Clear()
        {
            Firstname = "";
            Familyname = "";
            TranName = "";
            DateOfBirth = "";
        }
    }
}
