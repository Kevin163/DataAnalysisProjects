using GSDebugDataAnalysisWeb.Extensions;
using GSDebugDataAnalysisWeb.Modules;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GSDebugDataAnalysisWeb.Pages.Admin.Data;

public class QueryBuilderModel : PageModel
{
    private IConfiguration _configuration;

    public QueryBuilderModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public List<DataModule> DataModules { get; set; }
    public void OnGet()
    {
        using var conn = _configuration.GetBIConnection();
        DataModules = conn.GetDataModules();
    }
}
