using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService.FreeSqlExtensions
{
    public interface IMySql : IFreeSql<IMySql> { }
    public interface IDBLog : IFreeSql<IDBLog> { }
}
