﻿
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
          [XmlType("FieldChangeField",  Namespace=Strings.DEFAULT_NAMESPACE)]
          public  partial class FieldChangeField
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

  