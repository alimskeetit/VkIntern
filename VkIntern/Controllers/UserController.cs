using AutoMapper;
using DB;
using DB.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using VkIntern.Filters.ActionFilters;
using VkIntern.ViewModels;

namespace VkIntern.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        [ValidateModel("createUser")]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel createUser)
        {
            var user = _mapper.Map<User>(createUser);
            if (await _context.Users.FirstOrDefaultAsync(userInDb => userInDb.Login == user.Login) != null)
                return BadRequest(new
                {
                    error = $"Пользователь с  логином {user.Login} уже существует",
                    createUser
                });
            user.Group = await _context.UserGroups.FirstOrDefaultAsync(group => group.Code == UserGroupCodes.Active.ToString());
            user.State = await _context.UserStates.FirstOrDefaultAsync(state => state.Code == UserStateCodes.User.ToString());
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_mapper.Map<List<UserViewModel>>(await _context.Users.AsNoTracking().Include(user => user.Group).Include(user => user.State).ToListAsync()));
        }

        [HttpGet("{count:int}/{page:int}")]
        public async Task<IActionResult> Get(int count, int page = 0)
        {
            if (count < 0)
                return BadRequest("count должен быть >= 0");
            if (page == 0) 
                page = 1;
            var users = page < 0 ? _context.Users.OrderByDescending(u => u) : _context.Users.AsQueryable();

            return Ok(await users
                .Skip((int.Abs(page) - 1) * count)
                .Take(count)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        [Exist<User>]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(_mapper.Map<UserViewModel>(await _context.Users.AsNoTracking().Include(user => user.State)
                .Include(user => user.Group)
                .FirstOrDefaultAsync(user => user.Id == id)));
        }
    }
}
