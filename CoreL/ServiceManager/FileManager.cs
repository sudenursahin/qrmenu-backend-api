using CoreL.ServiceClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreL.ServiceManager
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly string[] _allowedFileTypes;
        private readonly long _maxFileSize;
        private readonly string _basePath;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            // GetSection ve GetValue kullanımını düzeltelim
            _allowedFileTypes = _configuration.GetSection("FileSettings:AllowedFileTypes").Get<string[]>()
                ?? new[] { ".jpg", ".jpeg", ".png" }; // Varsayılan değerler

            _maxFileSize = _configuration.GetValue("FileSettings:MaxFileSize", 5242880L); // Varsayılan 5MB
            _basePath = _configuration.GetValue("FileSettings:ImagePath", "wwwroot/images"); // Varsayılan yol
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            // Dosya uzantısını kontrol et
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedFileTypes.Contains(fileExtension))
                throw new ArgumentException("Invalid file type");

            // Dosya boyutunu kontrol et
            if (file.Length > _maxFileSize)
                throw new ArgumentException("File size exceeds limit");

            // Klasör yolunu oluştur
            var uploadPath = Path.Combine(_basePath, folderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Benzersiz dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Veritabanında saklanacak göreceli yolu döndür
            return Path.Combine(folderName, fileName);
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            var fullPath = Path.Combine(_basePath, imagePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
