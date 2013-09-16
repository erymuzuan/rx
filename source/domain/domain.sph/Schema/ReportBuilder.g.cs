
using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;


// ReSharper disable InconsistentNaming
namespace Bespoke.Sph.Domain
{

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReportDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_reportDefinitionId;
        public const string PropertyNameReportDefinitionId = "ReportDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_category;
        public const string PropertyNameCategory = "Category";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPrivate;
        public const string PropertyNameIsPrivate = "IsPrivate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isExportAllowed;
        public const string PropertyNameIsExportAllowed = "IsExportAllowed";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        private readonly ObjectCollection<ReportLayout> m_ReportLayoutCollection = new ObjectCollection<ReportLayout>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReportLayout", IsNullable = false)]
        public ObjectCollection<ReportLayout> ReportLayoutCollection
        {
            get { return m_ReportLayoutCollection; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DataSource m_dataSource
                = new DataSource();

        public const string PropertyNameDataSource = "DataSource";
        [DebuggerHidden]

        public DataSource DataSource
        {
            get { return m_dataSource; }
            set
            {
                m_dataSource = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ReportDefinitionId
        {
            set
            {
                if (m_reportDefinitionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReportDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reportDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reportDefinitionId;
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

        public string Category
        {
            set
            {
                if (String.Equals(m_category, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCategory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_category = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_category;
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

        public bool IsPrivate
        {
            set
            {
                if (m_isPrivate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPrivate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPrivate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPrivate;
            }
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
                if (m_isExportAllowed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsExportAllowed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isExportAllowed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isExportAllowed;
            }
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
    [XmlType("ReportLayout", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportLayout
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_row;
        public const string PropertyNameRow = "Row";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_column;
        public const string PropertyNameColumn = "Column";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_columnSpan;
        public const string PropertyNameColumnSpan = "ColumnSpan";


        private readonly ObjectCollection<ReportItem> m_ReportItemCollection = new ObjectCollection<ReportItem>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<ReportItem> ReportItemCollection
        {
            get { return m_ReportItemCollection; }
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

        public int Row
        {
            set
            {
                if (m_row == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRow, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_row = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_row;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int Column
        {
            set
            {
                if (m_column == value) return;
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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ColumnSpan
        {
            set
            {
                if (m_columnSpan == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameColumnSpan, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_columnSpan = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_columnSpan;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("BarChartItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class BarChartItem
    {

        private string m_ValueLabelFormat;
        [XmlAttribute]
        public string ValueLabelFormat
        {
            get
            {
                return m_ValueLabelFormat;
            }
            set
            {
                m_ValueLabelFormat = value;
                RaisePropertyChanged();
            }
        }


        private string m_HorizontalAxisField;
        [XmlAttribute]
        public string HorizontalAxisField
        {
            get
            {
                return m_HorizontalAxisField;
            }
            set
            {
                m_HorizontalAxisField = value;
                RaisePropertyChanged();
            }
        }


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


        private readonly ObjectCollection<ChartSeries> m_ChartSeriesCollection = new ObjectCollection<ChartSeries>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ChartSeries", IsNullable = false)]
        public ObjectCollection<ChartSeries> ChartSeriesCollection
        {
            get { return m_ChartSeriesCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("LineChartItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class LineChartItem
    {

        private string m_ValueLabelFormat;
        [XmlAttribute]
        public string ValueLabelFormat
        {
            get
            {
                return m_ValueLabelFormat;
            }
            set
            {
                m_ValueLabelFormat = value;
                RaisePropertyChanged();
            }
        }


        private string m_HorizontalAxisField;
        [XmlAttribute]
        public string HorizontalAxisField
        {
            get
            {
                return m_HorizontalAxisField;
            }
            set
            {
                m_HorizontalAxisField = value;
                RaisePropertyChanged();
            }
        }


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


        private readonly ObjectCollection<ChartSeries> m_ChartSeriesCollection = new ObjectCollection<ChartSeries>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ChartSeries", IsNullable = false)]
        public ObjectCollection<ChartSeries> ChartSeriesCollection
        {
            get { return m_ChartSeriesCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("PieChartItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class PieChartItem
    {

        private string m_CategoryField;
        [XmlAttribute]
        public string CategoryField
        {
            get
            {
                return m_CategoryField;
            }
            set
            {
                m_CategoryField = value;
                RaisePropertyChanged();
            }
        }


        private string m_ValueField;
        [XmlAttribute]
        public string ValueField
        {
            get
            {
                return m_ValueField;
            }
            set
            {
                m_ValueField = value;
                RaisePropertyChanged();
            }
        }


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


        private string m_TitlePlacement;
        [XmlAttribute]
        public string TitlePlacement
        {
            get
            {
                return m_TitlePlacement;
            }
            set
            {
                m_TitlePlacement = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DataGridItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DataGridItem
    {

        private readonly ObjectCollection<ReportRow> m_ReportRowCollection = new ObjectCollection<ReportRow>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReportRow", IsNullable = false)]
        public ObjectCollection<ReportRow> ReportRowCollection
        {
            get { return m_ReportRowCollection; }
        }

        private readonly ObjectCollection<DataGridColumn> m_DataGridColumnCollection = new ObjectCollection<DataGridColumn>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DataGridColumn", IsNullable = false)]
        public ObjectCollection<DataGridColumn> DataGridColumnCollection
        {
            get { return m_DataGridColumnCollection; }
        }

        private readonly ObjectCollection<DataGridGroupDefinition> m_DataGridGroupDefinitionCollection = new ObjectCollection<DataGridGroupDefinition>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DataGridGroupDefinition", IsNullable = false)]
        public ObjectCollection<DataGridGroupDefinition> DataGridGroupDefinitionCollection
        {
            get { return m_DataGridGroupDefinitionCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("LabelItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class LabelItem
    {

        public string Html { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("LineItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class LineItem
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DataSource", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DataSource
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityName;
        public const string PropertyNameEntityName = "EntityName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_query;
        public const string PropertyNameQuery = "Query";


        private readonly ObjectCollection<Parameter> m_ParameterCollection = new ObjectCollection<Parameter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Parameter", IsNullable = false)]
        public ObjectCollection<Parameter> ParameterCollection
        {
            get { return m_ParameterCollection; }
        }

        private readonly ObjectCollection<ReportFilter> m_ReportFilterCollection = new ObjectCollection<ReportFilter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReportFilter", IsNullable = false)]
        public ObjectCollection<ReportFilter> ReportFilterCollection
        {
            get { return m_ReportFilterCollection; }
        }

        private readonly ObjectCollection<EntityField> m_EntityFieldCollection = new ObjectCollection<EntityField>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("EntityField", IsNullable = false)]
        public ObjectCollection<EntityField> EntityFieldCollection
        {
            get { return m_EntityFieldCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string EntityName
        {
            set
            {
                if (String.Equals(m_entityName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Query
        {
            set
            {
                if (String.Equals(m_query, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQuery, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_query = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_query;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Parameter", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Parameter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_availableValues;
        public const string PropertyNameAvailableValues = "AvailableValues";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_label;
        public const string PropertyNameLabel = "Label";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isNullable;
        public const string PropertyNameIsNullable = "IsNullable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object m_value;
        public const string PropertyNameValue = "Value";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object m_defaultValue;
        public const string PropertyNameDefaultValue = "DefaultValue";



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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string AvailableValues
        {
            set
            {
                if (String.Equals(m_availableValues, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAvailableValues, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_availableValues = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_availableValues;
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

        [Required]

        [DebuggerHidden]

        public bool IsNullable
        {
            set
            {
                if (m_isNullable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsNullable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isNullable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isNullable;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public object Value
        {
            set
            {
                if (m_value == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_value = value;
                    OnPropertyChanged();
                }
            }
            get { return m_value; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public object DefaultValue
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
            get { return m_defaultValue; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReportFilter", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportFilter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fieldName;
        public const string PropertyNameFieldName = "FieldName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operator;
        public const string PropertyNameOperator = "Operator";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_value;
        public const string PropertyNameValue = "Value";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FieldName
        {
            set
            {
                if (String.Equals(m_fieldName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFieldName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_fieldName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_fieldName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Operator
        {
            set
            {
                if (String.Equals(m_operator, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperator, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operator = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operator;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityField", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityField
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isNullable;
        public const string PropertyNameIsNullable = "IsNullable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_aggregate;
        public const string PropertyNameAggregate = "Aggregate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_order;
        public const string PropertyNameOrder = "Order";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_orderPosition;
        public const string PropertyNameOrderPosition = "OrderPosition";


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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsNullable
        {
            set
            {
                if (m_isNullable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsNullable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isNullable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isNullable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Aggregate
        {
            set
            {
                if (String.Equals(m_aggregate, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAggregate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_aggregate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_aggregate;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Order
        {
            set
            {
                if (String.Equals(m_order, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrder, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_order = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_order;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int OrderPosition
        {
            set
            {
                if (m_orderPosition == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOrderPosition, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_orderPosition = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_orderPosition;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DataGridColumn", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DataGridColumn
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_header;
        public const string PropertyNameHeader = "Header";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_width;
        public const string PropertyNameWidth = "Width";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_expression;
        public const string PropertyNameExpression = "Expression";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_format;
        public const string PropertyNameFormat = "Format";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_action;
        public const string PropertyNameAction = "Action";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_footerExpression;
        public const string PropertyNameFooterExpression = "FooterExpression";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Header
        {
            set
            {
                if (String.Equals(m_header, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHeader, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_header = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_header;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Width
        {
            set
            {
                if (String.Equals(m_width, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWidth, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_width = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_width;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Expression
        {
            set
            {
                if (String.Equals(m_expression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_expression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_expression;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Format
        {
            set
            {
                if (String.Equals(m_format, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFormat, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_format = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_format;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Action
        {
            set
            {
                if (String.Equals(m_action, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAction, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_action = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_action;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FooterExpression
        {
            set
            {
                if (String.Equals(m_footerExpression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFooterExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_footerExpression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_footerExpression;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReportColumn", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportColumn
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_header;
        public const string PropertyNameHeader = "Header";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_width;
        public const string PropertyNameWidth = "Width";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isSelected;
        public const string PropertyNameIsSelected = "IsSelected";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isFilterable;
        public const string PropertyNameIsFilterable = "IsFilterable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isCustomField;
        public const string PropertyNameIsCustomField = "IsCustomField";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isNullable;
        public const string PropertyNameIsNullable = "IsNullable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object m_value;
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

        public string Header
        {
            set
            {
                if (String.Equals(m_header, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHeader, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_header = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_header;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Width
        {
            set
            {
                if (String.Equals(m_width, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameWidth, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_width = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_width;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsFilterable
        {
            set
            {
                if (m_isFilterable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsFilterable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isFilterable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isFilterable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsCustomField
        {
            set
            {
                if (m_isCustomField == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCustomField, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isCustomField = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isCustomField;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public bool IsNullable
        {
            set
            {
                if (m_isNullable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsNullable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isNullable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isNullable;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public object Value
        {
            set
            {
                if (m_value == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_value = value;
                    OnPropertyChanged();
                }
            }
            get { return m_value; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReportRow", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportRow
    {

        private readonly ObjectCollection<ReportColumn> m_ReportColumnCollection = new ObjectCollection<ReportColumn>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReportColumn", IsNullable = false)]
        public ObjectCollection<ReportColumn> ReportColumnCollection
        {
            get { return m_ReportColumnCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DailySchedule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DailySchedule
    {

        private int m_Hour;
        [XmlAttribute]
        public int Hour
        {
            get
            {
                return m_Hour;
            }
            set
            {
                m_Hour = value;
                RaisePropertyChanged();
            }
        }


        private int m_Minute;
        [XmlAttribute]
        public int Minute
        {
            get
            {
                return m_Minute;
            }
            set
            {
                m_Minute = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsSunday;
        [XmlAttribute]
        public bool IsSunday
        {
            get
            {
                return m_IsSunday;
            }
            set
            {
                m_IsSunday = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsMonday;
        [XmlAttribute]
        public bool IsMonday
        {
            get
            {
                return m_IsMonday;
            }
            set
            {
                m_IsMonday = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsTuesday;
        [XmlAttribute]
        public bool IsTuesday
        {
            get
            {
                return m_IsTuesday;
            }
            set
            {
                m_IsTuesday = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsWednesday;
        [XmlAttribute]
        public bool IsWednesday
        {
            get
            {
                return m_IsWednesday;
            }
            set
            {
                m_IsWednesday = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsThursday;
        [XmlAttribute]
        public bool IsThursday
        {
            get
            {
                return m_IsThursday;
            }
            set
            {
                m_IsThursday = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsFriday;
        [XmlAttribute]
        public bool IsFriday
        {
            get
            {
                return m_IsFriday;
            }
            set
            {
                m_IsFriday = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsSaturday;
        [XmlAttribute]
        public bool IsSaturday
        {
            get
            {
                return m_IsSaturday;
            }
            set
            {
                m_IsSaturday = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("HourlySchedule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class HourlySchedule
    {

        private int m_StartHour;
        [XmlAttribute]
        public int StartHour
        {
            get
            {
                return m_StartHour;
            }
            set
            {
                m_StartHour = value;
                RaisePropertyChanged();
            }
        }


        private int m_Interval;
        [XmlAttribute]
        public int Interval
        {
            get
            {
                return m_Interval;
            }
            set
            {
                m_Interval = value;
                RaisePropertyChanged();
            }
        }


        private int m_Minute;
        [XmlAttribute]
        public int Minute
        {
            get
            {
                return m_Minute;
            }
            set
            {
                m_Minute = value;
                RaisePropertyChanged();
            }
        }


        private int m_EndHour;
        [XmlAttribute]
        public int EndHour
        {
            get
            {
                return m_EndHour;
            }
            set
            {
                m_EndHour = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("WeeklySchedule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WeeklySchedule
    {

        private string m_Day;
        [XmlAttribute]
        public string Day
        {
            get
            {
                return m_Day;
            }
            set
            {
                m_Day = value;
                RaisePropertyChanged();
            }
        }


        private int m_Hour;
        [XmlAttribute]
        public int Hour
        {
            get
            {
                return m_Hour;
            }
            set
            {
                m_Hour = value;
                RaisePropertyChanged();
            }
        }


        private int m_Minute;
        [XmlAttribute]
        public int Minute
        {
            get
            {
                return m_Minute;
            }
            set
            {
                m_Minute = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("MonthlySchedule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class MonthlySchedule
    {

        private int m_Day;
        [XmlAttribute]
        public int Day
        {
            get
            {
                return m_Day;
            }
            set
            {
                m_Day = value;
                RaisePropertyChanged();
            }
        }


        private int m_Hour;
        [XmlAttribute]
        public int Hour
        {
            get
            {
                return m_Hour;
            }
            set
            {
                m_Hour = value;
                RaisePropertyChanged();
            }
        }


        private int m_Minute;
        [XmlAttribute]
        public int Minute
        {
            get
            {
                return m_Minute;
            }
            set
            {
                m_Minute = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReportDelivery", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportDelivery
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_reportDeliveryId;
        public const string PropertyNameReportDeliveryId = "ReportDeliveryId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_reportDefinitionId;
        public const string PropertyNameReportDefinitionId = "ReportDefinitionId";


        private readonly ObjectCollection<IntervalSchedule> m_IntervalScheduleCollection = new ObjectCollection<IntervalSchedule>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<IntervalSchedule> IntervalScheduleCollection
        {
            get { return m_IntervalScheduleCollection; }
        }

        private readonly ObjectCollection<string> m_Users = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> Users
        {
            get { return m_Users; }
        }

        private readonly ObjectCollection<string> m_Departments = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> Departments
        {
            get { return m_Departments; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ReportDeliveryId
        {
            set
            {
                if (m_reportDeliveryId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReportDeliveryId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reportDeliveryId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reportDeliveryId;
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

        [Required]

        [DebuggerHidden]

        public int ReportDefinitionId
        {
            set
            {
                if (m_reportDefinitionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReportDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reportDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reportDefinitionId;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ReportContent", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportContent
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_reportContentId;
        public const string PropertyNameReportContentId = "ReportContentId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_reportDefinitionId;
        public const string PropertyNameReportDefinitionId = "ReportDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_reportDeliveryId;
        public const string PropertyNameReportDeliveryId = "ReportDeliveryId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_htmlOutput;
        public const string PropertyNameHtmlOutput = "HtmlOutput";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ReportContentId
        {
            set
            {
                if (m_reportContentId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReportContentId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reportContentId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reportContentId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ReportDefinitionId
        {
            set
            {
                if (m_reportDefinitionId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReportDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reportDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reportDefinitionId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int ReportDeliveryId
        {
            set
            {
                if (m_reportDeliveryId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReportDeliveryId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_reportDeliveryId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_reportDeliveryId;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string HtmlOutput
        {
            set
            {
                if (String.Equals(m_htmlOutput, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHtmlOutput, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_htmlOutput = value;
                    OnPropertyChanged();
                }
            }
            get { return m_htmlOutput; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ChartSeries", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ChartSeries
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_header;
        public const string PropertyNameHeader = "Header";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_column;
        public const string PropertyNameColumn = "Column";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Header
        {
            set
            {
                if (String.Equals(m_header, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHeader, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_header = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_header;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DataGridGroupDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DataGridGroupDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_column;
        public const string PropertyNameColumn = "Column";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_expression;
        public const string PropertyNameExpression = "Expression";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_style;
        public const string PropertyNameStyle = "Style";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_footerExpression;
        public const string PropertyNameFooterExpression = "FooterExpression";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Expression
        {
            set
            {
                if (String.Equals(m_expression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_expression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_expression;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Style
        {
            set
            {
                if (String.Equals(m_style, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStyle, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_style = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_style;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string FooterExpression
        {
            set
            {
                if (String.Equals(m_footerExpression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFooterExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_footerExpression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_footerExpression;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DataGridGroup", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DataGridGroup
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_column;
        public const string PropertyNameColumn = "Column";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_text;
        public const string PropertyNameText = "Text";


        private readonly ObjectCollection<ReportRow> m_ReportRowCollection = new ObjectCollection<ReportRow>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ReportRow", IsNullable = false)]
        public ObjectCollection<ReportRow> ReportRowCollection
        {
            get { return m_ReportRowCollection; }
        }

        private readonly ObjectCollection<DataGridGroup> m_DataGridGroupCollection = new ObjectCollection<DataGridGroup>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("DataGridGroup", IsNullable = false)]
        public ObjectCollection<DataGridGroup> DataGridGroupCollection
        {
            get { return m_DataGridGroupCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public string Text
        {
            set
            {
                if (String.Equals(m_text, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_text = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_text;
            }
        }



    }

    [XmlType("ReportItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ReportItem
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_cssClass;
        public const string PropertyNameCssClass = "CssClass";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_visible;
        public const string PropertyNameVisible = "Visible";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_tooltip;
        public const string PropertyNameTooltip = "Tooltip";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_icon;
        public const string PropertyNameIcon = "Icon";


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
        public string CssClass
        {
            set
            {
                if (m_cssClass == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCssClass, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cssClass = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_cssClass;
            }
        }



        [XmlAttribute]
        public string Visible
        {
            set
            {
                if (m_visible == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVisible, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_visible = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_visible;
            }
        }



        [XmlAttribute]
        public string Tooltip
        {
            set
            {
                if (m_tooltip == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTooltip, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tooltip = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tooltip;
            }
        }



        [XmlAttribute]
        public string Icon
        {
            set
            {
                if (m_icon == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIcon, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_icon = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_icon;
            }
        }



    }


    [XmlType("IntervalSchedule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class IntervalSchedule
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";


        // public properties members



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



    }


}
// ReSharper restore InconsistentNaming

