using MiniBlog.Model;
using MiniBlog.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MiniBlogTest.RepositoryTest
{
    public class ArticleRepositoryTest
    {
        [Fact]
        public void Should_return_all_article_When_getArticles()
        {
            var mock = new Mock<IArticleRepository>();
            List<Article> result = new List<Article>
            {
                new Article(null, "Happy new year", "Happy 2021 new year"),
                new Article(null, "Happy Halloween", "Halloween is coming"),
            };
            mock.Setup(repository => repository.GetArticles()).Returns(Task.FromResult(result));

            IArticleRepository articleRepository = mock.Object;
            Task<List<Article>> articles = articleRepository.GetArticles();

            mock.Verify(repository => repository.GetArticles(), Times.Once());
        }

        [Fact]
        public void Should_return_created_article_When_createArticle()
        {
            var mock = new Mock<IArticleRepository>();
            string userNameWhoWillAdd = "Tom";
            string articleContent = "What a good day today!";
            string articleTitle = "Good day";
            Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);
            mock.Setup(repository => repository.CreateArticle(article)).Returns(Task.FromResult(article));

            IArticleRepository articleRepository = mock.Object;
            Task<Article> articles = articleRepository.CreateArticle(article);

            mock.Verify(repository => repository.CreateArticle(article), Times.Once());
        }

        [Fact]
        public void Should_return_the_article_When_getArticle_Given_id()
        {
            var mock = new Mock<IArticleRepository>();
            string userNameWhoWillAdd = "Tom";
            string articleContent = "What a good day today!";
            string articleTitle = "Good day";
            string id = "mock_id";
            Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);
            mock.Setup(repository => repository.GetArticle(id)).Returns(Task.FromResult(article));

            IArticleRepository articleRepository = mock.Object;
            Task<Article> articles = articleRepository.GetArticle(id);

            mock.Verify(repository => repository.GetArticle(id), Times.Once());
        }
    }
}
