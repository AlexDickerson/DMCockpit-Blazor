using DMCockpit_Library.Managers;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DMCockpit_Library.Services
{
    public interface IDMCockpitJSInterop
    {
        Task MakeElementDraggable(string className, string boundsElement);
        Task ResizeWithScroll(string elementName, string boundsElement);
        Task CreateDrawableCanvas(string canvasName, string boundsElement, string viewPortElement);
        Task ResizeImage(string imageElement);
        Task<bool[]> GetCanvasBitmap(string canvasName);
        Task<Coordinates[]> GetViewPortPositionOnMap(string viewPortElement, string boundsElement);
    }

    public class DMCockpitJSInterop(IJSRuntime? jsRuntime) : IDMCockpitJSInterop
    {
        private readonly WebView? webView;
        private readonly IJSRuntime? jsRuntime = jsRuntime;

        public DMCockpitJSInterop(WebView webView) : this((IJSRuntime?)null)
        {
            this.webView = webView;
        }

        public async Task MakeElementDraggable(string className, string boundsElement)
        {
            await ExecuteJSFunction("dragAndDrop", ".draggable", "controlMap");
        }

        public async Task ResizeWithScroll(string elementName, string boundsElement)
        {
            await ExecuteJSFunction("resizeWithScroll", elementName, boundsElement);
        }

        public async Task CreateDrawableCanvas(string canvasName, string boundsElement, string viewPortElement)
        {
            await ExecuteJSFunction("drawableMaskCanvas", canvasName, boundsElement, viewPortElement);
        }

        public async Task ResizeImage(string imageElement)
        {
            await ExecuteJSFunction("resizeImage", imageElement);
        }

        public async Task<bool[]> GetCanvasBitmap(string canvasName)
        {
            return await ExecuteJSFunction<bool[]>("getCanvasBitmap", canvasName);
        }

        public async Task<Coordinates[]> GetViewPortPositionOnMap(string viewPortElement, string boundsElement)
        {
            return await ExecuteJSFunction<Coordinates[]>("getPositionByID", viewPortElement, boundsElement);
        }

        private async Task ExecuteJSFunction(string functionName, params object[] args)
        {
            if (jsRuntime != null)
            {
                await jsRuntime.InvokeVoidAsync(functionName, args);
            }
            else if (webView != null)
            {
                await webView.EvaluateJavaScriptAsync(functionName + "(" + args + ")");
            }
            else throw new Exception("No JSRuntime or WebView available");
        }

        private async Task<T> ExecuteJSFunction<T>(string functionName, params object[] args)
        {
            if (jsRuntime != null)
            {
                return await jsRuntime.InvokeAsync<T>(functionName, args);
            }
            else if (webView != null)
            {
                var responseString = await webView.EvaluateJavaScriptAsync(functionName + "(" + args + ")");
                var response = JsonSerializer.Deserialize<T>(responseString);
                if (response != null) return response;
                else throw new Exception("Failed to deserialize response from JS function");
            }
            else throw new Exception("No JSRuntime or WebView available");
        }
    }
}
