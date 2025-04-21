using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreL.ServiceClasses
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
        void DeleteImage(string imagePath);
    }
}
