﻿using Shop.IService;
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
            var configs = moduleConfigs.FirstOrDefault(w => w.id == moduleId);
            return await Task.FromResult(configs);
        }

        public async Task<ForeignTableConfigs> GetModuleForeignTableAsync(string tableName, string fieldName)
        {
            var configs = foreignTableConfigs.FirstOrDefault(w => w.TableName == tableName && w.ForeignKey == fieldName);
            return await Task.FromResult(configs);
        }

        static List<ModuleConfigs> moduleConfigs = new List<ModuleConfigs>()
        {
            new ModuleConfigs()
            {
                id="MaterialPurchase",
                MasterTableConfigs = new TableConfigs()
                {
                    TableName = "MaterialPurchase"
                },
                DetailTableConfigs = new GridTableConfigs()
                {
                    TableName = "MaterialPurchaseDetail",
                    Columns = new List<ElTableColumn>()
                    {
                        new ElTableColumn(){ field="ProductID_ProductCategory_Name",title="货品类别"},
                        new ElTableColumn(){ field="ProductID_ProductCode",title="货品编号"},
                        new ElTableColumn(){ field="ProductID_ProductName",title="货品名称"},
                        new ElTableColumn(){ field="ProductID_ProductSpec",title="货品规格"},
                        new ElTableColumn(){ field="Unit",title="单位"},
                        new ElTableColumn(){ field="MaterialWareHouseID_Name",title="库位"},
                        new ElTableColumn(){ field="TotalQuantity",title="入库数量" ,align="right"},
                        new ElTableColumn(){ field="NoTaxPrice",title="未税单价",align="right"},
                        new ElTableColumn(){ field="NoTaxAmount",title="未税金额",align="right"},
                        new ElTableColumn(){ field="DTaxRate",title="税率",align="right"},
                        new ElTableColumn(){ field="AmountTax",title="税额",align="right"},
                        new ElTableColumn(){ field="TaxPrice",title="含税单价",align="right"},
                        new ElTableColumn(){ field="TaxAmount",title="含税金额",align="right"},
                        new ElTableColumn(){ field="BatNo",title="批号"},
                        new ElTableColumn(){ field="ItRemark",title="明细备注"}
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
            },
            new ModuleConfigs()
            {
                id="MaterialSalesOut",
                MasterTableConfigs = new TableConfigs()
                {
                    TableName = "MaterialSalesOut"
                },
                DetailTableConfigs = new GridTableConfigs()
                {
                    TableName = "MaterialSalesOutDetail",
                    Columns = new List<ElTableColumn>()
                    {
                        new ElTableColumn(){ field="ProductID_ProductCategory_Name",title="货品类别"},
                        new ElTableColumn(){ field="ProductID_ProductCode",title="货品编号"},
                        new ElTableColumn(){ field="ProductID_ProductName",title="货品名称"},
                        new ElTableColumn(){ field="ProductID_ProductSpec",title="货品规格"},
                        new ElTableColumn(){ field="Unit",title="单位"},
                        new ElTableColumn(){ field="MaterialWareHouseID_Name",title="库位"},
                        new ElTableColumn(){ field="TotalQuantity",title="入库数量" ,align="right"},
                        new ElTableColumn(){ field="NoTaxPrice",title="未税单价",align="right"},
                        new ElTableColumn(){ field="NoTaxAmount",title="未税金额",align="right"},
                        new ElTableColumn(){ field="DTaxRate",title="税率",align="right"},
                        new ElTableColumn(){ field="AmountTax",title="税额",align="right"},
                        new ElTableColumn(){ field="TaxPrice",title="含税单价",align="right"},
                        new ElTableColumn(){ field="TaxAmount",title="含税金额",align="right"},
                        new ElTableColumn(){ field="BatNo",title="批号"},
                        new ElTableColumn(){ field="ItRemark",title="明细备注"}
                    }
                },
                Toolbars = new List<Toolbar>()
                {
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="新增",Command = "handleAdd",Icon="el-icon-plus",Status=DataState.Browse},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="保存",Command = "submitForm",Status=DataState.Edit | DataState.New,Icon="el-icon-folder"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="编辑",Command = "handleEdit",Status= DataState.Browse,Icon="el-icon-edit"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="审核",Command = "handleAudit",ButtonType = "success",Status= DataState.Browse,Icon="el-icon-refresh-right"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="反审",Command = "handleUnAudit",ButtonType = "success",Status= DataState.Check,Icon="el-icon-refresh-left"},
                    new Toolbar(){ ID=Guid.NewGuid().ToString(),Title="选择库存",Command = "handleSelectStock",Icon="el-icon-document-add",Status=DataState.Edit| DataState.New}
                }
            }
        };

        static List<ForeignTableConfigs> foreignTableConfigs = new List<ForeignTableConfigs>()
        {
            new ForeignTableConfigs()
            {

                 ForeignKey = "ProductID",
                 TableName = "MaterialPurchaseDetail",
                 Columns = new List<ElTableColumn>()
                 {
                     new ElTableColumn(){ field="ProductCode",title="货品编号"},
                     new ElTableColumn(){ field="ProductName",title="货品名称"},
                     new ElTableColumn(){ field="ProductSpec",title="规格"},
                     new ElTableColumn(){ field="Unit",title="基本单位"},
                     new ElTableColumn(){ field="HelpCode",title="助记码"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/Product"
            },
            new ForeignTableConfigs()
            {

                 ForeignKey = "MaterialWareHouseID",
                 TableName = "MaterialPurchaseDetail",
                 Columns = new List<ElTableColumn>()
                 {
                     new ElTableColumn(){ field="Name",title="仓库名称"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/MaterialWarehouse"
            },
            new ForeignTableConfigs()
            {

                 ForeignKey = "VendorID",
                 TableName = "MaterialPurchase",
                 Columns = new List<ElTableColumn>()
                 {
                     new ElTableColumn(){ field="Code",title="供应商编号"},
                     new ElTableColumn(){ field="Name",title="供应商名称"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/Vendor"
            },
            new ForeignTableConfigs()
            {

                 ForeignKey = "CustomerID",
                 TableName = "MaterialSalesOut",
                 Columns = new List<ElTableColumn>()
                 {
                     new ElTableColumn(){ field="Code",title="客户编号"},
                     new ElTableColumn(){ field="Name",title="客户名称"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/Customer"
            },
            new ForeignTableConfigs()
            {

                 ForeignKey = "MaterialDepnameID",
                 TableName = "MaterialUseOutStore",
                 Columns = new List<ElTableColumn>()
                 {
                     new ElTableColumn(){ field="depcode",title="部门编号"},
                     new ElTableColumn(){ field="depname",title="部门名称"}
                 },
                 PrimaryKey = "ID",
                 Url="http://localhost:8090/api/Department"
            }
        };
    }
}
