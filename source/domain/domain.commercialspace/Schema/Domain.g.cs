
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
          [XmlType("Land",  Namespace=Strings.DefaultNamespace)]
          public  partial class Land
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_lot;
                public const string PropertyNameLot = "Lot";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_title;
                public const string PropertyNameTitle = "Title";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_location;
                public const string PropertyNameLocation = "Location";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_sizeUnit;
                public const string PropertyNameSizeUnit = "SizeUnit";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_currentMarketValue;
                public const string PropertyNameCurrentMarketValue = "CurrentMarketValue";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_rezabNo;
                public const string PropertyNameRezabNo = "RezabNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_sheetNo;
                public const string PropertyNameSheetNo = "SheetNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_approvedPlanNo;
                public const string PropertyNameApprovedPlanNo = "ApprovedPlanNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_landOffice;
                public const string PropertyNameLandOffice = "LandOffice";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_usage;
                public const string PropertyNameUsage = "Usage";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_ownLevel;
                public const string PropertyNameOwnLevel = "OwnLevel";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_planNo;
                public const string PropertyNamePlanNo = "PlanNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_status;
                public const string PropertyNameStatus = "Status";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isApproved;
                public const string PropertyNameIsApproved = "IsApproved";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_leaseExpiryDate;
                public const string PropertyNameLeaseExpiryDate = "LeaseExpiryDate";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_leasePeriod;
                public const string PropertyNameLeasePeriod = "LeasePeriod";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_approvedDateTime;
                public const string PropertyNameApprovedDateTime = "ApprovedDateTime";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private string  m_approvedBy;
                public const string PropertyNameApprovedBy = "ApprovedBy";

         
          
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Owner m_owner
					=  new Owner();
				
			public const string PropertyNameOwner = "Owner";
			[DebuggerHidden]

			public Owner Owner
			{
			get{ return m_owner;}
			set
			{
			m_owner = value;
			OnPropertyChanged(PropertyNameOwner);
			}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Address m_address
					=  new Address();
				
			public const string PropertyNameAddress = "Address";
			[DebuggerHidden]

			public Address Address
			{
			get{ return m_address;}
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
                if( String.Equals( m_lot, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLot, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_lot= value;
                OnPropertyChanged(PropertyNameLot);
                }
                }
                get
                {
                return m_lot;}
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
                if( String.Equals( m_title, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTitle, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_title= value;
                OnPropertyChanged(PropertyNameTitle);
                }
                }
                get
                {
                return m_title;}
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
                if( String.Equals( m_location, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLocation, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_location= value;
                OnPropertyChanged(PropertyNameLocation);
                }
                }
                get
                {
                return m_location;}
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
                if( m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_size= value;
                OnPropertyChanged(PropertyNameSize);
                }
                }
                get
                {
                return m_size;}
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
                if( String.Equals( m_sizeUnit, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSizeUnit, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_sizeUnit= value;
                OnPropertyChanged(PropertyNameSizeUnit);
                }
                }
                get
                {
                return m_sizeUnit;}
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
                if( m_currentMarketValue == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCurrentMarketValue, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_currentMarketValue= value;
                OnPropertyChanged(PropertyNameCurrentMarketValue);
                }
                }
                get
                {
                return m_currentMarketValue;}
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
                if( String.Equals( m_rezabNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRezabNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_rezabNo= value;
                OnPropertyChanged(PropertyNameRezabNo);
                }
                }
                get
                {
                return m_rezabNo;}
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
                if( String.Equals( m_sheetNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSheetNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_sheetNo= value;
                OnPropertyChanged(PropertyNameSheetNo);
                }
                }
                get
                {
                return m_sheetNo;}
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
                if( String.Equals( m_approvedPlanNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApprovedPlanNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_approvedPlanNo= value;
                OnPropertyChanged(PropertyNameApprovedPlanNo);
                }
                }
                get
                {
                return m_approvedPlanNo;}
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
                if( String.Equals( m_landOffice, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLandOffice, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_landOffice= value;
                OnPropertyChanged(PropertyNameLandOffice);
                }
                }
                get
                {
                return m_landOffice;}
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
                if( String.Equals( m_usage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUsage, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_usage= value;
                OnPropertyChanged(PropertyNameUsage);
                }
                }
                get
                {
                return m_usage;}
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
                if( String.Equals( m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_note= value;
                OnPropertyChanged(PropertyNameNote);
                }
                }
                get
                {
                return m_note;}
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
                if( String.Equals( m_ownLevel, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOwnLevel, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_ownLevel= value;
                OnPropertyChanged(PropertyNameOwnLevel);
                }
                }
                get
                {
                return m_ownLevel;}
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
                if( String.Equals( m_planNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePlanNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_planNo= value;
                OnPropertyChanged(PropertyNamePlanNo);
                }
                }
                get
                {
                return m_planNo;}
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
                if( String.Equals( m_status, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_status= value;
                OnPropertyChanged(PropertyNameStatus);
                }
                }
                get
                {
                return m_status;}
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
                if( m_isApproved == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsApproved, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isApproved= value;
                OnPropertyChanged(PropertyNameIsApproved);
                }
                }
                get
                {
                return m_isApproved;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? LeaseExpiryDate
            {
            set
            {
            if(m_leaseExpiryDate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameLeaseExpiryDate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_leaseExpiryDate= value;
            OnPropertyChanged(PropertyNameLeaseExpiryDate);
            }
            }
            get { return m_leaseExpiryDate;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? LeasePeriod
            {
            set
            {
            if(m_leasePeriod == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameLeasePeriod, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_leasePeriod= value;
            OnPropertyChanged(PropertyNameLeasePeriod);
            }
            }
            get { return m_leasePeriod;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? ApprovedDateTime
            {
            set
            {
            if(m_approvedDateTime == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameApprovedDateTime, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_approvedDateTime= value;
            OnPropertyChanged(PropertyNameApprovedDateTime);
            }
            }
            get { return m_approvedDateTime;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public string ApprovedBy
            {
            set
            {
            if(String.Equals( m_approvedBy, value, StringComparison.Ordinal)) return;
            var arg = new PropertyChangingEventArgs(PropertyNameApprovedBy, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_approvedBy= value;
            OnPropertyChanged(PropertyNameApprovedBy);
            }
            }
            get { return m_approvedBy;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Building",  Namespace=Strings.DefaultNamespace)]
          public  partial class Building
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_lotNo;
                public const string PropertyNameLotNo = "LotNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_status;
                public const string PropertyNameStatus = "Status";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_floors;
                public const string PropertyNameFloors = "Floors";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_height;
                public const string PropertyNameHeight = "Height";

         
          
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Address m_address
					=  new Address();
				
			public const string PropertyNameAddress = "Address";
			[DebuggerHidden]

			public Address Address
			{
			get{ return m_address;}
			set
			{
			m_address = value;
			OnPropertyChanged(PropertyNameAddress);
			}
			}
		
			private readonly ObjectCollection<Floor>  m_FloorCollection = new ObjectCollection<Floor> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Floor", IsNullable = false)]
			public ObjectCollection<Floor> FloorCollection
			{
			get{ return m_FloorCollection;}
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( String.Equals( m_lotNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLotNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_lotNo= value;
                OnPropertyChanged(PropertyNameLotNo);
                }
                }
                get
                {
                return m_lotNo;}
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
                if( m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_size= value;
                OnPropertyChanged(PropertyNameSize);
                }
                }
                get
                {
                return m_size;}
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
                if( String.Equals( m_status, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_status= value;
                OnPropertyChanged(PropertyNameStatus);
                }
                }
                get
                {
                return m_status;}
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
                if( m_floors == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloors, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_floors= value;
                OnPropertyChanged(PropertyNameFloors);
                }
                }
                get
                {
                return m_floors;}
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
                if( String.Equals( m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_note= value;
                OnPropertyChanged(PropertyNameNote);
                }
                }
                get
                {
                return m_note;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? Height
            {
            set
            {
            if(m_height == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameHeight, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_height= value;
            OnPropertyChanged(PropertyNameHeight);
            }
            }
            get { return m_height;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Address",  Namespace=Strings.DefaultNamespace)]
          public  partial class Address
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_state;
                public const string PropertyNameState = "State";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_city;
                public const string PropertyNameCity = "City";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_postcode;
                public const string PropertyNamePostcode = "Postcode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_street;
                public const string PropertyNameStreet = "Street";

              
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
                if( String.Equals( m_state, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameState, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_state= value;
                OnPropertyChanged(PropertyNameState);
                }
                }
                get
                {
                return m_state;}
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
                if( String.Equals( m_city, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCity, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_city= value;
                OnPropertyChanged(PropertyNameCity);
                }
                }
                get
                {
                return m_city;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Postcode
                {
                set
                {
                if( m_postcode == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePostcode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_postcode= value;
                OnPropertyChanged(PropertyNamePostcode);
                }
                }
                get
                {
                return m_postcode;}
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
                if( String.Equals( m_street, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStreet, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_street= value;
                OnPropertyChanged(PropertyNameStreet);
                }
                }
                get
                {
                return m_street;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Floor",  Namespace=Strings.DefaultNamespace)]
          public  partial class Floor
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_number;
                public const string PropertyNameNumber = "Number";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
			private readonly ObjectCollection<Lot>  m_LotCollection = new ObjectCollection<Lot> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Lot", IsNullable = false)]
			public ObjectCollection<Lot> LotCollection
			{
			get{ return m_LotCollection;}
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_size= value;
                OnPropertyChanged(PropertyNameSize);
                }
                }
                get
                {
                return m_size;}
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
                if( m_number == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNumber, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_number= value;
                OnPropertyChanged(PropertyNameNumber);
                }
                }
                get
                {
                return m_number;}
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
                if( String.Equals( m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_note= value;
                OnPropertyChanged(PropertyNameNote);
                }
                }
                get
                {
                return m_note;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Lot",  Namespace=Strings.DefaultNamespace)]
          public  partial class Lot
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_floorNo;
                public const string PropertyNameFloorNo = "FloorNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCommercialSpace;
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_size= value;
                OnPropertyChanged(PropertyNameSize);
                }
                }
                get
                {
                return m_size;}
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
                if( String.Equals( m_floorNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloorNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_floorNo= value;
                OnPropertyChanged(PropertyNameFloorNo);
                }
                }
                get
                {
                return m_floorNo;}
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
                if( m_isCommercialSpace == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCommercialSpace, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCommercialSpace= value;
                OnPropertyChanged(PropertyNameIsCommercialSpace);
                }
                }
                get
                {
                return m_isCommercialSpace;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("CommercialSpace",  Namespace=Strings.DefaultNamespace)]
          public  partial class CommercialSpace
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_buildingId;
                public const string PropertyNameBuildingId = "BuildingId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_lotName;
                public const string PropertyNameLotName = "LotName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_floorName;
                public const string PropertyNameFloorName = "FloorName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_category;
                public const string PropertyNameCategory = "Category";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_rentalRate;
                public const string PropertyNameRentalRate = "RentalRate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_rentalType;
                public const string PropertyNameRentalType = "RentalType";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isOnline;
                public const string PropertyNameIsOnline = "IsOnline";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_registrationNo;
                public const string PropertyNameRegistrationNo = "RegistrationNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_status;
                public const string PropertyNameStatus = "Status";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_contactPerson;
                public const string PropertyNameContactPerson = "ContactPerson";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_contactNo;
                public const string PropertyNameContactNo = "ContactNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_state;
                public const string PropertyNameState = "State";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_city;
                public const string PropertyNameCity = "City";

              
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
                if( m_buildingId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuildingId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_buildingId= value;
                OnPropertyChanged(PropertyNameBuildingId);
                }
                }
                get
                {
                return m_buildingId;}
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
                if( String.Equals( m_lotName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLotName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_lotName= value;
                OnPropertyChanged(PropertyNameLotName);
                }
                }
                get
                {
                return m_lotName;}
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
                if( String.Equals( m_floorName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFloorName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_floorName= value;
                OnPropertyChanged(PropertyNameFloorName);
                }
                }
                get
                {
                return m_floorName;}
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
                if( m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_size= value;
                OnPropertyChanged(PropertyNameSize);
                }
                }
                get
                {
                return m_size;}
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
                if( String.Equals( m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_category= value;
                OnPropertyChanged(PropertyNameCategory);
                }
                }
                get
                {
                return m_category;}
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
                if( m_rentalRate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentalRate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_rentalRate= value;
                OnPropertyChanged(PropertyNameRentalRate);
                }
                }
                get
                {
                return m_rentalRate;}
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
                if( String.Equals( m_rentalType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRentalType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_rentalType= value;
                OnPropertyChanged(PropertyNameRentalType);
                }
                }
                get
                {
                return m_rentalType;}
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
                if( m_isOnline == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsOnline, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isOnline= value;
                OnPropertyChanged(PropertyNameIsOnline);
                }
                }
                get
                {
                return m_isOnline;}
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
                if( String.Equals( m_registrationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRegistrationNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_registrationNo= value;
                OnPropertyChanged(PropertyNameRegistrationNo);
                }
                }
                get
                {
                return m_registrationNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool Status
                {
                set
                {
                if( m_status == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_status= value;
                OnPropertyChanged(PropertyNameStatus);
                }
                }
                get
                {
                return m_status;}
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
                if( String.Equals( m_contactPerson, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContactPerson, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_contactPerson= value;
                OnPropertyChanged(PropertyNameContactPerson);
                }
                }
                get
                {
                return m_contactPerson;}
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
                if( String.Equals( m_contactNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContactNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_contactNo= value;
                OnPropertyChanged(PropertyNameContactNo);
                }
                }
                get
                {
                return m_contactNo;}
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
                if( String.Equals( m_state, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameState, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_state= value;
                OnPropertyChanged(PropertyNameState);
                }
                }
                get
                {
                return m_state;}
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
                if( String.Equals( m_city, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCity, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_city= value;
                OnPropertyChanged(PropertyNameCity);
                }
                }
                get
                {
                return m_city;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("RentalApplication",  Namespace=Strings.DefaultNamespace)]
          public  partial class RentalApplication
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_companyName;
                public const string PropertyNameCompanyName = "CompanyName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_companyRegistrationNo;
                public const string PropertyNameCompanyRegistrationNo = "CompanyRegistrationNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateStart;
                public const string PropertyNameDateStart = "DateStart";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateEnd;
                public const string PropertyNameDateEnd = "DateEnd";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_purpose;
                public const string PropertyNamePurpose = "Purpose";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_companyType;
                public const string PropertyNameCompanyType = "CompanyType";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_commercialSpaceId;
                public const string PropertyNameCommercialSpaceId = "CommercialSpaceId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_status;
                public const string PropertyNameStatus = "Status";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_experience;
                public const string PropertyNameExperience = "Experience";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isRecordExist;
                public const string PropertyNameIsRecordExist = "IsRecordExist";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_previousAddress;
                public const string PropertyNamePreviousAddress = "PreviousAddress";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCompany;
                public const string PropertyNameIsCompany = "IsCompany";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal  m_currentYearSales;
                public const string PropertyNameCurrentYearSales = "CurrentYearSales";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal  m_lastYearSales;
                public const string PropertyNameLastYearSales = "LastYearSales";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal  m_previousYearSales;
                public const string PropertyNamePreviousYearSales = "PreviousYearSales";

         
          
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Address m_address
					=  new Address();
				
			public const string PropertyNameAddress = "Address";
			[DebuggerHidden]

			public Address Address
			{
			get{ return m_address;}
			set
			{
			m_address = value;
			OnPropertyChanged(PropertyNameAddress);
			}
			}
		
			private readonly ObjectCollection<Bank>  m_BankCollection = new ObjectCollection<Bank> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Bank", IsNullable = false)]
			public ObjectCollection<Bank> BankCollection
			{
			get{ return m_BankCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Contact m_contact
					=  new Contact();
				
			public const string PropertyNameContact = "Contact";
			[DebuggerHidden]

			public Contact Contact
			{
			get{ return m_contact;}
			set
			{
			m_contact = value;
			OnPropertyChanged(PropertyNameContact);
			}
			}
		
			private readonly ObjectCollection<Attachment>  m_AttachmentCollection = new ObjectCollection<Attachment> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Attachment", IsNullable = false)]
			public ObjectCollection<Attachment> AttachmentCollection
			{
			get{ return m_AttachmentCollection;}
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
                if( String.Equals( m_companyName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompanyName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_companyName= value;
                OnPropertyChanged(PropertyNameCompanyName);
                }
                }
                get
                {
                return m_companyName;}
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
                if( String.Equals( m_companyRegistrationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompanyRegistrationNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_companyRegistrationNo= value;
                OnPropertyChanged(PropertyNameCompanyRegistrationNo);
                }
                }
                get
                {
                return m_companyRegistrationNo;}
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
                if( m_dateStart == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateStart, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dateStart= value;
                OnPropertyChanged(PropertyNameDateStart);
                }
                }
                get
                {
                return m_dateStart;}
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
                if( m_dateEnd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateEnd, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dateEnd= value;
                OnPropertyChanged(PropertyNameDateEnd);
                }
                }
                get
                {
                return m_dateEnd;}
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
                if( String.Equals( m_purpose, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePurpose, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_purpose= value;
                OnPropertyChanged(PropertyNamePurpose);
                }
                }
                get
                {
                return m_purpose;}
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
                if( String.Equals( m_companyType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCompanyType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_companyType= value;
                OnPropertyChanged(PropertyNameCompanyType);
                }
                }
                get
                {
                return m_companyType;}
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
                if( m_commercialSpaceId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpaceId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_commercialSpaceId= value;
                OnPropertyChanged(PropertyNameCommercialSpaceId);
                }
                }
                get
                {
                return m_commercialSpaceId;}
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
                if( String.Equals( m_status, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_status= value;
                OnPropertyChanged(PropertyNameStatus);
                }
                }
                get
                {
                return m_status;}
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
                if( String.Equals( m_experience, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExperience, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_experience= value;
                OnPropertyChanged(PropertyNameExperience);
                }
                }
                get
                {
                return m_experience;}
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
                if( m_isRecordExist == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRecordExist, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isRecordExist= value;
                OnPropertyChanged(PropertyNameIsRecordExist);
                }
                }
                get
                {
                return m_isRecordExist;}
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
                if( String.Equals( m_previousAddress, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePreviousAddress, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_previousAddress= value;
                OnPropertyChanged(PropertyNamePreviousAddress);
                }
                }
                get
                {
                return m_previousAddress;}
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
                if( m_isCompany == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCompany, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCompany= value;
                OnPropertyChanged(PropertyNameIsCompany);
                }
                }
                get
                {
                return m_isCompany;}
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
                if( String.Equals( m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_type= value;
                OnPropertyChanged(PropertyNameType);
                }
                }
                get
                {
                return m_type;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal CurrentYearSales
            {
            set
            {
            if(m_currentYearSales == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameCurrentYearSales, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_currentYearSales= value;
            OnPropertyChanged(PropertyNameCurrentYearSales);
            }
            }
            get { return m_currentYearSales;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal LastYearSales
            {
            set
            {
            if(m_lastYearSales == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameLastYearSales, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_lastYearSales= value;
            OnPropertyChanged(PropertyNameLastYearSales);
            }
            }
            get { return m_lastYearSales;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal PreviousYearSales
            {
            set
            {
            if(m_previousYearSales == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNamePreviousYearSales, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_previousYearSales= value;
            OnPropertyChanged(PropertyNamePreviousYearSales);
            }
            }
            get { return m_previousYearSales;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Attachment",  Namespace=Strings.DefaultNamespace)]
          public  partial class Attachment
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isRequired;
                public const string PropertyNameIsRequired = "IsRequired";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isReceived;
                public const string PropertyNameIsReceived = "IsReceived";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_storeId;
                public const string PropertyNameStoreId = "StoreId";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_receivedDateTime;
                public const string PropertyNameReceivedDateTime = "ReceivedDateTime";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private string  m_receivedBy;
                public const string PropertyNameReceivedBy = "ReceivedBy";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private string  m_note;
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
                if( String.Equals( m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_type= value;
                OnPropertyChanged(PropertyNameType);
                }
                }
                get
                {
                return m_type;}
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isRequired= value;
                OnPropertyChanged(PropertyNameIsRequired);
                }
                }
                get
                {
                return m_isRequired;}
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
                if( m_isReceived == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsReceived, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isReceived= value;
                OnPropertyChanged(PropertyNameIsReceived);
                }
                }
                get
                {
                return m_isReceived;}
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
                if( String.Equals( m_storeId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_storeId= value;
                OnPropertyChanged(PropertyNameStoreId);
                }
                }
                get
                {
                return m_storeId;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? ReceivedDateTime
            {
            set
            {
            if(m_receivedDateTime == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameReceivedDateTime, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_receivedDateTime= value;
            OnPropertyChanged(PropertyNameReceivedDateTime);
            }
            }
            get { return m_receivedDateTime;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public string ReceivedBy
            {
            set
            {
            if(String.Equals( m_receivedBy, value, StringComparison.Ordinal)) return;
            var arg = new PropertyChangingEventArgs(PropertyNameReceivedBy, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_receivedBy= value;
            OnPropertyChanged(PropertyNameReceivedBy);
            }
            }
            get { return m_receivedBy;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public string Note
            {
            set
            {
            if(String.Equals( m_note, value, StringComparison.Ordinal)) return;
            var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_note= value;
            OnPropertyChanged(PropertyNameNote);
            }
            }
            get { return m_note;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Bank",  Namespace=Strings.DefaultNamespace)]
          public  partial class Bank
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_location;
                public const string PropertyNameLocation = "Location";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_accountNo;
                public const string PropertyNameAccountNo = "AccountNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_accountType;
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( String.Equals( m_location, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLocation, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_location= value;
                OnPropertyChanged(PropertyNameLocation);
                }
                }
                get
                {
                return m_location;}
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
                if( String.Equals( m_accountNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_accountNo= value;
                OnPropertyChanged(PropertyNameAccountNo);
                }
                }
                get
                {
                return m_accountNo;}
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
                if( String.Equals( m_accountType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_accountType= value;
                OnPropertyChanged(PropertyNameAccountType);
                }
                }
                get
                {
                return m_accountType;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Contact",  Namespace=Strings.DefaultNamespace)]
          public  partial class Contact
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_title;
                public const string PropertyNameTitle = "Title";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_icNo;
                public const string PropertyNameIcNo = "IcNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_role;
                public const string PropertyNameRole = "Role";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_mobileNo;
                public const string PropertyNameMobileNo = "MobileNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_officeNo;
                public const string PropertyNameOfficeNo = "OfficeNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_email;
                public const string PropertyNameEmail = "Email";

              
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( String.Equals( m_title, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTitle, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_title= value;
                OnPropertyChanged(PropertyNameTitle);
                }
                }
                get
                {
                return m_title;}
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
                if( String.Equals( m_icNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIcNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_icNo= value;
                OnPropertyChanged(PropertyNameIcNo);
                }
                }
                get
                {
                return m_icNo;}
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
                if( String.Equals( m_role, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRole, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_role= value;
                OnPropertyChanged(PropertyNameRole);
                }
                }
                get
                {
                return m_role;}
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
                if( String.Equals( m_mobileNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMobileNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_mobileNo= value;
                OnPropertyChanged(PropertyNameMobileNo);
                }
                }
                get
                {
                return m_mobileNo;}
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
                if( String.Equals( m_officeNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOfficeNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_officeNo= value;
                OnPropertyChanged(PropertyNameOfficeNo);
                }
                }
                get
                {
                return m_officeNo;}
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
                if( String.Equals( m_email, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmail, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_email= value;
                OnPropertyChanged(PropertyNameEmail);
                }
                }
                get
                {
                return m_email;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ContractTemplate",  Namespace=Strings.DefaultNamespace)]
          public  partial class ContractTemplate
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
			private readonly ObjectCollection<DocumentTemplate>  m_DocumentTemplateCollection = new ObjectCollection<DocumentTemplate> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("DocumentTemplate", IsNullable = false)]
			public ObjectCollection<DocumentTemplate> DocumentTemplateCollection
			{
			get{ return m_DocumentTemplateCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Type
                {
                set
                {
                if( String.Equals( m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_type= value;
                OnPropertyChanged(PropertyNameType);
                }
                }
                get
                {
                return m_type;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DocumentTemplate",  Namespace=Strings.DefaultNamespace)]
          public  partial class DocumentTemplate
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_storeId;
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( String.Equals( m_storeId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_storeId= value;
                OnPropertyChanged(PropertyNameStoreId);
                }
                }
                get
                {
                return m_storeId;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Contract",  Namespace=Strings.DefaultNamespace)]
          public  partial class Contract
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
			private readonly ObjectCollection<Document>  m_DocumentCollection = new ObjectCollection<Document> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Document", IsNullable = false)]
			public ObjectCollection<Document> DocumentCollection
			{
			get{ return m_DocumentCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string No
                {
                set
                {
                if( String.Equals( m_no, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_no= value;
                OnPropertyChanged(PropertyNameNo);
                }
                }
                get
                {
                return m_no;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Type
                {
                set
                {
                if( String.Equals( m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_type= value;
                OnPropertyChanged(PropertyNameType);
                }
                }
                get
                {
                return m_type;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public DateTime Date
                {
                set
                {
                if( m_date == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_date= value;
                OnPropertyChanged(PropertyNameDate);
                }
                }
                get
                {
                return m_date;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Document",  Namespace=Strings.DefaultNamespace)]
          public  partial class Document
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_title;
                public const string PropertyNameTitle = "Title";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_extension;
                public const string PropertyNameExtension = "Extension";

              
			private readonly ObjectCollection<DocumentVersion>  m_DocumentVersionCollection = new ObjectCollection<DocumentVersion> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("DocumentVersion", IsNullable = false)]
			public ObjectCollection<DocumentVersion> DocumentVersionCollection
			{
			get{ return m_DocumentVersionCollection;}
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
                if( String.Equals( m_title, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTitle, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_title= value;
                OnPropertyChanged(PropertyNameTitle);
                }
                }
                get
                {
                return m_title;}
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
                if( String.Equals( m_extension, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExtension, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_extension= value;
                OnPropertyChanged(PropertyNameExtension);
                }
                }
                get
                {
                return m_extension;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DocumentVersion",  Namespace=Strings.DefaultNamespace)]
          public  partial class DocumentVersion
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_storeId;
                public const string PropertyNameStoreId = "StoreId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_commitedBy;
                public const string PropertyNameCommitedBy = "CommitedBy";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
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
                if( String.Equals( m_storeId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_storeId= value;
                OnPropertyChanged(PropertyNameStoreId);
                }
                }
                get
                {
                return m_storeId;}
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
                if( m_date == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_date= value;
                OnPropertyChanged(PropertyNameDate);
                }
                }
                get
                {
                return m_date;}
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
                if( String.Equals( m_commitedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCommitedBy, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_commitedBy= value;
                OnPropertyChanged(PropertyNameCommitedBy);
                }
                }
                get
                {
                return m_commitedBy;}
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
                if( String.Equals( m_no, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_no= value;
                OnPropertyChanged(PropertyNameNo);
                }
                }
                get
                {
                return m_no;}
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
                if( String.Equals( m_note, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_note= value;
                OnPropertyChanged(PropertyNameNote);
                }
                }
                get
                {
                return m_note;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Owner",  Namespace=Strings.DefaultNamespace)]
          public  partial class Owner
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_referenceNo;
                public const string PropertyNameReferenceNo = "ReferenceNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_status;
                public const string PropertyNameStatus = "Status";

              
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
                if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_name= value;
                OnPropertyChanged(PropertyNameName);
                }
                }
                get
                {
                return m_name;}
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
                if( String.Equals( m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_type= value;
                OnPropertyChanged(PropertyNameType);
                }
                }
                get
                {
                return m_type;}
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
                if( String.Equals( m_referenceNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReferenceNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_referenceNo= value;
                OnPropertyChanged(PropertyNameReferenceNo);
                }
                }
                get
                {
                return m_referenceNo;}
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
                if( String.Equals( m_status, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_status= value;
                OnPropertyChanged(PropertyNameStatus);
                }
                }
                get
                {
                return m_status;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("AuditTrail",  Namespace=Strings.DefaultNamespace)]
          public  partial class AuditTrail
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_user;
                public const string PropertyNameUser = "User";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_operation;
                public const string PropertyNameOperation = "Operation";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_entityId;
                public const string PropertyNameEntityId = "EntityId";

              
			private readonly ObjectCollection<Change>  m_ChangeCollection = new ObjectCollection<Change> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Change", IsNullable = false)]
			public ObjectCollection<Change> ChangeCollection
			{
			get{ return m_ChangeCollection;}
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
                if( String.Equals( m_user, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUser, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_user= value;
                OnPropertyChanged(PropertyNameUser);
                }
                }
                get
                {
                return m_user;}
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
                if( m_dateTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateTime, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dateTime= value;
                OnPropertyChanged(PropertyNameDateTime);
                }
                }
                get
                {
                return m_dateTime;}
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
                if( String.Equals( m_operation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperation, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_operation= value;
                OnPropertyChanged(PropertyNameOperation);
                }
                }
                get
                {
                return m_operation;}
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
                if( String.Equals( m_type, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_type= value;
                OnPropertyChanged(PropertyNameType);
                }
                }
                get
                {
                return m_type;}
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
                if( m_entityId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_entityId= value;
                OnPropertyChanged(PropertyNameEntityId);
                }
                }
                get
                {
                return m_entityId;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Change",  Namespace=Strings.DefaultNamespace)]
          public  partial class Change
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_propertyName;
                public const string PropertyNamePropertyName = "PropertyName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_before;
                public const string PropertyNameBefore = "Before";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_after;
                public const string PropertyNameAfter = "After";

              
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
                if( String.Equals( m_propertyName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePropertyName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_propertyName= value;
                OnPropertyChanged(PropertyNamePropertyName);
                }
                }
                get
                {
                return m_propertyName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Before
                {
                set
                {
                if( String.Equals( m_before, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBefore, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_before= value;
                OnPropertyChanged(PropertyNameBefore);
                }
                }
                get
                {
                return m_before;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string After
                {
                set
                {
                if( String.Equals( m_after, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAfter, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_after= value;
                OnPropertyChanged(PropertyNameAfter);
                }
                }
                get
                {
                return m_after;}
                }

              


          }
        
    }

  