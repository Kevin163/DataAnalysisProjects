using GSDebugDataAnalysisWeb.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace GSDebugDataAnalysisWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    public DataTable JobQtysByProductNameTable { get; private set; }
    public void OnGet()
    {
        using var conn = _configuration.GetBIConnection();
        var commandText = @"EVALUATE
SUMMARIZECOLUMNS(
	'产品'[产品名称],
	FILTER(ALL('日期'[Date]),'日期'[Date]>= Date(2020,1,1) && '日期'[Date]<=Date(2020,1,31)),
	""本期新增"",[本期新增问题数],
	""本期发出"",[本期发出问题数]
)";
        JobQtysByProductNameTable = conn.ExecuteDataTable(commandText);
    }
}