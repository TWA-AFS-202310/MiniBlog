using MiniBlog.Model;
using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBlog.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> userCollection;

        public UserRepository(IMongoClient client)
        {
            var mongoDatabase = client.GetDatabase("MiniBlog");

            userCollection = mongoDatabase.GetCollection<User>(User.CollectionName);
        }

        public async Task<User> CreateUser(User user)
        {
            await userCollection.InsertOneAsync(user);
            return await userCollection.Find(u => u.Name == user.Name).FirstAsync();
        }

        public async Task<User> GetUser(string name)
        {
            return await userCollection.Find(u => u.Name == name).FirstOrDefaultAsync();
        }
    }
}
