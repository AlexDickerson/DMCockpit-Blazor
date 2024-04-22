using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace DMCockpit.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private IJSRuntime Js { get; set; } = null!;

        private bool hideSpotifyCombat = true;
        private bool hideSpotifyDungeon = true;
        private bool hideSpotifyCampfire = true;

        private void ToggleSpotifyCombat()
        {
            hideSpotifyCampfire = true;
            hideSpotifyDungeon = true;
            hideSpotifyCombat = false;
        }
        private void ToggleSpotifyDungeon()
        {
            hideSpotifyCampfire = true;
            hideSpotifyCombat = true;
            hideSpotifyDungeon = false;
        }
        private void ToggleSpotifyCampfire()
        {
            hideSpotifyCombat = true;
            hideSpotifyDungeon = true;
            hideSpotifyCampfire = false;
        }

        bool open;
        void OpenDrawer()
        {
            open = true;
        }
    }
}
