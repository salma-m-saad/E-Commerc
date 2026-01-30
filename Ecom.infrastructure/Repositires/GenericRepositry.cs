using Ecom.core.Interfaces;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositires
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepositry(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        => await _context.Set<T>().CountAsync();

        public async Task DeleteAsync(int id)
        {
           var entity=await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync(params System.Linq.Expressions.Expression<Func<T, object>>[] include)
        {
            var query= _context.Set<T>().AsQueryable();
            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }
            return await query.AsNoTracking().ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
           var entity =await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public Task<T> GetByIdAsync(int id, params System.Linq.Expressions.Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }
            var entity = query.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID") == id);
            return entity;

        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
      
    }
}
