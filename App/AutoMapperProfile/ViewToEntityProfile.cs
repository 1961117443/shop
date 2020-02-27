using AutoMapper;
using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.AutoMapperProfile
{ 
    /// <summary>
    /// 
    /// </summary>
    public class ViewToEntityProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ViewToEntityProfile()
        {
            CreateMap(typeof(QueryParamDto), typeof(QueryParam<,>))
                .AfterMap((view, entity,mapper) =>
                { 
                    var dto = view as QueryParamDto;
                    var types = entity.GetType().GenericTypeArguments;
                    var field = mapper.GetEntityField(dto.Field, types[0], types[1]);
                    if (!field.IsEmpty())
                    {
                        (entity as QueryParam).Field = field;
                    }
                });
        }
    }
}
