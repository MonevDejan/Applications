using ProjectInsight.Models.Comments;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.Comments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllComments : ContentPage
    {
        private CommentsViewModel viewModel;

        public AllComments(ObservableCollection<Comment> Comments, string itemName, Guid selectedComment = new Guid())
        {
            try
            {
                NavigationPage.SetBackButtonTitle(this, "");
               //HockeyApp.MetricsManager.TrackEvent("AllComments Initialize");


                StringBuilder HTMLComments = new StringBuilder();
                HTMLComments.Append("<html><body>");
                foreach (Comment item in Comments)
                {
                    // var photoUrl = Common.CurrentWorkspace.WorkspaceURL + item.UserCreated.PhotoUrl;

                    if (item == null) continue;
                    string userHTML = string.Empty;
                    if (!string.IsNullOrEmpty(item.UserCreated.PhotoUrl))
                    {
                        userHTML = string.Format("<img src='{0}'>", Common.CurrentWorkspace.WorkspaceURL + item.UserCreated.PhotoUrl);
                    }
                    else
                    {
                        if (item.UserCreated.AvatarHtml != String.Empty)
                        {
                            userHTML = "<style>.user-avatar {font-family: 'Open Sans',segoe ui,verdana,helvetica;width: 50px!important;height: 50px!important;border-radius: 2px;line-height: 50px!important;font-size: 32px!important;color: #fff;text-align: center;margin: 0 !important;vertical-align: middle;overflow: hidden;cursor: pointer;display: inline-block;}</style>";
                            userHTML += item.UserCreated.AvatarHtml;
                        }
                    }


                    DateTime CreatedDate = DateTime.Now;

                    if (item.CreatedDateTimeUTC != null)
                        CreatedDate =  TimeZoneInfo.ConvertTimeFromUtc(item.CreatedDateTimeUTC.Value, TimeZoneInfo.Local);
                    var body = item.CommentBodyRendered;
                    HTMLComments.Append(
                        CommentsHelperClass.CreateHtmlComments(
                            item.UserCreated.FirstName + " " + item.UserCreated.LastName,
                            userHTML,
                            CreatedDate,
                            body.Replace("<img ", "<img style='max-width: 100%;' "), item.Id == selectedComment)
                    );
                }
                //this.viewModel = viewModel;
                //BindingContext = viewModel;

                InitializeComponent();

                Title = itemName;

                // Adding style to HTMLComments Template
                HTMLComments.Append(CommentsHelperClass.GetHtmlCommentsStyle());
                HTMLComments.Append("</body></html>");

                var htmlString = HTMLComments.ToString();
                wvAllComments.Source = new HtmlWebViewSource()
                {
                    Html = htmlString
                };

                //var hybridWebView = new CustomControls.HybridWebView();
                //hybridWebView.Uri = htmlString;
                //hybridWebView.HeightRequest = 600;
                //slComments.Children.Add(hybridWebView);

            }
            catch (Exception ex)
            {
            }
        }
    }
}