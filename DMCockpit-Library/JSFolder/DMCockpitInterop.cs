using DMCockpit.Services;
using Microsoft.JSInterop;

namespace DMCockpit_Library.Javascript_Interop
{
    public interface IDMCockpitInterop
    {
        Task MakeElementDraggable(string className, string boundsElement);
        Task ResizeWithScroll(string elementName, string boundsElement);
        Task CreateDrawableCanvas(string canvasName, string boundsElement, string viewPortElement);
        Task ResizeImage(string imageElement);
        Task<bool[]> GetCanvasBitmap(string canvasName);
        Task<Coordinates[]> GetViewPortPositionOnMap(string viewPortElement, string boundsElement);
    }

    public class DMCockpitInterop(IJSRuntime jsRuntime) : IDMCockpitInterop
    {
        public async Task MakeElementDraggable(string className, string boundsElement)
        {
            string[] args = [".draggable", "controlMap"];
            await jsRuntime.InvokeVoidAsync("dragAndDrop", args);
        }

        public async Task ResizeWithScroll(string elementName, string boundsElement)
        {
            string[] args = [elementName, boundsElement];
            await jsRuntime.InvokeVoidAsync("resizeWithScroll", args);
        }

        public async Task CreateDrawableCanvas(string canvasName, string boundsElement, string viewPortElement)
        {
            string[] args = [canvasName, boundsElement, viewPortElement];
            await jsRuntime.InvokeVoidAsync("drawableMaskCanvas", args);
        }

        public async Task ResizeImage(string imageElement)
        {
            string[] args = [imageElement];
            await jsRuntime.InvokeVoidAsync("resizeImage", args);
        }

        public async Task<bool[]> GetCanvasBitmap(string canvasName)
        {
            return await jsRuntime.InvokeAsync<bool[]>("getCanvasBitmap", canvasName);
        }

        public async Task<Coordinates[]> GetViewPortPositionOnMap(string viewPortElement, string boundsElement)
        {
            string[] args = [viewPortElement, boundsElement];
            return await jsRuntime.InvokeAsync<Coordinates[]>("getPositionByID", args);
        }
    }
}
