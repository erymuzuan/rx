﻿
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



        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FormElement> FormElementCollection { get; } = new ObjectCollection<FormElement>();


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
    public partial class TextBox
    {

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


        private string m_AutoCompletionEntity;
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
    public partial class CheckBox
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DatePicker
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DateTimePicker
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ComboBox
    {

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ComboBoxItem> ComboBoxItemCollection { get; } = new ObjectCollection<ComboBoxItem>();


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
    public partial class TextAreaElement
    {

        private string m_Rows;
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
    public partial class WebsiteFormElement
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class EmailFormElement
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class NumberTextBox
    {

        private int m_Step;
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
    public partial class MapElement
    {

        private string m_Icon;
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
    public partial class SectionFormElement
    {


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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
    public partial class AddressElement
    {

        private bool m_IsUnitNoVisible;
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
    public partial class HtmlElement
    {

        public string Text { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Rule> RuleCollection { get; } = new ObjectCollection<Rule>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Rule> FilterCollection { get; } = new ObjectCollection<Rule>();


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
    public partial class FileUploadElement
    {

        private string m_AllowedExtensions;
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
    public partial class ChildEntityListView
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


        private string m_Query;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ViewColumn> ViewColumnCollection { get; } = new ObjectCollection<ViewColumn>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Sort> SortCollection { get; } = new ObjectCollection<Sort>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ConditionalFormatting> ConditionalFormattingCollection { get; } = new ObjectCollection<ConditionalFormatting>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ListView
    {

        private string m_ChildItemType;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ListViewColumn> ListViewColumnCollection { get; } = new ObjectCollection<ListViewColumn>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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
    public partial class Button
    {

        private string m_Command;
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


        private string m_DeleteOperationSuccessNavigateUrl;
        public string DeleteOperationSuccessNavigateUrl
        {
            get
            {
                return m_DeleteOperationSuccessNavigateUrl;
            }
            set
            {
                m_DeleteOperationSuccessNavigateUrl = value;
                RaisePropertyChanged();
            }
        }


        private string m_DeleteOperationSuccessMesage;
        public string DeleteOperationSuccessMesage
        {
            get
            {
                return m_DeleteOperationSuccessMesage;
            }
            set
            {
                m_DeleteOperationSuccessMesage = value;
                RaisePropertyChanged();
            }
        }


        private string m_DeleteOperation;
        public string DeleteOperation
        {
            get
            {
                return m_DeleteOperation;
            }
            set
            {
                m_DeleteOperation = value;
                RaisePropertyChanged();
            }
        }


        private string m_OperationMethod;
        public string OperationMethod
        {
            get
            {
                return m_OperationMethod;
            }
            set
            {
                m_OperationMethod = value;
                RaisePropertyChanged();
            }
        }


        private string m_OperationFailureCallback;
        public string OperationFailureCallback
        {
            get
            {
                return m_OperationFailureCallback;
            }
            set
            {
                m_OperationFailureCallback = value;
                RaisePropertyChanged();
            }
        }


        private string m_OperationSuccessCallback;
        public string OperationSuccessCallback
        {
            get
            {
                return m_OperationSuccessCallback;
            }
            set
            {
                m_OperationSuccessCallback = value;
                RaisePropertyChanged();
            }
        }


        private string m_OperationSuccessNavigateUrl;
        public string OperationSuccessNavigateUrl
        {
            get
            {
                return m_OperationSuccessNavigateUrl;
            }
            set
            {
                m_OperationSuccessNavigateUrl = value;
                RaisePropertyChanged();
            }
        }


        private string m_OperationSuccessMesage;
        public string OperationSuccessMesage
        {
            get
            {
                return m_OperationSuccessMesage;
            }
            set
            {
                m_OperationSuccessMesage = value;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class EntityDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_plural;
        public const string PropertyNamePlural = "Plural";


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
        private bool m_treatDataAsSource;
        public const string PropertyNameTreatDataAsSource = "TreatDataAsSource";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_transient;
        public const string PropertyNameTransient = "Transient";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_enableAuditing;
        public const string PropertyNameEnableAuditing = "EnableAuditing";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_enableTracking;
        public const string PropertyNameEnableTracking = "EnableTracking";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? m_storeInDatabase;
        public const string PropertyNameStoreInDatabase = "StoreInDatabase";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? m_storeInElasticsearch;
        public const string PropertyNameStoreInElasticsearch = "StoreInElasticsearch";



        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<BusinessRule> BusinessRuleCollection { get; } = new ObjectCollection<BusinessRule>();


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ServiceContract m_serviceContract
                = new ServiceContract();

        public const string PropertyNameServiceContract = "ServiceContract";
        [DebuggerHidden]

        public ServiceContract ServiceContract
        {
            get { return m_serviceContract; }
            set
            {
                m_serviceContract = value;
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
        [DebuggerHidden]

        public bool TreatDataAsSource
        {
            set
            {
                if (m_treatDataAsSource == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTreatDataAsSource, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_treatDataAsSource = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_treatDataAsSource;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public bool Transient
        {
            set
            {
                if (m_transient == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTransient, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_transient = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_transient;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public bool EnableAuditing
        {
            set
            {
                if (m_enableAuditing == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnableAuditing, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_enableAuditing = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_enableAuditing;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public bool EnableTracking
        {
            set
            {
                if (m_enableTracking == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnableTracking, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_enableTracking = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_enableTracking;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public bool? StoreInDatabase
        {
            set
            {
                if (m_storeInDatabase == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreInDatabase, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_storeInDatabase = value;
                    OnPropertyChanged();
                }
            }
            get { return m_storeInDatabase; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]
        [Obsolete("If you don't want it, the use SqlServerReadOnlyRepository<T>")]
        public bool? StoreInElasticsearch
        {
            set
            {
                if (m_storeInElasticsearch == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameStoreInElasticsearch, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_storeInElasticsearch = value;
                    OnPropertyChanged();
                }
            }
            get { return m_storeInElasticsearch; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ValueObjectDefinition
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<BusinessRule> BusinessRuleCollection { get; } = new ObjectCollection<BusinessRule>();


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
    public partial class SimpleMember
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


        private bool m_IsNullable;
        public bool IsNullable
        {
            get
            {
                return m_IsNullable;
            }
            set
            {
                m_IsNullable = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsNotIndexed;
        public bool IsNotIndexed
        {
            get
            {
                return m_IsNotIndexed;
            }
            set
            {
                m_IsNotIndexed = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsAnalyzed;
        public bool IsAnalyzed
        {
            get
            {
                return m_IsAnalyzed;
            }
            set
            {
                m_IsAnalyzed = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsFilterable;
        public bool IsFilterable
        {
            get
            {
                return m_IsFilterable;
            }
            set
            {
                m_IsFilterable = value;
                RaisePropertyChanged();
            }
        }


        private bool m_IsExcludeInAll;
        public bool IsExcludeInAll
        {
            get
            {
                return m_IsExcludeInAll;
            }
            set
            {
                m_IsExcludeInAll = value;
                RaisePropertyChanged();
            }
        }


        private int m_Boost;
        public int Boost
        {
            get
            {
                return m_Boost;
            }
            set
            {
                m_Boost = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ComplexMember
    {

        private string m_EmptyField;
        public string EmptyField
        {
            get
            {
                return m_EmptyField;
            }
            set
            {
                m_EmptyField = value;
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
    public partial class ValueObjectMember
    {

        private string m_ValueObjectName;
        public string ValueObjectName
        {
            get
            {
                return m_ValueObjectName;
            }
            set
            {
                m_ValueObjectName = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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
        private string m_operationSuccessMesage;
        public const string PropertyNameOperationSuccessMesage = "OperationSuccessMesage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationSuccessNavigateUrl;
        public const string PropertyNameOperationSuccessNavigateUrl = "OperationSuccessNavigateUrl";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationSuccessCallback;
        public const string PropertyNameOperationSuccessCallback = "OperationSuccessCallback";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationFailureCallback;
        public const string PropertyNameOperationFailureCallback = "OperationFailureCallback";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationMethod;
        public const string PropertyNameOperationMethod = "OperationMethod";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_deleteOperation;
        public const string PropertyNameDeleteOperation = "DeleteOperation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_deleteOperationSuccessMesage;
        public const string PropertyNameDeleteOperationSuccessMesage = "DeleteOperationSuccessMesage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_deleteOperationSuccessNavigateUrl;
        public const string PropertyNameDeleteOperationSuccessNavigateUrl = "DeleteOperationSuccessNavigateUrl";


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
        public ObjectCollection<string> Rules { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<RouteParameter> RouteParameterCollection { get; } = new ObjectCollection<RouteParameter>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FormLayout> FormLayoutCollection { get; } = new ObjectCollection<FormLayout>();


        ///<summary>
        /// 
        ///</summary>
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


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationSuccessMesage
        {
            set
            {
                if (String.Equals(m_operationSuccessMesage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationSuccessMesage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationSuccessMesage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationSuccessMesage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationSuccessNavigateUrl
        {
            set
            {
                if (String.Equals(m_operationSuccessNavigateUrl, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationSuccessNavigateUrl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationSuccessNavigateUrl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationSuccessNavigateUrl;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationSuccessCallback
        {
            set
            {
                if (String.Equals(m_operationSuccessCallback, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationSuccessCallback, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationSuccessCallback = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationSuccessCallback;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationFailureCallback
        {
            set
            {
                if (String.Equals(m_operationFailureCallback, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationFailureCallback, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationFailureCallback = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationFailureCallback;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationMethod
        {
            set
            {
                if (String.Equals(m_operationMethod, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationMethod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationMethod = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationMethod;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public string DeleteOperation
        {
            set
            {
                if (String.Equals(m_deleteOperation, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDeleteOperation, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_deleteOperation = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_deleteOperation;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string DeleteOperationSuccessMesage
        {
            set
            {
                if (String.Equals(m_deleteOperationSuccessMesage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDeleteOperationSuccessMesage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_deleteOperationSuccessMesage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_deleteOperationSuccessMesage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string DeleteOperationSuccessNavigateUrl
        {
            set
            {
                if (String.Equals(m_deleteOperationSuccessNavigateUrl, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDeleteOperationSuccessNavigateUrl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_deleteOperationSuccessNavigateUrl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_deleteOperationSuccessNavigateUrl;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class WorkflowForm
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_workflowDefinitionId;
        public const string PropertyNameWorkflowDefinitionId = "WorkflowDefinitionId";


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
        private string m_operation;
        public const string PropertyNameOperation = "Operation";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_variable;
        public const string PropertyNameVariable = "Variable";


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
        private string m_operationSuccessMesage;
        public const string PropertyNameOperationSuccessMesage = "OperationSuccessMesage";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationSuccessNavigateUrl;
        public const string PropertyNameOperationSuccessNavigateUrl = "OperationSuccessNavigateUrl";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationSuccessCallback;
        public const string PropertyNameOperationSuccessCallback = "OperationSuccessCallback";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationFailureCallback;
        public const string PropertyNameOperationFailureCallback = "OperationFailureCallback";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_operationMethod;
        public const string PropertyNameOperationMethod = "OperationMethod";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


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
        public ObjectCollection<string> Rules { get; } = new ObjectCollection<string>();


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


        ///<summary>
        /// 
        ///</summary>
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


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationSuccessMesage
        {
            set
            {
                if (String.Equals(m_operationSuccessMesage, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationSuccessMesage, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationSuccessMesage = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationSuccessMesage;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationSuccessNavigateUrl
        {
            set
            {
                if (String.Equals(m_operationSuccessNavigateUrl, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationSuccessNavigateUrl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationSuccessNavigateUrl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationSuccessNavigateUrl;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationSuccessCallback
        {
            set
            {
                if (String.Equals(m_operationSuccessCallback, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationSuccessCallback, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationSuccessCallback = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationSuccessCallback;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationFailureCallback
        {
            set
            {
                if (String.Equals(m_operationFailureCallback, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationFailureCallback, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationFailureCallback = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationFailureCallback;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string OperationMethod
        {
            set
            {
                if (String.Equals(m_operationMethod, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameOperationMethod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_operationMethod = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_operationMethod;
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


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_template;
        public const string PropertyNameTemplate = "Template";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_displayOnDashboard;
        public const string PropertyNameDisplayOnDashboard = "DisplayOnDashboard";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_endpoint;
        public const string PropertyNameEndpoint = "Endpoint";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ViewColumn> ViewColumnCollection { get; } = new ObjectCollection<ViewColumn>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ConditionalFormatting> ConditionalFormattingCollection { get; } = new ObjectCollection<ConditionalFormatting>();


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
        public ObjectCollection<RouteParameter> RouteParameterCollection { get; } = new ObjectCollection<RouteParameter>();


        ///<summary>
        /// 
        ///</summary>
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
        [DebuggerHidden]

        public string Template
        {
            set
            {
                if (String.Equals(m_template, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTemplate, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_template = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_template;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool DisplayOnDashboard
        {
            set
            {
                if (m_displayOnDashboard == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDisplayOnDashboard, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_displayOnDashboard = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_displayOnDashboard;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Endpoint
        {
            set
            {
                if (String.Equals(m_endpoint, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEndpoint, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_endpoint = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_endpoint;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ConditionalFormatting> ConditionalFormattingCollection { get; } = new ObjectCollection<ConditionalFormatting>();


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
    public partial class ImageElement
    {

        private bool m_IsThumbnail;
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
    public partial class DownloadLink
    {

        private bool m_IsTransformTemplate;
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
    public partial class OperationEndpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHttpPut;
        public const string PropertyNameIsHttpPut = "IsHttpPut";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHttpPatch;
        public const string PropertyNameIsHttpPatch = "IsHttpPatch";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHttpPost;
        public const string PropertyNameIsHttpPost = "IsHttpPost";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isHttpDelete;
        public const string PropertyNameIsHttpDelete = "IsHttpDelete";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_resource;
        public const string PropertyNameResource = "Resource";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isConflictDetectionEnabled;
        public const string PropertyNameIsConflictDetectionEnabled = "IsConflictDetectionEnabled";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<EntityPermission> EntityPermissionCollection { get; } = new ObjectCollection<EntityPermission>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> Rules { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<SetterActionChild> SetterActionChildCollection { get; } = new ObjectCollection<SetterActionChild>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<PatchSetter> PatchPathCollection { get; } = new ObjectCollection<PatchSetter>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection { get; } = new ObjectCollection<ReferencedAssembly>();


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
        [DebuggerHidden]

        [Required]
        public bool IsHttpPut
        {
            set
            {
                if (m_isHttpPut == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHttpPut, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHttpPut = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHttpPut;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsHttpPatch
        {
            set
            {
                if (m_isHttpPatch == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHttpPatch, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHttpPatch = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHttpPatch;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsHttpPost
        {
            set
            {
                if (m_isHttpPost == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHttpPost, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHttpPost = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHttpPost;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsHttpDelete
        {
            set
            {
                if (m_isHttpDelete == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsHttpDelete, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isHttpDelete = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isHttpDelete;
            }
        }


        ///<summary>
        /// 
        ///</summary>
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
        public string Resource
        {
            set
            {
                if (String.Equals(m_resource, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameResource, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_resource = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_resource;
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


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsConflictDetectionEnabled
        {
            set
            {
                if (m_isConflictDetectionEnabled == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsConflictDetectionEnabled, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isConflictDetectionEnabled = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isConflictDetectionEnabled;
            }
        }



    }

    // placeholder for SetterActionChild
    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
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



        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Series> SeriesCollection { get; } = new ObjectCollection<Series>();


        ///<summary>
        /// 
        ///</summary>
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
    public partial class EntityLookupElement
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


        private string m_DisplayMemberPath;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> LookupColumnCollection { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Filter> FilterCollection { get; } = new ObjectCollection<Filter>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class CurrencyElement
    {

        private string m_Currency;
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
    public partial class RouteParameter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_type;
        public const string PropertyNameType = "Type";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_defaultValue;
        public const string PropertyNameDefaultValue = "DefaultValue";


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

        public string DefaultValue
        {
            set
            {
                if (String.Equals(m_defaultValue, value, StringComparison.Ordinal)) return;
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

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class PartialJs
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ViewTemplate
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_viewModelType;
        public const string PropertyNameViewModelType = "ViewModelType";


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
        public string ViewModelType
        {
            set
            {
                if (String.Equals(m_viewModelType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameViewModelType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_viewModelType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_viewModelType;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class PatchSetter
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_defaultValue;
        public const string PropertyNameDefaultValue = "DefaultValue";


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
        [DebuggerHidden]

        public string DefaultValue
        {
            set
            {
                if (String.Equals(m_defaultValue, value, StringComparison.Ordinal)) return;
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

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FormDialog
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_title;
        public const string PropertyNameTitle = "Title";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAllowCancel;
        public const string PropertyNameIsAllowCancel = "IsAllowCancel";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_memberPath;
        public const string PropertyNameMemberPath = "MemberPath";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


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
        public ObjectCollection<DialogButton> DialogButtonCollection { get; } = new ObjectCollection<DialogButton>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> Rules { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
        public bool IsAllowCancel
        {
            set
            {
                if (m_isAllowCancel == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAllowCancel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAllowCancel = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAllowCancel;
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
        public string MemberPath
        {
            set
            {
                if (String.Equals(m_memberPath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMemberPath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_memberPath = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_memberPath;
            }
        }


        ///<summary>
        /// 
        ///</summary>
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DialogButton
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_text;
        public const string PropertyNameText = "Text";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isDefault;
        public const string PropertyNameIsDefault = "IsDefault";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isCancel;
        public const string PropertyNameIsCancel = "IsCancel";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
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


        ///<summary>
        /// 
        ///</summary>
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
        [DebuggerHidden]

        [Required]
        public bool IsCancel
        {
            set
            {
                if (m_isCancel == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsCancel, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isCancel = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isCancel;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class PartialView
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_memberPath;
        public const string PropertyNameMemberPath = "MemberPath";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


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
        public string MemberPath
        {
            set
            {
                if (String.Equals(m_memberPath, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameMemberPath, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_memberPath = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_memberPath;
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ChildView
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_partialView;
        public const string PropertyNamePartialView = "PartialView";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string PartialView
        {
            set
            {
                if (String.Equals(m_partialView, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePartialView, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_partialView = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_partialView;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class TabControl
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_partialView;
        public const string PropertyNamePartialView = "PartialView";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TabPanel> TabPanelCollection { get; } = new ObjectCollection<TabPanel>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string PartialView
        {
            set
            {
                if (String.Equals(m_partialView, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePartialView, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_partialView = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_partialView;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class TabPanel
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_header;
        public const string PropertyNameHeader = "Header";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_partialView;
        public const string PropertyNamePartialView = "PartialView";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_path;
        public const string PropertyNamePath = "Path";


        ///<summary>
        /// 
        ///</summary>
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
        [DebuggerHidden]

        [Required]
        public string PartialView
        {
            set
            {
                if (String.Equals(m_partialView, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePartialView, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_partialView = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_partialView;
            }
        }


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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class QueryEndpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_cacheProfile;
        public const string PropertyNameCacheProfile = "CacheProfile";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_route;
        public const string PropertyNameRoute = "Route";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_isReturnSource;
        public const string PropertyNameIsReturnSource = "IsReturnSource";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isPublished;
        public const string PropertyNameIsPublished = "IsPublished";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_resource;
        public const string PropertyNameResource = "Resource";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_cacheFilter;
        public const string PropertyNameCacheFilter = "CacheFilter";



        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Filter> FilterCollection { get; } = new ObjectCollection<Filter>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Sort> SortCollection { get; } = new ObjectCollection<Sort>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<string> MemberCollection { get; } = new ObjectCollection<string>();


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string CacheProfile
        {
            set
            {
                if (String.Equals(m_cacheProfile, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCacheProfile, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cacheProfile = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_cacheProfile;
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
        [DebuggerHidden]

        [Required]
        public string IsReturnSource
        {
            set
            {
                if (String.Equals(m_isReturnSource, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsReturnSource, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isReturnSource = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isReturnSource;
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
        [DebuggerHidden]

        [Required]
        public string Resource
        {
            set
            {
                if (String.Equals(m_resource, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameResource, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_resource = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_resource;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? CacheFilter
        {
            set
            {
                if (m_cacheFilter == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCacheFilter, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cacheFilter = value;
                    OnPropertyChanged();
                }
            }
            get { return m_cacheFilter; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ServiceContract
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FullSearchEndpoint m_fullSearchEndpoint
                = new FullSearchEndpoint();

        public const string PropertyNameFullSearchEndpoint = "FullSearchEndpoint";
        [DebuggerHidden]

        public FullSearchEndpoint FullSearchEndpoint
        {
            get { return m_fullSearchEndpoint; }
            set
            {
                m_fullSearchEndpoint = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OdataEndpoint m_odataEndpoint
                = new OdataEndpoint();

        public const string PropertyNameOdataEndpoint = "OdataEndpoint";
        [DebuggerHidden]

        public OdataEndpoint OdataEndpoint
        {
            get { return m_odataEndpoint; }
            set
            {
                m_odataEndpoint = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EntityResourceEndpoint m_entityResourceEndpoint
                = new EntityResourceEndpoint();

        public const string PropertyNameEntityResourceEndpoint = "EntityResourceEndpoint";
        [DebuggerHidden]

        public EntityResourceEndpoint EntityResourceEndpoint
        {
            get { return m_entityResourceEndpoint; }
            set
            {
                m_entityResourceEndpoint = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class QueryEndpointSetting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_cacheProfile;
        public const string PropertyNameCacheProfile = "CacheProfile";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_note;
        public const string PropertyNameNote = "Note";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_resource;
        public const string PropertyNameResource = "Resource";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_cacheFilter;
        public const string PropertyNameCacheFilter = "CacheFilter";



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
        private CachingSetting m_cachingSetting
                = new CachingSetting();

        public const string PropertyNameCachingSetting = "CachingSetting";
        [DebuggerHidden]

        public CachingSetting CachingSetting
        {
            get { return m_cachingSetting; }
            set
            {
                m_cachingSetting = value;
                OnPropertyChanged();
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string CacheProfile
        {
            set
            {
                if (String.Equals(m_cacheProfile, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCacheProfile, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cacheProfile = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_cacheProfile;
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
        public string Resource
        {
            set
            {
                if (String.Equals(m_resource, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameResource, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_resource = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_resource;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? CacheFilter
        {
            set
            {
                if (m_cacheFilter == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCacheFilter, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cacheFilter = value;
                    OnPropertyChanged();
                }
            }
            get { return m_cacheFilter; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class EntityResourceEndpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAllowed;
        public const string PropertyNameIsAllowed = "IsAllowed";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_filterExpression;
        public const string PropertyNameFilterExpression = "FilterExpression";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsAllowed
        {
            set
            {
                if (m_isAllowed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAllowed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAllowed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAllowed;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string FilterExpression
        {
            set
            {
                if (String.Equals(m_filterExpression, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFilterExpression, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_filterExpression = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_filterExpression;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FullSearchEndpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAllowed;
        public const string PropertyNameIsAllowed = "IsAllowed";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsAllowed
        {
            set
            {
                if (m_isAllowed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAllowed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAllowed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAllowed;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class OdataEndpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAllowed;
        public const string PropertyNameIsAllowed = "IsAllowed";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsAllowed
        {
            set
            {
                if (m_isAllowed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAllowed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAllowed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAllowed;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class BusinessRuleEndpoint
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isAllowed;
        public const string PropertyNameIsAllowed = "IsAllowed";


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
        [DebuggerHidden]

        [Required]
        public bool IsAllowed
        {
            set
            {
                if (m_isAllowed == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsAllowed, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isAllowed = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isAllowed;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ServiceContractSetting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ResourceEndpointSetting m_resourceEndpointSetting
                = new ResourceEndpointSetting();

        public const string PropertyNameResourceEndpointSetting = "ResourceEndpointSetting";
        [DebuggerHidden]

        public ResourceEndpointSetting ResourceEndpointSetting
        {
            get { return m_resourceEndpointSetting; }
            set
            {
                m_resourceEndpointSetting = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OdataEndpoint m_odataEndpoint
                = new OdataEndpoint();

        public const string PropertyNameOdataEndpoint = "OdataEndpoint";
        [DebuggerHidden]

        public OdataEndpoint OdataEndpoint
        {
            get { return m_odataEndpoint; }
            set
            {
                m_odataEndpoint = value;
                OnPropertyChanged();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SearchEndpointSetting m_searchEndpointSetting
                = new SearchEndpointSetting();

        public const string PropertyNameSearchEndpointSetting = "SearchEndpointSetting";
        [DebuggerHidden]

        public SearchEndpointSetting SearchEndpointSetting
        {
            get { return m_searchEndpointSetting; }
            set
            {
                m_searchEndpointSetting = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ResourceEndpointSetting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CachingSetting m_cachingSetting
                = new CachingSetting();

        public const string PropertyNameCachingSetting = "CachingSetting";
        [DebuggerHidden]

        public CachingSetting CachingSetting
        {
            get { return m_cachingSetting; }
            set
            {
                m_cachingSetting = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class SearchEndpointSetting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CachingSetting m_cachingSetting
                = new CachingSetting();

        public const string PropertyNameCachingSetting = "CachingSetting";
        [DebuggerHidden]

        public CachingSetting CachingSetting
        {
            get { return m_cachingSetting; }
            set
            {
                m_cachingSetting = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class OdataEndpointSetting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CachingSetting m_cachingSetting
                = new CachingSetting();

        public const string PropertyNameCachingSetting = "CachingSetting";
        [DebuggerHidden]

        public CachingSetting CachingSetting
        {
            get { return m_cachingSetting; }
            set
            {
                m_cachingSetting = value;
                OnPropertyChanged();
            }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class CachingSetting
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_cacheControl;
        public const string PropertyNameCacheControl = "CacheControl";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_noStore;
        public const string PropertyNameNoStore = "NoStore";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_expires;
        public const string PropertyNameExpires = "Expires";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string CacheControl
        {
            set
            {
                if (String.Equals(m_cacheControl, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameCacheControl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_cacheControl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_cacheControl;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool NoStore
        {
            set
            {
                if (m_noStore == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNoStore, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_noStore = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_noStore;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Expires
        {
            set
            {
                if (m_expires == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameExpires, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_expires = value;
                    OnPropertyChanged();
                }
            }
            get { return m_expires; }
        }


    }

    // placeholder for ReferencedAssembly
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


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entity;
        public const string PropertyNameEntity = "Entity";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_entityId;
        public const string PropertyNameEntityId = "EntityId";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_formatter;
        public const string PropertyNameFormatter = "Formatter";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TextFormatter m_textFormatter;
        public const string PropertyNameTextFormatter = "TextFormatter";



        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection { get; } = new ObjectCollection<ReferencedAssembly>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TextFieldMapping> FieldMappingCollection { get; } = new ObjectCollection<TextFieldMapping>();


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
        public string EntityId
        {
            set
            {
                if (String.Equals(m_entityId, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEntityId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_entityId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_entityId;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Formatter
        {
            set
            {
                if (String.Equals(m_formatter, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameFormatter, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_formatter = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_formatter;
            }
        }



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public TextFormatter TextFormatter
        {
            set
            {
                if (m_textFormatter == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameTextFormatter, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_textFormatter = value;
                    OnPropertyChanged();
                }
            }
            get { return m_textFormatter; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FolderReceiveLocation
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


        private string m_CredentialUserName;
        public string CredentialUserName
        {
            get
            {
                return m_CredentialUserName;
            }
            set
            {
                m_CredentialUserName = value;
                RaisePropertyChanged();
            }
        }


        private bool m_ChangeExtension;
        public bool ChangeExtension
        {
            get
            {
                return m_ChangeExtension;
            }
            set
            {
                m_ChangeExtension = value;
                RaisePropertyChanged();
            }
        }


        private string m_CredentialPassword;
        public string CredentialPassword
        {
            get
            {
                return m_CredentialPassword;
            }
            set
            {
                m_CredentialPassword = value;
                RaisePropertyChanged();
            }
        }


        private string m_Filter;
        public string Filter
        {
            get
            {
                return m_Filter;
            }
            set
            {
                m_Filter = value;
                RaisePropertyChanged();
            }
        }


        private string m_ArchiveLocation;
        public string ArchiveLocation
        {
            get
            {
                return m_ArchiveLocation;
            }
            set
            {
                m_ArchiveLocation = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FixedLengthTextFormatter
    {

        private string m_RecordTag;
        public string RecordTag
        {
            get
            {
                return m_RecordTag;
            }
            set
            {
                m_RecordTag = value;
                RaisePropertyChanged();
            }
        }


        private bool m_HasTagIdentifier;
        public bool HasTagIdentifier
        {
            get
            {
                return m_HasTagIdentifier;
            }
            set
            {
                m_HasTagIdentifier = value;
                RaisePropertyChanged();
            }
        }


        private bool m_HasLabel;
        public bool HasLabel
        {
            get
            {
                return m_HasLabel;
            }
            set
            {
                m_HasLabel = value;
                RaisePropertyChanged();
            }
        }


        private string m_IgnoreTag;
        public string IgnoreTag
        {
            get
            {
                return m_IgnoreTag;
            }
            set
            {
                m_IgnoreTag = value;
                RaisePropertyChanged();
            }
        }


        private bool m_AllowLessChars;
        public bool AllowLessChars
        {
            get
            {
                return m_AllowLessChars;
            }
            set
            {
                m_AllowLessChars = value;
                RaisePropertyChanged();
            }
        }


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FixedLengthTextFieldMapping> FieldMappingCollection { get; } = new ObjectCollection<FixedLengthTextFieldMapping>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DelimitedTextFormatter
    {

        private string m_Delimiter;
        public string Delimiter
        {
            get
            {
                return m_Delimiter;
            }
            set
            {
                m_Delimiter = value;
                RaisePropertyChanged();
            }
        }


        private bool m_HasTagIdentifier;
        public bool HasTagIdentifier
        {
            get
            {
                return m_HasTagIdentifier;
            }
            set
            {
                m_HasTagIdentifier = value;
                RaisePropertyChanged();
            }
        }


        private string m_RecordTag;
        public string RecordTag
        {
            get
            {
                return m_RecordTag;
            }
            set
            {
                m_RecordTag = value;
                RaisePropertyChanged();
            }
        }


        private string m_EscapeCharacter;
        public string EscapeCharacter
        {
            get
            {
                return m_EscapeCharacter;
            }
            set
            {
                m_EscapeCharacter = value;
                RaisePropertyChanged();
            }
        }


        private bool m_HasLabel;
        public bool HasLabel
        {
            get
            {
                return m_HasLabel;
            }
            set
            {
                m_HasLabel = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class SqlServerRecordSetFormatter
    {

        private int m_RecordReaderIndex;
        public int RecordReaderIndex
        {
            get
            {
                return m_RecordReaderIndex;
            }
            set
            {
                m_RecordReaderIndex = value;
                RaisePropertyChanged();
            }
        }


        private string m_Server;
        public string Server
        {
            get
            {
                return m_Server;
            }
            set
            {
                m_Server = value;
                RaisePropertyChanged();
            }
        }


        private string m_Database;
        public string Database
        {
            get
            {
                return m_Database;
            }
            set
            {
                m_Database = value;
                RaisePropertyChanged();
            }
        }


        private bool m_Trusted;
        public bool Trusted
        {
            get
            {
                return m_Trusted;
            }
            set
            {
                m_Trusted = value;
                RaisePropertyChanged();
            }
        }


        private string m_UserId;
        public string UserId
        {
            get
            {
                return m_UserId;
            }
            set
            {
                m_UserId = value;
                RaisePropertyChanged();
            }
        }


        private string m_Password;
        public string Password
        {
            get
            {
                return m_Password;
            }
            set
            {
                m_Password = value;
                RaisePropertyChanged();
            }
        }


        private string m_Query;
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


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<SqlServerRecordSet> SqlServerRecordSetCollection { get; } = new ObjectCollection<SqlServerRecordSet>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class JsonTextFormatter
    {

        private string m_SchemaStoreId;
        public string SchemaStoreId
        {
            get
            {
                return m_SchemaStoreId;
            }
            set
            {
                m_SchemaStoreId = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class XmlTextFormatter
    {

        private string m_Namespace;
        public string Namespace
        {
            get
            {
                return m_Namespace;
            }
            set
            {
                m_Namespace = value;
                RaisePropertyChanged();
            }
        }


        private string m_XmlSchemaStoreId;
        public string XmlSchemaStoreId
        {
            get
            {
                return m_XmlSchemaStoreId;
            }
            set
            {
                m_XmlSchemaStoreId = value;
                RaisePropertyChanged();
            }
        }


        private string m_RootPath;
        public string RootPath
        {
            get
            {
                return m_RootPath;
            }
            set
            {
                m_RootPath = value;
                RaisePropertyChanged();
            }
        }


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<XmlAttributeTextFieldMapping> ParentAttributeValueCollection { get; } = new ObjectCollection<XmlAttributeTextFieldMapping>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<XmlElementTextFieldMapping> ParentElementValueCollection { get; } = new ObjectCollection<XmlElementTextFieldMapping>();



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DelimitedTextFieldMapping
    {

        private int m_Column;
        public int Column
        {
            get
            {
                return m_Column;
            }
            set
            {
                m_Column = value;
                RaisePropertyChanged();
            }
        }


        private bool m_AllowMissing;
        public bool AllowMissing
        {
            get
            {
                return m_AllowMissing;
            }
            set
            {
                m_AllowMissing = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FixedLengthTextFieldMapping
    {

        private int m_Column;
        public int Column
        {
            get
            {
                return m_Column;
            }
            set
            {
                m_Column = value;
                RaisePropertyChanged();
            }
        }


        private bool m_AllowMissing;
        public bool AllowMissing
        {
            get
            {
                return m_AllowMissing;
            }
            set
            {
                m_AllowMissing = value;
                RaisePropertyChanged();
            }
        }


        private int m_Start;
        public int Start
        {
            get
            {
                return m_Start;
            }
            set
            {
                m_Start = value;
                RaisePropertyChanged();
            }
        }


        private int m_Length;
        public int Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                m_Length = value;
                RaisePropertyChanged();
            }
        }


        private string m_TrimMode;
        public string TrimMode
        {
            get
            {
                return m_TrimMode;
            }
            set
            {
                m_TrimMode = value;
                RaisePropertyChanged();
            }
        }


        private int m_Order;
        public int Order
        {
            get
            {
                return m_Order;
            }
            set
            {
                m_Order = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class FlatFileDetailTag
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_parent;
        public const string PropertyNameParent = "Parent";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_rowTag;
        public const string PropertyNameRowTag = "RowTag";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FlatFileDetailTag> DetailRowCollection { get; } = new ObjectCollection<FlatFileDetailTag>();


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
        public string Parent
        {
            set
            {
                if (String.Equals(m_parent, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameParent, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_parent = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_parent;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string RowTag
        {
            set
            {
                if (String.Equals(m_rowTag, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameRowTag, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_rowTag = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_rowTag;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class HeaderFieldMapping
    {

        private string m_Header;
        public string Header
        {
            get
            {
                return m_Header;
            }
            set
            {
                m_Header = value;
                RaisePropertyChanged();
            }
        }


        private string m_Pattern;
        public string Pattern
        {
            get
            {
                return m_Pattern;
            }
            set
            {
                m_Pattern = value;
                RaisePropertyChanged();
            }
        }


        public int? StartPosition { get; set; }

        public int? Length { get; set; }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class UriFieldMapping
    {

        private string m_Uri;
        public string Uri
        {
            get
            {
                return m_Uri;
            }
            set
            {
                m_Uri = value;
                RaisePropertyChanged();
            }
        }


        private string m_Pattern;
        public string Pattern
        {
            get
            {
                return m_Pattern;
            }
            set
            {
                m_Pattern = value;
                RaisePropertyChanged();
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class SqlServerRecordSet
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fieldName;
        public const string PropertyNameFieldName = "FieldName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_sqlServerDataType;
        public const string PropertyNameSqlServerDataType = "SqlServerDataType";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_sampleValue;
        public const string PropertyNameSampleValue = "SampleValue";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isNullable;
        public const string PropertyNameIsNullable = "IsNullable";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
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
        [DebuggerHidden]

        [Required]
        public string SqlServerDataType
        {
            set
            {
                if (String.Equals(m_sqlServerDataType, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSqlServerDataType, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sqlServerDataType = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sqlServerDataType;
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
        public string SampleValue
        {
            set
            {
                if (String.Equals(m_sampleValue, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSampleValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sampleValue = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sampleValue;
            }
        }


        ///<summary>
        /// 
        ///</summary>
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class XmlAttributeTextFieldMapping
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



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class XmlElementTextFieldMapping
    {

        private bool m_ContainsText;
        public bool ContainsText
        {
            get
            {
                return m_ContainsText;
            }
            set
            {
                m_ContainsText = value;
                RaisePropertyChanged();
            }
        }



    }


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



    public partial class Member
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_allowMultiple;
        public const string PropertyNameAllowMultiple = "AllowMultiple";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private Field m_defaultValue;
        public const string PropertyNameDefaultValue = "DefaultValue";

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FieldPermission> FieldPermissionCollection { get; } = new ObjectCollection<FieldPermission>();



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



        public bool AllowMultiple
        {
            set
            {
                if (m_allowMultiple == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowMultiple, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_allowMultiple = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_allowMultiple;
            }
        }


        ///<summary>
        /// 
        ///</summary>
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


    // placeholder for Field complext type


    public partial class ReceiveLocation
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isActive;
        public const string PropertyNameIsActive = "IsActive";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_validationEndpoint;
        public const string PropertyNameValidationEndpoint = "ValidationEndpoint";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_validationMethod;
        public const string PropertyNameValidationMethod = "ValidationMethod";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_submitEndpoint;
        public const string PropertyNameSubmitEndpoint = "SubmitEndpoint";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_submitMethod;
        public const string PropertyNameSubmitMethod = "SubmitMethod";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_invalidEndpoint;
        public const string PropertyNameInvalidEndpoint = "InvalidEndpoint";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_invalidMethod;
        public const string PropertyNameInvalidMethod = "InvalidMethod";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_errorEndpoint;
        public const string PropertyNameErrorEndpoint = "ErrorEndpoint";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_errorMethod;
        public const string PropertyNameErrorMethod = "ErrorMethod";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_jwtToken;
        public const string PropertyNameJwtToken = "JwtToken";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_receivePort;
        public const string PropertyNameReceivePort = "ReceivePort";

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection { get; } = new ObjectCollection<ReferencedAssembly>();



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



        public string ValidationEndpoint
        {
            set
            {
                if (m_validationEndpoint == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValidationEndpoint, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_validationEndpoint = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_validationEndpoint;
            }
        }



        public string ValidationMethod
        {
            set
            {
                if (m_validationMethod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameValidationMethod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_validationMethod = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_validationMethod;
            }
        }



        public string SubmitEndpoint
        {
            set
            {
                if (m_submitEndpoint == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubmitEndpoint, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_submitEndpoint = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_submitEndpoint;
            }
        }



        public string SubmitMethod
        {
            set
            {
                if (m_submitMethod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSubmitMethod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_submitMethod = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_submitMethod;
            }
        }



        public string InvalidEndpoint
        {
            set
            {
                if (m_invalidEndpoint == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInvalidEndpoint, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_invalidEndpoint = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_invalidEndpoint;
            }
        }



        public string InvalidMethod
        {
            set
            {
                if (m_invalidMethod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInvalidMethod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_invalidMethod = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_invalidMethod;
            }
        }



        public string ErrorEndpoint
        {
            set
            {
                if (m_errorEndpoint == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameErrorEndpoint, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_errorEndpoint = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_errorEndpoint;
            }
        }



        public string ErrorMethod
        {
            set
            {
                if (m_errorMethod == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameErrorMethod, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_errorMethod = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_errorMethod;
            }
        }



        public string JwtToken
        {
            set
            {
                if (m_jwtToken == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameJwtToken, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_jwtToken = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_jwtToken;
            }
        }



        public string ReceivePort
        {
            set
            {
                if (m_receivePort == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameReceivePort, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_receivePort = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_receivePort;
            }
        }



    }



    public partial class TextFormatter
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_name;
        public const string PropertyNameName = "Name";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_sampleStoreId;
        public const string PropertyNameSampleStoreId = "SampleStoreId";

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<FlatFileDetailTag> DetailRowCollection { get; } = new ObjectCollection<FlatFileDetailTag>();



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



        public string SampleStoreId
        {
            set
            {
                if (m_sampleStoreId == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSampleStoreId, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sampleStoreId = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sampleStoreId;
            }
        }



    }



    public partial class TextFieldMapping
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

        private string m_converter;
        public const string PropertyNameConverter = "Converter";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_sampleValue;
        public const string PropertyNameSampleValue = "SampleValue";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_isComplex;
        public const string PropertyNameIsComplex = "IsComplex";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_allowMultiple;
        public const string PropertyNameAllowMultiple = "AllowMultiple";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_nullPlaceholder;
        public const string PropertyNameNullPlaceholder = "NullPlaceholder";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private bool m_ignore;
        public const string PropertyNameIgnore = "Ignore";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_comment;
        public const string PropertyNameComment = "Comment";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private string m_defaultValueString;
        public const string PropertyNameDefaultValueString = "DefaultValueString";

        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<TextFieldMapping> FieldMappingCollection { get; } = new ObjectCollection<TextFieldMapping>();



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



        public string Converter
        {
            set
            {
                if (m_converter == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameConverter, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_converter = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_converter;
            }
        }



        public string SampleValue
        {
            set
            {
                if (m_sampleValue == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameSampleValue, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_sampleValue = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_sampleValue;
            }
        }



        public bool IsComplex
        {
            set
            {
                if (m_isComplex == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsComplex, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isComplex = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isComplex;
            }
        }



        public bool AllowMultiple
        {
            set
            {
                if (m_allowMultiple == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAllowMultiple, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_allowMultiple = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_allowMultiple;
            }
        }



        public string NullPlaceholder
        {
            set
            {
                if (m_nullPlaceholder == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameNullPlaceholder, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_nullPlaceholder = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_nullPlaceholder;
            }
        }



        public bool Ignore
        {
            set
            {
                if (m_ignore == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIgnore, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_ignore = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_ignore;
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



        public string DefaultValueString
        {
            set
            {
                if (m_defaultValueString == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameDefaultValueString, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_defaultValueString = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_defaultValueString;
            }
        }



    }


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

