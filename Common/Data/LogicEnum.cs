using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Data
{
    public enum LogicEnum
    {
        Equal = 0,
        Like = 1,
        GreaterThan = 2,
        GreaterThanOrEqual = 3,
        LessThan = 4,
        LessThanOrEqual = 5,
        In = 6,
        NotIn = 7,
        /// <summary>
        /// ***%
        /// </summary>
        LikeLeft = 8,
        /// <summary>
        /// %***
        /// </summary>
        LikeRight = 9,
        NoEqual = 10,
        IsNullOrEmpty = 11,
        IsNot = 12,
        NoLike = 13
    }
}
