
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.ComponentModel.DataAnnotations;

    namespace Bespoke.CommercialSpace.Domain
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
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_size;
                public const string PropertyNameSize = "Size";

              
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
        
    }

  