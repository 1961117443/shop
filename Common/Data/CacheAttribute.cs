﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Data
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute: Attribute
    {
    }
}
