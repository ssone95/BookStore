using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Models;

namespace PetStore.Repository
{
    public interface IStoreRepository
    {
        IQueryable<Article> Articles { get; }
        IQueryable<ArticleType> ArticleTypes { get; }

        void SaveArticle(Article b);
        void CreateArticle(Article b);
        void DeleteArticle(Article b);
        void SaveArticleType(ArticleType g);
        void CreateArticleType(ArticleType g);
        void DeleteArticleType(ArticleType g);
    }
}
