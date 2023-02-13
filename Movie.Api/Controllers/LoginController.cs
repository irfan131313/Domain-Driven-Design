using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movie.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Movie.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {

        private IConfiguration _config;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public LoginController(
            IConfiguration config,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager
            )
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string UserName,string Password)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return BadRequest("kullanıcı adı veya şifre hatalı");
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, Password, false);
            
            if (!result.Succeeded)
            {
                return BadRequest("kullanıcı adı veya şifre hatalı");
            }

            var token = await Generate(user);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(AppUser user)
        {
                AppUser appUser = new AppUser
                {
                    UserName = user.UserName,
                    Password = user.Password,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                
            return Ok();
        }

        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admins()
        {
            var admin = "admin giriş yapti";
            return Ok(admin);
        }



        [HttpGet("GetUsers")]
        [AllowAnonymous]
        public IActionResult Users()
        {
            return Ok(_userManager.Users);
        }
        private async Task<IActionResult> Generate(AppUser user)
        {
            /*
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };*/
            var claims = await GetAllValidClaims(user);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,                     // claim kısmını gönder
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(jwt);
        }
        private async Task<List<Claim>> GetAllValidClaims(AppUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user))[0]));

            // token dan gelen claim role degerleri userda var mı sorgula ona gore tokenı gonder

            // usera atanmış roller alınır
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // userlarn rollerı alınır claimlere eklenir
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        private AppUser GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;

            return new AppUser
            {
                UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
            };
        }

    }
}
