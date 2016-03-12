
using System;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Rule> RuleCollection { get; } = new ObjectCollection<Rule>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<CustomAction> ActionCollection { get; } = new ObjectCollection<CustomAction>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection { get; } = new ObjectCollection<ReferencedAssembly>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ExceptionFilter> RequeueFilterCollection { get; } = new ObjectCollection<ExceptionFilter>();


        ///<summary>
        /// 
        ///</summary>
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
    public partial class AssemblyField
    {

        private string m_Location;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<MethodArg> MethodArgCollection { get; } = new ObjectCollection<MethodArg>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class JavascriptExpressionField
    {

        private string m_Expression;
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
    public partial class RouteParameterField
    {

        private string m_Expression;
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


        private string m_DefaultValue;
        public string DefaultValue
        {
            get
            {
                return m_DefaultValue;
            }
            set
            {
                m_DefaultValue = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FunctionField
    {

        private string m_Script;
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
    public partial class ConstantField
    {

        private string m_TypeName;
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
    public partial class DocumentField
    {

        private string m_XPath;
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
    public partial class PropertyChangedField
    {

        private string m_Path;
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
    public partial class EmailAction
    {

        private string m_From;
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
    public partial class SetterAction
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<SetterActionChild> SetterActionChildCollection { get; } = new ObjectCollection<SetterActionChild>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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
    public partial class StartWorkflowAction
    {

        private string m_WorkflowDefinitionId;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<WorkflowTriggerMap> WorkflowTriggerMapCollection { get; } = new ObjectCollection<WorkflowTriggerMap>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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
    public partial class AssemblyAction
    {

        private bool m_IsAsyncMethod;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<MethodArg> MethodArgCollection { get; } = new ObjectCollection<MethodArg>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Activity> ActivityCollection { get; } = new ObjectCollection<Activity>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Variable> VariableDefinitionCollection { get; } = new ObjectCollection<Variable>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection { get; } = new ObjectCollection<ReferencedAssembly>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<CorrelationSet> CorrelationSetCollection { get; } = new ObjectCollection<CorrelationSet>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<CorrelationType> CorrelationTypeCollection { get; } = new ObjectCollection<CorrelationType>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TryScope> TryScopeCollection { get; } = new ObjectCollection<TryScope>();


        ///<summary>
        /// 
        ///</summary>
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<VariableValue> VariableValueCollection { get; } = new ObjectCollection<VariableValue>();


        ///<summary>
        /// 
        ///</summary>
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
    public partial class DecisionActivity
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<DecisionBranch> DecisionBranchCollection { get; } = new ObjectCollection<DecisionBranch>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DecisionBranch
    {

        private bool m_IsDefault;
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
    public partial class NotificationActivity
    {

        private string m_From;
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


        private bool m_IsMessageSuppressed;
        public bool IsMessageSuppressed
        {
            get
            {
                return m_IsMessageSuppressed;
            }
            set
            {
                m_IsMessageSuppressed = value;
                RaisePropertyChanged();
            }
        }


        public int? Retry { get; set; }

        public long? RetryInterval { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class SimpleVariable
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ComplexVariable
    {


    }

    // placeholder for FormDesign
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class VariableValue
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
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
    public partial class EndActivity
    {

        private bool m_IsTerminating;
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
    public partial class SimpleMapping
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FunctoidMapping
    {

        public Functoid Functoid { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class CreateEntityActivity
    {

        private string m_EntityType;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<PropertyMapping> PropertyMappingCollection { get; } = new ObjectCollection<PropertyMapping>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ExpressionActivity
    {

        public string Expression { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DeleteEntityActivity
    {

        private string m_EntityType;
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
    public partial class UpdateEntityActivity
    {

        private string m_Entity;
        public string Entity
        {
            get
            {
                return m_Entity;
            }
            set
            {
                m_Entity = value;
                RaisePropertyChanged();
            }
        }


        private string m_EntityIdPath;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<PropertyMapping> PropertyMappingCollection { get; } = new ObjectCollection<PropertyMapping>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ScriptFunctoid
    {

        private string m_Expression;
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
    public partial class ReceiveActivity
    {

        private string m_PortType;
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


        private string m_CancelMessageBody;
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


        private string m_CancelMessageSubject;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> InitializingCorrelationSetCollection { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> FollowingCorrelationSetCollection { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<CorrelationProperty> CorrelationPropertyCollection { get; } = new ObjectCollection<CorrelationProperty>();


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
    public partial class SendActivity
    {

        private string m_PortType;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ExceptionFilter> ExceptionFilterCollection { get; } = new ObjectCollection<ExceptionFilter>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> InitializingCorrelationSetCollection { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> FollowingCorrelationSetCollection { get; } = new ObjectCollection<string>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ListenActivity
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ListenBranch> ListenBranchCollection { get; } = new ObjectCollection<ListenBranch>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ParallelActivity
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ParallelBranch> ParallelBranchCollection { get; } = new ObjectCollection<ParallelBranch>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class JoinActivity
    {

        private string m_Placeholder;
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
    public partial class DelayActivity
    {

        private string m_Expression;
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
    public partial class ThrowActivity
    {

        private string m_Message;
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
    public partial class ParallelBranch
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Activity> ActivityCollection { get; } = new ObjectCollection<Activity>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ListenBranch
    {

        private bool m_IsWaitingAsync;
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
    public partial class ValueObjectVariable
    {

        private string m_ValueObjectDefinition;
        public string ValueObjectDefinition
        {
            get
            {
                return m_ValueObjectDefinition;
            }
            set
            {
                m_ValueObjectDefinition = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ClrTypeVariable
    {

        private string m_Assembly;
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
    public partial class ScheduledTriggerActivity
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<IntervalSchedule> IntervalScheduleCollection { get; } = new ObjectCollection<IntervalSchedule>();



    }

    // placeholder for IntervalSchedule
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Tracker
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowId;
        public const string PropertyNameWorkflowId = "WorkflowId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> ForbiddenActivities { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ExecutedActivity> ExecutedActivityCollection { get; } = new ObjectCollection<ExecutedActivity>();


        ///<summary>
        /// 
        ///</summary>
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
        private bool m_isCancelled;
        public const string PropertyNameIsCancelled = "IsCancelled";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_initiated;
        public const string PropertyNameInitiated = "Initiated";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? m_run;
        public const string PropertyNameRun = "Run";



        ///<summary>
        /// 
        ///</summary>
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

        public bool IsCancelled
        {
            set
            {
                if (m_isCancelled == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCancelled, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isCancelled = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isCancelled;
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
    public partial class MappingActivity
    {

        private string m_MappingDefinition;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<MappingSource> MappingSourceCollection { get; } = new ObjectCollection<MappingSource>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Map> MapCollection { get; } = new ObjectCollection<Map>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Functoid> FunctoidCollection { get; } = new ObjectCollection<Functoid>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<MethodArg> InputCollection { get; } = new ObjectCollection<MethodArg>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection { get; } = new ObjectCollection<ReferencedAssembly>();


        ///<summary>
        /// 
        ///</summary>
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
    public partial class DirectMap
    {

        private string m_Source;
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
    public partial class FunctoidMap
    {

        private string m___uuid;
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
    public partial class StringConcateFunctoid
    {


    }

    ///<summary>
    /// Convert string to bool
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ParseBooleanFunctoid
    {

        private string m_Format;
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
    public partial class ParseDoubleFunctoid
    {

        private string m_Styles;
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
    public partial class ParseDecimalFunctoid
    {

        private string m_Styles;
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
    public partial class ParseInt32Functoid
    {

        private string m_Styles;
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
    public partial class ParseDateTimeFunctoid
    {

        private string m_Format;
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
    public partial class FormattingFunctoid
    {

        private string m_Format;
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
    public partial class ConstantFunctoid
    {

        private string m_TypeName;
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
    public partial class SourceFunctoid
    {

        private string m_Field;
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
    public partial class CorrelationType
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<CorrelationProperty> CorrelationPropertyCollection { get; } = new ObjectCollection<CorrelationProperty>();


        ///<summary>
        /// 
        ///</summary>
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
    public partial class ChildWorkflowActivity
    {

        private string m_WorkflowDefinitionId;
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


        private bool m_IsAsync;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<PropertyMapping> PropertyMappingCollection { get; } = new ObjectCollection<PropertyMapping>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class TryScope
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<CatchScope> CatchScopeCollection { get; } = new ObjectCollection<CatchScope>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class CatchScope
    {

        private string m_ExceptionType;
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

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ReceivePortDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
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
    public partial class ReceivePort
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
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
    public partial class SendPort
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
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
    public partial class SendPortDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
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


    public partial class Field
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_note;
        public const string PropertyNameNote = "Note";


        // public properties members



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


        // public properties members



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



    public partial class PropertyMapping
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_source;
        public const string PropertyNameSource = "Source";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_destination;
        public const string PropertyNameDestination = "Destination";


        // public properties members



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

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FunctoidArg> ArgumentCollection { get; } = new ObjectCollection<FunctoidArg>();



        // public properties members



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



    public partial class Scope
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_id;
        public const string PropertyNameId = "Id";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";


        // public properties members



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

