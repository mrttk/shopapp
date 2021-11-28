using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreCartRepository : EFCoreGenericRepository<Cart>, ICartRepository
    {
        public EFCoreCartRepository(ShopContext context) : base(context)
        {
            
        }

        private ShopContext ShopContext
        {
            get { return context as ShopContext; }
        }
        public void ClearCart(int cartId)
        {
            var command = @"delete from CartItems where CartId=@p0";
            ShopContext.Database.ExecuteSqlRaw(command, cartId);
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var command = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
            ShopContext.Database.ExecuteSqlRaw(command, cartId, productId);
        }

        public Cart GetByUserId (string userId)
        {
            return ShopContext.Carts
                        .Include(i=>i.CartItems)
                        .ThenInclude(i=>i.Product)
                        .FirstOrDefault(i=>i.UserId == userId);
        }
        public override void Update(Cart entity)
        {
            ShopContext.Carts.Update(entity);
        }
    }
}