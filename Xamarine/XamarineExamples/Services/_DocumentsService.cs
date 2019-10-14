using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.Folders;
using ProjectInsight.WebApi.Client.Files;

using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectInsight.Models.Files;
using System.Net.Http;
using System.IO;
using ProjectInsightMobile.ViewModels;

namespace ProjectInsightMobile.Services
{

    public class DocumentsService
    {
        public static FileItemClient clientFiles;
        public static FolderClient clientFolders;

       

     
        public static List<ProjectInsight.Models.Files.FileItem> GetFilesByContainer(Guid guid)
        {
            try
            {
                //return await client.GetByProjectAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,UserCreated,UserAssignedBy,UserAssignedTo,WorkPercentCompleteType,StartDateTimeUserLocal,EndDateTimeUserLocal,WorkHours,DurationSeconds,Project"));
                return clientFiles.GetByContainer(guid, 
                    modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserUpdated,UserCreated,CreatedDateTimeUTC,UpdatedDateTimeUTC,UrlIcon"));

            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;
            }
        }

        public static List<ProjectInsight.Models.Folders.Folder> GetFoldersByContainer(Guid guid)
        {
            try
            {
                //return await client.GetByProjectAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,UserCreated,UserAssignedBy,UserAssignedTo,WorkPercentCompleteType,StartDateTimeUserLocal,EndDateTimeUserLocal,WorkHours,DurationSeconds,Project"));
                return  clientFolders.GetChildFolders(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserUpdated,UserCreated,CreatedDateTimeUTC,UpdatedDateTimeUTC,UrlIcon"));

            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;
            }
        }

        public static Task<ProjectInsight.Models.Files.FileItem> GetFile(Guid guid)
        {
            try
            {
                return clientFiles.GetAsync(guid,
                    modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserUpdated,UserCreated,CreatedDateTimeUTC,UpdatedDateTimeUTC,UrlIcon,UrlThumbnail"));

            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;
            }
        }

        public static ApiDeleteResponse DeleteFile(Guid guid)
        {
            try
            {
                return clientFiles.Delete(guid);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<ApiSaveResponse> AddFolder(ProjectInsight.Models.Folders.Folder folder)
        {
            try
            {
                return await clientFolders.SaveAsync(folder);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static async Task<bool> DownloadFile(FileDetailsViewModel file)
        {
            try
            {
                //Send a response to try and download the file
                HttpResponseMessage resp = clientFiles.Download(new Guid(file.Id));

                byte[] bytes = resp.Content.ReadAsByteArrayAsync().Result;


                string localPath = System.IO.Path.Combine(Common.Instance.DocumentFilePath, file.Name);

               // var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //var imagePath1 = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).ToString(), "MyFolderName");


                FileStream wFile = new FileStream(localPath, FileMode.Create);
                wFile.Write(bytes, 0, bytes.Length);
                wFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<ApiSaveResponse> UploadFile(string dataArray, string fileName, Guid id)
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
            ApiSaveResponse saveFileResp  = await clientFiles.SaveAsync(myFile);

            return saveFileResp;
        }

    }

}
