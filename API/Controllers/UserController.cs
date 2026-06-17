using BLL.Common;
using BLL.DTOs.UserDTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<UserReadDTO>>>> GetAll()
        {
            var response = await _userService.GetAllUsersAsync(true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAllWithDeleted")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<UserReadDTO>>>> GetAllWithDeleted()
        {
            var response = await _userService.GetAllUsersAsync(false);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetWithPagination")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<UserReadDTO>>>> GetWithPagination(int pageNumber,  int pageSize = 10)
        {
            var response = await _userService.GetUsersWithPaginationAsync(pageNumber, pageSize, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Deactivate(string id)
        {
            var response = await _userService.DeactivateUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserReadDTO>>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _userService.GetUserByIdAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProfile(UserUpdateDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _userService.UpdateUserProfileAsync(userId, model);
            return StatusCode(response.StatusCode, response);
        }
    }
}
