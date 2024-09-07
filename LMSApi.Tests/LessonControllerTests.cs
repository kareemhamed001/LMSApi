using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using LMSApi.Controllers;
using AutoMapper;
using BusinessLayer.Interfaces;
using DataAccessLayer.Exceptions;


namespace LMSApi.Tests
{
    public class LessonsControllerTests
    {
        private readonly LessonsController _controller;
        private readonly Mock<ILessonService> _mockLessonService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<LessonsController>> _mockLogger;

        public LessonsControllerTests()
        {
            _mockLessonService = new Mock<ILessonService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<LessonsController>>();
            _controller = new LessonsController(_mockLessonService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllLessons_ReturnsOkResult_WithLessons()
        {
            // Arrange
            var lessons = new List<Lesson>
            {
                new Lesson { Id = 1, Name = "Lesson 1", Description = "Description 1" },
                new Lesson { Id = 2, Name = "Lesson 2", Description = "Description 2" }
            };
            var lessonResponses = new List<LessonResponse>
            {
                new LessonResponse { Id = 1, Name = "Lesson 1", Description = "Description 1" },
                new LessonResponse { Id = 2, Name = "Lesson 2", Description = "Description 2" }
            };

            _mockLessonService.Setup(service => service.GetAllLessonsAsync()).ReturnsAsync(lessons);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<LessonResponse>>(lessons)).Returns(lessonResponses);

            // Act
            var result = await _controller.GetAllLessons();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<LessonResponse>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<LessonResponse>>(okResult.Value);
            Assert.Equal(201, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lessonResponses, apiResponse.Data);
        }

        [Fact]
        public async Task GetLessonById_LessonExists_ReturnsOkResult()
        {
            // Arrange
            var lesson = new Lesson { Id = 1, Name = "Lesson 1", Description = "Description 1" };
            var lessonResponse = new LessonResponse { Id = 1, Name = "Lesson 1", Description = "Description 1" };

            _mockLessonService.Setup(service => service.GetLessonByIdAsync(1)).ReturnsAsync(lesson);
            _mockMapper.Setup(mapper => mapper.Map<LessonResponse>(lesson)).Returns(lessonResponse);

            // Act
            var result = await _controller.GetLessonById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lessonResponse, apiResponse.Data);
        }

        [Fact]
        public async Task GetLessonById_LessonDoesNotExist_CatchNotFoundExceptionAndReturnNotFoundResponse()
        {
            // Arrange
            _mockLessonService.Setup(service => service.GetLessonByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new NotFoundException("lesson not found"));

            // Act
            var result = await _controller.GetLessonById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("lesson not found", apiResponse.Message);
        }


        [Fact]
        public async Task GetLessonById_WhenUnHandeledExceptionHappened_ReturnsResponseWithStatusCode500()
        {
            // Arrange

            _mockLessonService.Setup(service => service.GetLessonByIdAsync(1))
                .ThrowsAsync(new Exception("database error"));

            // Act
            var result = await _controller.GetLessonById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);

            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(objectResult.Value);
            Assert.NotNull(apiResponse);
            Assert.False(apiResponse.Success);
            Assert.Equal("Internal server error", apiResponse.Message);
        }



        [Fact]
        public async Task CreateLesson_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new LessonRequest { Name = "New Lesson", Description = "New Description", CourseId = 1, SectionNumber = 1 };
            var lesson = new Lesson { Id = 1, Name = "New Lesson", Description = "New Description", CourseId = 1, SectionNumber = 1 };

            _mockLessonService.Setup(service => service.CreateLessonAsync(It.IsAny<Lesson>())).ReturnsAsync(lesson);

            // Act
            var result = await _controller.CreateLesson(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(createdResult.Value);
            Assert.Equal(201, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(lesson, apiResponse.Data);
        }

        [Fact]
        public async Task UpdateLesson_LessonExists_ReturnsOkResult()
        {
            // Arrange
            var request = new LessonRequest { Name = "Updated Lesson", Description = "Updated Description", CourseId = 1, SectionNumber = 1 };
            var updatedLesson = new Lesson { Id = 1, Name = "Updated Lesson", Description = "Updated Description" };

            _mockLessonService.Setup(service => service.UpdateLessonAsync(1, It.IsAny<Lesson>())).ReturnsAsync(updatedLesson);

            // Act
            var result = await _controller.UpdateLesson(1, request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(updatedLesson, apiResponse.Data);
        }
        [Fact]
        public async Task UpdateLesson_LessonDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var request = new LessonRequest { Name = "Updated Lesson", Description = "Updated Description", CourseId = 1, SectionNumber = 1 };
            _mockLessonService.Setup(service => service.UpdateLessonAsync(1, It.IsAny<Lesson>())).ReturnsAsync((Lesson)null);

            // Act
            var result = await _controller.UpdateLesson(1, request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Lesson not found", apiResponse.Message);
        }

        [Fact]
        public async Task DeleteLesson_ReturnsNoContent()
        {
            // Act
            var result = await _controller.DeleteLesson(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var noContentResult = Assert.IsType<NoContentResult>(actionResult.Result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task CourseExists_ReturnsOkResult()
        {
            // Arrange
            _mockLessonService.Setup(service => service.CourseExistsAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.CourseExists(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.True((bool)apiResponse.Data);
        }

        [Fact]
        public async Task CourseExists_CourseDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockLessonService.Setup(service => service.CourseExistsAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.CourseExists(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.False((bool)apiResponse.Data);
        }
    }
}
