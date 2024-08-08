using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace InventorySystem.Utilities
{
    public class Helper
    {
        /*
        public static string RenderRazorViewToString(Controller controller, string viewName, object? model = null)
        {

            controller.ViewData.Model = model ?? new object();
            using var sw = new StringWriter();
            if (controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) is not ICompositeViewEngine viewEngine)
            {
                throw new InvalidOperationException("View engine not found.");
            }
            //previously false
            ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, true);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"View '{viewName}' not found.");
            }

            ViewContext viewContext = new(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()

                );

            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();
            return sw.GetStringBuilder().ToString();
        }
        */

        public static string RenderRazorViewToString(Controller controller, string viewName, object? model = null)
        {
            // Ensure the model type matches what the view expects
            controller.ViewData.Model = model ?? new object();
            using var sw = new StringWriter();

            var viewEngine = controller.HttpContext.RequestServices.GetService<ICompositeViewEngine>()
                              ?? throw new InvalidOperationException("View engine not found.");

            var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, true);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"View '{viewName}' not found.");
            }

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()
            );

            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();
            return sw.GetStringBuilder().ToString();
        }


        public static string RenderRazorViewToStringLoginPage(string viewName, object model, HttpContext httpContext)
        {
            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor()
            );

            using var sw = new StringWriter();
            var viewEngine = httpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine
                              ?? throw new InvalidOperationException("View engine not found.");

            var viewResult = viewEngine.FindView(actionContext, viewName, false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"View '{viewName}' not found.");
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var tempDataProvider = httpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider
                                   ?? throw new InvalidOperationException("TempData provider not found.");
            var tempData = new TempDataDictionary(httpContext, tempDataProvider);

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                sw,
                new HtmlHelperOptions()
            );

            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();
            return sw.GetStringBuilder().ToString();
        }



    }
}
