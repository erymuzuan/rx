﻿
using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


// ReSharper disable InconsistentNaming
namespace Bespoke.Sph.Domain
{

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ContractHistory", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ContractHistory
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_contractNo;
        public const string PropertyNameContractNo = "ContractNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateFrom;
        public const string PropertyNameDateFrom = "DateFrom";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateStart;
        public const string PropertyNameDateStart = "DateStart";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_dateEnd;
        public const string PropertyNameDateEnd = "DateEnd";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ContractNo
        {
            set
            {
                if (String.Equals(m_contractNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime DateFrom
        {
            set
            {
                if (m_dateFrom == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateFrom, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateFrom = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateFrom;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime DateStart
        {
            set
            {
                if (m_dateStart == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateStart, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateStart = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateStart;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

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
        [DebuggerHidden]

        public DateTime? DateEnd
        {
            set
            {
                if (m_dateEnd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateEnd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateEnd = value;
                    OnPropertyChanged();
                }
            }
            get { return m_dateEnd; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Tenant", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Tenant
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_tenantId;
        public const string PropertyNameTenantId = "TenantId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_idSsmNo;
        public const string PropertyNameIdSsmNo = "IdSsmNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_bussinessType;
        public const string PropertyNameBussinessType = "BussinessType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_phone;
        public const string PropertyNamePhone = "Phone";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fax;
        public const string PropertyNameFax = "Fax";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mobilePhone;
        public const string PropertyNameMobilePhone = "MobilePhone";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_username;
        public const string PropertyNameUsername = "Username";


        private readonly ObjectCollection<ContractHistory> m_ContractHistoryCollection = new ObjectCollection<ContractHistory>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ContractHistory", IsNullable = false)]
        public ObjectCollection<ContractHistory> ContractHistoryCollection
        {
            get { return m_ContractHistoryCollection; }
        }

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

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TenantId
        {
            set
            {
                if (m_tenantId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTenantId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tenantId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tenantId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string IdSsmNo
        {
            set
            {
                if (String.Equals(m_idSsmNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIdSsmNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_idSsmNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_idSsmNo;
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

        public string BussinessType
        {
            set
            {
                if (String.Equals(m_bussinessType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBussinessType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_bussinessType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_bussinessType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Phone
        {
            set
            {
                if (String.Equals(m_phone, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePhone, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_phone = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_phone;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string Fax
        {
            set
            {
                if (String.Equals(m_fax, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFax, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fax = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fax;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string MobilePhone
        {
            set
            {
                if (String.Equals(m_mobilePhone, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMobilePhone, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mobilePhone = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mobilePhone;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

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

        public string Username
        {
            set
            {
                if (String.Equals(m_username, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUsername, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_username = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_username;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Land", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Land
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_landId;
        public const string PropertyNameLandId = "LandId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_unit;
        public const string PropertyNameUnit = "Unit";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_location;
        public const string PropertyNameLocation = "Location";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_sizeUnit;
        public const string PropertyNameSizeUnit = "SizeUnit";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_rezabNo;
        public const string PropertyNameRezabNo = "RezabNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_sheetNo;
        public const string PropertyNameSheetNo = "SheetNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_approvedPlanNo;
        public const string PropertyNameApprovedPlanNo = "ApprovedPlanNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_landOffice;
        public const string PropertyNameLandOffice = "LandOffice";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_usage;
        public const string PropertyNameUsage = "Usage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_ownLevel;
        public const string PropertyNameOwnLevel = "OwnLevel";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_planNo;
        public const string PropertyNamePlanNo = "PlanNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isApproved;
        public const string PropertyNameIsApproved = "IsApproved";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_district;
        public const string PropertyNameDistrict = "District";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mukim;
        public const string PropertyNameMukim = "Mukim";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_lotNo;
        public const string PropertyNameLotNo = "LotNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_staticTerm;
        public const string PropertyNameStaticTerm = "StaticTerm";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fileNo;
        public const string PropertyNameFileNo = "FileNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_propertyValue;
        public const string PropertyNamePropertyValue = "PropertyValue";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_leaseExpiryDate;
        public const string PropertyNameLeaseExpiryDate = "LeaseExpiryDate";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_leasePeriod;
        public const string PropertyNameLeasePeriod = "LeasePeriod";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_approvedDateTime;
        public const string PropertyNameApprovedDateTime = "ApprovedDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_approvedBy;
        public const string PropertyNameApprovedBy = "ApprovedBy";



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

        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
        }

        private readonly ObjectCollection<Attachment> m_AttachmentCollection = new ObjectCollection<Attachment>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Attachment", IsNullable = false)]
        public ObjectCollection<Attachment> AttachmentCollection
        {
            get { return m_AttachmentCollection; }
        }

        private readonly ObjectCollection<Ownership> m_OwnershipCollection = new ObjectCollection<Ownership>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Ownership", IsNullable = false)]
        public ObjectCollection<Ownership> OwnershipCollection
        {
            get { return m_OwnershipCollection; }
        }

        private readonly ObjectCollection<MarketEvaluation> m_MarketEvaluationCollection = new ObjectCollection<MarketEvaluation>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("MarketEvaluation", IsNullable = false)]
        public ObjectCollection<MarketEvaluation> MarketEvaluationCollection
        {
            get { return m_MarketEvaluationCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int LandId
        {
            set
            {
                if (m_landId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLandId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_landId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_landId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Unit
        {
            set
            {
                if (String.Equals(m_unit, value, StringComparison.Ordinal)) return;
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

        public string Location
        {
            set
            {
                if (String.Equals(m_location, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLocation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_location = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_location;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public double Size
        {
            set
            {
                if (Math.Abs(m_size - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_size;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string SizeUnit
        {
            set
            {
                if (String.Equals(m_sizeUnit, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSizeUnit, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sizeUnit = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sizeUnit;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string RezabNo
        {
            set
            {
                if (String.Equals(m_rezabNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRezabNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rezabNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rezabNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string SheetNo
        {
            set
            {
                if (String.Equals(m_sheetNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSheetNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sheetNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sheetNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ApprovedPlanNo
        {
            set
            {
                if (String.Equals(m_approvedPlanNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApprovedPlanNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_approvedPlanNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_approvedPlanNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string LandOffice
        {
            set
            {
                if (String.Equals(m_landOffice, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLandOffice, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_landOffice = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_landOffice;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Usage
        {
            set
            {
                if (String.Equals(m_usage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUsage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_usage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_usage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string OwnLevel
        {
            set
            {
                if (String.Equals(m_ownLevel, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOwnLevel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_ownLevel = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_ownLevel;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PlanNo
        {
            set
            {
                if (String.Equals(m_planNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePlanNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_planNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_planNo;
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

        public bool IsApproved
        {
            set
            {
                if (m_isApproved == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsApproved, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isApproved = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isApproved;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string District
        {
            set
            {
                if (String.Equals(m_district, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDistrict, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_district = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_district;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Mukim
        {
            set
            {
                if (String.Equals(m_mukim, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMukim, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mukim = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mukim;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string LotNo
        {
            set
            {
                if (String.Equals(m_lotNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLotNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lotNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_lotNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string StaticTerm
        {
            set
            {
                if (String.Equals(m_staticTerm, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStaticTerm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_staticTerm = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_staticTerm;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FileNo
        {
            set
            {
                if (String.Equals(m_fileNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFileNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fileNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fileNo;
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

        public double PropertyValue
        {
            set
            {
                if (Math.Abs(m_propertyValue - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePropertyValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_propertyValue = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_propertyValue;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? LeaseExpiryDate
        {
            set
            {
                if (m_leaseExpiryDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLeaseExpiryDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_leaseExpiryDate = value;
                    OnPropertyChanged();
                }
            }
            get { return m_leaseExpiryDate; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? LeasePeriod
        {
            set
            {
                if (m_leasePeriod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLeasePeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_leasePeriod = value;
                    OnPropertyChanged();
                }
            }
            get { return m_leasePeriod; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? ApprovedDateTime
        {
            set
            {
                if (m_approvedDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApprovedDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_approvedDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_approvedDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string ApprovedBy
        {
            set
            {
                if (String.Equals(m_approvedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApprovedBy, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_approvedBy = value;
                    OnPropertyChanged();
                }
            }
            get { return m_approvedBy; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Building", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Building
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_templateId;
        public const string PropertyNameTemplateId = "TemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_templateName;
        public const string PropertyNameTemplateName = "TemplateName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_buildingId;
        public const string PropertyNameBuildingId = "BuildingId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_unitNo;
        public const string PropertyNameUnitNo = "UnitNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_buildingSize;
        public const string PropertyNameBuildingSize = "BuildingSize";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_floors;
        public const string PropertyNameFloors = "Floors";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_buildingType;
        public const string PropertyNameBuildingType = "BuildingType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_blocks;
        public const string PropertyNameBlocks = "Blocks";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_floorSize;
        public const string PropertyNameFloorSize = "FloorSize";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_buildingRegistrationNo;
        public const string PropertyNameBuildingRegistrationNo = "BuildingRegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_landRegistrationNo;
        public const string PropertyNameLandRegistrationNo = "LandRegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_height;
        public const string PropertyNameHeight = "Height";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_completedDateTime;
        public const string PropertyNameCompletedDateTime = "CompletedDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_builtDateTime;
        public const string PropertyNameBuiltDateTime = "BuiltDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal? m_cost;
        public const string PropertyNameCost = "Cost";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_handingOverDateTime;
        public const string PropertyNameHandingOverDateTime = "HandingOverDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_registeredDateTime;
        public const string PropertyNameRegisteredDateTime = "RegisteredDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_movingInDateTime;
        public const string PropertyNameMovingInDateTime = "MovingInDateTime";



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

        private readonly ObjectCollection<Floor> m_FloorCollection = new ObjectCollection<Floor>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Floor", IsNullable = false)]
        public ObjectCollection<Floor> FloorCollection
        {
            get { return m_FloorCollection; }
        }

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

        private readonly ObjectCollection<Block> m_BlockCollection = new ObjectCollection<Block>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Block", IsNullable = false)]
        public ObjectCollection<Block> BlockCollection
        {
            get { return m_BlockCollection; }
        }

        private readonly ObjectCollection<CustomListValue> m_CustomListValueCollection = new ObjectCollection<CustomListValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomListValue", IsNullable = false)]
        public ObjectCollection<CustomListValue> CustomListValueCollection
        {
            get { return m_CustomListValueCollection; }
        }

        private readonly ObjectCollection<MarketEvaluation> m_MarketEvaluationCollection = new ObjectCollection<MarketEvaluation>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("MarketEvaluation", IsNullable = false)]
        public ObjectCollection<MarketEvaluation> MarketEvaluationCollection
        {
            get { return m_MarketEvaluationCollection; }
        }

        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TemplateId
        {
            set
            {
                if (m_templateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string TemplateName
        {
            set
            {
                if (String.Equals(m_templateName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int BuildingId
        {
            set
            {
                if (m_buildingId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingId;
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

        [Required]

        [DebuggerHidden]

        public double BuildingSize
        {
            set
            {
                if (Math.Abs(m_buildingSize - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingSize = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingSize;
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

        public int Floors
        {
            set
            {
                if (m_floors == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloors, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floors = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_floors;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string BuildingType
        {
            set
            {
                if (String.Equals(m_buildingType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Blocks
        {
            set
            {
                if (m_blocks == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBlocks, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_blocks = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_blocks;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public double FloorSize
        {
            set
            {
                if (Math.Abs(m_floorSize - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloorSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floorSize = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_floorSize;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string BuildingRegistrationNo
        {
            set
            {
                if (String.Equals(m_buildingRegistrationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingRegistrationNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingRegistrationNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingRegistrationNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string LandRegistrationNo
        {
            set
            {
                if (String.Equals(m_landRegistrationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLandRegistrationNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_landRegistrationNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_landRegistrationNo;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Height
        {
            set
            {
                if (m_height == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHeight, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_height = value;
                    OnPropertyChanged();
                }
            }
            get { return m_height; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? CompletedDateTime
        {
            set
            {
                if (m_completedDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompletedDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_completedDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_completedDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? BuiltDateTime
        {
            set
            {
                if (m_builtDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuiltDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_builtDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_builtDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public decimal? Cost
        {
            set
            {
                if (m_cost == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCost, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cost = value;
                    OnPropertyChanged();
                }
            }
            get { return m_cost; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? HandingOverDateTime
        {
            set
            {
                if (m_handingOverDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHandingOverDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_handingOverDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_handingOverDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? RegisteredDateTime
        {
            set
            {
                if (m_registeredDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRegisteredDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_registeredDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_registeredDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? MovingInDateTime
        {
            set
            {
                if (m_movingInDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMovingInDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_movingInDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_movingInDateTime; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Block", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Block
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_floorPlanStoreId;
        public const string PropertyNameFloorPlanStoreId = "FloorPlanStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_floors;
        public const string PropertyNameFloors = "Floors";



        private readonly ObjectCollection<Floor> m_FloorCollection = new ObjectCollection<Floor>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Floor", IsNullable = false)]
        public ObjectCollection<Floor> FloorCollection
        {
            get { return m_FloorCollection; }
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

        public double Size
        {
            set
            {
                if (Math.Abs(m_size - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_size;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FloorPlanStoreId
        {
            set
            {
                if (String.Equals(m_floorPlanStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloorPlanStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floorPlanStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_floorPlanStoreId;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int Floors
        {
            set
            {
                if (m_floors == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloors, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floors = value;
                    OnPropertyChanged();
                }
            }
            get { return m_floors; }
        }


    }

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

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Floor", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Floor
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_number;
        public const string PropertyNameNumber = "Number";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        private readonly ObjectCollection<Unit> m_UnitCollection = new ObjectCollection<Unit>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Unit", IsNullable = false)]
        public ObjectCollection<Unit> UnitCollection
        {
            get { return m_UnitCollection; }
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

        public double Size
        {
            set
            {
                if (Math.Abs(m_size - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_size;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Number
        {
            set
            {
                if (String.Equals(m_number, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNumber, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_number = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_number;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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
    [XmlType("Unit", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Unit
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_no;
        public const string PropertyNameNo = "No";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_floorNo;
        public const string PropertyNameFloorNo = "FloorNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_blockNo;
        public const string PropertyNameBlockNo = "BlockNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isSpace;
        public const string PropertyNameIsSpace = "IsSpace";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_usage;
        public const string PropertyNameUsage = "Usage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_fillOpacity;
        public const string PropertyNameFillOpacity = "FillOpacity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fillColor;
        public const string PropertyNameFillColor = "FillColor";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_planStoreId;
        public const string PropertyNamePlanStoreId = "PlanStoreId";


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

        [Required]

        [DebuggerHidden]

        public double Size
        {
            set
            {
                if (Math.Abs(m_size - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_size;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FloorNo
        {
            set
            {
                if (String.Equals(m_floorNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloorNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floorNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_floorNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string BlockNo
        {
            set
            {
                if (String.Equals(m_blockNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBlockNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_blockNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_blockNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsSpace
        {
            set
            {
                if (m_isSpace == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsSpace, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isSpace = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isSpace;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Usage
        {
            set
            {
                if (String.Equals(m_usage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUsage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_usage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_usage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public double FillOpacity
        {
            set
            {
                if (Math.Abs(m_fillOpacity - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFillOpacity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fillOpacity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fillOpacity;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FillColor
        {
            set
            {
                if (String.Equals(m_fillColor, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFillColor, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fillColor = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fillColor;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PlanStoreId
        {
            set
            {
                if (String.Equals(m_planStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePlanStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_planStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_planStoreId;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Space", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Space
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_spaceId;
        public const string PropertyNameSpaceId = "SpaceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_templateId;
        public const string PropertyNameTemplateId = "TemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_templateName;
        public const string PropertyNameTemplateName = "TemplateName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_buildingId;
        public const string PropertyNameBuildingId = "BuildingId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_unitNo;
        public const string PropertyNameUnitNo = "UnitNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_floorName;
        public const string PropertyNameFloorName = "FloorName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_rentalType;
        public const string PropertyNameRentalType = "RentalType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isOnline;
        public const string PropertyNameIsOnline = "IsOnline";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAvailable;
        public const string PropertyNameIsAvailable = "IsAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_contactPerson;
        public const string PropertyNameContactPerson = "ContactPerson";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_contactNo;
        public const string PropertyNameContactNo = "ContactNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_state;
        public const string PropertyNameState = "State";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_city;
        public const string PropertyNameCity = "City";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_buildingName;
        public const string PropertyNameBuildingName = "BuildingName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_buildingUnit;
        public const string PropertyNameBuildingUnit = "BuildingUnit";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_rentalRate;
        public const string PropertyNameRentalRate = "RentalRate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_location;
        public const string PropertyNameLocation = "Location";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mapIcon;
        public const string PropertyNameMapIcon = "MapIcon";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_smallIcon;
        public const string PropertyNameSmallIcon = "SmallIcon";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_icon;
        public const string PropertyNameIcon = "Icon";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_furnishing;
        public const string PropertyNameFurnishing = "Furnishing";


        private readonly ObjectCollection<Unit> m_UnitCollection = new ObjectCollection<Unit>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Unit", IsNullable = false)]
        public ObjectCollection<Unit> UnitCollection
        {
            get { return m_UnitCollection; }
        }

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

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

        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
        }

        private readonly ObjectCollection<CustomListValue> m_CustomListValueCollection = new ObjectCollection<CustomListValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomListValue", IsNullable = false)]
        public ObjectCollection<CustomListValue> CustomListValueCollection
        {
            get { return m_CustomListValueCollection; }
        }

        private readonly ObjectCollection<FeatureDefinition> m_FeatureDefinitionCollection = new ObjectCollection<FeatureDefinition>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("FeatureDefinition", IsNullable = false)]
        public ObjectCollection<FeatureDefinition> FeatureDefinitionCollection
        {
            get { return m_FeatureDefinitionCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int SpaceId
        {
            set
            {
                if (m_spaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSpaceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_spaceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_spaceId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TemplateId
        {
            set
            {
                if (m_templateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string TemplateName
        {
            set
            {
                if (String.Equals(m_templateName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int BuildingId
        {
            set
            {
                if (m_buildingId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingId;
            }
        }


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

        [Required]

        [DebuggerHidden]

        public string FloorName
        {
            set
            {
                if (String.Equals(m_floorName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloorName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_floorName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_floorName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public double Size
        {
            set
            {
                if (Math.Abs(m_size - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_size;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string RentalType
        {
            set
            {
                if (String.Equals(m_rentalType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentalType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rentalType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rentalType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsOnline
        {
            set
            {
                if (m_isOnline == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsOnline, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isOnline = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isOnline;
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

        public bool IsAvailable
        {
            set
            {
                if (m_isAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string ContactPerson
        {
            set
            {
                if (String.Equals(m_contactPerson, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContactPerson, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contactPerson = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contactPerson;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string ContactNo
        {
            set
            {
                if (String.Equals(m_contactNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContactNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contactNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contactNo;
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

        public string BuildingName
        {
            set
            {
                if (String.Equals(m_buildingName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string BuildingUnit
        {
            set
            {
                if (String.Equals(m_buildingUnit, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingUnit, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingUnit = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_buildingUnit;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal RentalRate
        {
            set
            {
                if (m_rentalRate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentalRate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rentalRate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rentalRate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Location
        {
            set
            {
                if (String.Equals(m_location, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLocation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_location = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_location;
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

        public string MapIcon
        {
            set
            {
                if (String.Equals(m_mapIcon, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMapIcon, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mapIcon = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mapIcon;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string SmallIcon
        {
            set
            {
                if (String.Equals(m_smallIcon, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSmallIcon, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_smallIcon = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_smallIcon;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Icon
        {
            set
            {
                if (String.Equals(m_icon, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIcon, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_icon = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_icon;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Furnishing
        {
            set
            {
                if (String.Equals(m_furnishing, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFurnishing, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_furnishing = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_furnishing;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("RentalApplication", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class RentalApplication
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_rentalApplicationId;
        public const string PropertyNameRentalApplicationId = "RentalApplicationId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_templateId;
        public const string PropertyNameTemplateId = "TemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_templateName;
        public const string PropertyNameTemplateName = "TemplateName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_companyName;
        public const string PropertyNameCompanyName = "CompanyName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_companyRegistrationNo;
        public const string PropertyNameCompanyRegistrationNo = "CompanyRegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateStart;
        public const string PropertyNameDateStart = "DateStart";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateEnd;
        public const string PropertyNameDateEnd = "DateEnd";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_purpose;
        public const string PropertyNamePurpose = "Purpose";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_companyType;
        public const string PropertyNameCompanyType = "CompanyType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_spaceId;
        public const string PropertyNameSpaceId = "SpaceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_experience;
        public const string PropertyNameExperience = "Experience";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRecordExist;
        public const string PropertyNameIsRecordExist = "IsRecordExist";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_previousAddress;
        public const string PropertyNamePreviousAddress = "PreviousAddress";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isCompany;
        public const string PropertyNameIsCompany = "IsCompany";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_remarks;
        public const string PropertyNameRemarks = "Remarks";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_applicationDate;
        public const string PropertyNameApplicationDate = "ApplicationDate";


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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Contact m_contact
                = new Contact();

        public const string PropertyNameContact = "Contact";
        [DebuggerHidden]

        public Contact Contact
        {
            get { return m_contact; }
            set
            {
                m_contact = value;
                OnPropertyChanged();
            }
        }

        private readonly ObjectCollection<Attachment> m_AttachmentCollection = new ObjectCollection<Attachment>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Attachment", IsNullable = false)]
        public ObjectCollection<Attachment> AttachmentCollection
        {
            get { return m_AttachmentCollection; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Offer m_offer
                = new Offer();

        public const string PropertyNameOffer = "Offer";
        [DebuggerHidden]

        public Offer Offer
        {
            get { return m_offer; }
            set
            {
                m_offer = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Space m_space
                = new Space();

        public const string PropertyNameSpace = "Space";
        [DebuggerHidden]

        public Space Space
        {
            get { return m_space; }
            set
            {
                m_space = value;
                OnPropertyChanged();
            }
        }

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

        private readonly ObjectCollection<CustomListValue> m_CustomListValueCollection = new ObjectCollection<CustomListValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomListValue", IsNullable = false)]
        public ObjectCollection<CustomListValue> CustomListValueCollection
        {
            get { return m_CustomListValueCollection; }
        }

        private readonly ObjectCollection<Feature> m_FeatureCollection = new ObjectCollection<Feature>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Feature", IsNullable = false)]
        public ObjectCollection<Feature> FeatureCollection
        {
            get { return m_FeatureCollection; }
        }

        private readonly ObjectCollection<Bank> m_BankCollection = new ObjectCollection<Bank>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Bank", IsNullable = false)]
        public ObjectCollection<Bank> BankCollection
        {
            get { return m_BankCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int RentalApplicationId
        {
            set
            {
                if (m_rentalApplicationId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentalApplicationId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rentalApplicationId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rentalApplicationId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TemplateId
        {
            set
            {
                if (m_templateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string TemplateName
        {
            set
            {
                if (String.Equals(m_templateName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string CompanyName
        {
            set
            {
                if (String.Equals(m_companyName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompanyName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_companyName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_companyName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string CompanyRegistrationNo
        {
            set
            {
                if (String.Equals(m_companyRegistrationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompanyRegistrationNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_companyRegistrationNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_companyRegistrationNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime DateStart
        {
            set
            {
                if (m_dateStart == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateStart, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateStart = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateStart;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime DateEnd
        {
            set
            {
                if (m_dateEnd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateEnd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateEnd = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateEnd;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Purpose
        {
            set
            {
                if (String.Equals(m_purpose, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePurpose, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_purpose = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_purpose;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string CompanyType
        {
            set
            {
                if (String.Equals(m_companyType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompanyType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_companyType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_companyType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int SpaceId
        {
            set
            {
                if (m_spaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSpaceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_spaceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_spaceId;
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

        public string Experience
        {
            set
            {
                if (String.Equals(m_experience, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExperience, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_experience = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_experience;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsRecordExist
        {
            set
            {
                if (m_isRecordExist == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRecordExist, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRecordExist = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRecordExist;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PreviousAddress
        {
            set
            {
                if (String.Equals(m_previousAddress, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePreviousAddress, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_previousAddress = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_previousAddress;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsCompany
        {
            set
            {
                if (m_isCompany == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCompany, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isCompany = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isCompany;
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

        [DebuggerHidden]

        public string Remarks
        {
            set
            {
                if (String.Equals(m_remarks, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemarks, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_remarks = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_remarks;
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

        public DateTime ApplicationDate
        {
            set
            {
                if (m_applicationDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApplicationDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_applicationDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_applicationDate;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Attachment", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Attachment
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isReceived;
        public const string PropertyNameIsReceived = "IsReceived";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_storeId;
        public const string PropertyNameStoreId = "StoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isCompleted;
        public const string PropertyNameIsCompleted = "IsCompleted";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_receivedDateTime;
        public const string PropertyNameReceivedDateTime = "ReceivedDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_receivedBy;
        public const string PropertyNameReceivedBy = "ReceivedBy";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";



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

        public bool IsRequired
        {
            set
            {
                if (m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequired = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequired;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsReceived
        {
            set
            {
                if (m_isReceived == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsReceived, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isReceived = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isReceived;
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

        public bool IsCompleted
        {
            set
            {
                if (m_isCompleted == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCompleted, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isCompleted = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isCompleted;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? ReceivedDateTime
        {
            set
            {
                if (m_receivedDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceivedDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_receivedDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_receivedDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string ReceivedBy
        {
            set
            {
                if (String.Equals(m_receivedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceivedBy, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_receivedBy = value;
                    OnPropertyChanged();
                }
            }
            get { return m_receivedBy; }
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
            get { return m_note; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Bank", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Bank
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_location;
        public const string PropertyNameLocation = "Location";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_accountNo;
        public const string PropertyNameAccountNo = "AccountNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_accountType;
        public const string PropertyNameAccountType = "AccountType";


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

        public string Location
        {
            set
            {
                if (String.Equals(m_location, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLocation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_location = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_location;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string AccountNo
        {
            set
            {
                if (String.Equals(m_accountNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_accountNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_accountNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string AccountType
        {
            set
            {
                if (String.Equals(m_accountType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_accountType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_accountType;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Contact", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Contact
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_contractId;
        public const string PropertyNameContractId = "ContractId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mobileNo;
        public const string PropertyNameMobileNo = "MobileNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_officeNo;
        public const string PropertyNameOfficeNo = "OfficeNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_icNo;
        public const string PropertyNameIcNo = "IcNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_designation;
        public const string PropertyNameDesignation = "Designation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_age;
        public const string PropertyNameAge = "Age";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_job;
        public const string PropertyNameJob = "Job";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ContractId
        {
            set
            {
                if (m_contractId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractId;
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

        [DebuggerHidden]

        public string MobileNo
        {
            set
            {
                if (String.Equals(m_mobileNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMobileNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mobileNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mobileNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

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

        public string IcNo
        {
            set
            {
                if (String.Equals(m_icNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIcNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_icNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_icNo;
            }
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

        public int Age
        {
            set
            {
                if (m_age == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAge, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_age = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_age;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Job
        {
            set
            {
                if (String.Equals(m_job, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameJob, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_job = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_job;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ContractTemplate", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ContractTemplate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_contractTemplateId;
        public const string PropertyNameContractTemplateId = "ContractTemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_interestRate;
        public const string PropertyNameInterestRate = "InterestRate";


        private readonly ObjectCollection<DocumentTemplate> m_DocumentTemplateCollection = new ObjectCollection<DocumentTemplate>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DocumentTemplate", IsNullable = false)]
        public ObjectCollection<DocumentTemplate> DocumentTemplateCollection
        {
            get { return m_DocumentTemplateCollection; }
        }

        private readonly ObjectCollection<Topic> m_TopicCollection = new ObjectCollection<Topic>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Topic", IsNullable = false)]
        public ObjectCollection<Topic> TopicCollection
        {
            get { return m_TopicCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ContractTemplateId
        {
            set
            {
                if (m_contractTemplateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractTemplateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractTemplateId;
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

        public double InterestRate
        {
            set
            {
                if (Math.Abs(m_interestRate - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInterestRate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_interestRate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_interestRate;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DocumentTemplate", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DocumentTemplate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_storeId;
        public const string PropertyNameStoreId = "StoreId";


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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Contract", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Contract
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_contractId;
        public const string PropertyNameContractId = "ContractId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_referenceNo;
        public const string PropertyNameReferenceNo = "ReferenceNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_value;
        public const string PropertyNameValue = "Value";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_remarks;
        public const string PropertyNameRemarks = "Remarks";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_period;
        public const string PropertyNamePeriod = "Period";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_periodUnit;
        public const string PropertyNamePeriodUnit = "PeriodUnit";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_startDate;
        public const string PropertyNameStartDate = "StartDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_endDate;
        public const string PropertyNameEndDate = "EndDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_rentalApplicationId;
        public const string PropertyNameRentalApplicationId = "RentalApplicationId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_rentType;
        public const string PropertyNameRentType = "RentType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_interestRate;
        public const string PropertyNameInterestRate = "InterestRate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isEnd;
        public const string PropertyNameIsEnd = "IsEnd";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_option;
        public const string PropertyNameOption = "Option";



        private readonly ObjectCollection<Document> m_DocumentCollection = new ObjectCollection<Document>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Document", IsNullable = false)]
        public ObjectCollection<Document> DocumentCollection
        {
            get { return m_DocumentCollection; }
        }

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ContractingParty m_contractingParty
                = new ContractingParty();

        public const string PropertyNameContractingParty = "ContractingParty";
        [DebuggerHidden]

        public ContractingParty ContractingParty
        {
            get { return m_contractingParty; }
            set
            {
                m_contractingParty = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tenant m_tenant
                = new Tenant();

        public const string PropertyNameTenant = "Tenant";
        [DebuggerHidden]

        public Tenant Tenant
        {
            get { return m_tenant; }
            set
            {
                m_tenant = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Space m_space
                = new Space();

        public const string PropertyNameSpace = "Space";
        [DebuggerHidden]

        public Space Space
        {
            get { return m_space; }
            set
            {
                m_space = value;
                OnPropertyChanged();
            }
        }

        private readonly ObjectCollection<Topic> m_TopicCollection = new ObjectCollection<Topic>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Topic", IsNullable = false)]
        public ObjectCollection<Topic> TopicCollection
        {
            get { return m_TopicCollection; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Termination m_termination
                = new Termination();

        public const string PropertyNameTermination = "Termination";
        [DebuggerHidden]

        public Termination Termination
        {
            get { return m_termination; }
            set
            {
                m_termination = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Extension m_extension
                = new Extension();

        public const string PropertyNameExtension = "Extension";
        [DebuggerHidden]

        public Extension Extension
        {
            get { return m_extension; }
            set
            {
                m_extension = value;
                OnPropertyChanged();
            }
        }

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ContractId
        {
            set
            {
                if (m_contractId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ReferenceNo
        {
            set
            {
                if (String.Equals(m_referenceNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReferenceNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_referenceNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_referenceNo;
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

        public decimal Value
        {
            set
            {
                if (m_value == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_value = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_value;
            }
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

        public string Remarks
        {
            set
            {
                if (String.Equals(m_remarks, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemarks, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_remarks = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_remarks;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Period
        {
            set
            {
                if (m_period == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_period = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_period;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PeriodUnit
        {
            set
            {
                if (String.Equals(m_periodUnit, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriodUnit, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_periodUnit = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_periodUnit;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime StartDate
        {
            set
            {
                if (m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_startDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime EndDate
        {
            set
            {
                if (m_endDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_endDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_endDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int RentalApplicationId
        {
            set
            {
                if (m_rentalApplicationId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentalApplicationId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rentalApplicationId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rentalApplicationId;
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

        public string RentType
        {
            set
            {
                if (String.Equals(m_rentType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rentType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rentType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public double InterestRate
        {
            set
            {
                if (Math.Abs(m_interestRate - value) < 0.00001d) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInterestRate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_interestRate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_interestRate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsEnd
        {
            set
            {
                if (m_isEnd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsEnd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isEnd = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isEnd;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Option
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
            get { return m_option; }
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
    [XmlType("Offer", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Offer
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_businessPlan;
        public const string PropertyNameBusinessPlan = "BusinessPlan";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_spaceId;
        public const string PropertyNameSpaceId = "SpaceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_building;
        public const string PropertyNameBuilding = "Building";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_floor;
        public const string PropertyNameFloor = "Floor";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_deposit;
        public const string PropertyNameDeposit = "Deposit";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_rent;
        public const string PropertyNameRent = "Rent";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_expiryDate;
        public const string PropertyNameExpiryDate = "ExpiryDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_period;
        public const string PropertyNamePeriod = "Period";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_periodUnit;
        public const string PropertyNamePeriodUnit = "PeriodUnit";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_option;
        public const string PropertyNameOption = "Option";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_businessPlanText;
        public const string PropertyNameBusinessPlanText = "BusinessPlanText";



        private readonly ObjectCollection<OfferCondition> m_OfferConditionCollection = new ObjectCollection<OfferCondition>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("OfferCondition", IsNullable = false)]
        public ObjectCollection<OfferCondition> OfferConditionCollection
        {
            get { return m_OfferConditionCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string BusinessPlan
        {
            set
            {
                if (String.Equals(m_businessPlan, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBusinessPlan, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_businessPlan = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_businessPlan;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int SpaceId
        {
            set
            {
                if (m_spaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSpaceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_spaceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_spaceId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Size
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
            get
            {
                return m_size;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Building
        {
            set
            {
                if (String.Equals(m_building, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuilding, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_building = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_building;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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

        [Required]

        [DebuggerHidden]

        public decimal Deposit
        {
            set
            {
                if (m_deposit == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDeposit, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_deposit = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_deposit;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Rent
        {
            set
            {
                if (m_rent == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRent, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rent = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rent;
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

        public DateTime ExpiryDate
        {
            set
            {
                if (m_expiryDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpiryDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_expiryDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_expiryDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Period
        {
            set
            {
                if (m_period == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_period = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_period;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PeriodUnit
        {
            set
            {
                if (String.Equals(m_periodUnit, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriodUnit, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_periodUnit = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_periodUnit;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string BusinessPlanText
        {
            set
            {
                if (String.Equals(m_businessPlanText, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBusinessPlanText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_businessPlanText = value;
                    OnPropertyChanged();
                }
            }
            get { return m_businessPlanText; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("OfferCondition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class OfferCondition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsRequired
        {
            set
            {
                if (m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequired = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequired;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Topic", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Topic
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_text;
        public const string PropertyNameText = "Text";


        private readonly ObjectCollection<Clause> m_ClauseCollection = new ObjectCollection<Clause>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Clause", IsNullable = false)]
        public ObjectCollection<Clause> ClauseCollection
        {
            get { return m_ClauseCollection; }
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

        public string Text
        {
            set
            {
                if (String.Equals(m_text, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_text = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_text;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Clause", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Clause
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_no;
        public const string PropertyNameNo = "No";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_text;
        public const string PropertyNameText = "Text";


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

        [Required]

        [DebuggerHidden]

        public string Text
        {
            set
            {
                if (String.Equals(m_text, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_text = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_text;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ContractingParty", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ContractingParty
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Contact m_contact
                = new Contact();

        public const string PropertyNameContact = "Contact";
        [DebuggerHidden]

        public Contact Contact
        {
            get { return m_contact; }
            set
            {
                m_contact = value;
                OnPropertyChanged();
            }
        }

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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DepositPayment", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DepositPayment
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_depositPaymentId;
        public const string PropertyNameDepositPaymentId = "DepositPaymentId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_receiptNo;
        public const string PropertyNameReceiptNo = "ReceiptNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_amount;
        public const string PropertyNameAmount = "Amount";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int DepositPaymentId
        {
            set
            {
                if (m_depositPaymentId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDepositPaymentId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_depositPaymentId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_depositPaymentId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string ReceiptNo
        {
            set
            {
                if (String.Equals(m_receiptNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceiptNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_receiptNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_receiptNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Amount
        {
            set
            {
                if (m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_amount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_amount;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Payment", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Payment
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_paymentId;
        public const string PropertyNamePaymentId = "PaymentId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_amount;
        public const string PropertyNameAmount = "Amount";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_contractNo;
        public const string PropertyNameContractNo = "ContractNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_tenantIdSsmNo;
        public const string PropertyNameTenantIdSsmNo = "TenantIdSsmNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_receiptNo;
        public const string PropertyNameReceiptNo = "ReceiptNo";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int PaymentId
        {
            set
            {
                if (m_paymentId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePaymentId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_paymentId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_paymentId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Amount
        {
            set
            {
                if (m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_amount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_amount;
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

        public string ContractNo
        {
            set
            {
                if (String.Equals(m_contractNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string TenantIdSsmNo
        {
            set
            {
                if (String.Equals(m_tenantIdSsmNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTenantIdSsmNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tenantIdSsmNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tenantIdSsmNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ReceiptNo
        {
            set
            {
                if (String.Equals(m_receiptNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceiptNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_receiptNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_receiptNo;
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
        private string m_username;
        public const string PropertyNameUsername = "Username";


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

        public string Username
        {
            set
            {
                if (String.Equals(m_username, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUsername, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_username = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_username;
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
    [XmlType("Deposit", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Deposit
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_depositId;
        public const string PropertyNameDepositId = "DepositId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_dateTime;
        public const string PropertyNameDateTime = "DateTime";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iDNumber;
        public const string PropertyNameIDNumber = "IDNumber";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_registrationNo;
        public const string PropertyNameRegistrationNo = "RegistrationNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_amount;
        public const string PropertyNameAmount = "Amount";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPaid;
        public const string PropertyNameIsPaid = "IsPaid";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRefund;
        public const string PropertyNameIsRefund = "IsRefund";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_receiptNo;
        public const string PropertyNameReceiptNo = "ReceiptNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_refundedBy;
        public const string PropertyNameRefundedBy = "RefundedBy";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isVoid;
        public const string PropertyNameIsVoid = "IsVoid";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_paymentDateTime;
        public const string PropertyNamePaymentDateTime = "PaymentDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_refundDateTime;
        public const string PropertyNameRefundDateTime = "RefundDateTime";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_dueDate;
        public const string PropertyNameDueDate = "DueDate";



        private readonly ObjectCollection<DepositPayment> m_DepositPaymentCollection = new ObjectCollection<DepositPayment>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DepositPayment", IsNullable = false)]
        public ObjectCollection<DepositPayment> DepositPaymentCollection
        {
            get { return m_DepositPaymentCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int DepositId
        {
            set
            {
                if (m_depositId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDepositId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_depositId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_depositId;
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

        public string IDNumber
        {
            set
            {
                if (String.Equals(m_iDNumber, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIDNumber, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iDNumber = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iDNumber;
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

        public decimal Amount
        {
            set
            {
                if (m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_amount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_amount;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsPaid
        {
            set
            {
                if (m_isPaid == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPaid, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPaid = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPaid;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsRefund
        {
            set
            {
                if (m_isRefund == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRefund, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRefund = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRefund;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string ReceiptNo
        {
            set
            {
                if (String.Equals(m_receiptNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceiptNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_receiptNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_receiptNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public string RefundedBy
        {
            set
            {
                if (String.Equals(m_refundedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRefundedBy, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_refundedBy = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_refundedBy;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsVoid
        {
            set
            {
                if (m_isVoid == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsVoid, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isVoid = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isVoid;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? PaymentDateTime
        {
            set
            {
                if (m_paymentDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePaymentDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_paymentDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_paymentDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? RefundDateTime
        {
            set
            {
                if (m_refundDateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRefundDateTime, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_refundDateTime = value;
                    OnPropertyChanged();
                }
            }
            get { return m_refundDateTime; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? DueDate
        {
            set
            {
                if (m_dueDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDueDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dueDate = value;
                    OnPropertyChanged();
                }
            }
            get { return m_dueDate; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Reply", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Reply
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_text;
        public const string PropertyNameText = "Text";


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

        public string Text
        {
            set
            {
                if (String.Equals(m_text, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_text = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_text;
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
        private string m_username;
        public const string PropertyNameUsername = "Username";


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

        public string Username
        {
            set
            {
                if (String.Equals(m_username, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUsername, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_username = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_username;
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
    [XmlType("Rebate", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Rebate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_rebateId;
        public const string PropertyNameRebateId = "RebateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_contractNo;
        public const string PropertyNameContractNo = "ContractNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_amount;
        public const string PropertyNameAmount = "Amount";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_startDate;
        public const string PropertyNameStartDate = "StartDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_endDate;
        public const string PropertyNameEndDate = "EndDate";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int RebateId
        {
            set
            {
                if (m_rebateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRebateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rebateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rebateId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ContractNo
        {
            set
            {
                if (String.Equals(m_contractNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractNo;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Amount
        {
            set
            {
                if (m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_amount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_amount;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime StartDate
        {
            set
            {
                if (m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_startDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime EndDate
        {
            set
            {
                if (m_endDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_endDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_endDate;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Interest", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Interest
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_building;
        public const string PropertyNameBuilding = "Building";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_spaceCategory;
        public const string PropertyNameSpaceCategory = "SpaceCategory";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_percentage;
        public const string PropertyNamePercentage = "Percentage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_period;
        public const string PropertyNamePeriod = "Period";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_periodType;
        public const string PropertyNamePeriodType = "PeriodType";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Building
        {
            set
            {
                if (String.Equals(m_building, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuilding, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_building = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_building;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string SpaceCategory
        {
            set
            {
                if (String.Equals(m_spaceCategory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSpaceCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_spaceCategory = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_spaceCategory;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Percentage
        {
            set
            {
                if (m_percentage == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePercentage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_percentage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_percentage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Period
        {
            set
            {
                if (m_period == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_period = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_period;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PeriodType
        {
            set
            {
                if (String.Equals(m_periodType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriodType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_periodType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_periodType;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Rent", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Rent
    {

        private string m_Half;
        [XmlAttribute]
        public string Half
        {
            get
            {
                return m_Half;
            }
            set
            {
                m_Half = value;
                RaisePropertyChanged();
            }
        }


        private int m_Year;
        [XmlAttribute]
        public int Year
        {
            get
            {
                return m_Year;
            }
            set
            {
                m_Year = value;
                RaisePropertyChanged();
            }
        }


        public int? Month { get; set; }

        public int? Quarter { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tenant m_tenant
                = new Tenant();

        public const string PropertyNameTenant = "Tenant";
        [DebuggerHidden]

        public Tenant Tenant
        {
            get { return m_tenant; }
            set
            {
                m_tenant = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("AdhocInvoice", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class AdhocInvoice
    {

        private string m_Category;
        [XmlAttribute]
        public string Category
        {
            get
            {
                return m_Category;
            }
            set
            {
                m_Category = value;
                RaisePropertyChanged();
            }
        }


        private int m_F2;
        [XmlAttribute]
        public int F2
        {
            get
            {
                return m_F2;
            }
            set
            {
                m_F2 = value;
                RaisePropertyChanged();
            }
        }


        public DateTime? SentDate { get; set; }

        public InvoiceType Type2 { get; set; }

        public string Note2 { get; set; }

        private readonly ObjectCollection<InvoiceItem> m_InvoiceItemCollection = new ObjectCollection<InvoiceItem>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("InvoiceItem", IsNullable = false)]
        public ObjectCollection<InvoiceItem> InvoiceItemCollection
        {
            get { return m_InvoiceItemCollection; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tenant m_tenant
                = new Tenant();

        public const string PropertyNameTenant = "Tenant";
        [DebuggerHidden]

        public Tenant Tenant
        {
            get { return m_tenant; }
            set
            {
                m_tenant = value;
                OnPropertyChanged();
            }
        }

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

        private readonly ObjectCollection<Document> m_DocumentCollection = new ObjectCollection<Document>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Document", IsNullable = false)]
        public ObjectCollection<Document> DocumentCollection
        {
            get { return m_DocumentCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("InvoiceItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class InvoiceItem
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_amount;
        public const string PropertyNameAmount = "Amount";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Amount
        {
            set
            {
                if (m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_amount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_amount;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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
    [XmlType("State", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class State
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Complaint", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Complaint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_complaintId;
        public const string PropertyNameComplaintId = "ComplaintId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_templateId;
        public const string PropertyNameTemplateId = "TemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_remarks;
        public const string PropertyNameRemarks = "Remarks";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_tenantId;
        public const string PropertyNameTenantId = "TenantId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_space;
        public const string PropertyNameSpace = "Space";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_subCategory;
        public const string PropertyNameSubCategory = "SubCategory";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_referenceNo;
        public const string PropertyNameReferenceNo = "ReferenceNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_attachmentStoreId;
        public const string PropertyNameAttachmentStoreId = "AttachmentStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_department;
        public const string PropertyNameDepartment = "Department";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tenant m_tenant
                = new Tenant();

        public const string PropertyNameTenant = "Tenant";
        [DebuggerHidden]

        public Tenant Tenant
        {
            get { return m_tenant; }
            set
            {
                m_tenant = value;
                OnPropertyChanged();
            }
        }

        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
        }

        private readonly ObjectCollection<CustomListValue> m_CustomListValueCollection = new ObjectCollection<CustomListValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomListValue", IsNullable = false)]
        public ObjectCollection<CustomListValue> CustomListValueCollection
        {
            get { return m_CustomListValueCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ComplaintId
        {
            set
            {
                if (m_complaintId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComplaintId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_complaintId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_complaintId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TemplateId
        {
            set
            {
                if (m_templateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Remarks
        {
            set
            {
                if (String.Equals(m_remarks, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemarks, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_remarks = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_remarks;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TenantId
        {
            set
            {
                if (m_tenantId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTenantId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tenantId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tenantId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Space
        {
            set
            {
                if (String.Equals(m_space, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSpace, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_space = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_space;
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

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string SubCategory
        {
            set
            {
                if (String.Equals(m_subCategory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_subCategory = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_subCategory;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ReferenceNo
        {
            set
            {
                if (String.Equals(m_referenceNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReferenceNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_referenceNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_referenceNo;
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

        public string AttachmentStoreId
        {
            set
            {
                if (String.Equals(m_attachmentStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAttachmentStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_attachmentStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_attachmentStoreId;
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
    [XmlType("CustomFieldValue", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CustomFieldValue
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_value;
        public const string PropertyNameValue = "Value";


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
            get
            {
                return m_value;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Inspection", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Inspection
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_personInCharge;
        public const string PropertyNamePersonInCharge = "PersonInCharge";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_assignedDate;
        public const string PropertyNameAssignedDate = "AssignedDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_remark;
        public const string PropertyNameRemark = "Remark";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_inspectionDate;
        public const string PropertyNameInspectionDate = "InspectionDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_resolution;
        public const string PropertyNameResolution = "Resolution";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_observation;
        public const string PropertyNameObservation = "Observation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_contractor;
        public const string PropertyNameContractor = "Contractor";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_priority;
        public const string PropertyNamePriority = "Priority";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_severity;
        public const string PropertyNameSeverity = "Severity";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string PersonInCharge
        {
            set
            {
                if (String.Equals(m_personInCharge, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePersonInCharge, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_personInCharge = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_personInCharge;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime AssignedDate
        {
            set
            {
                if (m_assignedDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAssignedDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_assignedDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_assignedDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Remark
        {
            set
            {
                if (String.Equals(m_remark, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemark, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_remark = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_remark;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime InspectionDate
        {
            set
            {
                if (m_inspectionDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInspectionDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inspectionDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_inspectionDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Resolution
        {
            set
            {
                if (String.Equals(m_resolution, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameResolution, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_resolution = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_resolution;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Observation
        {
            set
            {
                if (String.Equals(m_observation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameObservation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_observation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_observation;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Contractor
        {
            set
            {
                if (String.Equals(m_contractor, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractor, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractor = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractor;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Priority
        {
            set
            {
                if (String.Equals(m_priority, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePriority, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_priority = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_priority;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Severity
        {
            set
            {
                if (String.Equals(m_severity, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSeverity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_severity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_severity;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Maintenance", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Maintenance
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_maintenanceId;
        public const string PropertyNameMaintenanceId = "MaintenanceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_complaintId;
        public const string PropertyNameComplaintId = "ComplaintId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workOrderNo;
        public const string PropertyNameWorkOrderNo = "WorkOrderNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_department;
        public const string PropertyNameDepartment = "Department";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_resolution;
        public const string PropertyNameResolution = "Resolution";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_officer;
        public const string PropertyNameOfficer = "Officer";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_attachmentStoreId;
        public const string PropertyNameAttachmentStoreId = "AttachmentStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_attachmentName;
        public const string PropertyNameAttachmentName = "AttachmentName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workOrderType;
        public const string PropertyNameWorkOrderType = "WorkOrderType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_templateId;
        public const string PropertyNameTemplateId = "TemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_startDate;
        public const string PropertyNameStartDate = "StartDate";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_endDate;
        public const string PropertyNameEndDate = "EndDate";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Complaint m_complaint
                = new Complaint();

        public const string PropertyNameComplaint = "Complaint";
        [DebuggerHidden]

        public Complaint Complaint
        {
            get { return m_complaint; }
            set
            {
                m_complaint = value;
                OnPropertyChanged();
            }
        }

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
        }

        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
        }

        private readonly ObjectCollection<CustomListValue> m_CustomListValueCollection = new ObjectCollection<CustomListValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomListValue", IsNullable = false)]
        public ObjectCollection<CustomListValue> CustomListValueCollection
        {
            get { return m_CustomListValueCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int MaintenanceId
        {
            set
            {
                if (m_maintenanceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaintenanceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_maintenanceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_maintenanceId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ComplaintId
        {
            set
            {
                if (m_complaintId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComplaintId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_complaintId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_complaintId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string WorkOrderNo
        {
            set
            {
                if (String.Equals(m_workOrderNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWorkOrderNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_workOrderNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_workOrderNo;
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

        public string Resolution
        {
            set
            {
                if (String.Equals(m_resolution, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameResolution, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_resolution = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_resolution;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Officer
        {
            set
            {
                if (String.Equals(m_officer, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOfficer, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_officer = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_officer;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string AttachmentStoreId
        {
            set
            {
                if (String.Equals(m_attachmentStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAttachmentStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_attachmentStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_attachmentStoreId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string AttachmentName
        {
            set
            {
                if (String.Equals(m_attachmentName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAttachmentName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_attachmentName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_attachmentName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string WorkOrderType
        {
            set
            {
                if (String.Equals(m_workOrderType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWorkOrderType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_workOrderType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_workOrderType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TemplateId
        {
            set
            {
                if (m_templateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateId;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? StartDate
        {
            set
            {
                if (m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startDate = value;
                    OnPropertyChanged();
                }
            }
            get { return m_startDate; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? EndDate
        {
            set
            {
                if (m_endDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_endDate = value;
                    OnPropertyChanged();
                }
            }
            get { return m_endDate; }
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
    [XmlType("WorkOrder", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WorkOrder
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_workOrderId;
        public const string PropertyNameWorkOrderId = "WorkOrderId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_priority;
        public const string PropertyNamePriority = "Priority";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_severity;
        public const string PropertyNameSeverity = "Severity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_estimationCost;
        public const string PropertyNameEstimationCost = "EstimationCost";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_no;
        public const string PropertyNameNo = "No";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_maintenanceId;
        public const string PropertyNameMaintenanceId = "MaintenanceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_templateId;
        public const string PropertyNameTemplateId = "TemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_status;
        public const string PropertyNameStatus = "Status";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_resolution;
        public const string PropertyNameResolution = "Resolution";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_officer;
        public const string PropertyNameOfficer = "Officer";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_department;
        public const string PropertyNameDepartment = "Department";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_startDate;
        public const string PropertyNameStartDate = "StartDate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_endDate;
        public const string PropertyNameEndDate = "EndDate";


        private readonly ObjectCollection<Comment> m_CommentCollection = new ObjectCollection<Comment>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Comment", IsNullable = false)]
        public ObjectCollection<Comment> CommentCollection
        {
            get { return m_CommentCollection; }
        }

        private readonly ObjectCollection<PartsAndLabor> m_PartsAndLaborCollection = new ObjectCollection<PartsAndLabor>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("PartsAndLabor", IsNullable = false)]
        public ObjectCollection<PartsAndLabor> PartsAndLaborCollection
        {
            get { return m_PartsAndLaborCollection; }
        }

        private readonly ObjectCollection<Warranty> m_WarrantyCollection = new ObjectCollection<Warranty>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Warranty", IsNullable = false)]
        public ObjectCollection<Warranty> WarrantyCollection
        {
            get { return m_WarrantyCollection; }
        }

        private readonly ObjectCollection<NonCompliance> m_NonComplianceCollection = new ObjectCollection<NonCompliance>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("NonCompliance", IsNullable = false)]
        public ObjectCollection<NonCompliance> NonComplianceCollection
        {
            get { return m_NonComplianceCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int WorkOrderId
        {
            set
            {
                if (m_workOrderId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWorkOrderId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_workOrderId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_workOrderId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Priority
        {
            set
            {
                if (String.Equals(m_priority, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePriority, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_priority = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_priority;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Severity
        {
            set
            {
                if (String.Equals(m_severity, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSeverity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_severity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_severity;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal EstimationCost
        {
            set
            {
                if (m_estimationCost == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEstimationCost, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_estimationCost = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_estimationCost;
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

        [Required]

        [DebuggerHidden]

        public int MaintenanceId
        {
            set
            {
                if (m_maintenanceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaintenanceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_maintenanceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_maintenanceId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int TemplateId
        {
            set
            {
                if (m_templateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_templateId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_templateId;
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

        public string Resolution
        {
            set
            {
                if (String.Equals(m_resolution, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameResolution, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_resolution = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_resolution;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Officer
        {
            set
            {
                if (String.Equals(m_officer, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOfficer, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_officer = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_officer;
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

        public DateTime StartDate
        {
            set
            {
                if (m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_startDate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime EndDate
        {
            set
            {
                if (m_endDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_endDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_endDate;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Comment", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Comment
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_user;
        public const string PropertyNameUser = "User";


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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("PartsAndLabor", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class PartsAndLabor
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_quantity;
        public const string PropertyNameQuantity = "Quantity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_cost;
        public const string PropertyNameCost = "Cost";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_total;
        public const string PropertyNameTotal = "Total";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Quantity
        {
            set
            {
                if (m_quantity == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_quantity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_quantity;
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

        public decimal Cost
        {
            set
            {
                if (m_cost == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCost, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cost = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_cost;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public decimal Total
        {
            set
            {
                if (m_total == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotal, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_total = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_total;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Warranty", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Warranty
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_yearWarranty;
        public const string PropertyNameYearWarranty = "YearWarranty";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_startDate;
        public const string PropertyNameStartDate = "StartDate";


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

        public string YearWarranty
        {
            set
            {
                if (String.Equals(m_yearWarranty, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameYearWarranty, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_yearWarranty = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_yearWarranty;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public DateTime StartDate
        {
            set
            {
                if (m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_startDate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_startDate;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("NonCompliance", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class NonCompliance
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_reason;
        public const string PropertyNameReason = "Reason";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_action;
        public const string PropertyNameAction = "Action";


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

        public string Reason
        {
            set
            {
                if (String.Equals(m_reason, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReason, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reason = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reason;
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
    [XmlType("Inventory", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Inventory
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_brand;
        public const string PropertyNameBrand = "Brand";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_specification;
        public const string PropertyNameSpecification = "Specification";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_quantity;
        public const string PropertyNameQuantity = "Quantity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_inventoryId;
        public const string PropertyNameInventoryId = "InventoryId";


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

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Brand
        {
            set
            {
                if (String.Equals(m_brand, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBrand, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_brand = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_brand;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Specification
        {
            set
            {
                if (String.Equals(m_specification, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSpecification, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_specification = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_specification;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Quantity
        {
            set
            {
                if (m_quantity == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_quantity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_quantity;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int InventoryId
        {
            set
            {
                if (m_inventoryId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInventoryId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inventoryId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_inventoryId;
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
    [XmlType("Termination", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Termination
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_remarks;
        public const string PropertyNameRemarks = "Remarks";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_approvalOfficer;
        public const string PropertyNameApprovalOfficer = "ApprovalOfficer";


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

        public string Remarks
        {
            set
            {
                if (String.Equals(m_remarks, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemarks, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_remarks = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_remarks;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string ApprovalOfficer
        {
            set
            {
                if (String.Equals(m_approvalOfficer, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApprovalOfficer, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_approvalOfficer = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_approvalOfficer;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Extension", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Extension
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_remarks;
        public const string PropertyNameRemarks = "Remarks";


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

        public string Remarks
        {
            set
            {
                if (String.Equals(m_remarks, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemarks, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_remarks = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_remarks;
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
    [XmlType("CustomListValue", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CustomListValue
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_label;
        public const string PropertyNameLabel = "Label";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_minOccurence;
        public const string PropertyNameMinOccurence = "MinOccurence";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_maxOccurence;
        public const string PropertyNameMaxOccurence = "MaxOccurence";



        private readonly ObjectCollection<CustomListRow> m_CustomListRowCollection = new ObjectCollection<CustomListRow>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomListRow", IsNullable = false)]
        public ObjectCollection<CustomListRow> CustomListRowCollection
        {
            get { return m_CustomListRowCollection; }
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

        public string Label
        {
            set
            {
                if (String.Equals(m_label, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_label = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_label;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int MinOccurence
        {
            set
            {
                if (m_minOccurence == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMinOccurence, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_minOccurence = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_minOccurence;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? MaxOccurence
        {
            set
            {
                if (m_maxOccurence == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaxOccurence, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_maxOccurence = value;
                    OnPropertyChanged();
                }
            }
            get { return m_maxOccurence; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CustomListRow", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CustomListRow
    {

        private readonly ObjectCollection<CustomFieldValue> m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CustomFieldValue", IsNullable = false)]
        public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
        {
            get { return m_CustomFieldValueCollection; }
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
    [XmlType("MarketEvaluation", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class MarketEvaluation
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_date;
        public const string PropertyNameDate = "Date";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_value;
        public const string PropertyNameValue = "Value";


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

        public decimal Value
        {
            set
            {
                if (m_value == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_value = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_value;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Feature", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Feature
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_occurence;
        public const string PropertyNameOccurence = "Occurence";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_occurenceTimeSpan;
        public const string PropertyNameOccurenceTimeSpan = "OccurenceTimeSpan";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_quantity;
        public const string PropertyNameQuantity = "Quantity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal? m_charge;
        public const string PropertyNameCharge = "Charge";



        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
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

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsRequired
        {
            set
            {
                if (m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequired = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequired;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Occurence
        {
            set
            {
                if (m_occurence == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOccurence, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_occurence = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_occurence;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string OccurenceTimeSpan
        {
            set
            {
                if (String.Equals(m_occurenceTimeSpan, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOccurenceTimeSpan, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_occurenceTimeSpan = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_occurenceTimeSpan;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Quantity
        {
            set
            {
                if (m_quantity == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_quantity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_quantity;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public decimal? Charge
        {
            set
            {
                if (m_charge == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCharge, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_charge = value;
                    OnPropertyChanged();
                }
            }
            get { return m_charge; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FeatureDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FeatureDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_occurence;
        public const string PropertyNameOccurence = "Occurence";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_occurenceTimeSpan;
        public const string PropertyNameOccurenceTimeSpan = "OccurenceTimeSpan";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_availableQuantity;
        public const string PropertyNameAvailableQuantity = "AvailableQuantity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_charge;
        public const string PropertyNameCharge = "Charge";



        private readonly ObjectCollection<Photo> m_PhotoCollection = new ObjectCollection<Photo>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Photo", IsNullable = false)]
        public ObjectCollection<Photo> PhotoCollection
        {
            get { return m_PhotoCollection; }
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

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsRequired
        {
            set
            {
                if (m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequired = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequired;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Occurence
        {
            set
            {
                if (m_occurence == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOccurence, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_occurence = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_occurence;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string OccurenceTimeSpan
        {
            set
            {
                if (String.Equals(m_occurenceTimeSpan, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOccurenceTimeSpan, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_occurenceTimeSpan = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_occurenceTimeSpan;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int AvailableQuantity
        {
            set
            {
                if (m_availableQuantity == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAvailableQuantity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_availableQuantity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_availableQuantity;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public decimal Charge
        {
            set
            {
                if (m_charge == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCharge, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_charge = value;
                    OnPropertyChanged();
                }
            }
            get { return m_charge; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Ownership", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Ownership
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_ownershipNo;
        public const string PropertyNameOwnershipNo = "OwnershipNo";


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

        public string OwnershipNo
        {
            set
            {
                if (String.Equals(m_ownershipNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOwnershipNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_ownershipNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_ownershipNo;
            }
        }



    }

    [XmlType("Invoice", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Invoice
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int m_invoiceId;
        public const string PropertyNameInvoiceId = "InvoiceId";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private DateTime m_date;
        public const string PropertyNameDate = "Date";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private decimal m_amount;
        public const string PropertyNameAmount = "Amount";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_invoiceNo;
        public const string PropertyNameInvoiceNo = "InvoiceNo";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private InvoiceType m_type;
        public const string PropertyNameType = "Type";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_contractNo;
        public const string PropertyNameContractNo = "ContractNo";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_tenantIdSsmNo;
        public const string PropertyNameTenantIdSsmNo = "TenantIdSsmNo";


        // public properties members



        [XmlAttribute]
        public int InvoiceId
        {
            set
            {
                if (m_invoiceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInvoiceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_invoiceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_invoiceId;
            }
        }



        [XmlAttribute]
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



        [XmlAttribute]
        public decimal Amount
        {
            set
            {
                if (m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_amount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_amount;
            }
        }



        [XmlAttribute]
        public string InvoiceNo
        {
            set
            {
                if (m_invoiceNo == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInvoiceNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_invoiceNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_invoiceNo;
            }
        }



        [XmlAttribute]
        public InvoiceType Type
        {
            set
            {
                if (m_type == value) return;
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



        [XmlAttribute]
        public string ContractNo
        {
            set
            {
                if (m_contractNo == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContractNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_contractNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_contractNo;
            }
        }



        [XmlAttribute]
        public string TenantIdSsmNo
        {
            set
            {
                if (m_tenantIdSsmNo == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTenantIdSsmNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tenantIdSsmNo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tenantIdSsmNo;
            }
        }



    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvoiceType
    {
        AdhocInvoice,
        Rental,
    }

}
// ReSharper restore InconsistentNaming

