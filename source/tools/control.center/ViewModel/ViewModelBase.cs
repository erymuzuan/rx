using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bespoke.Sph.ControlCenter.Annotations;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.RaisePropertyChanged(propertyName);
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            
        }
    }
}
