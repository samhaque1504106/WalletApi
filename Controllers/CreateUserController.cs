using Microsoft.AspNetCore.Mvc;
using WalletApi.Data;
using WalletApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace WalletApi.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ILogger<CreateUserController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public CreateUserController(ILogger<CreateUserController> logger, ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
            _mapper = mapper;
        }
        



        //create user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Users> CreateUser([FromBody] UsersDTO userdto)
        {
            if (userdto == null)
            {
                return BadRequest(userdto);
            }
            var user = _mapper.Map<Users>(userdto);

            if (_db.Users.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "UserName already taken,try using combinations with number and signs!");
                return Conflict(ModelState);
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
            _logger.LogInformation("new sign up username: " + user.UserName + " is done!");
            return Ok("sign-up successful, welcome to WalletApi!");
        }


    }
}

