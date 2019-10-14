using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SafeSportChat.Models
{
    public class Country
    {
        public string PhonePrefix { get; set; }
        public string CountryShortName { get; set; }
        public string CountryStringsConcatinated
        {
            get { return CountryShortName + PhonePrefix; }
            set { CountryStringsConcatinated = value; }
        }
    }

    public class CountryList
    {
        public ObservableCollection<Country> Countries { get; set; }
        public CountryList()
        {
            Countries = new ObservableCollection<Country>()
            {
                new Country { PhonePrefix = "+389", CountryShortName = "MK"},
                new Country { PhonePrefix = "+1", CountryShortName = "US" },
                new Country { PhonePrefix = "+850", CountryShortName = "KP"}
            };
        }

    }
}
