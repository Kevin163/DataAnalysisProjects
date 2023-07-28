using GSDebugDataAnalysisWeb.Extensions;
using System.Data;

namespace GSDebugDataAnalysisWeb.Modules;

/// <summary>
/// 数据模型下的表格
/// </summary>
public class DataModuleTable
{
    public string ID { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// 表格的列列表
    /// </summary>
    public List<DataModuleTableColumn> Columns { get; set; } = new List<DataModuleTableColumn>();
    public void FillColumnsFromSchemaColumnsDataSet(DataSet schemaColumnsDataSet)
    {
        foreach (DataTable dt in schemaColumnsDataSet.Tables)
        {
            var columnsForCurrentTable = dt.Select($"TableID='{ID}'");
            foreach (DataRow dr in columnsForCurrentTable)
            {
                var column = dr.ToDataModuleTableColumn(this);
                if (column != null) { Columns.Add(column); }
            }
        }
    }
}
