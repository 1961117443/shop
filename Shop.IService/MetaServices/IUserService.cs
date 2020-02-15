using Shop.Entity;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService.MetaServices
{
    public interface IUserService:IBaseService<SysUser>
    {
        bool IsValid(LoginViewModel loginViewModel);
    }
}
