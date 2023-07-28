using Microsoft.AnalysisServices.AdomdClient;

namespace GSDebugDataAnalysisWeb.Extensions;

public static class IConfigurationExtension
{
    /// <summary>
    /// 从配置中获取BI连接实例
    /// </summary>
    /// <param name="configuration">配置实例</param>
    /// <param name="connectionName">bi连接字符串名称，默认为BIConnectionString</param>
    /// <returns>对应的bi连接实例</returns>
    public static AdomdConnection GetBIConnection(this IConfiguration configuration, string connectionName = "BIConnectionString")
    {
        var connectionString = configuration.GetConnectionString(connectionName);
        return new AdomdConnection(connectionString);
    }
}
