using AutoMapper;
using ChatAppAPI.Authorization;
using ChatAppAPI.Data;
using ChatAppAPI.Dtos.UserContact;
using ChatAppAPI.Entities;
using ChatAppAPI.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Services
{
    public interface IUserContactService
    {
        IEnumerable<UserContactDto> GetUserContactDtos(string userId);
    }
    public class UserContactService : IUserContactService
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;

        public UserContactService(DataContext context, IMapper mapper, IJwtUtils jwtUtils)
        {
            _context = context;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }

        public IEnumerable<UserContactDto> GetUserContactDtos(string userId)
        {
            var result = new List<UserContactDto>();
            var userContacts = _context.UserContacts.Where(usercontact => usercontact.UserId.ToString() == userId && usercontact.ContactId.ToString() != userId).Include(x => x.Contact).ToList().DistinctBy(x => x.ContactId);

            foreach ( var usercontact in userContacts)
            {
                var newUserContact = _mapper.Map<User, UserContactDto>(usercontact.Contact);
                result.Add(newUserContact);
            }
            return result;
        }
    }
}
