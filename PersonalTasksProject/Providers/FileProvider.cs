namespace PersonalTasksProject.Providers;

public sealed class FileProvider
{
    private readonly IWebHostEnvironment _environment;

    public FileProvider(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public async Task<string> SaveFileImageAsync(IFormFile imageFile, string[] allowedExtensions)
    {
        
        if (imageFile == null) throw new ArgumentNullException(nameof(imageFile));
        
        var contentPath = _environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        var extension = Path.GetExtension(imageFile.FileName);

        if (!allowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedExtensions)} are allowed.");
        }

        var fileName = $"{Guid.NewGuid().ToString()}{extension}";
        var fileNameWithPath = Path.Combine(path, fileName);
        
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return fileNameWithPath;

    }
}