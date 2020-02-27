using Shop.Common.Data;
using Shop.Common.IData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService.MetaServices
{
    public interface IEntityService
    {
        //IList<QueryParamTree> GetEntity();

        /// <summary>
        /// 获取所有的dto模型
        /// </summary>
        /// <returns></returns>
        IList<string> GetViewModels();
        /// <summary>
        /// 根据dto获取所有的dto字段
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        IList<string> GetQueryFields(string view);
    }
}
