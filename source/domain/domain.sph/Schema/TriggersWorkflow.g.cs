
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

        private readonly ObjectCollection<ReferencedAssembly> m_ReferencedAssemblyCollection = new ObjectCollection<ReferencedAssembly>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReferencedAssembly", IsNullable = false)]
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection
        {
            get { return m_ReferencedAssemblyCollection; }
        }

        private readonly ObjectCollection<ExceptionFilter> m_RequeueFilterCollection = new ObjectCollection<ExceptionFilter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ExceptionFilter", IsNullable = false)]
        public ObjectCollection<ExceptionFilter> RequeueFilterCollection
        {
            get { return m_RequeueFilterCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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


        private bool m_LoadInCurrentAppDomain;
        [XmlAttribute]
        public bool LoadInCurrentAppDomain
        {
            get
            {
                return m_LoadInCurrentAppDomain;
            }
            set
            {
                m_LoadInCurrentAppDomain = value;
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
    [XmlType("JavascriptExpressionField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class JavascriptExpressionField
    {

        private string m_Expression;
        [XmlAttribute]
        public string Expression
        {
            get
            {
                return m_Expression;
            }
            set
            {
                m_Expression = value;
                RaisePropertyChanged();
            }
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
    [XmlType("StartWorkflowAction", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class StartWorkflowAction
    {

        private string m_WorkflowDefinitionId;
        [XmlAttribute]
        public string WorkflowDefinitionId
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


        private string m_Name;
        [XmlAttribute]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                RaisePropertyChanged();
            }
        }


        private int m_Version;
        [XmlAttribute]
        public int Version
        {
            get
            {
                return m_Version;
            }
            set
            {
                m_Version = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<WorkflowTriggerMap> m_WorkflowTriggerMapCollection = new ObjectCollection<WorkflowTriggerMap>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("WorkflowTriggerMap", IsNullable = false)]
        public ObjectCollection<WorkflowTriggerMap> WorkflowTriggerMapCollection
        {
            get { return m_WorkflowTriggerMapCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("WorkflowTriggerMap", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WorkflowTriggerMap
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_variablePath;
        public const string PropertyNameVariablePath = "VariablePath";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_field;
        public const string PropertyNameField = "Field";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string VariablePath
        {
            set
            {
                if (String.Equals(m_variablePath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVariablePath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_variablePath = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_variablePath;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

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
    [XmlType("AssemblyAction", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class AssemblyAction
    {

        private bool m_IsAsyncMethod;
        [XmlAttribute]
        public bool IsAsyncMethod
        {
            get
            {
                return m_IsAsyncMethod;
            }
            set
            {
                m_IsAsyncMethod = value;
                RaisePropertyChanged();
            }
        }


        private string m_Assembly;
        [XmlAttribute]
        public string Assembly
        {
            get
            {
                return m_Assembly;
            }
            set
            {
                m_Assembly = value;
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


        private string m_ReturnType;
        [XmlAttribute]
        public string ReturnType
        {
            get
            {
                return m_ReturnType;
            }
            set
            {
                m_ReturnType = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsVoid;
        [XmlAttribute]
        public bool IsVoid
        {
            get
            {
                return m_IsVoid;
            }
            set
            {
                m_IsVoid = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsStatic;
        [XmlAttribute]
        public bool IsStatic
        {
            get
            {
                return m_IsStatic;
            }
            set
            {
                m_IsStatic = value;
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
    [XmlType("WorkflowDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WorkflowDefinition
    {

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

        private readonly ObjectCollection<ReferencedAssembly> m_ReferencedAssemblyCollection = new ObjectCollection<ReferencedAssembly>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReferencedAssembly", IsNullable = false)]
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection
        {
            get { return m_ReferencedAssemblyCollection; }
        }

        private readonly ObjectCollection<CorrelationSet> m_CorrelationSetCollection = new ObjectCollection<CorrelationSet>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CorrelationSet", IsNullable = false)]
        public ObjectCollection<CorrelationSet> CorrelationSetCollection
        {
            get { return m_CorrelationSetCollection; }
        }

        private readonly ObjectCollection<CorrelationType> m_CorrelationTypeCollection = new ObjectCollection<CorrelationType>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CorrelationType", IsNullable = false)]
        public ObjectCollection<CorrelationType> CorrelationTypeCollection
        {
            get { return m_CorrelationTypeCollection; }
        }

        private readonly ObjectCollection<TryScope> m_TryScopeCollection = new ObjectCollection<TryScope>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("TryScope", IsNullable = false)]
        public ObjectCollection<TryScope> TryScopeCollection
        {
            get { return m_TryScopeCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        private string m_workflowDefinitionId;
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
        [DebuggerHidden]

        [Required]
        public string WorkflowDefinitionId
        {
            set
            {
                if (String.Equals(m_workflowDefinitionId, value, StringComparison.Ordinal)) return;
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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


        private string m_CancelMessageSubject;
        [XmlAttribute]
        public string CancelMessageSubject
        {
            get
            {
                return m_CancelMessageSubject;
            }
            set
            {
                m_CancelMessageSubject = value;
                RaisePropertyChanged();
            }
        }


        private string m_InvitationMessageSubject;
        [XmlAttribute]
        public string InvitationMessageSubject
        {
            get
            {
                return m_InvitationMessageSubject;
            }
            set
            {
                m_InvitationMessageSubject = value;
                RaisePropertyChanged();
            }
        }


        private string m_CancelMessageBody;
        [XmlAttribute]
        public string CancelMessageBody
        {
            get
            {
                return m_CancelMessageBody;
            }
            set
            {
                m_CancelMessageBody = value;
                RaisePropertyChanged();
            }
        }


        private string m_InvitationMessageBody;
        [XmlAttribute]
        public string InvitationMessageBody
        {
            get
            {
                return m_InvitationMessageBody;
            }
            set
            {
                m_InvitationMessageBody = value;
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConfirmationOptions m_confirmationOptions
                = new ConfirmationOptions();

        public const string PropertyNameConfirmationOptions = "ConfirmationOptions";
        [DebuggerHidden]

        public ConfirmationOptions ConfirmationOptions
        {
            get { return m_confirmationOptions; }
            set
            {
                m_confirmationOptions = value;
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


        private string m_Subject;
        [XmlAttribute]
        public string Subject
        {
            get
            {
                return m_Subject;
            }
            set
            {
                m_Subject = value;
                RaisePropertyChanged();
            }
        }


        private string m_Body;
        [XmlAttribute]
        public string Body
        {
            get
            {
                return m_Body;
            }
            set
            {
                m_Body = value;
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


        private string m_UserName;
        [XmlAttribute]
        public string UserName
        {
            get
            {
                return m_UserName;
            }
            set
            {
                m_UserName = value;
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


        private bool m_IsHtmlEmail;
        [XmlAttribute]
        public bool IsHtmlEmail
        {
            get
            {
                return m_IsHtmlEmail;
            }
            set
            {
                m_IsHtmlEmail = value;
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        private string m_name;
        public const string PropertyNameName = "Name";


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
        private string m_tag;
        public const string PropertyNameTag = "Tag";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_version;
        public const string PropertyNameVersion = "Version";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mode;
        public const string PropertyNameMode = "Mode";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_extension;
        public const string PropertyNameExtension = "Extension";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_code;
        public const string PropertyNameCode = "Code";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Tag
        {
            set
            {
                if (String.Equals(m_tag, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTag, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tag = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tag;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Mode
        {
            set
            {
                if (String.Equals(m_mode, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMode, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mode = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_mode;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_extension;
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


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublic;
        public const string PropertyNameIsPublic = "IsPublic";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public bool IsPublic
        {
            set
            {
                if (m_isPublic == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPublic, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPublic = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPublic;
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

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SimpleMapping", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SimpleMapping
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FunctoidMapping", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FunctoidMapping
    {

        public Functoid Functoid { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CreateEntityActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CreateEntityActivity
    {

        private string m_EntityType;
        [XmlAttribute]
        public string EntityType
        {
            get
            {
                return m_EntityType;
            }
            set
            {
                m_EntityType = value;
                RaisePropertyChanged();
            }
        }


        private string m_ReturnValuePath;
        [XmlAttribute]
        public string ReturnValuePath
        {
            get
            {
                return m_ReturnValuePath;
            }
            set
            {
                m_ReturnValuePath = value;
                RaisePropertyChanged();
            }
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


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ExpressionActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ExpressionActivity
    {

        public string Expression { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DeleteEntityActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DeleteEntityActivity
    {

        private string m_EntityType;
        [XmlAttribute]
        public string EntityType
        {
            get
            {
                return m_EntityType;
            }
            set
            {
                m_EntityType = value;
                RaisePropertyChanged();
            }
        }


        private string m_EntityIdPath;
        [XmlAttribute]
        public string EntityIdPath
        {
            get
            {
                return m_EntityIdPath;
            }
            set
            {
                m_EntityIdPath = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("UpdateEntityActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class UpdateEntityActivity
    {

        private string m_EntityType;
        [XmlAttribute]
        public string EntityType
        {
            get
            {
                return m_EntityType;
            }
            set
            {
                m_EntityType = value;
                RaisePropertyChanged();
            }
        }


        private string m_EntityIdPath;
        [XmlAttribute]
        public string EntityIdPath
        {
            get
            {
                return m_EntityIdPath;
            }
            set
            {
                m_EntityIdPath = value;
                RaisePropertyChanged();
            }
        }


        private string m_UseVariable;
        [XmlAttribute]
        public string UseVariable
        {
            get
            {
                return m_UseVariable;
            }
            set
            {
                m_UseVariable = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsUsingVariable;
        [XmlAttribute]
        public bool IsUsingVariable
        {
            get
            {
                return m_IsUsingVariable;
            }
            set
            {
                m_IsUsingVariable = value;
                RaisePropertyChanged();
            }
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


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ScriptFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ScriptFunctoid
    {

        private string m_Expression;
        [XmlAttribute]
        public string Expression
        {
            get
            {
                return m_Expression;
            }
            set
            {
                m_Expression = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ConfirmationOptions", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ConfirmationOptions
    {

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
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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

        [Required]
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
    [XmlType("ReceiveActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReceiveActivity
    {

        private string m_PortType;
        [XmlAttribute]
        public string PortType
        {
            get
            {
                return m_PortType;
            }
            set
            {
                m_PortType = value;
                RaisePropertyChanged();
            }
        }


        private string m_Operation;
        [XmlAttribute]
        public string Operation
        {
            get
            {
                return m_Operation;
            }
            set
            {
                m_Operation = value;
                RaisePropertyChanged();
            }
        }


        private string m_MessagePath;
        [XmlAttribute]
        public string MessagePath
        {
            get
            {
                return m_MessagePath;
            }
            set
            {
                m_MessagePath = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<string> m_InitializingCorrelationSetCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> InitializingCorrelationSetCollection
        {
            get { return m_InitializingCorrelationSetCollection; }
        }

        private readonly ObjectCollection<string> m_FollowingCorrelationSetCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> FollowingCorrelationSetCollection
        {
            get { return m_FollowingCorrelationSetCollection; }
        }

        private readonly ObjectCollection<CorrelationProperty> m_CorrelationPropertyCollection = new ObjectCollection<CorrelationProperty>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CorrelationProperty", IsNullable = false)]
        public ObjectCollection<CorrelationProperty> CorrelationPropertyCollection
        {
            get { return m_CorrelationPropertyCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SendActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SendActivity
    {

        private string m_PortType;
        [XmlAttribute]
        public string PortType
        {
            get
            {
                return m_PortType;
            }
            set
            {
                m_PortType = value;
                RaisePropertyChanged();
            }
        }


        private string m_Adapter;
        [XmlAttribute]
        public string Adapter
        {
            get
            {
                return m_Adapter;
            }
            set
            {
                m_Adapter = value;
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


        private string m_AdapterAssembly;
        [XmlAttribute]
        public string AdapterAssembly
        {
            get
            {
                return m_AdapterAssembly;
            }
            set
            {
                m_AdapterAssembly = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsSynchronous;
        [XmlAttribute]
        public bool IsSynchronous
        {
            get
            {
                return m_IsSynchronous;
            }
            set
            {
                m_IsSynchronous = value;
                RaisePropertyChanged();
            }
        }


        private string m_ArgumentPath;
        [XmlAttribute]
        public string ArgumentPath
        {
            get
            {
                return m_ArgumentPath;
            }
            set
            {
                m_ArgumentPath = value;
                RaisePropertyChanged();
            }
        }


        private string m_ReturnValuePath;
        [XmlAttribute]
        public string ReturnValuePath
        {
            get
            {
                return m_ReturnValuePath;
            }
            set
            {
                m_ReturnValuePath = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<ExceptionFilter> m_ExceptionFilterCollection = new ObjectCollection<ExceptionFilter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ExceptionFilter", IsNullable = false)]
        public ObjectCollection<ExceptionFilter> ExceptionFilterCollection
        {
            get { return m_ExceptionFilterCollection; }
        }

        private readonly ObjectCollection<string> m_InitializingCorrelationSetCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> InitializingCorrelationSetCollection
        {
            get { return m_InitializingCorrelationSetCollection; }
        }

        private readonly ObjectCollection<string> m_FollowingCorrelationSetCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> FollowingCorrelationSetCollection
        {
            get { return m_FollowingCorrelationSetCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ListenActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ListenActivity
    {

        private readonly ObjectCollection<ListenBranch> m_ListenBranchCollection = new ObjectCollection<ListenBranch>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ListenBranch", IsNullable = false)]
        public ObjectCollection<ListenBranch> ListenBranchCollection
        {
            get { return m_ListenBranchCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParallelActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParallelActivity
    {

        private readonly ObjectCollection<ParallelBranch> m_ParallelBranchCollection = new ObjectCollection<ParallelBranch>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ParallelBranch", IsNullable = false)]
        public ObjectCollection<ParallelBranch> ParallelBranchCollection
        {
            get { return m_ParallelBranchCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("JoinActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class JoinActivity
    {

        private string m_Placeholder;
        [XmlAttribute]
        public string Placeholder
        {
            get
            {
                return m_Placeholder;
            }
            set
            {
                m_Placeholder = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DelayActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DelayActivity
    {

        private string m_Expression;
        [XmlAttribute]
        public string Expression
        {
            get
            {
                return m_Expression;
            }
            set
            {
                m_Expression = value;
                RaisePropertyChanged();
            }
        }


        public long Miliseconds { get; set; }

        public int Seconds { get; set; }

        public int Hour { get; set; }

        public int Days { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ThrowActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ThrowActivity
    {

        private string m_Message;
        [XmlAttribute]
        public string Message
        {
            get
            {
                return m_Message;
            }
            set
            {
                m_Message = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParallelBranch", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParallelBranch
    {

        private readonly ObjectCollection<Activity> m_ActivityCollection = new ObjectCollection<Activity>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<Activity> ActivityCollection
        {
            get { return m_ActivityCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ListenBranch", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ListenBranch
    {

        private bool m_IsWaitingAsync;
        [XmlAttribute]
        public bool IsWaitingAsync
        {
            get
            {
                return m_IsWaitingAsync;
            }
            set
            {
                m_IsWaitingAsync = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsDestroyed;
        [XmlAttribute]
        public bool IsDestroyed
        {
            get
            {
                return m_IsDestroyed;
            }
            set
            {
                m_IsDestroyed = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ClrTypeVariable", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ClrTypeVariable
    {

        private string m_Assembly;
        [XmlAttribute]
        public string Assembly
        {
            get
            {
                return m_Assembly;
            }
            set
            {
                m_Assembly = value;
                RaisePropertyChanged();
            }
        }


        private bool m_CanInitiateWithDefaultConstructor;
        [XmlAttribute]
        public bool CanInitiateWithDefaultConstructor
        {
            get
            {
                return m_CanInitiateWithDefaultConstructor;
            }
            set
            {
                m_CanInitiateWithDefaultConstructor = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ScheduledTriggerActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ScheduledTriggerActivity
    {

        private readonly ObjectCollection<IntervalSchedule> m_IntervalScheduleCollection = new ObjectCollection<IntervalSchedule>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("IntervalSchedule", IsNullable = false)]
        public ObjectCollection<IntervalSchedule> IntervalScheduleCollection
        {
            get { return m_IntervalScheduleCollection; }
        }


    }

    // placeholder for IntervalSchedule
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Tracker", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Tracker
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowId;
        public const string PropertyNameWorkflowId = "WorkflowId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


        private readonly ObjectCollection<string> m_ForbiddenActivities = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> ForbiddenActivities
        {
            get { return m_ForbiddenActivities; }
        }

        private readonly ObjectCollection<ExecutedActivity> m_ExecutedActivityCollection = new ObjectCollection<ExecutedActivity>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ExecutedActivity", IsNullable = false)]
        public ObjectCollection<ExecutedActivity> ExecutedActivityCollection
        {
            get { return m_ExecutedActivityCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string WorkflowId
        {
            set
            {
                if (String.Equals(m_workflowId, value, StringComparison.Ordinal)) return;
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
        [DebuggerHidden]

        [Required]
        public string WorkflowDefinitionId
        {
            set
            {
                if (String.Equals(m_workflowDefinitionId, value, StringComparison.Ordinal)) return;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ExecutedActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ExecutedActivity
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_instanceId;
        public const string PropertyNameInstanceId = "InstanceId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_activityWebId;
        public const string PropertyNameActivityWebId = "ActivityWebId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_user;
        public const string PropertyNameUser = "User";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_initiated;
        public const string PropertyNameInitiated = "Initiated";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_run;
        public const string PropertyNameRun = "Run";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string InstanceId
        {
            set
            {
                if (String.Equals(m_instanceId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInstanceId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_instanceId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_instanceId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string ActivityWebId
        {
            set
            {
                if (String.Equals(m_activityWebId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameActivityWebId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_activityWebId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_activityWebId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string WorkflowDefinitionId
        {
            set
            {
                if (String.Equals(m_workflowDefinitionId, value, StringComparison.Ordinal)) return;
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
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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
        [DebuggerHidden]

        public DateTime? Initiated
        {
            set
            {
                if (m_initiated == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInitiated, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_initiated = value;
                    OnPropertyChanged();
                }
            }
            get { return m_initiated; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public DateTime? Run
        {
            set
            {
                if (m_run == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRun, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_run = value;
                    OnPropertyChanged();
                }
            }
            get { return m_run; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Breakpoint", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Breakpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isEnabled;
        public const string PropertyNameIsEnabled = "IsEnabled";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_activityWebId;
        public const string PropertyNameActivityWebId = "ActivityWebId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_conditionExpression;
        public const string PropertyNameConditionExpression = "ConditionExpression";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_hitCount;
        public const string PropertyNameHitCount = "HitCount";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_label;
        public const string PropertyNameLabel = "Label";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_whenHitPrintMessage;
        public const string PropertyNameWhenHitPrintMessage = "WhenHitPrintMessage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_whenHitContinueExecution;
        public const string PropertyNameWhenHitContinueExecution = "WhenHitContinueExecution";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_messageExpression;
        public const string PropertyNameMessageExpression = "MessageExpression";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsEnabled
        {
            set
            {
                if (m_isEnabled == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsEnabled, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isEnabled = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isEnabled;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string ActivityWebId
        {
            set
            {
                if (String.Equals(m_activityWebId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameActivityWebId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_activityWebId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_activityWebId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string WorkflowDefinitionId
        {
            set
            {
                if (String.Equals(m_workflowDefinitionId, value, StringComparison.Ordinal)) return;
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
        [DebuggerHidden]

        public string ConditionExpression
        {
            set
            {
                if (String.Equals(m_conditionExpression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameConditionExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_conditionExpression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_conditionExpression;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public int HitCount
        {
            set
            {
                if (m_hitCount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHitCount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_hitCount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_hitCount;
            }
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
                if (String.Equals(m_label, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_label = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_label;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool WhenHitPrintMessage
        {
            set
            {
                if (m_whenHitPrintMessage == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWhenHitPrintMessage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_whenHitPrintMessage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_whenHitPrintMessage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool WhenHitContinueExecution
        {
            set
            {
                if (m_whenHitContinueExecution == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWhenHitContinueExecution, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_whenHitContinueExecution = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_whenHitContinueExecution;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public string MessageExpression
        {
            set
            {
                if (String.Equals(m_messageExpression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMessageExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_messageExpression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_messageExpression;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReferencedAssembly", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReferencedAssembly
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fullName;
        public const string PropertyNameFullName = "FullName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_version;
        public const string PropertyNameVersion = "Version";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_location;
        public const string PropertyNameLocation = "Location";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isGac;
        public const string PropertyNameIsGac = "IsGac";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isStrongName;
        public const string PropertyNameIsStrongName = "IsStrongName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_runtimeVersion;
        public const string PropertyNameRuntimeVersion = "RuntimeVersion";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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
        [DebuggerHidden]

        [Required]
        public string Version
        {
            set
            {
                if (String.Equals(m_version, value, StringComparison.Ordinal)) return;
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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
        [DebuggerHidden]

        [Required]
        public bool IsGac
        {
            set
            {
                if (m_isGac == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsGac, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isGac = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isGac;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsStrongName
        {
            set
            {
                if (m_isStrongName == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsStrongName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isStrongName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isStrongName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string RuntimeVersion
        {
            set
            {
                if (String.Equals(m_runtimeVersion, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRuntimeVersion, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_runtimeVersion = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_runtimeVersion;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("MappingActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class MappingActivity
    {

        private string m_MappingDefinition;
        [XmlAttribute]
        public string MappingDefinition
        {
            get
            {
                return m_MappingDefinition;
            }
            set
            {
                m_MappingDefinition = value;
                RaisePropertyChanged();
            }
        }


        private string m_DestinationType;
        [XmlAttribute]
        public string DestinationType
        {
            get
            {
                return m_DestinationType;
            }
            set
            {
                m_DestinationType = value;
                RaisePropertyChanged();
            }
        }


        private string m_OutputPath;
        [XmlAttribute]
        public string OutputPath
        {
            get
            {
                return m_OutputPath;
            }
            set
            {
                m_OutputPath = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<MappingSource> m_MappingSourceCollection = new ObjectCollection<MappingSource>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("MappingSource", IsNullable = false)]
        public ObjectCollection<MappingSource> MappingSourceCollection
        {
            get { return m_MappingSourceCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("MappingSource", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class MappingSource
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_variable;
        public const string PropertyNameVariable = "Variable";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Variable
        {
            set
            {
                if (String.Equals(m_variable, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVariable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_variable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_variable;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("TransformDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class TransformDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_transformDefinitionId;
        public const string PropertyNameTransformDefinitionId = "TransformDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_inputTypeName;
        public const string PropertyNameInputTypeName = "InputTypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_outputTypeName;
        public const string PropertyNameOutputTypeName = "OutputTypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        private readonly ObjectCollection<Map> m_MapCollection = new ObjectCollection<Map>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<Map> MapCollection
        {
            get { return m_MapCollection; }
        }

        private readonly ObjectCollection<Functoid> m_FunctoidCollection = new ObjectCollection<Functoid>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<Functoid> FunctoidCollection
        {
            get { return m_FunctoidCollection; }
        }

        private readonly ObjectCollection<MethodArg> m_InputCollection = new ObjectCollection<MethodArg>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("MethodArg", IsNullable = false)]
        public ObjectCollection<MethodArg> InputCollection
        {
            get { return m_InputCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public int TransformDefinitionId
        {
            set
            {
                if (m_transformDefinitionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTransformDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_transformDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_transformDefinitionId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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
        [DebuggerHidden]

        [Required]
        public string InputTypeName
        {
            set
            {
                if (String.Equals(m_inputTypeName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputTypeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_inputTypeName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string OutputTypeName
        {
            set
            {
                if (String.Equals(m_outputTypeName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOutputTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_outputTypeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_outputTypeName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsPublished
        {
            set
            {
                if (m_isPublished == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPublished, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPublished = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPublished;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DirectMap", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DirectMap
    {

        private string m_Source;
        [XmlAttribute]
        public string Source
        {
            get
            {
                return m_Source;
            }
            set
            {
                m_Source = value;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FunctoidMap", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FunctoidMap
    {

        private string m___uuid;
        [XmlAttribute]
        public string __uuid
        {
            get
            {
                return m___uuid;
            }
            set
            {
                m___uuid = value;
                RaisePropertyChanged();
            }
        }


        private string m_Functoid;
        [XmlAttribute]
        public string Functoid
        {
            get
            {
                return m_Functoid;
            }
            set
            {
                m_Functoid = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("StringConcateFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class StringConcateFunctoid
    {


    }

    ///<summary>
    /// Convert string to bool
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParseBooleanFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParseBooleanFunctoid
    {

        private string m_Format;
        [XmlAttribute]
        public string Format
        {
            get
            {
                return m_Format;
            }
            set
            {
                m_Format = value;
                RaisePropertyChanged();
            }
        }


        private string m_SourceField;
        [XmlAttribute]
        public string SourceField
        {
            get
            {
                return m_SourceField;
            }
            set
            {
                m_SourceField = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// Convert string to double
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParseDoubleFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParseDoubleFunctoid
    {

        private string m_Styles;
        [XmlAttribute]
        public string Styles
        {
            get
            {
                return m_Styles;
            }
            set
            {
                m_Styles = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// Convert string to decimal
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParseDecimalFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParseDecimalFunctoid
    {

        private string m_Styles;
        [XmlAttribute]
        public string Styles
        {
            get
            {
                return m_Styles;
            }
            set
            {
                m_Styles = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// Convert string to int 32
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParseInt32Functoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParseInt32Functoid
    {

        private string m_Styles;
        [XmlAttribute]
        public string Styles
        {
            get
            {
                return m_Styles;
            }
            set
            {
                m_Styles = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// Convert string to date
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ParseDateTimeFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ParseDateTimeFunctoid
    {

        private string m_Format;
        [XmlAttribute]
        public string Format
        {
            get
            {
                return m_Format;
            }
            set
            {
                m_Format = value;
                RaisePropertyChanged();
            }
        }


        private string m_Styles;
        [XmlAttribute]
        public string Styles
        {
            get
            {
                return m_Styles;
            }
            set
            {
                m_Styles = value;
                RaisePropertyChanged();
            }
        }


        private string m_Culture;
        [XmlAttribute]
        public string Culture
        {
            get
            {
                return m_Culture;
            }
            set
            {
                m_Culture = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// User string.format to format any object to string
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FormattingFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FormattingFunctoid
    {

        private string m_Format;
        [XmlAttribute]
        public string Format
        {
            get
            {
                return m_Format;
            }
            set
            {
                m_Format = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FunctoidArg", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FunctoidArg
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_label;
        public const string PropertyNameLabel = "Label";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_comment;
        public const string PropertyNameComment = "Comment";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isOptional;
        public const string PropertyNameIsOptional = "IsOptional";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_functoid;
        public const string PropertyNameFunctoid = "Functoid";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_constant;
        public const string PropertyNameConstant = "Constant";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_default;
        public const string PropertyNameDefault = "Default";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
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
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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
        [DebuggerHidden]

        [Required]
        public string Label
        {
            set
            {
                if (String.Equals(m_label, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_label = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_label;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Comment
        {
            set
            {
                if (String.Equals(m_comment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_comment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_comment;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsOptional
        {
            set
            {
                if (m_isOptional == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsOptional, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isOptional = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isOptional;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Functoid
        {
            set
            {
                if (String.Equals(m_functoid, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFunctoid, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_functoid = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_functoid;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public string Constant
        {
            set
            {
                if (String.Equals(m_constant, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameConstant, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_constant = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_constant;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public string Default
        {
            set
            {
                if (String.Equals(m_default, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDefault, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_default = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_default;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ConstantFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ConstantFunctoid
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


        public object Value { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SourceFunctoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SourceFunctoid
    {

        private string m_Field;
        [XmlAttribute]
        public string Field
        {
            get
            {
                return m_Field;
            }
            set
            {
                m_Field = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ExceptionFilter", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ExceptionFilter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_filter;
        public const string PropertyNameFilter = "Filter";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_interval;
        public const string PropertyNameInterval = "Interval";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_intervalPeriod;
        public const string PropertyNameIntervalPeriod = "IntervalPeriod";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_maxRequeue;
        public const string PropertyNameMaxRequeue = "MaxRequeue";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [XmlAttribute]
        [DebuggerHidden]

        public string Filter
        {
            set
            {
                if (String.Equals(m_filter, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFilter, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_filter = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_filter;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Interval
        {
            set
            {
                if (m_interval == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInterval, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_interval = value;
                    OnPropertyChanged();
                }
            }
            get { return m_interval; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string IntervalPeriod
        {
            set
            {
                if (String.Equals(m_intervalPeriod, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIntervalPeriod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_intervalPeriod = value;
                    OnPropertyChanged();
                }
            }
            get { return m_intervalPeriod; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? MaxRequeue
        {
            set
            {
                if (m_maxRequeue == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaxRequeue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_maxRequeue = value;
                    OnPropertyChanged();
                }
            }
            get { return m_maxRequeue; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CorrelationType", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CorrelationType
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        private readonly ObjectCollection<CorrelationProperty> m_CorrelationPropertyCollection = new ObjectCollection<CorrelationProperty>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CorrelationProperty", IsNullable = false)]
        public ObjectCollection<CorrelationProperty> CorrelationPropertyCollection
        {
            get { return m_CorrelationPropertyCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CorrelationSet", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CorrelationSet
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
                    OnPropertyChanged();
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

        [Required]
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CorrelationProperty", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CorrelationProperty
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_origin;
        public const string PropertyNameOrigin = "Origin";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [XmlAttribute]
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
        [DebuggerHidden]

        public string Origin
        {
            set
            {
                if (String.Equals(m_origin, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrigin, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_origin = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_origin;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ChildWorkflowActivity", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ChildWorkflowActivity
    {

        private string m_WorkflowDefinitionId;
        [XmlAttribute]
        public string WorkflowDefinitionId
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


        private int m_Version;
        [XmlAttribute]
        public int Version
        {
            get
            {
                return m_Version;
            }
            set
            {
                m_Version = value;
                RaisePropertyChanged();
            }
        }


    



        private readonly ObjectCollection<Variable> m_VariableMapCollection = new ObjectCollection<Variable>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<Variable> VariableMapCollection
        {
            get { return m_VariableMapCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("TryScope", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class TryScope
    {

        private readonly ObjectCollection<CatchScope> m_CatchScopeCollection = new ObjectCollection<CatchScope>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("CatchScope", IsNullable = false)]
        public ObjectCollection<CatchScope> CatchScopeCollection
        {
            get { return m_CatchScopeCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CatchScope", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CatchScope
    {

        private string m_ExceptionType;
        [XmlAttribute]
        public string ExceptionType
        {
            get
            {
                return m_ExceptionType;
            }
            set
            {
                m_ExceptionType = value;
                RaisePropertyChanged();
            }
        }


        private string m_ExceptionVar;
        [XmlAttribute]
        public string ExceptionVar
        {
            get
            {
                return m_ExceptionVar;
            }
            set
            {
                m_ExceptionVar = value;
                RaisePropertyChanged();
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

        private string m_triggerId;
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
        public string TriggerId
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

        private string m_tryScope;
        public const string PropertyNameTryScope = "TryScope";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_catchScope;
        public const string PropertyNameCatchScope = "CatchScope";

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



        [XmlAttribute]
        public string TryScope
        {
            set
            {
                if (m_tryScope == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTryScope, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tryScope = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tryScope;
            }
        }



        [XmlAttribute]
        public string CatchScope
        {
            set
            {
                if (m_catchScope == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCatchScope, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_catchScope = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_catchScope;
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



    [XmlType("Functoid", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Functoid
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_outputTypeName;
        public const string PropertyNameOutputTypeName = "OutputTypeName";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_label;
        public const string PropertyNameLabel = "Label";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_comment;
        public const string PropertyNameComment = "Comment";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private double m_x;
        public const string PropertyNameX = "X";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private double m_y;
        public const string PropertyNameY = "Y";

        private readonly ObjectCollection<FunctoidArg> m_ArgumentCollection = new ObjectCollection<FunctoidArg>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("FunctoidArg", IsNullable = false)]
        public ObjectCollection<FunctoidArg> ArgumentCollection
        {
            get { return m_ArgumentCollection; }
        }


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
        public string OutputTypeName
        {
            set
            {
                if (m_outputTypeName == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOutputTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_outputTypeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_outputTypeName;
            }
        }



        [XmlAttribute]
        public string Label
        {
            set
            {
                if (m_label == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_label = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_label;
            }
        }



        [XmlAttribute]
        public string Comment
        {
            set
            {
                if (m_comment == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_comment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_comment;
            }
        }



        [XmlAttribute]
        public double X
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



        [XmlAttribute]
        public double Y
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



    [XmlType("Map", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Map
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_destination;
        public const string PropertyNameDestination = "Destination";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_sourceTypeName;
        public const string PropertyNameSourceTypeName = "SourceTypeName";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_destinationTypeName;
        public const string PropertyNameDestinationTypeName = "DestinationTypeName";


        // public properties members



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



        [XmlAttribute]
        public string SourceTypeName
        {
            set
            {
                if (m_sourceTypeName == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSourceTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sourceTypeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sourceTypeName;
            }
        }



        [XmlAttribute]
        public string DestinationTypeName
        {
            set
            {
                if (m_destinationTypeName == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDestinationTypeName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_destinationTypeName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_destinationTypeName;
            }
        }



    }



    [XmlType("Scope", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Scope
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_id;
        public const string PropertyNameId = "Id";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";


        // public properties members



        [XmlAttribute]
        public string Id
        {
            set
            {
                if (m_id == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_id = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_id;
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
        Neq,
        NotStartsWith,
        NotEndsWith,

    }

}
// ReSharper restore InconsistentNaming

