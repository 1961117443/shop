using AutoMapper;
using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.ViewModel;
using Shop.ViewModel.Common;
using Shop.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.AutoMapperProfile
{
    public class EntityToViewProfile : Profile
    {
        private void InitSystemMap()
        {
            CreateMap<string, Guid>().ConvertUsing(str => str.ToGuid());
            CreateMap<string, DateTime?>().ConvertUsing(o => o.IsEmpty() ? new DateTime?() : new DateTime?(DateTime.Parse(o)));
        }
        public EntityToViewProfile()
        {
            InitSystemMap();

            InitMaterialStock();

            CreateMap<BaseMasterEntity, IDataStatusViewModel>()
                .ForMember(a => a.DataStatus, m => m.MapFrom((a, b) =>
                {
                    DataState state = DataState.Empty;
                    if (a.AuditDate.HasValue)
                    {
                        state |= DataState.Check;
                    }
                    return state;
                }));


            CreateMap<Packing, PackingViewModel>()
                    .ForMember(t => t.id, m => m.MapFrom(s => s.AutoID))
                    .ForMember(t => t.name, m => m.MapFrom(s => s.PackingName))
                    .ForMember(t => t.code, m => m.MapFrom(s => s.PackingCode))
                    .ReverseMap();

            CreateMap<SectionBarCategory, SelectItem>()
                .ForMember(a => a.id, b => b.MapFrom(o => o.ID.ToString()))
                .ForMember(a => a.text, b => b.MapFrom(o => !string.IsNullOrEmpty(o.SectionBarCategoryName) && o.SectionBarCategoryName.Length > 4 ? o.SectionBarCategoryName.Substring(0, 4) : o.SectionBarCategoryName));

            CreateMap<SalesOrder, OrderViewModel>()
                .ForMember(a => a.Number, b => b.MapFrom(o => o.Detail == null ? 0 : o.Detail.Sum(w => w.TotalQuantity)))
                .ForMember(a => a.Weight, b => b.MapFrom(o => o.Detail == null ? 0 : o.Detail.Sum(w => w.TheoryWeight)))
                .ForMember(a => a.Money, b => b.MapFrom(o => o.Detail == null ? 0 : o.Detail.Sum(w => w.Money)))
                .ForMember(a => a.OrderStates, b => b.MapFrom((ts, td) =>
                {
                    DataState state = DataState.Empty;
                    if (ts.AuditDate.HasValue)
                    {
                        state |= DataState.Check;
                    }
                    if (ts.ApprovalDate.HasValue)
                    {
                        state |= DataState.Approval;
                    }
                    if (ts.CloseDate.HasValue)
                    {
                        state |= DataState.Closed;
                    }
                    if (ts.ProductionEndDate.HasValue)
                    {
                        state |= DataState.ProductionEnd;
                    }
                    if (ts.FinishDate.HasValue)
                    {
                        state |= DataState.Finish;
                    }
                    return state;
                }));

            CreateMap<SalesOrderDetail, OrderDetailViewModel>()
                .ForMember(a => a.Weight, m => m.MapFrom(b => b.TheoryWeight))
                .ForMember(a => a.TheoryMeter, m => m.MapFrom(b => b.TheoryMeter))
                .ForMember(a => a.SurfaceName, m => m.MapFrom(b => b.Surface.SurfaceName))
                .ForMember(a => a.PackingName, m => m.MapFrom(b => b.Packing.PackingName))
                .ForMember(a => a.TextureName, m => m.MapFrom(b => b.Texture.TextureName))
                .ForMember(a => a.ImageUrl, m => m.MapFrom(b => $"http://img.super-s.club/{b.SectionBar.Code}"))
                .ReverseMap();

            CreateMap<SectionBar, GoodsViewModel>()
                .ForMember(a => a.uuid, m => m.MapFrom(b => b.ID.ToString().ToLower()))
                .ForMember(a => a.title, m => m.MapFrom(b => b.Code))
                .ForMember(a => a.desc, m => m.MapFrom(b => $"名称:{b.Name} 壁厚:{b.WallThickness}mm 米重:{b.Theoreticalweight}kg/m"))
                .ForMember(a => a.imgUrl, m => m.MapFrom(b => $"http://img.super-s.club/{b.Code}"));


            InitMaterialEntity();
            InitBasicDataEntity();

            
            //ForAllMaps((t, map) =>
            //{
            //    map.BeforeMap((s, d) =>
            //    {

            //    })
            //})
            //ForAllPropertyMaps(pm =>
            //{
            //    return true;
            //}, (map, config) =>
            // {
            //     if (map.SourceType.Equal(typeof(string)) && map.DestinationType.Equal(typeof(Guid)))
            //     {
            //         //config.Condition((obj1, obj2, obj3, obj4) =>
            //         //{
            //         //    return true;
            //         //});
            //         config.MapFrom((obj1, obj2, obj3, obj4) =>
            //         {
            //             return obj3;
            //         });
            //     }
            // });
        }
        /// <summary>
        /// 配置基础资料的实体
        /// </summary>
        private void InitBasicDataEntity()
        {
            InitProductEntity();

            CreateMap<MaterialWarehouse, MaterialWarehouseQueryViewModel>().ReverseMap();
            CreateMap<Vendor, VendorQueryViewModel>().ReverseMap();
            CreateMap<MaterialWarehouse, SelectItem>()
                .ForMember(a => a.id, b => b.MapFrom(o => o.ID.ToString()))
                .ForMember(a => a.text, b => b.MapFrom(o => o.Name));

            CreateMap<Customer, CustomerQueryViewModel>().ReverseMap();
            CreateMap<Department, DepartmentQueryViewModel>().ReverseMap();
        }

        /// <summary>
        /// 配置材料仓的实体
        /// </summary>
        private void InitMaterialEntity()
        {
            CreateMap<MaterialPurchase, MaterialPurchaseViewModel>()
                .ForMember(a => a.InStoreDate, b => b.MapFrom(o => o.InStoreDate.ToShortDate()))
                .ForMember(a => a.MakeDate, b => b.MapFrom(o => o.MakeDate.ToLongDate()))
                .ForMember(a => a.AuditDate, b => b.MapFrom(o => o.AuditDate.ToLongDate()))
                .ForMember(a=>a.Status,b=>b.MapFrom((ts, td) =>
                {
                    DataState states = DataState.None;
                    if (ts.AuditDate.HasValue)
                    {
                        states |= DataState.Check;
                    }
                    if (ts.CloseDate.HasValue)
                    {
                        states |= DataState.Closed;
                    }
                    if (states == DataState.None && !ts.ID.IsEmpty())
                    {
                        states |= DataState.Browse;
                    }
                    return states;
                }))
                .ReverseMap()
                .ForMember(a => a.ID, b => b.MapFrom((o, d) =>
                {
                    if (Guid.TryParse(o.ID, out Guid guid))
                    {
                        return guid;
                    }
                    return Guid.Empty;
                }))
                .ForMember(a => a.MakeDate, b => b.MapFrom(o => o.MakeDate.IsEmpty() ? new DateTime?() : new DateTime?(DateTime.Parse(o.MakeDate))))
                .ForMember(a => a.AuditDate, b => b.MapFrom(o => o.AuditDate.IsEmpty() ? new DateTime?() : new DateTime?(DateTime.Parse(o.AuditDate))));


            CreateMap<MaterialPurchaseDetail, MaterialPurchaseDetailViewModel>()
                .ForMember(a => a.ProductID_ProductCategory_Name, m => m.MapFrom(b => b.Product.ProductCategory.Name))
                .ReverseMap();

            CreateMap<MaterialSalesOut, MaterialSalesOutViewModel>()
                .ForMember(a => a.OutStoreDate, m => m.MapFrom(b => b.OutStoreDate.ToShortDate()))
                .ForMember(a => a.MakeDate, b => b.MapFrom(o => o.MakeDate.ToLongDate()))
                .ForMember(a => a.AuditDate, b => b.MapFrom(o => o.AuditDate.ToLongDate()))
                .ForMember(a=>a.CustomerID_Code,m=>m.MapFrom(b=>b.Customer.Code))
                .ForMember(a => a.CustomerID_Name, m => m.MapFrom(b => b.Customer.Name))
                .ReverseMap();

            CreateMap<MaterialSalesOutDetail, MaterialSalesOutDetailViewModel>()
                .ForMember(a => a.MaterialWareHouseID_Name, m => m.MapFrom(b => b.MaterialWarehouse.Name))
                .ForMember(a => a.ProductID_ProductCode, m => m.MapFrom(b => b.Product.ProductCode))
                .ForMember(a => a.ProductID_ProductName, m => m.MapFrom(b => b.Product.ProductName))
                .ForMember(a => a.ProductID_ProductSpec, m => m.MapFrom(b => b.Product.ProductSpec))
                .ForMember(a => a.ProductID_Unit, m => m.MapFrom(b => b.Product.Unit))
                .ForMember(a => a.ProductID_ProductCategoryID_Name, m => m.MapFrom(b => b.Product.ProductCategory.Name))
                .ReverseMap();

            CreateMap<MaterialStock, MaterialStockViewModel>()
                .ForMember(a => a.MaterialWareHouseID_Name, m => m.MapFrom(b => b.MaterialWarehouse.Name))
                .ForMember(a => a.ProductID_ProductCode, m => m.MapFrom(b => b.Product.ProductCode))
                .ForMember(a => a.ProductID_ProductName, m => m.MapFrom(b => b.Product.ProductName))
                .ForMember(a => a.ProductID_ProductSpec, m => m.MapFrom(b => b.Product.ProductSpec))
                .ForMember(a => a.ProductID_Unit, m => m.MapFrom(b => b.Product.Unit))
                .ForMember(a => a.ProductID_ProductCategoryID_Name, m => m.MapFrom(b => b.Product.ProductCategory.Name))
                .ReverseMap()
                //.ForPath(a => a.Product.ProductCategory.Name, m => m.MapFrom(b => b.ProductID_ProductCategoryID_Name))
                .ForPath(a => a.Product.ProductCategoryName, m => m.Ignore());


            CreateMap<MaterialUseOutStore, MaterialUseOutStoreViewModel>()
            .ForMember(a => a.OutStoreDate, m => m.MapFrom(b => b.OutStoreDate.ToShortDate()))
            .ForMember(a => a.MakeDate, b => b.MapFrom(o => o.MakeDate.ToLongDate()))
            .ForMember(a => a.AuditDate, b => b.MapFrom(o => o.AuditDate.ToLongDate()))
            .ForMember(a => a.MaterialDepnameID_depname, m => m.MapFrom(b => b.MaterialDepname.depname))
            .ForMember(a=>a.DataStatus, m=> m.MapFrom((ts, td) =>
            {
                DataState states = DataState.None;
                if (ts.AuditDate.HasValue)
                {
                    states |= DataState.Check;
                }
                if (ts.CloseDate.HasValue)
                {
                    states |= DataState.Closed;
                }
                if (states == DataState.None && !ts.ID.IsEmpty())
                {
                    states |= DataState.Browse;
                }
                return states;
            }))
            .ReverseMap();

            CreateMap<MaterialUseOutStoreDetail, MaterialUseOutStoreDetailViewModel>()
                .ForMember(a => a.MaterialWareHouseID_Name, m => m.MapFrom(b => b.MaterialWarehouse.Name))
                .ForMember(a => a.ProductID_ProductCode, m => m.MapFrom(b => b.Product.ProductCode))
                .ForMember(a => a.ProductID_ProductName, m => m.MapFrom(b => b.Product.ProductName))
                .ForMember(a => a.ProductID_ProductSpec, m => m.MapFrom(b => b.Product.ProductSpec))
                .ForMember(a => a.ProductID_Unit, m => m.MapFrom(b => b.Product.Unit))
                .ForMember(a => a.ProductID_ProductCategoryID_Name, m => m.MapFrom(b => b.Product.ProductCategory.Name))
                .ReverseMap();

            


        }

        /// <summary>
        /// 材料库存相关映射
        /// </summary>
        private void InitMaterialStock()
        {
            CreateMap<MaterialSalesOutDetail, MaterialStock>()
                .ForMember(a => a.Quantity, m => m.MapFrom(b => b.TotalQuantity));
            CreateMap<MaterialUseOutStoreDetail, MaterialStock>()
                .ForMember(a => a.Quantity, m => m.MapFrom(b => b.TotalQuantity));
        }

        private void InitProductEntity()
        {

            CreateMap<Product, ProductViewModel>()
                .ForMember(a => a.ProductCategoryID_Name, b => b.MapFrom(o => o.ProductCategory.Name))
                .ReverseMap();
            CreateMap<ProductCategory, ProductCategoryViewModel>()
                .ReverseMap();
        }
    }
}
