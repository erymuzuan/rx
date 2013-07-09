﻿
using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Bespoke.SphCommercialSpaces.Domain
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
                    OnPropertyChanged(PropertyNameContractNo);
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
                    OnPropertyChanged(PropertyNameDateFrom);
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
                    OnPropertyChanged(PropertyNameDateStart);
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
                    OnPropertyChanged(PropertyNameDateEnd);
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
                OnPropertyChanged(PropertyNameAddress);
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
                    OnPropertyChanged(PropertyNameTenantId);
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
                    OnPropertyChanged(PropertyNameIdSsmNo);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameBussinessType);
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
                    OnPropertyChanged(PropertyNamePhone);
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
                    OnPropertyChanged(PropertyNameFax);
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
                    OnPropertyChanged(PropertyNameMobilePhone);
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
                    OnPropertyChanged(PropertyNameEmail);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
    [XmlType("Land", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Land
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_lot;
        public const string PropertyNameLot = "Lot";


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
        private decimal m_currentMarketValue;
        public const string PropertyNameCurrentMarketValue = "CurrentMarketValue";


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
                OnPropertyChanged(PropertyNameOwner);
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
                OnPropertyChanged(PropertyNameAddress);
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Lot
        {
            set
            {
                if (String.Equals(m_lot, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLot, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lot = value;
                    OnPropertyChanged(PropertyNameLot);
                }
            }
            get
            {
                return m_lot;
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameLocation);
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
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged(PropertyNameSize);
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
                    OnPropertyChanged(PropertyNameSizeUnit);
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

        [Required]

        [DebuggerHidden]

        public decimal CurrentMarketValue
        {
            set
            {
                if (m_currentMarketValue == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCurrentMarketValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_currentMarketValue = value;
                    OnPropertyChanged(PropertyNameCurrentMarketValue);
                }
            }
            get
            {
                return m_currentMarketValue;
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
                    OnPropertyChanged(PropertyNameRezabNo);
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
                    OnPropertyChanged(PropertyNameSheetNo);
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
                    OnPropertyChanged(PropertyNameApprovedPlanNo);
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
                    OnPropertyChanged(PropertyNameLandOffice);
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
                    OnPropertyChanged(PropertyNameUsage);
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
                    OnPropertyChanged(PropertyNameNote);
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
                    OnPropertyChanged(PropertyNameOwnLevel);
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
                    OnPropertyChanged(PropertyNamePlanNo);
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
                    OnPropertyChanged(PropertyNameStatus);
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
                    OnPropertyChanged(PropertyNameIsApproved);
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
                    OnPropertyChanged(PropertyNameLeaseExpiryDate);
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
                    OnPropertyChanged(PropertyNameLeasePeriod);
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
                    OnPropertyChanged(PropertyNameApprovedDateTime);
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
                    OnPropertyChanged(PropertyNameApprovedBy);
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
        private int m_buildingId;
        public const string PropertyNameBuildingId = "BuildingId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_lotNo;
        public const string PropertyNameLotNo = "LotNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


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
        private int? m_height;
        public const string PropertyNameHeight = "Height";



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
                OnPropertyChanged(PropertyNameAddress);
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
                    OnPropertyChanged(PropertyNameBuildingId);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameLotNo);
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

        public double Size
        {
            set
            {
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged(PropertyNameSize);
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
                    OnPropertyChanged(PropertyNameStatus);
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
                    OnPropertyChanged(PropertyNameFloors);
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
                    OnPropertyChanged(PropertyNameNote);
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
                    OnPropertyChanged(PropertyNameHeight);
                }
            }
            get { return m_height; }
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
                if (m_lat == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLat, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lat = value;
                    OnPropertyChanged(PropertyNameLat);
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
                if (m_lng == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLng, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lng = value;
                    OnPropertyChanged(PropertyNameLng);
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
                if (m_elevation == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameElevation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_elevation = value;
                    OnPropertyChanged(PropertyNameElevation);
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
                    OnPropertyChanged(PropertyNameUnitNo);
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
                    OnPropertyChanged(PropertyNameFloor);
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
                    OnPropertyChanged(PropertyNameBlock);
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
                    OnPropertyChanged(PropertyNameStreet);
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
                    OnPropertyChanged(PropertyNameCity);
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
                    OnPropertyChanged(PropertyNamePostcode);
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
                    OnPropertyChanged(PropertyNameState);
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
                    OnPropertyChanged(PropertyNameCountry);
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
        private int m_number;
        public const string PropertyNameNumber = "Number";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        private readonly ObjectCollection<Lot> m_LotCollection = new ObjectCollection<Lot>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Lot", IsNullable = false)]
        public ObjectCollection<Lot> LotCollection
        {
            get { return m_LotCollection; }
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
                    OnPropertyChanged(PropertyNameName);
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
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged(PropertyNameSize);
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

        public int Number
        {
            set
            {
                if (m_number == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNumber, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_number = value;
                    OnPropertyChanged(PropertyNameNumber);
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
                    OnPropertyChanged(PropertyNameNote);
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
    [XmlType("Lot", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Lot
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_floorNo;
        public const string PropertyNameFloorNo = "FloorNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isCommercialSpace;
        public const string PropertyNameIsCommercialSpace = "IsCommercialSpace";


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
                    OnPropertyChanged(PropertyNameName);
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
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged(PropertyNameSize);
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
                    OnPropertyChanged(PropertyNameFloorNo);
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

        public bool IsCommercialSpace
        {
            set
            {
                if (m_isCommercialSpace == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCommercialSpace, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isCommercialSpace = value;
                    OnPropertyChanged(PropertyNameIsCommercialSpace);
                }
            }
            get
            {
                return m_isCommercialSpace;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CommercialSpace", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CommercialSpace
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_commercialSpaceId;
        public const string PropertyNameCommercialSpaceId = "CommercialSpaceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_buildingId;
        public const string PropertyNameBuildingId = "BuildingId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_lotName;
        public const string PropertyNameLotName = "LotName";


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
        private string m_buildingLot;
        public const string PropertyNameBuildingLot = "BuildingLot";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_rentalRate;
        public const string PropertyNameRentalRate = "RentalRate";


        private readonly ObjectCollection<Lot> m_LotCollection = new ObjectCollection<Lot>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Lot", IsNullable = false)]
        public ObjectCollection<Lot> LotCollection
        {
            get { return m_LotCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int CommercialSpaceId
        {
            set
            {
                if (m_commercialSpaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpaceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_commercialSpaceId = value;
                    OnPropertyChanged(PropertyNameCommercialSpaceId);
                }
            }
            get
            {
                return m_commercialSpaceId;
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
                    OnPropertyChanged(PropertyNameBuildingId);
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

        public string LotName
        {
            set
            {
                if (String.Equals(m_lotName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLotName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lotName = value;
                    OnPropertyChanged(PropertyNameLotName);
                }
            }
            get
            {
                return m_lotName;
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
                    OnPropertyChanged(PropertyNameFloorName);
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
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged(PropertyNameSize);
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
                    OnPropertyChanged(PropertyNameCategory);
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
                    OnPropertyChanged(PropertyNameRentalType);
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
                    OnPropertyChanged(PropertyNameIsOnline);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
                    OnPropertyChanged(PropertyNameIsAvailable);
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
                    OnPropertyChanged(PropertyNameContactPerson);
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
                    OnPropertyChanged(PropertyNameContactNo);
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
                    OnPropertyChanged(PropertyNameState);
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
                    OnPropertyChanged(PropertyNameCity);
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
                    OnPropertyChanged(PropertyNameBuildingName);
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

        public string BuildingLot
        {
            set
            {
                if (String.Equals(m_buildingLot, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingLot, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_buildingLot = value;
                    OnPropertyChanged(PropertyNameBuildingLot);
                }
            }
            get
            {
                return m_buildingLot;
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
                    OnPropertyChanged(PropertyNameRentalRate);
                }
            }
            get
            {
                return m_rentalRate;
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
        private int m_commercialSpaceId;
        public const string PropertyNameCommercialSpaceId = "CommercialSpaceId";


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
        private decimal m_currentYearSales;
        public const string PropertyNameCurrentYearSales = "CurrentYearSales";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_lastYearSales;
        public const string PropertyNameLastYearSales = "LastYearSales";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private decimal m_previousYearSales;
        public const string PropertyNamePreviousYearSales = "PreviousYearSales";



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
                OnPropertyChanged(PropertyNameAddress);
            }
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
                OnPropertyChanged(PropertyNameContact);
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
                OnPropertyChanged(PropertyNameOffer);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CommercialSpace m_commercialSpace
                = new CommercialSpace();

        public const string PropertyNameCommercialSpace = "CommercialSpace";
        [DebuggerHidden]

        public CommercialSpace CommercialSpace
        {
            get { return m_commercialSpace; }
            set
            {
                m_commercialSpace = value;
                OnPropertyChanged(PropertyNameCommercialSpace);
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
                    OnPropertyChanged(PropertyNameRentalApplicationId);
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
                    OnPropertyChanged(PropertyNameCompanyName);
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
                    OnPropertyChanged(PropertyNameCompanyRegistrationNo);
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
                    OnPropertyChanged(PropertyNameDateStart);
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
                    OnPropertyChanged(PropertyNameDateEnd);
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
                    OnPropertyChanged(PropertyNamePurpose);
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
                    OnPropertyChanged(PropertyNameCompanyType);
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

        public int CommercialSpaceId
        {
            set
            {
                if (m_commercialSpaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpaceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_commercialSpaceId = value;
                    OnPropertyChanged(PropertyNameCommercialSpaceId);
                }
            }
            get
            {
                return m_commercialSpaceId;
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
                    OnPropertyChanged(PropertyNameStatus);
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
                    OnPropertyChanged(PropertyNameExperience);
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
                    OnPropertyChanged(PropertyNameIsRecordExist);
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
                    OnPropertyChanged(PropertyNamePreviousAddress);
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
                    OnPropertyChanged(PropertyNameIsCompany);
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameRemarks);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
                    OnPropertyChanged(PropertyNameApplicationDate);
                }
            }
            get
            {
                return m_applicationDate;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public decimal CurrentYearSales
        {
            set
            {
                if (m_currentYearSales == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCurrentYearSales, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_currentYearSales = value;
                    OnPropertyChanged(PropertyNameCurrentYearSales);
                }
            }
            get { return m_currentYearSales; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public decimal LastYearSales
        {
            set
            {
                if (m_lastYearSales == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLastYearSales, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_lastYearSales = value;
                    OnPropertyChanged(PropertyNameLastYearSales);
                }
            }
            get { return m_lastYearSales; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public decimal PreviousYearSales
        {
            set
            {
                if (m_previousYearSales == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePreviousYearSales, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_previousYearSales = value;
                    OnPropertyChanged(PropertyNamePreviousYearSales);
                }
            }
            get { return m_previousYearSales; }
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameIsRequired);
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
                    OnPropertyChanged(PropertyNameIsReceived);
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
                    OnPropertyChanged(PropertyNameStoreId);
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
                    OnPropertyChanged(PropertyNameIsCompleted);
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
                    OnPropertyChanged(PropertyNameReceivedDateTime);
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
                    OnPropertyChanged(PropertyNameReceivedBy);
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
                    OnPropertyChanged(PropertyNameNote);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameLocation);
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
                    OnPropertyChanged(PropertyNameAccountNo);
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
                    OnPropertyChanged(PropertyNameAccountType);
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
        private string m_icNo;
        public const string PropertyNameIcNo = "IcNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_role;
        public const string PropertyNameRole = "Role";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mobileNo;
        public const string PropertyNameMobileNo = "MobileNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_officeNo;
        public const string PropertyNameOfficeNo = "OfficeNo";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_email;
        public const string PropertyNameEmail = "Email";


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
                    OnPropertyChanged(PropertyNameContractId);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameIcNo);
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

        public string Role
        {
            set
            {
                if (String.Equals(m_role, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRole, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_role = value;
                    OnPropertyChanged(PropertyNameRole);
                }
            }
            get
            {
                return m_role;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

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
                    OnPropertyChanged(PropertyNameMobileNo);
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
                    OnPropertyChanged(PropertyNameOfficeNo);
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
                    OnPropertyChanged(PropertyNameEmail);
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
                    OnPropertyChanged(PropertyNameContractTemplateId);
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameDescription);
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
                    OnPropertyChanged(PropertyNameStatus);
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
                if (m_interestRate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInterestRate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_interestRate = value;
                    OnPropertyChanged(PropertyNameInterestRate);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameStoreId);
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
                OnPropertyChanged(PropertyNameOwner);
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
                OnPropertyChanged(PropertyNameContractingParty);
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
                OnPropertyChanged(PropertyNameTenant);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CommercialSpace m_commercialSpace
                = new CommercialSpace();

        public const string PropertyNameCommercialSpace = "CommercialSpace";
        [DebuggerHidden]

        public CommercialSpace CommercialSpace
        {
            get { return m_commercialSpace; }
            set
            {
                m_commercialSpace = value;
                OnPropertyChanged(PropertyNameCommercialSpace);
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
                    OnPropertyChanged(PropertyNameContractId);
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
                    OnPropertyChanged(PropertyNameReferenceNo);
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameDate);
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
                    OnPropertyChanged(PropertyNameValue);
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameRemarks);
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
                    OnPropertyChanged(PropertyNamePeriod);
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
                    OnPropertyChanged(PropertyNamePeriodUnit);
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
                    OnPropertyChanged(PropertyNameStartDate);
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
                    OnPropertyChanged(PropertyNameEndDate);
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
                    OnPropertyChanged(PropertyNameRentalApplicationId);
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
                    OnPropertyChanged(PropertyNameStatus);
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
                    OnPropertyChanged(PropertyNameRentType);
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
                if (m_interestRate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInterestRate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_interestRate = value;
                    OnPropertyChanged(PropertyNameInterestRate);
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
                    OnPropertyChanged(PropertyNameOption);
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameExtension);
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
                    OnPropertyChanged(PropertyNameStoreId);
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
                    OnPropertyChanged(PropertyNameDate);
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
                    OnPropertyChanged(PropertyNameCommitedBy);
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
                    OnPropertyChanged(PropertyNameNo);
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
                    OnPropertyChanged(PropertyNameNote);
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
                OnPropertyChanged(PropertyNameAddress);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameTelephoneNo);
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
                    OnPropertyChanged(PropertyNameFaxNo);
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
                    OnPropertyChanged(PropertyNameEmail);
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
                    OnPropertyChanged(PropertyNameUser);
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
                    OnPropertyChanged(PropertyNameDateTime);
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
                    OnPropertyChanged(PropertyNameOperation);
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameEntityId);
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
                    OnPropertyChanged(PropertyNamePropertyName);
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
                    OnPropertyChanged(PropertyNameOldValue);
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
                    OnPropertyChanged(PropertyNameNewValue);
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
                    OnPropertyChanged(PropertyNameAction);
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
                OnPropertyChanged(PropertyNameAddress);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
                    OnPropertyChanged(PropertyNameEmail);
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
                    OnPropertyChanged(PropertyNameOfficeNo);
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
                    OnPropertyChanged(PropertyNameFaxNo);
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
        private int m_commercialSpaceId;
        public const string PropertyNameCommercialSpaceId = "CommercialSpaceId";


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
                    OnPropertyChanged(PropertyNameBusinessPlan);
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

        public int CommercialSpaceId
        {
            set
            {
                if (m_commercialSpaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpaceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_commercialSpaceId = value;
                    OnPropertyChanged(PropertyNameCommercialSpaceId);
                }
            }
            get
            {
                return m_commercialSpaceId;
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
                    OnPropertyChanged(PropertyNameSize);
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
                    OnPropertyChanged(PropertyNameBuilding);
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
                    OnPropertyChanged(PropertyNameFloor);
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
                    OnPropertyChanged(PropertyNameDeposit);
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
                    OnPropertyChanged(PropertyNameRent);
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
                    OnPropertyChanged(PropertyNameDate);
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
                    OnPropertyChanged(PropertyNameExpiryDate);
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
                    OnPropertyChanged(PropertyNamePeriod);
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
                    OnPropertyChanged(PropertyNamePeriodUnit);
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
                    OnPropertyChanged(PropertyNameOption);
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
                    OnPropertyChanged(PropertyNameBusinessPlanText);
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameDescription);
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
                    OnPropertyChanged(PropertyNameNote);
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
                    OnPropertyChanged(PropertyNameIsRequired);
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameDescription);
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
                    OnPropertyChanged(PropertyNameText);
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameDescription);
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
                    OnPropertyChanged(PropertyNameNo);
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
                    OnPropertyChanged(PropertyNameText);
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
                OnPropertyChanged(PropertyNameContact);
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
                OnPropertyChanged(PropertyNameAddress);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
                    OnPropertyChanged(PropertyNameDepositPaymentId);
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
                    OnPropertyChanged(PropertyNameReceiptNo);
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
                    OnPropertyChanged(PropertyNameAmount);
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
                    OnPropertyChanged(PropertyNameDate);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
        private string m_date;
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
                    OnPropertyChanged(PropertyNamePaymentId);
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
                    OnPropertyChanged(PropertyNameAmount);
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

        public string Date
        {
            set
            {
                if (String.Equals(m_date, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_date = value;
                    OnPropertyChanged(PropertyNameDate);
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
                    OnPropertyChanged(PropertyNameContractNo);
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
                    OnPropertyChanged(PropertyNameTenantIdSsmNo);
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
                    OnPropertyChanged(PropertyNameReceiptNo);
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
                    OnPropertyChanged(PropertyNameUsername);
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
                    OnPropertyChanged(PropertyNameFullName);
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
                    OnPropertyChanged(PropertyNameDesignation);
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
                    OnPropertyChanged(PropertyNameTelephone);
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
                    OnPropertyChanged(PropertyNameMobile);
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
                    OnPropertyChanged(PropertyNameRoleTypes);
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
                    OnPropertyChanged(PropertyNameStartModule);
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
                    OnPropertyChanged(PropertyNameEmail);
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
                    OnPropertyChanged(PropertyNameUserProfileId);
                }
            }
            get
            {
                return m_userProfileId;
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
                    OnPropertyChanged(PropertyNameDepositId);
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
                    OnPropertyChanged(PropertyNameDateTime);
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameIDNumber);
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
                    OnPropertyChanged(PropertyNameRegistrationNo);
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
                    OnPropertyChanged(PropertyNameAmount);
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
                    OnPropertyChanged(PropertyNameIsPaid);
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
                    OnPropertyChanged(PropertyNameIsRefund);
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
                    OnPropertyChanged(PropertyNameReceiptNo);
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
                    OnPropertyChanged(PropertyNameRefundedBy);
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
                    OnPropertyChanged(PropertyNameIsVoid);
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
                    OnPropertyChanged(PropertyNamePaymentDateTime);
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
                    OnPropertyChanged(PropertyNameRefundDateTime);
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
                    OnPropertyChanged(PropertyNameDueDate);
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
    [XmlType("Role", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Role
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_roleId;
        public const string PropertyNameRoleId = "RoleId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        private readonly ObjectCollection<Permission> m_PermissionCollection = new ObjectCollection<Permission>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Permission", IsNullable = false)]
        public ObjectCollection<Permission> PermissionCollection
        {
            get { return m_PermissionCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string RoleId
        {
            set
            {
                if (String.Equals(m_roleId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRoleId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_roleId = value;
                    OnPropertyChanged(PropertyNameRoleId);
                }
            }
            get
            {
                return m_roleId;
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
                    OnPropertyChanged(PropertyNameName);
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
    [XmlType("Permission", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Permission
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAuthorized;
        public const string PropertyNameIsAuthorized = "IsAuthorized";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsAuthorized
        {
            set
            {
                if (m_isAuthorized == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAuthorized, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAuthorized = value;
                    OnPropertyChanged(PropertyNameIsAuthorized);
                }
            }
            get
            {
                return m_isAuthorized;
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameTitle);
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
                    OnPropertyChanged(PropertyNameText);
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
                    OnPropertyChanged(PropertyNameSettingId);
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
                    OnPropertyChanged(PropertyNameUsername);
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
                    OnPropertyChanged(PropertyNameKey);
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
                    OnPropertyChanged(PropertyNameValue);
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
                    OnPropertyChanged(PropertyNameRebateId);
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
                    OnPropertyChanged(PropertyNameContractNo);
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
                    OnPropertyChanged(PropertyNameAmount);
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
                    OnPropertyChanged(PropertyNameStartDate);
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
                    OnPropertyChanged(PropertyNameEndDate);
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
        private string m_commercialSpaceCategory;
        public const string PropertyNameCommercialSpaceCategory = "CommercialSpaceCategory";


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
                    OnPropertyChanged(PropertyNameBuilding);
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

        public string CommercialSpaceCategory
        {
            set
            {
                if (String.Equals(m_commercialSpaceCategory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpaceCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_commercialSpaceCategory = value;
                    OnPropertyChanged(PropertyNameCommercialSpaceCategory);
                }
            }
            get
            {
                return m_commercialSpaceCategory;
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
                    OnPropertyChanged(PropertyNamePercentage);
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
                    OnPropertyChanged(PropertyNamePeriod);
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
                    OnPropertyChanged(PropertyNamePeriodType);
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

        [XmlAttribute]
        public string Half { get; set; }


        [XmlAttribute]
        public int Year { get; set; }


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
                OnPropertyChanged(PropertyNameTenant);
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

        [XmlAttribute]
        public string Category { get; set; }


        [XmlAttribute]
        public int F2 { get; set; }


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
                OnPropertyChanged(PropertyNameTenant);
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
                OnPropertyChanged(PropertyNameAddress);
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
                    OnPropertyChanged(PropertyNameAmount);
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
                    OnPropertyChanged(PropertyNameCategory);
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
                    OnPropertyChanged(PropertyNameNote);
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
                    OnPropertyChanged(PropertyNameName);
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
        private string m_commercialSpace;
        public const string PropertyNameCommercialSpace = "CommercialSpace";


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
        private Inspection m_inspection
                = new Inspection();

        public const string PropertyNameInspection = "Inspection";
        [DebuggerHidden]

        public Inspection Inspection
        {
            get { return m_inspection; }
            set
            {
                m_inspection = value;
                OnPropertyChanged(PropertyNameInspection);
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
                    OnPropertyChanged(PropertyNameComplaintId);
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
                    OnPropertyChanged(PropertyNameTemplateId);
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
                    OnPropertyChanged(PropertyNameRemarks);
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
                    OnPropertyChanged(PropertyNameTenantId);
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

        public string CommercialSpace
        {
            set
            {
                if (String.Equals(m_commercialSpace, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpace, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_commercialSpace = value;
                    OnPropertyChanged(PropertyNameCommercialSpace);
                }
            }
            get
            {
                return m_commercialSpace;
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
                    OnPropertyChanged(PropertyNameStatus);
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
                    OnPropertyChanged(PropertyNameCategory);
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
                    OnPropertyChanged(PropertyNameSubCategory);
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
                    OnPropertyChanged(PropertyNameReferenceNo);
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameAttachmentStoreId);
                }
            }
            get
            {
                return m_attachmentStoreId;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CustomField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CustomField
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_order;
        public const string PropertyNameOrder = "Order";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isMandatory;
        public const string PropertyNameIsMandatory = "IsMandatory";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_size;
        public const string PropertyNameSize = "Size";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Order
        {
            set
            {
                if (m_order == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrder, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_order = value;
                    OnPropertyChanged(PropertyNameOrder);
                }
            }
            get
            {
                return m_order;
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
                    OnPropertyChanged(PropertyNameName);
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

        public bool IsMandatory
        {
            set
            {
                if (m_isMandatory == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsMandatory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isMandatory = value;
                    OnPropertyChanged(PropertyNameIsMandatory);
                }
            }
            get
            {
                return m_isMandatory;
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameSize);
                }
            }
            get
            {
                return m_size;
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameValue);
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
    [XmlType("ComplaintTemplate", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComplaintTemplate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_complaintTemplateId;
        public const string PropertyNameComplaintTemplateId = "ComplaintTemplateId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        private readonly ObjectCollection<ComplaintCategory> m_ComplaintCategoryCollection = new ObjectCollection<ComplaintCategory>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ComplaintCategory", IsNullable = false)]
        public ObjectCollection<ComplaintCategory> ComplaintCategoryCollection
        {
            get { return m_ComplaintCategoryCollection; }
        }

        private readonly ObjectCollection<ComplaintCustomField> m_ComplaintCustomFieldCollection = new ObjectCollection<ComplaintCustomField>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ComplaintCustomField", IsNullable = false)]
        public ObjectCollection<ComplaintCustomField> ComplaintCustomFieldCollection
        {
            get { return m_ComplaintCustomFieldCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ComplaintTemplateId
        {
            set
            {
                if (m_complaintTemplateId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComplaintTemplateId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_complaintTemplateId = value;
                    OnPropertyChanged(PropertyNameComplaintTemplateId);
                }
            }
            get
            {
                return m_complaintTemplateId;
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameDescription);
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
                    OnPropertyChanged(PropertyNameIsActive);
                }
            }
            get
            {
                return m_isActive;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ComplaintCategory", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComplaintCategory
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        private readonly ObjectCollection<ComplaintSubCategory> m_SubCategoryCollection = new ObjectCollection<ComplaintSubCategory>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ComplaintSubCategory", IsNullable = false)]
        public ObjectCollection<ComplaintSubCategory> SubCategoryCollection
        {
            get { return m_SubCategoryCollection; }
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
                    OnPropertyChanged(PropertyNameName);
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
                    OnPropertyChanged(PropertyNameDescription);
                }
            }
            get
            {
                return m_description;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ComplaintCustomField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComplaintCustomField
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_order;
        public const string PropertyNameOrder = "Order";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isMandatory;
        public const string PropertyNameIsMandatory = "IsMandatory";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_size;
        public const string PropertyNameSize = "Size";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_listing;
        public const string PropertyNameListing = "Listing";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Order
        {
            set
            {
                if (m_order == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrder, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_order = value;
                    OnPropertyChanged(PropertyNameOrder);
                }
            }
            get
            {
                return m_order;
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
                    OnPropertyChanged(PropertyNameName);
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

        public bool IsMandatory
        {
            set
            {
                if (m_isMandatory == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsMandatory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isMandatory = value;
                    OnPropertyChanged(PropertyNameIsMandatory);
                }
            }
            get
            {
                return m_isMandatory;
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameSize);
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

        [DebuggerHidden]

        public string Listing
        {
            set
            {
                if (String.Equals(m_listing, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameListing, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_listing = value;
                    OnPropertyChanged(PropertyNameListing);
                }
            }
            get
            {
                return m_listing;
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
                    OnPropertyChanged(PropertyNamePersonInCharge);
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
                    OnPropertyChanged(PropertyNameAssignedDate);
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
                    OnPropertyChanged(PropertyNameRemark);
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
                    OnPropertyChanged(PropertyNameInspectionDate);
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
                    OnPropertyChanged(PropertyNameResolution);
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
                    OnPropertyChanged(PropertyNameObservation);
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
                    OnPropertyChanged(PropertyNameContractor);
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
                    OnPropertyChanged(PropertyNamePriority);
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
                    OnPropertyChanged(PropertyNameSeverity);
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
    [XmlType("ComplaintSubCategory", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComplaintSubCategory
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


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
                    OnPropertyChanged(PropertyNameDescription);
                }
            }
            get
            {
                return m_description;
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

        private string m_no;
        public const string PropertyNameNo = "No";

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
                    OnPropertyChanged(PropertyNameInvoiceId);
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
                    OnPropertyChanged(PropertyNameDate);
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
                    OnPropertyChanged(PropertyNameAmount);
                }
            }
            get
            {
                return m_amount;
            }
        }



        [XmlAttribute]
        public string No
        {
            set
            {
                if (m_no == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNo, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_no = value;
                    OnPropertyChanged(PropertyNameNo);
                }
            }
            get
            {
                return m_no;
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
                    OnPropertyChanged(PropertyNameType);
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
                    OnPropertyChanged(PropertyNameContractNo);
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
                    OnPropertyChanged(PropertyNameTenantIdSsmNo);
                }
            }
            get
            {
                return m_tenantIdSsmNo;
            }
        }



    }


    public enum InvoiceType
    {
        AdhocInvoice,
        Rental,
    }

}

