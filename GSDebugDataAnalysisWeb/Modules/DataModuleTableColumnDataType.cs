namespace GSDebugDataAnalysisWeb.Modules
{
    /// <summary>
    /// sql server analysis里面的tabular表格模型中的表格列数据类型
    /// </summary>
    public enum DataModuleTableColumnDataType: ushort
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String = 2,
        /// <summary>
        /// 整数
        /// </summary>
        Integer = 6,
        /// <summary>
        /// 浮点数
        /// </summary>
        Decimal = 8,
        /// <summary>
        /// 日期类型
        /// </summary>
        DateTime = 9,
        /// <summary>
        /// 布尔类型
        /// </summary>
        Boolean = 11,
    }
}
