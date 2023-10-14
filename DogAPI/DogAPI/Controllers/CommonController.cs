using Microsoft.AspNetCore.Mvc;

namespace DogAPI.Controllers;

[ApiController]
[Route("/")]
public class CommonController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Dogshouseservice.Version1.0.1");
    }
}