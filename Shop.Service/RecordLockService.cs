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
        private readonly CSRedis.CSRedisClient redisClient;

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
            if (!await RedisHelper.ExistsAsync(key) && await RedisHelper.SetAsync(key, record))
            {
                record = null;
            }
            else
            {
                var lockRecord = await RedisHelper.GetAsync<RecordLockViewModel>(key);
                if (record.UserId == lockRecord.UserId)
                {
                    record = null;
                }
                else
                { 
                    record = lockRecord;
                }
            }
            return record;
        }

        public async Task<bool> UnLock(string resource, string keyId)
        {
            return await RedisHelper.DelAsync(GetKey(resource,keyId)) > 0;
        }
    }
}
