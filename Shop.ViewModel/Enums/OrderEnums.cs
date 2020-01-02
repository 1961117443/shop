using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shop.ViewModel.Enums
{
    public enum OrderEnums
    {
        None = 0,

        [Description("未审核")]
        UnAudit =1,

        [Description("已审核")]
        Audit = 2,

        [Description("已审价")]
        Approval = 4,

        [Description("已关闭")]
        Closed = 8,

        [Description("已生产")]
        ProductionEnd = 16,

        [Description("已完结")]
        Finish = 32
    }
}
