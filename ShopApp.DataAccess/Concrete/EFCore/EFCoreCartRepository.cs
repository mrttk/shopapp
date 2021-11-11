using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreCartRepository : EFCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {
        public Cart GetByUserId (string userId)
        {
            using (var context = new ShopContext())
            {
                return context.Carts
                            .Include(i=>i.CartItems)
                            .ThenInclude(i=>i.Product)
                            .FirstOrDefault(i=>i.UserId == userId);
            }
        }
    }
}