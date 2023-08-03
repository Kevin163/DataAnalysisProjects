using GSDebugDataAnalysisWeb.Modules;
using System.Data;

namespace GSDebugDataAnalysisWeb.Extensions
{
    public static class DataSetExtension
    {
        /// <summary>
        /// 将模型的元数据转换为模型列表
        /// </summary>
        /// <param name="schemaModuleDataSet"></param>
        /// <returns></returns>
        public static List<DataModule> ToDataModules(this DataSet schemaModuleDataSet)
        {
            var modules = new List<DataModule>();
            foreach(DataTable dt in schemaModuleDataSet.Tables)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    var module = new DataModule
                    {
                        ID = dr["ID"].ToString(),
                        Name = dr["Name"].ToString()
                    };
                    modules.Add(module);
                }
            }
            return modules;
        }
        /// <summary>
        /// 将表格元数据行转换为表格对象实例
        /// </summary>
        /// <param name="drSchemaTable">表格元数据行实例</param>
        /// <returns>对应的表格实例</returns>
        public static DataModuleTable ToDataModuleTable(this DataRow drSchemaTable)
        {
            return new DataModuleTable
            {
                ID = drSchemaTable["ID"].ToString(),
                Name = drSchemaTable["Name"].ToString(),                
            };
        }
        /// <summary>
        /// 将表格列元数据行转换为表格列对象实例
        /// </summary>
        /// <param name="drSchemaColumn">表格列元数据行实例</param>
        /// <param name="table">当前表格列所属表格实例</param>
        /// <returns>表格列对象实例，如果表格列是特殊的RowNumber-，则返回null</returns>
        public static DataModuleTableColumn? ToDataModuleTableColumn(this DataRow drSchemaColumn,DataModuleTable table)
        {
            var id = drSchemaColumn["ID"].ToString();
            var name = drSchemaColumn["ExplicitName"].ToString();
            var dataType = (DataModuleTableColumnDataType)Convert.ToUInt16(drSchemaColumn["ExplicitDataType"]);
            if (string.IsNullOrWhiteSpace(name)|| name.StartsWith("RowNumber-"))
            {                
                var inferredName = drSchemaColumn["InferredName"].ToString();
                if (string.IsNullOrWhiteSpace(inferredName))
                {
                    return null;
                }
                //当ExplicitName为空时，有一种特殊情况，就是日期表，日期表需要取InferredName,InferredDataType,针对日期表列进行特殊处理
                name = inferredName;
                dataType = (DataModuleTableColumnDataType)Convert.ToUInt16(drSchemaColumn["InferredDataType"]);
            }
            return new DataModuleTableColumn
            {
                ID = id,
                Name = name,
                UniqueName = $"'{table.Name}'.[{name}]",
                DataType = dataType
            };
        }
        /// <summary>
        /// 将度量值元数据转换为度量值列表
        /// </summary>
        /// <param name="schemaMeasureDataSet">度量值元数据</param>
        /// <returns></returns>
        public static List<DataModuleMeasure> ToDataModuleMeasures(this DataSet schemaMeasureDataSet)
        {
            var result = new List<DataModuleMeasure>();
            foreach(DataTable dt in schemaMeasureDataSet.Tables)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    var isHidden = Convert.ToBoolean(dr["IsHidden"]);
                    if (!isHidden)
                    {
                        result.Add(new DataModuleMeasure
                        {
                            ID = dr["ID"].ToString(),
                            Name = dr["Name"].ToString(),
                            DisplayFolder = dr["TableId"].ToString(),
                            DataType = (DataModuleTableColumnDataType)Convert.ToUInt16(dr["DataType"])
                        });
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 将度量值元数据转换为度量值实例
        /// </summary>
        /// <param name="drSchemaMeasure">度量值元数据行</param>
        /// <returns></returns>
        public static DataModuleMeasure ToDataModuleMeasure(this DataRow drSchemaMeasure)
        {
            return new DataModuleMeasure
            {
                ID = drSchemaMeasure["ID"].ToString(),
                Name = drSchemaMeasure["Name"].ToString(),
                DisplayFolder = drSchemaMeasure["TableID"].ToString(),
                DataType = (DataModuleTableColumnDataType)Convert.ToUInt16(drSchemaMeasure["DataType"])
            };
        }
    }
}
