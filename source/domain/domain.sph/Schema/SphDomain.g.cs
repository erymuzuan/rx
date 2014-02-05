
using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;


// ReSharper disable InconsistentNaming
namespace Bespoke.Sph.Domain
{

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("LatLng", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Document", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Document
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_extension;
        public const string PropertyNameExtension = "Extension";


        private readonly ObjectCollection<DocumentVersion> m_DocumentVersionCollection = new ObjectCollection<DocumentVersion>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DocumentVersion", IsNullable = false)]
        public ObjectCollection<DocumentVersion> DocumentVersionCollection
        {
            get { return m_DocumentVersionCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("DocumentVersion", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

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
    [XmlType("Owner", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("AuditTrail", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        private int m_entityId;
        public const string PropertyNameEntityId = "EntityId";


        private readonly ObjectCollection<Change> m_ChangeCollection = new ObjectCollection<Change>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Change", IsNullable = false)]
        public ObjectCollection<Change> ChangeCollection
        {
            get { return m_ChangeCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int EntityId
        {
            set
            {
                if (m_entityId == value) return;
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
    [XmlType("Change", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Organization", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("UserProfile", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        private int m_userProfileId;
        public const string PropertyNameUserProfileId = "UserProfileId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_department;
        public const string PropertyNameDepartment = "Department";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_hasChangedDefaultPassword;
        public const string PropertyNameHasChangedDefaultPassword = "HasChangedDefaultPassword";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int UserProfileId
        {
            set
            {
                if (m_userProfileId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserProfileId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userProfileId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userProfileId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Setting", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Setting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_settingId;
        public const string PropertyNameSettingId = "SettingId";


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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int SettingId
        {
            set
            {
                if (m_settingId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSettingId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_settingId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_settingId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Designation", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Designation
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_designationId;
        public const string PropertyNameDesignationId = "DesignationId";


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


        private readonly ObjectCollection<string> m_RoleCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> RoleCollection
        {
            get { return m_RoleCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int DesignationId
        {
            set
            {
                if (m_designationId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDesignationId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_designationId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_designationId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Watcher", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Watcher
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_watcherId;
        public const string PropertyNameWatcherId = "WatcherId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityName;
        public const string PropertyNameEntityName = "EntityName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_entityId;
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int WatcherId
        {
            set
            {
                if (m_watcherId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWatcherId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_watcherId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_watcherId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int EntityId
        {
            set
            {
                if (m_entityId == value) return;
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Profile", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Message", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Message
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_messageId;
        public const string PropertyNameMessageId = "MessageId";


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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int MessageId
        {
            set
            {
                if (m_messageId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMessageId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_messageId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_messageId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Photo", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
    [XmlType("Address", Namespace = Strings.DEFAULT_NAMESPACE)]
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

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
        [XmlAttribute]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

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

}
// ReSharper restore InconsistentNaming

