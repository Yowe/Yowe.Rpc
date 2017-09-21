using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Vice.Entity;

namespace Vice.Repository
{
    public class Repository<TEntity> where TEntity:class
    {
        internal ViceDataContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(ViceDataContext _context)
        {
            this.context = _context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperty="")
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            foreach (var prop in includeProperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual IEnumerable<TEntity> GetListBySql(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters);
        }
    }
}