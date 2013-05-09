using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.Windows.Helpers
{
   public static class CommandHelper
    {

        public static void RaiseCommandExecuteChangeSafe<TSource>(this RelayCommand<TSource> command)
        {
            if (null != command) command.RaiseCanExecuteChanged();
        }
        public static   void RaiseCommandExecuteChangeSafe(this RelayCommand command)
        {
            if (null != command) command.RaiseCanExecuteChanged();
        }
    }
}
