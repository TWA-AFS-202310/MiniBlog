using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MiniBlog.Model;
using MongoDB.Driver;

namespace MiniBlog.Repositories
{
    public class ArticleRepository
    {
        private readonly IMongoCollection<Article> articleCollection;

        public ArticleRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("MiniBlog");

            articleCollection = mongoDatabase.GetCollection<Article>(Article.CollectionName);
        }

        public async Task<List<Article>> GetAllArticles() =>
            await articleCollection.Find(_ => true).ToListAsync();
    }
}
