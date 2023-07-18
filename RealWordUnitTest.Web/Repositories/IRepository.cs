namespace RealWordUnitTest.Web.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>>GetAll();
        Task<TEntity>GetByID(int Id);
        Task Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
