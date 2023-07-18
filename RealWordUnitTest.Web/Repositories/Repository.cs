using Microsoft.EntityFrameworkCore;
using RealWordUnitTest.Web.Models;

namespace RealWordUnitTest.Web.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Context _context;

        private readonly DbSet<TEntity> _dbSet;

        public Repository(Context context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
             _dbSet.Remove(entity);
             _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
           return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByID(int Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified; //Direkt yansıtmak yerine sadece verinin memorydeki durumunu değiştirir. Bu yüZden async yoktur.
            // _dbSet.Update(entity);
            _context.SaveChangesAsync();
        }
    }
}
