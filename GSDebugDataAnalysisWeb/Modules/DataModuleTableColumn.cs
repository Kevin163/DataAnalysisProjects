namespace GSDebugDataAnalysisWeb.Modules
{
    /// <summary>
    /// 数据模型表格列
    /// </summary>
    public class DataModuleTableColumn
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string UniqueName { get; set; }
        public DataModuleTableColumnDataType DataType { get; set; }
        
    }
}
