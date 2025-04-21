using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class QRCodeManager : IQRCodeService
    {
        private readonly IDistributedCache _cache;
        private const int TOKEN_EXPIRATION_MINUTES = 3;



        public QRCodeManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public byte[] GenerateQRCode(int tableId, string baseUrl)
        {
            string tableUrl = $"{baseUrl}/menu/table/{tableId}";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(tableUrl, QRCodeGenerator.ECCLevel.Q);
                using (BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(20);
                }
            }
        }

        public async Task<string> GenerateTableTokenAsync(int tableId)
        {
            string token = Guid.NewGuid().ToString();

            // Token'ı cache'e kaydet (3 dakika geçerli)
            await _cache.SetStringAsync(
                $"table_access_{tableId}_{token}",
                token,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(TOKEN_EXPIRATION_MINUTES)
                }
            );

            return token;
        }

        public async Task<bool> ValidateTableTokenAsync(int tableId, string token)
        {
            var cachedToken = await _cache.GetStringAsync($"table_access_{tableId}_{token}");
            return !string.IsNullOrEmpty(cachedToken);
        }
    }
}
