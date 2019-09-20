using Data;
using Domain;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        private HastagPipointContext _dbContext;

        public ArticlesRepository(HastagPipointContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Article> GetAll()
        {
            return _dbContext.Articles.OrderBy(x => x.Id);
        }

        public Article GetById(int id)
        {
            return _dbContext.Articles.FirstOrDefault(x => x.Id == id);
        }

        public bool Save()
        {
            return (_dbContext.SaveChanges() >= 0);
        }

        public void Update(Article item)
        {
            _dbContext.Articles.Update(item);
            _dbContext.SaveChanges();
        }

        public void Insert(Article item)
        {
            _dbContext.Articles.Add(item);
            _dbContext.SaveChanges();
        }


        public void Delete(Article item)
        {
            _dbContext.Articles.Remove(item);
            _dbContext.SaveChanges();
        }
    }

}
