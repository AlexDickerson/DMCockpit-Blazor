using Microsoft.AspNetCore.Components;

namespace DMCockpit.Components.Layout
{
    public partial class NavMenu : ComponentBase
    {
        bool open = false;

        void ToggleDrawer()
        {
            open = !open;
        }
    }
}
