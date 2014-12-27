
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
    [XmlType("FormDesign", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FormDesign
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_confirmationText;
        public const string PropertyNameConfirmationText = "ConfirmationText";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_imageStoreId;
        public const string PropertyNameImageStoreId = "ImageStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_labelColLg;
        public const string PropertyNameLabelColLg = "LabelColLg";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_labelColMd;
        public const string PropertyNameLabelColMd = "LabelColMd";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_labelColSm;
        public const string PropertyNameLabelColSm = "LabelColSm";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_labelColXs;
        public const string PropertyNameLabelColXs = "LabelColXs";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_inputColLg;
        public const string PropertyNameInputColLg = "InputColLg";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_inputColMd;
        public const string PropertyNameInputColMd = "InputColMd";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_inputColSm;
        public const string PropertyNameInputColSm = "InputColSm";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_inputColXs;
        public const string PropertyNameInputColXs = "InputColXs";



        private readonly ObjectCollection<FormElement> m_FormElementCollection = new ObjectCollection<FormElement>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<FormElement> FormElementCollection
        {
            get { return m_FormElementCollection; }
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
        public string ConfirmationText
        {
            set
            {
                if (String.Equals(m_confirmationText, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameConfirmationText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_confirmationText = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_confirmationText;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string ImageStoreId
        {
            set
            {
                if (String.Equals(m_imageStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameImageStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_imageStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_imageStoreId;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? LabelColLg
        {
            set
            {
                if (m_labelColLg == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColLg, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColLg = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColLg; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? LabelColMd
        {
            set
            {
                if (m_labelColMd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColMd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColMd = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColMd; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? LabelColSm
        {
            set
            {
                if (m_labelColSm == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColSm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColSm = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColSm; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? LabelColXs
        {
            set
            {
                if (m_labelColXs == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColXs, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColXs = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColXs; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? InputColLg
        {
            set
            {
                if (m_inputColLg == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColLg, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColLg = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColLg; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? InputColMd
        {
            set
            {
                if (m_inputColMd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColMd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColMd = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColMd; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? InputColSm
        {
            set
            {
                if (m_inputColSm == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColSm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColSm = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColSm; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? InputColXs
        {
            set
            {
                if (m_inputColXs == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColXs, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColXs = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColXs; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("TextBox", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class TextBox
    {

        private string m_DefaultValue;
        [XmlAttribute]
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


        private string m_AutoCompletionEntity;
        [XmlAttribute]
        public string AutoCompletionEntity
        {
            get
            {
                return m_AutoCompletionEntity;
            }
            set
            {
                m_AutoCompletionEntity = value;
                RaisePropertyChanged();
            }
        }


        private string m_AutoCompletionField;
        [XmlAttribute]
        public string AutoCompletionField
        {
            get
            {
                return m_AutoCompletionField;
            }
            set
            {
                m_AutoCompletionField = value;
                RaisePropertyChanged();
            }
        }


        private string m_AutoCompletionQuery;
        [XmlAttribute]
        public string AutoCompletionQuery
        {
            get
            {
                return m_AutoCompletionQuery;
            }
            set
            {
                m_AutoCompletionQuery = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CheckBox", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CheckBox
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DatePicker", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DatePicker
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DateTimePicker", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DateTimePicker
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ComboBox", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComboBox
    {

        private readonly ObjectCollection<ComboBoxItem> m_ComboBoxItemCollection = new ObjectCollection<ComboBoxItem>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ComboBoxItem", IsNullable = false)]
        public ObjectCollection<ComboBoxItem> ComboBoxItemCollection
        {
            get { return m_ComboBoxItemCollection; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ComboBoxLookup m_comboBoxLookup
                = new ComboBoxLookup();

        public const string PropertyNameComboBoxLookup = "ComboBoxLookup";
        [DebuggerHidden]

        public ComboBoxLookup ComboBoxLookup
        {
            get { return m_comboBoxLookup; }
            set
            {
                m_comboBoxLookup = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("TextAreaElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class TextAreaElement
    {

        private string m_Rows;
        [XmlAttribute]
        public string Rows
        {
            get
            {
                return m_Rows;
            }
            set
            {
                m_Rows = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsHtml;
        [XmlAttribute]
        public bool IsHtml
        {
            get
            {
                return m_IsHtml;
            }
            set
            {
                m_IsHtml = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("WebsiteFormElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class WebsiteFormElement
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EmailFormElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EmailFormElement
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("NumberTextBox", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class NumberTextBox
    {

        private int m_Step;
        [XmlAttribute]
        public int Step
        {
            get
            {
                return m_Step;
            }
            set
            {
                m_Step = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("MapElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class MapElement
    {

        private string m_Icon;
        [XmlAttribute]
        public string Icon
        {
            get
            {
                return m_Icon;
            }
            set
            {
                m_Icon = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SectionFormElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SectionFormElement
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ComboBoxItem", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComboBoxItem
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_caption;
        public const string PropertyNameCaption = "Caption";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_value;
        public const string PropertyNameValue = "Value";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Caption
        {
            set
            {
                if (String.Equals(m_caption, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCaption, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_caption = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_caption;
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
    [XmlType("AddressElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class AddressElement
    {

        private bool m_IsUnitNoVisible;
        [XmlAttribute]
        public bool IsUnitNoVisible
        {
            get
            {
                return m_IsUnitNoVisible;
            }
            set
            {
                m_IsUnitNoVisible = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsFloorVisible;
        [XmlAttribute]
        public bool IsFloorVisible
        {
            get
            {
                return m_IsFloorVisible;
            }
            set
            {
                m_IsFloorVisible = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsBlockVisible;
        [XmlAttribute]
        public bool IsBlockVisible
        {
            get
            {
                return m_IsBlockVisible;
            }
            set
            {
                m_IsBlockVisible = value;
                RaisePropertyChanged();
            }
        }


        private string m_BlockOptionsPath;
        [XmlAttribute]
        public string BlockOptionsPath
        {
            get
            {
                return m_BlockOptionsPath;
            }
            set
            {
                m_BlockOptionsPath = value;
                RaisePropertyChanged();
            }
        }


        private string m_FloorOptionsPath;
        [XmlAttribute]
        public string FloorOptionsPath
        {
            get
            {
                return m_FloorOptionsPath;
            }
            set
            {
                m_FloorOptionsPath = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("HtmlElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class HtmlElement
    {

        public string Text { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DefaultValue", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DefaultValue
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_propertyName;
        public const string PropertyNamePropertyName = "PropertyName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


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
        [DebuggerHidden]

        [Required]
        public string PropertyName
        {
            set
            {
                if (String.Equals(m_propertyName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePropertyName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_propertyName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_propertyName;
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
    [XmlType("FieldValidation", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FieldValidation
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_pattern;
        public const string PropertyNamePattern = "Pattern";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mode;
        public const string PropertyNameMode = "Mode";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_message;
        public const string PropertyNameMessage = "Message";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float? m_min;
        public const string PropertyNameMin = "Min";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float? m_max;
        public const string PropertyNameMax = "Max";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_minLength;
        public const string PropertyNameMinLength = "MinLength";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_maxLength;
        public const string PropertyNameMaxLength = "MaxLength";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsRequired
        {
            set
            {
                if (m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequired = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequired;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Pattern
        {
            set
            {
                if (String.Equals(m_pattern, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePattern, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_pattern = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_pattern;
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
        public string Message
        {
            set
            {
                if (String.Equals(m_message, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMessage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_message = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_message;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public float? Min
        {
            set
            {
                if (m_min == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMin, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_min = value;
                    OnPropertyChanged();
                }
            }
            get { return m_min; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public float? Max
        {
            set
            {
                if (m_max == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMax, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_max = value;
                    OnPropertyChanged();
                }
            }
            get { return m_max; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? MinLength
        {
            set
            {
                if (m_minLength == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMinLength, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_minLength = value;
                    OnPropertyChanged();
                }
            }
            get { return m_minLength; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? MaxLength
        {
            set
            {
                if (m_maxLength == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMaxLength, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_maxLength = value;
                    OnPropertyChanged();
                }
            }
            get { return m_maxLength; }
        }


    }

    // placeholder for Performer
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("BusinessRule", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class BusinessRule
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_errorLocation;
        public const string PropertyNameErrorLocation = "ErrorLocation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_errorMessage;
        public const string PropertyNameErrorMessage = "ErrorMessage";


        private readonly ObjectCollection<Rule> m_RuleCollection = new ObjectCollection<Rule>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Rule", IsNullable = false)]
        public ObjectCollection<Rule> RuleCollection
        {
            get { return m_RuleCollection; }
        }

        private readonly ObjectCollection<Rule> m_FilterCollection = new ObjectCollection<Rule>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Rule", IsNullable = false)]
        public ObjectCollection<Rule> FilterCollection
        {
            get { return m_FilterCollection; }
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
        public string ErrorLocation
        {
            set
            {
                if (String.Equals(m_errorLocation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameErrorLocation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_errorLocation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_errorLocation;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string ErrorMessage
        {
            set
            {
                if (String.Equals(m_errorMessage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameErrorMessage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_errorMessage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_errorMessage;
            }
        }



    }

    // placeholder for Rule
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FileUploadElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FileUploadElement
    {

        private string m_AllowedExtensions;
        [XmlAttribute]
        public string AllowedExtensions
        {
            get
            {
                return m_AllowedExtensions;
            }
            set
            {
                m_AllowedExtensions = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ComboBoxLookup", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ComboBoxLookup
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_valuePath;
        public const string PropertyNameValuePath = "ValuePath";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_displayPath;
        public const string PropertyNameDisplayPath = "DisplayPath";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_query;
        public const string PropertyNameQuery = "Query";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isComputedQuery;
        public const string PropertyNameIsComputedQuery = "IsComputedQuery";


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
        public string ValuePath
        {
            set
            {
                if (String.Equals(m_valuePath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValuePath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_valuePath = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_valuePath;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string DisplayPath
        {
            set
            {
                if (String.Equals(m_displayPath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDisplayPath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_displayPath = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_displayPath;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsComputedQuery
        {
            set
            {
                if (m_isComputedQuery == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsComputedQuery, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isComputedQuery = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isComputedQuery;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ChildEntityListView", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ChildEntityListView
    {

        private string m_Entity;
        [XmlAttribute]
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


        private string m_Query;
        [XmlAttribute]
        public string Query
        {
            get
            {
                return m_Query;
            }
            set
            {
                m_Query = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsAllowAddItem;
        [XmlAttribute]
        public bool IsAllowAddItem
        {
            get
            {
                return m_IsAllowAddItem;
            }
            set
            {
                m_IsAllowAddItem = value;
                RaisePropertyChanged();
            }
        }


        private string m_NewItemFormRoute;
        [XmlAttribute]
        public string NewItemFormRoute
        {
            get
            {
                return m_NewItemFormRoute;
            }
            set
            {
                m_NewItemFormRoute = value;
                RaisePropertyChanged();
            }
        }


        private string m_NewItemMappingSource;
        [XmlAttribute]
        public string NewItemMappingSource
        {
            get
            {
                return m_NewItemMappingSource;
            }
            set
            {
                m_NewItemMappingSource = value;
                RaisePropertyChanged();
            }
        }


        private string m_NewItemMappingDestination;
        [XmlAttribute]
        public string NewItemMappingDestination
        {
            get
            {
                return m_NewItemMappingDestination;
            }
            set
            {
                m_NewItemMappingDestination = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<ViewColumn> m_ViewColumnCollection = new ObjectCollection<ViewColumn>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ViewColumn", IsNullable = false)]
        public ObjectCollection<ViewColumn> ViewColumnCollection
        {
            get { return m_ViewColumnCollection; }
        }

        private readonly ObjectCollection<Sort> m_SortCollection = new ObjectCollection<Sort>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Sort", IsNullable = false)]
        public ObjectCollection<Sort> SortCollection
        {
            get { return m_SortCollection; }
        }

        private readonly ObjectCollection<ConditionalFormatting> m_ConditionalFormattingCollection = new ObjectCollection<ConditionalFormatting>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ConditionalFormatting", IsNullable = false)]
        public ObjectCollection<ConditionalFormatting> ConditionalFormattingCollection
        {
            get { return m_ConditionalFormattingCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ListView", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ListView
    {

        private string m_ChildItemType;
        [XmlAttribute]
        public string ChildItemType
        {
            get
            {
                return m_ChildItemType;
            }
            set
            {
                m_ChildItemType = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsChildItemFunction;
        [XmlAttribute]
        public bool IsChildItemFunction
        {
            get
            {
                return m_IsChildItemFunction;
            }
            set
            {
                m_IsChildItemFunction = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<ListViewColumn> m_ListViewColumnCollection = new ObjectCollection<ListViewColumn>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ListViewColumn", IsNullable = false)]
        public ObjectCollection<ListViewColumn> ListViewColumnCollection
        {
            get { return m_ListViewColumnCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ListViewColumn", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ListViewColumn
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_label;
        public const string PropertyNameLabel = "Label";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FormElement m_input;
        public const string PropertyNameInput = "Input";



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

        public FormElement Input
        {
            set
            {
                if (m_input == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInput, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_input = value;
                    OnPropertyChanged();
                }
            }
            get { return m_input; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Button", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Button
    {

        private string m_Command;
        [XmlAttribute]
        public string Command
        {
            get
            {
                return m_Command;
            }
            set
            {
                m_Command = value;
                RaisePropertyChanged();
            }
        }


        private bool m_UseClick;
        [XmlAttribute]
        public bool IsAsynchronous
        {
            get
            {
                return m_UseClick;
            }
            set
            {
                m_UseClick = value;
                RaisePropertyChanged();
            }
        }


        private string m_CommandName;
        [XmlAttribute]
        public string CommandName
        {
            get
            {
                return m_CommandName;
            }
            set
            {
                m_CommandName = value;
                RaisePropertyChanged();
            }
        }


        private string m_LoadingText;
        [XmlAttribute]
        public string LoadingText
        {
            get
            {
                return m_LoadingText;
            }
            set
            {
                m_LoadingText = value;
                RaisePropertyChanged();
            }
        }


        private string m_CompleteText;
        [XmlAttribute]
        public string CompleteText
        {
            get
            {
                return m_CompleteText;
            }
            set
            {
                m_CompleteText = value;
                RaisePropertyChanged();
            }
        }


        private string m_IconClass;
        [XmlAttribute]
        public string IconClass
        {
            get
            {
                return m_IconClass;
            }
            set
            {
                m_IconClass = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsToolbarItem;
        [XmlAttribute]
        public bool IsToolbarItem
        {
            get
            {
                return m_IsToolbarItem;
            }
            set
            {
                m_IsToolbarItem = value;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_plural;
        public const string PropertyNamePlural = "Plural";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconStoreId;
        public const string PropertyNameIconStoreId = "IconStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconClass;
        public const string PropertyNameIconClass = "IconClass";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_recordName;
        public const string PropertyNameRecordName = "RecordName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isShowOnNavigationBar;
        public const string PropertyNameIsShowOnNavigationBar = "IsShowOnNavigationBar";


        private readonly ObjectCollection<Member> m_MemberCollection = new ObjectCollection<Member>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Member", IsNullable = false)]
        public ObjectCollection<Member> MemberCollection
        {
            get { return m_MemberCollection; }
        }

        private readonly ObjectCollection<BusinessRule> m_BusinessRuleCollection = new ObjectCollection<BusinessRule>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("BusinessRule", IsNullable = false)]
        public ObjectCollection<BusinessRule> BusinessRuleCollection
        {
            get { return m_BusinessRuleCollection; }
        }

        private readonly ObjectCollection<EntityOperation> m_EntityOperationCollection = new ObjectCollection<EntityOperation>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("EntityOperation", IsNullable = false)]
        public ObjectCollection<EntityOperation> EntityOperationCollection
        {
            get { return m_EntityOperationCollection; }
        }

        private readonly ObjectCollection<string> m_AuthorizedRoleCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> AuthorizedRoleCollection
        {
            get { return m_AuthorizedRoleCollection; }
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
        public string Plural
        {
            set
            {
                if (String.Equals(m_plural, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePlural, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_plural = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_plural;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconStoreId
        {
            set
            {
                if (String.Equals(m_iconStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconStoreId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconClass
        {
            set
            {
                if (String.Equals(m_iconClass, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconClass, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconClass = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconClass;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string RecordName
        {
            set
            {
                if (String.Equals(m_recordName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRecordName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_recordName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_recordName;
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsShowOnNavigationBar
        {
            set
            {
                if (m_isShowOnNavigationBar == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsShowOnNavigationBar, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isShowOnNavigationBar = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isShowOnNavigationBar;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Member", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Member
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
        private bool m_isNotIndexed;
        public const string PropertyNameIsNotIndexed = "IsNotIndexed";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAnalyzed;
        public const string PropertyNameIsAnalyzed = "IsAnalyzed";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isFilterable;
        public const string PropertyNameIsFilterable = "IsFilterable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isExcludeInAll;
        public const string PropertyNameIsExcludeInAll = "IsExcludeInAll";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_boost;
        public const string PropertyNameBoost = "Boost";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_defaultValue;
        public const string PropertyNameDefaultValue = "DefaultValue";



        private readonly ObjectCollection<Member> m_MemberCollection = new ObjectCollection<Member>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Member", IsNullable = false)]
        public ObjectCollection<Member> MemberCollection
        {
            get { return m_MemberCollection; }
        }

        private readonly ObjectCollection<FieldPermission> m_FieldPermissionCollection = new ObjectCollection<FieldPermission>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("FieldPermission", IsNullable = false)]
        public ObjectCollection<FieldPermission> FieldPermissionCollection
        {
            get { return m_FieldPermissionCollection; }
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
        [DebuggerHidden]

        [Required]
        public bool IsNotIndexed
        {
            set
            {
                if (m_isNotIndexed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsNotIndexed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isNotIndexed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isNotIndexed;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsAnalyzed
        {
            set
            {
                if (m_isAnalyzed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAnalyzed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAnalyzed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAnalyzed;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
        public bool IsExcludeInAll
        {
            set
            {
                if (m_isExcludeInAll == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsExcludeInAll, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isExcludeInAll = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isExcludeInAll;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public int Boost
        {
            set
            {
                if (m_boost == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameBoost, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_boost = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_boost;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public Field DefaultValue
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
    [XmlType("EntityForm", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityForm
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityDefinitionId;
        public const string PropertyNameEntityDefinitionId = "EntityDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAllowedNewItem;
        public const string PropertyNameIsAllowedNewItem = "IsAllowedNewItem";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconClass;
        public const string PropertyNameIconClass = "IconClass";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconStoreId;
        public const string PropertyNameIconStoreId = "IconStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isDefault;
        public const string PropertyNameIsDefault = "IsDefault";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isWatchAvailable;
        public const string PropertyNameIsWatchAvailable = "IsWatchAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isEmailAvailable;
        public const string PropertyNameIsEmailAvailable = "IsEmailAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPrintAvailable;
        public const string PropertyNameIsPrintAvailable = "IsPrintAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAuditTrailAvailable;
        public const string PropertyNameIsAuditTrailAvailable = "IsAuditTrailAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRemoveAvailable;
        public const string PropertyNameIsRemoveAvailable = "IsRemoveAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isImportAvailable;
        public const string PropertyNameIsImportAvailable = "IsImportAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isExportAvailable;
        public const string PropertyNameIsExportAvailable = "IsExportAvailable";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operation;
        public const string PropertyNameOperation = "Operation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_partial;
        public const string PropertyNamePartial = "Partial";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_caption;
        public const string PropertyNameCaption = "Caption";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_layout;
        public const string PropertyNameLayout = "Layout";


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

        private readonly ObjectCollection<string> m_Rules = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> Rules
        {
            get { return m_Rules; }
        }

        private readonly ObjectCollection<RouteParameter> m_RouteParameterCollection = new ObjectCollection<RouteParameter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("RouteParameter", IsNullable = false)]
        public ObjectCollection<RouteParameter> RouteParameterCollection
        {
            get { return m_RouteParameterCollection; }
        }

        private readonly ObjectCollection<FormLayout> m_FormLayoutCollection = new ObjectCollection<FormLayout>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("FormLayout", IsNullable = false)]
        public ObjectCollection<FormLayout> FormLayoutCollection
        {
            get { return m_FormLayoutCollection; }
        }

        private readonly ObjectCollection<string> m_CompilerCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> CompilerCollection
        {
            get { return m_CompilerCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string EntityDefinitionId
        {
            set
            {
                if (String.Equals(m_entityDefinitionId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityDefinitionId;
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
        public string Route
        {
            set
            {
                if (String.Equals(m_route, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRoute, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_route = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_route;
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
        public bool IsAllowedNewItem
        {
            set
            {
                if (m_isAllowedNewItem == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAllowedNewItem, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAllowedNewItem = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAllowedNewItem;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconClass
        {
            set
            {
                if (String.Equals(m_iconClass, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconClass, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconClass = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconClass;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconStoreId
        {
            set
            {
                if (String.Equals(m_iconStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconStoreId;
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsDefault
        {
            set
            {
                if (m_isDefault == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsDefault, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isDefault = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isDefault;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsWatchAvailable
        {
            set
            {
                if (m_isWatchAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsWatchAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isWatchAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isWatchAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsEmailAvailable
        {
            set
            {
                if (m_isEmailAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsEmailAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isEmailAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isEmailAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsPrintAvailable
        {
            set
            {
                if (m_isPrintAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsPrintAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isPrintAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isPrintAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsAuditTrailAvailable
        {
            set
            {
                if (m_isAuditTrailAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAuditTrailAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAuditTrailAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAuditTrailAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsRemoveAvailable
        {
            set
            {
                if (m_isRemoveAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRemoveAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRemoveAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRemoveAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsImportAvailable
        {
            set
            {
                if (m_isImportAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsImportAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isImportAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isImportAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsExportAvailable
        {
            set
            {
                if (m_isExportAvailable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsExportAvailable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isExportAvailable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isExportAvailable;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Operation
        {
            set
            {
                if (String.Equals(m_operation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operation;
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

        public string Partial
        {
            set
            {
                if (String.Equals(m_partial, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePartial, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_partial = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_partial;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public string Caption
        {
            set
            {
                if (String.Equals(m_caption, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCaption, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_caption = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_caption;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public string Layout
        {
            set
            {
                if (String.Equals(m_layout, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLayout, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_layout = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_layout;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FormLayout", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FormLayout
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_position;
        public const string PropertyNamePosition = "Position";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isForm;
        public const string PropertyNameIsForm = "IsForm";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAuditTrail;
        public const string PropertyNameIsAuditTrail = "IsAuditTrail";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_content;
        public const string PropertyNameContent = "Content";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_xsmallCol;
        public const string PropertyNameXsmallCol = "XsmallCol";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_mediumCol;
        public const string PropertyNameMediumCol = "MediumCol";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_smallCol;
        public const string PropertyNameSmallCol = "SmallCol";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_largeCol;
        public const string PropertyNameLargeCol = "LargeCol";



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
        public string Position
        {
            set
            {
                if (String.Equals(m_position, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePosition, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_position = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_position;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsForm
        {
            set
            {
                if (m_isForm == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsForm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isForm = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isForm;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsAuditTrail
        {
            set
            {
                if (m_isAuditTrail == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAuditTrail, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAuditTrail = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAuditTrail;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string Content
        {
            set
            {
                if (String.Equals(m_content, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameContent, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_content = value;
                    OnPropertyChanged();
                }
            }
            get { return m_content; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? XsmallCol
        {
            set
            {
                if (m_xsmallCol == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameXsmallCol, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_xsmallCol = value;
                    OnPropertyChanged();
                }
            }
            get { return m_xsmallCol; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? MediumCol
        {
            set
            {
                if (m_mediumCol == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMediumCol, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_mediumCol = value;
                    OnPropertyChanged();
                }
            }
            get { return m_mediumCol; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? SmallCol
        {
            set
            {
                if (m_smallCol == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSmallCol, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_smallCol = value;
                    OnPropertyChanged();
                }
            }
            get { return m_smallCol; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? LargeCol
        {
            set
            {
                if (m_largeCol == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLargeCol, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_largeCol = value;
                    OnPropertyChanged();
                }
            }
            get { return m_largeCol; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityView", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityView
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconClass;
        public const string PropertyNameIconClass = "IconClass";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconStoreId;
        public const string PropertyNameIconStoreId = "IconStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityDefinitionId;
        public const string PropertyNameEntityDefinitionId = "EntityDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_query;
        public const string PropertyNameQuery = "Query";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_visibilty;
        public const string PropertyNameVisibilty = "Visibilty";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_tileColour;
        public const string PropertyNameTileColour = "TileColour";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_countMessage;
        public const string PropertyNameCountMessage = "CountMessage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_partial;
        public const string PropertyNamePartial = "Partial";


        private readonly ObjectCollection<Filter> m_FilterCollection = new ObjectCollection<Filter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Filter", IsNullable = false)]
        public ObjectCollection<Filter> FilterCollection
        {
            get { return m_FilterCollection; }
        }

        private readonly ObjectCollection<ViewColumn> m_ViewColumnCollection = new ObjectCollection<ViewColumn>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ViewColumn", IsNullable = false)]
        public ObjectCollection<ViewColumn> ViewColumnCollection
        {
            get { return m_ViewColumnCollection; }
        }

        private readonly ObjectCollection<Sort> m_SortCollection = new ObjectCollection<Sort>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Sort", IsNullable = false)]
        public ObjectCollection<Sort> SortCollection
        {
            get { return m_SortCollection; }
        }

        private readonly ObjectCollection<ConditionalFormatting> m_ConditionalFormattingCollection = new ObjectCollection<ConditionalFormatting>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ConditionalFormatting", IsNullable = false)]
        public ObjectCollection<ConditionalFormatting> ConditionalFormattingCollection
        {
            get { return m_ConditionalFormattingCollection; }
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

        private readonly ObjectCollection<RouteParameter> m_RouteParameterCollection = new ObjectCollection<RouteParameter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("RouteParameter", IsNullable = false)]
        public ObjectCollection<RouteParameter> RouteParameterCollection
        {
            get { return m_RouteParameterCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconClass
        {
            set
            {
                if (String.Equals(m_iconClass, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconClass, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconClass = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconClass;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconStoreId
        {
            set
            {
                if (String.Equals(m_iconStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconStoreId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string EntityDefinitionId
        {
            set
            {
                if (String.Equals(m_entityDefinitionId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityDefinitionId;
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
        public string Route
        {
            set
            {
                if (String.Equals(m_route, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRoute, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_route = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_route;
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Visibilty
        {
            set
            {
                if (String.Equals(m_visibilty, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameVisibilty, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_visibilty = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_visibilty;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string TileColour
        {
            set
            {
                if (String.Equals(m_tileColour, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTileColour, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_tileColour = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_tileColour;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        public string CountMessage
        {
            set
            {
                if (String.Equals(m_countMessage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCountMessage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_countMessage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_countMessage;
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
        public string Partial
        {
            set
            {
                if (String.Equals(m_partial, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePartial, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_partial = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_partial;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Filter", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Filter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_term;
        public const string PropertyNameTerm = "Term";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Operator m_operator;
        public const string PropertyNameOperator = "Operator";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Field m_field;
        public const string PropertyNameField = "Field";



        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Term
        {
            set
            {
                if (String.Equals(m_term, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTerm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_term = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_term;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
            get
            {
                return m_operator;
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
    [XmlType("ViewColumn", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ViewColumn
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_header;
        public const string PropertyNameHeader = "Header";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_sort;
        public const string PropertyNameSort = "Sort";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isLinkColumn;
        public const string PropertyNameIsLinkColumn = "IsLinkColumn";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_formRoute;
        public const string PropertyNameFormRoute = "FormRoute";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconCssClass;
        public const string PropertyNameIconCssClass = "IconCssClass";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_iconStoreId;
        public const string PropertyNameIconStoreId = "IconStoreId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_format;
        public const string PropertyNameFormat = "Format";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_routeValueField;
        public const string PropertyNameRouteValueField = "RouteValueField";


        private readonly ObjectCollection<ConditionalFormatting> m_ConditionalFormattingCollection = new ObjectCollection<ConditionalFormatting>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ConditionalFormatting", IsNullable = false)]
        public ObjectCollection<ConditionalFormatting> ConditionalFormattingCollection
        {
            get { return m_ConditionalFormattingCollection; }
        }

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

        [Required]
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
        [DebuggerHidden]

        public string Sort
        {
            set
            {
                if (String.Equals(m_sort, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSort, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sort = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sort;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsLinkColumn
        {
            set
            {
                if (m_isLinkColumn == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsLinkColumn, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isLinkColumn = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isLinkColumn;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string FormRoute
        {
            set
            {
                if (String.Equals(m_formRoute, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFormRoute, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_formRoute = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_formRoute;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconCssClass
        {
            set
            {
                if (String.Equals(m_iconCssClass, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconCssClass, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconCssClass = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconCssClass;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string IconStoreId
        {
            set
            {
                if (String.Equals(m_iconStoreId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIconStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_iconStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_iconStoreId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
        public string RouteValueField
        {
            set
            {
                if (String.Equals(m_routeValueField, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRouteValueField, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_routeValueField = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_routeValueField;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Sort", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Sort
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SortDirection m_direction;
        public const string PropertyNameDirection = "Direction";


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

        [Required]
        public SortDirection Direction
        {
            set
            {
                if (m_direction == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDirection, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_direction = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_direction;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ImageElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ImageElement
    {

        private bool m_IsThumbnail;
        [XmlAttribute]
        public bool IsThumbnail
        {
            get
            {
                return m_IsThumbnail;
            }
            set
            {
                m_IsThumbnail = value;
                RaisePropertyChanged();
            }
        }


        public int? Width { get; set; }

        public int? Height { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("DownloadLink", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class DownloadLink
    {

        private bool m_IsTransformTemplate;
        [XmlAttribute]
        public bool IsTransformTemplate
        {
            get
            {
                return m_IsTransformTemplate;
            }
            set
            {
                m_IsTransformTemplate = value;
                RaisePropertyChanged();
            }
        }


        private int m_TemplateId;
        [XmlAttribute]
        public int TemplateId
        {
            get
            {
                return m_TemplateId;
            }
            set
            {
                m_TemplateId = value;
                RaisePropertyChanged();
            }
        }


        private string m_Entity;
        [XmlAttribute]
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


        private string m_IconClass;
        [XmlAttribute]
        public string IconClass
        {
            get
            {
                return m_IconClass;
            }
            set
            {
                m_IconClass = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("FieldPermission", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FieldPermission
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_role;
        public const string PropertyNameRole = "Role";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHidden;
        public const string PropertyNameIsHidden = "IsHidden";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isReadOnly;
        public const string PropertyNameIsReadOnly = "IsReadOnly";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Role
        {
            set
            {
                if (String.Equals(m_role, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRole, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_role = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_role;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsHidden
        {
            set
            {
                if (m_isHidden == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHidden, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHidden = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHidden;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsReadOnly
        {
            set
            {
                if (m_isReadOnly == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsReadOnly, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isReadOnly = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isReadOnly;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityPermission", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityPermission
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_role;
        public const string PropertyNameRole = "Role";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHidden;
        public const string PropertyNameIsHidden = "IsHidden";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isReadOnly;
        public const string PropertyNameIsReadOnly = "IsReadOnly";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Role
        {
            set
            {
                if (String.Equals(m_role, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRole, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_role = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_role;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsHidden
        {
            set
            {
                if (m_isHidden == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHidden, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHidden = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHidden;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsReadOnly
        {
            set
            {
                if (m_isReadOnly == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsReadOnly, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isReadOnly = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isReadOnly;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityOperation", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityOperation
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_successMessage;
        public const string PropertyNameSuccessMessage = "SuccessMessage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_showSuccessMessage;
        public const string PropertyNameShowSuccessMessage = "ShowSuccessMessage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_navigateSuccessUrl;
        public const string PropertyNameNavigateSuccessUrl = "NavigateSuccessUrl";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_successCommand;
        public const string PropertyNameSuccessCommand = "SuccessCommand";


        private readonly ObjectCollection<EntityPermission> m_EntityPermissionCollection = new ObjectCollection<EntityPermission>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("EntityPermission", IsNullable = false)]
        public ObjectCollection<EntityPermission> EntityPermissionCollection
        {
            get { return m_EntityPermissionCollection; }
        }

        private readonly ObjectCollection<string> m_Rules = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> Rules
        {
            get { return m_Rules; }
        }

        private readonly ObjectCollection<string> m_Permissions = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> Permissions
        {
            get { return m_Permissions; }
        }

        private readonly ObjectCollection<SetterActionChild> m_SetterActionChildCollection = new ObjectCollection<SetterActionChild>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("SetterActionChild", IsNullable = false)]
        public ObjectCollection<SetterActionChild> SetterActionChildCollection
        {
            get { return m_SetterActionChildCollection; }
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
        public string SuccessMessage
        {
            set
            {
                if (String.Equals(m_successMessage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSuccessMessage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_successMessage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_successMessage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool ShowSuccessMessage
        {
            set
            {
                if (m_showSuccessMessage == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameShowSuccessMessage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_showSuccessMessage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_showSuccessMessage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string NavigateSuccessUrl
        {
            set
            {
                if (String.Equals(m_navigateSuccessUrl, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNavigateSuccessUrl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_navigateSuccessUrl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_navigateSuccessUrl;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string SuccessCommand
        {
            set
            {
                if (String.Equals(m_successCommand, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSuccessCommand, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_successCommand = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_successCommand;
            }
        }



    }

    // placeholder for SetterActionChild
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityChart", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityChart
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityDefinitionId;
        public const string PropertyNameEntityDefinitionId = "EntityDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityViewId;
        public const string PropertyNameEntityViewId = "EntityViewId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_query;
        public const string PropertyNameQuery = "Query";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_aggregate;
        public const string PropertyNameAggregate = "Aggregate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_field;
        public const string PropertyNameField = "Field";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_dateInterval;
        public const string PropertyNameDateInterval = "DateInterval";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isDashboardItem;
        public const string PropertyNameIsDashboardItem = "IsDashboardItem";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_dasboardItemPosition;
        public const string PropertyNameDasboardItemPosition = "DasboardItemPosition";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_histogramInterval;
        public const string PropertyNameHistogramInterval = "HistogramInterval";



        private readonly ObjectCollection<Series> m_SeriesCollection = new ObjectCollection<Series>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Series", IsNullable = false)]
        public ObjectCollection<Series> SeriesCollection
        {
            get { return m_SeriesCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string EntityDefinitionId
        {
            set
            {
                if (String.Equals(m_entityDefinitionId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityDefinitionId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityDefinitionId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityDefinitionId;
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
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string EntityViewId
        {
            set
            {
                if (String.Equals(m_entityViewId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityViewId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityViewId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityViewId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
        public string Field
        {
            set
            {
                if (String.Equals(m_field, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameField, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_field = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_field;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string DateInterval
        {
            set
            {
                if (String.Equals(m_dateInterval, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateInterval, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateInterval = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateInterval;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public bool IsDashboardItem
        {
            set
            {
                if (m_isDashboardItem == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsDashboardItem, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isDashboardItem = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isDashboardItem;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public int DasboardItemPosition
        {
            set
            {
                if (m_dasboardItemPosition == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDasboardItemPosition, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dasboardItemPosition = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dasboardItemPosition;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? HistogramInterval
        {
            set
            {
                if (m_histogramInterval == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHistogramInterval, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_histogramInterval = value;
                    OnPropertyChanged();
                }
            }
            get { return m_histogramInterval; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("Series", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class Series
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_color;
        public const string PropertyNameColor = "Color";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_query;
        public const string PropertyNameQuery = "Query";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_aggregate;
        public const string PropertyNameAggregate = "Aggregate";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_field;
        public const string PropertyNameField = "Field";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_dateInterval;
        public const string PropertyNameDateInterval = "DateInterval";


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
        public string Color
        {
            set
            {
                if (String.Equals(m_color, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameColor, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_color = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_color;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
        public string Field
        {
            set
            {
                if (String.Equals(m_field, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameField, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_field = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_field;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string DateInterval
        {
            set
            {
                if (String.Equals(m_dateInterval, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDateInterval, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_dateInterval = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_dateInterval;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("SearchDefinition", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SearchDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OwnerType m_ownerType;
        public const string PropertyNameOwnerType = "OwnerType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_owner;
        public const string PropertyNameOwner = "Owner";


        private readonly ObjectCollection<Filter> m_FilterCollection = new ObjectCollection<Filter>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Filter", IsNullable = false)]
        public ObjectCollection<Filter> FilterCollection
        {
            get { return m_FilterCollection; }
        }

        private readonly ObjectCollection<ViewColumn> m_ViewColumnCollection = new ObjectCollection<ViewColumn>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("ViewColumn", IsNullable = false)]
        public ObjectCollection<ViewColumn> ViewColumnCollection
        {
            get { return m_ViewColumnCollection; }
        }

        private readonly ObjectCollection<Sort> m_SortCollection = new ObjectCollection<Sort>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Sort", IsNullable = false)]
        public ObjectCollection<Sort> SortCollection
        {
            get { return m_SortCollection; }
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Route
        {
            set
            {
                if (String.Equals(m_route, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRoute, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_route = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_route;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public OwnerType OwnerType
        {
            set
            {
                if (m_ownerType == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOwnerType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_ownerType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_ownerType;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Owner
        {
            set
            {
                if (String.Equals(m_owner, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOwner, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_owner = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_owner;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("ConditionalFormatting", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class ConditionalFormatting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_cssClass;
        public const string PropertyNameCssClass = "CssClass";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_condition;
        public const string PropertyNameCondition = "Condition";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string CssClass
        {
            set
            {
                if (String.Equals(m_cssClass, value, StringComparison.Ordinal)) return;
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


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]
        [DebuggerHidden]

        [Required]
        public string Condition
        {
            set
            {
                if (String.Equals(m_condition, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCondition, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_condition = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_condition;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("EntityLookupElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class EntityLookupElement
    {

        private string m_Entity;
        [XmlAttribute]
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


        private string m_DisplayMemberPath;
        [XmlAttribute]
        public string DisplayMemberPath
        {
            get
            {
                return m_DisplayMemberPath;
            }
            set
            {
                m_DisplayMemberPath = value;
                RaisePropertyChanged();
            }
        }


        private string m_ValueMemberPath;
        [XmlAttribute]
        public string ValueMemberPath
        {
            get
            {
                return m_ValueMemberPath;
            }
            set
            {
                m_ValueMemberPath = value;
                RaisePropertyChanged();
            }
        }


        private string m_DisplayTemplate;
        [XmlAttribute]
        public string DisplayTemplate
        {
            get
            {
                return m_DisplayTemplate;
            }
            set
            {
                m_DisplayTemplate = value;
                RaisePropertyChanged();
            }
        }


        private readonly ObjectCollection<string> m_LookupColumnCollection = new ObjectCollection<string>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("", IsNullable = false)]
        public ObjectCollection<string> LookupColumnCollection
        {
            get { return m_LookupColumnCollection; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("CurrencyElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class CurrencyElement
    {

        private string m_Currency;
        [XmlAttribute]
        public string Currency
        {
            get
            {
                return m_Currency;
            }
            set
            {
                m_Currency = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("RouteParameter", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class RouteParameter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    [XmlType("PartialJs", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class PartialJs
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


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



    }


    [XmlType("FormElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class FormElement
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_label;
        public const string PropertyNameLabel = "Label";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_tooltip;
        public const string PropertyNameTooltip = "Tooltip";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_path;
        public const string PropertyNamePath = "Path";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_size;
        public const string PropertyNameSize = "Size";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_cssClass;
        public const string PropertyNameCssClass = "CssClass";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_visible;
        public const string PropertyNameVisible = "Visible";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_enable;
        public const string PropertyNameEnable = "Enable";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_elementId;
        public const string PropertyNameElementId = "ElementId";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_helpText;
        public const string PropertyNameHelpText = "HelpText";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_useDisplayTemplate;
        public const string PropertyNameUseDisplayTemplate = "UseDisplayTemplate";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_toolboxIconClass;
        public const string PropertyNameToolboxIconClass = "ToolboxIconClass";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isUniqueName;
        public const string PropertyNameIsUniqueName = "IsUniqueName";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_labelColLg;
        public const string PropertyNameLabelColLg = "LabelColLg";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_labelColMd;
        public const string PropertyNameLabelColMd = "LabelColMd";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_labelColSm;
        public const string PropertyNameLabelColSm = "LabelColSm";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_labelColXs;
        public const string PropertyNameLabelColXs = "LabelColXs";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_inputColLg;
        public const string PropertyNameInputColLg = "InputColLg";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_inputColMd;
        public const string PropertyNameInputColMd = "InputColMd";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_inputColSm;
        public const string PropertyNameInputColSm = "InputColSm";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private int? m_inputColXs;
        public const string PropertyNameInputColXs = "InputColXs";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FieldValidation m_fieldValidation
                = new FieldValidation();

        public const string PropertyNameFieldValidation = "FieldValidation";
        [DebuggerHidden]

        public FieldValidation FieldValidation
        {
            get { return m_fieldValidation; }
            set
            {
                m_fieldValidation = value;
                OnPropertyChanged();
            }
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
        public string Path
        {
            set
            {
                if (m_path == value) return;
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



        [XmlAttribute]
        public bool IsRequired
        {
            set
            {
                if (m_isRequired == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsRequired, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isRequired = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isRequired;
            }
        }



        [XmlAttribute]
        public string Size
        {
            set
            {
                if (m_size == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSize, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_size = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_size;
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
        public string Enable
        {
            set
            {
                if (m_enable == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnable, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_enable = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_enable;
            }
        }



        [XmlAttribute]
        public string ElementId
        {
            set
            {
                if (m_elementId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameElementId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_elementId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_elementId;
            }
        }



        [XmlAttribute]
        public string HelpText
        {
            set
            {
                if (m_helpText == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHelpText, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_helpText = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_helpText;
            }
        }



        [XmlAttribute]
        public bool UseDisplayTemplate
        {
            set
            {
                if (m_useDisplayTemplate == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUseDisplayTemplate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_useDisplayTemplate = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_useDisplayTemplate;
            }
        }



        [XmlAttribute]
        public string ToolboxIconClass
        {
            set
            {
                if (m_toolboxIconClass == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameToolboxIconClass, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_toolboxIconClass = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_toolboxIconClass;
            }
        }



        [XmlAttribute]
        public bool IsUniqueName
        {
            set
            {
                if (m_isUniqueName == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsUniqueName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isUniqueName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isUniqueName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? LabelColLg
        {
            set
            {
                if (m_labelColLg == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColLg, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColLg = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColLg; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? LabelColMd
        {
            set
            {
                if (m_labelColMd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColMd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColMd = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColMd; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? LabelColSm
        {
            set
            {
                if (m_labelColSm == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColSm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColSm = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColSm; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? LabelColXs
        {
            set
            {
                if (m_labelColXs == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameLabelColXs, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_labelColXs = value;
                    OnPropertyChanged();
                }
            }
            get { return m_labelColXs; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? InputColLg
        {
            set
            {
                if (m_inputColLg == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColLg, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColLg = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColLg; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? InputColMd
        {
            set
            {
                if (m_inputColMd == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColMd, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColMd = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColMd; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? InputColSm
        {
            set
            {
                if (m_inputColSm == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColSm, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColSm = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColSm; }
        }


        ///<summary>
        /// 
        ///</summary>
        public int? InputColXs
        {
            set
            {
                if (m_inputColXs == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInputColXs, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_inputColXs = value;
                    OnPropertyChanged();
                }
            }
            get { return m_inputColXs; }
        }



    }


    // placeholder for Field complext type

    // placeholder for Operator enum

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortDirection
    {
        Asc,
        Desc,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OwnerType
    {
        User,
        Everyone,
        Role,
        Designation,
        Department,

    }

}
// ReSharper restore InconsistentNaming

