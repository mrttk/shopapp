using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart { UserId = userId});
        }
    }
}