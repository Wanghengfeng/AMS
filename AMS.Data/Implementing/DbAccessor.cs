using AMS.Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AMS.Data.Implementing
{
    public class DbAccessor : IDbAccessor, IDisposable
    {
        #region ctor
        private AMSContext _amsContext;
        private readonly IServiceProvider _rootProvider;
        public DbAccessor(AMSContext amsContext, IServiceProvider serviceProvider)
        {
            _amsContext = amsContext;
            _rootProvider = serviceProvider;
        }
        #endregion

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (AMSContext.Entry(entity).State == EntityState.Detached)
            {
                AMSContext.Attach(entity);
            }
            AMSContext.Remove(entity);
        }

        public void DeleteById<TEntity>(params object[] ids) where TEntity : class
        {
            Delete(GetById<TEntity>(ids));
        }

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities, int batchSize = 100, bool autoCommitEnabled = false) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            if (entities.Count() > 0)
            {
                foreach (var entity in entities)
                {
                    Delete(entity);
                }
            }
        }

        public AMSContext AMSContext
        {
            get
            {
                if (_amsContext == null)//不保证线程安全
                {
                    _amsContext = _rootProvider.CreateScope().ServiceProvider.GetService(typeof(AMSContext)) as AMSContext;
                }
                return _amsContext;
            }
        }

        public void Dispose()
        {
            _amsContext.Dispose();
            _amsContext = null;
        }

        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable ExeSqlReturnDT(string sql, SqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Expand<TEntity>(IQueryable<TEntity> query, string path) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return AMSContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) where TEntity : class
        {
            IQueryable<TEntity> query = AMSContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public TEntity GetById<TEntity>(params object[] ids) where TEntity : class
        {
            return AMSContext.Find<TEntity>(ids);
        }

        public IDictionary<string, object> GetModifiedProperties<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            AMSContext.Add(entity);
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities, int batchSize = 100, bool autoCommitEnabled = false) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            if (entities.Count() > 0)
            {
                foreach (var entity in entities)
                {
                    Insert(entity);
                }
            }
        }

        public IQueryable<TResult> Join<TEntityOuter, TEntityInner, TResult>(Func<TEntityOuter, object> outerKeySelector, Func<TEntityInner, object> innerKeySelector, Func<TEntityOuter, TEntityInner, TResult> resultSelector)
            where TEntityOuter : class
            where TEntityInner : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<TResult> Join<TEntityOuter, TEntityInner, TResult>(Func<TEntityOuter, object> outerKeySelector, Func<TEntityInner, object> innerKeySelector, Func<TEntityOuter, TEntityInner, TResult> resultSelector, IEqualityComparer<object> comparer)
            where TEntityOuter : class
            where TEntityInner : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<TResult> LeftJoin<TEntityOuter, TEntityInner, TResult>(Func<TEntityOuter, object> outerKeySelector, Func<TEntityInner, object> innerKeySelector, Func<TEntityOuter, TEntityInner, TResult> resultSelector)
            where TEntityOuter : class
            where TEntityInner : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<TResult> LeftJoin<TEntityOuter, TEntityInner, TResult>(Func<TEntityOuter, object> outerKeySelector, Func<TEntityInner, object> innerKeySelector, Func<TEntityOuter, TEntityInner, TResult> resultSelector, IEqualityComparer<object> comparer)
            where TEntityOuter : class
            where TEntityInner : class
        {
            throw new NotImplementedException();
        }

        public void Query(Action query)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges(bool isAsync = false)
        {
            if (isAsync)
            {
                AMSContext.SaveChangesAsync();
            }
            else
            {
                AMSContext.SaveChanges();
            }
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            var track = AMSContext.Entry(entity);
            if (track.State == EntityState.Detached)
            {
                AMSContext.Attach(entity);
            }
            else
            {
                track.CurrentValues.SetValues(entity);
            }

            track.State = EntityState.Modified;
            var createdTime = track.Property("CreatedTime");
            if (createdTime != null)
            {
                createdTime.IsModified = false;//不更新创建时间
            }
            var CreatedBy = track.Property("CreatedBy");
            if (CreatedBy != null)
            {
                CreatedBy.IsModified = false;//不更新创建人
            }
        }
    }
}
