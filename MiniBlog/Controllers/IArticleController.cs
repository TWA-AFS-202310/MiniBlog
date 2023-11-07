using Microsoft.AspNetCore.Mvc;
using MiniBlog.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBlog.Controllers
{
    public interface IArticleController
    {
        Task<IActionResult> Create(Article article);
        Task<Article> GetById(string id);
        Task<List<Article>> List();
    }
}