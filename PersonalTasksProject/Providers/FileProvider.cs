using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace PersonalTasksProject.Providers;

public sealed class FileProvider
{
    private readonly IWebHostEnvironment _environment;
    
    private readonly Cloudinary _cloudinary;

    private readonly string[] _allowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png"
    ];

    public FileProvider(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _cloudinary = new Cloudinary(configuration["CloudinaryURL"])
        {
            Api =
            {
                Secure = true
            }
        };
    }
    
    public async Task<string> SaveFileImageAsync(IFormFile imageFile)
    {
        if (imageFile == null) throw new ArgumentNullException(nameof(imageFile));
        
        var contentPath = _environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        var extension = Path.GetExtension(imageFile.FileName).ToLower();

        if (!_allowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"Only {string.Join(",", _allowedExtensions)} are allowed.");
        }

        var fileName = $"{Guid.NewGuid().ToString()}{extension}";
        var fileNameWithPath = Path.Combine(path, fileName);
        
        await using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileNameWithPath),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            
        };  
        
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        
        File.Delete(fileNameWithPath);
        return uploadResult.SecureUri.AbsoluteUri;

    }
}