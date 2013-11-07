
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
    [XmlType("Trigger", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Trigger
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeOf;
        public const string PropertyNameTypeOf = "TypeOf";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_triggerId;
        public const string PropertyNameTriggerId = "TriggerId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isFiredOnAdded;
        public const string PropertyNameIsFiredOnAdded = "IsFiredOnAdded";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isFiredOnDeleted;
        public const string PropertyNameIsFiredOnDeleted = "IsFiredOnDeleted";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isFiredOnChanged;
        public const string PropertyNameIsFiredOnChanged = "IsFiredOnChanged";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_firedOnOperations;
        public const string PropertyNameFiredOnOperations = "FiredOnOperations";


        private readonly ObjectCollection<Rule> m_RuleCollection = new ObjectCollection<Rule>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Rule", IsNullable = false)]
        public ObjectCollection<Rule> RuleCollection
        {
            get { return m_RuleCollection; }
        }

        private readonly ObjectCollection<CustomAction> m_ActionCollection = new ObjectCollection<CustomAction>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<CustomAction> ActionCollection
        {
            get { return m_ActionCollection; }
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

        public string Entity
        {
            set
            {
                if (String.Equals(m_entity, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntity, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entity = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entity;
            }
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
                if (String.Equals(m_typeOf, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTypeOf, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_typeOf = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_typeOf;
            }
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
                if (m_triggerId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTriggerId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_triggerId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_triggerId;
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

        public bool IsFiredOnAdded
        {
            set
            {
                if (m_isFiredOnAdded == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsFiredOnAdded, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isFiredOnAdded = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isFiredOnAdded;
            }
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
                if (m_isFiredOnDeleted == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsFiredOnDeleted, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isFiredOnDeleted = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isFiredOnDeleted;
            }
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
                if (m_isFiredOnChanged == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsFiredOnChanged, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isFiredOnChanged = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isFiredOnChanged;
            }
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
                if (String.Equals(m_firedOnOperations, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFiredOnOperations, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_firedOnOperations = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_firedOnOperations;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("AssemblyField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class AssemblyField
    {

        private string m_Location;
        [XmlAttribute]
        public string Location
        {
            get
            {
                return m_Location;
            }
            set
            {
                m_Location = value;
                RaisePropertyChanged();
            }
        }


        private string m_TypeName;
        [XmlAttribute]
        public string TypeName
        {
            get
            {
                return m_TypeName;
            }
            set
            {
                m_TypeName = value;
                RaisePropertyChanged();
            }
        }


        private string m_Method;
        [XmlAttribute]
        public string Method
        {
            get
            {
                return m_Method;
            }
            set
            {
                m_Method = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsAsync;
        [XmlAttribute]
        public bool IsAsync
        {
            get
            {
                return m_IsAsync;
            }
            set
            {
                m_IsAsync = value;
                RaisePropertyChanged();
            }
        }


        private int m_AsyncTimeout;
        [XmlAttribute]
        public int AsyncTimeout
        {
            get
            {
                return m_AsyncTimeout;
            }
            set
            {
                m_AsyncTimeout = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<MethodArg> m_MethodArgCollection = new ObjectCollection<MethodArg>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("MethodArg", IsNullable = false)]
        public ObjectCollection<MethodArg> MethodArgCollection
        {
            get { return m_MethodArgCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FunctionField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FunctionField
    {

        private string m_Script;
        [XmlAttribute]
        public string Script
        {
            get
            {
                return m_Script;
            }
            set
            {
                m_Script = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ConstantField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ConstantField
    {

        private string m_TypeName;
        [XmlAttribute]
        public string TypeName
        {
            get
            {
                return m_TypeName;
            }
            set
            {
                m_TypeName = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DocumentField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DocumentField
    {

        private string m_XPath;
        [XmlAttribute]
        public string XPath
        {
            get
            {
                return m_XPath;
            }
            set
            {
                m_XPath = value;
                RaisePropertyChanged();
            }
        }


        private string m_NamespacePrefix;
        [XmlAttribute]
        public string NamespacePrefix
        {
            get
            {
                return m_NamespacePrefix;
            }
            set
            {
                m_NamespacePrefix = value;
                RaisePropertyChanged();
            }
        }


        private string m_TypeName;
        [XmlAttribute]
        public string TypeName
        {
            get
            {
                return m_TypeName;
            }
            set
            {
                m_TypeName = value;
                RaisePropertyChanged();
            }
        }


        private string m_Path;
        [XmlAttribute]
        public string Path
        {
            get
            {
                return m_Path;
            }
            set
            {
                m_Path = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("PropertyChangedField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class PropertyChangedField
    {

        private string m_Path;
        [XmlAttribute]
        public string Path
        {
            get
            {
                return m_Path;
            }
            set
            {
                m_Path = value;
                RaisePropertyChanged();
            }
        }


        private string m_TypeName;
        [XmlAttribute]
        public string TypeName
        {
            get
            {
                return m_TypeName;
            }
            set
            {
                m_TypeName = value;
                RaisePropertyChanged();
            }
        }


        private string m_OldValue;
        [XmlAttribute]
        public string OldValue
        {
            get
            {
                return m_OldValue;
            }
            set
            {
                m_OldValue = value;
                RaisePropertyChanged();
            }
        }


        private string m_NewValue;
        [XmlAttribute]
        public string NewValue
        {
            get
            {
                return m_NewValue;
            }
            set
            {
                m_NewValue = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Rule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Rule
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_left;
        public const string PropertyNameLeft = "Left";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_right;
        public const string PropertyNameRight = "Right";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Operator m_operator;
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
                if (m_left == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLeft, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_left = value;
                    OnPropertyChanged();
                }
            }
            get { return m_left; }
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
                if (m_right == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRight, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_right = value;
                    OnPropertyChanged();
                }
            }
            get { return m_right; }
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
                if (m_operator == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperator, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operator = value;
                    OnPropertyChanged();
                }
            }
            get { return m_operator; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EmailAction", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EmailAction
    {

        private string m_From;
        [XmlAttribute]
        public string From
        {
            get
            {
                return m_From;
            }
            set
            {
                m_From = value;
                RaisePropertyChanged();
            }
        }


        private string m_To;
        [XmlAttribute]
        public string To
        {
            get
            {
                return m_To;
            }
            set
            {
                m_To = value;
                RaisePropertyChanged();
            }
        }


        private string m_SubjectTemplate;
        [XmlAttribute]
        public string SubjectTemplate
        {
            get
            {
                return m_SubjectTemplate;
            }
            set
            {
                m_SubjectTemplate = value;
                RaisePropertyChanged();
            }
        }


        private string m_BodyTemplate;
        [XmlAttribute]
        public string BodyTemplate
        {
            get
            {
                return m_BodyTemplate;
            }
            set
            {
                m_BodyTemplate = value;
                RaisePropertyChanged();
            }
        }


        private string m_Bcc;
        [XmlAttribute]
        public string Bcc
        {
            get
            {
                return m_Bcc;
            }
            set
            {
                m_Bcc = value;
                RaisePropertyChanged();
            }
        }


        private string m_Cc;
        [XmlAttribute]
        public string Cc
        {
            get
            {
                return m_Cc;
            }
            set
            {
                m_Cc = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SetterAction", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SetterAction
    {

        private readonly ObjectCollection<SetterActionChild> m_SetterActionChildCollection = new ObjectCollection<SetterActionChild>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("SetterActionChild", IsNullable = false)]
        public ObjectCollection<SetterActionChild> SetterActionChildCollection
        {
            get { return m_SetterActionChildCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SetterActionChild", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SetterActionChild
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_field;
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
                if (String.Equals(m_path, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_path = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_path;
            }
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
                if (m_field == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameField, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_field = value;
                    OnPropertyChanged();
                }
            }
            get { return m_field; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("MethodArg", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class MethodArg
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_valueProvider;
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

        public string TypeName
        {
            set
            {
                if (String.Equals(m_typeName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_typeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_typeName;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public Field ValueProvider
        {
            set
            {
                if (m_valueProvider == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValueProvider, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_valueProvider = value;
                    OnPropertyChanged();
                }
            }
            get { return m_valueProvider; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("WorkflowDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WorkflowDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_schemaStoreId;
        public const string PropertyNameSchemaStoreId = "SchemaStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_version;
        public const string PropertyNameVersion = "Version";


        private readonly ObjectCollection<Activity> m_ActivityCollection = new ObjectCollection<Activity>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<Activity> ActivityCollection
        {
            get { return m_ActivityCollection; }
        }

        private readonly ObjectCollection<Variable> m_VariableDefinitionCollection = new ObjectCollection<Variable>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<Variable> VariableDefinitionCollection
        {
            get { return m_VariableDefinitionCollection; }
        }

        private readonly ObjectCollection<PropertyMapping> m_PropertyMappingCollection = new ObjectCollection<PropertyMapping>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<PropertyMapping> PropertyMappingCollection
        {
            get { return m_PropertyMappingCollection; }
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
                if (m_workflowDefinitionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWorkflowDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_workflowDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_workflowDefinitionId;
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

        public string SchemaStoreId
        {
            set
            {
                if (String.Equals(m_schemaStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSchemaStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_schemaStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_schemaStoreId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Version
        {
            set
            {
                if (m_version == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVersion, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_version = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_version;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Workflow", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Workflow
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_workflowId;
        public const string PropertyNameWorkflowId = "WorkflowId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_state;
        public const string PropertyNameState = "State";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_version;
        public const string PropertyNameVersion = "Version";


        private readonly ObjectCollection<VariableValue> m_VariableValueCollection = new ObjectCollection<VariableValue>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("VariableValue", IsNullable = false)]
        public ObjectCollection<VariableValue> VariableValueCollection
        {
            get { return m_VariableValueCollection; }
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
                if (m_workflowId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWorkflowId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_workflowId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_workflowId;
            }
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
                if (m_workflowDefinitionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWorkflowDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_workflowDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_workflowDefinitionId;
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

        public int Version
        {
            set
            {
                if (m_version == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVersion, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_version = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_version;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ScreenActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ScreenActivity
    {

        private string m_Title;
        [XmlAttribute]
        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
                RaisePropertyChanged();
            }
        }


        private string m_ViewVirtualPath;
        [XmlAttribute]
        public string ViewVirtualPath
        {
            get
            {
                return m_ViewVirtualPath;
            }
            set
            {
                m_ViewVirtualPath = value;
                RaisePropertyChanged();
            }
        }


        private int m_WorkflowDefinitionId;
        [XmlAttribute]
        public int WorkflowDefinitionId
        {
            get
            {
                return m_WorkflowDefinitionId;
            }
            set
            {
                m_WorkflowDefinitionId = value;
                RaisePropertyChanged();
            }
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FormDesign m_formDesign
                = new FormDesign();

        public const string PropertyNameFormDesign = "FormDesign";
        [DebuggerHidden]

        public FormDesign FormDesign
        {
            get { return m_formDesign; }
            set
            {
                m_formDesign = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Performer m_performer
                = new Performer();

        public const string PropertyNamePerformer = "Performer";
        [DebuggerHidden]

        public Performer Performer
        {
            get { return m_performer; }
            set
            {
                m_performer = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DecisionActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DecisionActivity
    {

        private readonly ObjectCollection<DecisionBranch> m_DecisionBranchCollection = new ObjectCollection<DecisionBranch>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DecisionBranch", IsNullable = false)]
        public ObjectCollection<DecisionBranch> DecisionBranchCollection
        {
            get { return m_DecisionBranchCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DecisionBranch", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DecisionBranch
    {

        private bool m_IsDefault;
        [XmlAttribute]
        public bool IsDefault
        {
            get
            {
                return m_IsDefault;
            }
            set
            {
                m_IsDefault = value;
                RaisePropertyChanged();
            }
        }


        public string Expression { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("NotificationActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class NotificationActivity
    {

        private string m_From;
        [XmlAttribute]
        public string From
        {
            get
            {
                return m_From;
            }
            set
            {
                m_From = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SimpleVariable", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SimpleVariable
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ComplexVariable", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComplexVariable
    {


    }

    // placeholder for FormDesign
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("VariableValue", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class VariableValue
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


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
    [XmlType("Page", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Page
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_pageId;
        public const string PropertyNamePageId = "PageId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRazor;
        public const string PropertyNameIsRazor = "IsRazor";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPartial;
        public const string PropertyNameIsPartial = "IsPartial";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_virtualPath;
        public const string PropertyNameVirtualPath = "VirtualPath";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_code;
        public const string PropertyNameCode = "Code";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int PageId
        {
            set
            {
                if (m_pageId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePageId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_pageId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_pageId;
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

        public bool IsRazor
        {
            set
            {
                if (m_isRazor == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRazor, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRazor = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRazor;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsPartial
        {
            set
            {
                if (m_isPartial == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPartial, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPartial = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPartial;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string VirtualPath
        {
            set
            {
                if (String.Equals(m_virtualPath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVirtualPath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_virtualPath = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_virtualPath;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Code
        {
            set
            {
                if (String.Equals(m_code, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCode, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_code = value;
                    OnPropertyChanged();
                }
            }
            get { return m_code; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EndActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EndActivity
    {

        private bool m_IsTerminating;
        [XmlAttribute]
        public bool IsTerminating
        {
            get
            {
                return m_IsTerminating;
            }
            set
            {
                m_IsTerminating = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Performer", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Performer
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_userProperty;
        public const string PropertyNameUserProperty = "UserProperty";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_value;
        public const string PropertyNameValue = "Value";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string UserProperty
        {
            set
            {
                if (String.Equals(m_userProperty, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserProperty, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userProperty = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userProperty;
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
    [XmlType("WorkflowDesigner", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WorkflowDesigner
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_x;
        public const string PropertyNameX = "X";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_y;
        public const string PropertyNameY = "Y";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public int X
        {
            set
            {
                if (m_x == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameX, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_x = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_x;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [DebuggerHidden]

        public int Y
        {
            set
            {
                if (m_y == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameY, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_y = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_y;
            }
        }



    }

    [XmlType("Field", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Field
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_note;
        public const string PropertyNameNote = "Note";


        // public properties members



        [XmlAttribute]
        public string Name
        {
            set
            {
                if (m_name == value) return;
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



        [XmlAttribute]
        public string Note
        {
            set
            {
                if (m_note == value) return;
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


    [XmlType("CustomAction", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CustomAction
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_title;
        public const string PropertyNameTitle = "Title";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int m_triggerId;
        public const string PropertyNameTriggerId = "TriggerId";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_note;
        public const string PropertyNameNote = "Note";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int m_customActionId;
        public const string PropertyNameCustomActionId = "CustomActionId";


        // public properties members



        [XmlAttribute]
        public string Title
        {
            set
            {
                if (m_title == value) return;
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



        [XmlAttribute]
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



        [XmlAttribute]
        public int TriggerId
        {
            set
            {
                if (m_triggerId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTriggerId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_triggerId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_triggerId;
            }
        }



        [XmlAttribute]
        public string Note
        {
            set
            {
                if (m_note == value) return;
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



        [XmlAttribute]
        public int CustomActionId
        {
            set
            {
                if (m_customActionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCustomActionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_customActionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_customActionId;
            }
        }



    }


    [XmlType("Activity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Activity
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isInitiator;
        public const string PropertyNameIsInitiator = "IsInitiator";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_nextActivityWebId;
        public const string PropertyNameNextActivityWebId = "NextActivityWebId";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WorkflowDesigner m_workflowDesigner
                = new WorkflowDesigner();

        public const string PropertyNameWorkflowDesigner = "WorkflowDesigner";
        [DebuggerHidden]

        public WorkflowDesigner WorkflowDesigner
        {
            get { return m_workflowDesigner; }
            set
            {
                m_workflowDesigner = value;
                OnPropertyChanged();
            }
        }


        // public properties members



        [XmlAttribute]
        public bool IsInitiator
        {
            set
            {
                if (m_isInitiator == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsInitiator, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isInitiator = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isInitiator;
            }
        }



        [XmlAttribute]
        public string NextActivityWebId
        {
            set
            {
                if (m_nextActivityWebId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNextActivityWebId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_nextActivityWebId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_nextActivityWebId;
            }
        }



        [XmlAttribute]
        public string Name
        {
            set
            {
                if (m_name == value) return;
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


    [XmlType("Variable", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Variable
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_defaultValue;
        public const string PropertyNameDefaultValue = "DefaultValue";


        // public properties members



        [XmlAttribute]
        public string Name
        {
            set
            {
                if (m_name == value) return;
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



        [XmlAttribute]
        public string TypeName
        {
            set
            {
                if (m_typeName == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_typeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_typeName;
            }
        }



        [XmlAttribute]
        public string DefaultValue
        {
            set
            {
                if (m_defaultValue == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDefaultValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_defaultValue = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_defaultValue;
            }
        }



    }


    [XmlType("PropertyMapping", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class PropertyMapping
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_source;
        public const string PropertyNameSource = "Source";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_destination;
        public const string PropertyNameDestination = "Destination";


        // public properties members



        [XmlAttribute]
        public string Source
        {
            set
            {
                if (m_source == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSource, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_source = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_source;
            }
        }



        [XmlAttribute]
        public string Destination
        {
            set
            {
                if (m_destination == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDestination, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_destination = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_destination;
            }
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

