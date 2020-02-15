using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shop.ViewModel
{
    public interface IBaseViewModel
    {

    }
    public class BaseViewModel:IBaseViewModel
    {
         
    }

    public class BaseViewModel<TDetail>:BaseViewModel where TDetail:BaseViewModel
    {
        public IList<TDetail> Detail { get; set; }
    }

    public interface IMasterDetailViewModel<TDetail>: IBaseViewModel where TDetail: IBaseViewModel
    {
        IList<TDetail> Detail { get; set; }
    }

    public interface IMasterDetailViewModel<TMaster,TDetail> : IBaseViewModel where TDetail : IBaseViewModel
    {
        IList<TDetail> Detail { get; set; }
    }
}
