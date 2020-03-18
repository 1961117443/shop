using FreeSql;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    { 

        //public BaseService()
        //{
        //    this.freeSqlInstance = CreateFreeSql();
        //}

        public BaseService(IFreeSql freeSql)
        {
            this.Instance = freeSql ?? throw new ArgumentNullException(nameof(freeSql));
        }

        internal virtual IFreeSql CreateFreeSql()
        {
            //默认通过反射获取 IFreeSql 对象
            var prop = this.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField).FirstOrDefault(p => p.DeclaringType.Equals(typeof(IFreeSql)));
            if (prop != null)
            {
                return prop.GetValue(this) as IFreeSql;
            }
            return null;
        }

        /// <summary>
        /// 查询所有属性的sql语句，包含外键，从表
        /// </summary>
        /// <returns></returns>
        protected virtual ISelect<T> SelectEntity()
        {
            return this.Select;
        }

        public IFreeSql Instance
        {
            get; protected set;
        }

        protected virtual ISelect<T> Select
        {
            get
            {
                if (this.Instance == null)
                {
                    this.Instance = CreateFreeSql();
                }
                if (this.Instance == null)
                {
                    throw new ArgumentNullException("FreeSql实例对象为空！");
                }
                return this.Instance.Select<T>();
            }
        }
        public virtual async Task<IList<T>> GetListAsync()
        {
            return await this.Select.ToListAsync();
        }

        public virtual async Task<IList<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            var select = this.Select;
            if (where != null)
            {
                select = select.Where(where);
            }
            return await select.ToListAsync();
        }

        public virtual async Task<IList<T>> GetPageListAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, object>> order)
        {

            var query = this.SelectEntity().WhereIf(where != null, where);
            if (pageSize>0)
            {
                query = query.Page(pageIndex, pageSize);
            }
            
            if (order != null)
            {
                query = query.OrderBy(order);
            }
            return await query.ToListAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> where = null)
        {
            var query = this.Select.WhereIf(where != null, where);
            var res = await query.CountAsync();
            return (int)res;
        }

        
        public async Task<bool> UpdateAsync(T entity, Expression<Func<T, T>> func = null, Expression<Func<T, bool>> where = null)
        {
            var updater = this.Instance.Update<T>(entity);
            if (func != null)
            {
                updater = updater.Set(func);
            }
            else
            {
                updater = updater.SetSource(entity);
            }
            if (where != null)
            {
                updater = updater.Where(where);
            }
            int res = await updater.ExecuteAffrowsAsync();
            return res > 0;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await this.SelectEntity().WhereIf(where != null, where).ToOneAsync();
        }


        public async Task<T> GetAsync<TKey>(TKey id) where TKey : struct
        { 
            return await this.Instance.GetRepository<T, TKey>().GetAsync(id);
        }

        public async Task<bool> InsertAsync(T entity)
        {
            var res = await this.Instance.Insert(entity).ExecuteAffrowsAsync();
            return res > 0;
        }

        public async Task<bool> InsertAsync(IList<T> items)
        {
            return await this.Instance.Insert<T>().AppendData(items).ExecuteAffrowsAsync() > 0;
        }

        public async Task<bool> UpdateAsync(IList<T> items)
        {
            return await this.Instance.Update<T>().SetSource(items).ExecuteAffrowsAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            var res =await this.Instance.Delete<T>().Where(where).ExecuteAffrowsAsync();
            return res > 0;
        }

        public IList<T> GetPageList(int page, int limit, out int total, Expression<Func<T, bool>> where = null, Expression<Func<T, object>> order = null, bool asc = true)
        {
            var query = this.SelectEntity().WhereIf(where != null, where);
            if (order != null)
            {
                if (asc)
                {
                    query = query.OrderBy(order);
                }
                else
                {
                    query = query.OrderByDescending(order);
                }
               
            }
            total = (int)query.Count();
            var list = query.Page(page, limit).ToList();
            return list;
        }
    }
}
