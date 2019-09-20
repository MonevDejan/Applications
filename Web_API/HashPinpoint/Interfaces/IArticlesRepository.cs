using Domain;
using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IArticlesRepository
    {
        Article GetById(int id);
        void Insert(Article item);
        void Update(Article item);
        IEnumerable<Article> GetAll();
        bool Save();
        void Delete(Article item);
    }
}
