using ShopApp.DataAccess.Abstract;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext _context;
        public UnitOfWork(ShopContext context)
        {
            _context = context;
        }

        private EFCoreCartRepository _cartRepository;
        private EFCoreCategoryRepository _categoryRepository;
        private EFCoreOrderRepository _orderRepository;
        private EFCoreProductRepository _productRepository;

        public ICartRepository Carts => 
            _cartRepository = _cartRepository ?? new EFCoreCartRepository(_context);

        public ICategoryRepository Categories => 
            _categoryRepository = _categoryRepository ?? new EFCoreCategoryRepository(_context);

        public IOrderRepository Orders => 
            _orderRepository = _orderRepository ?? new EFCoreOrderRepository(_context);

        public IProductRepository Products => 
            _productRepository = _productRepository ?? new EFCoreProductRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}