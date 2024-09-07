using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using LMSApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LMSApi.Tests
{
    public class LessonContentControllerTests
    {
        private readonly LessonContentController _controller;
        private readonly Mock<ILessonContentService> _mockLessonContentService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<LessonContentController>> _mockLogger;

        public LessonContentControllerTests()
        {
            _mockLessonContentService = new Mock<ILessonContentService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<LessonContentController>>();
            _controller = new LessonContentController(_mockLessonContentService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        private LessonContent CreateLessonContent(int id) => new LessonContent
        {
            Id = id,
            Name = $"Content {id}",
            Type = id % 2 == 0 ? LessonContentTypesEnum.Video : LessonContentTypesEnum.Audio,
            Link = $"http://example{id}.com",
            Content = $"Content {id} description"
        };

        private LessonContentResponse CreateLessonContentResponse(int id) => new LessonContentResponse
        {
            Id = id,
            Name = $"Content {id}",
            Type = id % 2 == 0 ? LessonContentTypesEnum.Video : LessonContentTypesEnum.Audio,
            Link = $"http://example{id}.com",
            Content = $"Content {id} description"
        };

        [Fact]
        public async Task GetById_LessonContentExists_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var lessonContent = CreateLessonContent(id);
            var lessonContentResponse = CreateLessonContentResponse(id);

            _mockLessonContentService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(lessonContent);
            _mockMapper.Setup(mapper => mapper.Map<LessonContentResponse>(lessonContent)).Returns(lessonContentResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lessonContentResponse, apiResponse.Data);
        }

        [Fact]
        public async Task GetById_LessonContentDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _mockLessonContentService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((LessonContent)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Lesson content not found", apiResponse.Message);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithLessonContents()
        {
            // Arrange
            var lessonContents = new List<LessonContent>
            {
                CreateLessonContent(1),
                CreateLessonContent(2)
            };
            var lessonContentResponses = new List<LessonContentResponse>
            {
                CreateLessonContentResponse(1),
                CreateLessonContentResponse(2)
            };

            _mockLessonContentService.Setup(service => service.GetAllAsync()).ReturnsAsync(lessonContents);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<LessonContentResponse>>(lessonContents)).Returns(lessonContentResponses);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<LessonContentResponse>>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lessonContentResponses, apiResponse.Data);
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new LessonContentRequest { LessonId = 1, Name = "New Content", Type = LessonContentTypesEnum.Video, Content = "New content" };
            var lessonContent = CreateLessonContent(1);
            var lessonContentResponse = CreateLessonContentResponse(1);

            _mockLessonContentService.Setup(service => service.LessonExistsAsync(request.LessonId)).ReturnsAsync(true);
            _mockLessonContentService.Setup(service => service.CreateAsync(request)).ReturnsAsync(lessonContent);
            _mockMapper.Setup(mapper => mapper.Map<LessonContentResponse>(lessonContent)).Returns(lessonContentResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(createdResult.Value);
            Assert.Equal(201, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lessonContentResponse, apiResponse.Data);
        }

        [Fact]
        public async Task Create_LessonDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var request = new LessonContentRequest { LessonId = 1, Name = "New Content", Type = LessonContentTypesEnum.Video, Content = "New content" };

            _mockLessonContentService.Setup(service => service.LessonExistsAsync(request.LessonId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(badRequestResult.Value);
            Assert.Equal(400, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Lesson with the specified ID does not exist.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_LessonContentExists_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var request = new LessonContentRequest { Name = "Updated Content", Type = LessonContentTypesEnum.Audio, Content = "Updated content" };
            var existingLessonContent = CreateLessonContent(id);
            var updatedLessonContent = CreateLessonContent(id);
            updatedLessonContent.Name = "Updated Content";
            updatedLessonContent.Type = LessonContentTypesEnum.Audio;
            updatedLessonContent.Content = "Updated content";

            var lessonContentResponse = CreateLessonContentResponse(id);
            lessonContentResponse.Name = "Updated Content";
            lessonContentResponse.Type = LessonContentTypesEnum.Audio;
            lessonContentResponse.Content = "Updated content";

            _mockLessonContentService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(existingLessonContent);
            _mockLessonContentService.Setup(service => service.UpdateAsync(id, request)).ReturnsAsync(updatedLessonContent);
            _mockMapper.Setup(mapper => mapper.Map<LessonContentResponse>(updatedLessonContent)).Returns(lessonContentResponse);

            // Act
            var result = await _controller.Update(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lessonContentResponse, apiResponse.Data);
        }

        [Fact]
        public async Task Update_LessonContentDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var request = new LessonContentRequest { Name = "Updated Content", Type = LessonContentTypesEnum.Audio, Content = "Updated content" };

            _mockLessonContentService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((LessonContent)null);

            // Act
            var result = await _controller.Update(id, request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Lesson content not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_LessonContentExists_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var lessonContent = CreateLessonContent(id);
            _mockLessonContentService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(lessonContent);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task Delete_LessonContentDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _mockLessonContentService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((LessonContent)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Lesson content not found", apiResponse.Message);
        }
    }
}
