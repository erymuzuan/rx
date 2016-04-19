
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


// ReSharper disable InconsistentNaming
namespace Bespoke.Sph.Domain
{

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class LatLng
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_lat;
        public const string PropertyNameLat = "Lat";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_lng;
        public const string PropertyNameLng = "Lng";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double? m_elevation;
        public const string PropertyNameElevation = "Elevation";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public double Lat
        {
            set
            {
                if (Math.Abs(m_lat - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLat, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lat = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_lat;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public double Lng
        {
            set
            {
                if (Math.Abs(m_lng - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLng, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lng = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_lng;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public double? Elevation
        {
            set
            {
                if (Math.Abs(m_elevation ?? 0d - value ?? 0d) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameElevation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_elevation = value;
                    OnPropertyChanged();
                }
            }
            get { return m_elevation; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Document
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_extension;
        public const string PropertyNameExtension = "Extension";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<DocumentVersion> DocumentVersionCollection { get; } = new ObjectCollection<DocumentVersion>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Title
        {
            set
            {
                if (String.Equals(m_title, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTitle, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_title = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_title;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Extension
        {
            set
            {
                if (String.Equals(m_extension, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExtension, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_extension = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_extension;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DocumentVersion
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_storeId;
        public const string PropertyNameStoreId = "StoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_commitedBy;
        public const string PropertyNameCommitedBy = "CommitedBy";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_no;
        public const string PropertyNameNo = "No";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string StoreId
        {
            set
            {
                if (String.Equals(m_storeId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_storeId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_storeId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public DateTime Date
        {
            set
            {
                if (m_date == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_date = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_date;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string CommitedBy
        {
            set
            {
                if (String.Equals(m_commitedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommitedBy, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_commitedBy = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_commitedBy;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string No
        {
            set
            {
                if (String.Equals(m_no, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_no = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_no;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Note
        {
            set
            {
                if (String.Equals(m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_note = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_note;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Owner
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_telephoneNo;
        public const string PropertyNameTelephoneNo = "TelephoneNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_faxNo;
        public const string PropertyNameFaxNo = "FaxNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Address m_address
                = new Address();

        public const string PropertyNameAddress = "Address";
        [DebuggerHidden]

        public Address Address
        {
            get { return m_address; }
            set
            {
                m_address = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Name
        {
            set
            {
                if (String.Equals(m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_name = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_name;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string TelephoneNo
        {
            set
            {
                if (String.Equals(m_telephoneNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTelephoneNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_telephoneNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_telephoneNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string FaxNo
        {
            set
            {
                if (String.Equals(m_faxNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFaxNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_faxNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_faxNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Email
        {
            set
            {
                if (String.Equals(m_email, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmail, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_email = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_email;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class AuditTrail
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_user;
        public const string PropertyNameUser = "User";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateTime;
        public const string PropertyNameDateTime = "DateTime";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operation;
        public const string PropertyNameOperation = "Operation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityId;
        public const string PropertyNameEntityId = "EntityId";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Change> ChangeCollection { get; } = new ObjectCollection<Change>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string User
        {
            set
            {
                if (String.Equals(m_user, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUser, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_user = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_user;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public DateTime DateTime
        {
            set
            {
                if (m_dateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateTime = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateTime;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Operation
        {
            set
            {
                if (String.Equals(m_operation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operation;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Type
        {
            set
            {
                if (String.Equals(m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_type = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_type;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string EntityId
        {
            set
            {
                if (String.Equals(m_entityId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityId;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Change
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_propertyName;
        public const string PropertyNamePropertyName = "PropertyName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_oldValue;
        public const string PropertyNameOldValue = "OldValue";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_newValue;
        public const string PropertyNameNewValue = "NewValue";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_action;
        public const string PropertyNameAction = "Action";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string PropertyName
        {
            set
            {
                if (String.Equals(m_propertyName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePropertyName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_propertyName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_propertyName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OldValue
        {
            set
            {
                if (String.Equals(m_oldValue, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOldValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_oldValue = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_oldValue;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string NewValue
        {
            set
            {
                if (String.Equals(m_newValue, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNewValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_newValue = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_newValue;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Action
        {
            set
            {
                if (String.Equals(m_action, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAction, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_action = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_action;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Organization
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_officeNo;
        public const string PropertyNameOfficeNo = "OfficeNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_faxNo;
        public const string PropertyNameFaxNo = "FaxNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Address m_address
                = new Address();

        public const string PropertyNameAddress = "Address";
        [DebuggerHidden]

        public Address Address
        {
            get { return m_address; }
            set
            {
                m_address = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Name
        {
            set
            {
                if (String.Equals(m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_name = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_name;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string RegistrationNo
        {
            set
            {
                if (String.Equals(m_registrationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRegistrationNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_registrationNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_registrationNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Email
        {
            set
            {
                if (String.Equals(m_email, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmail, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_email = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_email;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OfficeNo
        {
            set
            {
                if (String.Equals(m_officeNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOfficeNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_officeNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_officeNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string FaxNo
        {
            set
            {
                if (String.Equals(m_faxNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFaxNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_faxNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_faxNo;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class UserProfile
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_userName;
        public const string PropertyNameUserName = "UserName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fullName;
        public const string PropertyNameFullName = "FullName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_designation;
        public const string PropertyNameDesignation = "Designation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_telephone;
        public const string PropertyNameTelephone = "Telephone";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mobile;
        public const string PropertyNameMobile = "Mobile";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_roleTypes;
        public const string PropertyNameRoleTypes = "RoleTypes";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_startModule;
        public const string PropertyNameStartModule = "StartModule";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_department;
        public const string PropertyNameDepartment = "Department";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_hasChangedDefaultPassword;
        public const string PropertyNameHasChangedDefaultPassword = "HasChangedDefaultPassword";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_language;
        public const string PropertyNameLanguage = "Language";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string UserName
        {
            set
            {
                if (String.Equals(m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string FullName
        {
            set
            {
                if (String.Equals(m_fullName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFullName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fullName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fullName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Designation
        {
            set
            {
                if (String.Equals(m_designation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDesignation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_designation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_designation;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Telephone
        {
            set
            {
                if (String.Equals(m_telephone, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTelephone, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_telephone = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_telephone;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Mobile
        {
            set
            {
                if (String.Equals(m_mobile, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMobile, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mobile = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mobile;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string RoleTypes
        {
            set
            {
                if (String.Equals(m_roleTypes, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRoleTypes, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_roleTypes = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_roleTypes;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string StartModule
        {
            set
            {
                if (String.Equals(m_startModule, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartModule, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startModule = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_startModule;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Email
        {
            set
            {
                if (String.Equals(m_email, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmail, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_email = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_email;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Department
        {
            set
            {
                if (String.Equals(m_department, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDepartment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_department = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_department;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool HasChangedDefaultPassword
        {
            set
            {
                if (m_hasChangedDefaultPassword == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHasChangedDefaultPassword, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_hasChangedDefaultPassword = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_hasChangedDefaultPassword;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Language
        {
            set
            {
                if (String.Equals(m_language, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLanguage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_language = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_language;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Setting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_userName;
        public const string PropertyNameUserName = "UserName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_key;
        public const string PropertyNameKey = "Key";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_value;
        public const string PropertyNameValue = "Value";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string UserName
        {
            set
            {
                if (String.Equals(m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userName;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Key
        {
            set
            {
                if (String.Equals(m_key, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameKey, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_key = value;
                    OnPropertyChanged();
                }
            }
            get { return m_key; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Value
        {
            set
            {
                if (String.Equals(m_value, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_value = value;
                    OnPropertyChanged();
                }
            }
            get { return m_value; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Designation
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_startModule;
        public const string PropertyNameStartModule = "StartModule";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_enforceStartModule;
        public const string PropertyNameEnforceStartModule = "EnforceStartModule";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isSearchVisible;
        public const string PropertyNameIsSearchVisible = "IsSearchVisible";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isMessageVisible;
        public const string PropertyNameIsMessageVisible = "IsMessageVisible";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHelpVisible;
        public const string PropertyNameIsHelpVisible = "IsHelpVisible";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_helpUri;
        public const string PropertyNameHelpUri = "HelpUri";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_option;
        public const string PropertyNameOption = "Option";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> RoleCollection { get; } = new ObjectCollection<string>();


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Owner m_owner
                = new Owner();

        public const string PropertyNameOwner = "Owner";
        [DebuggerHidden]

        public Owner Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> SearchableEntityCollection { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Name
        {
            set
            {
                if (String.Equals(m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_name = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_name;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Description
        {
            set
            {
                if (String.Equals(m_description, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDescription, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_description = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_description;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsActive
        {
            set
            {
                if (m_isActive == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsActive, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isActive = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isActive;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string StartModule
        {
            set
            {
                if (String.Equals(m_startModule, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartModule, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startModule = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_startModule;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public bool EnforceStartModule
        {
            set
            {
                if (m_enforceStartModule == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnforceStartModule, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_enforceStartModule = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_enforceStartModule;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsSearchVisible
        {
            set
            {
                if (m_isSearchVisible == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsSearchVisible, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isSearchVisible = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isSearchVisible;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsMessageVisible
        {
            set
            {
                if (m_isMessageVisible == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsMessageVisible, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isMessageVisible = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isMessageVisible;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsHelpVisible
        {
            set
            {
                if (m_isHelpVisible == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHelpVisible, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHelpVisible = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHelpVisible;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string HelpUri
        {
            set
            {
                if (String.Equals(m_helpUri, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHelpUri, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_helpUri = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_helpUri;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Title
        {
            set
            {
                if (String.Equals(m_title, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTitle, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_title = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_title;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int Option
        {
            set
            {
                if (m_option == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOption, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_option = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_option;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Watcher
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityName;
        public const string PropertyNameEntityName = "EntityName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityId;
        public const string PropertyNameEntityId = "EntityId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_user;
        public const string PropertyNameUser = "User";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateTime;
        public const string PropertyNameDateTime = "DateTime";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string EntityName
        {
            set
            {
                if (String.Equals(m_entityName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string EntityId
        {
            set
            {
                if (String.Equals(m_entityId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string User
        {
            set
            {
                if (String.Equals(m_user, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUser, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_user = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_user;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsActive
        {
            set
            {
                if (m_isActive == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsActive, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isActive = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isActive;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public DateTime DateTime
        {
            set
            {
                if (m_dateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateTime = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateTime;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Profile
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fullName;
        public const string PropertyNameFullName = "FullName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_userName;
        public const string PropertyNameUserName = "UserName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_password;
        public const string PropertyNamePassword = "Password";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_confirmPassword;
        public const string PropertyNameConfirmPassword = "ConfirmPassword";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_designation;
        public const string PropertyNameDesignation = "Designation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_telephone;
        public const string PropertyNameTelephone = "Telephone";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mobile;
        public const string PropertyNameMobile = "Mobile";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isNew;
        public const string PropertyNameIsNew = "IsNew";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_department;
        public const string PropertyNameDepartment = "Department";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string FullName
        {
            set
            {
                if (String.Equals(m_fullName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFullName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fullName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fullName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string UserName
        {
            set
            {
                if (String.Equals(m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Email
        {
            set
            {
                if (String.Equals(m_email, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmail, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_email = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_email;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Password
        {
            set
            {
                if (String.Equals(m_password, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePassword, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_password = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_password;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ConfirmPassword
        {
            set
            {
                if (String.Equals(m_confirmPassword, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameConfirmPassword, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_confirmPassword = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_confirmPassword;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Status
        {
            set
            {
                if (String.Equals(m_status, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_status = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_status;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Designation
        {
            set
            {
                if (String.Equals(m_designation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDesignation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_designation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_designation;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Telephone
        {
            set
            {
                if (String.Equals(m_telephone, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTelephone, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_telephone = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_telephone;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Mobile
        {
            set
            {
                if (String.Equals(m_mobile, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMobile, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mobile = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mobile;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsNew
        {
            set
            {
                if (m_isNew == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsNew, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isNew = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isNew;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Department
        {
            set
            {
                if (String.Equals(m_department, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDepartment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_department = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_department;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Message
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_subject;
        public const string PropertyNameSubject = "Subject";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRead;
        public const string PropertyNameIsRead = "IsRead";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_body;
        public const string PropertyNameBody = "Body";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_userName;
        public const string PropertyNameUserName = "UserName";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Subject
        {
            set
            {
                if (String.Equals(m_subject, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubject, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_subject = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_subject;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsRead
        {
            set
            {
                if (m_isRead == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRead, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRead = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRead;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Body
        {
            set
            {
                if (String.Equals(m_body, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBody, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_body = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_body;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string UserName
        {
            set
            {
                if (String.Equals(m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userName;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Photo
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_storeId;
        public const string PropertyNameStoreId = "StoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_thumbnailStoreId;
        public const string PropertyNameThumbnailStoreId = "ThumbnailStoreId";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Title
        {
            set
            {
                if (String.Equals(m_title, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTitle, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_title = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_title;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Description
        {
            set
            {
                if (String.Equals(m_description, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDescription, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_description = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_description;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string StoreId
        {
            set
            {
                if (String.Equals(m_storeId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_storeId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_storeId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ThumbnailStoreId
        {
            set
            {
                if (String.Equals(m_thumbnailStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameThumbnailStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_thumbnailStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_thumbnailStoreId;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Address
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_unitNo;
        public const string PropertyNameUnitNo = "UnitNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_floor;
        public const string PropertyNameFloor = "Floor";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_block;
        public const string PropertyNameBlock = "Block";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_street;
        public const string PropertyNameStreet = "Street";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_city;
        public const string PropertyNameCity = "City";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_postcode;
        public const string PropertyNamePostcode = "Postcode";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_state;
        public const string PropertyNameState = "State";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_country;
        public const string PropertyNameCountry = "Country";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string UnitNo
        {
            set
            {
                if (String.Equals(m_unitNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUnitNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_unitNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_unitNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Floor
        {
            set
            {
                if (String.Equals(m_floor, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloor, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floor = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_floor;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Block
        {
            set
            {
                if (String.Equals(m_block, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBlock, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_block = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_block;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Street
        {
            set
            {
                if (String.Equals(m_street, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStreet, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_street = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_street;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string City
        {
            set
            {
                if (String.Equals(m_city, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_city = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_city;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Postcode
        {
            set
            {
                if (String.Equals(m_postcode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePostcode, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_postcode = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_postcode;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string State
        {
            set
            {
                if (String.Equals(m_state, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameState, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_state = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_state;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Country
        {
            set
            {
                if (String.Equals(m_country, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCountry, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_country = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_country;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class EmailTemplate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_subjectTemplate;
        public const string PropertyNameSubjectTemplate = "SubjectTemplate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_bodyTemplate;
        public const string PropertyNameBodyTemplate = "BodyTemplate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Entity
        {
            set
            {
                if (String.Equals(m_entity, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entity;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Name
        {
            set
            {
                if (String.Equals(m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_name = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_name;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Note
        {
            set
            {
                if (String.Equals(m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_note = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_note;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string SubjectTemplate
        {
            set
            {
                if (String.Equals(m_subjectTemplate, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubjectTemplate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_subjectTemplate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_subjectTemplate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string BodyTemplate
        {
            set
            {
                if (String.Equals(m_bodyTemplate, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBodyTemplate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_bodyTemplate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_bodyTemplate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsPublished
        {
            set
            {
                if (m_isPublished == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPublished, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPublished = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPublished;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DocumentTemplate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_wordTemplateStoreId;
        public const string PropertyNameWordTemplateStoreId = "WordTemplateStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Name
        {
            set
            {
                if (String.Equals(m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_name = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_name;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Note
        {
            set
            {
                if (String.Equals(m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_note = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_note;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string WordTemplateStoreId
        {
            set
            {
                if (String.Equals(m_wordTemplateStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWordTemplateStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_wordTemplateStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_wordTemplateStoreId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsPublished
        {
            set
            {
                if (m_isPublished == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPublished, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPublished = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPublished;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Entity
        {
            set
            {
                if (String.Equals(m_entity, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entity;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class QuotaPolicy
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RateLimit m_rateLimit
                = new RateLimit();

        public const string PropertyNameRateLimit = "RateLimit";
        [DebuggerHidden]

        public RateLimit RateLimit
        {
            get { return m_rateLimit; }
            set
            {
                m_rateLimit = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private QuotaLimit m_quotaLimit
                = new QuotaLimit();

        public const string PropertyNameQuotaLimit = "QuotaLimit";
        [DebuggerHidden]

        public QuotaLimit QuotaLimit
        {
            get { return m_quotaLimit; }
            set
            {
                m_quotaLimit = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BandwidthLimit m_bandwidthLimit
                = new BandwidthLimit();

        public const string PropertyNameBandwidthLimit = "BandwidthLimit";
        [DebuggerHidden]

        public BandwidthLimit BandwidthLimit
        {
            get { return m_bandwidthLimit; }
            set
            {
                m_bandwidthLimit = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<EndpointLimit> EndpointLimitCollection { get; } = new ObjectCollection<EndpointLimit>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Name
        {
            set
            {
                if (String.Equals(m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_name = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_name;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class RateLimit
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isUnlimited;
        public const string PropertyNameIsUnlimited = "IsUnlimited";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_calls;
        public const string PropertyNameCalls = "Calls";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimePeriod m_renewalPeriod;
        public const string PropertyNameRenewalPeriod = "RenewalPeriod";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsUnlimited
        {
            set
            {
                if (m_isUnlimited == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsUnlimited, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isUnlimited = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isUnlimited;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Calls
        {
            set
            {
                if (m_calls == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCalls, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_calls = value;
                    OnPropertyChanged();
                }
            }
            get { return m_calls; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public TimePeriod RenewalPeriod
        {
            set
            {
                if (m_renewalPeriod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRenewalPeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_renewalPeriod = value;
                    OnPropertyChanged();
                }
            }
            get { return m_renewalPeriod; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class QuotaLimit
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isUnlimited;
        public const string PropertyNameIsUnlimited = "IsUnlimited";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_calls;
        public const string PropertyNameCalls = "Calls";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimePeriod m_renewalPeriod;
        public const string PropertyNameRenewalPeriod = "RenewalPeriod";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsUnlimited
        {
            set
            {
                if (m_isUnlimited == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsUnlimited, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isUnlimited = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isUnlimited;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Calls
        {
            set
            {
                if (m_calls == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCalls, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_calls = value;
                    OnPropertyChanged();
                }
            }
            get { return m_calls; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public TimePeriod RenewalPeriod
        {
            set
            {
                if (m_renewalPeriod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRenewalPeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_renewalPeriod = value;
                    OnPropertyChanged();
                }
            }
            get { return m_renewalPeriod; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class BandwidthLimit
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isUnlimited;
        public const string PropertyNameIsUnlimited = "IsUnlimited";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_size;
        public const string PropertyNameSize = "Size";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimePeriod m_renewalPeriod;
        public const string PropertyNameRenewalPeriod = "RenewalPeriod";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsUnlimited
        {
            set
            {
                if (m_isUnlimited == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsUnlimited, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isUnlimited = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isUnlimited;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Size
        {
            set
            {
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get { return m_size; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public TimePeriod RenewalPeriod
        {
            set
            {
                if (m_renewalPeriod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRenewalPeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_renewalPeriod = value;
                    OnPropertyChanged();
                }
            }
            get { return m_renewalPeriod; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class EndpointLimit
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_controller;
        public const string PropertyNameController = "Controller";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_action;
        public const string PropertyNameAction = "Action";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_parent;
        public const string PropertyNameParent = "Parent";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_calls;
        public const string PropertyNameCalls = "Calls";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimePeriod m_renewalPeriod;
        public const string PropertyNameRenewalPeriod = "RenewalPeriod";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Controller
        {
            set
            {
                if (String.Equals(m_controller, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameController, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_controller = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_controller;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Action
        {
            set
            {
                if (String.Equals(m_action, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAction, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_action = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_action;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Parent
        {
            set
            {
                if (String.Equals(m_parent, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameParent, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_parent = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_parent;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Note
        {
            set
            {
                if (String.Equals(m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_note = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_note;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Calls
        {
            set
            {
                if (m_calls == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCalls, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_calls = value;
                    OnPropertyChanged();
                }
            }
            get { return m_calls; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public TimePeriod RenewalPeriod
        {
            set
            {
                if (m_renewalPeriod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRenewalPeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_renewalPeriod = value;
                    OnPropertyChanged();
                }
            }
            get { return m_renewalPeriod; }
        }


    }


    public partial class TimePeriod
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int m_count;
        public const string PropertyNameCount = "Count";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_unit;
        public const string PropertyNameUnit = "Unit";


        // public properties members



        public int Count
        {
            set
            {
                if (m_count == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_count = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_count;
            }
        }



        public string Unit
        {
            set
            {
                if (m_unit == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUnit, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_unit = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_unit;
            }
        }



    }


}
// ReSharper restore InconsistentNaming

