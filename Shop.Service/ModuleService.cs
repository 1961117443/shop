using Shop.IService;
using Shop.ViewModel;
using Shop.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class ModuleService : IModuleService
    {  
        public async Task<ModuleConfigs> GetModuleConfigsAsync(string moduleId)
        {
            ModuleConfigs configs = new ModuleConfigs()
            {
                MasterTableConfigs = new TableConfigs()
                {
                    TableName = "MaterialPurchase"
                },
                DetailTableConfigs = new GridTableConfigs()
                {
                    TableName = "MaterialPurchaseDetail",
                    Columns = new List<ElementTableColumn>()
                    {
                        new ElementTableColumn(){ field="ProductID_ProductCategory_Name",title="货品类别"},
                        new ElementTableColumn(){ field="ProductID_ProductCode",title="货品编号"},
                        new ElementTableColumn(){ field="ProductID_ProductName",title="货品名称"},
                        new ElementTableColumn(){ field="ProductID_ProductSpec",title="货品规格"},
                        new ElementTableColumn(){ field="Unit",title="单位"},
                        new ElementTableColumn(){ field="MaterialWareHouseID_Name",title="库位"},
                        new ElementTableColumn(){ field="TotalQuantity",title="入库数量" ,align="right"},
                        new ElementTableColumn(){ field="NoTaxPrice",title="未税单价",align="right"},
                        new ElementTableColumn(){ field="NoTaxAmount",title="未税金额",align="right"},
                        new ElementTableColumn(){ field="DTaxRate",title="税率",align="right"},
                        new ElementTableColumn(){ field="AmountTax",title="税额",align="right"},
                        new ElementTableColumn(){ field="TaxPrice",title="含税单价",align="right"},
                        new ElementTableColumn(){ field="TaxAmount",title="含税金额",align="right"},
                        new ElementTableColumn(){ field="BatNo",title="批号"},
                        new ElementTableColumn(){ field="ItRemark",title="明细备注"}
                    }
                },
                Toolbars = new List<Toolbar>()
                {
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="新增",Command = "handleAdd",Icon="el-icon-plus",Status=DataState.Browse},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="保存",Command = "submitForm",Status=DataState.Edit | DataState.New,Icon="el-icon-folder"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="编辑",Command = "handleEdit",Status= DataState.Browse,Icon="el-icon-edit"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="审核",Command = "handleAudit",ButtonType = "success",Status= DataState.Browse,Icon="el-icon-refresh-right"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="反审",Command = "handleUnAudit",ButtonType = "success",Status= DataState.Check,Icon="el-icon-refresh-left"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="添加明细",Command = "handleAppend",Icon="el-icon-document-add",Status=DataState.Edit| DataState.New}
                }
            };
            return await Task.FromResult(configs);
        }

        public async Task<ForeignTableConfigs> GetModuleForeignTableAsync(string tableName, string fieldName)
        {
            var configs = foreignTableConfigs.FirstOrDefault(w => w.TableName == tableName && w.ForeignKey == fieldName);
            return await Task.FromResult(configs);
        }

        static List<ForeignTableConfigs> foreignTableConfigs = new List<ForeignTableConfigs>()
        {
            new ForeignTableConfigs()
            {

                 ForeignKey = "ProductID",
                 TableName = "MaterialPurchaseDetail",
                 Columns = new List<ElementTableColumn>()
                 {
                     new ElementTableColumn(){ field="ProductCode",title="货品编号"},
                     new ElementTableColumn(){ field="ProductName",title="货品名称"},
                     new ElementTableColumn(){ field="ProductSpec",title="规格"},
                     new ElementTableColumn(){ field="Unit",title="基本单位"},
                     new ElementTableColumn(){ field="HelpCode",title="助记码"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/Product"
            },
            new ForeignTableConfigs()
            {

                 ForeignKey = "MaterialWareHouseID",
                 TableName = "MaterialPurchaseDetail",
                 Columns = new List<ElementTableColumn>()
                 {
                     new ElementTableColumn(){ field="Name",title="仓库名称"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/MaterialWarehouse"
            },
            new ForeignTableConfigs()
            {

                 ForeignKey = "VendorID",
                 TableName = "MaterialPurchase",
                 Columns = new List<ElementTableColumn>()
                 {
                     new ElementTableColumn(){ field="Code",title="供应商编号"},
                     new ElementTableColumn(){ field="Name",title="供应商名称"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/Vendor"
            }
        };
    }
}
