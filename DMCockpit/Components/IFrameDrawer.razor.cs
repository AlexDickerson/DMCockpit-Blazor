using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DMCockpit.Components
{
    public partial class IFrameDrawer : ComponentBase
    {
        [Parameter]
        public string URL { get; set; } = "https://www.google.com";

        [Parameter]
        public string Title { get; set; } = "IFrame Drawer";

        bool open;
        Anchor anchor;

        public void OpenDrawer()
        {
            open = true;
            this.anchor = Anchor.End;
            StateHasChanged();
        }

        public void ToggleDrawer()
        {
            open = !open;
            this.anchor = Anchor.End;
            StateHasChanged();
        }
    }
}
