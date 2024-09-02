using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace InventorySystem.Utilities
{
    public class Helper
    {
        public static string RenderRazorViewToString(Controller controller, string viewName, object? model = null)
        {
            // Set the model for the view
            controller.ViewData.Model = model ?? new object();

            using var sw = new StringWriter();

            // Get the view engine service
            var viewEngine = controller.HttpContext.RequestServices.GetService<ICompositeViewEngine>() ?? throw new InvalidOperationException("View engine not found.");

            // Find the view
            var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, true);
            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"View '{viewName}' not found.");
            }

            // Create the view context
            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()
            );

            try
            {
                // Render the view to the string writer
                viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new InvalidOperationException("An error occurred while rendering the view.", ex);
            }

            // Return the rendered HTML as a string
            return sw.ToString();
        }

    }
}
