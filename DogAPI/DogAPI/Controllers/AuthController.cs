using BLL.Services.Interfaces;
using Common.DTO.AuthDTO;
using Microsoft.AspNetCore.Mvc;

namespace DogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserDTO userDTO)
    {
        try
        {
            return Ok(await _authService.RegisterAsync(userDTO));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserDTO userDTO)
    {
        try
        {
            return Ok(await _authService.LoginAsync(userDTO));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}