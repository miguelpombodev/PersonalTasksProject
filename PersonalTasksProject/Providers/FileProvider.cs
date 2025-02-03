using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace PersonalTasksProject.Providers;

public sealed class FileProvider
{
    private readonly IWebHostEnvironment _environment;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _fileKeyBase = "uploads/";

    private readonly string[] _allowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png"
    ];

    public FileProvider(IWebHostEnvironment environment, IAmazonS3 s3Client, IConfiguration configuration)
    {
        _environment = environment;
        _s3Client = s3Client;
        _bucketName = configuration["AWSS3:BucketName"];
    }
    
    public async Task<string> SaveFileImageAsync(IFormFile imageFile)
    {
        if (imageFile == null | imageFile.Length <= 0) throw new ArgumentNullException(nameof(imageFile));
        using (var stream = new MemoryStream())
        {
            await imageFile.CopyToAsync(stream);
        
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{_fileKeyBase}{imageFile.FileName}",
                InputStream = stream,
            };
            
            var response = await _s3Client.PutObjectAsync(putRequest);
        
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Error saving image");
            } 
            
            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{_fileKeyBase}{imageFile.FileName}",
                Expires = DateTime.UtcNow.AddHours(1)
            };
            var presignedUrl = _s3Client.GetPreSignedURL(urlRequest);
        
            return presignedUrl;
        }
    }
}