using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace ComBrowser
{
    internal sealed class Browser : DependencyObject, INotifyPropertyChanged
    {
        private int numberOfLoadingThreads;
        private RegistryView registryView = RegistryView.Registry32;


        public Browser()
        {
            ServerCollection = new ServerCollection(this);
            InterfaceCollection = new InterfaceCollection(this);
            TypeLibraryCollection = new TypeLibraryCollection(this);

            BeginRefresh();
        }

        public void BeginRefresh()
        {
            if (Interlocked.Increment(ref numberOfLoadingThreads) == 1)
                IsLoading = true;

            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    Server[] servers;
                    Interface[] interfaces;
                    TypeLibrary[] typeLibraries;

                    using (var classesRootKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView))
                    {
                        using (var clsidKey = classesRootKey.OpenSubKey("CLSID"))
                        {
                            servers = clsidKey.GetSubKeyNames()
                                //.AsParallel()
                                .Select(guid => new Server(clsidKey, guid))
                                //.Take(20)
                                .ToArray();
                        }

                        using (var typeLibKey = classesRootKey.OpenSubKey("TypeLib"))
                        {
                            typeLibraries = typeLibKey.GetSubKeyNames()
                                //.AsParallel()
                                .SelectMany(guid => TypeLibrary.GetTypeLibrarys(typeLibKey, guid))
                                //.Take(20)
                                .ToArray();
                        }

                        using (var interfaceKey = classesRootKey.OpenSubKey("Interface"))
                        {
                            var tlbLookup = typeLibraries.ToDictionary(t => t.Key);

                            interfaces = interfaceKey.GetSubKeyNames()
                                //.AsParallel()
                                .Select(guid => new Interface(interfaceKey, guid, tlbLookup))
                                //.Take(20)
                                .ToArray();
                        }
                    }

                    Dispatcher.Invoke((Action)delegate
                    {
                        ServerCollection.Items = servers;
                        InterfaceCollection.Items = interfaces;
                        TypeLibraryCollection.Items = typeLibraries;

                        if (Interlocked.Decrement(ref numberOfLoadingThreads) == 0)
                            IsLoading = false;
                    });
                });
        }


        public ServerCollection ServerCollection
        {
            get { return (ServerCollection)GetValue(ServerCollectionProperty); }
            set { SetValue(ServerCollectionProperty, value); }
        }
        /// <summary>
        /// Identifies the ServerCollection dependency property
        /// </summary>
        public static readonly DependencyProperty ServerCollectionProperty =
            DependencyProperty.Register("ServerCollection", typeof(ServerCollection), typeof(Browser));


        public InterfaceCollection InterfaceCollection
        {
            get { return (InterfaceCollection)GetValue(InterfaceCollectionProperty); }
            set { SetValue(InterfaceCollectionProperty, value); }
        }
        /// <summary>
        /// Identifies the InterfaceCollection dependency property
        /// </summary>
        public static readonly DependencyProperty InterfaceCollectionProperty =
            DependencyProperty.Register("InterfaceCollection", typeof(InterfaceCollection), typeof(Browser));


        public TypeLibraryCollection TypeLibraryCollection
        {
            get { return (TypeLibraryCollection)GetValue(TypeLibraryCollectionProperty); }
            set { SetValue(TypeLibraryCollectionProperty, value); }
        }
        /// <summary>
        /// Identifies the TypeLibraryCollection dependency property
        /// </summary>
        public static readonly DependencyProperty TypeLibraryCollectionProperty =
            DependencyProperty.Register("TypeLibraryCollection", typeof(TypeLibraryCollection), typeof(Browser));


        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        /// <summary>
        /// Identifies the IsLoading dependency property
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(Browser));


        public RegistryView RegistryView
        {
            get
            {
                return registryView;
            }
            set
            {
                if (registryView != value)
                {
                    registryView = value;
                    OnPropertyChanged("RegistryView");
                    BeginRefresh();
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public object Dictionary { get; set; }
    }
}
