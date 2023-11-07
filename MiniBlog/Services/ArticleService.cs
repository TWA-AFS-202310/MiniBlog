using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Services;

public class ArticleService
{
    private readonly ArticleStore articleStore = null!;
    private readonly UserStore userStore = null!;

    public ArticleService(ArticleStore articleStore, UserStore userStore)
    {
        this.articleStore = articleStore;
        this.userStore = userStore;
    }

    public Article? CreateArticle(Article article)
    {
        if (article.UserName != null)
        {
            if (!userStore.Users.Exists(_ => article.UserName == _.Name))
            {
                userStore.Users.Add(new User(article.UserName));
            }

            articleStore.Articles.Add(article);
        }

        return articleStore.Articles.Find(articleExisted => articleExisted.Title == article.Title);
    }
}
