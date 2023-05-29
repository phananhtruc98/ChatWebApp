using AutoMapper;
using ChatAppAPI.Dtos.UserContact;
using ChatAppAPI.Entities;
using ChatAppAPI.Models.Users;

namespace ChatAppAPI.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // CreateRequest -> User
            CreateMap<UserForCreationDto, User>();

            // UpdateRequest -> User
            CreateMap<UserForUpdateDto, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore both null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));


            CreateMap<User, UserContactDto>();
            //CreateMap<UserContactDto, UserContact>().ForMember(d => d.Contact, opt => opt.MapFrom(s => s));
        }
    }
}
