using System;
using System.Reflection;
using System.Text;
using Bespoke.SphCommercialSpaces.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace observable.generator
{
    internal class MainViewModel : ViewModelBase
    {
        public RelayCommand<Type> GenerateObservableCommand { get; set; }

        public MainViewModel()
        {
            this.GenerateObservableCommand = new RelayCommand<Type>(GenerateObservableExecute, t => null != t);
        }

        private void GenerateObservableExecute(Type type)
        {
            this.ObservableText = GenerateObservable(type, type.Name.ToLower());
        }

        private string GenerateObservable(Type type, string name, int tab = 4)
        {
            var sb = new StringBuilder(name + ":{");
            sb.AppendLine();
            var natives = new[] { typeof(int), typeof(DateTime), typeof(string), typeof(double), typeof(decimal), typeof(bool) };
            var properties = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).AsQueryable()
                             where natives.Contains(p.PropertyType)
                            && p.CanRead && p.CanWrite
                             select string.Format("{1}{0} : ko.observable()", p.Name, " ".PadLeft(tab, ' '));
            sb.AppendLine(string.Join(",\r\n", properties));

            var colls = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).AsQueryable()
                        where p.Name.EndsWith("Collection")
                        select string.Format("{1}{0} : ko.observableArray([])", p.Name, " ".PadLeft(tab, ' '));
            sb.AppendLine(string.Join(",\r\n", colls));

            var customProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).AsQueryable()
                                       .Where(
                                           p =>
                                           p.PropertyType.Namespace == typeof(Entity).Namespace &&
                                           !p.PropertyType.Name.EndsWith("Collection"));

            foreach (var p in customProperties)
            {
                if (p.Name.EndsWith("Collection")) continue;
                sb.AppendLine(this.GenerateObservable(p.PropertyType,p.Name, tab + 4));
            }


            sb.AppendLine("},");
            return sb.ToString();

        }

        public void Load()
        {
            this.IsBusy = true;
            var asembly = typeof(Entity).Assembly;
            var types = asembly.GetTypes();
            this.TypeCollection.ClearAndAddRange(types);

            this.IsBusy = false;
        }


        private readonly ObjectCollection<Type> m_typeCollection = new ObjectCollection<Type>();
        private Type m_selectedType;
        private string m_observableText;
        private bool m_isBusy;

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string ObservableText
        {
            get { return m_observableText; }
            set
            {
                m_observableText = value;
                RaisePropertyChanged("ObservableText");
            }
        }

        public Type SelectedType
        {
            get { return m_selectedType; }
            set
            {
                m_selectedType = value;
                RaisePropertyChanged("SelectedType");
            }
        }
        public ObjectCollection<Type> TypeCollection
        {
            get { return m_typeCollection; }
        }
    }
}