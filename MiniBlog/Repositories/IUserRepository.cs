using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBlog.Model;

namespace MiniBlog.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task CreateUser(User user);
        public Task<User?> GetUserById(string id);
        public Task<User?> GetUserByName(string name);
    }
}
