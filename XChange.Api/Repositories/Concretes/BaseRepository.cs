using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class, new()
    {
        protected  XChangeDatabaseContext _dbContext { get; set; }

        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
        public IEnumerable<T> QueryEnum()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public async Task InsertAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.Entry(entities).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task Commit()
        {
            try
            {
                int x = _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}",
                                                 validationError.PropertyName,
                                                 validationError.ErrorMessage);
                    }
                }

                throw new Exception(msg);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
