using FreeSql;
using FreeSql.DataAnnotations;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    
    public class DataTranService : IDataTranService
    {
        private readonly IFreeSql sqlServer;
        private readonly IFreeSql<IMySql> mySql;
        private static Assembly entityAssembly = Assembly.Load("Shop.EntityModel");
        private static Type[] entityTypes;
        private Type[] EntityTypes
        {
            get
            {
                if (entityTypes==null)
                {
                    entityTypes = entityAssembly.GetTypes().Where(w => !w.IsAbstract && !w.IsInterface).ToArray();
                }
                return entityTypes;
            }
        }


        public DataTranService(IFreeSql sqlserver,IFreeSql<IMySql> mySql)
        {
            this.sqlServer = sqlserver;
            this.mySql = mySql;
            this.mySql.CodeFirst.ConfigEntity<SectionBar>(a =>
            { 
                a.Property(o => o.Diagram).DbType("Blob");
            });
        }

        public async Task<string> StructureSQL()
        {
            string sql = mySql.CodeFirst.GetComparisonDDLStatements(EntityTypes);
            return await Task.FromResult(sql);
        }

        static MethodInfo DeleteMehtod = typeof(IFreeSql).GetMethod("Delete", 1, new Type[] { });
        static MethodInfo InsertMehtod = typeof(IFreeSql).GetMethod("Insert", 1, new Type[] { });
        static MethodInfo SelectMehtod = typeof(IFreeSql).GetMethod("Select", 1, new Type[] { });
        public async Task<bool> Tran()
        {
            //int c = mySql.Delete<Packing>().Where("1=1").ExecuteAffrows();
            foreach (var entity in EntityTypes)
            {
                MethodInfo method = DeleteMehtod.MakeGenericMethod(entity);
                object obj = method.Invoke(mySql, null);
                method = obj.GetType().GetMethod("Where", new Type[] { typeof(string), typeof(object[]) });
                obj = method.Invoke(obj, new object[] { "1=1", null });
                method = obj.GetType().GetMethod("ExecuteAffrows");
                var count = method.Invoke(obj, null);

                //insert
                method = InsertMehtod.MakeGenericMethod(entity);
                obj = method.Invoke(mySql, null);
                method = obj.GetType().GetMethod("NoneParameter");
                obj = method.Invoke(obj, null);// iinsert<T>()
                var ExecuteAffrows = obj.GetType().GetMethod("ExecuteAffrows");
                //var insert = mySql.Insert<SectionBar>().NoneParameter(); 
                var lt = typeof(IEnumerable<>).MakeGenericType(entity);
                method= obj.GetType().GetMethod("AppendData", new Type[] { lt });
                var select = SelectMehtod.MakeGenericMethod(entity).Invoke(sqlServer, null);
               
                var pageMethod = select.GetType().GetMethod("Page");
                var tolistMethod = select.GetType().GetMethod("ToList", new Type[] { typeof(Boolean) });
                int page = 1;
                while (true)
                {
                    var query = pageMethod.Invoke(select, new object[] { page, 500 });
                    var arr = tolistMethod.Invoke(query, new object[] { false });
                    if (arr is IList && (arr as IList).Count>0)
                    {
                        method.Invoke(obj, new object[] { arr });
                        var res = ExecuteAffrows.Invoke(obj, null);
                        page++;
                    } 
                    else
                    {
                        break;
                    }
                } 
            }



            return await Task.FromResult(true);
        }
    }
}
