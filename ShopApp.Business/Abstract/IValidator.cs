namespace ShopApp.Business.Abstract
{
    public interface IValidator<TEntity>
    {
        public string ErrorMessage { get; set; }
        public bool Validation (TEntity entity);
    }
}