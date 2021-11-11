using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetByUserId (string userId);
    }
}