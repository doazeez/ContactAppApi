using ContactBusinessLogic.Interface;
using ContactDatabase.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ContactBookApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        readonly IAuthentication _authetication;
        public AuthenticationController(IAuthentication authetication)
        {
            _authetication = authetication;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO user)
        {
            try
            {
                return Ok(await _authetication.Login(user));
            }
            catch (AccessViolationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
