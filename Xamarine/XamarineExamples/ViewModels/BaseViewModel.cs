using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsight.Models.Comments;
using System.Collections.ObjectModel;

namespace ProjectInsightMobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
       // public IDataStore<MyWorkItem> DataStore => DependencyService.Get<IDataStore<MyWorkItem>>() ?? new MockDataStore();

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel()
        {
            //PropertyChanged += (sender, args) => OnPropertyChanged("Title");

        }
        bool isBusy = false;
        public bool IsBusy
        {
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
            get
            {
                return isBusy;
            }
        }

        bool isVisibleLoad;
        public bool VisibleLoad
        {
            set
            {
                if (isVisibleLoad != value)
                {
                    isVisibleLoad = value;
                    OnPropertyChanged("VisibleLoad");
                    OnPropertyChanged("VisibleContent");
                }
            }
            get
            {
                return isVisibleLoad;
            }
        }
        public bool VisibleContent
        {
            get
            {
                return !isVisibleLoad;
            }
        }

        string loadingMessage;
        public string LoadingMessage
        {
            set
            {
                if (loadingMessage != value)
                {
                    loadingMessage = value;
                    OnPropertyChanged("LoadingMessage");
                }
            }
            get
            {
                return loadingMessage;
            }
        }

        string title = string.Empty;
        public string Title
        {
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
            get
            {
                return title;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {

            }
        }
    }

}