using AutoMapper;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using VkIntern.Controllers;
using VkIntern.Filters.ActionFilters;
using VkIntern.ViewModels;
using VkIntern.ViewModels.Account;

namespace VkIntern.Tests
{
    public class UserControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<AppDbContext> _contextMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AppMappingProfile(_contextMock.Object));
            });
            _mapper = config.CreateMapper();


            _controller = new UserController(_mapper, _contextMock.Object);
        }

        [Fact]
        public async Task Exist_Attribute_Create_BadRequest_When_Entity_NotFound()
        {
            var users = new List<User>();
            {
                new User
                {
                    Login = "user1"
                };
            }
            var createUser = new CreateUserViewModel
            {
                Login = "user1"
            };

            _contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        }
    }
}