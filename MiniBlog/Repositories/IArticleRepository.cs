using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBlog.Model;

namespace MiniBlog.Repositories
{
    public interface IArticleRepository
    {
        public Task<List<Article>> GetAllArticles();
        public Task CreateArticle(Article article);
        public Task<Article?> GetArticleById(string id);
    }
}
