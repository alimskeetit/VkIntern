using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VkIntern.Filters.ActionFilters;
using VkIntern.ViewModels.Account;

namespace VkIntern.Controllers
{
    [Route("[action]")]
    public class AccountController: ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public AccountController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(user =>
                    user.Login == loginViewModel.Login && user.Password == loginViewModel.Password) == null)
                return BadRequest("Неправильный логин или пароль");
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loginViewModel.Login),
                new(ClaimTypes.Role, 
                    await _appDbContext.Users
                         .Where(usr => usr.Login == loginViewModel.Login)
                         .Select(usr => usr.State.Code)
                         .FirstOrDefaultAsync())
            };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            HttpContext.Response.Headers.Add("Authorization", $"Bearer {encodedJwt}");
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
