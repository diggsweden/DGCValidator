using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DGCValidator.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public BaseModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
        [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }
    }
}
