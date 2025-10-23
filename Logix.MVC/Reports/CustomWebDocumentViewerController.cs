using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// We use Swagger, and need to use DevExpress,, but there is an ambiguous between them,
/// So we must override the controllers used by DevExpress and hide these controllers from Swagger.
/// We implement that by using the ApiExplorerSettings attribute.
/// </summary>

[ApiExplorerSettings(IgnoreApi = true)]
[Route("DXXRDV")]
public class CustomWebDocumentViewerController : WebDocumentViewerController
{
    public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService) { }
    public override Task<IActionResult> Invoke()
    {
        return base.Invoke();
    }
}

[ApiExplorerSettings(IgnoreApi = true)]
[Route("DXXQB")]
public class CustomQueryBuilderController : QueryBuilderController
{
    public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService) { }
    public override Task<IActionResult> Invoke()
    {
        return base.Invoke();
    }
}

[ApiExplorerSettings(IgnoreApi = true)]
[Route("DXXRD")]
public class CustomReportDesignerController : ReportDesignerController
{
    public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService) { }
    public override Task<IActionResult> Invoke()
    {
        return base.Invoke();
    }
}