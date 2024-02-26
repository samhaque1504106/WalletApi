using Microsoft.AspNetCore.Mvc;

namespace Bank_API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public string GetUser()
        {
            Console.WriteLine("Inside GET request in hello");
            return ("hello");
        }
    }
}

