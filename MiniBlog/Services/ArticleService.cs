using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniBlog.Model;
using MiniBlog.Repositories;
using MiniBlog.Stores;

namespace MiniBlog.Services;

public class ArticleService
{
    private readonly IArticleRepository articleRepository = null!;
    private readonly IUserRepository userRepository = null!;

    public ArticleService(IArticleRepository articleRepository, IUserRepository userRepository)
    {
        this.articleRepository = articleRepository;
        this.userRepository = userRepository;
    }

    public async Task<Article?> CreateArticle(Article article)
    {
        if (article.UserName != null)
        {
            User? user = await userRepository.GetUserByName(article.UserName);
            if (user == null)
            {
                await userRepository.CreateUser(new User(article.UserName));
            }

            await articleRepository.CreateArticle(article);
            return article;
        }

        return null;
    }

    public async Task<List<Article>> GetAll()
    {
        return await articleRepository.GetAllArticles();
    }

    public async Task<Article?> GetById(string id)
    {
        return await articleRepository.GetArticleById(id.ToString());
    }
}
