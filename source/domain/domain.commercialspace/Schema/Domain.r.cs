
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;


    // ReSharper disable InconsistentNaming
    namespace Bespoke.SphCommercialSpaces.Domain
    {
    
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ReportLayout",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ReportLayout
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_type;
                    public const string PropertyNameType = "Type";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
          [XmlType("Report",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class Report
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_title;
                    public const string PropertyNameTitle = "Title";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isPrivate;
                    public const string PropertyNameIsPrivate = "IsPrivate";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isExportAllowed;
                    public const string PropertyNameIsExportAllowed = "IsExportAllowed";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  DateTime  m_createdDate;
                    public const string PropertyNameCreatedDate = "CreatedDate";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_createdBy;
                    public const string PropertyNameCreatedBy = "CreatedBy";

                  
			private readonly ObjectCollection<ReportItem>  m_ReportItemCollection = new ObjectCollection<ReportItem> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public ObjectCollection<ReportItem> ReportItemCollection
			{
			get{ return m_ReportItemCollection;}
			}
		
			private readonly ObjectCollection<ReportLayout>  m_ReportLayoutCollection = new ObjectCollection<ReportLayout> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("ReportLayout", IsNullable = false)]
			public ObjectCollection<ReportLayout> ReportLayoutCollection
			{
			get{ return m_ReportLayoutCollection;}
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                
                    public bool IsPrivate
                    {
                    set
                    {
                    if( m_isPrivate == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameIsPrivate, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_isPrivate= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_isPrivate;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public bool IsExportAllowed
                    {
                    set
                    {
                    if( m_isExportAllowed == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameIsExportAllowed, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_isExportAllowed= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_isExportAllowed;}
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
                    OnPropertyChanged();
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
                
                    public DateTime CreatedDate
                    {
                    set
                    {
                    if( m_createdDate == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameCreatedDate, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_createdDate= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_createdDate;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public string CreatedBy
                    {
                    set
                    {
                    if( String.Equals( m_createdBy, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameCreatedBy, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_createdBy= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_createdBy;}
                    }

                  

          }
        
      [XmlType("ReportItem",  Namespace=Strings.DEFAULT_NAMESPACE)]
      public partial class ReportItem
      {

      

      // public properties members
      


      }

    
    }
// ReSharper restore InconsistentNaming

  