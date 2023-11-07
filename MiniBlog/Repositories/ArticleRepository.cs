using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MiniBlog.Model;
using MongoDB.Driver;

namespace MiniBlog.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IMongoCollection<Article> articleCollection;

        public ArticleRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("MiniBlog");

            articleCollection = mongoDatabase.GetCollection<Article>(Article.CollectionName);
        }

        public async Task<List<Article>> GetAllArticles() =>
            await articleCollection.Find(_ => true).ToListAsync();

        public async Task<Article?> GetArticleById(string id) =>
            await articleCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateArticle(Article newArticle) =>
            await articleCollection.InsertOneAsync(newArticle);

        public async Task UpdateAsync(string id, Article updatedArticle) =>
            await articleCollection.ReplaceOneAsync(x => x.Id == id, updatedArticle);

        public async Task RemoveAsync(string? id) =>
            await articleCollection.DeleteOneAsync(x => x.Id == id);
    }
}
