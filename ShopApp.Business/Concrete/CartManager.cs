using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class CartManager : ICartService
    {
        private IUnitOfWork _uniofwork;
        public CartManager(IUnitOfWork uniofwork)
        {
            _uniofwork = uniofwork;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(i=>i.ProductId == productId);

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem(){
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }

                _uniofwork.Carts.Update(cart);
                _uniofwork.Save();
            }
        }

        public void ClearCart(int cartId)
        {
            _uniofwork.Carts.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);

            if (cart != null)
            {
                _uniofwork.Carts.DeleteFromCart(cart.Id, productId);
                _uniofwork.Save();
            }
        }

        public Cart GetCartByUserId (string userId)
        {
            return _uniofwork.Carts.GetByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _uniofwork.Carts.Create(new Cart { UserId = userId});
            _uniofwork.Save();
        }
    }
}