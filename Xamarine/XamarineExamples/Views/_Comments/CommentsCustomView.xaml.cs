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
using ProjectInsightMobile.Views.Profile;
using HtmlAgilityPack;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsCustomView : ContentView
    {

        public static Guid relatedId;
        public static string itemName;
        List<Comment> comments;
        public CommentsViewModel viewModel;

        public CommentsCustomView()
        {
            //HockeyApp.MetricsManager.TrackEvent("CommentsCustomView Initialize");
            InitializeComponent();
            slComments.BindingContext = viewModel = new CommentsViewModel();

            //  ShowComments();
        }

        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(CommentsCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemIdPropertyChanged);

        public static readonly BindableProperty ItemNameProperty = BindableProperty.Create(
                                                       propertyName: "ItemName",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(CommentsCustomView),
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
            var control = (CommentsCustomView)bindable;
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
            var control = (CommentsCustomView)bindable;
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

            sfComment.HasError = false;
            lstComments.IsVisible = false;
            AddComment.IsVisible = true;

            listofUsers = await UsersService.ListCommunicateActiveAsync();


        }
        public async void onCreateNewComment(object sender, EventArgs e)
        {


            var commentBody = txtCommentBody.Text;
            if (commentBody == null || commentBody.Length == 0)
            {
                sfComment.HasError = true;
                //Common.Instance.ShowToastMessage("Save failed because(s): A comment is required!", DoubleResources.DangerSnackBar);
            }
            else
            {
                //progressBar.IsRunning = true;
                //loadingContent.IsVisible = true;

                sfComment.HasError = false;
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

        public void ShowComments()
        {
            try
            {
                comments = new List<Comment>();
                //comments = CommentsService.GetAll(relatedId);
                comments = CommentsService.client.ListByObjectUserComments(relatedId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserCreated;User:FirstName,LastName,Name,CreatedDateTimeUTC,PhotoUrl,AvatarHtml"));
                if (comments.Count == 0) return;
                if (comments.Count > 4)
                    btnSeeAll.IsVisible = true;
                else
                    btnSeeAll.IsVisible = false;

                viewModel.Comments = new ObservableCollection<Comment>(comments.Take(4));

                if (slComments.Children.Count > 0)
                {
                    slComments.Children.Clear();
                }
           

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
                    commentBody = commentBody.Replace("&nbsp;", " ");
                    bool isCommentCut = false;
                    if (commentBody.Length > 100)
                    {
                        commentBody = commentBody.Substring(0, 100) + "...";
                        isCommentCut = true;
                    }

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Margin = new Thickness(0, 4, 0, 4);

                    StackLayout slImage = new StackLayout();
                    slImage.Margin = 0;
                    slImage.Spacing = 0;
                    slImage.Padding = 0;
                    slImage.VerticalOptions = LayoutOptions.Start;
                    slImage.HorizontalOptions = LayoutOptions.Start;

                    Label lblUserId = new Label();
                    lblUserId.IsVisible = false;
                    lblUserId.Text = item.UserCreated.Id.ToString();
                    lblUserId.Margin = 0;
                    slImage.Children.Add(lblUserId);
                    var imageGestureRecognizer = new TapGestureRecognizer();
                    imageGestureRecognizer.Tapped += (s, e) =>
                    {
                        StackLayout sl = (StackLayout)s;
                        Label lbl = (Label)sl.Children[0];
                        Navigation.PushAsync(new UserProfile(new Guid(lbl.Text)));

                    };
                    slImage.GestureRecognizers.Add(imageGestureRecognizer);

                    if (!string.IsNullOrEmpty(item.UserCreated.PhotoUrl))
                    {
                      
                        Image photo = new Image();
                        photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + item.UserCreated.PhotoUrl));
                        photo.HeightRequest = 50;
                        photo.WidthRequest = 50;
                        photo.HorizontalOptions = LayoutOptions.Start;
                        photo.VerticalOptions = LayoutOptions.Start;
                        

                       
                        slImage.Children.Add(photo);
                        grid.Children.Add(slImage, 0, 0);
                    }
                    else
                    {
                        if (item.UserCreated.AvatarHtml != String.Empty)
                        {
                            //userHTML = "<style>.user-avatar {font-family: 'Open Sans',segoe ui,verdana,helvetica;width: 60px!important;height: 60px!important;border-radius: 2px;line-height: 60px!important;font-size: 32px!important;color: #fff;text-align: center;margin: 0 !important;vertical-align: middle;overflow: hidden;cursor: pointer;display: inline-block;}</style>";
                            //userHTML += item.UserCreated.AvatarHtml;


                            string myDiv = item.UserCreated.AvatarHtml;
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(myDiv);
                            HtmlNode node = doc.DocumentNode.SelectSingleNode("div");
                            string AvatarInitials = "PI";
                            string AvatarColor = "#fff";
                            string PhotoURL = string.Empty;

                            if (node != null)
                            {
                                AvatarInitials = (node.ChildNodes[0]).OuterHtml;
                                foreach (HtmlAttribute attr in node.Attributes)
                                {
                                    if (attr.Name.ToLower() == "style")
                                    {
                                        string[] parts = attr.Value.Split('#');
                                        if (parts != null && parts.Length > 1)
                                        {
                                            AvatarColor = parts[1];
                                        }
                                    }
                                }
                            }


                            slImage.BackgroundColor = Color.FromHex(AvatarColor);

                            Label lbInitials = new Label();
                            lbInitials.HeightRequest = 50;
                            lbInitials.WidthRequest = 50;
                            lbInitials.HorizontalOptions = LayoutOptions.CenterAndExpand;
                           
                            lbInitials.HorizontalTextAlignment = TextAlignment.Center;
                            lbInitials.VerticalTextAlignment = TextAlignment.Center;
                            lbInitials.TextColor = Color.White;
                            lbInitials.Text = AvatarInitials;
                            lbInitials.FontSize = 26;
                            lbInitials.LineBreakMode = LineBreakMode.NoWrap;
                            if (Device.RuntimePlatform.ToLower() == "android")
                                lbInitials.FontFamily = "OpenSans-SemiBold.ttf#Open Sans";
                            else
                                lbInitials.FontFamily = "OpenSans-SemiBold";
                            slImage.Children.Add(lbInitials);

                            grid.Children.Add(slImage, 0, 0);
                        }
                    }

                    Frame frmBody = new Frame()
                    {
                        HasShadow = false,
                        Padding = 5,
                        Margin = new Thickness(0,0,0,0),
                        CornerRadius = 5,
                        BackgroundColor = (Color)Application.Current.Resources["LightBackgroundColor"],
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    



                    StackLayout slBody = new StackLayout();
                    slBody.VerticalOptions = LayoutOptions.FillAndExpand;
                    slBody.HorizontalOptions = LayoutOptions.FillAndExpand;
                    slBody.Orientation = StackOrientation.Vertical;
                    slBody.Margin = 0;
                    slBody.Spacing = 0;
                    slBody.Padding = 0;
                  

                    Label lblId = new Label();
                    lblId.IsVisible = false;
                    lblId.Text = item.Id.ToString();
                    lblId.Margin = 0;
                    

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) =>
                    {
                        StackLayout sl = (StackLayout)s;
                        Label lbl = (Label)sl.Children[0];
                        OpenAllComments(lbl.Text);
                    };
                    slBody.GestureRecognizers.Add(tapGestureRecognizer);


                    Label lblName = new Label();
                    lblName.FontSize = 16;
                    lblName.HorizontalTextAlignment = TextAlignment.Start;
                    lblName.HorizontalOptions = LayoutOptions.FillAndExpand;
                    lblName.VerticalTextAlignment = TextAlignment.Start;
                    lblName.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
                    lblName.FontAttributes = FontAttributes.Bold;
                    lblName.LineBreakMode = LineBreakMode.NoWrap;
                    lblName.Margin = 0;
                    lblName.Text = item.UserCreated.Name;
                    if (Device.RuntimePlatform.ToLower() == "android")
                        lblName.FontFamily = "OpenSans-SemiBold.ttf#Open Sans";
                    else
                        lblName.FontFamily = "OpenSans-SemiBold";
                   


                    Label lblBody = new Label();
                    lblBody.FontSize = 14;
                    lblBody.HorizontalTextAlignment = TextAlignment.Start;
                    lblBody.VerticalOptions = LayoutOptions.FillAndExpand;
                    lblBody.HorizontalOptions = LayoutOptions.FillAndExpand;
                    lblBody.VerticalTextAlignment = TextAlignment.Start;

                    lblBody.LineBreakMode = LineBreakMode.WordWrap;
                    lblBody.Margin = 0;
                    if (Device.RuntimePlatform.ToLower() == "android")
                        lblBody.FontFamily = "OpenSans-Regular.ttf#Open Sans";
                    else
                        lblBody.FontFamily = "OpenSans-Regular";

                    FormattedString fs = new FormattedString();

                    Span spBody = new Span();
                    spBody.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
                    spBody.Text = commentBody;
                    fs.Spans.Add(spBody);

                    if (isCommentCut)
                    {
                        Span spSeeMore = new Span();
                        spSeeMore.Text = " See More";
                        spSeeMore.TextColor = (Color)Application.Current.Resources["DarkGrayTextColor"];
                        fs.Spans.Add(spSeeMore);
                    }
                    lblBody.FormattedText = fs;


                    if (Device.RuntimePlatform.ToLower() == "android")
                        lblBody.FontFamily = "OpenSans-Regular.ttf#Open Sans";
                    else
                        lblBody.FontFamily = "OpenSans-Regular";

                    slComments.Children.Add(grid);
                    grid.Children.Add(frmBody, 1, 0);
                    frmBody.Content = slBody;
                    slBody.Children.Add(lblId);
                    slBody.Children.Add(lblName);
                    slBody.Children.Add(lblBody);
                    
                   
                  
                    
                }

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

            if (!string.IsNullOrEmpty(NewTextValue))
                sfComment.HasError = false;
            else
                sfComment.HasError = true;


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