using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using LMSApi.App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LMSApi.Tests
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseService> _mockCourseService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CoursesController>> _logger;
        private readonly CoursesController _coursesController;

        public CourseControllerTests()
        {
            _mockCourseService = new Mock<ICourseService>();
            _mockMapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<CoursesController>>();
            _coursesController = new CoursesController(_mockCourseService.Object, _mockMapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetById_WhenCourseExists_ReturnsOkResult()
        {
            // Arrange
            int courseId = 1;
            var course = new Course { Id = courseId, Name = "Course 1", Description = "Description 1" };
            var courseResponse = new CourseResponse { Id = courseId, Name = "Course 1", Description = "Description 1" };

            _mockCourseService.Setup(x => x.GetByIdAsync(courseId)).ReturnsAsync(course);
            _mockMapper.Setup(m => m.Map<CourseResponse>(course)).Returns(courseResponse);

            // Act
            var result = await _coursesController.GetById(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(courseResponse, apiResponse.Data);
        }

        [Fact]
        public async Task GetById_WhenCourseNotExists_CatchNotFoundExceptionAndReturnsNotFoundResult()
        {
            // Arrange
            int courseId = 1;

            _mockCourseService.Setup(x => x.GetByIdAsync(courseId))
                .ThrowsAsync(new NotFoundException("Course not found"));

            // Act
            var result = await _coursesController.GetById(courseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal("Course not found", apiResponse.Message);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task GetById_WhenUnHandeledExceptionThrown_CatchExceptionAndReturnsObjectResultWithCode500()
        {
            // Arrange
            int courseId = 1;

            _mockCourseService.Setup(x => x.GetByIdAsync(courseId))
                .ThrowsAsync(new Exception("Course not found"));

            // Act
            var result = await _coursesController.GetById(courseId);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, notFoundResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal("Course not found", apiResponse.Message);
            Assert.Equal(500, apiResponse.Status);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task Add_WhenValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var courseRequest = new CourseRequest { Id = 1, Name = "Course 1", Description = "Description 1" };
            Course course = new Course { Id = 1, Name = "Course 1", Description = "Description 1" };
            CourseResponse courseResponse = new CourseResponse { Id = 1, Name = "Course 1", Description = "Description 1" };
            _mockCourseService.Setup(s => s.AddAsync(courseRequest))
                .ReturnsAsync(course);

            _mockMapper.Setup(m => m.Map<CourseResponse>(course))
                .Returns(courseResponse);

            // Act
            var result = await _coursesController.Add(courseRequest);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, createdResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(createdResult.Value);
            Assert.Equal(201, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal("Course created successfully", apiResponse.Message);
            Assert.Equal(courseResponse, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenNotFoundExceptionThrown_CatchNotFoundExceptionAndReturnNotFoundResult()
        {
            // Arrange
            var courseRequest = new CourseRequest { Id = 1, Name = "Course 1", Description = "Description 1" };

            _mockCourseService.Setup(s => s.AddAsync(courseRequest))
                .ThrowsAsync(new NotFoundException("exception"));

            // Act
            var result = await _coursesController.Add(courseRequest);

            // Assert
            var createdResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, createdResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(createdResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("exception", apiResponse.Message);
        }
        
        [Fact]
        public async Task Add_WhenUnHandeledExceptionThrown_CatchExceptionAndReturnsObjectResultWithCode500()
        {
            // Arrange
            var courseRequest = new CourseRequest { Id = 1, Name = "Course 1", Description = "Description 1" };

            _mockCourseService.Setup(s => s.AddAsync(courseRequest))
                .ThrowsAsync(new Exception("exception"));

            // Act
            var result = await _coursesController.Add(courseRequest);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, createdResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(createdResult.Value);
            Assert.Equal(500, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("exception", apiResponse.Message);
        }


        [Fact]
        public async Task Update_WhenCourseExists_ReturnsOkResult()
        {
            // Arrange
            int courseId = 1;
            var courseRequest = new CourseRequest { Id = courseId, Name = "Updated Course", Description = "Updated Description" };
            var existingCourse = new Course { Id = courseId, Name = "Old Course", Description = "Old Description" };

            _mockCourseService.Setup(x => x.GetByIdAsync(courseId)).ReturnsAsync(existingCourse);

            // Act
            var result = await _coursesController.Update(courseId, courseRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal("Course updated successfully", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenCourseNotExists_CatchNotFoundExceptionAndReturnsNotFoundResult()
        {
            // Arrange
            int courseId = 1;
            var courseRequest = new CourseRequest { Id = courseId, Name = "Updated Course", Description = "Updated Description" };

            _mockCourseService.Setup(x => x.GetByIdAsync(courseId))
                .Throws(new NotFoundException("Course not found"));

            // Act
            var result = await _coursesController.Update(courseId, courseRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal("Course not found", apiResponse.Message);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task Delete_WhenCourseExists_ReturnsOkResult()
        {
            // Arrange
            int courseId = 1;

            // Act
            var result = await _coursesController.Delete(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal("Course deleted successfully", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenCourseNotExists_ReturnsNotFoundResult()
        {
            // Arrange
            int courseId = 1;
            _mockCourseService.Setup(x => x.DeleteAsync(courseId)).Throws(new NotFoundException("Course not found"));

            // Act
            var result = await _coursesController.Delete(courseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal("Course not found", apiResponse.Message);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
        }
    }
}
