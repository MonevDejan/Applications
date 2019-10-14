using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models.Items;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.Views.RelatedItems;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RelatedItemsCustomView : ContentView
    {
        public static Guid relatedId;
        public static string itemType;
        public static int itemsCount;
        //public ObservableCollection<MyWorkItem> listWork { get; set; }
        public static RelatedItemsViewModel viewModel;
        public RelatedItemsCustomView ()
		{
			InitializeComponent ();

           // TextEntry.BindingContext = this;
            slRelatedItems.BindingContext = viewModel = new RelatedItemsViewModel();
            viewModel.IsExpandedList = false;

                                      
        }    
        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(RelatedItemsCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemIdPropertyChanged);

        public string ItemId
        {
            get { return base.GetValue(ItemIdProperty).ToString(); }
            set { base.SetValue(ItemIdProperty, value); }
        }

        private static void ItemIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RelatedItemsCustomView)bindable;
            if (control == null) return;
           // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                relatedId = new Guid(newValue.ToString());
        }


        public static readonly BindableProperty ItemTypeProperty = BindableProperty.Create(
                                                       propertyName: "ItemType",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(RelatedItemsCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemTypePropertyChanged);

        public string ItemType
        {
            get { return base.GetValue(ItemTypeProperty).ToString(); }
            set { base.SetValue(ItemTypeProperty, value); }
        }

        private static void ItemTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RelatedItemsCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                itemType = newValue.ToString();
        }

        public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(
                                          propertyName: "ItemsCount",
                                          returnType: typeof(int),
                                          declaringType: typeof(RelatedItemsCustomView),
                                          defaultValue: -1,
                                          defaultBindingMode: BindingMode.OneWay,
                                          propertyChanged: ItemsCountPropertyChanged);

        public string ItemsCount
        {
            get { return base.GetValue(ItemsCountProperty).ToString(); }
            set { base.SetValue(ItemsCountProperty, value); }
        }

        private static void ItemsCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RelatedItemsCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            viewModel.IsExpandedList = false;
            viewModel.ShowHideIcon = "Arrow_right.png";

            if (newValue != null)
            {
                itemsCount = (int)newValue;
                viewModel.RelatedItemsLabel = String.Format("Related Items ({0})", itemsCount.ToString());
            }
        }

        public async void OnTappedRelatedItems(object sender, EventArgs e)
        {

            // Do stuff       
            if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "Arrow_right.png";
                return;
            }

            await LoadAndExpand();
        }

        private async Task LoadAndExpand()
        {
            //get RalatedItems
            var relatedItems = await ItemUtilityService.GetItem(relatedId);


            if (relatedItems != null)
            {
                //this is temporary, since the API returns wrong UrlIconFull
                foreach (var item in relatedItems)
                {
                    //item.UrlIconFull = item.UrlIconFull.Replace("/ProjectInsight.WebApp/projectinsight.webapp/", "/ProjectInsight.WebApp/");
                    if (item.UrlIconFull.StartsWith("//"))
                    {
                        item.UrlIconFull = "http:" + item.UrlIconFull;
                    }
                }

                viewModel.Items = new ObservableCollection<RelatedItem>(relatedItems);
                viewModel.RelatedItemsLabel = "Related Items (" + viewModel.Items.Count + ")";
            }
            viewModel.ShowHideIcon = "Arrow_down.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;
            //SQLiteConnection connection = Common.Instance.InitializeDatabase();
            //listWork = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork());
            //ItemsListView.ItemsSource = null;
            //ItemsListView.ItemsSource = listWork;
            viewModel.VisibleLoad = false;
            if (viewModel.Items.Count == 0)
            {
                ItemsListView.IsVisible = false;
            }
            else
            {
                ItemsListView.IsVisible = true;
                int count = viewModel.Items.Count;
                ItemsListView.HeightRequest = count * 45 + 5;

            }
        }

        public async void OnRelatedItemSelected(object sender, EventArgs e)
        {

            var templateGrid = (StackLayout)sender;
                                                     
            var item = (RelatedItem)templateGrid.Parent.BindingContext; 

            if (item.InternalSDKType.Equals("ProjectInsight.Items.Domains.DomainIntegrations.DomainIntegrationExternalObject")) //url link
            {
                Device.OpenUri(new Uri(item.Url));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Items.Files.File")) //file link
            {

                MessagingCenter.Subscribe<FileDetailsPage, Guid>(this, "FileDeleted", async (obj, itemId) =>
                {
                    await LoadAndExpand();

                    MessagingCenter.Unsubscribe<FileDetailsPage, Guid>(this, "FileDeleted");
                });

                await Navigation.PushAsync(new FileDetailsPage(item.Id));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Items.Tasks.Task")) //Task link
            {
                await Navigation.PushAsync(new TaskDetailsPage(item.Id));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Items.Issues.Issue")) //Issue link
            {                                              

                await Navigation.PushAsync(new IssueDetailsPage(item.Id));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Items.ToDos.ToDo")) //ToDo link
            {

                await Navigation.PushAsync(new ToDoDetailsPage(item.Id));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Items.ApprovalRequests.ApprovalRequest")) //ApprovalRequests link
            {      
                await Navigation.PushAsync(new ApprovalRequestPage(item.Id));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Reports.ReportOutput"))
            {
                Device.OpenUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + "/reports/scheduled/" + item.Id));
            }
            else if (item.InternalSDKType.Equals("ProjectInsight.Items.Projects.Project")) //Projects link
            {
                Device.OpenUri(new Uri(item.Url));
                //Common.Instance.ShowToastMessage("In progress - type:Project !", DoubleResources.DangerSnackBar);
                //await Navigation.PushAsync(new ProjectsDetailsPage(item.Id));
            }

        }
        private async Task<bool> CheckPermissionsAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        //await DisplayAlert("Xamarin.Forms Sample", "Storage permission is needed for file picking", "OK");
                        Common.Instance.ShowToastMessage("Storage permission is needed for file picking", DoubleResources.DangerSnackBar);

                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

                    if (results.ContainsKey(Permission.Storage))
                    {
                        status = results[Permission.Storage];
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    Common.Instance.ShowToastMessage("Storage permission was denied.", DoubleResources.DangerSnackBar);
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async void OnAddFile(object sender, EventArgs e)
        {


            MessagingCenter.Subscribe<UploadItem, Guid>(this, "RelatedItemsAdded", async (obj, itemId) =>
            {
                await LoadAndExpand();

                MessagingCenter.Unsubscribe<UploadItem, Guid>(this, "RelatedItemsAdded");
            });


            await Navigation.PushAsync(new UploadItem(parentId: new Guid(ItemId), itemType:ItemType));
            //try
            //{
            //    if (Device.RuntimePlatform == Device.Android &&
            //      !await this.CheckPermissionsAsync())
            //    {
            //        return;
            //    }


            //    var crossFilePicker = CrossFilePicker.Current;
            //    var myResult = await crossFilePicker.PickFile();
            //    if (myResult != null && !string.IsNullOrEmpty(myResult.FileName))  
            //    {
            //        string FileContent = Convert.ToBase64String(myResult.DataArray) ;
            //        //foreach (byte b in myResult.DataArray) //Empty array
            //        //    FileContent = b.ToString();

            //        ProjectInsight.WebApi.Client.Files.FileItemClient fileItemClient = null;          

            //        if (ItemType.Equals("Task")) //Task link
            //        {
            //            fileItemClient = TasksService.client.ProjectInsightWebApiClient.FileItem;
            //        }
            //        else if (ItemType.Equals("Issue")) //Issue link
            //        {
            //            fileItemClient = IssuesService.client.ProjectInsightWebApiClient.FileItem;
            //        }
            //        else if (ItemType.Equals("ToDo")) //ToDo link
            //        {
            //            fileItemClient = ToDoService.client.ProjectInsightWebApiClient.FileItem;
            //        }
            //        else if (ItemType.Equals("ApprovalRequest")) //ToDo link
            //        {
            //            fileItemClient = ApprovalRequestService.client.ProjectInsightWebApiClient.FileItem;
            //        }

            //        var result = await FilesFoldersService.UploadFile(fileItemClient, FileContent, myResult.FileName, myResult.FileName, new Guid(ItemId));

            //        if (result !=null && !result.HasErrors)
            //        {

            //            //get RalatedItems
            //            var relatedItems = await ItemUtilityService.GetItem(relatedId);

            //            viewModel.Items = new ObservableCollection<RelatedItem>();

            //            if (relatedItems != null && relatedItems.Count > 0)
            //            {

            //                //this is temporary, since the API returns wrong UrlIconFull
            //                foreach (var item in relatedItems)
            //                {
            //                    item.UrlIconFull = item.UrlIconFull.Replace("/ProjectInsight.WebApp/projectinsight.webapp/", "/ProjectInsight.WebApp/");
            //                    viewModel.Items.Add(item);
            //                }

            //                ItemsListView.IsVisible = true;
            //                int count = viewModel.Items.Count;
            //                ItemsListView.HeightRequest = count * 45;
            //                viewModel.RelatedItemsLabel = "Related Items (" + count + ")";
            //            }
            //        }
            //        else
            //        {                                                                                        
            //            Common.Instance.ShowToastMessage("Error.." + result.Errors.FirstOrDefault(), DoubleResources.DangerSnackBar);
            //        }                                            
            //    }
            //    else
            //    {

            //        //Common.Instance.ShowToastMessage("Error with file!", DoubleResources.DangerSnackBar);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Common.Instance.ShowToastMessage("Error - " + ex.ToString(), DoubleResources.DangerSnackBar);
            //    ex.ToString(); //"Only one operation can be active at a time"
            //}
        }
        
    }
}