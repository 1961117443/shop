using Shop.ViewModel;
using Shop.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

public class MessageResultModel
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    /// 返回消息
    /// </summary>
    public string message { get; set; }
}

public class MessageResultModel<T> : MessageResultModel
{
    public T data { get; set; }
}


/// <summary>
/// ajax 请求的返回数据模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class AjaxResultModel<T> 
{
    /// <summary>
    /// 返回状态
    /// </summary>
    public int code { get; set; } = HttpResponseCode.OK;
    /// <summary>
    /// 返回数据
    /// </summary>
    public virtual T data { get; set; }

    static bool _autoCreateT = typeof(T).GetConstructors().Length > 0;

    public AjaxResultModel()
    {
        this.code = HttpResponseCode.OK;
        
        
    }

    public AjaxResultModel(bool autoCreate):this()
    {
        if (autoCreate && _autoCreateT)
        {
            data = Activator.CreateInstance<T>();
        }
    }

    public AjaxResultModel(T data):this()
    {
        this.data = data;
    }
}

public class AjaxResultModelList<T> : AjaxResultModel<IEnumerable<T>>
{
    public AjaxResultModelList():base()
    {

    }
    public AjaxResultModelList(IEnumerable<T> data) : base(data)
    { 
    }
}

public class AjaxResultPageModel<T> : AjaxResultModel<PageData<T>>
{
    public AjaxResultPageModel():base(true)
    {

    }
}