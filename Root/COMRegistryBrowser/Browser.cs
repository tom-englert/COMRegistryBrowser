using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace COMRegistryBrowser
{
    internal sealed class Browser : DependencyObject, INotifyPropertyChanged
    {
        private int numberOfLoadingThreads;
        private RegistryView registryView = RegistryView.Registry32;


        public Browser()
        {
            ServerCollection = new MasterServerCollection(this);
            InterfaceCollection = new MasterInterfaceCollection(this);
            TypeLibraryCollection = new MasterTypeLibraryCollection(this);

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
                        servers = Server.GetServers(classesRootKey);
                        typeLibraries = TypeLibrary.GetTypeLibrarys(classesRootKey);
                        interfaces = Interface.GetInterfaces(classesRootKey, servers, typeLibraries);
                    }

                    Dispatcher.Invoke((Action)delegate
                    {
                        ServerCollection.Items = new ObservableCollection<Server>(servers);
                        InterfaceCollection.Items = new ObservableCollection<Interface>(interfaces);
                        TypeLibraryCollection.Items = new ObservableCollection<TypeLibrary>(typeLibraries);

                        if (Interlocked.Decrement(ref numberOfLoadingThreads) == 0)
                            IsLoading = false;
                    });
                });
        }

        public void RemoveEntries(HashSet<RegistryEntry> itemsToRemove)
        {
            using (var classesRootKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView))
            {
                foreach (var item in itemsToRemove)
                {
                    item.Remove(classesRootKey);

                    if (!MasterCollections.Any(collection => collection.Remove(item)))
                    {
                        Trace.Fail("Removing an item that was not part of any master collection.");
                    }
                }
            }
        }

        public MasterServerCollection ServerCollection
        {
            get { return (MasterServerCollection)GetValue(ServerCollectionProperty); }
            set { SetValue(ServerCollectionProperty, value); }
        }
        /// <summary>
        /// Identifies the ServerCollection dependency property
        /// </summary>
        public static readonly DependencyProperty ServerCollectionProperty =
            DependencyProperty.Register("ServerCollection", typeof(MasterServerCollection), typeof(Browser));


        public MasterInterfaceCollection InterfaceCollection
        {
            get { return (MasterInterfaceCollection)GetValue(InterfaceCollectionProperty); }
            set { SetValue(InterfaceCollectionProperty, value); }
        }
        /// <summary>
        /// Identifies the InterfaceCollection dependency property
        /// </summary>
        public static readonly DependencyProperty InterfaceCollectionProperty =
            DependencyProperty.Register("InterfaceCollection", typeof(MasterInterfaceCollection), typeof(Browser));


        public MasterTypeLibraryCollection TypeLibraryCollection
        {
            get { return (MasterTypeLibraryCollection)GetValue(TypeLibraryCollectionProperty); }
            set { SetValue(TypeLibraryCollectionProperty, value); }
        }
        /// <summary>
        /// Identifies the TypeLibraryCollection dependency property
        /// </summary>
        public static readonly DependencyProperty TypeLibraryCollectionProperty =
            DependencyProperty.Register("TypeLibraryCollection", typeof(MasterTypeLibraryCollection), typeof(Browser));


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


        private IEnumerable<IItemsCollection> MasterCollections
        {
            get
            {
                yield return ServerCollection;
                yield return InterfaceCollection;
                yield return TypeLibraryCollection;
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
