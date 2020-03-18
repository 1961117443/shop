using Newtonsoft.Json;
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
    [JsonProperty("success")]
    public bool Success { get; set; }
    /// <summary>
    /// 返回消息
    /// </summary>

    [JsonProperty("message")]
    public string Message { get; set; }
}

public class MessageResultModel<T> : MessageResultModel
{
    [JsonProperty("data")]
    public T Data { get; set; }
}

/// <summary>
/// ajax 请求的返回数据模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class AjaxResultModel
{
    /// <summary>
    /// 返回状态
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; } = HttpResponseCode.OK;
    /// <summary>
    /// 返回消息
    /// </summary>
    [JsonProperty("msg")]
    public string Msg { get; set; }

    public AjaxResultModel():this(HttpResponseCode.OK,string.Empty)
    {
        //this.Code = HttpResponseCode.OK;
        //this.Msg = string.Empty;
    }

    public AjaxResultModel(int code ,string msg)
    {
        this.Code = code;
        this.Msg = msg;
    }
}

/// <summary>
/// ajax 请求的返回数据模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class AjaxResultModel<T> : AjaxResultModel
{
    /// <summary>
    /// 返回数据
    /// </summary>
    [JsonProperty("data")]
    public virtual T Data { get; set; }

    static bool _autoCreateT = typeof(T).GetConstructors().Length > 0;

    public AjaxResultModel() : base()
    {
    }

    public AjaxResultModel(bool autoCreate) : this()
    {
        if (autoCreate && _autoCreateT)
        {
            Data = Activator.CreateInstance<T>();
        }
    }

    public AjaxResultModel(T data) : this()
    {
        this.Data = data;
    }
}

public class AjaxResultModelList<T> : AjaxResultModel<IEnumerable<T>>
{
    public AjaxResultModelList() : base()
    {

    }
    public AjaxResultModelList(IEnumerable<T> data) : base(data)
    {
    }
}

public class AjaxResultPageModel<T> : AjaxResultModel<PageData<T>>
{
    public AjaxResultPageModel() : base(true)
    {

    }
}