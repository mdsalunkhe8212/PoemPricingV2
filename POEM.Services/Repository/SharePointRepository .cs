using Azure.Identity;
using Microsoft.Graph;
using POEM.Model.Model;
using POEM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
//using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace POEM.Services.Repository
{
    // Repositories/SharePointRepository.cs
    public class SharePointRepository : ISharePointRepository
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _siteId;
        private readonly int siteid;
        public SharePointRepository()
        {
            string progress = "In function -> ";
            try
            {
                progress+= "Reading ClientSecretCredential -> ";
                var credential = new ClientSecretCredential(
                tenantId: ConfigurationManager.AppSettings["AzureTenantId"],
                clientId: ConfigurationManager.AppSettings["AzureClientId"],
                clientSecret: ConfigurationManager.AppSettings["AzureClientSecret"]
            );
                progress += "GraphServiceClient initlizing -> ";
                _graphClient = new GraphServiceClient(credential);
                progress += "Reading SharePointSiteId -> ";
                _siteId = ConfigurationManager.AppSettings["SharePointSiteId"];
            }
            catch (Exception ex)
            {
                //return ();
                throw new Exception($"Progress: {ex.Message + "  --  " + progress}", ex);
                //throw (new { error = ex.Message + "  --  " + progress, inner = ex.InnerException?.Message, stack = ex.StackTrace });
            }
            
        }

        public async Task<SharePointImage> GetImageAsync(string filePath)
        {
            // Step 1: Get the default drive ID for the site
            //var drive = await _graphClient
            //    .Sites[_siteId]
            //    .Drive
            //    .GetAsync();

            string driveName= ConfigurationManager.AppSettings["SharePointDriveName"]; 
            string imageFolder= ConfigurationManager.AppSettings["SharePointImageFolder"]; 
            var drives = await _graphClient
                        .Sites[_siteId]
                        .Drives
                        .GetAsync();

            var drive = drives.Value.FirstOrDefault(d => d.Name == driveName);


            // Step 2: Resolve item by path using "root:/path:" syntax
            //var item = await _graphClient
            //    .Drives[drive.Id]
            //    .Items[$"root:/{filePath}:"]
            //    .GetAsync();

            // Step 3: Get content stream using resolved item ID
            //var stream = await _graphClient
            //    .Drives[drive.Id]
            //    .Items[item.Id]
            //    .Content
            //    .GetAsync();
            string filePathWithFolder = imageFolder + "\\" + filePath;
            var stream = await _graphClient
                        .Drives[drive.Id]
                        .Root
                        .ItemWithPath(filePathWithFolder)
                        .Content
                        .GetAsync();
            return new SharePointImage
            {
                FileName = Path.GetFileName(filePathWithFolder),
                FilePath = filePath,
                MimeType = GetMimeType(filePathWithFolder),
                Content = stream
            };
        }

        //public async Task<List<string>> GetImageListAsync(string folderPath)
        //{
        //    var items = await _graphClient.Sites[_siteId].Drive.Root
        //        .ItemWithPath(folderPath)
        //        .Children
        //        .Request()
        //        .GetAsync();

        //    var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        //    return items
        //        .Where(i => imageExtensions.Contains(
        //            Path.GetExtension(i.Name).ToLower()))
        //        .Select(i => $"{folderPath}/{i.Name}")
        //        .ToList();
        //}

        private string GetMimeType(string filePath)
        {
            switch (Path.GetExtension(filePath).ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".webp":
                    return "image/webp";
                default:
                    return "application/octet-stream";
            }
        }
    }
}