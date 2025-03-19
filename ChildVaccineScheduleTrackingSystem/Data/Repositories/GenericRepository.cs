﻿using Microsoft.EntityFrameworkCore;
using Data.Interface;
using Data.PaggingItem;
using System.Linq.Expressions;
using Data.Base;

namespace Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ChildVaccineScheduleDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ChildVaccineScheduleDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public void Delete(object entity)
        {
            _dbSet.Remove((T)entity);
        }

        public async Task DeleteAsync(object entity)
        {
            _dbSet.Remove((T)entity);
            await Task.CompletedTask;
        }

        public T? Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public T? GetById(object id)
        {

            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<PaginatedList<T>> GetPagging(IQueryable<T> query, int index, int pageSize)
        {
            return await query.GetPaginatedList(index, pageSize);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }
        public void InsertRange(List<T> obj)
        {
            _dbSet.AddRange(obj);
        }



        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }

        //public async Task UpdateAsync(T obj)
        //{
        //    _dbSet.Attach(obj);
        //    _context.Entry(obj).State = EntityState.Modified;
        //}

        public Task UpdateAsync(T obj)
        {
            return Task.FromResult(_dbSet.Update(obj));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task InsertRangeAsync(List<T> obj)
        {
            await _dbSet.AddRangeAsync(obj);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
