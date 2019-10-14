using ProjectInsight.Models.Comments;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        public string Id { get; set; }

        public string firstName;
        public string FirstName
        {
            set
            {
                if (firstName != value)
                {
                    firstName = value;

                    OnPropertyChanged("FirstName");
                }
            }
            get
            {
                return firstName;
            }
        }

        public string lastName;
        public string LastName
        {
            set
            {
                if (lastName != value)
                {
                    lastName = value;

                    OnPropertyChanged("LastName");
                }
            }
            get
            {
                return lastName;
            }
        }

        public string name;
        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;

                    OnPropertyChanged("Name");
                }
            }
            get
            {
                return name;
            }
        }

        string email;
        public string Email
        {
            set
            {
                if (email != value)
                {
                    email = value;

                    OnPropertyChanged("Email");
                }
            }
            get
            {
                return email;
            }
        }
        string company;
        public string Company
        {
            set
            {
                if (company != value)
                {
                    company = value;

                    OnPropertyChanged("Company");
                }
            }
            get
            {
                return company;
            }
        }
        public string Icon { get; set; }
        public string JobTitle { get; set; }
        public DateTime CreatedDate { get; set; }

        string address1;
        public string Address1
        {
            set
            {
                if (address1 != value)
                {
                    address1 = value;

                    OnPropertyChanged("Address1");
                }
            }
            get
            {
                return address1;
            }
        }

        string address2;
        public string Address2
        {
            set
            {
                if (address2 != value)
                {
                    address2 = value;

                    OnPropertyChanged("Address2");
                }
            }
            get
            {
                return address2;
            }
        }

        public string phone;
        public string Phone
        {
            set
            {
                if (phone != value)
                {
                    phone = value;

                    OnPropertyChanged("Phone");
                }
            }
            get
            {
                return phone;
            }
        }

        public string photo;
        public string Photo
        {
            set
            {
                if (photo != value)
                {
                    photo = value;

                    OnPropertyChanged("Photo");
                }
            }
            get
            {
                return photo;
            }
        }
        ImageSource photoSource;
        public ImageSource PhotoSource
        {
            set
            {
                if (photoSource != value)
                {
                    photoSource = value;

                    OnPropertyChanged("PhotoSource");
                }
            }
            get
            {
                return photoSource;
            }
        }

        string photoName;
        public string PhotoName
        {
            set
            {
                if (photoName != value)
                {
                    photoName = value;
                    OnPropertyChanged("PhotoName");
                }
            }
            get
            {
                return photoName;
            }
        }

        byte[] photoArray;
        public byte[] PhotoArray
        {
            set
            {
                if (photoArray != value)
                {
                    photoArray = value;

                    OnPropertyChanged("PhotoArray");
                    OnPropertyChanged("ShowContinueOnPhotoPage");
                    OnPropertyChanged("ShowSkipOnPhotoPage");
                }
            }
            get
            {
                return photoArray;
            }
        }

        decimal? amount;
        public decimal? Amount
        {
            set
            {
                if (amount != value)
                {
                    amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("AmountError");
                    OnPropertyChanged("ShowContinueOnAmountPage");

                }
            }
            get
            {
                return amount;
            }
        }

        DateTime? date;
        public DateTime? Date
        {
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                    OnPropertyChanged("ShowContinueOnDatePage");

                }
            }
            get
            {
                return date;
            }
        }

        string description;
        public string Description
        {
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                    OnPropertyChanged("ShowContinueOnDescriptionPage");

                }
            }
            get
            {
                return description;
            }
        }
        public bool ShowContinueOnDescriptionPage
        {
            get
            {
                return !string.IsNullOrEmpty(description);
            }
        }

        public string department;
        public string Department
        {
            set
            {
                if (department != value)
                {
                    department = value;
                    OnPropertyChanged("Department");
                }
            }
            get
            {
                return department;
            }
        }

        string userFirstLastName;
        public string UserFirstLastName
        {
            set
            {
                if (userFirstLastName != value)
                {
                    userFirstLastName = value;

                    OnPropertyChanged("UserFirstLastName");
                }
            }
            get
            {
                return userFirstLastName;
            }
        }

        string city;
        public string City
        {
            set
            {
                if (city != value)
                {
                    city = value;

                    OnPropertyChanged("City");
                }
            }
            get
            {
                return city;
            }
        }

        string region;
        public string Region
        {
            set
            {
                if (region != value)
                {
                    region = value;

                    OnPropertyChanged("Region");
                }
            }
            get
            {
                return region;
            }
        }

        string zip;
        public string ZIP
        {
            set
            {
                if (zip != value)
                {
                    zip = value;

                    OnPropertyChanged("ZIP");
                }
            }
            get
            {
                return zip;
            }
        }

        string cityRegionZip;
        public string CityRegionZip
        {
            set
            {
                if (cityRegionZip != value)
                {
                    cityRegionZip = value;

                    OnPropertyChanged("CityRegionZip");
                }
            }
            get
            {
                return cityRegionZip;
            }
        }
        string country;
        public string Country
        {
            set
            {
                if (country != value)
                {
                    country = value;

                    OnPropertyChanged("Country");
                }
            }
            get
            {
                return country;
            }
        }

        string avatarHtml;
        public string AvatarHtml
        {
            set
            {
                if (avatarHtml != value)
                {
                    avatarHtml = value;

                    OnPropertyChanged("AvatarHtml");
                }
            }
            get
            {
                return avatarHtml;
            }
        }
        #region Check value for all controls
        public bool isEmailVisible
        {
            get
            {
                if (Email != null && Email.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isNameVisible
        {
            get
            {
                if (Name != null && Name.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isUserFirstLastNamelVisible
        {
            get
            {
                if ((FirstName != null && FirstName.Length > 0) || (LastName != null && LastName.Length > 0))
                    return true;
                return false;
            }
        }
        public bool isIconVisible
        {
            get
            {
                if (Icon != null && Icon.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isJobTitleVisible
        {
            get
            {
                if (JobTitle != null && JobTitle.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isRegionVisible
        {
            get
            {
                if (Region != null && Region.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isZIPVisible
        {
            get
            {
                if (ZIP != null && ZIP.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isCityRegionZipVisible
        {
            get
            {
                if ((City != null && City.Length > 0) || (Region != null && Region.Length > 0) || (ZIP != null && ZIP.Length > 0))
                    return true;
                return false;
            }
        }
        bool isDepartmentVisible;
        public bool IsDepartmentVisible
        {
            set
            {
                if (isDepartmentVisible != value)
                {
                    isDepartmentVisible = value;

                    OnPropertyChanged("IsDepartmentVisible");
                }
            }
            get
            {
                return isDepartmentVisible;
            }
        }
        bool isAddress2Visible;
        public bool IsAddress2Visible
        {
            set
            {
                if (isAddress2Visible != value)
                {
                    isAddress2Visible = value;

                    OnPropertyChanged("IsAddress2Visible");
                }
            }
            get
            {
                return isAddress2Visible;
            }
        }
        bool isAddress1Visible;
        public bool IsAddress1Visible
        {
            set
            {
                if (isAddress1Visible != value)
                {
                    isAddress1Visible = value;

                    OnPropertyChanged("IsAddress1Visible");
                }
            }
            get
            {
                return isAddress1Visible;
            }
        }
        bool isCountryVisible;
        public bool IsCountryVisible
        {
            set
            {
                if (isCountryVisible != value)
                {
                    isCountryVisible = value;

                    OnPropertyChanged("IsCountryVisible");
                }
            }
            get
            {
                return isCountryVisible;
            }
        }
        bool isPhoneVisible;
        public bool IsPhoneVisible
        {
            set
            {
                if (isPhoneVisible != value)
                {
                    isPhoneVisible = value;

                    OnPropertyChanged("IsPhoneVisible");
                }
            }
            get
            {
                return isPhoneVisible;
            }
        }
        bool isCityVisible;
        public bool IsCityVisible
        {
            set
            {
                if (isCityVisible != value)
                {
                    isCityVisible = value;

                    OnPropertyChanged("IsCityVisible");
                }
            }
            get
            {
                return isCityVisible;
            }
        }


        bool isCompanyVisible;
        public bool IsCompanyVisible
        {
            set
            {
                if (isCompanyVisible != value)
                {
                    isCompanyVisible = value;

                    OnPropertyChanged("IsCompanyVisible");
                }
            }
            get
            {
                return isCompanyVisible;
            }
        }


        bool isPhotoVisible;
        public bool IsPhotoVisible
        {
            set
            {
                if (isPhotoVisible != value)
                {
                    isPhotoVisible = value;

                    OnPropertyChanged("IsPhotoVisible");
                }
            }
            get
            {
                return isPhotoVisible;
            }
        }

        bool isAvatarVisible;
        public bool IsAvatarVisible
        {
            set
            {
                if (isAvatarVisible != value)
                {
                    isAvatarVisible = value;

                    OnPropertyChanged("IsAvatarVisible");
                }
            }
            get
            {
                return isAvatarVisible;
            }
        }

        #endregion
    }
}