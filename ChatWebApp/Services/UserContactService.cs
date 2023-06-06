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
        Task<UserContact> AddUserContact(string userId, string contactId);
    }
    public class UserContactService : IUserContactService
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserContactService(DataContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
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

        public async Task<UserContact> GetUserContactAsync(string userId, string contactId)
        {
            var userContact = _context.UserContacts.Where(x => x.UserId == Guid.Parse(userId) && x.ContactId == Guid.Parse(contactId)).FirstOrDefault();
            return userContact;
        }
        public async Task<UserContact> AddUserContact(string userId, string contactId)
        {
            Thread.Sleep(3000);
            var existedUserContact = await GetUserContactAsync(userId, contactId);
            if (existedUserContact == null)
            {
                var contact = _userService.GetById(Guid.Parse(contactId));
                var userContact = new UserContact();
                userContact.ContactId = contact.Id;
                userContact.UserId = Guid.Parse(userId);
                _context.UserContacts.Add(userContact);
                await _context.SaveChangesAsync();
                return userContact;
            }
            return existedUserContact;
        }
    }
}
