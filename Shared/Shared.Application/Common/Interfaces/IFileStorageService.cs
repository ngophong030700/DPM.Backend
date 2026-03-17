using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<(string logicalPath, string physicalPath, long size, string extension)> UploadFileAsync(string base64, string fileName);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}
