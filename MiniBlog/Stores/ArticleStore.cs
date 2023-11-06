using System;
using System.Collections.Generic;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class ArticleStore
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
    }
}
