using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBlog.Model;
using MiniBlog.Repositories;

namespace MiniBlog.Stores
{
    public class ArticleStore : IArticleRepository
    {
        public ArticleStore()
        {
            Articles = new List<Article>
            {
                new Article(null, "Happy new year", "Happy 2021 new year"),
                new Article(null, "Happy Halloween", "Halloween is coming"),
            };
        }

        public ArticleStore(List<Article> articles)
        {
            Articles = articles;
        }

        public List<Article> Articles { get; set; }

        public Task CreateArticle(Article article)
        {
            Articles.Add(article);
            return Task.CompletedTask;
        }

        public Task<List<Article>> GetAllArticles()
        {
            return Task.FromResult(Articles);
        }

        public Task<Article?> GetArticleById(string id)
        {
            return Task.FromResult(Articles.Find(a => a.Id == id));
        }
    }
}
