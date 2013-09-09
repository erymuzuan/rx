
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
          [XmlType("CustomField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class CustomField
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_order;
                    public const string PropertyNameOrder = "Order";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isRequired;
                    public const string PropertyNameIsRequired = "IsRequired";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_type;
                    public const string PropertyNameType = "Type";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_size;
                    public const string PropertyNameSize = "Size";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_listing;
                    public const string PropertyNameListing = "Listing";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_group;
                    public const string PropertyNameGroup = "Group";

                  
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_maxLength;
                public const string PropertyNameMaxLength = "MaxLength";


              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int?  m_minLength;
                public const string PropertyNameMinLength = "MinLength";


              
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
                    if( m_order == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameOrder, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_order= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_order;}
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                
                    public string Listing
                    {
                    set
                    {
                    if( String.Equals( m_listing, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameListing, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_listing= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_listing;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                [DebuggerHidden]
                
                    public string Group
                    {
                    set
                    {
                    if( String.Equals( m_group, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameGroup, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_group= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_group;}
                    }

                  

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				
                public int? MaxLength
                {
                set
                {
                if(m_maxLength == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaxLength, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_maxLength= value;
                OnPropertyChanged();
                }
                }
                get { return m_maxLength;}
                }
              

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				
                public int? MinLength
                {
                set
                {
                if(m_minLength == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMinLength, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_minLength= value;
                OnPropertyChanged();
                }
                }
                get { return m_minLength;}
                }
              

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ComplaintTemplate",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ComplaintTemplate
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_complaintTemplateId;
                    public const string PropertyNameComplaintTemplateId = "ComplaintTemplateId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_category;
                    public const string PropertyNameCategory = "Category";

                  
			private readonly ObjectCollection<ComplaintCategory>  m_ComplaintCategoryCollection = new ObjectCollection<ComplaintCategory> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("ComplaintCategory", IsNullable = false)]
			public ObjectCollection<ComplaintCategory> ComplaintCategoryCollection
			{
			get{ return m_ComplaintCategoryCollection;}
			}
		
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FormDesign m_formDesign
					=  new FormDesign();
				
			public const string PropertyNameFormDesign = "FormDesign";
			[DebuggerHidden]

			public FormDesign FormDesign
			{
			get{ return m_formDesign;}
			set
			{
			m_formDesign = value;
			OnPropertyChanged();
			}
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
                    if( m_complaintTemplateId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameComplaintTemplateId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_complaintTemplateId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_complaintTemplateId;}
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
          [XmlType("BuildingTemplate",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class BuildingTemplate
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_buildingTemplateId;
                    public const string PropertyNameBuildingTemplateId = "BuildingTemplateId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_icon;
                    public const string PropertyNameIcon = "Icon";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_category;
                    public const string PropertyNameCategory = "Category";

                  
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FormDesign m_formDesign
					=  new FormDesign();
				
			public const string PropertyNameFormDesign = "FormDesign";
			[DebuggerHidden]

			public FormDesign FormDesign
			{
			get{ return m_formDesign;}
			set
			{
			m_formDesign = value;
			OnPropertyChanged();
			}
			}
		
			private readonly ObjectCollection<CustomListDefinition>  m_CustomListDefinitionCollection = new ObjectCollection<CustomListDefinition> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomListDefinition", IsNullable = false)]
			public ObjectCollection<CustomListDefinition> CustomListDefinitionCollection
			{
			get{ return m_CustomListDefinitionCollection;}
			}
		
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int BuildingTemplateId
                    {
                    set
                    {
                    if( m_buildingTemplateId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameBuildingTemplateId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_buildingTemplateId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_buildingTemplateId;}
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
                    OnPropertyChanged();
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
                
                    public string Icon
                    {
                    set
                    {
                    if( String.Equals( m_icon, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameIcon, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_icon= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_icon;}
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
                    OnPropertyChanged();
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
          [XmlType("ApplicationTemplate",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ApplicationTemplate
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_applicationTemplateId;
                    public const string PropertyNameApplicationTemplateId = "ApplicationTemplateId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_category;
                    public const string PropertyNameCategory = "Category";

                  
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FormDesign m_formDesign
					=  new FormDesign();
				
			public const string PropertyNameFormDesign = "FormDesign";
			[DebuggerHidden]

			public FormDesign FormDesign
			{
			get{ return m_formDesign;}
			set
			{
			m_formDesign = value;
			OnPropertyChanged();
			}
			}
		
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int ApplicationTemplateId
                    {
                    set
                    {
                    if( m_applicationTemplateId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameApplicationTemplateId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_applicationTemplateId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_applicationTemplateId;}
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
          [XmlType("MaintenanceTemplate",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class MaintenanceTemplate
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_maintenanceTemplateId;
                    public const string PropertyNameMaintenanceTemplateId = "MaintenanceTemplateId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_category;
                    public const string PropertyNameCategory = "Category";

                  
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FormDesign m_formDesign
					=  new FormDesign();
				
			public const string PropertyNameFormDesign = "FormDesign";
			[DebuggerHidden]

			public FormDesign FormDesign
			{
			get{ return m_formDesign;}
			set
			{
			m_formDesign = value;
			OnPropertyChanged();
			}
			}
		
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int MaintenanceTemplateId
                    {
                    set
                    {
                    if( m_maintenanceTemplateId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameMaintenanceTemplateId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_maintenanceTemplateId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_maintenanceTemplateId;}
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
          [XmlType("CommercialSpaceTemplate",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class CommercialSpaceTemplate
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_commercialSpaceTemplateId;
                    public const string PropertyNameCommercialSpaceTemplateId = "CommercialSpaceTemplateId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_category;
                    public const string PropertyNameCategory = "Category";

                  
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FormDesign m_formDesign
					=  new FormDesign();
				
			public const string PropertyNameFormDesign = "FormDesign";
			[DebuggerHidden]

			public FormDesign FormDesign
			{
			get{ return m_formDesign;}
			set
			{
			m_formDesign = value;
			OnPropertyChanged();
			}
			}
		
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int CommercialSpaceTemplateId
                    {
                    set
                    {
                    if( m_commercialSpaceTemplateId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameCommercialSpaceTemplateId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_commercialSpaceTemplateId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_commercialSpaceTemplateId;}
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
          [XmlType("FormDesign",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class FormDesign
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_confirmationText;
                    public const string PropertyNameConfirmationText = "ConfirmationText";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_imageStoreId;
                    public const string PropertyNameImageStoreId = "ImageStoreId";

                  
			private readonly ObjectCollection<FormElement>  m_FormElementCollection = new ObjectCollection<FormElement> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public ObjectCollection<FormElement> FormElementCollection
			{
			get{ return m_FormElementCollection;}
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
                    OnPropertyChanged();
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
                
                    public string ConfirmationText
                    {
                    set
                    {
                    if( String.Equals( m_confirmationText, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameConfirmationText, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_confirmationText= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_confirmationText;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public string ImageStoreId
                    {
                    set
                    {
                    if( String.Equals( m_imageStoreId, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameImageStoreId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_imageStoreId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_imageStoreId;}
                    }

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("TextBox",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class TextBox
          {
          
                    private string  m_DefaultValue;
                    [XmlAttribute]
                    public  string DefaultValue {get{
                    return m_DefaultValue;}
                    set{
                    m_DefaultValue = value;
                      RaisePropertyChanged();
                    }}

                  
                    public int? MinLength {get;set;}
                  
                    public int? MaxLength {get;set;}
                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("CheckBox",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class CheckBox
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DatePicker",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class DatePicker
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ComboBox",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ComboBox
          {
          
			private readonly ObjectCollection<ComboBoxItem>  m_ComboBoxItemCollection = new ObjectCollection<ComboBoxItem> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("ComboBoxItem", IsNullable = false)]
			public ObjectCollection<ComboBoxItem> ComboBoxItemCollection
			{
			get{ return m_ComboBoxItemCollection;}
			}
		

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("TextAreaElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class TextAreaElement
          {
          
                    private string  m_Rows;
                    [XmlAttribute]
                    public  string Rows {get{
                    return m_Rows;}
                    set{
                    m_Rows = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("WebsiteFormElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class WebsiteFormElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("EmailFormElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class EmailFormElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("NumberTextBox",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class NumberTextBox
          {
          
                    private int  m_Step;
                    [XmlAttribute]
                    public  int Step {get{
                    return m_Step;}
                    set{
                    m_Step = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("BuildingMapElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class BuildingMapElement
          {
          
                    private string  m_Icon;
                    [XmlAttribute]
                    public  string Icon {get{
                    return m_Icon;}
                    set{
                    m_Icon = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("BuildingFloorsElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class BuildingFloorsElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("SectionFormElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class SectionFormElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ComboBoxItem",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ComboBoxItem
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_caption;
                    public const string PropertyNameCaption = "Caption";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_value;
                    public const string PropertyNameValue = "Value";

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public string Caption
                    {
                    set
                    {
                    if( String.Equals( m_caption, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameCaption, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_caption= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_caption;}
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
                    OnPropertyChanged();
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
          [XmlType("AddressElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class AddressElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ComplaintCategoryElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ComplaintCategoryElement
          {
          
                    private string  m_SubCategoryLabel;
                    [XmlAttribute]
                    public  string SubCategoryLabel {get{
                    return m_SubCategoryLabel;}
                    set{
                    m_SubCategoryLabel = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("RentalApplicationBanksElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class RentalApplicationBanksElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("RentalApplicationAttachmentsElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class RentalApplicationAttachmentsElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("RentalApplicationContactElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class RentalApplicationContactElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("CommercialSpaceLotsElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class CommercialSpaceLotsElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("HtmlElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class HtmlElement
          {
          
                    public string Text {get;set;}
                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("BuildingElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class BuildingElement
          {
          

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ComplaintCategory",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ComplaintCategory
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_description;
                    public const string PropertyNameDescription = "Description";

                  
			private readonly  ObjectCollection<string>  m_SubCategoryCollection = new  ObjectCollection<string> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public  ObjectCollection<string> SubCategoryCollection
			{
			get{ return m_SubCategoryCollection;}
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
                    OnPropertyChanged();
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
          [XmlType("CustomListDefinition",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class CustomListDefinition
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_label;
                    public const string PropertyNameLabel = "Label";

                  
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
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
                    if( String.Equals( m_name, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameName, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_name= value;
                    OnPropertyChanged();
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
                
                [DebuggerHidden]
                
                    public string Label
                    {
                    set
                    {
                    if( String.Equals( m_label, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameLabel, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_label= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_label;}
                    }

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("CustomListDefinitionElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class CustomListDefinitionElement
          {
          
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("CustomField", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("MaintenanceOfficerElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class MaintenanceOfficerElement
          {
          

          }
        
      [XmlType("FormElement",  Namespace=Strings.DEFAULT_NAMESPACE)]
      public partial class FormElement
      {

      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_name;
        public const string PropertyNameName = "Name";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_label;
        public const string PropertyNameLabel = "Label";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_tooltip;
        public const string PropertyNameTooltip = "Tooltip";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_path;
        public const string PropertyNamePath = "Path";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  bool  m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_size;
        public const string PropertyNameSize = "Size";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_cssClass;
        public const string PropertyNameCssClass = "CssClass";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_visible;
        public const string PropertyNameVisible = "Visible";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_enable;
        public const string PropertyNameEnable = "Enable";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_elementId;
        public const string PropertyNameElementId = "ElementId";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_helpText;
        public const string PropertyNameHelpText = "HelpText";
      

      // public properties members
      


        [XmlAttribute]
        public string Name
        {
        set
        {
        if(m_name== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameName, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_name= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_name;}
        }
      


        [XmlAttribute]
        public string Label
        {
        set
        {
        if(m_label== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameLabel, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_label= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_label;}
        }
      


        [XmlAttribute]
        public string Tooltip
        {
        set
        {
        if(m_tooltip== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameTooltip, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_tooltip= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_tooltip;}
        }
      


        [XmlAttribute]
        public string Path
        {
        set
        {
        if(m_path== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNamePath, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_path= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_path;}
        }
      


        [XmlAttribute]
        public bool IsRequired
        {
        set
        {
        if(m_isRequired== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_isRequired= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_isRequired;}
        }
      


        [XmlAttribute]
        public string Size
        {
        set
        {
        if(m_size== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_size= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_size;}
        }
      


        [XmlAttribute]
        public string CssClass
        {
        set
        {
        if(m_cssClass== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameCssClass, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_cssClass= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_cssClass;}
        }
      


        [XmlAttribute]
        public string Visible
        {
        set
        {
        if(m_visible== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameVisible, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_visible= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_visible;}
        }
      


        [XmlAttribute]
        public string Enable
        {
        set
        {
        if(m_enable== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameEnable, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_enable= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_enable;}
        }
      


        [XmlAttribute]
        public string ElementId
        {
        set
        {
        if(m_elementId== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameElementId, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_elementId= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_elementId;}
        }
      


        [XmlAttribute]
        public string HelpText
        {
        set
        {
        if(m_helpText== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameHelpText, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_helpText= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_helpText;}
        }
      


      }

    
    }
// ReSharper restore InconsistentNaming

  