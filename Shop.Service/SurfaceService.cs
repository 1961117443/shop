
using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class SurfaceService : BaseService<Surface>, ISurfaceService
    {

        public SurfaceService(IFreeSql freeSql):base(freeSql)
        { 
        }  
    }
}
