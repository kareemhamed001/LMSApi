using LMSApi.App.Requests;
using LMSApi.App.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LMSApi.App.Controllers.Auth
{
    public interface IAuthController
    {
        public Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request);
        public Task<ActionResult<ApiResponse<string>>> Register([FromBody] UserRequest request);
    }
}