using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        public BaseRepository(ApplicationDbContext _context)
        {
            context = _context;
        }



        public async Task<List<T>> GetAllAsync()
        {

            return await context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> PaginateAsync(int start, int end)
        {

            return await
                context
                .Set<T>()
                .Skip(start)
                .Take(end)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().FirstOrDefaultAsync(expression);
        }
        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.FromResult(context.Set<T>().Where(expression).ToList());
        }
        public async void AddAsync(T t)
        {
            await context.Set<T>().AddAsync(t);
        }
        public void UpdateAsync(T t)
        {
            Task.FromResult(context.Set<T>().Update(t));
        }
        public async Task<T> DeleteAsync(T t)
        {
            await Task.Run(() => context.Remove(t));
            return t;
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public async void SaveAsync()
        {
            await context.SaveChangesAsync();
        }


    }
}
