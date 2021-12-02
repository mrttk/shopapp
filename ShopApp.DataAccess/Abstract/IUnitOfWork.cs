using System;

namespace ShopApp.DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        ICartRepository Carts {get;}
        ICategoryRepository Categories {get;}
        IOrderRepository Orders {get;}
        IProductRepository Products {get;}
        void Save();
    }
}