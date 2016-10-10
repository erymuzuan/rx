
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
    public partial class WorkersConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_description;
        public const string PropertyNameDescription = "Description";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isEnabled;
        public const string PropertyNameIsEnabled = "IsEnabled";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_environment;
        public const string PropertyNameEnvironment = "Environment";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_homeDirectory;
        public const string PropertyNameHomeDirectory = "HomeDirectory";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<SubscriberConfig> SubscriberConfigs { get; } = new ObjectCollection<SubscriberConfig>();


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
        public string Environment
        {
            set
            {
                if (String.Equals(m_environment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnvironment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_environment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_environment;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string HomeDirectory
        {
            set
            {
                if (String.Equals(m_homeDirectory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHomeDirectory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_homeDirectory = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_homeDirectory;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class SubscriberConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_queueName;
        public const string PropertyNameQueueName = "QueueName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_fullName;
        public const string PropertyNameFullName = "FullName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_assembly;
        public const string PropertyNameAssembly = "Assembly";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_typeName;
        public const string PropertyNameTypeName = "TypeName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_instancesCount;
        public const string PropertyNameInstancesCount = "InstancesCount";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_prefetchCount;
        public const string PropertyNamePrefetchCount = "PrefetchCount";



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? m_priority;
        public const string PropertyNamePriority = "Priority";



        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string QueueName
        {
            set
            {
                if (String.Equals(m_queueName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameQueueName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_queueName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_queueName;
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
        public string Assembly
        {
            set
            {
                if (String.Equals(m_assembly, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameAssembly, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_assembly = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_assembly;
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

        public int? InstancesCount
        {
            set
            {
                if (m_instancesCount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameInstancesCount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_instancesCount = value;
                    OnPropertyChanged();
                }
            }
            get { return m_instancesCount; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? PrefetchCount
        {
            set
            {
                if (m_prefetchCount == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePrefetchCount, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_prefetchCount = value;
                    OnPropertyChanged();
                }
            }
            get { return m_prefetchCount; }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        public int? Priority
        {
            set
            {
                if (m_priority == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePriority, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_priority = value;
                    OnPropertyChanged();
                }
            }
            get { return m_priority; }
        }


    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class WebServerConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_environment;
        public const string PropertyNameEnvironment = "Environment";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_computerName;
        public const string PropertyNameComputerName = "ComputerName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_homeDirectory;
        public const string PropertyNameHomeDirectory = "HomeDirectory";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_applicationPool;
        public const string PropertyNameApplicationPool = "ApplicationPool";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_applicationPoolCredential;
        public const string PropertyNameApplicationPoolCredential = "ApplicationPoolCredential";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isConsole;
        public const string PropertyNameIsConsole = "IsConsole";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_enableRemoteManagement;
        public const string PropertyNameEnableRemoteManagement = "EnableRemoteManagement";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_hostNameBinding;
        public const string PropertyNameHostNameBinding = "HostNameBinding";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_portBinding;
        public const string PropertyNamePortBinding = "PortBinding";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_useSsl;
        public const string PropertyNameUseSsl = "UseSsl";


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
        public string Environment
        {
            set
            {
                if (String.Equals(m_environment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnvironment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_environment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_environment;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ComputerName
        {
            set
            {
                if (String.Equals(m_computerName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComputerName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_computerName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_computerName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string HomeDirectory
        {
            set
            {
                if (String.Equals(m_homeDirectory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHomeDirectory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_homeDirectory = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_homeDirectory;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ApplicationPool
        {
            set
            {
                if (String.Equals(m_applicationPool, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApplicationPool, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_applicationPool = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_applicationPool;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ApplicationPoolCredential
        {
            set
            {
                if (String.Equals(m_applicationPoolCredential, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApplicationPoolCredential, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_applicationPoolCredential = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_applicationPoolCredential;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsConsole
        {
            set
            {
                if (m_isConsole == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsConsole, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isConsole = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isConsole;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool EnableRemoteManagement
        {
            set
            {
                if (m_enableRemoteManagement == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnableRemoteManagement, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_enableRemoteManagement = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_enableRemoteManagement;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string HostNameBinding
        {
            set
            {
                if (String.Equals(m_hostNameBinding, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHostNameBinding, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_hostNameBinding = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_hostNameBinding;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public int PortBinding
        {
            set
            {
                if (m_portBinding == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePortBinding, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_portBinding = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_portBinding;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool UseSsl
        {
            set
            {
                if (m_useSsl == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUseSsl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_useSsl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_useSsl;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class WorkerServerConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_environment;
        public const string PropertyNameEnvironment = "Environment";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_computerName;
        public const string PropertyNameComputerName = "ComputerName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_homeDirectory;
        public const string PropertyNameHomeDirectory = "HomeDirectory";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_applicationPool;
        public const string PropertyNameApplicationPool = "ApplicationPool";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_applicationPoolCredential;
        public const string PropertyNameApplicationPoolCredential = "ApplicationPoolCredential";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isConsole;
        public const string PropertyNameIsConsole = "IsConsole";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_enableRemoteManagement;
        public const string PropertyNameEnableRemoteManagement = "EnableRemoteManagement";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_hostNameBinding;
        public const string PropertyNameHostNameBinding = "HostNameBinding";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_portBinding;
        public const string PropertyNamePortBinding = "PortBinding";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_useSsl;
        public const string PropertyNameUseSsl = "UseSsl";


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
        public string Environment
        {
            set
            {
                if (String.Equals(m_environment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnvironment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_environment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_environment;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ComputerName
        {
            set
            {
                if (String.Equals(m_computerName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComputerName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_computerName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_computerName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string HomeDirectory
        {
            set
            {
                if (String.Equals(m_homeDirectory, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHomeDirectory, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_homeDirectory = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_homeDirectory;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ApplicationPool
        {
            set
            {
                if (String.Equals(m_applicationPool, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApplicationPool, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_applicationPool = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_applicationPool;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ApplicationPoolCredential
        {
            set
            {
                if (String.Equals(m_applicationPoolCredential, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameApplicationPoolCredential, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_applicationPoolCredential = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_applicationPoolCredential;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool IsConsole
        {
            set
            {
                if (m_isConsole == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameIsConsole, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_isConsole = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_isConsole;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool EnableRemoteManagement
        {
            set
            {
                if (m_enableRemoteManagement == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnableRemoteManagement, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_enableRemoteManagement = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_enableRemoteManagement;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string HostNameBinding
        {
            set
            {
                if (String.Equals(m_hostNameBinding, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameHostNameBinding, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_hostNameBinding = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_hostNameBinding;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public int PortBinding
        {
            set
            {
                if (m_portBinding == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePortBinding, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_portBinding = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_portBinding;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public bool UseSsl
        {
            set
            {
                if (m_useSsl == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUseSsl, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_useSsl = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_useSsl;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class BrokerConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_computerName;
        public const string PropertyNameComputerName = "ComputerName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_userName;
        public const string PropertyNameUserName = "UserName";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_password;
        public const string PropertyNamePassword = "Password";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_environment;
        public const string PropertyNameEnvironment = "Environment";


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
        public string ComputerName
        {
            set
            {
                if (String.Equals(m_computerName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComputerName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_computerName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_computerName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string UserName
        {
            set
            {
                if (String.Equals(m_userName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameUserName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_userName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_userName;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Password
        {
            set
            {
                if (String.Equals(m_password, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePassword, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_password = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_password;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Environment
        {
            set
            {
                if (String.Equals(m_environment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnvironment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_environment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_environment;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ElasticsearchConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_environment;
        public const string PropertyNameEnvironment = "Environment";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_port;
        public const string PropertyNamePort = "Port";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_computerName;
        public const string PropertyNameComputerName = "ComputerName";


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
        public string Environment
        {
            set
            {
                if (String.Equals(m_environment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnvironment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_environment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_environment;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public int Port
        {
            set
            {
                if (m_port == value) return;
                var arg = new PropertyChangingEventArgs(PropertyNamePort, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_port = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_port;
            }
        }


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string ComputerName
        {
            set
            {
                if (String.Equals(m_computerName, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameComputerName, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_computerName = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_computerName;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class DscConfig
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_name;
        public const string PropertyNameName = "Name";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_environment;
        public const string PropertyNameEnvironment = "Environment";


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ConfigData> Configs { get; } = new ObjectCollection<ConfigData>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<WebServerConfig> WebServers { get; } = new ObjectCollection<WebServerConfig>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<WorkerServerConfig> WorkerServers { get; } = new ObjectCollection<WorkerServerConfig>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<ElasticsearchConfig> ElasticsearchServers { get; } = new ObjectCollection<ElasticsearchConfig>();


        ///<summary>
        /// 
        ///</summary>
        public ObjectCollection<BrokerConfig> BrokerServers { get; } = new ObjectCollection<BrokerConfig>();


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
        public string Environment
        {
            set
            {
                if (String.Equals(m_environment, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameEnvironment, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_environment = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_environment;
            }
        }



    }

    ///<summary>
    /// 
    ///</summary>
    [DataObject(true)]
    [Serializable]
    public partial class ConfigData
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_key;
        public const string PropertyNameKey = "Key";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_value;
        public const string PropertyNameValue = "Value";


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isRequired;
        public const string PropertyNameIsRequired = "IsRequired";


        ///<summary>
        /// 
        ///</summary>
        [DebuggerHidden]

        [Required]
        public string Key
        {
            set
            {
                if (String.Equals(m_key, value, StringComparison.Ordinal)) return;
                var arg = new PropertyChangingEventArgs(PropertyNameKey, value);
                OnPropertyChanging(arg);
                if (!arg.Cancel)
                {
                    m_key = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return m_key;
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



    }

}
// ReSharper restore InconsistentNaming

