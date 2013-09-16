using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
{
    [Serializable]
    public abstract class DomainObject : INotifyPropertyChanged, IEditableObject, IDataErrorInfo
    {
        [NonSerialized]
        private static TraceSource m_traceSource;
        [XmlAttribute]
        public string WebId { get; set; }

        protected TraceSource TraceSource
        {
            get { return m_traceSource ?? (m_traceSource = new TraceSource("Application")); }
        }
        /// <summary>
        /// Write into trace source
        /// </summary>
        /// <param name="id">error id</param>
        /// <param name="message">message format</param>
        /// <param name="args">args to the message format</param>
        protected void TraceInfo(int id, string message, params object[] args)
        {
            TraceSource.TraceEvent(TraceEventType.Information, id, string.Format(message, args));

        }

        /// <summary>
        /// Write error trace source
        /// </summary>
        /// <param name="id">error id</param>
        /// <param name="message">message format</param>
        /// <param name="args">args to the message format</param>
        protected void TraceError(int id, string message, params object[] args)
        {
            TraceSource.TraceEvent(TraceEventType.Error, id, string.Format(message, args));

        }

        [NonSerialized]
        private Dictionary<string, string> m_errors;


        [NonSerialized]
        private PropertyDescriptorCollection m_shape;
        [NonSerialized]
        Hashtable m_propertyHashtable;

        [NonSerialized]

        // A flag to avoid stack calling OnPropertyChanging
        private bool m_setValueFlag;

        // private members	

        ///<summary>
        ///A flag wether this instance has been modified or not</summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Dirty { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public int Bil { get; set; }

        protected static bool IsInBlend()
        {
            return string.Equals(Process.GetCurrentProcess().ProcessName, "Blend", StringComparison.OrdinalIgnoreCase);

        }

        #region Public API

#if !SILVERLIGHT
        public bool HasErrors()
        {
            return (this.Errors.Count > 0);
        }
#endif
        #endregion

        #region Private API
        private Dictionary<string, string> Errors
        {
            get { return m_errors ?? (m_errors = new Dictionary<string, string>()); }
        }

        private PropertyDescriptorCollection Shape
        {
            get { return m_shape ?? (m_shape = TypeDescriptor.GetProperties(this)); }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            Dirty = true;
            if (null != PropertyChanged)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region IDataErrorInfo Members

#if !SILVERLIGHT
        [JsonIgnore]
        public string Error
        {
            get { return ((Errors.Count > 0) ? "Business object is in an invalid state" : string.Empty); }
        }

        public string this[string columnName]
        {
            get { return GetColumnError(columnName); }
        }

        private string GetColumn(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
                return string.Empty;
            string col = ((null == column) ? null : column.Trim().ToLower());

            if (string.IsNullOrEmpty(col) || (null == Shape.Find(col, true)))
            {
                throw new ArgumentException("Unable to find column: " + column);
            }

            return col;
        }

        protected void SetColumnError(string column, string error)
        {
            string col = GetColumn(column);

            if (null == error)
            {
                Errors.Remove(col);
            }
            else
            {
                Errors[col] = error;
            }
        }

        protected void ClearColumnErrors()
        {
            ClearColumnError(null);
        }

        protected void ClearColumnError(string column)
        {
            if (String.IsNullOrEmpty(column))
            {
                Errors.Clear();
            }
            else
            {
                SetColumnError(column, null);
            }
        }

        protected string GetColumnError(string column)
        {
            string col = GetColumn(column);

            return (Errors.ContainsKey(col) ? Errors[col] : null);
        }
#endif
        /// <summary>
        /// Validate the object and set the IDataError
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            return true;
        }
        #endregion

        #region IEditableObject Members

        [NonSerialized]

        private bool m_usingIEditable;
        public event EventHandler BeginEditFired;

        /// <summary>
        /// Workaround for Windows Forms BindingSource to have the tendency to call BeginEdit on every IEditableObject 
        /// </summary>
        /// <param name="useIEditableObject">true to begin to use IEditableObject</param>
        public void BeginEdit(bool useIEditableObject)
        {
            m_usingIEditable = useIEditableObject;
            BeginEdit();
        }
        /// <summary>
        /// Creates a temporary store before changes made to this instance is commited, see EndEdit and CancelEdit for followup
        /// NOTE : call BeginEdit(true) to actually use this interface
        /// </summary>
        public void BeginEdit()
        {
            if (!m_usingIEditable) return;
            if (null != BeginEditFired) BeginEditFired(this, EventArgs.Empty);

            if (null == m_propertyHashtable)
            {
                PropertyInfo[] props = (this.GetType()).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                m_propertyHashtable = new Hashtable(props.Length - 1);
            }
        }

        /// <summary>
        /// CancelEdit would discard the temporary store , since BeginEdit is called thus it would not be committed,
        /// use EndEdit to commit the changes to the instance
        /// </summary>
        public void CancelEdit()
        {
            m_usingIEditable = false;
            m_propertyHashtable = null;
        }

        /// <summary>
        /// Calling EndEdit will commit all the transient values into the instance
        /// </summary>
        public void EndEdit()
        {
            if (m_propertyHashtable == null) return;

            PropertyInfo[] props = (this.GetType()).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo t in props)
            {
                if (t.PropertyType.IsInterface) continue;
                if (!m_propertyHashtable.ContainsKey(t.Name)) continue;

                //check if there is set accessor
                try
                {
                    if (null != t.GetSetMethod())
                    {
                        m_setValueFlag = true;
                        t.SetValue(this, m_propertyHashtable[t.Name], null);
                    }
                }
                finally
                {
                    m_setValueFlag = false;
                }
            }
            CancelEdit();
        }



        /// <summary>
        /// Use for validation as well as IEditable object,
        /// for Editable object the value for the data will not be committed until the EndEdit is called
        /// </summary>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (m_setValueFlag) return;
            if (null != PropertyChanging)
            {
                PropertyChanging(this, e);
                if (e.Cancel) return;
            }


            if (null != m_propertyHashtable)
            {
                if (m_propertyHashtable.ContainsKey(e.PropertyName))
                {
                    m_propertyHashtable[e.PropertyName] = e.NewValue;
                }
                else
                {
                    m_propertyHashtable.Add(e.PropertyName, e.NewValue);
                }
                e.Cancel = true;
            }

        }
        #endregion
    }
}
