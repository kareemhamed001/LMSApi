using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;

//using DataAccessLayer.Exceptions;
using LMSApi.App.Controllers;
using LMSApi.App.Options;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LMSApi.Tests
{
    public class TeachersControllerTests
    {
        private readonly TeachersController _controller;
        private readonly Mock<ITeacherService> _teacherServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TeachersController>> _loggerMock;
        private readonly JwtOptions _jwtOptions;

        public TeachersControllerTests()
        {
            _teacherServiceMock = new Mock<ITeacherService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TeachersController>>();
            _jwtOptions = new JwtOptions();

            _controller = new TeachersController(
                _teacherServiceMock.Object,
                _loggerMock.Object,
                _jwtOptions,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Index_ReturnsOkResult_WithTeachers()
        {
            // Arrange
            var teachers = new List<Teacher>
        {
            new Teacher { Id = 1, NickName = "John", Phone = "1234567890", Email = "john@example.com" }
        };
            var teacherResponses = new List<TeacherResponse>
        {
            new TeacherResponse { Id = 1, NickName = "John", Phone = "1234567890", Email = "john@example.com" }
        };

            _teacherServiceMock.Setup(service => service.Index()).ReturnsAsync(teachers);
            _mapperMock.Setup(mapper => mapper.Map<List<TeacherResponse>>(teachers)).Returns(teacherResponses);

            // Act
            var result = await _controller.Index();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<TeacherResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Teachers fetched successfully", apiResponse.Message);
            Assert.NotNull(apiResponse.Data);
            Assert.Single(apiResponse.Data);
        }
        [Fact]
        public async Task Index_HandlesException_ReturnsInternalServerError()
        {
            // Arrange
            _teacherServiceMock.Setup(service => service.Index()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Index();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            var apiResponse = Assert.IsType<ApiResponseBase>(objectResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Internal server error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_OnSuccessfulDeletion()
        {

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var noContentResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, noContentResult.StatusCode);
        }
        [Fact]
        public async Task Delete_WhenTeacherNotFound_ReturnsNotFound()
        {
            // Arrange
            var teacherId = 1;
            _teacherServiceMock.Setup(s => s.Delete(teacherId))
                    .ThrowsAsync(new NotFoundException("teacher Not Found"));

            // Act
            var result = await _controller.Delete(teacherId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
            var reurnedData = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(reurnedData);
            Assert.Equal("teacher Not Found", reurnedData.Message);
            Assert.Equal(404, reurnedData.Status);
            Assert.False(reurnedData.Success);
        }


        [Fact]
        public async Task Delete_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int teacherId = 1;
            _teacherServiceMock.Setup(service => service.Delete(teacherId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Delete(teacherId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            var apiResponse = Assert.IsType<ApiResponseBase>(objectResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Internal server error", apiResponse.Message);
        }

        [Fact]
        public async Task Show_ReturnsOkResult_WhenTeacherIsFound_ReturnsOkResult()
        {
            // Arrange
            int teacherId = 1;
            var teacher = new Teacher { Id = teacherId }; 
            var response = new ShowTeacherResponse(); 
            _teacherServiceMock.Setup(s => s.Show(teacherId)).ReturnsAsync(teacher);
            _mapperMock.Setup(m => m.Map<ShowTeacherResponse>(teacher)).Returns(response);

            // Act
            var result = await _controller.Show(teacherId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(response, apiResponse.Data);
        }
        [Fact]
        public async Task Show_ReturnsNotFound_WhenTeacherIsNotFound()
        {
            // Arrange
            _teacherServiceMock.Setup(service => service.Show(It.IsAny<int>()))
                .ThrowsAsync(new NotFoundException("lesson not found"));

            // Act
            var result = await _controller.Show(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("lesson not found", apiResponse.Message);
        }
        [Fact]
        public async Task Show_WhenUnHandeledExceptionHappened_ReturnsResponseWithStatusCode500()
        {
            // Arrange

            _teacherServiceMock.Setup(service => service.Show(1))
                .ThrowsAsync(new Exception("database error"));

            // Act
            var result = await _controller.Show(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IApiResponse>>(result);

            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(objectResult.Value);
            Assert.NotNull(apiResponse);
            Assert.False(apiResponse.Success);
            Assert.Equal("Internal server error", apiResponse.Message);
        }
     
    }
}