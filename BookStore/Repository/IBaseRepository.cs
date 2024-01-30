using BookStore.DTOs.Book;
using System.Linq.Expressions;

namespace BookStore.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string Id);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> PaginateAsync(int start, int end);
        Task<T> AddAsync(T t);
        Task<T> DeleteAsync(T t);
        void Save();
        Task<T> UpdateAsync(T t);
        Task SaveAsync();
    }
}
