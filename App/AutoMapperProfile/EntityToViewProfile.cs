using AutoMapper;
using Shop.EntityModel;
using Shop.ViewModel;
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
                .ForMember(a => a.Money, b => b.MapFrom(o => o.Detail == null ? 0 : o.Detail.Sum(w => w.Money)));

            CreateMap<SalesOrderDetail, OrderDetailViewModel>()
                .ForMember(a => a.Weight, m => m.MapFrom(b => b.TheoryWeight))
                .ForMember(a => a.SurfaceName,m=>m.MapFrom(b=>b.Surface.SurfaceName))
                .ForMember(a => a.PackingName, m => m.MapFrom(b => b.Packing.PackingName))
                .ForMember(a => a.ImageUrl, m => m.MapFrom(b => $"http://img.super-s.club/{b.SectionBarID.ToString().Replace("-", "")}"));
        }
    }
}
