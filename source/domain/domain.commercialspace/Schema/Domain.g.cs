
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.ComponentModel.DataAnnotations;

    namespace Bespoke.Station.Domain
    {
    
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Order",  Namespace=Strings.DefaultNamespace)]
          public  partial class Order
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_orderNo;
                public const string PropertyNameOrderNo = "OrderNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_creationDate;
                public const string PropertyNameCreationDate = "CreationDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_supplierId;
                public const string PropertyNameSupplierId = "SupplierId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_shippingCondition;
                public const string PropertyNameShippingCondition = "ShippingCondition";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_salesRep;
                public const string PropertyNameSalesRep = "SalesRep";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_customerNo;
                public const string PropertyNameCustomerNo = "CustomerNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_customerName;
                public const string PropertyNameCustomerName = "CustomerName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_purchaseOrder;
                public const string PropertyNamePurchaseOrder = "PurchaseOrder";

              
			private readonly ObjectCollection<OrderLine>  m_OrderLineCollection = new ObjectCollection<OrderLine> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("OrderLine", IsNullable = false)]
			public ObjectCollection<OrderLine> OrderLineCollection
			{
			get{ return m_OrderLineCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string OrderNo
                {
                set
                {
                if( String.Equals( m_orderNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrderNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_orderNo= value;
                OnPropertyChanged(PropertyNameOrderNo);
                }
                }
                get
                {
                return m_orderNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime CreationDate
                {
                set
                {
                if( m_creationDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCreationDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_creationDate= value;
                OnPropertyChanged(PropertyNameCreationDate);
                }
                }
                get
                {
                return m_creationDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int SupplierId
                {
                set
                {
                if( m_supplierId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSupplierId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_supplierId= value;
                OnPropertyChanged(PropertyNameSupplierId);
                }
                }
                get
                {
                return m_supplierId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string ShippingCondition
                {
                set
                {
                if( String.Equals( m_shippingCondition, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShippingCondition, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shippingCondition= value;
                OnPropertyChanged(PropertyNameShippingCondition);
                }
                }
                get
                {
                return m_shippingCondition;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string SalesRep
                {
                set
                {
                if( String.Equals( m_salesRep, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSalesRep, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_salesRep= value;
                OnPropertyChanged(PropertyNameSalesRep);
                }
                }
                get
                {
                return m_salesRep;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string CustomerNo
                {
                set
                {
                if( String.Equals( m_customerNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCustomerNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_customerNo= value;
                OnPropertyChanged(PropertyNameCustomerNo);
                }
                }
                get
                {
                return m_customerNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string CustomerName
                {
                set
                {
                if( String.Equals( m_customerName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCustomerName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_customerName= value;
                OnPropertyChanged(PropertyNameCustomerName);
                }
                }
                get
                {
                return m_customerName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string PurchaseOrder
                {
                set
                {
                if( String.Equals( m_purchaseOrder, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePurchaseOrder, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_purchaseOrder= value;
                OnPropertyChanged(PropertyNamePurchaseOrder);
                }
                }
                get
                {
                return m_purchaseOrder;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("OrderLine",  Namespace=Strings.DefaultNamespace)]
          public  partial class OrderLine
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_productCode;
                public const string PropertyNameProductCode = "ProductCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_supplierProductCode;
                public const string PropertyNameSupplierProductCode = "SupplierProductCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_volume;
                public const string PropertyNameVolume = "Volume";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_deliveryNote;
                public const string PropertyNameDeliveryNote = "DeliveryNote";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_uom;
                public const string PropertyNameUom = "Uom";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_depot;
                public const string PropertyNameDepot = "Depot";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_vehicleNo;
                public const string PropertyNameVehicleNo = "VehicleNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  OrderStatus  m_status;
                public const string PropertyNameStatus = "Status";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_unitPrice;
                public const string PropertyNameUnitPrice = "UnitPrice";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_quantity;
                public const string PropertyNameQuantity = "Quantity";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_eta;
                public const string PropertyNameEta = "Eta";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_etl;
                public const string PropertyNameEtl = "Etl";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_deliveryDate;
                public const string PropertyNameDeliveryDate = "DeliveryDate";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_atl;
                public const string PropertyNameAtl = "Atl";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string ProductCode
                {
                set
                {
                if( String.Equals( m_productCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProductCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_productCode= value;
                OnPropertyChanged(PropertyNameProductCode);
                }
                }
                get
                {
                return m_productCode;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string SupplierProductCode
                {
                set
                {
                if( String.Equals( m_supplierProductCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSupplierProductCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_supplierProductCode= value;
                OnPropertyChanged(PropertyNameSupplierProductCode);
                }
                }
                get
                {
                return m_supplierProductCode;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string DeliveryNote
                {
                set
                {
                if( String.Equals( m_deliveryNote, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDeliveryNote, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_deliveryNote= value;
                OnPropertyChanged(PropertyNameDeliveryNote);
                }
                }
                get
                {
                return m_deliveryNote;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Uom
                {
                set
                {
                if( String.Equals( m_uom, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUom, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_uom= value;
                OnPropertyChanged(PropertyNameUom);
                }
                }
                get
                {
                return m_uom;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Depot
                {
                set
                {
                if( String.Equals( m_depot, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDepot, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_depot= value;
                OnPropertyChanged(PropertyNameDepot);
                }
                }
                get
                {
                return m_depot;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string VehicleNo
                {
                set
                {
                if( String.Equals( m_vehicleNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVehicleNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_vehicleNo= value;
                OnPropertyChanged(PropertyNameVehicleNo);
                }
                }
                get
                {
                return m_vehicleNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public OrderStatus Status
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
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal UnitPrice
                {
                set
                {
                if( m_unitPrice == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUnitPrice, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_unitPrice= value;
                OnPropertyChanged(PropertyNameUnitPrice);
                }
                }
                get
                {
                return m_unitPrice;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public int Quantity
                {
                set
                {
                if( m_quantity == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantity, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_quantity= value;
                OnPropertyChanged(PropertyNameQuantity);
                }
                }
                get
                {
                return m_quantity;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? Eta
            {
            set
            {
            if(m_eta == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameEta, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_eta= value;
            OnPropertyChanged(PropertyNameEta);
            }
            }
            get { return m_eta;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? Etl
            {
            set
            {
            if(m_etl == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameEtl, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_etl= value;
            OnPropertyChanged(PropertyNameEtl);
            }
            }
            get { return m_etl;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? DeliveryDate
            {
            set
            {
            if(m_deliveryDate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDeliveryDate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_deliveryDate= value;
            OnPropertyChanged(PropertyNameDeliveryDate);
            }
            }
            get { return m_deliveryDate;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? Atl
            {
            set
            {
            if(m_atl == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameAtl, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_atl= value;
            OnPropertyChanged(PropertyNameAtl);
            }
            }
            get { return m_atl;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("FileAttribute",  Namespace=Strings.DefaultNamespace)]
          public  partial class FileAttribute
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_fullPath;
                public const string PropertyNameFullPath = "FullPath";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_month;
                public const string PropertyNameMonth = "Month";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_year;
                public const string PropertyNameYear = "Year";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_monthName;
                public const string PropertyNameMonthName = "MonthName";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string FullPath
                {
                set
                {
                if( String.Equals( m_fullPath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFullPath, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fullPath= value;
                OnPropertyChanged(PropertyNameFullPath);
                }
                }
                get
                {
                return m_fullPath;}
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
            
                public int Month
                {
                set
                {
                if( m_month == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMonth, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_month= value;
                OnPropertyChanged(PropertyNameMonth);
                }
                }
                get
                {
                return m_month;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Year
                {
                set
                {
                if( m_year == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameYear, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_year= value;
                OnPropertyChanged(PropertyNameYear);
                }
                }
                get
                {
                return m_year;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string MonthName
                {
                set
                {
                if( String.Equals( m_monthName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMonthName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_monthName= value;
                OnPropertyChanged(PropertyNameMonthName);
                }
                }
                get
                {
                return m_monthName;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Pump",  Namespace=Strings.DefaultNamespace)]
          public  partial class Pump
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_pumpNo;
                public const string PropertyNamePumpNo = "PumpNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_island;
                public const string PropertyNameIsland = "Island";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_brand;
                public const string PropertyNameBrand = "Brand";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_model;
                public const string PropertyNameModel = "Model";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_digit;
                public const string PropertyNameDigit = "Digit";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_decimal;
                public const string PropertyNameDecimal = "Decimal";

         
          
			private readonly ObjectCollection<ProductHistory>  m_ProductHistoryCollection = new ObjectCollection<ProductHistory> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("ProductHistory", IsNullable = false)]
			public ObjectCollection<ProductHistory> ProductHistoryCollection
			{
			get{ return m_ProductHistoryCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string PumpNo
                {
                set
                {
                if( String.Equals( m_pumpNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePumpNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pumpNo= value;
                OnPropertyChanged(PropertyNamePumpNo);
                }
                }
                get
                {
                return m_pumpNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Island
                {
                set
                {
                if( String.Equals( m_island, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsland, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_island= value;
                OnPropertyChanged(PropertyNameIsland);
                }
                }
                get
                {
                return m_island;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Brand
                {
                set
                {
                if( String.Equals( m_brand, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBrand, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_brand= value;
                OnPropertyChanged(PropertyNameBrand);
                }
                }
                get
                {
                return m_brand;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Model
                {
                set
                {
                if( String.Equals( m_model, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameModel, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_model= value;
                OnPropertyChanged(PropertyNameModel);
                }
                }
                get
                {
                return m_model;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? Digit
            {
            set
            {
            if(m_digit == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDigit, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_digit= value;
            OnPropertyChanged(PropertyNameDigit);
            }
            }
            get { return m_digit;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? Decimal
            {
            set
            {
            if(m_decimal == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDecimal, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_decimal= value;
            OnPropertyChanged(PropertyNameDecimal);
            }
            }
            get { return m_decimal;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Product",  Namespace=Strings.DefaultNamespace)]
          public  partial class Product
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_code;
                public const string PropertyNameCode = "Code";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tix;
                public const string PropertyNameTix = "Tix";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_supplierId;
                public const string PropertyNameSupplierId = "SupplierId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_brand;
                public const string PropertyNameBrand = "Brand";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_measuringUnit;
                public const string PropertyNameMeasuringUnit = "MeasuringUnit";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isDiscontinued;
                public const string PropertyNameIsDiscontinued = "IsDiscontinued";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_supplierDeliveryCode;
                public const string PropertyNameSupplierDeliveryCode = "SupplierDeliveryCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_supplierOrderCode;
                public const string PropertyNameSupplierOrderCode = "SupplierOrderCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_supplierTix;
                public const string PropertyNameSupplierTix = "SupplierTix";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_quantityInStore;
                public const string PropertyNameQuantityInStore = "QuantityInStore";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_quantityOnShelf;
                public const string PropertyNameQuantityOnShelf = "QuantityOnShelf";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_minimumLevel;
                public const string PropertyNameMinimumLevel = "MinimumLevel";

         
          
			private readonly ObjectCollection<ProductPriceHistory>  m_ProductPriceHistoryCollection = new ObjectCollection<ProductPriceHistory> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("ProductPriceHistory", IsNullable = false)]
			public ObjectCollection<ProductPriceHistory> ProductPriceHistoryCollection
			{
			get{ return m_ProductPriceHistoryCollection;}
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
            
                public string Code
                {
                set
                {
                if( String.Equals( m_code, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_code= value;
                OnPropertyChanged(PropertyNameCode);
                }
                }
                get
                {
                return m_code;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Tix
                {
                set
                {
                if( String.Equals( m_tix, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTix, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tix= value;
                OnPropertyChanged(PropertyNameTix);
                }
                }
                get
                {
                return m_tix;}
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
            
                public int SupplierId
                {
                set
                {
                if( m_supplierId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSupplierId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_supplierId= value;
                OnPropertyChanged(PropertyNameSupplierId);
                }
                }
                get
                {
                return m_supplierId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Brand
                {
                set
                {
                if( String.Equals( m_brand, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBrand, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_brand= value;
                OnPropertyChanged(PropertyNameBrand);
                }
                }
                get
                {
                return m_brand;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string MeasuringUnit
                {
                set
                {
                if( String.Equals( m_measuringUnit, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMeasuringUnit, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_measuringUnit= value;
                OnPropertyChanged(PropertyNameMeasuringUnit);
                }
                }
                get
                {
                return m_measuringUnit;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Size
                {
                set
                {
                if( String.Equals( m_size, value, StringComparison.Ordinal)) return;
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
            
                public bool IsDiscontinued
                {
                set
                {
                if( m_isDiscontinued == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsDiscontinued, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isDiscontinued= value;
                OnPropertyChanged(PropertyNameIsDiscontinued);
                }
                }
                get
                {
                return m_isDiscontinued;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string SupplierDeliveryCode
                {
                set
                {
                if( String.Equals( m_supplierDeliveryCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSupplierDeliveryCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_supplierDeliveryCode= value;
                OnPropertyChanged(PropertyNameSupplierDeliveryCode);
                }
                }
                get
                {
                return m_supplierDeliveryCode;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string SupplierOrderCode
                {
                set
                {
                if( String.Equals( m_supplierOrderCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSupplierOrderCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_supplierOrderCode= value;
                OnPropertyChanged(PropertyNameSupplierOrderCode);
                }
                }
                get
                {
                return m_supplierOrderCode;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string SupplierTix
                {
                set
                {
                if( String.Equals( m_supplierTix, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSupplierTix, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_supplierTix= value;
                OnPropertyChanged(PropertyNameSupplierTix);
                }
                }
                get
                {
                return m_supplierTix;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public int QuantityInStore
                {
                set
                {
                if( m_quantityInStore == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantityInStore, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_quantityInStore= value;
                OnPropertyChanged(PropertyNameQuantityInStore);
                }
                }
                get
                {
                return m_quantityInStore;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public int QuantityOnShelf
                {
                set
                {
                if( m_quantityOnShelf == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantityOnShelf, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_quantityOnShelf= value;
                OnPropertyChanged(PropertyNameQuantityOnShelf);
                }
                }
                get
                {
                return m_quantityOnShelf;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? MinimumLevel
            {
            set
            {
            if(m_minimumLevel == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameMinimumLevel, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_minimumLevel= value;
            OnPropertyChanged(PropertyNameMinimumLevel);
            }
            }
            get { return m_minimumLevel;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ProductPriceHistory",  Namespace=Strings.DefaultNamespace)]
          public  partial class ProductPriceHistory
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_from;
                public const string PropertyNameFrom = "From";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_selling;
                public const string PropertyNameSelling = "Selling";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_buying;
                public const string PropertyNameBuying = "Buying";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_to;
                public const string PropertyNameTo = "To";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime From
                {
                set
                {
                if( m_from == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFrom, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_from= value;
                OnPropertyChanged(PropertyNameFrom);
                }
                }
                get
                {
                return m_from;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Selling
                {
                set
                {
                if( m_selling == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSelling, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_selling= value;
                OnPropertyChanged(PropertyNameSelling);
                }
                }
                get
                {
                return m_selling;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Buying
                {
                set
                {
                if( m_buying == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBuying, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_buying= value;
                OnPropertyChanged(PropertyNameBuying);
                }
                }
                get
                {
                return m_buying;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? To
            {
            set
            {
            if(m_to == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameTo, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_to= value;
            OnPropertyChanged(PropertyNameTo);
            }
            }
            get { return m_to;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("IslandGroupSale",  Namespace=Strings.DefaultNamespace)]
          public  partial class IslandGroupSale
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_island;
                public const string PropertyNameIsland = "Island";

              
			private readonly ObjectCollection<PumpSale>  m_PumpSaleCollection = new ObjectCollection<PumpSale> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("PumpSale", IsNullable = false)]
			public ObjectCollection<PumpSale> PumpSaleCollection
			{
			get{ return m_PumpSaleCollection;}
			}
		
			private readonly ObjectCollection<Submission>  m_SubmissionCollection = new ObjectCollection<Submission> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Submission", IsNullable = false)]
			public ObjectCollection<Submission> SubmissionCollection
			{
			get{ return m_SubmissionCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Island
                {
                set
                {
                if( String.Equals( m_island, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsland, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_island= value;
                OnPropertyChanged(PropertyNameIsland);
                }
                }
                get
                {
                return m_island;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("PumpSale",  Namespace=Strings.DefaultNamespace)]
          public  partial class PumpSale
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_product;
                public const string PropertyNameProduct = "Product";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_openingMeter;
                public const string PropertyNameOpeningMeter = "OpeningMeter";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_pump;
                public const string PropertyNamePump = "Pump";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_productPrice;
                public const string PropertyNameProductPrice = "ProductPrice";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private double?  m_closingMeter;
                public const string PropertyNameClosingMeter = "ClosingMeter";

         
          
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private double  m_PumpTest;
			public const string PropertyNamePumpTest = "PumpTest";

			public double PumpTest
			{
			get { return m_PumpTest; }

			set
			{
			

			if(m_PumpTest== value) return;

			m_PumpTest = value;
			ClearColumnError(PropertyNamePumpTest);
			OnPropertyChanged(PropertyNamePumpTest);
			}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Product
                {
                set
                {
                if( String.Equals( m_product, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProduct, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_product= value;
                OnPropertyChanged(PropertyNameProduct);
                }
                }
                get
                {
                return m_product;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                  [Range(0,9999999)]
                
                public double OpeningMeter
                {
                set
                {
                if( m_openingMeter == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOpeningMeter, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_openingMeter= value;
                OnPropertyChanged(PropertyNameOpeningMeter);
                }
                }
                get
                {
                return m_openingMeter;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                  [StringLength(10, MinimumLength = 1)]
                
                public string Pump
                {
                set
                {
                if( String.Equals( m_pump, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePump, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pump= value;
                OnPropertyChanged(PropertyNamePump);
                }
                }
                get
                {
                return m_pump;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                  [Range(0,50000)]
                
                public decimal ProductPrice
                {
                set
                {
                if( m_productPrice == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProductPrice, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_productPrice= value;
                OnPropertyChanged(PropertyNameProductPrice);
                }
                }
                get
                {
                return m_productPrice;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public double? ClosingMeter
            {
            set
            {
            if(m_closingMeter == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameClosingMeter, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_closingMeter= value;
            OnPropertyChanged(PropertyNameClosingMeter);
            }
            }
            get { return m_closingMeter;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Submission",  Namespace=Strings.DefaultNamespace)]
          public  partial class Submission
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_method;
                public const string PropertyNameMethod = "Method";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_received;
                public const string PropertyNameReceived = "Received";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_cashier;
                public const string PropertyNameCashier = "Cashier";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_amount;
                public const string PropertyNameAmount = "Amount";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Method
                {
                set
                {
                if( String.Equals( m_method, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMethod, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_method= value;
                OnPropertyChanged(PropertyNameMethod);
                }
                }
                get
                {
                return m_method;}
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
            
                public string Received
                {
                set
                {
                if( String.Equals( m_received, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceived, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_received= value;
                OnPropertyChanged(PropertyNameReceived);
                }
                }
                get
                {
                return m_received;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Cashier
                {
                set
                {
                if( String.Equals( m_cashier, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCashier, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_cashier= value;
                OnPropertyChanged(PropertyNameCashier);
                }
                }
                get
                {
                return m_cashier;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? Amount
            {
            set
            {
            if(m_amount == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_amount= value;
            OnPropertyChanged(PropertyNameAmount);
            }
            }
            get { return m_amount;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Shift",  Namespace=Strings.DefaultNamespace)]
          public  partial class Shift
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_shiftName;
                public const string PropertyNameShiftName = "ShiftName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_sequence;
                public const string PropertyNameSequence = "Sequence";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
			private readonly ObjectCollection<IslandGroupSale>  m_IslandGroupSaleCollection = new ObjectCollection<IslandGroupSale> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("IslandGroupSale", IsNullable = false)]
			public ObjectCollection<IslandGroupSale> IslandGroupSaleCollection
			{
			get{ return m_IslandGroupSaleCollection;}
			}
		
			private readonly ObjectCollection<Staff>  m_StaffCollection = new ObjectCollection<Staff> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Staff", IsNullable = false)]
			public ObjectCollection<Staff> StaffCollection
			{
			get{ return m_StaffCollection;}
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
		
			private readonly ObjectCollection<Sale>  m_SaleCollection = new ObjectCollection<Sale> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Sale", IsNullable = false)]
			public ObjectCollection<Sale> SaleCollection
			{
			get{ return m_SaleCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                  [StringLength(5, MinimumLength = 1)]
                
                public string ShiftName
                {
                set
                {
                if( String.Equals( m_shiftName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShiftName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shiftName= value;
                OnPropertyChanged(PropertyNameShiftName);
                }
                }
                get
                {
                return m_shiftName;}
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
            
                public int Sequence
                {
                set
                {
                if( m_sequence == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSequence, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_sequence= value;
                OnPropertyChanged(PropertyNameSequence);
                }
                }
                get
                {
                return m_sequence;}
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
          [XmlType("Tank",  Namespace=Strings.DefaultNamespace)]
          public  partial class Tank
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tankNo;
                public const string PropertyNameTankNo = "TankNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_capacity;
                public const string PropertyNameCapacity = "Capacity";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isActive;
                public const string PropertyNameIsActive = "IsActive";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_dippingLookup;
                public const string PropertyNameDippingLookup = "DippingLookup";

              
			private readonly ObjectCollection<ProductHistory>  m_ProductHistoryCollection = new ObjectCollection<ProductHistory> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("ProductHistory", IsNullable = false)]
			public ObjectCollection<ProductHistory> ProductHistoryCollection
			{
			get{ return m_ProductHistoryCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string TankNo
                {
                set
                {
                if( String.Equals( m_tankNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankNo= value;
                OnPropertyChanged(PropertyNameTankNo);
                }
                }
                get
                {
                return m_tankNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Capacity
                {
                set
                {
                if( m_capacity == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCapacity, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_capacity= value;
                OnPropertyChanged(PropertyNameCapacity);
                }
                }
                get
                {
                return m_capacity;}
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
                if( m_isActive == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsActive, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isActive= value;
                OnPropertyChanged(PropertyNameIsActive);
                }
                }
                get
                {
                return m_isActive;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string DippingLookup
                {
                set
                {
                if( String.Equals( m_dippingLookup, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDippingLookup, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dippingLookup= value;
                OnPropertyChanged(PropertyNameDippingLookup);
                }
                }
                get
                {
                return m_dippingLookup;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Dipping",  Namespace=Strings.DefaultNamespace)]
          public  partial class Dipping
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_tankId;
                public const string PropertyNameTankId = "TankId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tankNo;
                public const string PropertyNameTankNo = "TankNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isDayClosing;
                public const string PropertyNameIsDayClosing = "IsDayClosing";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_level;
                public const string PropertyNameLevel = "Level";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_volume;
                public const string PropertyNameVolume = "Volume";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_employeeId;
                public const string PropertyNameEmployeeId = "EmployeeId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_employeeName;
                public const string PropertyNameEmployeeName = "EmployeeName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_productCode;
                public const string PropertyNameProductCode = "ProductCode";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private double?  m_waterLevel;
                public const string PropertyNameWaterLevel = "WaterLevel";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_closingForDay;
                public const string PropertyNameClosingForDay = "ClosingForDay";

         
          
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
            
                public int TankId
                {
                set
                {
                if( m_tankId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankId= value;
                OnPropertyChanged(PropertyNameTankId);
                }
                }
                get
                {
                return m_tankId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string TankNo
                {
                set
                {
                if( String.Equals( m_tankNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankNo= value;
                OnPropertyChanged(PropertyNameTankNo);
                }
                }
                get
                {
                return m_tankNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsDayClosing
                {
                set
                {
                if( m_isDayClosing == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsDayClosing, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isDayClosing= value;
                OnPropertyChanged(PropertyNameIsDayClosing);
                }
                }
                get
                {
                return m_isDayClosing;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Level
                {
                set
                {
                if( m_level == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLevel, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_level= value;
                OnPropertyChanged(PropertyNameLevel);
                }
                }
                get
                {
                return m_level;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int EmployeeId
                {
                set
                {
                if( m_employeeId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmployeeId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_employeeId= value;
                OnPropertyChanged(PropertyNameEmployeeId);
                }
                }
                get
                {
                return m_employeeId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string EmployeeName
                {
                set
                {
                if( String.Equals( m_employeeName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmployeeName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_employeeName= value;
                OnPropertyChanged(PropertyNameEmployeeName);
                }
                }
                get
                {
                return m_employeeName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string ProductCode
                {
                set
                {
                if( String.Equals( m_productCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProductCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_productCode= value;
                OnPropertyChanged(PropertyNameProductCode);
                }
                }
                get
                {
                return m_productCode;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public double? WaterLevel
            {
            set
            {
            if(m_waterLevel == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameWaterLevel, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_waterLevel= value;
            OnPropertyChanged(PropertyNameWaterLevel);
            }
            }
            get { return m_waterLevel;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? ClosingForDay
            {
            set
            {
            if(m_closingForDay == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameClosingForDay, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_closingForDay= value;
            OnPropertyChanged(PropertyNameClosingForDay);
            }
            }
            get { return m_closingForDay;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Delivery",  Namespace=Strings.DefaultNamespace)]
          public  partial class Delivery
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_product;
                public const string PropertyNameProduct = "Product";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_volume;
                public const string PropertyNameVolume = "Volume";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_orderId;
                public const string PropertyNameOrderId = "OrderId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_orderNo;
                public const string PropertyNameOrderNo = "OrderNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_vehicleRegistration;
                public const string PropertyNameVehicleRegistration = "VehicleRegistration";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_driver;
                public const string PropertyNameDriver = "Driver";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_deliveryNoteNo;
                public const string PropertyNameDeliveryNoteNo = "DeliveryNoteNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isAdhoc;
                public const string PropertyNameIsAdhoc = "IsAdhoc";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_acceptedBy;
                public const string PropertyNameAcceptedBy = "AcceptedBy";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_inspectedBy;
                public const string PropertyNameInspectedBy = "InspectedBy";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
			private readonly ObjectCollection<DeliveryDistribution>  m_DeliveryDistributionCollection = new ObjectCollection<DeliveryDistribution> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("DeliveryDistribution", IsNullable = false)]
			public ObjectCollection<DeliveryDistribution> DeliveryDistributionCollection
			{
			get{ return m_DeliveryDistributionCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private AdhocDelivery m_adhocDelivery
					=  new AdhocDelivery();
				
			public const string PropertyNameAdhocDelivery = "AdhocDelivery";
			[DebuggerHidden]

			public AdhocDelivery AdhocDelivery
			{
			get{ return m_adhocDelivery;}
			set
			{
			m_adhocDelivery = value;
			OnPropertyChanged(PropertyNameAdhocDelivery);
			}
			}
		
			private readonly ObjectCollection<PumpSale>  m_PumpSaleCollection = new ObjectCollection<PumpSale> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("PumpSale", IsNullable = false)]
			public ObjectCollection<PumpSale> PumpSaleCollection
			{
			get{ return m_PumpSaleCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Product
                {
                set
                {
                if( String.Equals( m_product, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProduct, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_product= value;
                OnPropertyChanged(PropertyNameProduct);
                }
                }
                get
                {
                return m_product;}
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
            
                public double Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int OrderId
                {
                set
                {
                if( m_orderId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrderId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_orderId= value;
                OnPropertyChanged(PropertyNameOrderId);
                }
                }
                get
                {
                return m_orderId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string OrderNo
                {
                set
                {
                if( String.Equals( m_orderNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrderNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_orderNo= value;
                OnPropertyChanged(PropertyNameOrderNo);
                }
                }
                get
                {
                return m_orderNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string VehicleRegistration
                {
                set
                {
                if( String.Equals( m_vehicleRegistration, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVehicleRegistration, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_vehicleRegistration= value;
                OnPropertyChanged(PropertyNameVehicleRegistration);
                }
                }
                get
                {
                return m_vehicleRegistration;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Driver
                {
                set
                {
                if( String.Equals( m_driver, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDriver, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_driver= value;
                OnPropertyChanged(PropertyNameDriver);
                }
                }
                get
                {
                return m_driver;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string DeliveryNoteNo
                {
                set
                {
                if( String.Equals( m_deliveryNoteNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDeliveryNoteNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_deliveryNoteNo= value;
                OnPropertyChanged(PropertyNameDeliveryNoteNo);
                }
                }
                get
                {
                return m_deliveryNoteNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsAdhoc
                {
                set
                {
                if( m_isAdhoc == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAdhoc, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isAdhoc= value;
                OnPropertyChanged(PropertyNameIsAdhoc);
                }
                }
                get
                {
                return m_isAdhoc;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string AcceptedBy
                {
                set
                {
                if( String.Equals( m_acceptedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAcceptedBy, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_acceptedBy= value;
                OnPropertyChanged(PropertyNameAcceptedBy);
                }
                }
                get
                {
                return m_acceptedBy;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string InspectedBy
                {
                set
                {
                if( String.Equals( m_inspectedBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInspectedBy, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_inspectedBy= value;
                OnPropertyChanged(PropertyNameInspectedBy);
                }
                }
                get
                {
                return m_inspectedBy;}
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
          [XmlType("Supplier",  Namespace=Strings.DefaultNamespace)]
          public  partial class Supplier
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_code;
                public const string PropertyNameCode = "Code";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_creditLimit;
                public const string PropertyNameCreditLimit = "CreditLimit";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private string  m_contactName;
                public const string PropertyNameContactName = "ContactName";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private string  m_contactTelephoneNo;
                public const string PropertyNameContactTelephoneNo = "ContactTelephoneNo";

         
          
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
            
                public string Code
                {
                set
                {
                if( String.Equals( m_code, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_code= value;
                OnPropertyChanged(PropertyNameCode);
                }
                }
                get
                {
                return m_code;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? CreditLimit
            {
            set
            {
            if(m_creditLimit == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameCreditLimit, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_creditLimit= value;
            OnPropertyChanged(PropertyNameCreditLimit);
            }
            }
            get { return m_creditLimit;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public string ContactName
            {
            set
            {
            if(String.Equals( m_contactName, value, StringComparison.Ordinal)) return;
            var arg = new PropertyChangingEventArgs(PropertyNameContactName, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_contactName= value;
            OnPropertyChanged(PropertyNameContactName);
            }
            }
            get { return m_contactName;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public string ContactTelephoneNo
            {
            set
            {
            if(String.Equals( m_contactTelephoneNo, value, StringComparison.Ordinal)) return;
            var arg = new PropertyChangingEventArgs(PropertyNameContactTelephoneNo, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_contactTelephoneNo= value;
            OnPropertyChanged(PropertyNameContactTelephoneNo);
            }
            }
            get { return m_contactTelephoneNo;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DippingLookup",  Namespace=Strings.DefaultNamespace)]
          public  partial class DippingLookup
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_level;
                public const string PropertyNameLevel = "Level";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_volume;
                public const string PropertyNameVolume = "Volume";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tankType;
                public const string PropertyNameTankType = "TankType";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Level
                {
                set
                {
                if( m_level == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLevel, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_level= value;
                OnPropertyChanged(PropertyNameLevel);
                }
                }
                get
                {
                return m_level;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string TankType
                {
                set
                {
                if( String.Equals( m_tankType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankType= value;
                OnPropertyChanged(PropertyNameTankType);
                }
                }
                get
                {
                return m_tankType;}
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
                private  string  m_email;
                public const string PropertyNameEmail = "Email";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_emergencyContactName1;
                public const string PropertyNameEmergencyContactName1 = "EmergencyContactName1";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_emergencyContactName2;
                public const string PropertyNameEmergencyContactName2 = "EmergencyContactName2";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_fullName;
                public const string PropertyNameFullName = "FullName";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
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

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string EmergencyContactName1
                {
                set
                {
                if( String.Equals( m_emergencyContactName1, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmergencyContactName1, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_emergencyContactName1= value;
                OnPropertyChanged(PropertyNameEmergencyContactName1);
                }
                }
                get
                {
                return m_emergencyContactName1;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string EmergencyContactName2
                {
                set
                {
                if( String.Equals( m_emergencyContactName2, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmergencyContactName2, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_emergencyContactName2= value;
                OnPropertyChanged(PropertyNameEmergencyContactName2);
                }
                }
                get
                {
                return m_emergencyContactName2;}
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
                if( String.Equals( m_fullName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFullName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fullName= value;
                OnPropertyChanged(PropertyNameFullName);
                }
                }
                get
                {
                return m_fullName;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Telephone",  Namespace=Strings.DefaultNamespace)]
          public  partial class Telephone
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_areaCode;
                public const string PropertyNameAreaCode = "AreaCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_extension;
                public const string PropertyNameExtension = "Extension";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_noEmergency1;
                public const string PropertyNameNoEmergency1 = "NoEmergency1";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_noEmergency2;
                public const string PropertyNameNoEmergency2 = "NoEmergency2";

              
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
            
                public string AreaCode
                {
                set
                {
                if( String.Equals( m_areaCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAreaCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_areaCode= value;
                OnPropertyChanged(PropertyNameAreaCode);
                }
                }
                get
                {
                return m_areaCode;}
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

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string NoEmergency1
                {
                set
                {
                if( String.Equals( m_noEmergency1, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNoEmergency1, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_noEmergency1= value;
                OnPropertyChanged(PropertyNameNoEmergency1);
                }
                }
                get
                {
                return m_noEmergency1;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string NoEmergency2
                {
                set
                {
                if( String.Equals( m_noEmergency2, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNoEmergency2, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_noEmergency2= value;
                OnPropertyChanged(PropertyNameNoEmergency2);
                }
                }
                get
                {
                return m_noEmergency2;}
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
                private  string  m_street;
                public const string PropertyNameStreet = "Street";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_city;
                public const string PropertyNameCity = "City";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_postcode;
                public const string PropertyNamePostcode = "Postcode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_state;
                public const string PropertyNameState = "State";

              
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
            
                public string Postcode
                {
                set
                {
                if( String.Equals( m_postcode, value, StringComparison.Ordinal)) return;
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

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Employee",  Namespace=Strings.DefaultNamespace)]
          public  partial class Employee
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_fullName;
                public const string PropertyNameFullName = "FullName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCashier;
                public const string PropertyNameIsCashier = "IsCashier";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_identificationNo;
                public const string PropertyNameIdentificationNo = "IdentificationNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_pictureStoreId;
                public const string PropertyNamePictureStoreId = "PictureStoreId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isActive;
                public const string PropertyNameIsActive = "IsActive";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_birthDate;
                public const string PropertyNameBirthDate = "BirthDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_birthPlace;
                public const string PropertyNameBirthPlace = "BirthPlace";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_nationality;
                public const string PropertyNameNationality = "Nationality";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_maritalStatus;
                public const string PropertyNameMaritalStatus = "MaritalStatus";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_title;
                public const string PropertyNameTitle = "Title";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_gender;
                public const string PropertyNameGender = "Gender";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_icNo;
                public const string PropertyNameIcNo = "IcNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_epfNo;
                public const string PropertyNameEpfNo = "EpfNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_socsoNo;
                public const string PropertyNameSocsoNo = "SocsoNo";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private bool  m_isSpouseWorking;
                public const string PropertyNameIsSpouseWorking = "IsSpouseWorking";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int  m_childCount;
                public const string PropertyNameChildCount = "ChildCount";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_epfRate;
                public const string PropertyNameEpfRate = "EpfRate";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_socsoRate;
                public const string PropertyNameSocsoRate = "SocsoRate";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_employerEpfRate;
                public const string PropertyNameEmployerEpfRate = "EmployerEpfRate";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_employerSocsoRate;
                public const string PropertyNameEmployerSocsoRate = "EmployerSocsoRate";

         
          
			private readonly ObjectCollection<Attachment>  m_AttachmentCollection = new ObjectCollection<Attachment> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Attachment", IsNullable = false)]
			public ObjectCollection<Attachment> AttachmentCollection
			{
			get{ return m_AttachmentCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Employment m_employment
					=  new Employment();
				
			public const string PropertyNameEmployment = "Employment";
			[DebuggerHidden]

			public Employment Employment
			{
			get{ return m_employment;}
			set
			{
			m_employment = value;
			OnPropertyChanged(PropertyNameEmployment);
			}
			}
		
			private readonly ObjectCollection<Telephone>  m_TelephoneCollection = new ObjectCollection<Telephone> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Telephone", IsNullable = false)]
			public ObjectCollection<Telephone> TelephoneCollection
			{
			get{ return m_TelephoneCollection;}
			}
		
			private readonly ObjectCollection<Contact>  m_ContactCollection = new ObjectCollection<Contact> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Contact", IsNullable = false)]
			public ObjectCollection<Contact> ContactCollection
			{
			get{ return m_ContactCollection;}
			}
		
			private readonly ObjectCollection<Benefit>  m_BenefitCollection = new ObjectCollection<Benefit> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Benefit", IsNullable = false)]
			public ObjectCollection<Benefit> BenefitCollection
			{
			get{ return m_BenefitCollection;}
			}
		
			private readonly ObjectCollection<Document>  m_DocumentCollection = new ObjectCollection<Document> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Document", IsNullable = false)]
			public ObjectCollection<Document> DocumentCollection
			{
			get{ return m_DocumentCollection;}
			}
		
			private readonly ObjectCollection<EmployeeHistory>  m_EmployeeHistoryCollection = new ObjectCollection<EmployeeHistory> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("EmployeeHistory", IsNullable = false)]
			public ObjectCollection<EmployeeHistory> EmployeeHistoryCollection
			{
			get{ return m_EmployeeHistoryCollection;}
			}
		
			private readonly ObjectCollection<LeaveEntitlement>  m_LeaveEntitlementCollection = new ObjectCollection<LeaveEntitlement> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("LeaveEntitlement", IsNullable = false)]
			public ObjectCollection<LeaveEntitlement> LeaveEntitlementCollection
			{
			get{ return m_LeaveEntitlementCollection;}
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
                if( String.Equals( m_fullName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFullName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fullName= value;
                OnPropertyChanged(PropertyNameFullName);
                }
                }
                get
                {
                return m_fullName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsCashier
                {
                set
                {
                if( m_isCashier == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCashier, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCashier= value;
                OnPropertyChanged(PropertyNameIsCashier);
                }
                }
                get
                {
                return m_isCashier;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string IdentificationNo
                {
                set
                {
                if( String.Equals( m_identificationNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIdentificationNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_identificationNo= value;
                OnPropertyChanged(PropertyNameIdentificationNo);
                }
                }
                get
                {
                return m_identificationNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string PictureStoreId
                {
                set
                {
                if( String.Equals( m_pictureStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePictureStoreId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pictureStoreId= value;
                OnPropertyChanged(PropertyNamePictureStoreId);
                }
                }
                get
                {
                return m_pictureStoreId;}
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
                if( m_isActive == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsActive, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isActive= value;
                OnPropertyChanged(PropertyNameIsActive);
                }
                }
                get
                {
                return m_isActive;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime BirthDate
                {
                set
                {
                if( m_birthDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBirthDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_birthDate= value;
                OnPropertyChanged(PropertyNameBirthDate);
                }
                }
                get
                {
                return m_birthDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string BirthPlace
                {
                set
                {
                if( String.Equals( m_birthPlace, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBirthPlace, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_birthPlace= value;
                OnPropertyChanged(PropertyNameBirthPlace);
                }
                }
                get
                {
                return m_birthPlace;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Nationality
                {
                set
                {
                if( String.Equals( m_nationality, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNationality, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_nationality= value;
                OnPropertyChanged(PropertyNameNationality);
                }
                }
                get
                {
                return m_nationality;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string MaritalStatus
                {
                set
                {
                if( String.Equals( m_maritalStatus, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaritalStatus, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_maritalStatus= value;
                OnPropertyChanged(PropertyNameMaritalStatus);
                }
                }
                get
                {
                return m_maritalStatus;}
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
            
                public string Gender
                {
                set
                {
                if( String.Equals( m_gender, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameGender, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_gender= value;
                OnPropertyChanged(PropertyNameGender);
                }
                }
                get
                {
                return m_gender;}
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
            
            [DebuggerHidden]
            
                public string EpfNo
                {
                set
                {
                if( String.Equals( m_epfNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEpfNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_epfNo= value;
                OnPropertyChanged(PropertyNameEpfNo);
                }
                }
                get
                {
                return m_epfNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string SocsoNo
                {
                set
                {
                if( String.Equals( m_socsoNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSocsoNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_socsoNo= value;
                OnPropertyChanged(PropertyNameSocsoNo);
                }
                }
                get
                {
                return m_socsoNo;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public bool IsSpouseWorking
            {
            set
            {
            if(m_isSpouseWorking == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameIsSpouseWorking, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_isSpouseWorking= value;
            OnPropertyChanged(PropertyNameIsSpouseWorking);
            }
            }
            get { return m_isSpouseWorking;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int ChildCount
            {
            set
            {
            if(m_childCount == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameChildCount, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_childCount= value;
            OnPropertyChanged(PropertyNameChildCount);
            }
            }
            get { return m_childCount;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? EpfRate
            {
            set
            {
            if(m_epfRate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameEpfRate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_epfRate= value;
            OnPropertyChanged(PropertyNameEpfRate);
            }
            }
            get { return m_epfRate;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? SocsoRate
            {
            set
            {
            if(m_socsoRate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameSocsoRate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_socsoRate= value;
            OnPropertyChanged(PropertyNameSocsoRate);
            }
            }
            get { return m_socsoRate;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? EmployerEpfRate
            {
            set
            {
            if(m_employerEpfRate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameEmployerEpfRate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_employerEpfRate= value;
            OnPropertyChanged(PropertyNameEmployerEpfRate);
            }
            }
            get { return m_employerEpfRate;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? EmployerSocsoRate
            {
            set
            {
            if(m_employerSocsoRate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameEmployerSocsoRate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_employerSocsoRate= value;
            OnPropertyChanged(PropertyNameEmployerSocsoRate);
            }
            }
            get { return m_employerSocsoRate;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Staff",  Namespace=Strings.DefaultNamespace)]
          public  partial class Staff
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_employeeId;
                public const string PropertyNameEmployeeId = "EmployeeId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_fullName;
                public const string PropertyNameFullName = "FullName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_role;
                public const string PropertyNameRole = "Role";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_clockInTime;
                public const string PropertyNameClockInTime = "ClockInTime";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_clockOutTime;
                public const string PropertyNameClockOutTime = "ClockOutTime";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private string  m_status;
                public const string PropertyNameStatus = "Status";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_actualClockInTime;
                public const string PropertyNameActualClockInTime = "ActualClockInTime";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_actualClockOutTime;
                public const string PropertyNameActualClockOutTime = "ActualClockOutTime";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int EmployeeId
                {
                set
                {
                if( m_employeeId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmployeeId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_employeeId= value;
                OnPropertyChanged(PropertyNameEmployeeId);
                }
                }
                get
                {
                return m_employeeId;}
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
            
                public string FullName
                {
                set
                {
                if( String.Equals( m_fullName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFullName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fullName= value;
                OnPropertyChanged(PropertyNameFullName);
                }
                }
                get
                {
                return m_fullName;}
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
            [DebuggerHidden]

            public DateTime? ClockInTime
            {
            set
            {
            if(m_clockInTime == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameClockInTime, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_clockInTime= value;
            OnPropertyChanged(PropertyNameClockInTime);
            }
            }
            get { return m_clockInTime;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? ClockOutTime
            {
            set
            {
            if(m_clockOutTime == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameClockOutTime, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_clockOutTime= value;
            OnPropertyChanged(PropertyNameClockOutTime);
            }
            }
            get { return m_clockOutTime;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public string Status
            {
            set
            {
            if(String.Equals( m_status, value, StringComparison.Ordinal)) return;
            var arg = new PropertyChangingEventArgs(PropertyNameStatus, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_status= value;
            OnPropertyChanged(PropertyNameStatus);
            }
            }
            get { return m_status;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? ActualClockInTime
            {
            set
            {
            if(m_actualClockInTime == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameActualClockInTime, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_actualClockInTime= value;
            OnPropertyChanged(PropertyNameActualClockInTime);
            }
            }
            get { return m_actualClockInTime;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? ActualClockOutTime
            {
            set
            {
            if(m_actualClockOutTime == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameActualClockOutTime, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_actualClockOutTime= value;
            OnPropertyChanged(PropertyNameActualClockOutTime);
            }
            }
            get { return m_actualClockOutTime;}
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
                private  string  m_storeId;
                public const string PropertyNameStoreId = "StoreId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_extension;
                public const string PropertyNameExtension = "Extension";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  long  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
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

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public long Size
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
          [XmlType("Employment",  Namespace=Strings.DefaultNamespace)]
          public  partial class Employment
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateJoined;
                public const string PropertyNameDateJoined = "DateJoined";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_reason;
                public const string PropertyNameReason = "Reason";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_designation;
                public const string PropertyNameDesignation = "Designation";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bankName;
                public const string PropertyNameBankName = "BankName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bankAccountNo;
                public const string PropertyNameBankAccountNo = "BankAccountNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isOvertime;
                public const string PropertyNameIsOvertime = "IsOvertime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_overtimeRate;
                public const string PropertyNameOvertimeRate = "OvertimeRate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isShift;
                public const string PropertyNameIsShift = "IsShift";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_designationEffectiveDate;
                public const string PropertyNameDesignationEffectiveDate = "DesignationEffectiveDate";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_dateLeft;
                public const string PropertyNameDateLeft = "DateLeft";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime DateJoined
                {
                set
                {
                if( m_dateJoined == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateJoined, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dateJoined= value;
                OnPropertyChanged(PropertyNameDateJoined);
                }
                }
                get
                {
                return m_dateJoined;}
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
                if( String.Equals( m_reason, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReason, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_reason= value;
                OnPropertyChanged(PropertyNameReason);
                }
                }
                get
                {
                return m_reason;}
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
                if( String.Equals( m_designation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDesignation, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_designation= value;
                OnPropertyChanged(PropertyNameDesignation);
                }
                }
                get
                {
                return m_designation;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string BankName
                {
                set
                {
                if( String.Equals( m_bankName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBankName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bankName= value;
                OnPropertyChanged(PropertyNameBankName);
                }
                }
                get
                {
                return m_bankName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string BankAccountNo
                {
                set
                {
                if( String.Equals( m_bankAccountNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBankAccountNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bankAccountNo= value;
                OnPropertyChanged(PropertyNameBankAccountNo);
                }
                }
                get
                {
                return m_bankAccountNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsOvertime
                {
                set
                {
                if( m_isOvertime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsOvertime, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isOvertime= value;
                OnPropertyChanged(PropertyNameIsOvertime);
                }
                }
                get
                {
                return m_isOvertime;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double OvertimeRate
                {
                set
                {
                if( m_overtimeRate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOvertimeRate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_overtimeRate= value;
                OnPropertyChanged(PropertyNameOvertimeRate);
                }
                }
                get
                {
                return m_overtimeRate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsShift
                {
                set
                {
                if( m_isShift == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsShift, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isShift= value;
                OnPropertyChanged(PropertyNameIsShift);
                }
                }
                get
                {
                return m_isShift;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime DesignationEffectiveDate
                {
                set
                {
                if( m_designationEffectiveDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDesignationEffectiveDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_designationEffectiveDate= value;
                OnPropertyChanged(PropertyNameDesignationEffectiveDate);
                }
                }
                get
                {
                return m_designationEffectiveDate;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? DateLeft
            {
            set
            {
            if(m_dateLeft == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDateLeft, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_dateLeft= value;
            OnPropertyChanged(PropertyNameDateLeft);
            }
            }
            get { return m_dateLeft;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("SubmissionLog",  Namespace=Strings.DefaultNamespace)]
          public  partial class SubmissionLog
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_user;
                public const string PropertyNameUser = "User";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_reson;
                public const string PropertyNameReson = "Reson";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_initialValue;
                public const string PropertyNameInitialValue = "InitialValue";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private decimal?  m_finalValue;
                public const string PropertyNameFinalValue = "FinalValue";

         
          
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
            
                public DateTime User
                {
                set
                {
                if( m_user == value) return;
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
            
                public DateTime Reson
                {
                set
                {
                if( m_reson == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReson, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_reson= value;
                OnPropertyChanged(PropertyNameReson);
                }
                }
                get
                {
                return m_reson;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? InitialValue
            {
            set
            {
            if(m_initialValue == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameInitialValue, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_initialValue= value;
            OnPropertyChanged(PropertyNameInitialValue);
            }
            }
            get { return m_initialValue;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public decimal? FinalValue
            {
            set
            {
            if(m_finalValue == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameFinalValue, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_finalValue= value;
            OnPropertyChanged(PropertyNameFinalValue);
            }
            }
            get { return m_finalValue;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Sale",  Namespace=Strings.DefaultNamespace)]
          public  partial class Sale
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_shiftId;
                public const string PropertyNameShiftId = "ShiftId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_shiftName;
                public const string PropertyNameShiftName = "ShiftName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_unit;
                public const string PropertyNameUnit = "Unit";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_price;
                public const string PropertyNamePrice = "Price";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_discount;
                public const string PropertyNameDiscount = "Discount";

              
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Product m_product
					=  new Product();
				
			public const string PropertyNameProduct = "Product";
			[DebuggerHidden]

			public Product Product
			{
			get{ return m_product;}
			set
			{
			m_product = value;
			OnPropertyChanged(PropertyNameProduct);
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
            
                public int ShiftId
                {
                set
                {
                if( m_shiftId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShiftId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shiftId= value;
                OnPropertyChanged(PropertyNameShiftId);
                }
                }
                get
                {
                return m_shiftId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string ShiftName
                {
                set
                {
                if( String.Equals( m_shiftName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShiftName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shiftName= value;
                OnPropertyChanged(PropertyNameShiftName);
                }
                }
                get
                {
                return m_shiftName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Unit
                {
                set
                {
                if( m_unit == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUnit, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_unit= value;
                OnPropertyChanged(PropertyNameUnit);
                }
                }
                get
                {
                return m_unit;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Price
                {
                set
                {
                if( m_price == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePrice, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_price= value;
                OnPropertyChanged(PropertyNamePrice);
                }
                }
                get
                {
                return m_price;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Discount
                {
                set
                {
                if( m_discount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDiscount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_discount= value;
                OnPropertyChanged(PropertyNameDiscount);
                }
                }
                get
                {
                return m_discount;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DeliveryDistribution",  Namespace=Strings.DefaultNamespace)]
          public  partial class DeliveryDistribution
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_tankId;
                public const string PropertyNameTankId = "TankId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tankNo;
                public const string PropertyNameTankNo = "TankNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_volume;
                public const string PropertyNameVolume = "Volume";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_dippingBefore;
                public const string PropertyNameDippingBefore = "DippingBefore";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_dippingAfter;
                public const string PropertyNameDippingAfter = "DippingAfter";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private double?  m_volumeBefore;
                public const string PropertyNameVolumeBefore = "VolumeBefore";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private double?  m_volumeAfter;
                public const string PropertyNameVolumeAfter = "VolumeAfter";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int TankId
                {
                set
                {
                if( m_tankId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankId= value;
                OnPropertyChanged(PropertyNameTankId);
                }
                }
                get
                {
                return m_tankId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string TankNo
                {
                set
                {
                if( String.Equals( m_tankNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankNo= value;
                OnPropertyChanged(PropertyNameTankNo);
                }
                }
                get
                {
                return m_tankNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? DippingBefore
            {
            set
            {
            if(m_dippingBefore == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDippingBefore, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_dippingBefore= value;
            OnPropertyChanged(PropertyNameDippingBefore);
            }
            }
            get { return m_dippingBefore;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? DippingAfter
            {
            set
            {
            if(m_dippingAfter == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDippingAfter, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_dippingAfter= value;
            OnPropertyChanged(PropertyNameDippingAfter);
            }
            }
            get { return m_dippingAfter;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public double? VolumeBefore
            {
            set
            {
            if(m_volumeBefore == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameVolumeBefore, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_volumeBefore= value;
            OnPropertyChanged(PropertyNameVolumeBefore);
            }
            }
            get { return m_volumeBefore;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public double? VolumeAfter
            {
            set
            {
            if(m_volumeAfter == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameVolumeAfter, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_volumeAfter= value;
            OnPropertyChanged(PropertyNameVolumeAfter);
            }
            }
            get { return m_volumeAfter;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("AdhocDelivery",  Namespace=Strings.DefaultNamespace)]
          public  partial class AdhocDelivery
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_orderNo;
                public const string PropertyNameOrderNo = "OrderNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_orgininalDestination;
                public const string PropertyNameOrgininalDestination = "OrgininalDestination";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_product;
                public const string PropertyNameProduct = "Product";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_volume;
                public const string PropertyNameVolume = "Volume";

              
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
            
                public string OrderNo
                {
                set
                {
                if( String.Equals( m_orderNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrderNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_orderNo= value;
                OnPropertyChanged(PropertyNameOrderNo);
                }
                }
                get
                {
                return m_orderNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string OrgininalDestination
                {
                set
                {
                if( String.Equals( m_orgininalDestination, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrgininalDestination, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_orgininalDestination= value;
                OnPropertyChanged(PropertyNameOrgininalDestination);
                }
                }
                get
                {
                return m_orgininalDestination;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Product
                {
                set
                {
                if( String.Equals( m_product, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProduct, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_product= value;
                OnPropertyChanged(PropertyNameProduct);
                }
                }
                get
                {
                return m_product;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Account",  Namespace=Strings.DefaultNamespace)]
          public  partial class Account
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_openingBalance;
                public const string PropertyNameOpeningBalance = "OpeningBalance";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  AccountType  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_description;
                public const string PropertyNameDescription = "Description";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_category;
                public const string PropertyNameCategory = "Category";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_subCategory;
                public const string PropertyNameSubCategory = "SubCategory";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isActive;
                public const string PropertyNameIsActive = "IsActive";

              
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
            
                public decimal OpeningBalance
                {
                set
                {
                if( m_openingBalance == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOpeningBalance, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_openingBalance= value;
                OnPropertyChanged(PropertyNameOpeningBalance);
                }
                }
                get
                {
                return m_openingBalance;}
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
            
              [Required]
            
            [DebuggerHidden]
            
                public AccountType Type
                {
                set
                {
                if( m_type == value) return;
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
            
                public string Description
                {
                set
                {
                if( String.Equals( m_description, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDescription, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_description= value;
                OnPropertyChanged(PropertyNameDescription);
                }
                }
                get
                {
                return m_description;}
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
            
                public string SubCategory
                {
                set
                {
                if( String.Equals( m_subCategory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubCategory, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_subCategory= value;
                OnPropertyChanged(PropertyNameSubCategory);
                }
                }
                get
                {
                return m_subCategory;}
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
                if( m_isActive == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsActive, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isActive= value;
                OnPropertyChanged(PropertyNameIsActive);
                }
                }
                get
                {
                return m_isActive;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("BankingAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class BankingAccount
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bank;
                public const string PropertyNameBank = "Bank";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_overdraft;
                public const string PropertyNameOverdraft = "Overdraft";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isChecking;
                public const string PropertyNameIsChecking = "IsChecking";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isSaving;
                public const string PropertyNameIsSaving = "IsSaving";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_branch;
                public const string PropertyNameBranch = "Branch";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCash;
                public const string PropertyNameIsCash = "IsCash";

              
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
            
            [DebuggerHidden]
            
                public string Bank
                {
                set
                {
                if( String.Equals( m_bank, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBank, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bank= value;
                OnPropertyChanged(PropertyNameBank);
                }
                }
                get
                {
                return m_bank;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Overdraft
                {
                set
                {
                if( m_overdraft == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOverdraft, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_overdraft= value;
                OnPropertyChanged(PropertyNameOverdraft);
                }
                }
                get
                {
                return m_overdraft;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsChecking
                {
                set
                {
                if( m_isChecking == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsChecking, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isChecking= value;
                OnPropertyChanged(PropertyNameIsChecking);
                }
                }
                get
                {
                return m_isChecking;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsSaving
                {
                set
                {
                if( m_isSaving == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsSaving, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isSaving= value;
                OnPropertyChanged(PropertyNameIsSaving);
                }
                }
                get
                {
                return m_isSaving;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Branch
                {
                set
                {
                if( String.Equals( m_branch, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBranch, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_branch= value;
                OnPropertyChanged(PropertyNameBranch);
                }
                }
                get
                {
                return m_branch;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsCash
                {
                set
                {
                if( m_isCash == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCash, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCash= value;
                OnPropertyChanged(PropertyNameIsCash);
                }
                }
                get
                {
                return m_isCash;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("EquityAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class EquityAccount
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_telephone;
                public const string PropertyNameTelephone = "Telephone";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_relationship;
                public const string PropertyNameRelationship = "Relationship";

              
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
            
            [DebuggerHidden]
            
                public string Telephone
                {
                set
                {
                if( String.Equals( m_telephone, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTelephone, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_telephone= value;
                OnPropertyChanged(PropertyNameTelephone);
                }
                }
                get
                {
                return m_telephone;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Relationship
                {
                set
                {
                if( String.Equals( m_relationship, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRelationship, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_relationship= value;
                OnPropertyChanged(PropertyNameRelationship);
                }
                }
                get
                {
                return m_relationship;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("PayableAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class PayableAccount
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_providerList;
                public const string PropertyNameProviderList = "ProviderList";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCostOfSale;
                public const string PropertyNameIsCostOfSale = "IsCostOfSale";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_statementDay;
                public const string PropertyNameStatementDay = "StatementDay";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_dueDay;
                public const string PropertyNameDueDay = "DueDay";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isOnAlert;
                public const string PropertyNameIsOnAlert = "IsOnAlert";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string ProviderList
                {
                set
                {
                if( String.Equals( m_providerList, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProviderList, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_providerList= value;
                OnPropertyChanged(PropertyNameProviderList);
                }
                }
                get
                {
                return m_providerList;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsCostOfSale
                {
                set
                {
                if( m_isCostOfSale == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCostOfSale, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCostOfSale= value;
                OnPropertyChanged(PropertyNameIsCostOfSale);
                }
                }
                get
                {
                return m_isCostOfSale;}
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

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public int StatementDay
                {
                set
                {
                if( m_statementDay == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatementDay, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_statementDay= value;
                OnPropertyChanged(PropertyNameStatementDay);
                }
                }
                get
                {
                return m_statementDay;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public int DueDay
                {
                set
                {
                if( m_dueDay == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDueDay, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dueDay= value;
                OnPropertyChanged(PropertyNameDueDay);
                }
                }
                get
                {
                return m_dueDay;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public bool IsOnAlert
                {
                set
                {
                if( m_isOnAlert == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsOnAlert, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isOnAlert= value;
                OnPropertyChanged(PropertyNameIsOnAlert);
                }
                }
                get
                {
                return m_isOnAlert;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ReceivableAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class ReceivableAccount
          {

          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("FixAssetAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class FixAssetAccount
          {

          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Transaction",  Namespace=Strings.DefaultNamespace)]
          public  partial class Transaction
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_from;
                public const string PropertyNameFrom = "From";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_to;
                public const string PropertyNameTo = "To";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCancelled;
                public const string PropertyNameIsCancelled = "IsCancelled";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_subType;
                public const string PropertyNameSubType = "SubType";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCompleted;
                public const string PropertyNameIsCompleted = "IsCompleted";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int From
                {
                set
                {
                if( m_from == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFrom, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_from= value;
                OnPropertyChanged(PropertyNameFrom);
                }
                }
                get
                {
                return m_from;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int To
                {
                set
                {
                if( m_to == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_to= value;
                OnPropertyChanged(PropertyNameTo);
                }
                }
                get
                {
                return m_to;}
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
            
                public bool IsCancelled
                {
                set
                {
                if( m_isCancelled == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCancelled, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCancelled= value;
                OnPropertyChanged(PropertyNameIsCancelled);
                }
                }
                get
                {
                return m_isCancelled;}
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
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
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
            
            [DebuggerHidden]
            
                public string SubType
                {
                set
                {
                if( String.Equals( m_subType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_subType= value;
                OnPropertyChanged(PropertyNameSubType);
                }
                }
                get
                {
                return m_subType;}
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
                if( m_isCompleted == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCompleted, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCompleted= value;
                OnPropertyChanged(PropertyNameIsCompleted);
                }
                }
                get
                {
                return m_isCompleted;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Billing",  Namespace=Strings.DefaultNamespace)]
          public  partial class Billing
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_chequeNo;
                public const string PropertyNameChequeNo = "ChequeNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_expensesAccountId;
                public const string PropertyNameExpensesAccountId = "ExpensesAccountId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_payingAccountId;
                public const string PropertyNamePayingAccountId = "PayingAccountId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_purpose;
                public const string PropertyNamePurpose = "Purpose";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isPaid;
                public const string PropertyNameIsPaid = "IsPaid";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_remarks;
                public const string PropertyNameRemarks = "Remarks";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateOfCheque;
                public const string PropertyNameDateOfCheque = "DateOfCheque";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_paymentMethod;
                public const string PropertyNamePaymentMethod = "PaymentMethod";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_pay;
                public const string PropertyNamePay = "Pay";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_paymentTransationId;
                public const string PropertyNamePaymentTransationId = "PaymentTransationId";

         
          
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_dueDate;
                public const string PropertyNameDueDate = "DueDate";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string ChequeNo
                {
                set
                {
                if( String.Equals( m_chequeNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameChequeNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_chequeNo= value;
                OnPropertyChanged(PropertyNameChequeNo);
                }
                }
                get
                {
                return m_chequeNo;}
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
            
                public decimal Amount
                {
                set
                {
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int ExpensesAccountId
                {
                set
                {
                if( m_expensesAccountId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpensesAccountId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_expensesAccountId= value;
                OnPropertyChanged(PropertyNameExpensesAccountId);
                }
                }
                get
                {
                return m_expensesAccountId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int PayingAccountId
                {
                set
                {
                if( m_payingAccountId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePayingAccountId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_payingAccountId= value;
                OnPropertyChanged(PropertyNamePayingAccountId);
                }
                }
                get
                {
                return m_payingAccountId;}
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
            
                public bool IsPaid
                {
                set
                {
                if( m_isPaid == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPaid, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isPaid= value;
                OnPropertyChanged(PropertyNameIsPaid);
                }
                }
                get
                {
                return m_isPaid;}
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
                if( String.Equals( m_remarks, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRemarks, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_remarks= value;
                OnPropertyChanged(PropertyNameRemarks);
                }
                }
                get
                {
                return m_remarks;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public DateTime DateOfCheque
                {
                set
                {
                if( m_dateOfCheque == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateOfCheque, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dateOfCheque= value;
                OnPropertyChanged(PropertyNameDateOfCheque);
                }
                }
                get
                {
                return m_dateOfCheque;}
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
            
                public string PaymentMethod
                {
                set
                {
                if( String.Equals( m_paymentMethod, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePaymentMethod, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_paymentMethod= value;
                OnPropertyChanged(PropertyNamePaymentMethod);
                }
                }
                get
                {
                return m_paymentMethod;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Pay
                {
                set
                {
                if( String.Equals( m_pay, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePay, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pay= value;
                OnPropertyChanged(PropertyNamePay);
                }
                }
                get
                {
                return m_pay;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? PaymentTransationId
            {
            set
            {
            if(m_paymentTransationId == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNamePaymentTransationId, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_paymentTransationId= value;
            OnPropertyChanged(PropertyNamePaymentTransationId);
            }
            }
            get { return m_paymentTransationId;}
            }
          

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? DueDate
            {
            set
            {
            if(m_dueDate == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameDueDate, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_dueDate= value;
            OnPropertyChanged(PropertyNameDueDate);
            }
            }
            get { return m_dueDate;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Reminder",  Namespace=Strings.DefaultNamespace)]
          public  partial class Reminder
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_occurence;
                public const string PropertyNameOccurence = "Occurence";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_title;
                public const string PropertyNameTitle = "Title";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isDismissed;
                public const string PropertyNameIsDismissed = "IsDismissed";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_accountId;
                public const string PropertyNameAccountId = "AccountId";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Occurence
                {
                set
                {
                if( String.Equals( m_occurence, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOccurence, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_occurence= value;
                OnPropertyChanged(PropertyNameOccurence);
                }
                }
                get
                {
                return m_occurence;}
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
            
                public bool IsDismissed
                {
                set
                {
                if( m_isDismissed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsDismissed, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isDismissed= value;
                OnPropertyChanged(PropertyNameIsDismissed);
                }
                }
                get
                {
                return m_isDismissed;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public int? AccountId
            {
            set
            {
            if(m_accountId == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameAccountId, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_accountId= value;
            OnPropertyChanged(PropertyNameAccountId);
            }
            }
            get { return m_accountId;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Setting",  Namespace=Strings.DefaultNamespace)]
          public  partial class Setting
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_key;
                public const string PropertyNameKey = "Key";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_value;
                public const string PropertyNameValue = "Value";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_userName;
                public const string PropertyNameUserName = "UserName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Key
                {
                set
                {
                if( String.Equals( m_key, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameKey, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_key= value;
                OnPropertyChanged(PropertyNameKey);
                }
                }
                get
                {
                return m_key;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Value
                {
                set
                {
                if( String.Equals( m_value, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_value= value;
                OnPropertyChanged(PropertyNameValue);
                }
                }
                get
                {
                return m_value;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string UserName
                {
                set
                {
                if( String.Equals( m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_userName= value;
                OnPropertyChanged(PropertyNameUserName);
                }
                }
                get
                {
                return m_userName;}
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

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ProductHistory",  Namespace=Strings.DefaultNamespace)]
          public  partial class ProductHistory
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_productCode;
                public const string PropertyNameProductCode = "ProductCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_from;
                public const string PropertyNameFrom = "From";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tankDippingLookup;
                public const string PropertyNameTankDippingLookup = "TankDippingLookup";

              
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private DateTime?  m_to;
                public const string PropertyNameTo = "To";

         
          
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string ProductCode
                {
                set
                {
                if( String.Equals( m_productCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProductCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_productCode= value;
                OnPropertyChanged(PropertyNameProductCode);
                }
                }
                get
                {
                return m_productCode;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime From
                {
                set
                {
                if( m_from == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFrom, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_from= value;
                OnPropertyChanged(PropertyNameFrom);
                }
                }
                get
                {
                return m_from;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string TankDippingLookup
                {
                set
                {
                if( String.Equals( m_tankDippingLookup, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankDippingLookup, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankDippingLookup= value;
                OnPropertyChanged(PropertyNameTankDippingLookup);
                }
                }
                get
                {
                return m_tankDippingLookup;}
                }

              

            ///<summary>
            /// 
            ///</summary>
            [DebuggerHidden]

            public DateTime? To
            {
            set
            {
            if(m_to == value) return;
            var arg = new PropertyChangingEventArgs(PropertyNameTo, value);
            OnPropertyChanging(arg);
            if(! arg.Cancel)
            {
            m_to= value;
            OnPropertyChanged(PropertyNameTo);
            }
            }
            get { return m_to;}
            }
          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("LogEntry",  Namespace=Strings.DefaultNamespace)]
          public  partial class LogEntry
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_userName;
                public const string PropertyNameUserName = "UserName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  LogLevel  m_level;
                public const string PropertyNameLevel = "Level";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_computer;
                public const string PropertyNameComputer = "Computer";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_source;
                public const string PropertyNameSource = "Source";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_message;
                public const string PropertyNameMessage = "Message";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_ipAddress;
                public const string PropertyNameIpAddress = "IpAddress";

              
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
            
                public string UserName
                {
                set
                {
                if( String.Equals( m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_userName= value;
                OnPropertyChanged(PropertyNameUserName);
                }
                }
                get
                {
                return m_userName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public LogLevel Level
                {
                set
                {
                if( m_level == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLevel, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_level= value;
                OnPropertyChanged(PropertyNameLevel);
                }
                }
                get
                {
                return m_level;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Computer
                {
                set
                {
                if( String.Equals( m_computer, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComputer, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_computer= value;
                OnPropertyChanged(PropertyNameComputer);
                }
                }
                get
                {
                return m_computer;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Source
                {
                set
                {
                if( String.Equals( m_source, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSource, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_source= value;
                OnPropertyChanged(PropertyNameSource);
                }
                }
                get
                {
                return m_source;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Message
                {
                set
                {
                if( String.Equals( m_message, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMessage, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_message= value;
                OnPropertyChanged(PropertyNameMessage);
                }
                }
                get
                {
                return m_message;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string IpAddress
                {
                set
                {
                if( String.Equals( m_ipAddress, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIpAddress, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_ipAddress= value;
                OnPropertyChanged(PropertyNameIpAddress);
                }
                }
                get
                {
                return m_ipAddress;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ProfitAndLoss",  Namespace=Strings.DefaultNamespace)]
          public  partial class ProfitAndLoss
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_fromDate;
                public const string PropertyNameFromDate = "FromDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_toDate;
                public const string PropertyNameToDate = "ToDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_totalSales;
                public const string PropertyNameTotalSales = "TotalSales";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_totalPurchase;
                public const string PropertyNameTotalPurchase = "TotalPurchase";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime FromDate
                {
                set
                {
                if( m_fromDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFromDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fromDate= value;
                OnPropertyChanged(PropertyNameFromDate);
                }
                }
                get
                {
                return m_fromDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime ToDate
                {
                set
                {
                if( m_toDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameToDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_toDate= value;
                OnPropertyChanged(PropertyNameToDate);
                }
                }
                get
                {
                return m_toDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal TotalSales
                {
                set
                {
                if( m_totalSales == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotalSales, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_totalSales= value;
                OnPropertyChanged(PropertyNameTotalSales);
                }
                }
                get
                {
                return m_totalSales;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal TotalPurchase
                {
                set
                {
                if( m_totalPurchase == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotalPurchase, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_totalPurchase= value;
                OnPropertyChanged(PropertyNameTotalPurchase);
                }
                }
                get
                {
                return m_totalPurchase;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("InventoryItem",  Namespace=Strings.DefaultNamespace)]
          public  partial class InventoryItem
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_quantityInStore;
                public const string PropertyNameQuantityInStore = "QuantityInStore";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_quantityOnShelf;
                public const string PropertyNameQuantityOnShelf = "QuantityOnShelf";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_productCode;
                public const string PropertyNameProductCode = "ProductCode";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_size;
                public const string PropertyNameSize = "Size";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_currentPrice;
                public const string PropertyNameCurrentPrice = "CurrentPrice";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_category;
                public const string PropertyNameCategory = "Category";

              
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
            
                public int QuantityInStore
                {
                set
                {
                if( m_quantityInStore == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantityInStore, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_quantityInStore= value;
                OnPropertyChanged(PropertyNameQuantityInStore);
                }
                }
                get
                {
                return m_quantityInStore;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int QuantityOnShelf
                {
                set
                {
                if( m_quantityOnShelf == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuantityOnShelf, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_quantityOnShelf= value;
                OnPropertyChanged(PropertyNameQuantityOnShelf);
                }
                }
                get
                {
                return m_quantityOnShelf;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string ProductCode
                {
                set
                {
                if( String.Equals( m_productCode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameProductCode, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_productCode= value;
                OnPropertyChanged(PropertyNameProductCode);
                }
                }
                get
                {
                return m_productCode;}
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
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Size
                {
                set
                {
                if( String.Equals( m_size, value, StringComparison.Ordinal)) return;
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
            
                public decimal CurrentPrice
                {
                set
                {
                if( m_currentPrice == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCurrentPrice, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_currentPrice= value;
                OnPropertyChanged(PropertyNameCurrentPrice);
                }
                }
                get
                {
                return m_currentPrice;}
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

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Inventory",  Namespace=Strings.DefaultNamespace)]
          public  partial class Inventory
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_closingDate;
                public const string PropertyNameClosingDate = "ClosingDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_takenBy;
                public const string PropertyNameTakenBy = "TakenBy";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amount;
                public const string PropertyNameAmount = "Amount";

              
			private readonly ObjectCollection<InventoryItem>  m_InventoryItemCollection = new ObjectCollection<InventoryItem> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("InventoryItem", IsNullable = false)]
			public ObjectCollection<InventoryItem> InventoryItemCollection
			{
			get{ return m_InventoryItemCollection;}
			}
		
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime ClosingDate
                {
                set
                {
                if( m_closingDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameClosingDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_closingDate= value;
                OnPropertyChanged(PropertyNameClosingDate);
                }
                }
                get
                {
                return m_closingDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string TakenBy
                {
                set
                {
                if( String.Equals( m_takenBy, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTakenBy, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_takenBy= value;
                OnPropertyChanged(PropertyNameTakenBy);
                }
                }
                get
                {
                return m_takenBy;}
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
            
                public decimal Amount
                {
                set
                {
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ChequeRegistry",  Namespace=Strings.DefaultNamespace)]
          public  partial class ChequeRegistry
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_transactionDate;
                public const string PropertyNameTransactionDate = "TransactionDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCleared;
                public const string PropertyNameIsCleared = "IsCleared";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_account;
                public const string PropertyNameAccount = "Account";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
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
            
                public DateTime TransactionDate
                {
                set
                {
                if( m_transactionDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTransactionDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_transactionDate= value;
                OnPropertyChanged(PropertyNameTransactionDate);
                }
                }
                get
                {
                return m_transactionDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public bool IsCleared
                {
                set
                {
                if( m_isCleared == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCleared, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCleared= value;
                OnPropertyChanged(PropertyNameIsCleared);
                }
                }
                get
                {
                return m_isCleared;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public decimal Amount
                {
                set
                {
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public int Account
                {
                set
                {
                if( m_account == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_account= value;
                OnPropertyChanged(PropertyNameAccount);
                }
                }
                get
                {
                return m_account;}
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
          [XmlType("LoanAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class LoanAccount
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_startDate;
                public const string PropertyNameStartDate = "StartDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_endDate;
                public const string PropertyNameEndDate = "EndDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amountLoaned;
                public const string PropertyNameAmountLoaned = "AmountLoaned";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_montlyCommitment;
                public const string PropertyNameMontlyCommitment = "MontlyCommitment";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_dayofMonthDue;
                public const string PropertyNameDayofMonthDue = "DayofMonthDue";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_interestRate;
                public const string PropertyNameInterestRate = "InterestRate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bank;
                public const string PropertyNameBank = "Bank";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_branch;
                public const string PropertyNameBranch = "Branch";

              
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
                if( m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_startDate= value;
                OnPropertyChanged(PropertyNameStartDate);
                }
                }
                get
                {
                return m_startDate;}
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
                if( m_endDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_endDate= value;
                OnPropertyChanged(PropertyNameEndDate);
                }
                }
                get
                {
                return m_endDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal AmountLoaned
                {
                set
                {
                if( m_amountLoaned == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmountLoaned, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amountLoaned= value;
                OnPropertyChanged(PropertyNameAmountLoaned);
                }
                }
                get
                {
                return m_amountLoaned;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal MontlyCommitment
                {
                set
                {
                if( m_montlyCommitment == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMontlyCommitment, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_montlyCommitment= value;
                OnPropertyChanged(PropertyNameMontlyCommitment);
                }
                }
                get
                {
                return m_montlyCommitment;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int DayofMonthDue
                {
                set
                {
                if( m_dayofMonthDue == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDayofMonthDue, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dayofMonthDue= value;
                OnPropertyChanged(PropertyNameDayofMonthDue);
                }
                }
                get
                {
                return m_dayofMonthDue;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal InterestRate
                {
                set
                {
                if( m_interestRate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInterestRate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_interestRate= value;
                OnPropertyChanged(PropertyNameInterestRate);
                }
                }
                get
                {
                return m_interestRate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Bank
                {
                set
                {
                if( String.Equals( m_bank, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBank, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bank= value;
                OnPropertyChanged(PropertyNameBank);
                }
                }
                get
                {
                return m_bank;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Branch
                {
                set
                {
                if( String.Equals( m_branch, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBranch, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_branch= value;
                OnPropertyChanged(PropertyNameBranch);
                }
                }
                get
                {
                return m_branch;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("CreditCardAccount",  Namespace=Strings.DefaultNamespace)]
          public  partial class CreditCardAccount
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bank;
                public const string PropertyNameBank = "Bank";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_cardType;
                public const string PropertyNameCardType = "CardType";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_statementDay;
                public const string PropertyNameStatementDay = "StatementDay";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_dueDay;
                public const string PropertyNameDueDay = "DueDay";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Bank
                {
                set
                {
                if( String.Equals( m_bank, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBank, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bank= value;
                OnPropertyChanged(PropertyNameBank);
                }
                }
                get
                {
                return m_bank;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string CardType
                {
                set
                {
                if( String.Equals( m_cardType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCardType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_cardType= value;
                OnPropertyChanged(PropertyNameCardType);
                }
                }
                get
                {
                return m_cardType;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int StatementDay
                {
                set
                {
                if( m_statementDay == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatementDay, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_statementDay= value;
                OnPropertyChanged(PropertyNameStatementDay);
                }
                }
                get
                {
                return m_statementDay;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int DueDay
                {
                set
                {
                if( m_dueDay == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDueDay, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dueDay= value;
                OnPropertyChanged(PropertyNameDueDay);
                }
                }
                get
                {
                return m_dueDay;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DailySummary",  Namespace=Strings.DefaultNamespace)]
          public  partial class DailySummary
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_cash;
                public const string PropertyNameCash = "Cash";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_bank;
                public const string PropertyNameBank = "Bank";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_totalFuelInventory;
                public const string PropertyNameTotalFuelInventory = "TotalFuelInventory";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_debtor;
                public const string PropertyNameDebtor = "Debtor";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_creditor;
                public const string PropertyNameCreditor = "Creditor";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_unclearCheck;
                public const string PropertyNameUnclearCheck = "UnclearCheck";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_totalGroceryInventory;
                public const string PropertyNameTotalGroceryInventory = "TotalGroceryInventory";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_totalAsset;
                public const string PropertyNameTotalAsset = "TotalAsset";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_totalLiability;
                public const string PropertyNameTotalLiability = "TotalLiability";

              
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
            
                public decimal Cash
                {
                set
                {
                if( m_cash == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCash, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_cash= value;
                OnPropertyChanged(PropertyNameCash);
                }
                }
                get
                {
                return m_cash;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Bank
                {
                set
                {
                if( m_bank == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBank, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bank= value;
                OnPropertyChanged(PropertyNameBank);
                }
                }
                get
                {
                return m_bank;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal TotalFuelInventory
                {
                set
                {
                if( m_totalFuelInventory == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotalFuelInventory, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_totalFuelInventory= value;
                OnPropertyChanged(PropertyNameTotalFuelInventory);
                }
                }
                get
                {
                return m_totalFuelInventory;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Debtor
                {
                set
                {
                if( m_debtor == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDebtor, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_debtor= value;
                OnPropertyChanged(PropertyNameDebtor);
                }
                }
                get
                {
                return m_debtor;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal Creditor
                {
                set
                {
                if( m_creditor == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCreditor, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_creditor= value;
                OnPropertyChanged(PropertyNameCreditor);
                }
                }
                get
                {
                return m_creditor;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal UnclearCheck
                {
                set
                {
                if( m_unclearCheck == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUnclearCheck, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_unclearCheck= value;
                OnPropertyChanged(PropertyNameUnclearCheck);
                }
                }
                get
                {
                return m_unclearCheck;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal TotalGroceryInventory
                {
                set
                {
                if( m_totalGroceryInventory == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotalGroceryInventory, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_totalGroceryInventory= value;
                OnPropertyChanged(PropertyNameTotalGroceryInventory);
                }
                }
                get
                {
                return m_totalGroceryInventory;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal TotalAsset
                {
                set
                {
                if( m_totalAsset == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotalAsset, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_totalAsset= value;
                OnPropertyChanged(PropertyNameTotalAsset);
                }
                }
                get
                {
                return m_totalAsset;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal TotalLiability
                {
                set
                {
                if( m_totalLiability == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTotalLiability, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_totalLiability= value;
                OnPropertyChanged(PropertyNameTotalLiability);
                }
                }
                get
                {
                return m_totalLiability;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ShiftSchedule",  Namespace=Strings.DefaultNamespace)]
          public  partial class ShiftSchedule
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_startTime;
                public const string PropertyNameStartTime = "StartTime";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_endTime;
                public const string PropertyNameEndTime = "EndTime";

              
			private readonly ObjectCollection<Staff>  m_StaffCollection = new ObjectCollection<Staff> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Staff", IsNullable = false)]
			public ObjectCollection<Staff> StaffCollection
			{
			get{ return m_StaffCollection;}
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
            
                public DateTime StartTime
                {
                set
                {
                if( m_startTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartTime, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_startTime= value;
                OnPropertyChanged(PropertyNameStartTime);
                }
                }
                get
                {
                return m_startTime;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime EndTime
                {
                set
                {
                if( m_endTime == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndTime, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_endTime= value;
                OnPropertyChanged(PropertyNameEndTime);
                }
                }
                get
                {
                return m_endTime;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Attendance",  Namespace=Strings.DefaultNamespace)]
          public  partial class Attendance
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
			private readonly ObjectCollection<Staff>  m_StaffCollection = new ObjectCollection<Staff> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Staff", IsNullable = false)]
			public ObjectCollection<Staff> StaffCollection
			{
			get{ return m_StaffCollection;}
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

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Salary",  Namespace=Strings.DefaultNamespace)]
          public  partial class Salary
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_taxAmount;
                public const string PropertyNameTaxAmount = "TaxAmount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_pcbAmount;
                public const string PropertyNamePcbAmount = "PcbAmount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_zakatAmount;
                public const string PropertyNameZakatAmount = "ZakatAmount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_allowanceAmount;
                public const string PropertyNameAllowanceAmount = "AllowanceAmount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_period;
                public const string PropertyNamePeriod = "Period";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bankName;
                public const string PropertyNameBankName = "BankName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_bankAccountNo;
                public const string PropertyNameBankAccountNo = "BankAccountNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_epfAmount;
                public const string PropertyNameEpfAmount = "EpfAmount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_socsoAmount;
                public const string PropertyNameSocsoAmount = "SocsoAmount";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Amount
                {
                set
                {
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double TaxAmount
                {
                set
                {
                if( m_taxAmount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTaxAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_taxAmount= value;
                OnPropertyChanged(PropertyNameTaxAmount);
                }
                }
                get
                {
                return m_taxAmount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double PcbAmount
                {
                set
                {
                if( m_pcbAmount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePcbAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pcbAmount= value;
                OnPropertyChanged(PropertyNamePcbAmount);
                }
                }
                get
                {
                return m_pcbAmount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double ZakatAmount
                {
                set
                {
                if( m_zakatAmount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameZakatAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_zakatAmount= value;
                OnPropertyChanged(PropertyNameZakatAmount);
                }
                }
                get
                {
                return m_zakatAmount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double AllowanceAmount
                {
                set
                {
                if( m_allowanceAmount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowanceAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_allowanceAmount= value;
                OnPropertyChanged(PropertyNameAllowanceAmount);
                }
                }
                get
                {
                return m_allowanceAmount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Period
                {
                set
                {
                if( String.Equals( m_period, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriod, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_period= value;
                OnPropertyChanged(PropertyNamePeriod);
                }
                }
                get
                {
                return m_period;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string BankName
                {
                set
                {
                if( String.Equals( m_bankName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBankName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bankName= value;
                OnPropertyChanged(PropertyNameBankName);
                }
                }
                get
                {
                return m_bankName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int BankAccountNo
                {
                set
                {
                if( m_bankAccountNo == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBankAccountNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bankAccountNo= value;
                OnPropertyChanged(PropertyNameBankAccountNo);
                }
                }
                get
                {
                return m_bankAccountNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double EpfAmount
                {
                set
                {
                if( m_epfAmount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEpfAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_epfAmount= value;
                OnPropertyChanged(PropertyNameEpfAmount);
                }
                }
                get
                {
                return m_epfAmount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double SocsoAmount
                {
                set
                {
                if( m_socsoAmount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSocsoAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_socsoAmount= value;
                OnPropertyChanged(PropertyNameSocsoAmount);
                }
                }
                get
                {
                return m_socsoAmount;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("LeaveEntitlement",  Namespace=Strings.DefaultNamespace)]
          public  partial class LeaveEntitlement
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_day;
                public const string PropertyNameDay = "Day";

              
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
            
                public int Day
                {
                set
                {
                if( m_day == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDay, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_day= value;
                OnPropertyChanged(PropertyNameDay);
                }
                }
                get
                {
                return m_day;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Benefit",  Namespace=Strings.DefaultNamespace)]
          public  partial class Benefit
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_benefitType;
                public const string PropertyNameBenefitType = "BenefitType";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_description;
                public const string PropertyNameDescription = "Description";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string BenefitType
                {
                set
                {
                if( String.Equals( m_benefitType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBenefitType, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_benefitType= value;
                OnPropertyChanged(PropertyNameBenefitType);
                }
                }
                get
                {
                return m_benefitType;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Description
                {
                set
                {
                if( String.Equals( m_description, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDescription, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_description= value;
                OnPropertyChanged(PropertyNameDescription);
                }
                }
                get
                {
                return m_description;}
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
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_no;
                public const string PropertyNameNo = "No";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_expiryDate;
                public const string PropertyNameExpiryDate = "ExpiryDate";

              
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
            
                public DateTime ExpiryDate
                {
                set
                {
                if( m_expiryDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpiryDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_expiryDate= value;
                OnPropertyChanged(PropertyNameExpiryDate);
                }
                }
                get
                {
                return m_expiryDate;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("LeaveSchedule",  Namespace=Strings.DefaultNamespace)]
          public  partial class LeaveSchedule
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_name;
                public const string PropertyNameName = "Name";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_fromDate;
                public const string PropertyNameFromDate = "FromDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_toDate;
                public const string PropertyNameToDate = "ToDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_period;
                public const string PropertyNamePeriod = "Period";

              
			private readonly ObjectCollection<Employee>  m_EmployeeCollection = new ObjectCollection<Employee> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Employee", IsNullable = false)]
			public ObjectCollection<Employee> EmployeeCollection
			{
			get{ return m_EmployeeCollection;}
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
            
                public DateTime FromDate
                {
                set
                {
                if( m_fromDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFromDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fromDate= value;
                OnPropertyChanged(PropertyNameFromDate);
                }
                }
                get
                {
                return m_fromDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime ToDate
                {
                set
                {
                if( m_toDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameToDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_toDate= value;
                OnPropertyChanged(PropertyNameToDate);
                }
                }
                get
                {
                return m_toDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Period
                {
                set
                {
                if( String.Equals( m_period, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePeriod, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_period= value;
                OnPropertyChanged(PropertyNamePeriod);
                }
                }
                get
                {
                return m_period;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("EmployeeHistory",  Namespace=Strings.DefaultNamespace)]
          public  partial class EmployeeHistory
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_effectiveDate;
                public const string PropertyNameEffectiveDate = "EffectiveDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_value;
                public const string PropertyNameValue = "Value";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public DateTime EffectiveDate
                {
                set
                {
                if( m_effectiveDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEffectiveDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_effectiveDate= value;
                OnPropertyChanged(PropertyNameEffectiveDate);
                }
                }
                get
                {
                return m_effectiveDate;}
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
            
                public string Value
                {
                set
                {
                if( String.Equals( m_value, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_value= value;
                OnPropertyChanged(PropertyNameValue);
                }
                }
                get
                {
                return m_value;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Leave",  Namespace=Strings.DefaultNamespace)]
          public  partial class Leave
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_staffName;
                public const string PropertyNameStaffName = "StaffName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_type;
                public const string PropertyNameType = "Type";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_reason;
                public const string PropertyNameReason = "Reason";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_fromDate;
                public const string PropertyNameFromDate = "FromDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_toDate;
                public const string PropertyNameToDate = "ToDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_alternateContact;
                public const string PropertyNameAlternateContact = "AlternateContact";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_employeeId;
                public const string PropertyNameEmployeeId = "EmployeeId";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_duration;
                public const string PropertyNameDuration = "Duration";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string StaffName
                {
                set
                {
                if( String.Equals( m_staffName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStaffName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_staffName= value;
                OnPropertyChanged(PropertyNameStaffName);
                }
                }
                get
                {
                return m_staffName;}
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
            
            [DebuggerHidden]
            
                public string Reason
                {
                set
                {
                if( String.Equals( m_reason, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReason, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_reason= value;
                OnPropertyChanged(PropertyNameReason);
                }
                }
                get
                {
                return m_reason;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime FromDate
                {
                set
                {
                if( m_fromDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFromDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_fromDate= value;
                OnPropertyChanged(PropertyNameFromDate);
                }
                }
                get
                {
                return m_fromDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime ToDate
                {
                set
                {
                if( m_toDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameToDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_toDate= value;
                OnPropertyChanged(PropertyNameToDate);
                }
                }
                get
                {
                return m_toDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string AlternateContact
                {
                set
                {
                if( String.Equals( m_alternateContact, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAlternateContact, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_alternateContact= value;
                OnPropertyChanged(PropertyNameAlternateContact);
                }
                }
                get
                {
                return m_alternateContact;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int EmployeeId
                {
                set
                {
                if( m_employeeId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEmployeeId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_employeeId= value;
                OnPropertyChanged(PropertyNameEmployeeId);
                }
                }
                get
                {
                return m_employeeId;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int Duration
                {
                set
                {
                if( m_duration == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDuration, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_duration= value;
                OnPropertyChanged(PropertyNameDuration);
                }
                }
                get
                {
                return m_duration;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("PumpTest",  Namespace=Strings.DefaultNamespace)]
          public  partial class PumpTest
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_pumpNo;
                public const string PropertyNamePumpNo = "PumpNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_shift;
                public const string PropertyNameShift = "Shift";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTaken;
                public const string PropertyNameDateTaken = "DateTaken";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_volume;
                public const string PropertyNameVolume = "Volume";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_volumeRecorded;
                public const string PropertyNameVolumeRecorded = "VolumeRecorded";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tester;
                public const string PropertyNameTester = "Tester";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_calibrationTool;
                public const string PropertyNameCalibrationTool = "CalibrationTool";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_note;
                public const string PropertyNameNote = "Note";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string PumpNo
                {
                set
                {
                if( String.Equals( m_pumpNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePumpNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pumpNo= value;
                OnPropertyChanged(PropertyNamePumpNo);
                }
                }
                get
                {
                return m_pumpNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Shift
                {
                set
                {
                if( String.Equals( m_shift, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShift, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shift= value;
                OnPropertyChanged(PropertyNameShift);
                }
                }
                get
                {
                return m_shift;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime DateTaken
                {
                set
                {
                if( m_dateTaken == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateTaken, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dateTaken= value;
                OnPropertyChanged(PropertyNameDateTaken);
                }
                }
                get
                {
                return m_dateTaken;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double Volume
                {
                set
                {
                if( m_volume == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolume, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volume= value;
                OnPropertyChanged(PropertyNameVolume);
                }
                }
                get
                {
                return m_volume;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public double VolumeRecorded
                {
                set
                {
                if( m_volumeRecorded == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVolumeRecorded, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_volumeRecorded= value;
                OnPropertyChanged(PropertyNameVolumeRecorded);
                }
                }
                get
                {
                return m_volumeRecorded;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Tester
                {
                set
                {
                if( String.Equals( m_tester, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTester, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tester= value;
                OnPropertyChanged(PropertyNameTester);
                }
                }
                get
                {
                return m_tester;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string CalibrationTool
                {
                set
                {
                if( String.Equals( m_calibrationTool, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCalibrationTool, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_calibrationTool= value;
                OnPropertyChanged(PropertyNameCalibrationTool);
                }
                }
                get
                {
                return m_calibrationTool;}
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
          [XmlType("Alert",  Namespace=Strings.DefaultNamespace)]
          public  partial class Alert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_description;
                public const string PropertyNameDescription = "Description";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  AlertType  m_category;
                public const string PropertyNameCategory = "Category";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_startDate;
                public const string PropertyNameStartDate = "StartDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_endDate;
                public const string PropertyNameEndDate = "EndDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  bool  m_isCompleted;
                public const string PropertyNameIsCompleted = "IsCompleted";

              
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
                if( String.Equals( m_description, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDescription, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_description= value;
                OnPropertyChanged(PropertyNameDescription);
                }
                }
                get
                {
                return m_description;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public AlertType Category
                {
                set
                {
                if( m_category == value) return;
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
            
                public DateTime StartDate
                {
                set
                {
                if( m_startDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStartDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_startDate= value;
                OnPropertyChanged(PropertyNameStartDate);
                }
                }
                get
                {
                return m_startDate;}
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
                if( m_endDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_endDate= value;
                OnPropertyChanged(PropertyNameEndDate);
                }
                }
                get
                {
                return m_endDate;}
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
                if( m_isCompleted == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCompleted, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_isCompleted= value;
                OnPropertyChanged(PropertyNameIsCompleted);
                }
                }
                get
                {
                return m_isCompleted;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("PumpTestAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class PumpTestAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_pump;
                public const string PropertyNamePump = "Pump";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_shift;
                public const string PropertyNameShift = "Shift";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Pump
                {
                set
                {
                if( String.Equals( m_pump, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePump, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_pump= value;
                OnPropertyChanged(PropertyNamePump);
                }
                }
                get
                {
                return m_pump;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
            [DebuggerHidden]
            
                public string Shift
                {
                set
                {
                if( String.Equals( m_shift, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShift, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shift= value;
                OnPropertyChanged(PropertyNameShift);
                }
                }
                get
                {
                return m_shift;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("UnclearCheckAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class UnclearCheckAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_bank;
                public const string PropertyNameBank = "Bank";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_contactName;
                public const string PropertyNameContactName = "ContactName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_contactNo;
                public const string PropertyNameContactNo = "ContactNo";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Bank
                {
                set
                {
                if( String.Equals( m_bank, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBank, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_bank= value;
                OnPropertyChanged(PropertyNameBank);
                }
                }
                get
                {
                return m_bank;}
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
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string ContactName
                {
                set
                {
                if( String.Equals( m_contactName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContactName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_contactName= value;
                OnPropertyChanged(PropertyNameContactName);
                }
                }
                get
                {
                return m_contactName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
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

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("BillingPaymentAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class BillingPaymentAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dueDate;
                public const string PropertyNameDueDate = "DueDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_accountName;
                public const string PropertyNameAccountName = "AccountName";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime DueDate
                {
                set
                {
                if( m_dueDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDueDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_dueDate= value;
                OnPropertyChanged(PropertyNameDueDate);
                }
                }
                get
                {
                return m_dueDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Amount
                {
                set
                {
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string AccountName
                {
                set
                {
                if( String.Equals( m_accountName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_accountName= value;
                OnPropertyChanged(PropertyNameAccountName);
                }
                }
                get
                {
                return m_accountName;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Reconciliation",  Namespace=Strings.DefaultNamespace)]
          public  partial class Reconciliation
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_date;
                public const string PropertyNameDate = "Date";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_accountName;
                public const string PropertyNameAccountName = "AccountName";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_statementDate;
                public const string PropertyNameStatementDate = "StatementDate";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  decimal  m_statementBalance;
                public const string PropertyNameStatementBalance = "StatementBalance";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  int  m_accountId;
                public const string PropertyNameAccountId = "AccountId";

              
			private readonly ObjectCollection<Transaction>  m_TransactionCollection = new ObjectCollection<Transaction> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Transaction", IsNullable = false)]
			public ObjectCollection<Transaction> TransactionCollection
			{
			get{ return m_TransactionCollection;}
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
            
                public string AccountName
                {
                set
                {
                if( String.Equals( m_accountName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountName, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_accountName= value;
                OnPropertyChanged(PropertyNameAccountName);
                }
                }
                get
                {
                return m_accountName;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime StatementDate
                {
                set
                {
                if( m_statementDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatementDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_statementDate= value;
                OnPropertyChanged(PropertyNameStatementDate);
                }
                }
                get
                {
                return m_statementDate;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public decimal StatementBalance
                {
                set
                {
                if( m_statementBalance == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStatementBalance, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_statementBalance= value;
                OnPropertyChanged(PropertyNameStatementBalance);
                }
                }
                get
                {
                return m_statementBalance;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public int AccountId
                {
                set
                {
                if( m_accountId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAccountId, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_accountId= value;
                OnPropertyChanged(PropertyNameAccountId);
                }
                }
                get
                {
                return m_accountId;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("CreditCardReconciliation",  Namespace=Strings.DefaultNamespace)]
          public  partial class CreditCardReconciliation
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_creditCardNo;
                public const string PropertyNameCreditCardNo = "CreditCardNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  double  m_amount;
                public const string PropertyNameAmount = "Amount";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_status;
                public const string PropertyNameStatus = "Status";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string CreditCardNo
                {
                set
                {
                if( String.Equals( m_creditCardNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCreditCardNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_creditCardNo= value;
                OnPropertyChanged(PropertyNameCreditCardNo);
                }
                }
                get
                {
                return m_creditCardNo;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public double Amount
                {
                set
                {
                if( m_amount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAmount, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_amount= value;
                OnPropertyChanged(PropertyNameAmount);
                }
                }
                get
                {
                return m_amount;}
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
          [XmlType("WaterTestAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class WaterTestAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_tankNo;
                public const string PropertyNameTankNo = "TankNo";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_dateTime;
                public const string PropertyNameDateTime = "DateTime";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string TankNo
                {
                set
                {
                if( String.Equals( m_tankNo, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTankNo, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_tankNo= value;
                OnPropertyChanged(PropertyNameTankNo);
                }
                }
                get
                {
                return m_tankNo;}
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

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ReceivedBillAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class ReceivedBillAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_purposed;
                public const string PropertyNamePurposed = "Purposed";

              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  DateTime  m_expectedDate;
                public const string PropertyNameExpectedDate = "ExpectedDate";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Purposed
                {
                set
                {
                if( String.Equals( m_purposed, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePurposed, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_purposed= value;
                OnPropertyChanged(PropertyNamePurposed);
                }
                }
                get
                {
                return m_purposed;}
                }

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public DateTime ExpectedDate
                {
                set
                {
                if( m_expectedDate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpectedDate, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_expectedDate= value;
                OnPropertyChanged(PropertyNameExpectedDate);
                }
                }
                get
                {
                return m_expectedDate;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DailyCashAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class DailyCashAlert
          {

          


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Ss1aAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class Ss1aAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_shift;
                public const string PropertyNameShift = "Shift";

              
            ///<summary>
            /// 
            ///</summary>
            [XmlAttribute]
            
              [Required]
            
            [DebuggerHidden]
            
                public string Shift
                {
                set
                {
                if( String.Equals( m_shift, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShift, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_shift= value;
                OnPropertyChanged(PropertyNameShift);
                }
                }
                get
                {
                return m_shift;}
                }

              


          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DailyJournalAlert",  Namespace=Strings.DefaultNamespace)]
          public  partial class DailyJournalAlert
          {

          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  string  m_subCategory;
                public const string PropertyNameSubCategory = "SubCategory";

              
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
                if( String.Equals( m_subCategory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubCategory, value);
                OnPropertyChanging(arg);
                if( !arg.Cancel)
                {
                m_subCategory= value;
                OnPropertyChanged(PropertyNameSubCategory);
                }
                }
                get
                {
                return m_subCategory;}
                }

              


          }
        
      public enum ShipmentStatus
      {
      SalesOrderCreated,
      DeliveryNotesCreated,
      Cancelled,
      ShipmentDelivered,
      }
    
      public enum OrderStatus
      {
      SalesOrderCreated,
      DeliveryNotesCreated,
      Cancelled,
      ShipmentDelivered,
      ShipmentLoaded,
      ShipmentScheduled ,
      DeliveryBlocked,
      }
    
      public enum AccountType
      {
      Banking,
      Equity,
      Payable,
      Receiveable,
      FixAsset,
      Loan,
      CreditCard,
      }
    
      public enum LogLevel
      {
      Information,
      Critical,
      Warning,
      Verbose,
      Error,
      }
    
      public enum AlertType
      {
      PumpTest,
      UnclearCheck,
      BillReceiving,
      BillPayment,
      WaterTest,
      DailyCash,
      DailyJournal,
      SS1A,
      }
    
    }

  