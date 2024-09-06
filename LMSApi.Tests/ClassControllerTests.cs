using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using LMSApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace LMSApi.Tests
{
    public class ClassControllerTests
    {
        private readonly Mock<IClassService> _mockClassService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ClassController>> _mockLogger;
        private readonly ClassController _controller;

        public ClassControllerTests()
        {
            _mockClassService = new Mock<IClassService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ClassController>>();

            _controller = new ClassController(_mockClassService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetClassById_ClassExists_ReturnsOkResultWithClassData()
        {
            // Arrange
            int classId = 1;
            Class @class = new Class { Id = 1, Name = "class 1", Description = "class1" };
            _mockClassService.Setup(s => s.GetClassByIdAsync(classId))
                .ReturnsAsync(@class);

            var classEntity = _mockMapper.Setup(m => m.Map<ClassResponse>(@class))
                .Returns(new ClassResponse { Id = 1, Name = "class 1", Description = "class1" });


            //act
            var result = await _controller.GetClassById(classId);

            //assert
            Assert.IsType<ActionResult<ClassResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);


            var response = okResult.Value as ApiResponseSingleStrategy;
            Assert.NotNull(response);
            Assert.Equal("Class Exists", response.Message);
            Assert.Equal(201, response.Status);
            Assert.True(response.Success);


            ClassResponse? dataReturned = response.Data as ClassResponse;

            Assert.NotNull(dataReturned);


            Assert.Equal(1, dataReturned.Id);
            Assert.Equal("class 1", dataReturned.Name);
            Assert.Equal("class1", dataReturned.Description);



        }

        [Fact]
        public async Task GetClassById_ClassDoesNotExist_CatchNotFoundExceptionAndReturnNotFoundResult()
        {
            // Arrange
            int classId = 1;
            _mockClassService.Setup(s => s.GetClassByIdAsync(classId))
                .ThrowsAsync(new NotFoundException("Class Not Found"));

            // Act
            var result = await _controller.GetClassById(classId);

            // Assert
            Assert.IsType<ActionResult<ClassResponse>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            var response = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(response);
            Assert.Equal(404, response.Status);
            Assert.False(response.Success);
            Assert.Equal("Class Not Found", response.Message);

        }

        [Fact]
        public async Task GetClassById_ClassDoesNotExist_CatchUnhandeledExceptionAndReturnObjectResultWithCode500()
        {
            // Arrange
            int classId = 1;
            _mockClassService.Setup(s => s.GetClassByIdAsync(classId))
                .ThrowsAsync(new Exception("UnhandeledException"));

            // Act
            var result = await _controller.GetClassById(classId);

            // Assert
            Assert.IsType<ActionResult<ClassResponse>>(result);
            var notFoundResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, notFoundResult.StatusCode);

            var response = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(response);
            Assert.Equal(500, response.Status);
            Assert.False(response.Success);
            Assert.Equal("UnhandeledException", response.Message);

        }

        [Fact]
        public async Task GetAllClasses_WhenClasses_ReturnOkResultWithListOfClasseResponse()
        {
            // Arrange
            List<Class> classes = new List<Class>
            {
                new Class { Id = 1, Name = "class 1", Description = "class1" },
                new Class { Id = 2, Name = "class 2", Description = "class2" }
            };

            List<ClassResponse> classesResponse = new List<ClassResponse>
            {
                new ClassResponse { Id = 1, Name = "class 1", Description = "class1" },
                new ClassResponse { Id = 2, Name = "class 2", Description = "class2" }
            };

            _mockClassService.Setup(s => s.GetAllClassesAsync())
                .ReturnsAsync(classes);

            _mockMapper.Setup(m => m.Map<IEnumerable<ClassResponse>>(classes))
                .Returns(classesResponse);

            // Act
            var result = await _controller.GetAllClasses();
            ApiResponseListStrategy<ClassResponse>? response = ApiResponseFactory.Create(classesResponse, "Classes Fetched Successfully", 201, true) as ApiResponseListStrategy<ClassResponse>;

            // Assert
            Assert.IsType<ActionResult<IEnumerable<ClassResponse>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            // Compare properties of the response and the result
            var actualResponse = okResult.Value as ApiResponseListStrategy<ClassResponse>;

            Assert.NotNull(actualResponse);
            Assert.NotNull(response);
            Assert.Equal(response.Status, actualResponse.Status);
            Assert.Equal(response.Message, actualResponse.Message);
            Assert.Equal(response.Success, actualResponse.Success);

            // Compare the data lists item by item
            Assert.Equal(response.Data.Count(), actualResponse.Data.Count());
            for (int i = 0; i < response.Data.Count(); i++)
            {
                Assert.Equal(response.Data.ElementAt(i).Id, actualResponse.Data.ElementAt(i).Id);
                Assert.Equal(response.Data.ElementAt(i).Name, actualResponse.Data.ElementAt(i).Name);
                Assert.Equal(response.Data.ElementAt(i).Description, actualResponse.Data.ElementAt(i).Description);
            }
        }

        [Fact]
        public async Task GetAllClasses_WhenNoClasses_ReturnOkResultWithEmptyListOfClasseResponse()
        {
            // Arrange

            _mockClassService.Setup(s => s.GetAllClassesAsync())
                .ReturnsAsync((List<Class>)null);

            _mockMapper.Setup(m => m.Map<IEnumerable<ClassResponse>>((List<Class>)null))
                .Returns((List<ClassResponse>)null);

            // Act
            var result = await _controller.GetAllClasses();
            ApiResponseListStrategy<ClassResponse>? response = ApiResponseFactory.Create((List<ClassResponse>)null, "Classes Fetched Successfully", 201, true) as ApiResponseListStrategy<ClassResponse>;

            // Assert
            Assert.IsType<ActionResult<IEnumerable<ClassResponse>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            // Compare properties of the response and the result
            var actualResponse = okResult.Value as ApiResponseListStrategy<ClassResponse>;
            Assert.NotNull(actualResponse);
            Assert.NotNull(response);

            Assert.Equal(response.Status, actualResponse.Status);
            Assert.Equal(response.Message, actualResponse.Message);
            Assert.Equal(response.Success, actualResponse.Success);

            Assert.Null(response.Data);
            Assert.Null(actualResponse.Data);
        }

        [Fact]
        public async Task UpdateClass_ClassExists_ReturnsOkResult()
        {
            // Arrange
            int classId = 1;
            ClassRequest classDto = new ClassRequest { Name = "class 1", Description = "class1" };
            Class updatedClass = new Class { Id = 1, Name = "class 1", Description = "class1" };

            _mockClassService.Setup(s => s.UpdateClassAsync(classId, classDto))
                .ReturnsAsync(updatedClass);

            var classEntity = _mockMapper.Setup(m => m.Map<ClassResponse>(updatedClass))
                .Returns(new ClassResponse { Id = 1, Name = "class 1", Description = "class1" });

            //act
            var result = await _controller.UpdateClass(classId, classDto);

            //assert
            Assert.IsType<ActionResult<ClassResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);


            //test the result object
            var resultValue = okResult.Value as ApiResponseSingleStrategy;
            Assert.NotNull(resultValue);
            Assert.Equal("Class Updated Successfully", resultValue.Message);
            Assert.Equal(201, resultValue.Status);
            Assert.True(resultValue.Success);
            Assert.Equal(1, (resultValue.Data as ClassResponse).Id);
            Assert.Equal("class 1", (resultValue.Data as ClassResponse).Name);
            Assert.Equal("class1", (resultValue.Data as ClassResponse).Description);

        }
        [Fact]
        public async Task UpdateClass_ClassNotExists_CatchNoFoundExceptionAndReturnNotFoundResult()
        {
            // Arrange
            int classId = 1;
            ClassRequest classDto = new ClassRequest { Name = "class 1", Description = "class1" };

            _mockClassService.Setup(s => s.UpdateClassAsync(classId, classDto))
                .ThrowsAsync(new NotFoundException("Class Not Found"));

            //act
            var result = await _controller.UpdateClass(classId, classDto);

            //assert
            Assert.IsType<ActionResult<ClassResponse>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);


            //test the result object
            var resultValue = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(resultValue);
            Assert.Equal("Class Not Found", resultValue.Message);
            Assert.Equal(404, resultValue.Status);
            Assert.False(resultValue.Success);

        }

        [Fact]
        public async Task UpdateClass_ClassExistsOrNot_CatchNotHandeledExceptionAndReturnStatusCode500()
        {
            // Arrange
            int classId = 1;
            ClassRequest classDto = new ClassRequest { Name = "class 1", Description = "class1" };

            _mockClassService.Setup(s => s.UpdateClassAsync(classId, classDto))
                .ThrowsAsync(new Exception("Unhandeled exception"));

            //act
            var result = await _controller.UpdateClass(classId, classDto);

            //assert
            Assert.IsType<ActionResult<ClassResponse>>(result);
            Assert.IsType<ObjectResult>(result.Result);
            var notFoundResult = result.Result as ObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(500, notFoundResult.StatusCode);

            var resultValue = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(resultValue);
            Assert.Equal("Unhandeled exception", resultValue.Message);
            Assert.Equal(500, resultValue.Status);
            Assert.False(resultValue.Success);

        }


        [Fact]
        public async void CreateClass_WhenClassCreated_ReturnsOkResultWithClassResponse()
        {
            // Arrange
            ClassRequest classDto = new ClassRequest { Name = "class 1", Description = "class1" };
            Class @class = new Class
            {
                Id = 1,
                Name = "class 1",
                Description = "class1"
            };
            _mockClassService.Setup(s => s.CreateClassAsync(classDto))
                .ReturnsAsync(@class);

            _mockMapper.Setup(m => m.Map<ClassResponse>(@class))
                .Returns(new ClassResponse
                {
                    Id = 1,
                    Name = "class 1",
                    Description = "class1"
                });

            // Act
            var result = await _controller.CreateClass(classDto);

            // Assert
            Assert.IsType<ActionResult<ClassResponse>>(result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseSingleStrategy;
            Assert.NotNull(response);
            Assert.Equal("Success", response.Message);
            Assert.Equal(201, response.Status);
            Assert.True(response.Success);


            ClassResponse? returnedData = response.Data as ClassResponse;
            Assert.NotNull(returnedData);
            Assert.Equal(1, returnedData.Id);
            Assert.Equal("class 1", returnedData.Name);
            Assert.Equal("class1", returnedData.Description);
        }

        [Fact]
        public async void CreateClass_WhenClassCreatedOrNoButSomethingNotExcpectedHappedned_ReturnsObjectResultWithCode500()
        {
            // Arrange
            ClassRequest classDto = new ClassRequest { Name = "class 1", Description = "class1" };

            _mockClassService.Setup(s => s.CreateClassAsync(classDto))
                .ThrowsAsync(new Exception("Unhandeled exception"));


            // Act
            var result = await _controller.CreateClass(classDto);

            // Assert
            Assert.IsType<ActionResult<ClassResponse>>(result);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = objectResult.Value as ApiResponseBase;
            Assert.NotNull(response);
            Assert.Equal("Unhandeled exception", response.Message);
            Assert.Equal(500, response.Status);
            Assert.False(response.Success);

        }

    }
}
