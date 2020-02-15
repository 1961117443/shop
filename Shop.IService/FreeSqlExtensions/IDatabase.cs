using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService
{
    public interface IMySql : IFreeSql<IMySql> { }
    public interface IDBLog : IFreeSql<IDBLog> { }
    public interface IMetaDatabase : IFreeSql<IMetaDatabase> { }
}
