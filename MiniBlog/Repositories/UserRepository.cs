using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MiniBlog.Model;
using MongoDB.Driver;

namespace MiniBlog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> userCollection;

        public UserRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("MiniBlog");

            userCollection = mongoDatabase.GetCollection<User>(User.CollectionName);
        }

        public async Task<List<User>> GetAllUsers() =>
            await userCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserById(string id) =>
            await userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetUserByName(string name) =>
            await userCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

        public async Task CreateUser(User newUser) =>
            await userCollection.InsertOneAsync(newUser);

        public async Task UpdateUserById(string id, User updatedUser) =>
            await userCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveUserById(string? id) =>
            await userCollection.DeleteOneAsync(x => x.Id == id);
    }
}
