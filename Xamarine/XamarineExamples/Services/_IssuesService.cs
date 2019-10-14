using ProjectInsight.Models.Base;
using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.Comments;
using ProjectInsight.Models.Issues;
using ProjectInsight.WebApi.Client.Issues;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class IssuesService
    {
        public static IssueClient client;
        public static async Task<Issue> GetIssue(Guid issueId)
        {                                                                                             
            return await client.GetAsync(issueId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,IssueType,FoundBy,FoundDateTimeUTC,IssuePriority,IssueSeverity,ProjectResolution,ProjectAffiliation;Project:ItemNumberFullAndNameDisplayPreference"));
        }

        public static async Task<List<Comment>> GetResolutions(Guid issueId)
        {
            try
            {
                var res = await IssuesService.client.ListResolutions(issueId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserCreated"));
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
            //await Task.Factory.StartNew(() => {

                
                //return res;
            //});
            //return new List<Comment>();
        }

        public static async Task<ApiSaveResponse> UploadFile(string dataArray, string filePath, string fileName, Guid id)
        {
            //create new file item
            ProjectInsight.Models.Files.FileItem myFile = new ProjectInsight.Models.Files.FileItem();
            myFile.ItemContainer_Id = id;
            myFile.Name = fileName;

            //create FileUpload object for file item
            ProjectInsight.Models.Files.FileUpload fileUpload = new ProjectInsight.Models.Files.FileUpload();
            fileUpload.FileName = fileName;
            fileUpload.FileContentsBase64Encoded = dataArray; //Content of the file to upload
            myFile.UploadedFile = fileUpload;

            //Save and return a response
            ProjectInsight.Models.Base.Responses.ApiSaveResponse saveFileResp = new ProjectInsight.Models.Base.Responses.ApiSaveResponse();
            saveFileResp = client.ProjectInsightWebApiClient.FileItem.Save(myFile);

            return saveFileResp;
        }


        public static async Task<List<ProjectInsight.Models.Issues.Issue>> GetByProject(Guid guid)
        {
            try
            {
                return await client.SearchAsync(projectList: guid.ToString(), modelProperties: new ModelProperties("default,IssueStatusType"));

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }
    }
}
