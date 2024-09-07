using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using LMSApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
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


        }
    }
}
