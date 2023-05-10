using System.Security.Claims;
using DB;
using DB.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace VkIntern.Policy
{

    public class ActiveUserHandler: AuthorizationHandler<ActiveUserRequirement>
    {
        private readonly AppDbContext _appDbContext;

        public ActiveUserHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(user =>
                user.Login == context.User.FindFirstValue(ClaimTypes.Name));
            if (user.Group.Code == UserGroupCodes.Blocked.ToString())
            {
                context.Fail();
            }
        }
    }

    public class ActiveUserRequirement: IAuthorizationRequirement
    {
    }
}
