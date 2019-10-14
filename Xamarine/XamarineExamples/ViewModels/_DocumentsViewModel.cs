using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ProjectInsightMobile.Models;
using ProjectInsightMobile.Views;
using ProjectInsight.Models.Users;
using System.Linq;
using ProjectInsightMobile.Helpers;
using System.Collections.Generic;
using ProjectInsightMobile.Services;
using ProjectInsight.Models.Files;
using ProjectInsight.Models.Folders;

namespace ProjectInsightMobile.ViewModels
{
    public class DocumentsViewModel : BaseViewModel
    {
        public ObservableCollection<DocumentViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public bool hasItems { get { return Items != null && Items.Count > 0; } }


        bool noItems = false;
        public bool NoItems
        {
            set
            {
                if (noItems != value)
                {
                    noItems = value;
                    OnPropertyChanged("NoItems");
                }
            }
            get
            {
                return noItems;
            }
        }

        public DocumentsViewModel(Guid parentId, bool isProject = false)
        {
            VisibleLoad = true;
            Title = "Documents";

            Items = new ObservableCollection<DocumentViewModel>(); 
            //if (showProjects)
            //    LoadItemsCommand = new Command(async () => await ExecuteLoadProjectsCommand());
            //else
                LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(parentId,isProject));

            //MessagingCenter.Subscribe<NewItemPage, MyWorkItem>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as MyWorkItem;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
            VisibleLoad = false;
        }                                          
        async Task ExecuteLoadItemsCommand(Guid parentId, bool isProject = false)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            VisibleLoad = true;
            try
            {
                Items.Clear();
                if (isProject == true)
                {
                    List<ProjectInsight.Models.Folders.Folder> folders = DocumentsService.GetFoldersByContainer(parentId);
                    Folder docFolder = folders.Where(x => x.Name == "Documents").FirstOrDefault();
                    if (docFolder != null)
                    {
                        parentId = docFolder.Id.Value;
                    }

                }

                List<ProjectInsight.Models.Folders.Folder> myFolders = DocumentsService.GetFoldersByContainer(parentId);
                foreach (Folder item in myFolders)
                {

                    DocumentViewModel newItem = new DocumentViewModel();
                    newItem.Id = item.Id.Value;
                    newItem.Name = item.Name;
                    newItem.UpdatedAt = item.UpdatedDateTimeUTC == null ? item.CreatedDateTimeUTC.Value : item.UpdatedDateTimeUTC.Value;
                    newItem.UpdatedBy = item.UserUpdated == null ? item.UserCreated.FirstName + " " + item.UserCreated.LastName : item.UserUpdated.FirstName + " " + item.UserUpdated.LastName;
                    newItem.Icon = Common.CurrentWorkspace.WorkspaceURL.ToLower().Replace("/projectinsight.webapp", "") + item.UrlIcon;
                    newItem.IsFolder = true;
                    Items.Add(newItem);
                }

                List<ProjectInsight.Models.Files.FileItem> myFiles = DocumentsService.GetFilesByContainer(parentId);


                foreach (var item in myFiles)
                {

                    DocumentViewModel newItem = new DocumentViewModel();
                    newItem.Id = item.Id.Value;
                    newItem.Name = item.Name;
                    newItem.UpdatedAt = item.UpdatedDateTimeUTC == null ? item.CreatedDateTimeUTC.Value : item.UpdatedDateTimeUTC.Value;
                    newItem.UpdatedBy = item.UserUpdated == null ? item.UserCreated.FirstName + " " + item.UserCreated.LastName : item.UserUpdated.FirstName + " " + item.UserUpdated.LastName;
                    newItem.Icon = Common.CurrentWorkspace.WorkspaceURL.ToLower().Replace("/projectinsight.webapp", "") + item.UrlIcon;
                    newItem.IsFolder = false;
                    Items.Add(newItem);
                }

                if ((myFiles != null && myFiles.Count > 0) || (myFolders != null && myFolders.Count > 0))
                    NoItems = false;
                else
                    NoItems = true;


                bool isAndroid = false;
                if (Device.RuntimePlatform.ToLower() == "android")
                {
                    isAndroid = true;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }


            VisibleLoad = false;
        }
    }
}