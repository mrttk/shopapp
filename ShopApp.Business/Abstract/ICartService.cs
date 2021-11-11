using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);
        Cart GetCartByUserId (string userId);
    }
}