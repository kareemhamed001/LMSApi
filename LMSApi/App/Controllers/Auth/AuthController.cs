using AutoMapper;
using LMSApi.App.Attributes;
using LMSApi.App.Options;
using LMSApi.App.Requests;
using LMSApi.App.Responses;
using LMSApi.Database.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSApi.App.Controllers.Auth
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper mapper;

        public AuthController(AppDbContext appDbContext, ILogger<AuthController> logger, JwtOptions jwtOptions, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _jwtOptions = jwtOptions;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return NotFound(ApiResponseFactory.Create("User not found", 404, false));

            if (!user.VerifyPassword(request.Password))
                return BadRequest(ApiResponseFactory.Create("Invalid password", 400, false));

            return Ok(ApiResponseFactory.Create((object)user.GenerateJwtToken(_jwtOptions, _appDbContext), "Succeeded", 201, true));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserRequest request)
        {
            //where user with the same name or phone exists
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.Phone == request.Phone);
            if (existingUser != null)
                return BadRequest(ApiResponseFactory.Create("User already exists", 400, false));


            var userRole = await _appDbContext.Roles.Include(u => u.Permissions).SingleOrDefaultAsync(r => r.Name == "User");

            if (userRole == null)
            {
                userRole = new Role { Name = "User" };
                await _appDbContext.Roles.AddAsync(userRole);
                await _appDbContext.SaveChangesAsync();  // Save changes to get the new role's ID
            }


            User user = mapper.Map<User>(request);
            user.Roles = new List<Role> { userRole };

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "Token", user.GenerateJwtToken(_jwtOptions, _appDbContext)},
                { "Permission", userRole.Permissions.Select(p => p.RouteName).ToList() }
            };

            return Ok(ApiResponseFactory.Create(
                data.Select(kvp => new { key = kvp.Key, value = kvp.Value }).ToList(),
                "User registered successfully",
                201,
                true));
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize]
        [CheckPermission("users.show")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int userId)
        {
            var user = await _appDbContext.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(ApiResponseFactory.Create("User not found", 404, false));

            var userDto = mapper.Map<UserDto>(user);

            return Ok(userDto);
        }
    }
}
