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
using System.Linq;
using System.Text.RegularExpressions;
using ProjectInsightMobile.Views.Profile;
using HtmlAgilityPack;
using System.Threading.Tasks;
using ProjectInsightMobile.Views.Comments;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResolutionsCustomView : ContentView
    {
        public static Guid relatedId;
        public static string itemName;

        public CommentsViewModel viewModel;
        List<Comment> comments;

        public ResolutionsCustomView()
        {
            InitializeComponent();


            slResolutions.BindingContext = viewModel = new CommentsViewModel();
            viewModel.IsExpandedList = false;
        }

        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(ResolutionsCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemIdPropertyChanged);


        public static readonly BindableProperty ItemNameProperty = BindableProperty.Create(
                                                       propertyName: "ItemName",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(ResolutionsCustomView),
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
            var control = (ResolutionsCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                relatedId = new Guid(newValue.ToString());
        }

        private static void ItemNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ResolutionsCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                itemName = newValue.ToString();
        }

        public async void OnAddComment(object sender, EventArgs e)
        {

            MessagingCenter.Subscribe<AddResolutionPage, Guid>(this, "IssueResolutionAdded", async (obj, taskId) =>
            {
                OnTappedRelatedItems(null, null);

                MessagingCenter.Unsubscribe<AddResolutionPage, Guid>(this, "IssueResolutionAdded");
            });


            await Navigation.PushAsync(new AddResolutionPage(relatedId));
            viewModel.ShowHideIcon = "Arrow_right.png";
            viewModel.IsExpandedList = false;
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

            //get RalatedItems
            ////List<Comment> comments = new List<Comment>();

            ////comments = await IssuesService.GetResolutions(relatedId);

            ShowComments();

            viewModel.Comments = new ObservableCollection<Comment>(comments);
            viewModel.ShowHideIcon = "Arrow_down.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;
            viewModel.VisibleLoad = false;
           


            ////ItemsListView.ItemsSource = null;
            ////ItemsListView.ItemsSource = viewModel.Comments;
            ////ItemsListView.IsVisible = true;
            ////if (viewModel.Comments.Count == 0)
            ////{
            ////    ItemsListView.IsVisible = false;
            ////}
        }


        public async void ShowComments()
        {
            try
            {
                comments = new List<Comment>();
                //comments = CommentsService.GetAll(relatedId);
                comments = await IssuesService.client.ListResolutions(relatedId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserCreated;User:FirstName,LastName,Name,CreatedDateTimeUTC,PhotoUrl,AvatarHtml"));
                if (comments.Count == 0) return;

                viewModel.Comments = new ObservableCollection<Comment>(comments);

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
                        Margin = new Thickness(0, 0, 0, 0),
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
    }
}