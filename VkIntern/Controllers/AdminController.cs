using AutoMapper;
using DB;
using DB.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VkIntern.Filters.ActionFilters;
using VkIntern.ViewModels.UserGroup;
using VkIntern.ViewModels.UserState;

namespace VkIntern.Controllers
{

    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class AdminController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddUserState([FromBody] UserStateViewModel userStateViewModel)
        {
            var userState = _mapper.Map<UserState>(userStateViewModel);
            await _context.Set<UserState>().AddAsync(userState);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddUserGroup([FromBody] UserGroupViewModel userGroupViewModel)
        {
            var userGroup = _mapper.Map<UserGroup>(userGroupViewModel);
            await _context.Set<UserGroup>().AddAsync(userGroup);
            await _context.SaveChangesAsync();
            return Ok();
        }
        

        [HttpPost]
        [Exist<User>]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.Group = await _context.Set<UserGroup>()
                .FirstOrDefaultAsync(group => group.Code == UserGroupCodes.Blocked.ToString());
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Exist<User>]
        public async Task<IActionResult> ReturnUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.Group = await _context.Set<UserGroup>()
                .FirstOrDefaultAsync(group => group.Code == UserGroupCodes.Active.ToString());
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
