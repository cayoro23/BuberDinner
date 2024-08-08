using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorController : ControllerBase
{
    [Route("Error")]
    public IActionResult Error()
    {
        return Problem();
    }
}
