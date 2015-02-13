using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.SyntaxVisualizers.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        public RelayCommand<string> OpenCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NewFileCommand { get; set; }

        public AppViewModel()
        {
            this.OpenCommand = new RelayCommand<string>(Open);
            this.SaveCommand = new RelayCommand(Save, () => File.Exists(this.EditedFilePath));
            this.NewFileCommand = new RelayCommand(NewFile, () => File.Exists(this.EditedFilePath));
        }

        private void NewFile()
        {
            throw new System.NotImplementedException();
        }

        public Task LoadAsync(IView view)
        {
            return Task.FromResult(0);

        }

        private void Save()
        {
            File.WriteAllText(this.EditedFilePath, this.Code);
        }

        private void Open(string file)
        {
            if (File.Exists(file))
                this.Code = File.ReadAllText(file);
        }


        private string m_code;
        private string m_editedFilePath;
        private readonly ObjectCollection<string> m_openFileCollection = new ObjectCollection<string>();
        private readonly ObjectCollection<string> m_fileCollection = new ObjectCollection<string>();
        private bool m_isBusy;

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged();
            }
        }

        public ObjectCollection<string> FileCollection
        {
            get { return m_fileCollection; }
        }

        public ObjectCollection<string> OpenFileCollection
        {
            get { return m_openFileCollection; }
        }

        public string EditedFilePath
        {
            get { return m_editedFilePath; }
            set
            {
                m_editedFilePath = value;
                RaisePropertyChanged();
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Code
        {
            get { return m_code; }
            set
            {
                m_code = value;
                RaisePropertyChanged();
            }
        }
    }
}
