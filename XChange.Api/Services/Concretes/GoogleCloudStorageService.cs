using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        private readonly GoogleCredential googleCredential;
        private readonly StorageClient storageClient;
        private readonly string bucketName;

        public GoogleCloudStorageService(IConfiguration configuration)
        {
            googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
            storageClient = StorageClient.Create(googleCredential);
            bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var dataObject = await storageClient.UploadObjectAsync(bucketName, fileName, null, memoryStream);
                return dataObject.MediaLink;
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await storageClient.DeleteObjectAsync(bucketName, fileName);
        }
    }
}
