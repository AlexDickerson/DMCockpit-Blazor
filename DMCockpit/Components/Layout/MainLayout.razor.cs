using BlazorBootstrap;
using DMCockpit.MAUI_Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace DMCockpit.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private IJSRuntime js { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool hideSpotifyCombat = true;
        private bool hideSpotifyDungeon = true;
        private bool hideSpotifyCampfire = true;

        private Offcanvas offcanvas = default!;

        private async Task OnShowOffcanvasClick() => await offcanvas.ShowAsync();

        private async Task OnHideOffcanvasClick() => await offcanvas.HideAsync();

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
        Anchor anchor;

        void OpenDrawer(Anchor anchor)
        {
            open = true;
            this.anchor = anchor;
        }
    }
}
