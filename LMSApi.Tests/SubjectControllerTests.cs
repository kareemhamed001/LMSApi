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
using Xunit;

namespace LMSApi.Tests
{
    public class SubjectsControllerTests
    {
        private readonly Mock<ISubjectService> _mockSubjectService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<SubjectsController>> _mockLogger;
        private readonly SubjectsController _controller;

        public SubjectsControllerTests()
        {
            _mockSubjectService = new Mock<ISubjectService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<SubjectsController>>();

            _controller = new SubjectsController(_mockSubjectService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Index_SubjectsExist_ReturnsOkResultWithSubjectData()
        {
            // Arrange
            var subjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Subject 1" },
                new Subject { Id = 2, Name = "Subject 2" }
            };

            var subjectResponses = new List<SubjectResponse>
            {
                new SubjectResponse { Id = 1, Name = "Subject 1" },
                new SubjectResponse { Id = 2, Name = "Subject 2" }
            };

            _mockSubjectService.Setup(s => s.Index()).ReturnsAsync(subjects);
            _mockMapper.Setup(m => m.Map<List<SubjectResponse>>(subjects)).Returns(subjectResponses);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseListStrategy<SubjectResponse>;
            Assert.NotNull(response);
            Assert.Equal("Subjects fetched successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as List<SubjectResponse>;
            Assert.NotNull(dataReturned);
            Assert.Equal(subjectResponses.Count, dataReturned.Count);
        }

        [Fact]
        public async Task Show_SubjectExists_ReturnsOkResultWithSubjectData()
        {
            // Arrange
            int subjectId = 1;
            var subject = new Subject { Id = subjectId, Name = "Subject 1" };
            var subjectResponse = new SubjectResponse { Id = subjectId, Name = "Subject 1" };

            _mockSubjectService.Setup(s => s.Show(subjectId)).ReturnsAsync(subject);
            _mockMapper.Setup(m => m.Map<SubjectResponse>(subject)).Returns(subjectResponse);

            // Act
            var result = await _controller.Show(subjectId);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseSingleStrategy;
            Assert.NotNull(response);
            Assert.Equal("Subject fetched successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as SubjectResponse;
            Assert.NotNull(dataReturned);
            Assert.Equal(subjectResponse.Id, dataReturned.Id);
            Assert.Equal(subjectResponse.Name, dataReturned.Name);
        }
     



        [Fact]
        public async Task Store_SubjectCreated_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new SubjectRequest { Name = "New Subject" };
            var createdSubject = new Subject { Id = 1, Name = "New Subject" };
            var subjectResponse = new SubjectResponse { Id = 1, Name = "New Subject" };

            _mockSubjectService.Setup(s => s.StoreAsync(request)).ReturnsAsync(createdSubject);
            _mockMapper.Setup(m => m.Map<SubjectResponse>(createdSubject)).Returns(subjectResponse);

            // Act
            var result = await _controller.Store(request);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);

            var response = createdResult.Value as ApiResponseSingleStrategy;
            Assert.NotNull(response);
            Assert.Equal("Subject created successfully", response.Message);
            Assert.Equal(201, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as SubjectResponse;
            Assert.NotNull(dataReturned);
            Assert.Equal(subjectResponse.Id, dataReturned.Id);
            Assert.Equal(subjectResponse.Name, dataReturned.Name);
        }

        [Fact]
        public async Task Update_SubjectUpdated_ReturnsOkResult()
        {
            // Arrange
            int subjectId = 1;
            var request = new UpdateSubjectRequest { Name = "Updated Subject" };
            var updatedSubject = new Subject { Id = subjectId, Name = "Updated Subject" };
            var subjectResponse = new SubjectResponse { Id = subjectId, Name = "Updated Subject" };

            _mockSubjectService.Setup(s => s.UpdateAsync(subjectId, request)).ReturnsAsync(updatedSubject);
            _mockMapper.Setup(m => m.Map<SubjectResponse>(updatedSubject)).Returns(subjectResponse);

            // Act
            var result = await _controller.Update(subjectId, request);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseSingleStrategy;
            Assert.NotNull(response);
            Assert.Equal("Subject updated successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as SubjectResponse;
            Assert.NotNull(dataReturned);
            Assert.Equal(subjectResponse.Id, dataReturned.Id);
            Assert.Equal(subjectResponse.Name, dataReturned.Name);
        }

        [Fact]
        public async Task Delete_SubjectExists_ReturnsNoContent()
        {
            // Arrange
            int subjectId = 1;
            _mockSubjectService.Setup(s => s.DeleteAsync(subjectId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(subjectId);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetClasses_SubjectExists_ReturnsOkResultWithClasses()
        {
            // Arrange
            int subjectId = 1;
            var classes = new List<Class> { new Class { Id = 1, Name = "Class 1" } };
            var classResponses = new List<ClassResponse> { new ClassResponse { Id = 1, Name = "Class 1" } };

            _mockSubjectService.Setup(s => s.ClassesAsync(subjectId)).ReturnsAsync(classes);
            _mockMapper.Setup(m => m.Map<List<ClassResponse>>(classes)).Returns(classResponses);

            // Act
            var result = await _controller.GetClasses(subjectId);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseListStrategy<ClassResponse>;
            Assert.NotNull(response);
            Assert.Equal("Classes fetched successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as List<ClassResponse>;
            Assert.NotNull(dataReturned);
            Assert.Equal(classResponses.Count, dataReturned.Count);
        }

        [Fact]
        public async Task GetCourses_SubjectExists_ReturnsOkResultWithCourses()
        {
            // Arrange
            int subjectId = 1;
            var courses = new List<Course> { new Course { Id = 1, Name = "Course 1" } };
            var courseResponses = new List<CourseResponse> { new CourseResponse { Id = 1, Name = "Course 1" } };

            _mockSubjectService.Setup(s => s.CoursesAsync(subjectId)).ReturnsAsync(courses);
            _mockMapper.Setup(m => m.Map<List<CourseResponse>>(courses)).Returns(courseResponses);

            // Act
            var result = await _controller.GetCourses(subjectId);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseListStrategy<CourseResponse>;
            Assert.NotNull(response);
            Assert.Equal("Courses fetched successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as List<CourseResponse>;
            Assert.NotNull(dataReturned);
            Assert.Equal(courseResponses.Count, dataReturned.Count);
        }

        [Fact]
        public async Task GetStudents_SubjectExists_ReturnsOkResultWithStudents()
        {
            // Arrange
            int subjectId = 1;
            var students = new List<Student> { new Student { Id = 1, ParentPhone = "01273148385" } };
            var studentResponses = new List<StudentResponse> { new StudentResponse { Id = 1, ParentPhone = "01273148385" } };

            _mockSubjectService.Setup(s => s.StudentsAsync(subjectId)).ReturnsAsync(students);
            _mockMapper.Setup(m => m.Map<List<StudentResponse>>(students)).Returns(studentResponses);

            // Act
            var result = await _controller.GetStudents(subjectId);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseListStrategy<StudentResponse>;
            Assert.NotNull(response);
            Assert.Equal("Students fetched successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as List<StudentResponse>;
            Assert.NotNull(dataReturned);
            Assert.Equal(studentResponses.Count, dataReturned.Count);
        }

        [Fact]
        public async Task GetTeachers_SubjectExists_ReturnsOkResultWithTeachers()
        {
            // Arrange
            int subjectId = 1;
            var teachers = new List<Teacher> { new Teacher { Id = 1, NickName = "Teacher 1", Phone = "01273148385", Email = "mohamedashraf11ma11@gmail.com" } };
            var teacherResponses = new List<TeacherResponse> { new TeacherResponse { Id = 1, NickName = "Teacher 1", Phone = "01273148385", Email = "mohamedashraf11ma11@gmail.com" } };

            _mockSubjectService.Setup(s => s.TeachersAsync(subjectId)).ReturnsAsync(teachers);
            _mockMapper.Setup(m => m.Map<List<TeacherResponse>>(teachers)).Returns(teacherResponses);

            // Act
            var result = await _controller.GetTeachers(subjectId);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseListStrategy<TeacherResponse>;
            Assert.NotNull(response);
            Assert.Equal("Teachers fetched successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);

            var dataReturned = response.Data as List<TeacherResponse>;
            Assert.NotNull(dataReturned);
            Assert.Equal(teacherResponses.Count, dataReturned.Count);
        }

        [Fact]
        public async Task AddSubjectToClass_SubjectAdded_ReturnsOkResult()
        {
            // Arrange
            var request = new AddSubjectToClassRequest { SubjectId = 1, ClassId = 1 };
            bool resultSuccess = true;

            _mockSubjectService.Setup(s => s.AddSubjectToClass(request)).ReturnsAsync(resultSuccess);

            // Act
            var result = await _controller.AddSubjectToClass(request);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponseBase;
            Assert.NotNull(response);
            Assert.Equal("Subject added to class successfully", response.Message);
            Assert.Equal(200, response.Status);
            Assert.True(response.Success);
        }


    }
}
////COMMENT
///Sj
////test