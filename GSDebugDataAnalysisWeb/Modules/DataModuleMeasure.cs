namespace GSDebugDataAnalysisWeb.Modules
{
    /// <summary>
    /// 数据模型度量值
    /// </summary>
    public class DataModuleMeasure
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string UniqueName => $"[{Name}]";
        public string? DisplayFolder { get; set; }
        public DataModuleTableColumnDataType DataType { get; set; }
    }
}
