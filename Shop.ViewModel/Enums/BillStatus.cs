using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shop.ViewModel.Enums
{
    public enum DataState
    {
        None = 0,
        Empty = 8388608,
        [Description("浏览")]
        Browse = 1,
        [Description("新增")]
        New = 2,
        [Description("编辑中")]
        Edit = 4,
        [Description("已审核")]
        Check = 8,
        [Description("已生产")]
        ProductionEnd = 16,
        [Description("已完结")]
        Finish = 32,
        [Description("已关闭")]
        Closed = 64,


        [Description("已审价")]
        Approval = 128,

    }
    public enum BillStatus
    {
        None = 0,

        [Description("未审核")]
        UnAudit = 1,

        [Description("已审核")]
        Audit = 2,

        [Description("已审价")]
        Approval = 4,

        [Description("已关闭")]
        Closed = 8,

        [Description("已生产")]
        ProductionEnd = 16,

        [Description("已完结")]
        Finish = 32,
        [Description("编辑中")]
        Edit = 64
    }
}
