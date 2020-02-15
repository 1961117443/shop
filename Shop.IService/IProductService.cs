﻿using Shop.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService
{
    public interface IProductService: IBaseService<Product>
    {
    }
    public interface IVendorService : IBaseService<Vendor>
    {

    }
}