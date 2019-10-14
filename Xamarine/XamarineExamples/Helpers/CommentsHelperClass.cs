using System;

namespace ProjectInsightMobile.Helpers
{
    public static class CommentsHelperClass
    {
        #region HTML Comments Helper Methods: CreateHtmlComments, GetHtmlCommentsStyle

        public static string CreateHtmlComments(string FullName, string imageHTML, DateTime Date, string Comment, bool selected = false)
        {
            var htmlString = @"
                            <div " + (selected ? "id='selectedComment'" : "") + @" class='CommentsList'>
                                <div class='ProfileImage'>"
                                +
                                 imageHTML
                                +
                                @"</div> 
                                <div class='CommentContainer'>
                                  <div class='Content'>
                                    <span class='Username'>" + FullName + @"</span>        
                                    <div class='CommentText'>"
                                      + Comment +
                                    @"</div> 
                                  </div>     
                                  <div class='CommentDate'>"
                                     + Date.Month + "/" + Date.Day + "/" + Date.Year + " " + Date.ToShortTimeString() +
                                  @"</div> 
                                </div> 
                          
                            </div> 
                            ";

            return htmlString;
        }

        public static string CreateHtmlCommentsWithSeeMore(string FullName, string imageHTML, DateTime Date, string Comment, string commentID, bool isCommentCut = false)
        {
            string seeMore = string.Empty;
            if (isCommentCut)
            {
                seeMore = @"&nbsp<span style='white-space: nowrap;' class='btn-SeeMore' >See more</span>";
            }
            var htmlString = @"
                            <div class='CommentsList' onclick=""javascript:invokeCSCode('" + commentID + @"');"">
                                <div class='ProfileImage'>"
                                +
                                 imageHTML
                                +
                                @"</div> 
                                <div class='CommentContainer'>
                                  <div class='Content'>
                                    <span class='Username'>" + FullName + @"</span>        
                                    <div class='CommentText'>"
                                      + Comment +
                                       seeMore +
                                    @"</div> 
                                    
                                  </div>    
                                </div> 
                              </div>  
                          
                            ";

            return htmlString;
        }

        public static string GetHtmlCommentsStyle()
        {
            return @"<style>

                            body {
                               font-family:'Open Sans', Arial, sans-serif;
                            } 
                            a
                            {
                                color: #000;
                            }                           

                              .CommentsList{
                              display:grid;
                              grid-template-columns: 60px auto;
                              
                              margin-bottom: 10px;
                            }
                            
                            .CommentsList .ProfileImage img{
                              width:100%;
                              max-width:50px;
                              padding: 2px 0;
                            }
                            
                            .CommentsList .Username{
                              font-weight:bold;
                              display:block;
                              color: #000000;
                            }
                            
                            .CommentsList .CommentContainer .Content{
                              background: #f0f0f0;
                              color: #545454;
                              padding: 10px;
                              border-radius: 5px;
                            }
                            
                            .CommentsList .CommentContainer .Content .CommentText img{
                              max-width:100%;
                              height:auto;
                            }
                            
                            .CommentsList .CommentContainer .CommentDate{
                              margin: 5px 10px;
                              font-weight: bold;
                              color: #545454;
                            }
                              
                            .CommentsList .CommentContainer .CommentText .btn-SeeMore{
                              font-weight: bold;
                              text-transform: capitalize;
                              
                            }
                            </style>

                        <script type='text/javascript'>
                            document.addEventListener('DOMContentLoaded', function(event) {
                            window.location.hash = '#selectedComment';
                        });                            
                        </script>";
        }

        public static string GetHtmlCommentsWithSeeMoreStyle()
        {
            return @"<style>
                              body {
                               font-family:'Open Sans', Arial, sans-serif;
                            }      

                                *{
                                    margin-right: 0;
                                    margin-left: 0;
                                    padding-left: 0;
                                    padding-right: 0;
                                    }
                              .CommentsList{
                              display:grid;
                              grid-template-columns: 60px auto;
                              grid-gap: 10px;
                              margin-bottom: 10px;
                            }
                            
                            .CommentsList .ProfileImage img{
                              width:100%;
                              max-width:60px;
                              padding: 0px 0;
                            }
                            
                            .CommentsList .Username{
                              font-weight:bold;
                              display:block;
                              color: #000000;
                            }
                            
                            .CommentsList .CommentContainer .Content{
                              background: #f0f0f0;
                              color: #545454;
                              padding: 10px;
                              border-radius: 5px;
                            }
                            
                            .CommentsList .CommentContainer .Content .CommentText img{
                              max-width:100%;
                              height:auto;
                            }
                            
                            .CommentsList .CommentContainer .CommentDate{
                              margin: 0px 0px;
                              font-weight: bold;
                              color: #545454;
                            }
                              
                            .CommentsList .CommentContainer .CommentText .btn-SeeMore{
                              font-weight: bold;
                              text-transform: capitalize;
                            }
                            </style>

                        <script type='text/javascript'>                           
                            function invokeCSCode(data) {
                            try {
                                invokeCSharpAction(data);
                                }
                            catch (err){
                               
                            }
                        }
                        </script>";
        }

        #endregion
    }
}
