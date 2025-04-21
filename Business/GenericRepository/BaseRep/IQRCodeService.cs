using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(int tableId, string baseUrl);
        Task<string> GenerateTableTokenAsync(int tableId);
        Task<bool> ValidateTableTokenAsync(int tableId, string token);
    }
}
