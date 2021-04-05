using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XChange.Api.Repositories.Interfaces
{

    public interface IBaseRepository<T>
    {
        Task<T> GetAsync(int id);

        IQueryable<T> Query();

        IEnumerable<T> QueryEnum();

        Task InsertAsync(T entity);

        Task InsertRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        Task Commit();
    }

}
