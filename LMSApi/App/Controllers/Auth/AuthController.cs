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
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtOptions _jwtOptions;

        public AuthController(AppDbContext appDbContext, ILogger<AuthController> logger, JwtOptions jwtOptions)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _jwtOptions = jwtOptions;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return NotFound(new ApiResponse<string> { Message = "User not found", Success = false, Status = 404 });

            if (!user.VerifyPassword(request.Password))
                return BadRequest(new ApiResponse<string> { Message = "Invalid password", Success = false, Status = 400 });

            return Ok(new ApiResponse<string>
            {
                Data = new List<string> { user.GenerateJwtToken(_jwtOptions) },
                Message = "Succeeded",
                Success = true,
                Status = 201
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserRequest request)
        {
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return BadRequest(new ApiResponse<string> { Message = "User already exists", Success = false, Status = 400 });

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Password = request.Password
            };

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return Ok(new ApiResponse<string>
            {
                Data = new List<string> { user.GenerateJwtToken(_jwtOptions) },
                Message = "User registered successfully",
                Success = true,
                Status = 201
            });
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int userId)
        {
            var user = await _appDbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(new ApiResponse<UserDto> { Message = "User not found", Success = false, Status = 404 });

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };

            return Ok(userDto);
        }
    }
}
