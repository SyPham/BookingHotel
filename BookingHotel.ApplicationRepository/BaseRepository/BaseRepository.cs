using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.Data.Entities;
using BookingHotel.ApplicationRepository.Implementation;

namespace BookingHotel.ApplicationRepository.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Field
        private readonly VietCouponDBContext _context;
        #endregion

        #region Ctor
        public BaseRepository(VietCouponDBContext context)
        {
            _context = context;
        }
        #endregion

        #region Action
        public void Add(T entity)
        {
            _context.Add(entity);
        }
        public void AddRange(List<T> entity)
        {
            _context.AddRange(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void UpdateRange(List<T> entities)
        {
            var set = _context.Set<T>();

            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType.FindPrimaryKey();
            var keyValues = new object[primaryKey.Properties.Count];

            foreach (T e in entities)
            {
                for (int i = 0; i < keyValues.Length; i++)
                    keyValues[i] = primaryKey.Properties[i].GetGetter().GetClrValue(e);

                var obj = set.Find(keyValues);

                if (obj == null)
                {
                    set.Add(e);
                }
                else
                {
                    _context.Entry(obj).CurrentValues.SetValues(e);
                }
            }
            //_context.Set<T>().UpdateRange(entities);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Remove(object id)
        {
            Remove(FindById(id));
        }
        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        #endregion

        #region LoadData

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.AsQueryable();
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate).AsQueryable();
        }

        public T FindById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }
        public async Task<T> FindByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<bool> FindAsync(params object[] keyValues)
        {
            return await _context.Set<T>().FindAsync(keyValues) != null;
        }

        #endregion

    }
}
