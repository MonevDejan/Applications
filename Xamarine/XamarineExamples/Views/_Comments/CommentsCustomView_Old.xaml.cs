using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Models;
using SQLite;
using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models.Comments;
using ProjectInsightMobile.Services;
using System.Collections.Generic;
using ProjectInsightMobile.Views.Comments;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ProjectInsight.Models.Users;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsCustomView_Old : ContentView
    {

        public static Guid relatedId;
        public static string itemName;
        List<Comment> comments;
        public CommentsViewModel viewModel;

        public CommentsCustomView_Old()
        {
            //HockeyApp.MetricsManager.TrackEvent("CommentsCustomView Initialize");
            InitializeComponent();
            slComments.BindingContext = viewModel = new CommentsViewModel();

            //  ShowComments();
        }

        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(CommentsCustomView_Old),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemIdPropertyChanged);

        public static readonly BindableProperty ItemNameProperty = BindableProperty.Create(
                                                       propertyName: "ItemName",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(CommentsCustomView_Old),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemNamePropertyChanged);

        public string ItemId
        {
            get { return base.GetValue(ItemIdProperty).ToString(); }
            set { base.SetValue(ItemIdProperty, value); }
        }

        public string ItemName
        {
            get { return base.GetValue(ItemNameProperty).ToString(); }
            set { base.SetValue(ItemNameProperty, value); }
        }

        private static void ItemIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CommentsCustomView_Old)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
            {
                relatedId = new Guid(newValue.ToString());
                control.ShowComments();
            }
        }

        private static void ItemNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CommentsCustomView_Old)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                itemName = newValue.ToString();
        }

        List<User> listofUsers;
        public async void OnAddComment(object sender, EventArgs e)
        {
            //viewModel.ShowHideIcon = "plus.png";
            //viewModel.IsExpandedList = false;
            //await Navigation.PushAsync(new AddCommentPage(relatedId, itemName));

            lstComments.IsVisible = false;
            AddComment.IsVisible = true;

            listofUsers = await UsersService.ListCommunicateActiveAsync();


        }
        public async void onCreateNewComment(object sender, EventArgs e)
        {


            var commentBody = txtCommentBody.Text;
            if (commentBody == null || commentBody.Length == 0)
            {

                Common.Instance.ShowToastMessage("Save failed because(s): A comment is required!", DoubleResources.DangerSnackBar);
            }
            else
            {
                //progressBar.IsRunning = true;
                //loadingContent.IsVisible = true;

                
                if (mentionedUsers.Count>0)
                {
                    foreach (var user in mentionedUsers)
                    {
                        commentBody = commentBody.Replace(user.Name, "@user:" + user.Id.ToString());

                    }
                }

                Comment comment = new Comment();
                comment.CommentBody = commentBody;// "Comment Added from Mobile Application";
                comment.CreatedDateTimeUTC = DateTime.UtcNow;
                comment.UserCreated_Id = Common.CurrentWorkspace.UserID;
                comment.ObjectId = relatedId;

                // var com = await CommentsService.Get(new Guid("3c9195e9-272a-48f5-8b9a-5029e82d2a7c"));

                try
                {
                    var response = await CommentsService.AddComment(comment);

                    if (!response.HasErrors)
                    {
                        txtCommentBody.Text = string.Empty;
                        lstComments.IsVisible = true;
                        AddComment.IsVisible = false;

                        ShowComments();
                    }
                    else
                        Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                }
                catch (Exception ex)
                {
                    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                }

                //progressBar.IsRunning = false;
                //loadingContent.IsVisible = false;
            }



        }
        public async void onCancel(object sender, EventArgs e)
        {
            txtCommentBody.Text = string.Empty;
            lstComments.IsVisible = true;
            AddComment.IsVisible = false;
        }
        public async void OnShowAllComments(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new AllComments(viewModel, itemName));
            var allComments = new ObservableCollection<Comment>(comments);
            await Navigation.PushAsync(new AllComments(allComments, itemName));
        }

        public void OpenAllComments(string guid)
        {
            //var selectedComment = viewModel.Comments.Where(x => x.Id.ToString() == guid).FirstOrDefault();
            var allComments = new ObservableCollection<Comment>(comments);
            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(new AllComments(allComments, itemName, new Guid(guid)));
                });
            });
        }

        public async void ShowComments()
        {
            try
            {
                comments = new List<Comment>();
                comments = await CommentsService.GetAll(relatedId);
                if (comments.Count == 0) return;
                if (comments.Count > 4)
                    btnSeeAll.IsVisible = true;
                else
                    btnSeeAll.IsVisible = false;

                viewModel.Comments = new ObservableCollection<Comment>(comments.Take(4));

                if (slComments.Children.Count > 0)
                {
                    slComments.Children.RemoveAt(0);
                }
                StringBuilder HTMLComments = new StringBuilder();
                if (Device.RuntimePlatform.ToLower() == "android")
                    HTMLComments.Append("<html><body>");
                else
                    HTMLComments.Append("<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>");


                foreach (Comment item in viewModel.Comments)
                {
                    string body = item.CommentBody;
                    body = item.CommentBodyRendered;
                    while (body.Contains("display:none"))
                    {
                        int from = body.Substring(0, body.IndexOf("display:none")).LastIndexOf("<");
                        int to = body.IndexOf(">", body.IndexOf("display:none;\">") + 15) + 1;
                        body = body.Substring(0, from) + body.Substring(to);
                    }
                    string commentBody = Regex.Replace(body, "<.*?>", String.Empty);
                    //string commentBody = item.CommentBody;
                    bool isCommentCut = false;
                    if (commentBody.Length > 100)
                    {
                        commentBody = commentBody.Substring(0, 100) + "...";
                        isCommentCut = true;
                    }
                    // var photoUrl = Common.CurrentWorkspace.WorkspaceURL + item.UserCreated.PhotoUrl;


                    string userHTML = string.Empty;
                    if (!string.IsNullOrEmpty(item.UserCreated.PhotoUrl))
                    {
                        userHTML = string.Format("<img src='{0}'>", Common.CurrentWorkspace.WorkspaceURL + item.UserCreated.PhotoUrl);
                    }
                    else
                    {
                        if (item.UserCreated.AvatarHtml != String.Empty)
                        {
                            userHTML = "<style>.user-avatar {font-family: 'Open Sans',segoe ui,verdana,helvetica;width: 60px!important;height: 60px!important;border-radius: 2px;line-height: 60px!important;font-size: 32px!important;color: #fff;text-align: center;margin: 0 !important;vertical-align: middle;overflow: hidden;cursor: pointer;display: inline-block;}</style>";
                            userHTML += item.UserCreated.AvatarHtml;
                        }
                    }









                    HTMLComments.Append(
                        CommentsHelperClass.CreateHtmlCommentsWithSeeMore(
                            item.UserCreated.Name,
                            userHTML,
                            item.CreatedDateTimeUTC != null ? (DateTime)item.CreatedDateTimeUTC : DateTime.Now,
                            commentBody, item.Id.ToString(),
                            isCommentCut)
                    );
                }

                HTMLComments.Append(CommentsHelperClass.GetHtmlCommentsWithSeeMoreStyle());
                HTMLComments.Append("</body></html>");
                var htmlString = HTMLComments.ToString();
                var hybridWebView = new CustomControls.HybridWebView();
                hybridWebView.Uri = htmlString;
                hybridWebView.RegisterAction(data => OpenAllComments(data));
                hybridWebView.HorizontalOptions = LayoutOptions.FillAndExpand;
                hybridWebView.VerticalOptions = LayoutOptions.FillAndExpand;
                hybridWebView.HeightRequest = 450;
                hybridWebView.Margin = 0;
                hybridWebView.BackgroundColor = Color.Transparent;

                slComments.Children.Add(hybridWebView);
            }
            catch (Exception ex)
            {
                //return null;
            }
        }


        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null) return;
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        public async void OnSeeMoreTaped(object sender, EventArgs e)
        {
            var context = ((Label)sender).BindingContext as Comment;
            viewModel.Comments = new ObservableCollection<Comment>(comments);
            await Navigation.PushAsync(new AllComments(viewModel.Comments, itemName, context.Id.Value));
        }

        public void OnSeeMoreBindingContext(object sender, EventArgs e)
        {
            var context = ((Label)sender).BindingContext;
        }

        bool suggestionStarted = false;
        int positionOfSuggestion = -1;
        string suggestionFound = string.Empty;
        private void TxtCommentBody_TextChanged(object sender, TextChangedEventArgs e)
        {

            string NewTextValue = e.NewTextValue;
            string OldTextValue = e.OldTextValue;

            if (NewTextValue != null && OldTextValue != null)
            {
                List<string> diff;
                IEnumerable<string> set1 = NewTextValue.Split(' ').Distinct();
                IEnumerable<string> set2 = OldTextValue.Split(' ').Distinct();

                if (set2.Count() > set1.Count())
                {
                    diff = set2.Except(set1).ToList();
                }
                else
                {
                    diff = set1.Except(set2).ToList();
                }

                if (diff != null && diff.Count > 0 && !string.IsNullOrEmpty(diff[0]) && diff[0].StartsWith("@"))
                {
                    string name = diff[0].Substring(1);

                    if (!string.IsNullOrEmpty(name))
                    {
                        suggestionFound = diff[0];
                        suggestionStarted = true;
                        positionOfSuggestion = txtCommentBody.Text.IndexOf(" @" + name);
                        var users = listofUsers.Where(x => x.FirstName.ToLower().StartsWith(name.ToLower()) || x.LastName.ToLower().StartsWith(name.ToLower())).ToList();
                        listView.ItemsSource = users;
                    }
                    else
                    {
                        suggestionStarted = false;
                        positionOfSuggestion = -1;
                        suggestionFound = string.Empty;
                        listView.ItemsSource = null;
                    }
                }
                else
                {
                    suggestionStarted = false;
                    positionOfSuggestion = -1;
                    suggestionFound = string.Empty;
                    listView.ItemsSource = null;
                }
            }

            //find last position of Space
            int positionOfSpace = NewTextValue.LastIndexOf(' ');

        }

        List<User> mentionedUsers = new List<User>();
        private void FrmSuggestion_Tapped(object sender, EventArgs e)
        {

            var templateGrid = (Frame)sender;
            if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
            {

                var user = ((User)templateGrid.Parent.BindingContext);

                if (suggestionStarted && !string.IsNullOrEmpty(suggestionFound))
                {
                    txtCommentBody.Text = txtCommentBody.Text.Replace(suggestionFound,  user.Name + " ");
                    mentionedUsers.Add(user);
                    suggestionFound = string.Empty;
                    suggestionStarted = false;
                    positionOfSuggestion = -1;
                    suggestionFound = string.Empty;
                    listView.ItemsSource = null;
                }

            }

            txtCommentBody.Focus();
        }
    }
}