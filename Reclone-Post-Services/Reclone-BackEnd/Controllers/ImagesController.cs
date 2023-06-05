using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Microsoft.Extensions.Configuration;
using Reclone_BackEnd.ServiceBus;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly Cloudinary _cloudinary;
    private readonly ContentModeratorClient _client;

    public ImagesController(IConfiguration configuration)
    {
        var cloudinaryCloudName = configuration["CLOUDINARY_CLOUD_NAME"];
        var cloudinaryApiKey = configuration["CLOUDINARY_API_KEY"];
        var cloudinaryApiSecret = configuration["CLOUDINARY_API_SECRET"];
        var contentModeratorKey = configuration["AZURE_CONTENT_KEY"];
        var contentModeratorEndpoint = configuration["AZURE_CONTENT_ENDPOINT"];

        Account account = new Account(cloudinaryCloudName, cloudinaryApiKey, cloudinaryApiSecret);
        _cloudinary = new Cloudinary(account);
        _client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(contentModeratorKey)) { Endpoint = contentModeratorEndpoint };
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        // Check if file is null
        if (file == null || file.Length == 0)
            return BadRequest("No image file was uploaded.");

        // Check that the image size is within limits
        if (file.Length > 100 * 1024) // 100 KB = 100 * 1024 bytes
        {
            return BadRequest("File size cannot be greater than 100 KB.");
        }

        // Upload image to Cloudinary
        try
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = "my_image3"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Return the uploaded image URL
            return Ok(uploadResult.SecureUrl.AbsoluteUri);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost("review-image")]
    public async Task<IActionResult> ReviewImageUpload(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return BadRequest("Please provide a valid image file.");
        }

        // Create a memory stream from the uploaded file
        MemoryStream stream = new MemoryStream();
        await imageFile.CopyToAsync(stream);
        stream.Position = 0;

        // Analyze the image
        try
        {
            var result = await _client.ImageModeration.EvaluateFileInputAsync(stream, cacheImage: true);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("uploadVideo")]
    public async Task<IActionResult> UploadVideo(IFormFile video)
    {
        // Check if file is null
        if (video == null || video.Length == 0)
            return BadRequest("No video file was uploaded.");

        // Check that the file is a video
        var validVideoTypes = new[] { "mp4", "avi", "mov" };
        var fileExtension = Path.GetExtension(video.FileName).ToLower().Trim('.');
        if (!validVideoTypes.Contains(fileExtension))
        {
            return BadRequest("Only mp4, avi, and mov video formats are supported.");
        }

        // Check that the video size is within limits
        if (video.Length > 20 * 1024 * 1024) // 20 MB = 20 * 1024 * 1024 bytes
        {
            return BadRequest("Video file size cannot be greater than 20MB.");
        }

        // Upload video to Cloudinary
        try
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(video.FileName, video.OpenReadStream()),
                PublicId = "my_video3"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Return the uploaded video URL
            return Ok(uploadResult.SecureUrl.AbsoluteUri);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

   
    [HttpPost("send-to-service-bus")]
    public async Task<IActionResult> SendToServiceBus([FromBody] Reclone_BackEnd.Models.Image imageDetails, [FromServices] IServiceBus serviceBus)
    {
        if (imageDetails == null)
        {
            return BadRequest("No image details provided.");
        }

        try
        {
            // Call the SendMessageAsync method of the service bus
            await serviceBus.SendMessageAsync(imageDetails);

            return Ok("Image details sent to Service Bus successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to send image details to Service Bus: {ex.Message}");
        }
    }


}
