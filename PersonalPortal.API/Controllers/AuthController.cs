using Microsoft.AspNetCore.Mvc;
using PersonalPortal.API.Data;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        
        if (user == null || user.Password != request.Password)
        {
            return Ok(new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password"
            });
        }

        var token = Guid.NewGuid().ToString();
        
        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            User = new UserInfo
            {
                Username = user.Username,
                DisplayName = user.DisplayName,
                Role = user.Role
            }
        });
    }

    [HttpPost("validate")]
    public async Task<ActionResult<bool>> ValidateToken([FromBody] string token)
    {
        return Ok(!string.IsNullOrEmpty(token));
    }
}
