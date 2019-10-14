using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.Files;
using ProjectInsight.WebApi.Client.Files;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{
    public class FilesFoldersService
    {
        public static async Task<ApiSaveResponse> UploadFile(FileItemClient fileItem, string dataArray, string filePath, string fileName, Guid id)
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
            saveFileResp = fileItem.Save(myFile);

            return saveFileResp;
        }


        public static async Task<bool> DownloadFile(FileItemClient fileItem, Guid fileID)
        {
            try
            {
                //Retrieve the FileItem to use properties
                FileItem file = fileItem.Get(fileID);

                //Send a response to try and download the file
                HttpResponseMessage resp = fileItem.Download(file.Id.Value);

                byte[] bytes = resp.Content.ReadAsByteArrayAsync().Result;
                

                string localPath = System.IO.Path.Combine(Common.Instance.DocumentFilePath, file.FileName.ToString());
                FileStream wFile = new FileStream(localPath, FileMode.Create);
                wFile.Write(bytes, 0, bytes.Length);
                wFile.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
