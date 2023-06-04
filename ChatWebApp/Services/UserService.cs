using AutoMapper;
using ChatAppAPI.Authorization;
using ChatAppAPI.Data;
using ChatAppAPI.Entities;
using ChatAppAPI.Helpers;
using ChatAppAPI.Models.Users;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Imagekit;
using Imagekit.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using System.IO;

namespace ChatAppAPI.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        IEnumerable<User> GetSuggestions(string userId);
        User GetById(Guid id);
        void Register(UserForCreationDto model);
        void Update(Guid id, UserForUpdateDto model);
        void Delete(Guid id);
        string Login(string username, string password);
        Task<Result> SaveAvatar(IFormFile file);
        Task<User> UpdateAvatar(Guid id, string avatar);
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
        public void Update(Guid id, UserForUpdateDto model)
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
        public async Task<User> UpdateAvatar(Guid id, string avatar)
        {
            var user = getUser(id);
            user.Avatar = avatar;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public void Delete(Guid id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public void Register(UserForCreationDto model)
        {

            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("Username '" + model.Email + "' is already taken");
            
            var user = _mapper.Map<User>(model);
            user.Avatar = "default-avatar.png";
            user.PasswordHash = PasswordHelper.Hash(model.Password);
            user.RoleId = 1;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public string Login(string username, string password)
        {
            var result = new UserDto();
            var user = _context.Users.SingleOrDefault(x => x.Email == username);
            if (user == null || !PasswordHelper.Verify(password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");
            result.Token = _jwtUtils.GenerateToken(user);
            result.Id = user.Id;
            result.Email = user.Email;
            result.FullName = user.FullName;
            ;
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public IEnumerable<User> GetSuggestions(string userId)
        {           
            var user = _context.Users.Where(x => x.Id == Guid.Parse(userId)).Include(x => x.Contacts).FirstOrDefault();
            var contacts = user.Contacts.Select(x => x.ContactId);
            var exceptList = new List<Guid>() { user.Id };
            exceptList.AddRange(contacts);
            var suggestions = this.GetAll().ExceptBy(exceptList, x=>x.Id);
            return suggestions;
        }
        public async Task<Result> SaveAvatar(IFormFile file)
        {
            var filesPath = Directory.GetCurrentDirectory() + "\\Uploadfiles";
            if (!System.IO.Directory.Exists(filesPath))//create path 
            {
                Directory.CreateDirectory(filesPath);
            }
            var path = Path.Combine(filesPath, Path.GetFileName(file.FileName));
            var fileStream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(fileStream);
            fileStream.Close();
            var pathTest = filesPath + "\\"+ file.FileName;
            byte[] readText = File.ReadAllBytes(pathTest);

            var base64String = Convert.ToBase64String(readText);
            var imageKit = new ImagekitClient("public_BCskujhJD0iuvZFK5kWy7NF9bRQ=", "private_Zy5hINsuTadcBM8wt4QOF2/ctFc=", "https://ik.imagekit.io/anhtrucphan/");
           
            // Upload by Base64
            FileCreateRequest ob2 = new FileCreateRequest
            {
                file = base64String,
                fileName = Guid.NewGuid().ToString()
            };
            Result resp = imageKit.Upload(ob2);
            return resp;
        }
    }
}
