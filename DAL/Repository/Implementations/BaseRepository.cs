using DAL.Data;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Repository.Implementations
{
    public class BaseRepository<T> : IRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

         
        #region common methode
    
        private IQueryable<T> ApplyIncludes(IQueryable<T> query, Expression<Func<T, object>>[] includes)
        {
            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);

            }

            return query;
        }    
        
        
        
        #endregion


        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().AnyAsync(expression);

        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _context.Set<T>().Update(entity);
        }

    

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null , params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> Query = _context.Set<T>();

            Query = ApplyIncludes(Query, includes);

            if (expression != null)
            {
                Query = Query.Where(expression);
            }

            return Query.AsNoTracking().ToListAsync();
        }


        public Task<T> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> Query = _context.Set<T>();

            Query = ApplyIncludes(Query, includes);

            return Query.FirstOrDefaultAsync(expression);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);

        }



        public  Task<List<T>> GetAllWithPaginationAsync(int pageNumber,int pageSize,Expression<Func<T, bool>> expression = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> Query = _context.Set<T>();

            Query = ApplyIncludes(Query, includes);

            if (expression != null)
            {
                Query = Query.Where(expression);
            }

            return  Query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public  Task<List<T>> GetAllWithDeletedAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> Query = _context.Set<T>();

            Query = ApplyIncludes(Query, includes);

            return  Query
                .IgnoreQueryFilters()
                .AsNoTracking()
                .ToListAsync();
        }






        public Task<T> GetWithStringsAsync(Expression<Func<T, bool>> expression, params string[] includes)
        {
            IQueryable<T> Query = _context.Set<T>();

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                    Query = Query.Include(include);
            }

            return Query.FirstOrDefaultAsync(expression);
        }
    }
}
