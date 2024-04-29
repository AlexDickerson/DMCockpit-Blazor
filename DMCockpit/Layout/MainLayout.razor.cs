using DMCockpit.Components;
using DMCockpit.Services;
using DMCockpit_Library.Managers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DMCockpit.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IHotKeyObersevable HotKeyHandler { get; set; }

        [Inject]
        public ISettingsManager SettingsManager { get; set; }

        SpotifyDrawer spotifyDrawer;
        SettingsDrawer settingsDrawer;

        private Dictionary<string, IFrameDrawer> iframeDrawerElements = [];

        protected override void OnInitialized()
        {
            HotKeyHandler.HotKeyHandlerEvent += HotKeyHandler_HotKeyHandlerEvent;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("JsFunctions.addKeyboardListenerEvent");
        }

        private void OpenSpotifyDrawer()
        {
            spotifyDrawer.OpenDrawer();
        }

        private void OpenIFrameDrawer(string name)
        {
            iframeDrawerElements[name].OpenDrawer();
        }

        private void OpenSettingsDrawer()
        {
            settingsDrawer.OpenDrawer();
        }

        private void HotKeyHandler_HotKeyHandlerEvent(ModifierKeys modifier, string keysPressed)
        {
            if (modifier == ModifierKeys.Ctrl && keysPressed == "s")
            {
                spotifyDrawer.ToggleDrawer();
            }
        }
    }
}