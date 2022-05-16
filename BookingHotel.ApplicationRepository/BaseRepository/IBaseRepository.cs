using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.ApplicationRepository.BaseRepository
{
    public interface IBaseRepository<T> where T : class
    {
        #region LoadData
        T FindById(object id);
        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindByIdAsync(object id);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);
        Task<bool> FindAsync(params object[] keyValues);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll();
        #endregion

        #region Action
        void Add(T entity);
        void AddRange(List<T> entity);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Remove(T entity);
        void Remove(object id);
        void RemoveMultiple(List<T> entities);

        Task<bool> SaveAll();
        void Save();
        #endregion
    }
}
