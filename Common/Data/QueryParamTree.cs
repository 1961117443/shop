using AutoMapper;
using Shop.Common.Extensions;
using Shop.Common.IData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shop.Common.Data
{ 
    public class QueryParamTree : QueryParam, ITree<QueryParamTree>
    {
        private readonly IMapper mapper;

        //public QueryParamTree()
        //{

        //}
        //public QueryParamTree(IMapper mapper) : base(mapper)
        //{
        //    this.mapper = mapper;
        //}
        public QueryParamTree Parent { get; set; }
        public IList<QueryParamTree> Children { get; set; }

        public int Depth
        {
            get
            {
                int dept = 0;
                var tree = this.Parent;
                while (tree != null)
                {
                    dept++;
                    tree = tree.Parent;
                }
                return Parent == null ? 0 : Parent.Depth + 1;
            }
        }

        public bool IsRoot
        {
            get
            {
                return this.Depth == 0;
            }
        }

        public bool IsLeaf
        {
            get
            {
                return !IsRoot;
            }
        }

        public bool HasChildren
        {
            get
            {
                bool flag = false;
                if (Children != null)
                {
                    for (int i = Children.Count - 1; i >= 0; i--)
                    {
                        if (Children[i] == null)
                        {
                            Children.RemoveAt(i);
                        }
                        else
                        {
                            Children[i].Parent = this;
                        }                        
                    }
                    flag = Children.Count > 0;
                    //Children.Where(w => w == null).ToList().ForEach(item =>
                    //{
                    //    Children.Remove()
                    //})
                    //flag = true;
                }
                return flag;
            }
        }

        public string Path
        {
            get
            {
                return string.Empty;
            }
        }

        //public override void SetMapper(IMapper mapper)
        //{
        //    base.SetMapper(mapper);
        //    if (HasChildren)
        //    {
        //        foreach (var item in Children)
        //        {
        //            item.SetMapper(mapper);
        //        }
        //    }
        //}

        public override Expression<Func<TEntity, bool>> ToExpression<TEntity, TView>()
        {
            Expression<Func<TEntity, bool>> where = base.ToExpression<TEntity, TView>();
            if (HasChildren)
            {
                var exp1 = Children[0].ToExpression<TEntity, TView>();
                for (int i = 1; i < Children.Count; i++)
                {
                    var item = Children[i];
                    exp1= item.JoinExpression(exp1, item.ToExpression<TEntity, TView>()); 
                }
                return Children[0].JoinExpression(exp1, where);
            } 
            return where;
        }

        //public Expression<Func<T,bool>> ToExpression<T>()
        //{

        //}
    }

}
