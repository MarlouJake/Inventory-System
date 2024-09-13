using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace InventorySystem.Utilities
{
    public class Render(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
    {
        private readonly ICompositeViewEngine _viewEngine = viewEngine;
        private readonly ITempDataProvider _tempDataProvider = tempDataProvider;

        public async Task<string> RenderViewAsync<TModel>(ControllerContext controllerContext, string viewName, TModel model, bool partial = false)
        {
            // Initialize ViewDataDictionary without ModelState
            var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), controllerContext.ModelState)
            {
                Model = model
            };

            var tempData = new TempDataDictionary(controllerContext.HttpContext, _tempDataProvider);

            using var writer = new StringWriter();
            var viewResult = _viewEngine.FindView(controllerContext, viewName, !partial);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"View {viewName} was not found.");
            }

            var viewContext = new ViewContext(
                controllerContext,
                viewResult.View,
                viewData,
                tempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}
