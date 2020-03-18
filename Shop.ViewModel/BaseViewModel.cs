using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shop.ViewModel
{
    public interface IDataStatusViewModel
    {
        Enums.DataState DataStatus { get; set; }
    }
    public interface IBaseViewModel
    {
        string ID { get; set; }
    }
    public interface IMasterViewModel: IBaseViewModel
    {
        
        string BillCode { get; set; }
        string Maker { get; set; }
        string MakeDate { get; set; }
        string Audit { get; set; }
        string AuditDate { get; set; }
    }
    
    public class BaseViewModel:IBaseViewModel
    {
        public virtual string ID { get; set; }
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
