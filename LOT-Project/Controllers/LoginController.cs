using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using LOT_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace LOT_Project.Controllers
{
    [Route("api/[controler]/")]
    public class LoginController : Controller
    {
        private readonly IAccountService _accountService;


        private readonly FlightsDbContext _dbContext;
        public LoginController(IAccountService accountService, FlightsDbContext dbContext)
        {
            _accountService = accountService;
            _dbContext = dbContext;
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
