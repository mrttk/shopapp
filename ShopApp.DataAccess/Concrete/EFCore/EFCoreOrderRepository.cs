using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreOrderRepository : EFCoreGenericRepository<Order, ShopContext>, IOrderRepository
    {
        
    }
}