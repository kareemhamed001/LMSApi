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

        [Fact]
        public async void DeleteClass_WhenClassExistsAndDeleted_ReturnOKResult()
        {
            int classId = 1;
            _mockClassService.Setup(s => s.DeleteClassAsync(classId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteClass(classId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async void DeleteClass_WhenClassNotExists_CatchNotFoundExceptionAndReturnNotFoundResponse()
        {
            int classId = 1;
            _mockClassService.Setup(s => s.DeleteClassAsync(classId))
                .ThrowsAsync(new NotFoundException("Class Not Found"));

            // Act
            var result = await _controller.DeleteClass(classId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            var reurnedData = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(reurnedData);
            Assert.Equal("Class Not Found", reurnedData.Message);
            Assert.Equal(404, reurnedData.Status);
            Assert.False(reurnedData.Success);
        }

        [Fact]
        public async void DeleteClass_WhenUnHandeledException_CatchUnHandeledExceptionAndReturnObjectResultWithCode500()
        {
            int classId = 1;
            _mockClassService.Setup(s => s.DeleteClassAsync(classId))
                .ThrowsAsync(new Exception("UnHandeled Exception"));

            // Act
            var result = await _controller.DeleteClass(classId);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, notFoundResult.StatusCode);
            var reurnedData = notFoundResult.Value as ApiResponseBase;
            Assert.NotNull(reurnedData);
            Assert.Equal("UnHandeled Exception", reurnedData.Message);
            Assert.Equal(500, reurnedData.Status);
            Assert.False(reurnedData.Success);
        }

        [Fact]
        public async void GetStudentsByClassId_WhenClassExists_ReturnListOfStudentsOfClass()
        {
            int classId = 1;
            List<Student> students = new List<Student>
            {
                new Student { Id = 1, ParentPhone = "0123456789", UserId = 1,ClassId=1,User=new User{
                    Id=1
                    ,FirstName="studednt1"
                    ,LastName="Student1"
                    ,Email="student1@example.com"
                    ,Phone="01123456789"
                    ,Password="asdjahsdkjasgdjhagfdhjasgdfjhasgfjhadsgfasdhjgf"
                },
                Class=new Class{
                    Id=1,
                    Name="class1",
                    Description="class1"
                }

                },
                new Student { Id = 2, ParentPhone = "0133456789", UserId = 2,ClassId=1,User=new User{
                    Id=2
                    ,FirstName="studednt2"
                    ,LastName="Student2"
                    ,Email="student2@example.com"
                    ,Phone="01223456789"
                    ,Password="asdjahsdkjasgdjhagfdhjasgdfjhasgfjhadsgfasdhjgf"
                },
                Class=new Class{
                    Id=1,
                    Name="class1",
                    Description="class1"
                }

                },
            };
            _mockClassService.Setup(s => s.GetStudentsByClassIdAsync(classId))
                .ReturnsAsync(students);

            _mockMapper.Setup(m => m.Map<IEnumerable<StudentResponse>>(students))
                .Returns(new List<StudentResponse>
                {
                    new StudentResponse { Id = 1, ParentPhone = "0123456789", User = new UserResponse {
                    Id=1
                    ,FirstName="studednt1"
                    ,LastName="Student1"
                    ,Email="student1@example.com"
                    ,Phone="01123456789"
                    }
                    ,Class=new ClassResponse{
                        Id=1,
                        Name="class1",
                        Description="class1"
                    }
                    },
                    new StudentResponse { Id = 2, ParentPhone = "0133456789", User = new UserResponse {
                    Id=2
                    ,FirstName="studednt2"
                    ,LastName="Student2"
                    ,Email="student2@example.com"
                    ,Phone="01223456789"
                    }
                    ,Class=new ClassResponse{
                        Id=1,
                        Name="class1",
                        Description="class1"
                    }
                    },
                });

            // Act
            var result = await _controller.GetStudentsByClassId(classId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var reurnedData = okObjectResult.Value as ApiResponseListStrategy<StudentResponse>;
            Assert.NotNull(reurnedData);
            Assert.Equal("Students Fetched Successfully", reurnedData.Message);
            Assert.Equal(201, reurnedData.Status);
            Assert.True(reurnedData.Success);

            var data = reurnedData.Data.ToList();
            Assert.Equal(2, data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Assert.Equal(students[i].Id, data[i].Id);
                Assert.Equal(students[i].ParentPhone, data[i].ParentPhone);
                Assert.Equal(students[i].User.Id, data[i].User.Id);
                Assert.Equal(students[i].User.FirstName, data[i].User.FirstName);
                Assert.Equal(students[i].User.LastName, data[i].User.LastName);
                Assert.Equal(students[i].User.Email, data[i].User.Email);
                Assert.Equal(students[i].User.Phone, data[i].User.Phone);
                Assert.Equal(students[i].Class.Id, data[i].Class.Id);
                Assert.Equal(students[i].Class.Name, data[i].Class.Name);
                Assert.Equal(students[i].Class.Description, data[i].Class.Description);
            }
        }

        [Fact]
        public async void GetStudentsByClassId_WhenClassNotExistsOrNoStudentsExists_ReturnEmptyListOfStudentsResponse()
        {
            int classId = 1;

            _mockClassService.Setup(s => s.GetStudentsByClassIdAsync(classId))
                .ReturnsAsync((List<Student>)null);

            _mockMapper.Setup(m => m.Map<IEnumerable<StudentResponse>>((List<Student>)null))
                .Returns((List<StudentResponse>)null);

            // Act
            var result = await _controller.GetStudentsByClassId(classId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var reurnedData = okObjectResult.Value as ApiResponseListStrategy<StudentResponse>;
            Assert.NotNull(reurnedData);
            Assert.Equal("Students Fetched Successfully", reurnedData.Message);
            Assert.Equal(201, reurnedData.Status);
            Assert.True(reurnedData.Success);

            var data = reurnedData?.Data?.ToList();
            Assert.Null(data);

        }

        [Fact]
        public async void GetCoursesByClassId_WhenClassExists_ReturnListOfCoursesOfClass()
        {
            int classId = 1;
            List<Course> courses = new List<Course>
            {
                new Course { Id = 1
                , Name = "course 1"
                , Description = "course1"
                , ClassId = classId
                , Price = 100
                , SubjectId = 1
                ,TeacherId = 1,
                Teacher=new Teacher{
                    Id=1,
                    UserId=1,
                    Email="teacher@emaple.com",
                    Phone="0123456789",
                    User=new User{
                        Id=1,
                        FirstName="teacher1",
                        LastName="teacher1",
                        Email="teacher@emaple.com",
                        Phone="0123456789",
                        Password="asdjahsdkjasgdjhagfdhjasgdfjhasgfjhadsgfasdhjgf"
                    }
                }
                },
                new Course { Id = 2
                , Name = "course 2"
                , Description = "course2"
                , ClassId = classId
                , Price = 120
                , SubjectId = 1
                ,TeacherId = 1,
                Teacher=new Teacher{
                    Id=1,
                    UserId=1,
                    Email="teacher@emaple.com",
                    Phone="0123456789",
                    User=new User{
                        Id=1,
                        FirstName="teacher1",
                        LastName="teacher1",
                        Email="teacher@emaple.com",
                        Phone="0123456789",
                        Password="asdjahsdkjasgdjhagfdhjasgdfjhasgfjhadsgfasdhjgf"
                    }
                }
                },


            };

            List<CourseResponse> coursesResponse = new List<CourseResponse>
            {
                new CourseResponse { Id = 1
                , Name = "course 1"
                , Description = "course1",
                Teacher=new TeacherResponse{
                    Id=1,
                    Email="teacher@emaple.com",
                    Phone="0123456789",
                    User=new UserResponse{
                        Id=1,
                        FirstName="teacher1",
                        LastName="teacher1",
                        Email="teacher@emaple.com",
                        Phone="0123456789",
                    }
                }
                },
                new CourseResponse { Id = 2
                , Name = "course 2"
                , Description = "course2"
                ,Teacher=new TeacherResponse{
                    Id=1,
                    Email="teacher@emaple.com",
                    Phone="0123456789",
                    User=new UserResponse{
                        Id=1,
                        FirstName="teacher1",
                        LastName="teacher1",
                        Email="teacher@emaple.com",
                        Phone="0123456789",
                    }
                }
                },


            };
            _mockClassService.Setup(s => s.GetCoursesByClassIdAsync(classId))
                .ReturnsAsync(courses);

            _mockMapper.Setup(m => m.Map<IEnumerable<CourseResponse>>(courses))
                .Returns(coursesResponse);

            // Act
            var result = await _controller.GetCoursesByClassId(classId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var reurnedData = okObjectResult.Value as ApiResponseListStrategy<CourseResponse>;
            Assert.NotNull(reurnedData);
            Assert.Equal("Courses Fetched Successfully", reurnedData.Message);
            Assert.Equal(201, reurnedData.Status);
            Assert.True(reurnedData.Success);

            var data = reurnedData.Data.ToList();
            Assert.Equal(2, data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Assert.Equal(courses[i].Id, data[i].Id);
                Assert.Equal(courses[i].Name, data[i].Name);
                Assert.Equal(courses[i].Description, data[i].Description);
                Assert.Equal(courses[i].Teacher.Id, data[i].Teacher.Id);
                Assert.Equal(courses[i].Teacher.Email, data[i].Teacher.Email);
                Assert.Equal(courses[i].Teacher.Phone, data[i].Teacher.Phone);
                Assert.Equal(courses[i].Teacher.User.Id, data[i].Teacher.User.Id);
                Assert.Equal(courses[i].Teacher.User.FirstName, data[i].Teacher.User.FirstName);
                Assert.Equal(courses[i].Teacher.User.LastName, data[i].Teacher.User.LastName);
                Assert.Equal(courses[i].Teacher.User.Email, data[i].Teacher.User.Email);
                Assert.Equal(courses[i].Teacher.User.Phone, data[i].Teacher.User.Phone);

            }
        }


        [Fact]
        public async void GetCoursesByClassId_WhenClassNotExistsOrNoCoursesExists_ReturnEmptyListOfCoursesResponse()
        {
            int classId = 1;

            _mockClassService.Setup(s => s.GetCoursesByClassIdAsync(classId))
                .ReturnsAsync((List<Course>)null);

            _mockMapper.Setup(m => m.Map<IEnumerable<CourseResponse>>((List<Course>)null))
                .Returns((List<CourseResponse>)null);

            // Act
            var result = await _controller.GetCoursesByClassId(classId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var reurnedData = okObjectResult.Value as ApiResponseListStrategy<CourseResponse>;
            Assert.NotNull(reurnedData);
            Assert.Equal("Courses Fetched Successfully", reurnedData.Message);
            Assert.Equal(201, reurnedData.Status);
            Assert.True(reurnedData.Success);

            var data = reurnedData?.Data?.ToList();
            Assert.Null(data);

        }
        [Fact]
        public async void GetTeachersByClassId_WhenClassExists_ReturnListOfTeachersOfClass()
        {
            int classId = 1;
            List<Teacher> teachers = new List<Teacher>
            {
                new Teacher{
                    Id=1,
                    UserId=1,
                    Email="teacher@emaple.com",
                    Phone="0123456789",
                    User=new User{
                        Id=1,
                        FirstName="teacher1",
                        LastName="teacher1",
                        Email="teacher@emaple.com",
                        Phone="0123456789",
                        Password="asdjahsdkjasgdjhagfdhjasgdfjhasgfjhadsgfasdhjgf"
                    }
                },

                new Teacher{
                    Id=2,
                    UserId=2,
                    Email="teacher2@emaple.com",
                    Phone="0133456789",
                    User=new User{
                        Id=2,
                        FirstName="teacher2",
                        LastName="teacher2",
                        Email="teacher2@emaple.com",
                        Phone="0223456789",
                        Password="asdjahsdkjasgdjhagfdhjasgdfjhasgfjhadsgfasdhjgf"
                    }
                },

            };
            List<TeacherResponse> teachersResponse = new List<TeacherResponse>
            {
                new TeacherResponse{
                    Id=1,

                    Email="teacher@emaple.com",
                    Phone="0123456789",
                    User=new UserResponse{
                        Id=1,
                        FirstName="teacher1",
                        LastName="teacher1",
                        Email="teacher@emaple.com",
                        Phone="0123456789",

                    }
                },

                new TeacherResponse{
                    Id=2,
                    Email="teacher2@emaple.com",
                    Phone="0133456789",
                    User=new UserResponse{
                        Id=2,
                        FirstName="teacher2",
                        LastName="teacher2",
                        Email="teacher2@emaple.com",
                        Phone="0223456789",
                    }
                },

            };


            _mockClassService.Setup(s => s.GetTeachersByClassIdAsync(classId))
                .ReturnsAsync(teachers);

            _mockMapper.Setup(m => m.Map<IEnumerable<TeacherResponse>>(teachers))
                .Returns(teachersResponse);

            // Act
            var result = await _controller.GetTeachersByClassId(classId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var reurnedData = okObjectResult.Value as ApiResponseListStrategy<TeacherResponse>;
            Assert.NotNull(reurnedData);
            Assert.Equal("Teachers Fetched Successfully", reurnedData.Message);
            Assert.Equal(201, reurnedData.Status);
            Assert.True(reurnedData.Success);

            var data = reurnedData.Data.ToList();
            Assert.Equal(2, data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Assert.Equal(teachers[i].Id, data[i].Id);
                Assert.Equal(teachers[i].Email, data[i].Email);
                Assert.Equal(teachers[i].Phone, data[i].Phone);
                Assert.Equal(teachers[i].User.Id, data[i].User.Id);
                Assert.Equal(teachers[i].User.FirstName, data[i].User.FirstName);
                Assert.Equal(teachers[i].User.LastName, data[i].User.LastName);
                Assert.Equal(teachers[i].User.Email, data[i].User.Email);
                Assert.Equal(teachers[i].User.Phone, data[i].User.Phone);
            }
        }


        [Fact]
        public async void GetTeachersByClassId_WhenClassNotExistsOrNoCoursesExists_ReturnEmptyListOfTeachersResponse()
        {
            int classId = 1;

            _mockClassService.Setup(s => s.GetTeachersByClassIdAsync(classId))
                .ReturnsAsync((List<Teacher>)null);

            _mockMapper.Setup(m => m.Map<IEnumerable<TeacherResponse>>((List<Teacher>)null))
                .Returns((List<TeacherResponse>)null);

            // Act
            var result = await _controller.GetTeachersByClassId(classId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okObjectResult.StatusCode);

            var reurnedData = okObjectResult.Value as ApiResponseListStrategy<TeacherResponse>;
            Assert.NotNull(reurnedData);
            Assert.Equal("Teachers Fetched Successfully", reurnedData.Message);
            Assert.Equal(201, reurnedData.Status);
            Assert.True(reurnedData.Success);

            var data = reurnedData?.Data?.ToList();
            Assert.Null(data);

        }


    }

}

