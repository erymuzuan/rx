
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


// ReSharper disable InconsistentNaming
namespace Bespoke.Sph.Domain.Api
{

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class TableDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isSelected;
        public const string PropertyNameIsSelected = "IsSelected";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_allowRead;
        public const string PropertyNameAllowRead = "AllowRead";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_allowInsert;
        public const string PropertyNameAllowInsert = "AllowInsert";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_allowUpdate;
        public const string PropertyNameAllowUpdate = "AllowUpdate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_allowDelete;
        public const string PropertyNameAllowDelete = "AllowDelete";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_schema;
        public const string PropertyNameSchema = "Schema";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_versionColumn;
        public const string PropertyNameVersionColumn = "VersionColumn";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_modifiedDateColumn;
        public const string PropertyNameModifiedDateColumn = "ModifiedDateColumn";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Column> ColumnCollection { get; } = new ObjectCollection<Column>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TableRelation> ChildRelationCollection { get; } = new ObjectCollection<TableRelation>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TableRelation> ParentRelationCollection { get; } = new ObjectCollection<TableRelation>();


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
        public bool IsSelected
        {
            set
            {
                if (m_isSelected == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsSelected, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isSelected = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isSelected;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool AllowRead
        {
            set
            {
                if (m_allowRead == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowRead, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_allowRead = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_allowRead;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool AllowInsert
        {
            set
            {
                if (m_allowInsert == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowInsert, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_allowInsert = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_allowInsert;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool AllowUpdate
        {
            set
            {
                if (m_allowUpdate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowUpdate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_allowUpdate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_allowUpdate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool AllowDelete
        {
            set
            {
                if (m_allowDelete == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowDelete, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_allowDelete = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_allowDelete;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Schema
        {
            set
            {
                if (String.Equals(m_schema, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSchema, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_schema = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_schema;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string VersionColumn
        {
            set
            {
                if (String.Equals(m_versionColumn, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVersionColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_versionColumn = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_versionColumn;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string ModifiedDateColumn
        {
            set
            {
                if (String.Equals(m_modifiedDateColumn, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameModifiedDateColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_modifiedDateColumn = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_modifiedDateColumn;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Column
    {

        private bool m_IsPrimaryKey;
        public bool IsPrimaryKey
        {
            get
            {
                return m_IsPrimaryKey;
            }
            set
            {
                m_IsPrimaryKey = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsVersion;
        public bool IsVersion
        {
            get
            {
                return m_IsVersion;
            }
            set
            {
                m_IsVersion = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsModifiedDate;
        public bool IsModifiedDate
        {
            get
            {
                return m_IsModifiedDate;
            }
            set
            {
                m_IsModifiedDate = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsComputed;
        public bool IsComputed
        {
            get
            {
                return m_IsComputed;
            }
            set
            {
                m_IsComputed = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsIdentity;
        public bool IsIdentity
        {
            get
            {
                return m_IsIdentity;
            }
            set
            {
                m_IsIdentity = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsComplex;
        public bool IsComplex
        {
            get
            {
                return m_IsComplex;
            }
            set
            {
                m_IsComplex = value;
                RaisePropertyChanged();
            }
        }


        private string m_MimeType;
        public string MimeType
        {
            get
            {
                return m_MimeType;
            }
            set
            {
                m_MimeType = value;
                RaisePropertyChanged();
            }
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LookupColumnTable m_lookupColumnTable
                = new LookupColumnTable();

        public const string PropertyNameLookupColumnTable = "LookupColumnTable";
        [DebuggerHidden]

        public LookupColumnTable LookupColumnTable
        {
            get { return m_lookupColumnTable; }
            set
            {
                m_lookupColumnTable = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ColumnMetadata
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class OperationDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_methodName;
        public const string PropertyNameMethodName = "MethodName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isOneWay;
        public const string PropertyNameIsOneWay = "IsOneWay";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isSelected;
        public const string PropertyNameIsSelected = "IsSelected";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_schema;
        public const string PropertyNameSchema = "Schema";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ParameterDefinition m_parameterDefinition
                = new ParameterDefinition();

        public const string PropertyNameParameterDefinition = "ParameterDefinition";
        [DebuggerHidden]

        public ParameterDefinition ParameterDefinition
        {
            get { return m_parameterDefinition; }
            set
            {
                m_parameterDefinition = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Member> RequestMemberCollection { get; } = new ObjectCollection<Member>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Member> ResponseMemberCollection { get; } = new ObjectCollection<Member>();


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ErrorRetry m_errorRetry
                = new ErrorRetry();

        public const string PropertyNameErrorRetry = "ErrorRetry";
        [DebuggerHidden]

        public ErrorRetry ErrorRetry
        {
            get { return m_errorRetry; }
            set
            {
                m_errorRetry = value;
                OnPropertyChanged();
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
        public string MethodName
        {
            set
            {
                if (String.Equals(m_methodName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMethodName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_methodName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_methodName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsOneWay
        {
            set
            {
                if (m_isOneWay == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsOneWay, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isOneWay = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isOneWay;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsSelected
        {
            set
            {
                if (m_isSelected == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsSelected, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isSelected = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isSelected;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Schema
        {
            set
            {
                if (String.Equals(m_schema, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSchema, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_schema = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_schema;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ErrorRetry
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_attempt;
        public const string PropertyNameAttempt = "Attempt";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isEnabled;
        public const string PropertyNameIsEnabled = "IsEnabled";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_wait;
        public const string PropertyNameWait = "Wait";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WaitAlgorithm m_algorithm;
        public const string PropertyNameAlgorithm = "Algorithm";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public int Attempt
        {
            set
            {
                if (m_attempt == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAttempt, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_attempt = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_attempt;
            }
        }


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
        public int Wait
        {
            set
            {
                if (m_wait == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWait, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_wait = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_wait;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public WaitAlgorithm Algorithm
        {
            set
            {
                if (m_algorithm == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAlgorithm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_algorithm = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_algorithm;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ParameterDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequest;
        public const string PropertyNameIsRequest = "IsRequest";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isResponse;
        public const string PropertyNameIsResponse = "IsResponse";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_codeNamespace;
        public const string PropertyNameCodeNamespace = "CodeNamespace";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();


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
        public bool IsRequest
        {
            set
            {
                if (m_isRequest == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequest, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequest = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequest;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsResponse
        {
            set
            {
                if (m_isResponse == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsResponse, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isResponse = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isResponse;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string CodeNamespace
        {
            set
            {
                if (String.Equals(m_codeNamespace, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCodeNamespace, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_codeNamespace = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_codeNamespace;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class TableRelation
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_table;
        public const string PropertyNameTable = "Table";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_constraint;
        public const string PropertyNameConstraint = "Constraint";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_column;
        public const string PropertyNameColumn = "Column";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_foreignColumn;
        public const string PropertyNameForeignColumn = "ForeignColumn";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isSelected;
        public const string PropertyNameIsSelected = "IsSelected";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Table
        {
            set
            {
                if (String.Equals(m_table, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_table = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_table;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Constraint
        {
            set
            {
                if (String.Equals(m_constraint, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameConstraint, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_constraint = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_constraint;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Column
        {
            set
            {
                if (String.Equals(m_column, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_column = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_column;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ForeignColumn
        {
            set
            {
                if (String.Equals(m_foreignColumn, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameForeignColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_foreignColumn = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_foreignColumn;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsSelected
        {
            set
            {
                if (m_isSelected == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsSelected, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isSelected = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isSelected;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class Adapter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TableDefinition> TableDefinitionCollection { get; } = new ObjectCollection<TableDefinition>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<OperationDefinition> OperationDefinitionCollection { get; } = new ObjectCollection<OperationDefinition>();


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



    }


    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class LookupColumnTable
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isEnabled;
        public const string PropertyNameIsEnabled = "IsEnabled";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_table;
        public const string PropertyNameTable = "Table";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_column;
        public const string PropertyNameColumn = "Column";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_dbType;
        public const string PropertyNameDbType = "DbType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_valueColumn;
        public const string PropertyNameValueColumn = "ValueColumn";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_keyColumn;
        public const string PropertyNameKeyColumn = "KeyColumn";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


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
        public string Table
        {
            set
            {
                if (String.Equals(m_table, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_table = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_table;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Column
        {
            set
            {
                if (String.Equals(m_column, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_column = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_column;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string DbType
        {
            set
            {
                if (String.Equals(m_dbType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDbType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dbType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dbType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ValueColumn
        {
            set
            {
                if (String.Equals(m_valueColumn, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValueColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_valueColumn = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_valueColumn;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string KeyColumn
        {
            set
            {
                if (String.Equals(m_keyColumn, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameKeyColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_keyColumn = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_keyColumn;
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



    }

    // placeholder for Member complext type

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WaitAlgorithm
    {
        Constant,
        Linear,
        Exponential,

    }

}
// ReSharper restore InconsistentNaming

