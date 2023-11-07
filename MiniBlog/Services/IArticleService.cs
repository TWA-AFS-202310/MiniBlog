using MiniBlog.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBlog.Services
{
    public interface IArticleService
    {
        Task<Article?> CreateArticle(Article article);
        Task<List<Article>> GetAll();
        Task<Article> GetById(string id);
    }
}