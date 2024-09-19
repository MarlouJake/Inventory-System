using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace InventorySystem.Utilities
{
    public static class ViewRenderer
    {
        public static async Task<string> RenderViewToStringAsync(
        IViewEngine viewEngine,
        ITempDataProvider tempDataProvider,
        IActionContextAccessor actionContextAccessor,
        string viewName,
        object model)
        {
            if (viewEngine == null) throw new ArgumentNullException(nameof(viewEngine));
            if (actionContextAccessor == null) throw new ArgumentNullException(nameof(actionContextAccessor));

            var actionContext = actionContextAccessor.ActionContext;
            if (actionContext == null) throw new ArgumentNullException(nameof(actionContext));

            var viewEngineResult = viewEngine.FindView(actionContext, viewName, false);
            if (!viewEngineResult.Success) throw new InvalidOperationException($"Could not find view '{viewName}'");

            var view = viewEngineResult.View;
            if (view == null) throw new ArgumentNullException(nameof(view));

            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary(new EmptyModelMetadataProvider(), actionContext.ModelState),
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);
                return writer.ToString();
            }
        }
    }
}
