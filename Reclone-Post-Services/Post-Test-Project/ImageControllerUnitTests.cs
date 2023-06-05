/*using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Reclone_BackEnd.ServiceBus;
using Xunit;

namespace Post_Test_Project
{
    public class ImagesControllerTests
    {
        private readonly ImagesController _controller;

        public ImagesControllerTests()
        {
            // Mock IConfiguration
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["CLOUDINARY_CLOUD_NAME"]).Returns("your_cloud_name");
            configuration.Setup(c => c["CLOUDINARY_API_KEY"]).Returns("your_api_key");
            configuration.Setup(c => c["CLOUDINARY_API_SECRET"]).Returns("your_api_secret");
            configuration.Setup(c => c["AZURE_CONTENT_KEY"]).Returns("your_content_moderator_key");
            configuration.Setup(c => c["AZURE_CONTENT_ENDPOINT"]).Returns("your_content_moderator_endpoint");

            // Mock IServiceBus
            var serviceBus = new Mock<IServiceBus>();

            _controller = new ImagesController(configuration.Object)
            {
                ControllerContext = new ControllerContext(),
            };
        }

        [Fact]
        public async Task Upload_ValidImage_ReturnsOkResult()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.Length).Returns(100 * 1024); // 100 KB

            // Act
            var result = await _controller.Upload(formFile.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Upload_InvalidImageSize_ReturnsBadRequest()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.Length).Returns(200 * 1024); // 200 KB

            // Act
            var result = await _controller.Upload(formFile.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ReviewImageUpload_ValidImage_ReturnsOkResult()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.Length).Returns(100 * 1024); // 100 KB

            // Act
            var result = await _controller.ReviewImageUpload(formFile.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ReviewImageUpload_InvalidImage_ReturnsBadRequest()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.Length).Returns(200 * 1024); // 200 KB

            // Act
            var result = await _controller.ReviewImageUpload(formFile.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadVideo_ValidVideo_ReturnsOkResult()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.Length).Returns(10 * 1024 * 1024); // 10 MB
            formFile.Setup(f => f.FileName).Returns("video.mp4");

            // Act
            var result = await _controller.UploadVideo(formFile.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UploadVideo_InvalidVideoSize_ReturnsBadRequest()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.Length).Returns(30 * 1024 * 1024); // 30 MB
            formFile.Setup(f => f.FileName).Returns("video.mp4");

            // Act
            var result = await _controller.UploadVideo(formFile.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SendToServiceBus_ValidImageDetails_ReturnsOkResult()
        {
            // Arrange
            var imageDetails = new Reclone_BackEnd.Models.Image();

            // Mock IServiceBus
            var serviceBus = new Mock<IServiceBus>();
            serviceBus.Setup(s => s.SendMessageAsync(imageDetails)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendToServiceBus(imageDetails, serviceBus.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SendToServiceBus_NullImageDetails_ReturnsBadRequest()
        {
            // Arrange
            Reclone_BackEnd.Models.Image imageDetails = null;

            // Mock IServiceBus
            var serviceBus = new Mock<IServiceBus>();

            // Act
            var result = await _controller.SendToServiceBus(imageDetails, serviceBus.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
*/