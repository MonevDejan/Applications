using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{
    public class ExpenseBulkEntryViewModel : BaseViewModel
    {
        public ExpenseBulkEntryViewModel()
        {
        }



        BulkEntry currentEntry;
        public BulkEntry CurrentEntry
        {
            set
            {
                if (currentEntry != value)
                {
                    currentEntry = value;
                    OnPropertyChanged("CurrentEntry");
                }
            }
            get
            {
                return currentEntry;
            }
        }

        List<BulkEntry> bulkEntries;
        public List<BulkEntry> BulkEntries
        {
            set
            {
                if (bulkEntries != value)
                {
                    bulkEntries = value;
                    OnPropertyChanged("BulkEntries");
                }
            }
            get
            {
                return bulkEntries;
            }
        }

        

       

    }

    public class BulkEntry : BaseViewModel
    {

        DateTime entryDate = DateTime.Now.Date;
        public DateTime EntryDate
        {
            set
            {
                if (entryDate != value)
                {
                    entryDate = value;

                    OnPropertyChanged("EntryDate");
                }
            }
            get
            {
                return entryDate;
            }
        }

        List<EntryCode> expenseCodes;
        public List<EntryCode> ExpenseCodes
        {
            set
            {
                if (expenseCodes != value)
                {
                    expenseCodes = value;

                    OnPropertyChanged("ExpenseCodes");
                    OnPropertyChanged("ListViewHeight");
                    
                }
            }
            get
            {
                return expenseCodes;
            }
        }

        public int ListViewHeight
        {
            get
            {
                return ExpenseCodes.Count * 60;
            }
        }

    }

    public class EntryCode : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Rate { get; set; }
        string quantity { get; set; }
        public string Quantity
        {
            set
            {
                if (quantity != value)
                {
                    quantity = value;

                    OnPropertyChanged("Quantity");
                }
            }
            get
            {
                return quantity;
            }
        }
        public decimal? Total { get; set; }

    }

}
