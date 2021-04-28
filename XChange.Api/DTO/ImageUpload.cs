using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class ImageUpload
    {
        public IFormFile Image{get; set;}
        public string ProductName { get; set; }
    }

    public class ImageResponse
    {
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
    }
}
