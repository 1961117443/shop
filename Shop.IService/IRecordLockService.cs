using Shop.Entity;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    /// <summary>
    /// 锁定记录服务
    /// </summary>
    public interface IRecordLockService
    {
        /// <summary>
        /// 锁定记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<RecordLockViewModel> Lock(RecordLockViewModel record);

        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<bool> UnLock(string resource,string keyId);
    }
}
