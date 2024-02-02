using BookStore.Data;
using BookStore.DTOs.Book;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<T> AddAsync(T t)
        {
            await context.Set<T>().AddAsync(t);
            return t;
        }
        public async Task<List<T>> AddRangeAsync(List<T> t)
        {
            await context.Set<T>().AddRangeAsync(t);
            return t;
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
        public async Task SaveAsync()
        {
            await Task.Run(()=> context.SaveChangesAsync());
        }

        public async Task<T> UpdateAsync(T t, string Id)
        {
            if (context.Set<T>().Find(Id) == null)
                return null;
            await Task.Run(()=> context.Set<T>().Update(t));
            return t;
        }

        
    }
}
