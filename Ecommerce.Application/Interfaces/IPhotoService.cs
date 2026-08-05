using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<string> CreatePhotoAsync(byte[] fileBytes, string fileName);
        Task DeletePhotoAsync(string avatarUrl);
    }
}
