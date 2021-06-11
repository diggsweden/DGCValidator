using System;
using System.Collections.Generic;
using System.ComponentModel;
using DGCValidator.Resources;

namespace DGCValidator.Models
{
    public class SubjectModel : BaseModel
    {
        string _name="";
        string _tranName = "";
        string _dateOfBirth ="";

        public SubjectModel()
        {
        }
        public SubjectModel(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
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
            Name = "";
            TranName = "";
            DateOfBirth = "";
        }
    }
}
