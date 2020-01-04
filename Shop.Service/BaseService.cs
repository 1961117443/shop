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
        public BaseService()
        {
            this.freeSqlInstance = CreateFreeSql();
        }

        public BaseService(IFreeSql freeSql)
        {
            this.freeSqlInstance = freeSql;
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

        public IFreeSql freeSqlInstance
        {
            get; private set;
        }
         
        protected virtual ISelect<T> Select
        {
            get
            {
                if (this.freeSqlInstance == null)
                {
                    this.freeSqlInstance = CreateFreeSql();
                }
                if (this.freeSqlInstance == null)
                {
                    throw new ArgumentNullException("FreeSql实例对象为空！");
                }
                return this.freeSqlInstance.Select<T>();
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
            var query = this.SelectEntity().Page(pageIndex, pageSize).WhereIf(where != null, where);
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
            var updater = this.freeSqlInstance.Update<T>(entity);
            if (func != null)
            {
                updater = updater.Set(func);
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
            return await this.Select.WhereIf(where != null, where).ToOneAsync();
        }


        public async Task<T> GetAsync<TKey>(TKey id) where TKey : struct
        { 
            return await this.freeSqlInstance.GetRepository<T, TKey>().GetAsync(id);
        }
    }
}
