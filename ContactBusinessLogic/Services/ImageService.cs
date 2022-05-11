using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBusinessLogic.Interface;
using ContactDatabase.Model;
using DatabaseAndModel.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBusinessLogic.Services
{
    public class ImageService:IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly Cloudinary _cloudinary;
        ImageUploadSettings _accountSettings;

        public ImageService(IConfiguration config, IOptions<ImageUploadSettings> accountSettings, UserManager<User> userManager)
        {
            _accountSettings = accountSettings.Value;
            _configuration = config;
            _userManager = userManager;
            _cloudinary = new Cloudinary(new Account(_accountSettings.CloudName, 
                _accountSettings.ApiKey, _accountSettings.ApiSecret));
           
        }
        public async Task<UploadResult> UploadAsync(IFormFile image)
        {
            bool pictureFormat = false;
            var size = Convert.ToInt64(_configuration.GetSection("PhotoSettings:Size").Get<string>());
            if (image.Length > size)
            {
                throw new ArgumentException("File Size exceeded");
            }

            var listOfImageExtensions = _configuration.GetSection("PhotoSettings:Formats").Get<List<string>>();

            foreach (var item in listOfImageExtensions)
            {
                if (image.FileName.EndsWith(item))
                {
                    pictureFormat = true;
                    break;
                }
            }

            // Pls confirm if this method will throw error without the if statement while testing
            if (pictureFormat == false)
            {
                throw new BadImageFormatException("File format not supported");
            }

            var upload = new ImageUploadResult();

            using (var imageStream = image.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + image.FileName;
                upload = await _cloudinary.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(filename, imageStream),
                    Transformation = new Transformation().Crop("thumb").Gravity("face").Width(150)
                });
            }

            return upload;
        }

        public async Task<bool> UpdateImageAsync(string Id, string Url)
        {
            User user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                user.ImageUrl = string.IsNullOrWhiteSpace(Url) ? user.ImageUrl : Url;
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return true;
                }
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                throw new MissingMemberException(errors);
            }
            throw new ArgumentException("Resource Not Found");
        }

    }
}
