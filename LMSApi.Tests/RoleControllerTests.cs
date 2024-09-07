using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using LMSApi.App.Requests;
using LMSApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSApi.Tests
{
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<ILogger<RolesController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RolesController _roleController;

        public RoleControllerTests()
        {
            _mockRoleService = new Mock<IRoleService>();
            _mockLogger = new Mock<ILogger<RolesController>>();
            _mockMapper = new Mock<IMapper>();
            _roleController = new RolesController(_mockRoleService.Object, _mockLogger.Object, _mockMapper.Object);

        }

        [Fact]
        public async Task GetAllRoles_WhenRolesExists_ReturnOkResponseWithListOfRoles()
        {
            //arrange
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Admin",
                    Permissions=new List<Permission>
                    {
                        new Permission{Id=1,Name="Create"},
                        new Permission{Id=2,Name="Read"},
                        new Permission{Id=3,Name="Update"},
                        new Permission{Id=4,Name="Delete"}
                    }
                },
                new Role { Id = 2, Name = "User",
                    Permissions=new List<Permission>
                    {
                        new Permission{Id=2,Name="Read"},
                    }
                }
            };
            var rolesResponse = new List<RoleResponse>
            {
                new RoleResponse {  Name = "Admin",
                    Permissions=new List<PermissionResponse>
                    {
                        new PermissionResponse{Name="Create",Category="Teachers",RouteName="teachers.create"},
                        new PermissionResponse{Name="Read",Category="Teachers",RouteName="teachers.Read"},
                        new PermissionResponse{Name="Update",Category="Teachers",RouteName="teachers.Update"},
                        new PermissionResponse{Name="Delete",Category="Teachers",RouteName="teachers.Delete"}
                    }
                },
                new RoleResponse { Name = "User",
                    Permissions=new List<PermissionResponse>
                    {
                        new PermissionResponse{Name="Read",Category="Teachers",RouteName="teachers.Read"},
                    }
                }
            };

            _mockRoleService.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(roles);
            _mockMapper.Setup(x => x.Map<List<RoleResponse>>(roles)).Returns(rolesResponse);

            //act
            var result = await _roleController.GetAllRoles();

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseListStrategy<RoleResponse>>(okResult.Value);
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Roles fetched successfully", apiResponse.Message);
            Assert.True(apiResponse.Success);
            Assert.Equal(rolesResponse, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllRoles_WhenRolesExistsOrNotExistsButUnhandeledExceptionHappened_CatchExceptionAndReturnObjectResultWithCode500()
        {
            //arrange
            _mockRoleService.Setup(x => x.GetAllRolesAsync())
                .ThrowsAsync(new Exception("Unhandeled exception happened"));

            //act
            var result = await _roleController.GetAllRoles();

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unhandeled exception happened", apiResponse.Message);
            Assert.False(apiResponse.Success);


        }

        [Fact]
        public async Task GetRoleById_WhenRoleExists_ReturnOkResponseWithRole()
        {
            //arrange
            int roleId = 1;
            var role = new Role
            {
                Id = roleId,
                Name = "Admin",
                Permissions = new List<Permission>
                    {
                        new Permission{Id=1,Name="Create"},
                        new Permission{Id=2,Name="Read"},
                        new Permission{Id=3,Name="Update"},
                        new Permission{Id=4,Name="Delete"}
                    }
            };
            var roleResponse = new RoleResponse
            {
                Name = "Admin",
                Permissions = new List<PermissionResponse>
                    {
                        new PermissionResponse{Name="Create",Category="Teachers",RouteName="teachers.create"},
                        new PermissionResponse{Name="Read",Category="Teachers",RouteName="teachers.Read"},
                        new PermissionResponse{Name="Update",Category="Teachers",RouteName="teachers.Update"},
                        new PermissionResponse{Name="Delete",Category="Teachers",RouteName="teachers.Delete"}
                    }
            };

            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ReturnsAsync(role);

            _mockMapper.Setup(x => x.Map<RoleResponse>(role))
                .Returns(roleResponse);

            //act
            var result = await _roleController.GetRoleById(roleId);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Role fetched successfully", apiResponse.Message);
            Assert.True(apiResponse.Success);
            Assert.Equal(roleResponse, apiResponse.Data);
        }

        [Fact]
        public async Task GetRoleById_WhenRoleNotExists_CatchNotFoundExceptionAndReturnNotFoundResult()
        {
            //arrange
            int roleId = 1;


            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ThrowsAsync(new NotFoundException("Role not found"));

            //act
            var result = await _roleController.GetRoleById(roleId);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Role not found", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task GetRoleById_WhenRoleExistsOrNotExists_CatchExceptionAndReturnObjectResultWithCode500()
        {
            //arrange
            int roleId = 1;


            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ThrowsAsync(new Exception("Unhandeled Exception"));

            //act
            var result = await _roleController.GetRoleById(roleId);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);
            Assert.Equal("Unhandeled Exception", apiResponse.Message);
            Assert.Equal(500, apiResponse.Status);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task CreateRole_WhenRoleCreated_ReturnOkResponseWithRole()
        {
            //arrange
            int roleId = 1;

            var roleRequest = new CreateRoleRequest
            {
                Name = "Admin",
                Permissions = new List<int> { 1, 2, 3, 4 }
            };

            var role = new Role
            {
                Id = roleId,
                Name = "Admin",
                Permissions = new List<Permission>
                    {
                        new Permission{Id=1,Name="Create"},
                        new Permission{Id=2,Name="Read"},
                        new Permission{Id=3,Name="Update"},
                        new Permission{Id=4,Name="Delete"}
                    }
            };
            var roleResponse = new RoleResponse
            {
                Name = "Admin",
                Permissions = new List<PermissionResponse>
                    {
                        new PermissionResponse{Name="Create",Category="Teachers",RouteName="teachers.create"},
                        new PermissionResponse{Name="Read",Category="Teachers",RouteName="teachers.Read"},
                        new PermissionResponse{Name="Update",Category="Teachers",RouteName="teachers.Update"},
                        new PermissionResponse{Name="Delete",Category="Teachers",RouteName="teachers.Delete"}
                    }
            };

            _mockRoleService.Setup(x => x.CreateRoleAsync(roleRequest))
                .ReturnsAsync(role);

            _mockMapper.Setup(x => x.Map<RoleResponse>(role))
                .Returns(roleResponse);

            //act
            var result = await _roleController.CreateRole(roleRequest);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.NotNull(apiResponse);


            Assert.Equal(roleResponse, apiResponse.Data);
            Assert.Equal(201, apiResponse.Status);
            Assert.Equal("Role created successfully", apiResponse.Message);
            Assert.True(apiResponse.Success);
        }

        [Fact]
        public async Task CreateRole_WhenCreateRoleRequestIsNull_CatchInvalidNullArgumentExceptionAndReturnBadRequestResponse()
        {
            //arrange
            int roleId = 1;

            CreateRoleRequest roleRequest = (CreateRoleRequest)null;



            _mockRoleService.Setup(x => x.CreateRoleAsync(roleRequest))
                .ThrowsAsync(new ArgumentNullException("Role request cannot be null.", "roleRequest"));


            //act
            var result = await _roleController.CreateRole(roleRequest);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);

            Assert.Equal(400, apiResponse.Status);
            Assert.Equal("roleRequest (Parameter 'Role request cannot be null.')", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task CreateRole_WhenUnHandeledExceptionHappedned_CatchExceptionAndReturnObjectResultWithCode500()
        {
            //arrange
            int roleId = 1;

            CreateRoleRequest roleRequest = (CreateRoleRequest)null;



            _mockRoleService.Setup(x => x.CreateRoleAsync(roleRequest))
                .ThrowsAsync(new Exception("unhandeledException"));


            //act
            var result = await _roleController.CreateRole(roleRequest);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("unhandeledException", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task UpdateRole_WhenRoleExistsAndUpdated_ReturnOkResponseWithUpdatedRole()
        {
            //arrange
            int roleId = 1;

            var roleRequest = new CreateRoleRequest
            {
                Name = "Admin",
                Permissions = new List<int> { 1, 2, 3, 4 }
            };

            var role = new Role
            {
                Id = roleId,
                Name = "Admin",
                Permissions = new List<Permission>
                    {
                        new Permission{Id=1,Name="Create"},
                        new Permission{Id=2,Name="Read"},
                        new Permission{Id=3,Name="Update"},
                        new Permission{Id=4,Name="Delete"}
                    }
            };

            var roleResponse = new RoleResponse
            {
                Name = "Admin",
                Permissions = new List<PermissionResponse>
                    {
                        new PermissionResponse{Name="Create",Category="Teachers",RouteName="teachers.create"},
                        new PermissionResponse{Name="Read",Category="Teachers",RouteName="teachers.Read"},
                        new PermissionResponse{Name="Update",Category="Teachers",RouteName="teachers.Update"},
                        new PermissionResponse{Name="Delete",Category="Teachers",RouteName="teachers.Delete"}
                    }
            };

            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ReturnsAsync(role);

            _mockRoleService.Setup(x => x.UpdateRoleAsync(roleId, roleRequest))
                .ReturnsAsync(role);

            _mockMapper.Setup(x => x.Map<RoleResponse>(role))
                .Returns(roleResponse);

            //act
            var result = await _roleController.UpdateRole(roleId, roleRequest);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.NotNull(apiResponse);


            Assert.Equal(roleResponse, apiResponse.Data);
            Assert.Equal(201, apiResponse.Status);
            Assert.Equal("Role updated successfully", apiResponse.Message);
            Assert.True(apiResponse.Success);
        }

        [Fact]
        public async Task UpdateRole_WhenRoleNotExists_CatchNotFoundExceptionAndReturnNotFoundResponse()
        {
            //arrange
            int roleId = 1;

            var roleRequest = new CreateRoleRequest
            {
                Name = "Admin",
                Permissions = new List<int> { 1, 2, 3, 4 }
            };


            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ThrowsAsync(new NotFoundException("role not found"));

            //act
            var result = await _roleController.UpdateRole(roleId, roleRequest);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);

            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("role not found", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task UpdateRole_WhenUnHandeledExceptionHappened_CatchExceptionAndReturnObjectResultWithCode500()
        {
            //arrange
            int roleId = 1;

            var roleRequest = new CreateRoleRequest
            {
                Name = "Admin",
                Permissions = new List<int> { 1, 2, 3, 4 }
            };

            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ThrowsAsync(new Exception("UnhandeledException"));

            //act
            var result = await _roleController.UpdateRole(roleId, roleRequest);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);

            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("UnhandeledException", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }


        [Fact]
        public async Task DeleteRole_WhenRoleExistsAndDeleted_ReturnOkResponse()
        {
            //arrange
            int roleId = 1;

            var role = new Role
            {
                Id = roleId,
                Name = "Admin",
                Permissions = new List<Permission>
                    {
                        new Permission{Id=1,Name="Create"},
                        new Permission{Id=2,Name="Read"},
                        new Permission{Id=3,Name="Update"},
                        new Permission{Id=4,Name="Delete"}
                    }
            };


            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ReturnsAsync(role);

            _mockRoleService.Setup(x => x.DeleteRoleAsync(roleId));


            //act
            var result = await _roleController.DeleteRole(roleId);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);


            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Role deleted successfully", apiResponse.Message);
            Assert.True(apiResponse.Success);
        }

        [Fact]
        public async Task DeleteRole_WhenRoleNotExists_CatchNotFoundExceptionAndReturnNotFoundResponse()
        {
            //arrange
            int roleId = 1;

            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ThrowsAsync(new NotFoundException("role not found"));

            _mockRoleService.Setup(x => x.DeleteRoleAsync(roleId));


            //act
            var result = await _roleController.DeleteRole(roleId);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);


            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("role not found", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }

        [Fact]
        public async Task DeleteRole_WhenUnhandeledExceptionThrown_CatchExceptionAndReturnObjectResultWithCode500()
        {
            //arrange
            int roleId = 1;

            _mockRoleService.Setup(x => x.GetRoleByIdAsync(roleId))
                .ThrowsAsync(new Exception("role not found"));

            _mockRoleService.Setup(x => x.DeleteRoleAsync(roleId));

            //act
            var result = await _roleController.DeleteRole(roleId);

            //assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.NotNull(apiResponse);


            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("role not found", apiResponse.Message);
            Assert.False(apiResponse.Success);
        }


        [Fact]
        public async Task AssignRoleToUser_WhenRoleAssigned_ReturnsOk()
        {
            // Arrange
            var request = new AssignRoleRequest { UserId = 1, RoleId = 2 };
            _mockRoleService.Setup(s => s.IsRoleAssignedToUserAsync(request.UserId, request.RoleId))
                            .ReturnsAsync(true);

            // Act
            var result = await _roleController.AssignRoleToUser(request);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = Assert.IsType<ApiResponseBase>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Role assigned to user successfully", response.Message);
            Assert.Equal(200, response.Status);
        }

        [Fact]
        public async Task AssignRoleToUser_WhenUserOrRoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new AssignRoleRequest { UserId = 1, RoleId = 2 };
            _mockRoleService.Setup(s => s.IsRoleAssignedToUserAsync(request.UserId, request.RoleId))
                            .ReturnsAsync(false);

            // Act
            var result = await _roleController.AssignRoleToUser(request);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            var response = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User or role not found", response.Message);
            Assert.Equal(404, response.Status);
        }

        [Fact]
        public async Task AssignRoleToUser_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var request = new AssignRoleRequest { UserId = 1, RoleId = 2 };
            _mockRoleService.Setup(s => s.IsRoleAssignedToUserAsync(request.UserId, request.RoleId))
                            .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _roleController.AssignRoleToUser(request);

            // Assert
            Assert.IsType<ActionResult<IApiResponse>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);

            var response = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Internal server error", response.Message);
            Assert.Equal(500, response.Status);
        }
    }
}