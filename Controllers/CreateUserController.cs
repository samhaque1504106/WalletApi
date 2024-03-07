using Microsoft.AspNetCore.Mvc;
using WalletApi.Data;
using WalletApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WalletApi.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ILogger<CreateUserController> _logger;
        public CreateUserController(ILogger<CreateUserController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;   
        }
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public string GetUser()
        {
            Console.WriteLine("Inside GET request in hello");
            return ("hello");
        }
        
        //create user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Users> CreateUser([FromBody] Users user)
        {
            if (_db.Users.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError","UserName already taken,try using combinations with number and signs!");
                return Conflict(ModelState);
            }
            else if (user == null)
            {
                return BadRequest(user);
            }
            else if (user.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Set CreatedAt to current UTC time and UpdatedAt to null
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = null;
            
            _db.Users.Add(user);
            _db.SaveChanges();
            _logger.LogInformation("new sign up username: "+user.UserName+" is done!");
            return Ok("sign-up successful, welcome to WalletApi!");
        }
    }
}

