using AutoMapper;
using Shop.Common.Data;
using Shop.Common.Utils;
using Shop.IService.MetaServices;
using Shop.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shop.Service
{
    public class EntityService : IEntityService
    {
        private readonly IMapper mapper;
        Type[] viewTypes = Assembly.Load("Shop.ViewModel")?.GetTypes();

        public EntityService(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IList<string> GetQueryFields(string view)
        {
            IList<string> list = new List<string>();
            if (viewTypes!=null )
            {
                var type = viewTypes.FirstOrDefault(w => w.Name == view);
                if (type==null)
                {
                    var map = mapper.ConfigurationProvider.GetAllTypeMaps().FirstOrDefault(w => w.DestinationType == type);
                    if (map != null)
                    {

                        foreach (var item in map.MemberMaps.OfType<PropertyMap>())
                        {
                            var sourceProperty = (item.SourceMember as PropertyInfo);
                            if (sourceProperty.PropertyType.Equals(typeof(Guid)))
                            {
                                continue;
                            }
                            var ft = sourceProperty.PropertyType.Name.ToString().ToLower();
                            list.Add(item.DestinationName);
                            //list.Add(new QueryField
                            //{
                            //    Field = item.DestinationName,
                            //    Title = item.DestinationMember.Description(),
                            //    FieldType = ft,
                            //    Logics = GetLogicEnum(ft)
                            //});
                        }

                    }

                }
            }
            return list;
        }

        public IList<string> GetViewModels()
        {
            IList<string> list = new List<string>();
            if (viewTypes!=null)
            {
                list = viewTypes.Select(w => w.Name).ToList();
            }
            return list;
        }

        private LogicEnum GetLogicEnum(string fieldType)
        {
            LogicEnum logicEnum = LogicEnum.Equal | LogicEnum.Like | LogicEnum.GreaterThan | LogicEnum.GreaterThanOrEqual
                | LogicEnum.LessThan | LogicEnum.LessThanOrEqual | LogicEnum.LikeLeft
                | LogicEnum.LikeRight | LogicEnum.NoEqual | LogicEnum.IsNullOrEmpty | LogicEnum.NoLike;

            if (fieldType == "datetime" || fieldType == "int")
            {
                logicEnum -= LogicEnum.Like | LogicEnum.LikeLeft | LogicEnum.LikeRight | LogicEnum.IsNullOrEmpty | LogicEnum.NoLike;
            }
            return logicEnum;

        }
    }
}
