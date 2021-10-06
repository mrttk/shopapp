using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreGenericRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext, new()
    {
        public void Create(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public List<TEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public TEntity GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}