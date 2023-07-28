using GSDebugDataAnalysisWeb.Modules;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;

namespace GSDebugDataAnalysisWeb.Extensions;

public static class AdomdConnectionExtension
{
    /// <summary>
    /// 获取指定连接对象上的表格模型，多维模型暂未考虑，将来需要可再实现
    /// </summary>
    /// <param name="connection">连接实例</param>
    /// <returns>连接上的模型实例列表</returns>
    public static List<DataModule> GetDataModules(this AdomdConnection connection)
    {
        var modules = new List<DataModule>();
        connection.DoActionOnConnection(() =>
        {
            var schemaModuleDataSet = connection.GetSchemaDataSet(AdomdSchemaGuid.TabularModel, null);
            var schemaTablesDataSet = connection.GetSchemaDataSet(AdomdSchemaGuid.TabularTables, null);
            var schemaColumnsDataSet = connection.GetSchemaDataSet(AdomdSchemaGuid.TabularColumns,null);
            var schemaMeasuresDataSet = connection.GetSchemaDataSet(AdomdSchemaGuid.TabularMeasures, null);
            
            modules.AddRange(schemaModuleDataSet.ToDataModules());
            foreach(var module in modules)
            {
                module.FillTablesFromSchemaTablesDataSet(schemaTablesDataSet,schemaColumnsDataSet);
                module.FillMeasuresFromSchemaMeasuresDataSet(schemaMeasuresDataSet);
            }
        });
        return modules;
    }
    /// <summary>
    /// 执行指定的命令，并且将返回结果加载到DataTable中进行返回
    /// </summary>
    /// <param name="connection">连接实例</param>
    /// <param name="commandText">要执行的命令文本</param>
    /// <returns>包含命令执行后的返回数据的DataTable实例</returns>
    public static DataTable ExecuteDataTable(this AdomdConnection connection, string commandText)
    {
        var dt = new DataTable();
        connection.DoActionOnConnection(() =>
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;

            using (var reader = cmd.ExecuteReader())
            {
                //不能直接使用dt.Load方法，因为有些数据可能会返回null，使用Load方法时会抛异常，所以需要手工实现
                var schema = reader.GetSchemaTable();
                foreach (DataRow row in schema.Rows)
                {
                    var dc = new DataColumn(row["ColumnName"].ToString(), Type.GetType(row["DataType"].ToString()));
                    dt.Columns.Add(dc);
                }
                while (reader.Read())
                {
                    var dr = dt.NewRow();
                    foreach (DataRow row in schema.Rows)
                    {
                        var columnName = row["ColumnName"].ToString();
                        dr[columnName] = reader[columnName] ?? DBNull.Value;
                    }
                    dt.Rows.Add(dr);
                }
                reader.Close();
            }
        });

        return dt;
    }
    /// <summary>
    /// 在指定的连接实例上执行动作
    /// 在执行动作前检查是否有打开连接，如果没有则自动打开连接
    /// 在执行动作后，如果连接是由本方法打开的，则自动关闭连接
    /// </summary>
    /// <param name="connection">连接实例</param>
    /// <param name="action">要执行的动作</param>
    private static void DoActionOnConnection(this AdomdConnection connection, Action action)
    {
        var isOpendByMe = false;
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                isOpendByMe = true;
            }
            action();
        }
        finally
        {
            if (isOpendByMe)
            {
                connection.Close();
            }
        }
    }
}