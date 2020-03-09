using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class StockOverExcpetion<T> : Exception
    {
        public StockOverExcpetion(IEnumerable<T> data)
        {
            this.OverData = data;
        }
        /// <summary>
        /// 超库存的集合
        /// </summary>
        public IEnumerable<T> OverData { get; }
    }
}
