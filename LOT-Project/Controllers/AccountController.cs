using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using LOT_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace LOT_Project.Controllers
{
    [Route("api/[controler]/")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;


        private readonly FlightsDbContext _dbContext;
        public AccountController(IAccountService accountService, FlightsDbContext dbContext)
        {
            _accountService = accountService;
            _dbContext = dbContext;
        }
        [HttpPost("/register")]
        public ActionResult RegisterUser([FromBody]RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }
        [HttpPost("/login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                string token = _accountService.GenerateJwt(dto);
                return Ok(token);
            }
            catch (BadRequestExeption ex)
            {

                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
