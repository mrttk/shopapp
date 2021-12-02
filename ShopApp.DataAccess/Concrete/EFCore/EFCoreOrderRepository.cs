using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreOrderRepository : EFCoreGenericRepository<Order>, IOrderRepository
    {
        public EFCoreOrderRepository(ShopContext context) : base(context)
        {
            
        }

        private ShopContext ShopContext
        {
            get { return context as ShopContext; }
        }
        public List<Order> GetOrders(string userId)
        {
            var orders = ShopContext.Orders
                                .Include(i=>i.OrderItems)
                                .ThenInclude(i=>i.Product)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                orders = orders.Where(i=>i.UserId == userId);
            }

            return orders.ToList();
        }
    }
}