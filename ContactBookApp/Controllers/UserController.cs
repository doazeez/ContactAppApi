using ContactBusinessLogic.Interface;
using ContactDatabase.DTO;
using ContactDatabase.Model;
using DatabaseAndModel.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactBookApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        private readonly IImageService _imageService;

        public UserController(IUserService userService, IImageService imageService)
        {
            _userService = userService;
            _imageService = imageService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDTO register)
        {
            try
            {
                var result = await _userService.Register(register);
                return Created("", result);
            }
            catch (MissingFieldException Mex)
            {
                return BadRequest(Mex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            try
            {
                return Ok(await _userService.GetUserAsync(userId));
            }
            catch (ArgumentException arge)
            {
                return BadRequest(arge.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            try
            {
                return Ok(await _userService.GetUserByEmailAsync(email));
            }
            catch (ArgumentException arge)
            {
                return BadRequest(arge.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        /// <summary>
        ///  This is to update the data
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync(UpdateRequest updateUser)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userService.Update(userId, updateUser);
                return NoContent();
            }
            catch (MissingMemberException mme)
            {
                return BadRequest(mme.Message);
            }
            catch (ArgumentException arge)
            {
                return BadRequest(arge.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string userId)
        {
            try
            {
                return Ok(await _userService.Delete(userId));
            }
            catch (MissingMemberException mme)
            {
                return BadRequest(mme.Message);
            }
            catch (ArgumentException arge)
            {
                return BadRequest(arge.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,Regular")]
        public async Task<IActionResult> UploadImageAsync([FromForm] AddImageDTO imageDTO)
        {
            try
            {
                var resultUpload = await _imageService.UploadAsync(imageDTO.Image);
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _imageService.UpdateImageAsync(userId, resultUpload.Url.ToString());
                return NoContent();
            }
            catch (MissingMemberException mme)
            {
                return BadRequest(mme.Message);
            }
            catch (ArgumentException arge)
            {
                return BadRequest(arge.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<User> GetAll()
        {
            var result = _userService.GetListOfUsers();
            return result;
        }
    }
}
