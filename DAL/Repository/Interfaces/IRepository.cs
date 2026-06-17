using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        void Add(T entity);

        Task<T> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);


        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null ,params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetAllWithPaginationAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> expression = null,
       params Expression<Func<T, object>>[] includes);
        void Update(T entity);

        void Delete(T entity);

         Task<List<T>> GetAllWithDeletedAsync(params Expression<Func<T, object>>[] includes);

        Task<T> GetWithStringsAsync(Expression<Func<T, bool>> expression, params string[] includes);
    }
}
