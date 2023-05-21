using AutoMapper;
using ChatAppAPI.Authorization;
using ChatAppAPI.Data;
using ChatAppAPI.Entities;
using ChatAppAPI.Helpers;
using ChatAppAPI.Models.Users;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;

namespace ChatAppAPI.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        void Register(CreateRequest model);
        void Update(Guid id, UpdateRequest model);
        void Delete(Guid id);
        ResponseLoginViewModel Login(string username, string password);
    }

    public class UserService : IUserService
    {
        private DataContext _context; 
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;

        public UserService(DataContext context, IMapper mapper, IJwtUtils jwtUtils)
        {
            _context = context;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(Guid id)
        {
            return getUser(id);
        }

        private User getUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
        public void Update(Guid id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
                throw new AppException("Username '" + model.Email + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = PasswordHelper.Hash(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public void Register(CreateRequest model)
        {

            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("Username '" + model.Email + "' is already taken");

            var user = _mapper.Map<User>(model);

            user.PasswordHash = PasswordHelper.Hash(model.Password);
            user.RoleId = 1;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public ResponseLoginViewModel Login(string username, string password)
        {
            var result = new ResponseLoginViewModel();
            var user = _context.Users.SingleOrDefault(x => x.Email == username);
            if (user == null || !PasswordHelper.Verify(password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");
            result.Token = _jwtUtils.GenerateToken(user);
            result.Id = user.Id;
            result.Email = user.Email;
            result.FullName = user.FullName;;
            return result;
        }
    }
}
