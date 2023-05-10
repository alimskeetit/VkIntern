using AutoMapper;
using DB;
using Microsoft.EntityFrameworkCore;
using VkIntern.ViewModels;
using VkIntern.ViewModels.UserGroup;
using VkIntern.ViewModels.UserState;

namespace VkIntern
{
    public class AppMappingProfile: Profile
    {
        public AppMappingProfile(AppDbContext _context)
        {
            CreateMap<User, CreateUserViewModel>()
                .ReverseMap()
                .ForMember(user => user.CreatedDate, opt => opt.MapFrom(createUser => DateTime.UtcNow));
            CreateMap<User, UserViewModel>()
                .ForMember(userViewModel => userViewModel.Group,
                    opt => opt.MapFrom(user =>
                        _context.Set<UserGroup>()
                            .FirstOrDefault(usrGroup => usrGroup.Code == user.Group.Code)))
                .ForMember(userViewModel => userViewModel.State,
                    opt => opt.MapFrom(user =>
                        _context.Set<UserState>()
                            .FirstOrDefault(usrState => usrState.Code == user.State.Code)))
                .ReverseMap()
                .ForMember(user => user.GroupId,
                    opt => opt.MapFrom(usrViewModel =>
                        _context.Set<UserGroup>()
                            .FirstOrDefault(usrGroup => usrGroup.Code == usrViewModel.Group.Code).Id))
                .ForMember(user => user.StateId,
                    opt => opt.MapFrom(usrViewModel =>
                        _context.Set<UserState>()
                            .FirstOrDefault(usrState => usrState.Code == usrViewModel.State.Code)
                            .Id));
            CreateMap<UserViewModel, CreateUserViewModel>().ReverseMap();

            CreateMap<UserStateViewModel, UserState>().ReverseMap();

            CreateMap<UserGroup, UserGroupViewModel>().ReverseMap();
        }
    }
}
