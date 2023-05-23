using AntonDB;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Core;
using AntonS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Repositories.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly AntonDBContext _context;
        protected readonly DbSet<TEntity> _entities;
        public Repository(AntonDBContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public virtual async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await _context.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
             await _context.AddRangeAsync(entities);
        }

        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync();
        }

        public async void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var result = _entities.Where(predicate);
            if (includes.Any())
            {
                result = includes
                    .Aggregate(result,
                        (current, include)
                            => current.Include(include));                
            }
            return result;
        }

        public Task<List<TEntity>> GetAllAsync()
        {
           var ents =  _entities.AsQueryable().ToListAsync();
            return ents;
        }

        public IQueryable<TEntity> GetAsQueryable()
        {
            return _entities.AsQueryable();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task Remove(int id)
        {
            var entity = await _entities.FirstOrDefaultAsync(entity => entity.Id == id);
            _entities.Remove(entity);
        }

        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public async Task Update(TEntity entity)
        {
            _entities.Update(entity);
        }
        public virtual async Task PatchAsync(int id, List<PatchDTO> patchDtos)
        {
            var entity =
                await _entities.FirstOrDefaultAsync(ent => ent.Id == id);

            var nameValuePairProperties = patchDtos.ToDictionary
            (k => k.Name,
                v => v.Value);

            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
            dbEntityEntry.State = EntityState.Modified;
        }
    }
}
