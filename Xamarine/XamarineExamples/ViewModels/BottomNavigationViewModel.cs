using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.ViewModels
{
    public class BottomNavigationViewModel : BaseViewModel
    {

        int numberWorkListItems;
        public int NumberWorkListItems
        {
            set
            {
                if (numberWorkListItems != value)
                {
                    numberWorkListItems = value;

                    OnPropertyChanged("NumberWorkListItems");
                    OnPropertyChanged("IsWorkListItemsCountVisible");
                }
            }
            get
            {
                return numberWorkListItems;
            }
        }

        int numberProjectItems;
        public int NumberProjectItems
        {
            set
            {
                if (numberProjectItems != value)
                {
                    numberProjectItems = value;

                    OnPropertyChanged("NumberProjectItems");
                    OnPropertyChanged("IsNumberProjectCountVisible");
                }
            }
            get
            {
                return numberProjectItems;
            }
        }

        string numberNottificationItems;
        public string NumberNottificationItems
        {
            set
            {
                if (numberNottificationItems != value)
                {
                    numberNottificationItems = value;

                    OnPropertyChanged("NumberNottificationItems");
                    OnPropertyChanged("IsNotificationCountVisible");

                }
            }
            get
            {
                return numberNottificationItems;
            }
        }

        public BottomNavigationViewModel()
        {
            numberWorkListItems = 0;
            numberProjectItems = 0;
            numberNottificationItems = string.Empty;
            activeIcon = 1;
        }

        
        public bool IsNotificationCountVisible
        {
            
            get
            {
                return (!string.IsNullOrEmpty(NumberNottificationItems) && NumberNottificationItems != "0");
            }
        }

        public bool IsNumberProjectCountVisible
        {
            get
            {
                return (NumberProjectItems > 0);
            }
        }
        public bool IsWorkListItemsCountVisible
        {
            get
            {
                return (NumberWorkListItems > 0);
            }
        }

        int activeIcon;
        public int ActiveIcon
        {
            set
            {
                if (activeIcon != value)
                {
                    activeIcon = value;

                    OnPropertyChanged("ActiveIcon");
                }
            }
            get
            {
                return activeIcon;
            }
        }
    }
}                     
