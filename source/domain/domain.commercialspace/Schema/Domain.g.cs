
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
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
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

              


          }
        
    }

  