
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

        [Required]

        [DebuggerHidden]

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


        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }


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

        [Required]

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

        [Required]

        [DebuggerHidden]

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
    [XmlType("SpaceFeaturesElement", Namespace = Strings.DEFAULT_NAMESPACE)]
    public partial class SpaceFeaturesElement
    {


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
        private int m_maxLength;
        public const string PropertyNameMaxLength = "MaxLength";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_minLength;
        public const string PropertyNameMinLength = "MinLength";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_pattern;
        public const string PropertyNamePattern = "Pattern";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_mode;
        public const string PropertyNameMode = "Mode";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_message;
        public const string PropertyNameMessage = "Message";


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

        [Required]

        [DebuggerHidden]

        public int MaxLength
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
            get
            {
                return m_maxLength;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int MinLength
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
            get
            {
                return m_minLength;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

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

        [Required]

        [DebuggerHidden]

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

        [Required]

        [DebuggerHidden]

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

        [Required]

        [DebuggerHidden]

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

        [Required]

        [DebuggerHidden]

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

        [Required]

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
        public bool UseClick
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
        private int m_entityDefinitionId;
        public const string PropertyNameEntityDefinitionId = "EntityDefinitionId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_plural;
        public const string PropertyNamePlural = "Plural";


        private readonly ObjectCollection<Member> m_MemberCollection = new ObjectCollection<Member>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Member", IsNullable = false)]
        public ObjectCollection<Member> MemberCollection
        {
            get { return m_MemberCollection; }
        }

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int EntityDefinitionId
        {
            set
            {
                if (m_entityDefinitionId == value) return;
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
        private bool m_isAnalyzed;
        public const string PropertyNameIsAnalyzed = "IsAnalyzed";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isFilterable;
        public const string PropertyNameIsFilterable = "IsFilterable";


        private readonly ObjectCollection<Member> m_MemberCollection = new ObjectCollection<Member>();

        ///<summary>
        /// 
        ///</summary>
        [XmlArrayItem("Member", IsNullable = false)]
        public ObjectCollection<Member> MemberCollection
        {
            get { return m_MemberCollection; }
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
        private int m_entityFormId;
        public const string PropertyNameEntityFormId = "EntityFormId";


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

        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int EntityFormId
        {
            set
            {
                if (m_entityFormId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityFormId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityFormId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityFormId;
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
        private int m_entityViewId;
        public const string PropertyNameEntityViewId = "EntityViewId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        ///<summary>
        /// 
        ///</summary>
        [XmlAttribute]

        [Required]

        [DebuggerHidden]

        public int EntityViewId
        {
            set
            {
                if (m_entityViewId == value) return;
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


}
// ReSharper restore InconsistentNaming

