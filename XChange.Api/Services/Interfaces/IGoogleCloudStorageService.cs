using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.Services.Interfaces
{
    public interface IGoogleCloudStorageService
    {
        Task<string> UploadFileAsync(IFormFile image, string imageName);
        Task DeleteFileAsync(string imageName);
    }
}
