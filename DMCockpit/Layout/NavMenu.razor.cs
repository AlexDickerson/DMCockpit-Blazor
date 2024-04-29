using Microsoft.AspNetCore.Components;

namespace DMCockpit.Layout
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
