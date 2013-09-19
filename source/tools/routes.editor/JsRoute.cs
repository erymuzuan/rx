using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using routes.editor.Annotations;

namespace routes.editor
{
    public class JsRoute : INotifyPropertyChanged, IDataErrorInfo
    {
        public override string ToString()
        {
            return this.ModuleId;
        }
        public string Role { get; set; }
        public string GroupName { get; set; }
        public string Url { set; get; }
        public string ModuleId { set; get; }
        public string Name { set; get; }
        public bool Visible { set; get; }
        public string Icon { set; get; }
        public string Caption { set; get; }
        public JsRouteSetting Settings { set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Show if the user if logged in, applicable only for route null or empty role
        /// </summary>
        public bool ShowWhenLoggedIn { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Dictionary<string, string> m_errors = new Dictionary<string, string>();
        public void AddErrors(string property, string message)
        {
            if (!m_errors.ContainsKey(property))
                m_errors.Add(property, message);
            else
                m_errors[property] = message;
        }

        public string this[string columnName]
        {
            get
            {
                return m_errors.ContainsKey(columnName) ? m_errors[columnName] : null;
            }
        }

        public string Error
        {
            get { return ((m_errors.Count > 0) ? "Route object is in an invalid state" : string.Empty); }
        }

        public void RemoveErrors(string property)
        {
            if (m_errors.ContainsKey(property))
                m_errors.Remove(property);
        }
    }
}