using Shop.IService;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class RecordLockService : IRecordLockService
    {
        private string GetKey(RecordLockViewModel lockViewModel)
        {
            return GetKey(lockViewModel.TableName, lockViewModel.KeyId);
        }
        private string GetKey(string resource, string keyId)
        {
            return $"RecordLock:{resource}:{keyId}";
        }
        public async Task<RecordLockViewModel> Lock(RecordLockViewModel record)
        {
            var key = GetKey(record);
            if (RedisHelper.SetNx(key, record))
            {
                return null;
            }
            else
            {
                record = await RedisHelper.GetAsync<RecordLockViewModel>(key);
                return record;
            }
        }

        public async Task<bool> UnLock(string resource, string keyId)
        {
            return await RedisHelper.DelAsync(GetKey(resource,keyId)) > 0;
        }
    }
}
