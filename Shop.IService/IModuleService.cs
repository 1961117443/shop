using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IModuleService
    {
        Task<ModuleConfigs> GetModuleConfigsAsync(string moduleId);

        Task<ForeignTableConfigs> GetModuleForeignTableAsync(string tableName, string fieldName);
    }
}
