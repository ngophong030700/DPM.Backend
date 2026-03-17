using Shared.Application.Common.Interfaces;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class FileStorageService : IFileStorageService
{
    private readonly string _storageFolder;

    public FileStorageService()
    {
        _storageFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "E:/Storage"
            : "/storage"; // Linux container or server
    }

    public async Task<(string logicalPath, string physicalPath, long size, string extension)> UploadFileAsync(string base64, string fileName)
    {
        if (string.IsNullOrEmpty(base64))
            throw new ArgumentException("Base64 content is empty");

        // Loại header nếu base64 dạng: data:image/png;base64,...
        var parts = base64.Split(",");
        string base64Data = parts.Length > 1 ? parts[1] : parts[0];

        byte[] fileBytes;
        try
        {
            fileBytes = Convert.FromBase64String(base64Data);
        }
        catch
        {
            throw new Exception("Base64 không hợp lệ");
        }

        string extension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extension))
            extension = GetImageExtension(fileBytes);

        // Tạo thư mục ngày/tháng/năm cho gọn
        string dateFolder = DateTime.Now.ToString("yyyyMMdd");
        string physicalFolder = Path.Combine(_storageFolder, dateFolder);

        Directory.CreateDirectory(physicalFolder);

        // Tên file unique
        string newFileName = $"{Guid.NewGuid()}{extension}";
        string physicalPath = Path.Combine(physicalFolder, newFileName);
        physicalPath = physicalPath.Replace("\\", "/"); // chuẩn hóa đường dẫn

        await File.WriteAllBytesAsync(physicalPath, fileBytes);

        // logical path cung cấp cho client → sử dụng cho static file
        string logicalPath = $"/storage/{dateFolder}/{newFileName}";
        logicalPath = logicalPath.Replace("\\", "/"); // chuẩn hóa đường dẫn

        return (logicalPath, physicalPath, fileBytes.LongLength, extension);
    }

    public Task<bool> DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return Task.FromResult(false);

        // fileUrl dạng /storage/yyyyMMdd/abc.png
        var cleanedUrl = fileUrl.Replace("/storage", "").TrimStart('/');

        string physicalPath = Path.Combine(_storageFolder, cleanedUrl);

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    private string GetImageExtension(byte[] bytes)
    {
        if (bytes.Length < 8)
            return ".png"; // fallback

        if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            return ".png";

        if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            return ".jpg";

        if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
            return ".gif";

        if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            return ".bmp";

        if (bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[8] == 0x57)
            return ".webp";

        return ".png"; // fallback
    }
}
