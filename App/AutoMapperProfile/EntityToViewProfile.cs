using AutoMapper;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.ViewModel;
using Shop.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.AutoMapperProfile
{
    public class EntityToViewProfile : Profile
    {
        public EntityToViewProfile()
        {
            CreateMap<string, Guid>().ConvertUsing(str => str.ToGuid());

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
                    BillStatus orderEnums = BillStatus.None;
                    if (ts.AuditDate.HasValue)
                    {
                        orderEnums |= BillStatus.Audit;
                    }
                    if (ts.ApprovalDate.HasValue)
                    {
                        orderEnums |= BillStatus.Approval;
                    }
                    if (ts.CloseDate.HasValue)
                    {
                        orderEnums |= BillStatus.Closed;
                    }
                    if (ts.ProductionEndDate.HasValue)
                    {
                        orderEnums |= BillStatus.ProductionEnd;
                    }
                    if (ts.FinishDate.HasValue)
                    {
                        orderEnums |= BillStatus.Finish;
                    }
                    return orderEnums;
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
        }

        /// <summary>
        /// 配置材料仓的实体
        /// </summary>
        private void InitMaterialEntity()
        {
            CreateMap<MaterialPurchase, MaterialPurchaseViewModel>()
                .ForMember(a => a.InStoreDate, b => b.MapFrom(o => o.InStoreDate.HasValue ? o.InStoreDate.Value.ToString("yyyy-MM-dd") : null))
                .ForMember(a => a.MakeDate, b => b.MapFrom(o => o.MakeDate.HasValue ? o.MakeDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null))
                .ForMember(a => a.AuditDate, b => b.MapFrom(o => o.AuditDate.HasValue ? o.AuditDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null))
                .ForMember(a=>a.Status,b=>b.MapFrom((ts, td) =>
                {
                    BillStatus orderEnums = ts.AuditDate.HasValue ? BillStatus.Audit : BillStatus.UnAudit;
                    if (ts.CloseDate.HasValue)
                    {
                        orderEnums |= BillStatus.Closed;
                    }
                    return orderEnums;
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
        }

        private void InitProductEntity()
        {

            CreateMap<Product, ProductQueryViewModel>()
                .ForMember(a => a.ProductCategory_Name, b => b.MapFrom(o => o.ProductCategory.Name));
        }
    }
}
