using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}