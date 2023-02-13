using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Domain;
using System.Reflection.Metadata.Ecma335;

namespace Movie.Api.Controllers
{

    [ApiController]
    //  [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager; 
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            var users = _roleManager.Roles.ToList();
            
            return Ok(users);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string Role)
        {
            AppRole role = new AppRole
            {
                Name = Role
            };
            var result = await _roleManager.CreateAsync(role);
            return Ok();
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {

            var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            var result = await _roleManager.DeleteAsync(values);
            return Ok();
        }
        
        [HttpGet("GetUserRoleList")]
        public IActionResult GetUserRoleList()
        {
            var values = _userManager.Users.ToList();

            return Ok(values);
        }
        
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var roles = _roleManager.Roles.ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(userRoles);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(string id,string RoleId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            await _userManager.AddToRoleAsync(user, RoleId);

            return Ok(user);
        }

        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRole(string id, string RoleId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            await _userManager.RemoveFromRoleAsync(user, RoleId);

            return Ok(user);
        }


    }
}
