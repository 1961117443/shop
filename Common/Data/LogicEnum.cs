using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shop.Common.Data
{
    public enum LogicEnum
    {
        [Description("等于")]
        Equal = 1,
        [Description("包含")]
        Like = 2,
        [Description("大于")]
        GreaterThan = 4,
        [Description("大于等于")]
        GreaterThanOrEqual = 8,
        [Description("小于")]
        LessThan = 16,
        [Description("小于等于")]
        LessThanOrEqual = 32,
        In = 64,
        NotIn = 128,
        /// <summary>
        /// ***%
        /// </summary>
        [Description("左包含")]
        LikeLeft = 256,
        /// <summary>
        /// %***
        /// </summary>
        [Description("右包含")]
        LikeRight = 512,
        [Description("不等于")]
        NoEqual = 1024,
        [Description("等于空")]
        IsNullOrEmpty = 2048,
        IsNot = 4096,
        [Description("不包含")]
        NoLike = 8192
    }

    public enum JoinEnum
    {
        And =0,
        Or = 1
    }
}
