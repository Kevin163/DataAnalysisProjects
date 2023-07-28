using GSDebugDataAnalysisWeb.Extensions;
using System.Data;

namespace GSDebugDataAnalysisWeb.Modules;

/// <summary>
/// 数据模型，对应分析模型部署后的模型名称，一般情况下只会部署一个模型
/// </summary>
public class DataModule
{
    public string ID { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// 模型中拥有的所有物理表格
    /// </summary>
    public List<DataModuleTable> Tables { get; set; } = new List<DataModuleTable>();
    /// <summary>
    /// 表格中拥有的所有度量值列表，单独出来放在特殊的度量值表格中
    /// </summary>
    public List<DataModuleMeasure> Measurements { get; set; } = new List<DataModuleMeasure>();
    /// <summary>
    /// 根据元数据，填充当前模型下的表格列表以及表格里面的表格列列表
    /// </summary>
    /// <param name="schemaTablesDataSet">表格元数据</param>
    /// <param name="schemaColumnsDataSet">列元数据</param>
    public void FillTablesFromSchemaTablesDataSet(DataSet schemaTablesDataSet,DataSet schemaColumnsDataSet)
    {
        foreach(DataTable dt in schemaTablesDataSet.Tables)
        {
            var tablesInCurrentModule = dt.Select($"ModelID='{ID}'");
            foreach(DataRow row in tablesInCurrentModule)
            {
                var table = row.ToDataModuleTable();
                if(table != null)
                {
                    table.FillColumnsFromSchemaColumnsDataSet(schemaColumnsDataSet);
                    Tables.Add(table);
                }
            }
        }
    }
    /// <summary>
    /// 根据指标元数据，填充当前模型下的指标列表
    /// </summary>
    /// <param name="schemaMeasureDataSet">指标元数据</param>
    public void FillMeasuresFromSchemaMeasuresDataSet(DataSet schemaMeasureDataSet)
    {
        Measurements.AddRange(schemaMeasureDataSet.ToDataModuleMeasures());
        ChangeMeasureDisplayFolderIdToName();
    }
    /// <summary>
    /// 将指标的显示名称从TableId修改为对应的名称
    /// </summary>
    private void ChangeMeasureDisplayFolderIdToName()
    {
        foreach(var measure in Measurements)
        {
            var displayFolder = measure.DisplayFolder;
            if (!string.IsNullOrWhiteSpace(displayFolder))
            {
                var table = Tables.FirstOrDefault(w=>w.ID == displayFolder);
                if(table != null)
                {
                    measure.DisplayFolder = table.Name;
                }
            }
        }
    }
}
