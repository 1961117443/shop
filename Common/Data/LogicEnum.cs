using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Data
{
    public enum LogicEnum
    {
        Equal = 1,
        Like = 2,
        GreaterThan = 4,
        GreaterThanOrEqual = 8,
        LessThan = 16,
        LessThanOrEqual = 32,
        In = 64,
        NotIn = 128,
        /// <summary>
        /// ***%
        /// </summary>
        LikeLeft = 256,
        /// <summary>
        /// %***
        /// </summary>
        LikeRight = 512,
        NoEqual = 1024,
        IsNullOrEmpty = 2048,
        IsNot = 4096,
        NoLike = 8192
    }
}
