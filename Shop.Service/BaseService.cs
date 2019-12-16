using FreeSql;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public abstract class BaseService<T> : IBaseService<T> where T : class
    {
        public BaseService()
        {
            this.freeSqlInstance = CreateFreeSql();
        }

        internal abstract IFreeSql CreateFreeSql();

        public IFreeSql freeSqlInstance { get; private set; }
        protected virtual ISelect<T> Select
        {
            get
            {
                if (this.freeSqlInstance==null)
                {
                    this.freeSqlInstance = CreateFreeSql();
                }
                if (this.freeSqlInstance==null)
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
    }
}
