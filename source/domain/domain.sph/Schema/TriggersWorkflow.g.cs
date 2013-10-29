
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
          [XmlType("Trigger",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class Trigger
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_entity;
                    public const string PropertyNameEntity = "Entity";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_typeOf;
                    public const string PropertyNameTypeOf = "TypeOf";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_triggerId;
                    public const string PropertyNameTriggerId = "TriggerId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_note;
                    public const string PropertyNameNote = "Note";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isFiredOnAdded;
                    public const string PropertyNameIsFiredOnAdded = "IsFiredOnAdded";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isFiredOnDeleted;
                    public const string PropertyNameIsFiredOnDeleted = "IsFiredOnDeleted";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isFiredOnChanged;
                    public const string PropertyNameIsFiredOnChanged = "IsFiredOnChanged";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_firedOnOperations;
                    public const string PropertyNameFiredOnOperations = "FiredOnOperations";

                  
			private readonly ObjectCollection<Rule>  m_RuleCollection = new ObjectCollection<Rule> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("Rule", IsNullable = false)]
			public ObjectCollection<Rule> RuleCollection
			{
			get{ return m_RuleCollection;}
			}
		
			private readonly ObjectCollection<CustomAction>  m_ActionCollection = new ObjectCollection<CustomAction> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public ObjectCollection<CustomAction> ActionCollection
			{
			get{ return m_ActionCollection;}
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
                
                    public string Entity
                    {
                    set
                    {
                    if( String.Equals( m_entity, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameEntity, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_entity= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_entity;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public string TypeOf
                    {
                    set
                    {
                    if( String.Equals( m_typeOf, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameTypeOf, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_typeOf= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_typeOf;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int TriggerId
                    {
                    set
                    {
                    if( m_triggerId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameTriggerId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_triggerId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_triggerId;}
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
                    OnPropertyChanged();
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
                
                    public bool IsFiredOnAdded
                    {
                    set
                    {
                    if( m_isFiredOnAdded == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameIsFiredOnAdded, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_isFiredOnAdded= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_isFiredOnAdded;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public bool IsFiredOnDeleted
                    {
                    set
                    {
                    if( m_isFiredOnDeleted == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameIsFiredOnDeleted, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_isFiredOnDeleted= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_isFiredOnDeleted;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public bool IsFiredOnChanged
                    {
                    set
                    {
                    if( m_isFiredOnChanged == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameIsFiredOnChanged, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_isFiredOnChanged= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_isFiredOnChanged;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public string FiredOnOperations
                    {
                    set
                    {
                    if( String.Equals( m_firedOnOperations, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameFiredOnOperations, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_firedOnOperations= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_firedOnOperations;}
                    }

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("AssemblyField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class AssemblyField
          {
          
                    private string  m_Location;
                    [XmlAttribute]
                    public  string Location {get{
                    return m_Location;}
                    set{
                    m_Location = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_TypeName;
                    [XmlAttribute]
                    public  string TypeName {get{
                    return m_TypeName;}
                    set{
                    m_TypeName = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_Method;
                    [XmlAttribute]
                    public  string Method {get{
                    return m_Method;}
                    set{
                    m_Method = value;
                      RaisePropertyChanged();
                    }}

                  
                    private bool  m_IsAsync;
                    [XmlAttribute]
                    public  bool IsAsync {get{
                    return m_IsAsync;}
                    set{
                    m_IsAsync = value;
                      RaisePropertyChanged();
                    }}

                  
                    private int  m_AsyncTimeout;
                    [XmlAttribute]
                    public  int AsyncTimeout {get{
                    return m_AsyncTimeout;}
                    set{
                    m_AsyncTimeout = value;
                      RaisePropertyChanged();
                    }}

                  
			private readonly ObjectCollection<MethodArg>  m_MethodArgCollection = new ObjectCollection<MethodArg> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("MethodArg", IsNullable = false)]
			public ObjectCollection<MethodArg> MethodArgCollection
			{
			get{ return m_MethodArgCollection;}
			}
		

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("FunctionField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class FunctionField
          {
          
                    private string  m_Script;
                    [XmlAttribute]
                    public  string Script {get{
                    return m_Script;}
                    set{
                    m_Script = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("ConstantField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ConstantField
          {
          
                    private string  m_TypeName;
                    [XmlAttribute]
                    public  string TypeName {get{
                    return m_TypeName;}
                    set{
                    m_TypeName = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("DocumentField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class DocumentField
          {
          
                    private string  m_XPath;
                    [XmlAttribute]
                    public  string XPath {get{
                    return m_XPath;}
                    set{
                    m_XPath = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_NamespacePrefix;
                    [XmlAttribute]
                    public  string NamespacePrefix {get{
                    return m_NamespacePrefix;}
                    set{
                    m_NamespacePrefix = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_TypeName;
                    [XmlAttribute]
                    public  string TypeName {get{
                    return m_TypeName;}
                    set{
                    m_TypeName = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_Path;
                    [XmlAttribute]
                    public  string Path {get{
                    return m_Path;}
                    set{
                    m_Path = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("PropertyChangedField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class PropertyChangedField
          {
          
                    private string  m_Path;
                    [XmlAttribute]
                    public  string Path {get{
                    return m_Path;}
                    set{
                    m_Path = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_TypeName;
                    [XmlAttribute]
                    public  string TypeName {get{
                    return m_TypeName;}
                    set{
                    m_TypeName = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_OldValue;
                    [XmlAttribute]
                    public  string OldValue {get{
                    return m_OldValue;}
                    set{
                    m_OldValue = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_NewValue;
                    [XmlAttribute]
                    public  string NewValue {get{
                    return m_NewValue;}
                    set{
                    m_NewValue = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Rule",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class Rule
          {
          
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Field  m_left;
                public const string PropertyNameLeft = "Left";


              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Field  m_right;
                public const string PropertyNameRight = "Right";


              
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Operator  m_operator;
                public const string PropertyNameOperator = "Operator";


              

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				 [JsonProperty(TypeNameHandling = TypeNameHandling.All)] 
                public Field Left
                {
                set
                {
                if(m_left == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLeft, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_left= value;
                OnPropertyChanged();
                }
                }
                get { return m_left;}
                }
              

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				[JsonProperty(TypeNameHandling = TypeNameHandling.All)] 
                public Field Right
                {
                set
                {
                if(m_right == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRight, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_right= value;
                OnPropertyChanged();
                }
                }
                get { return m_right;}
                }
              

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				[JsonConverter(typeof(StringEnumConverter))]
                public Operator Operator
                {
                set
                {
                if(m_operator == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperator, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_operator= value;
                OnPropertyChanged();
                }
                }
                get { return m_operator;}
                }
              

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("EmailAction",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class EmailAction
          {
          
                    private string  m_From;
                    [XmlAttribute]
                    public  string From {get{
                    return m_From;}
                    set{
                    m_From = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_To;
                    [XmlAttribute]
                    public  string To {get{
                    return m_To;}
                    set{
                    m_To = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_SubjectTemplate;
                    [XmlAttribute]
                    public  string SubjectTemplate {get{
                    return m_SubjectTemplate;}
                    set{
                    m_SubjectTemplate = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_BodyTemplate;
                    [XmlAttribute]
                    public  string BodyTemplate {get{
                    return m_BodyTemplate;}
                    set{
                    m_BodyTemplate = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_Bcc;
                    [XmlAttribute]
                    public  string Bcc {get{
                    return m_Bcc;}
                    set{
                    m_Bcc = value;
                      RaisePropertyChanged();
                    }}

                  
                    private string  m_Cc;
                    [XmlAttribute]
                    public  string Cc {get{
                    return m_Cc;}
                    set{
                    m_Cc = value;
                      RaisePropertyChanged();
                    }}

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("SetterAction",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class SetterAction
          {
          
			private readonly ObjectCollection<SetterActionChild>  m_SetterActionChildCollection = new ObjectCollection<SetterActionChild> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("SetterActionChild", IsNullable = false)]
			public ObjectCollection<SetterActionChild> SetterActionChildCollection
			{
			get{ return m_SetterActionChildCollection;}
			}
		

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("SetterActionChild",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class SetterActionChild
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_path;
                    public const string PropertyNamePath = "Path";

                  
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Field  m_field;
                public const string PropertyNameField = "Field";


              
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public string Path
                    {
                    set
                    {
                    if( String.Equals( m_path, value, StringComparison.Ordinal)) return;
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

                  

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				 [JsonProperty(TypeNameHandling = TypeNameHandling.All)] 
                public Field Field
                {
                set
                {
                if(m_field == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameField, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_field= value;
                OnPropertyChanged();
                }
                }
                get { return m_field;}
                }
              

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("MethodArg",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class MethodArg
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_typeName;
                    public const string PropertyNameTypeName = "TypeName";

                  
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Field  m_valueProvider;
                public const string PropertyNameValueProvider = "ValueProvider";


              
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
                
                    public string TypeName
                    {
                    set
                    {
                    if( String.Equals( m_typeName, value, StringComparison.Ordinal)) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameTypeName, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_typeName= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_typeName;}
                    }

                  

                ///<summary>
                /// 
                ///</summary>
                [DebuggerHidden]
				
                public Field ValueProvider
                {
                set
                {
                if(m_valueProvider == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValueProvider, value);
                OnPropertyChanging(arg);
                if(! arg.Cancel)
                {
                m_valueProvider= value;
                OnPropertyChanged();
                }
                }
                get { return m_valueProvider;}
                }
              

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("WorkflowDefinition",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class WorkflowDefinition
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_workflowDefinitionId;
                    public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_note;
                    public const string PropertyNameNote = "Note";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  bool  m_isActive;
                    public const string PropertyNameIsActive = "IsActive";

                  
			private readonly ObjectCollection<Activity>  m_ActivityCollection = new ObjectCollection<Activity> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public ObjectCollection<Activity> ActivityCollection
			{
			get{ return m_ActivityCollection;}
			}
		
			private readonly ObjectCollection<CustomField>  m_CustomFieldCollection = new ObjectCollection<CustomField> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public ObjectCollection<CustomField> CustomFieldCollection
			{
			get{ return m_CustomFieldCollection;}
			}
		
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int WorkflowDefinitionId
                    {
                    set
                    {
                    if( m_workflowDefinitionId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameWorkflowDefinitionId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_workflowDefinitionId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_workflowDefinitionId;}
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
                    OnPropertyChanged();
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

                  

          }
        
          ///<summary>
          /// 
          ///</summary>
          [DataObject(true)]
          [Serializable]
          [XmlType("Workflow",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class Workflow
          {
          
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_workflowId;
                    public const string PropertyNameWorkflowId = "WorkflowId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  int  m_workflowDefinitionId;
                    public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_name;
                    public const string PropertyNameName = "Name";

                  
                    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private  string  m_status;
                    public const string PropertyNameStatus = "Status";

                  
			private readonly ObjectCollection<CustomFieldValue>  m_CustomFieldValueCollection = new ObjectCollection<CustomFieldValue> ();

			///<summary>
			/// 
			///</summary>
			[XmlArrayItem("", IsNullable = false)]
			public ObjectCollection<CustomFieldValue> CustomFieldValueCollection
			{
			get{ return m_CustomFieldValueCollection;}
			}
		
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int WorkflowId
                    {
                    set
                    {
                    if( m_workflowId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameWorkflowId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_workflowId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_workflowId;}
                    }

                  
                ///<summary>
                /// 
                ///</summary>
                [XmlAttribute]
                
                  [Required]
                
                [DebuggerHidden]
                
                    public int WorkflowDefinitionId
                    {
                    set
                    {
                    if( m_workflowDefinitionId == value) return;
                    var arg = new PropertyChangingEventArgs(PropertyNameWorkflowDefinitionId, value);
                    OnPropertyChanging(arg);
                    if( !arg.Cancel)
                    {
                    m_workflowDefinitionId= value;
                    OnPropertyChanged();
                    }
                    }
                    get
                    {
                    return m_workflowDefinitionId;}
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
                    OnPropertyChanged();
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
          [XmlType("ScreenActivity",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class ScreenActivity
          {
          
                    private string  m_Title;
                    [XmlAttribute]
                    public  string Title {get{
                    return m_Title;}
                    set{
                    m_Title = value;
                      RaisePropertyChanged();
                    }}

                  
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FormDesign m_FormDesign
					=  new FormDesign();
				
			public const string PropertyNameFormDesign = "FormDesign";
			[DebuggerHidden]

			public FormDesign FormDesign
			{
			get{ return m_FormDesign;}
			set
			{
			m_FormDesign = value;
			OnPropertyChanged();
			}
			}
		

          }
        
      [XmlType("Field",  Namespace=Strings.DEFAULT_NAMESPACE)]
      public partial class Field
      {

      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_name;
        public const string PropertyNameName = "Name";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_note;
        public const string PropertyNameNote = "Note";
      

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
        public string Note
        {
        set
        {
        if(m_note== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_note= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_note;}
        }
      


      }

    
      [XmlType("CustomAction",  Namespace=Strings.DEFAULT_NAMESPACE)]
      public partial class CustomAction
      {

      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_title;
        public const string PropertyNameTitle = "Title";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  bool  m_isActive;
        public const string PropertyNameIsActive = "IsActive";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  int  m_triggerId;
        public const string PropertyNameTriggerId = "TriggerId";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  string  m_note;
        public const string PropertyNameNote = "Note";
      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  int  m_customActionId;
        public const string PropertyNameCustomActionId = "CustomActionId";
      

      // public properties members
      


        [XmlAttribute]
        public string Title
        {
        set
        {
        if(m_title== value) return;
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
      


        [XmlAttribute]
        public bool IsActive
        {
        set
        {
        if(m_isActive== value) return;
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
      


        [XmlAttribute]
        public int TriggerId
        {
        set
        {
        if(m_triggerId== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameTriggerId, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_triggerId= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_triggerId;}
        }
      


        [XmlAttribute]
        public string Note
        {
        set
        {
        if(m_note== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameNote, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_note= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_note;}
        }
      


        [XmlAttribute]
        public int CustomActionId
        {
        set
        {
        if(m_customActionId== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameCustomActionId, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_customActionId= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_customActionId;}
        }
      


      }

    
      [XmlType("Activity",  Namespace=Strings.DEFAULT_NAMESPACE)]
      public partial class Activity
      {

      
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  bool  m_isInitiator;
        public const string PropertyNameIsInitiator = "IsInitiator";
      

      // public properties members
      


        [XmlAttribute]
        public bool IsInitiator
        {
        set
        {
        if(m_isInitiator== value) return;
        var arg = new PropertyChangingEventArgs(PropertyNameIsInitiator, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_isInitiator= value;
        OnPropertyChanged();
        }
        }
        get
        {
        return m_isInitiator;}
        }
      


      }

    
      [JsonConverter(typeof(StringEnumConverter))]
      public enum FieldType
      {
      DocumentField,
      ConstantField,
      FunctionField,
      }
    
      [JsonConverter(typeof(StringEnumConverter))]
      public enum Operator
      {
      Eq,
      Lt,
      Le,
      Gt,
      Ge,
      Substringof,
      StartsWith,
      EndsWith,
      NotContains,
      }
    
    }
// ReSharper restore InconsistentNaming

  