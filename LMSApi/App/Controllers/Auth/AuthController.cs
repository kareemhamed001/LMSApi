using LMSApi.App.Options;
using LMSApi.App.Requests;
using LMSApi.App.Responses;
using LMSApi.Database.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace LMSApi.App.Controllers.Auth
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext appDbContext, ILogger<AuthController> logger, JwtOptions jwtOptions) : ControllerBase, IAuthController
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtOptions _jwtOptions;

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            using (appDbContext)
            {
                var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                    return NotFound("User not found");

                if (!user.VerifyPassword(request.Password))
                    return BadRequest("Invalid password");

                return Ok(new ApiResponse<string>()
                {
                    Data = new List<string> { user.GenerateJwtToken(jwtOptions) },
                    Message = "Succeeded",
                    Success = true,
                    Status = 201
                });

            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserRequest request)
        {
            using (appDbContext)
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Password = request.Password
                };

                await appDbContext.Users.AddAsync(user);
                await appDbContext.SaveChangesAsync();

                return Ok(new ApiResponse<string>()
                {
                    Data = new List<string> { user.GenerateJwtToken(jwtOptions) },
                    Message = "User registered successfully",
                    Success = true,
                    Status = 201
                });
            }
        }


    }
}
