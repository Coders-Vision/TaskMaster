using Microsoft.EntityFrameworkCore;
using TaskMasterBackend.Contracts;
using TaskMasterBackend.Database;

namespace TaskMasterBackend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TaskManagerDbContext _context;
        public GenericRepository(TaskManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
         await _context.AddAsync(entity);
         await _context.SaveChangesAsync();
         return entity;
         
        }

        public  async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> Exists(Guid id)
        {
            var entity = await GetAsync(id);
            return entity != null;  
        }

        public async Task<List<T>> GetAllAsync()
        {
          return await _context.Set<T>().ToListAsync(); 
        }
         
        public async Task<T> GetAsync(Guid? id)
        {
            if (id is null)
            {
                return null;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(T entity)
        {
             _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
