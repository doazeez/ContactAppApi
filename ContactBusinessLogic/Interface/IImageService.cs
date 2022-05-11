using CloudinaryDotNet.Actions;
using ContactDatabase.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBusinessLogic.Interface
{
    public interface IImageService
    {
        Task<UploadResult> UploadAsync(IFormFile image);
        Task<bool> UpdateImageAsync(string Id, string Url);
    }
}
