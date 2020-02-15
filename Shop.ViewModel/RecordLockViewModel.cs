using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class RecordLockViewModel
    {
        /// <summary>
        /// 资源名称（表名）
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 资源主键
        /// </summary>
        public string KeyId { get; set; }
        /// <summary>
        /// 锁定用户
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 加锁时间
        /// </summary>
        public long LockAt { get; set; }
        /// <summary>
        /// IP 地址
        /// </summary>
        public string IP { get; set; }
        public string hostName { get; set; }

    }
}
