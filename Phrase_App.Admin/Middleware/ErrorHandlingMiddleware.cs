using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Phrase_App.Admin.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ErrorHandlingMiddleware> _log;
        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IRazorViewEngine viewEngine,
            ITempDataDictionaryFactory tempDataFactory,
            IServiceProvider serviceProvider,
            ILogger<ErrorHandlingMiddleware> log)
        {
            _next = next;
            _logger = logger;
            _viewEngine = viewEngine;
            _tempDataFactory = tempDataFactory;
            _serviceProvider = serviceProvider;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // If downstream set an error status (e.g. 404), render error view when response not started
                if (!context.Response.HasStarted && context.Response.StatusCode >= 400)
                {
                    var status = context.Response.StatusCode;
                    // Skip if content already present
                    await RenderErrorView(context, status);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    await RenderErrorView(context, 500, ex.Message);
                }
            }
        }

        private async Task RenderErrorView(HttpContext context, int statusCode, string? errorMessage = null)
        {
            // Prepare action context
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData, new ActionDescriptor());

            // Find view
            var viewResult = _viewEngine.FindView(actionContext, "Error", isMainPage: false);
            if (!viewResult.Success)
            {
                // fallback simple HTML
                context.Response.ContentType = "text/html; charset=utf-8";
                var fallback = $"<html><body><h1>Error {statusCode}</h1><p>{System.Text.Encodings.Web.HtmlEncoder.Default.Encode(errorMessage ?? "")}</p></body></html>";
                await context.Response.WriteAsync(fallback);
                return;
            }

            // Prepare view data
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                ["ErrorCode"] = statusCode,
                ["ErrorMessage"] = errorMessage,
                ["RequestId"] = context.TraceIdentifier,
                ["ShowRequestId"] = true
            };

            var tempData = _tempDataFactory.GetTempData(context);

            await using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(sw.ToString());
        }
    }
}