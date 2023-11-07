using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBlog.Model;
using MiniBlog.Repositories;

namespace MiniBlog.Stores
{
    public class UserStore : IUserRepository
    {
        public UserStore()
        {
            Users = new List<User>
            {
                new User("Andrew", "1@1.com"),
                new User("William", "2@2.com"),
            };
        }

        public UserStore(List<User> users)
        {
            Users = users;
        }

        public List<User> Users { get; set; }

        public Task CreateUser(User user)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }

        public Task<List<User>> GetAllUsers()
        {
            return Task.FromResult(Users);
        }

        public Task<User?> GetUserById(string id)
        {
            return Task.FromResult(Users.Find(u => u.Id == id));
        }

        public Task<User?> GetUserByName(string name)
        {
            return Task.FromResult(Users.Find(u => u.Name == name));
        }
    }
}
