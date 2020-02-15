using Shop.Entity;
using Shop.IService;
using Shop.IService.MetaServices;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service.MetaServices
{
    public class UserService : BaseService<SysUser>, IUserService
    {
        public UserService(IFreeSql<IMetaDatabase> freeSql) : base(freeSql)
        {
        }

        public bool IsValid(LoginViewModel loginViewModel)
        {
            return base.Instance.Select<SysUser>().ToOne(w => w.Code == loginViewModel.Username && w.Password == loginViewModel.Password);
        }
    }
}
