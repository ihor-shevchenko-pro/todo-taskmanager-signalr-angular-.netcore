using AutoMapper;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Entities.UserAccount;
using System.Linq;

namespace signalr_best_practice_core.Configuration
{
    public class DataMapperConfig : Profile
    {
        public DataMapperConfig()
        {
            // UserProfile
            CreateMap<UserProfile, UserProfileGetFullApiModel>();
            CreateMap<UserProfile, UserProfileGetMinApiModel>();
            CreateMap<UserProfileAddApiModel, UserProfile>();

            //User
            CreateMap<User, UserGetFullApiModel>()
                .ForMember(d => d.Roles, c => c.MapFrom(dc => dc.UserRoles.Select(x => x.Role)));
            CreateMap<User, UserGetMinApiModel>();
            CreateMap<UserAddApiModel, User>();

            //Role
            CreateMap<Role, RoleGetFullApiModel>()
                .ForMember(x => x.Users, y => y.MapFrom(xy => xy.UserRoles.Select(z => z.User)));
            CreateMap<Role, RoleGetMinApiModel>();
            CreateMap<RoleAddApiModel, Role>();

            //ToDoTask
            CreateMap<ToDoTask, ToDoTaskGetFullApiModel>();
            CreateMap<ToDoTask, ToDoTaskGetMinApiModel>();
            CreateMap<ToDoTaskAddApiModel, ToDoTask>();
        }

        public static IMapper GetMapper()
        {
            var mapplineConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DataMapperConfig());
            });

            IMapper mapper = mapplineConfig.CreateMapper();

            return mapper;
        }
    }
}
