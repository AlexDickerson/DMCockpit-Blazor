using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DMCockpit.Components
{
    public partial class AONDrawer : ComponentBase
    {
        bool open;
        Anchor anchor;

        public void OpenDrawer()
        {
            open = true;
            this.anchor = Anchor.End;
            StateHasChanged();
        }
    }
}
