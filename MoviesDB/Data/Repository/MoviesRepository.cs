﻿using Microsoft.EntityFrameworkCore;
using MoviesDB.Models;
using System.Drawing;
using System.Linq.Expressions;
using System.Security.Policy;

namespace MoviesDB.Data.Repository
{
    public class MoviesRepository<T> : IMoviesRepository<T> where T : class
    {
        private readonly SakilaDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public MoviesRepository(SakilaDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
            {
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }

            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();

            return dbRecord;
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            _dbSet.Entry(dbRecord).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> Count()
        {
            return await _dbSet.CountAsync();
        }

        public List<T> GetPaginated(int page, int size)
        {
            var records = _dbSet.ToList();

            return records.Skip((page - 1) * size).Take(size).ToList();
        }

        public async Task<T> GetLastAsync()
        {
            var cnt = _dbSet.ToList().Count();

            return await _dbSet.Skip(cnt - 1).Take(1).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstAsync()
        {
            return await _dbSet.FirstOrDefaultAsync();
        }
    }
}
