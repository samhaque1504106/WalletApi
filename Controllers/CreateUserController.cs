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
        



        //create user endpoint
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
        
        
        //get users with pagination
        [HttpGet("GetAll_Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUsers([FromQuery] string cursor = "")
        {
            /*pagination:
             * 1. Retrieve query parameters(cursor) from request url
             * 2. Is cursor empty? -> If Yes -> 1st page generate.
             *                                  for that use sql query, returns List of users(Max 20)
             *                                  then calculate next page info:  1. start cursor 2. end cursor 3. has_next_page
             * 3. Is cursor empty? -> If No ->  2nd page start cursor = 1st page end cursor+1
             *                                  again query.
             *                                  calculate same 1. start cursor 2. end cursor 3. has_next_page
             */

            
            // getting cursored data
            int CurrentCursor = 0;
            int limit = 20;
            var users = new List<Users>();
            if (int.TryParse(cursor, out int GivenCursor) && GivenCursor > 0)
            {
                CurrentCursor = GivenCursor;
            }
            
            var parameters = new[]
            {
                new SqlParameter("@cursor", CurrentCursor),
                new SqlParameter("@limit", limit)
            };
            users = _db.Users.FromSqlRaw("EXEC WA_SP_GetUserCursored @cursor, @limit", parameters).ToList();
            
            //getting start_cursor, end_cursor, has_next_page,
            string connectionString = _configuration.GetConnectionString("DefaultConnectionStrings");

            long StartCursor = users[0].Id;
            int len = users.Count;
            long EndCursor = users[len - 1].Id;
            bool hasNextPage = false;
            if (users.Any())
            {
                long lastUserId = users.Last().Id; // last user's ID is needed for SP_HasNextPage
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("WA_SP_HasNextPage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@EndCursor", SqlDbType.BigInt) { Value = lastUserId });

                        // SP_HasNextPage returns a bit (0 or 1) indicating if there is a next page
                        hasNextPage = (bool)command.ExecuteScalar();
                    }
                }
            }

            var pageInfo = new
            {
                StartCursor = users.Any() ? users.First().Id : 0,
                EndCursor = users.Any() ? users.Last().Id : 0,
                HasNextPage = hasNextPage
            };

            var response = new
            {
                PageInfo = pageInfo,
                Users = users
            };

            return Ok(response);



        }
        


    }
}

