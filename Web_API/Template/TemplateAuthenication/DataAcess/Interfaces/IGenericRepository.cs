using System;
using System.Collections.Generic;
using System.Text;


namespace DataAcess.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void CreateEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
    }
}
