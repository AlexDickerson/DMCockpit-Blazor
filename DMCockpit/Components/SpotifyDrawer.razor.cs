using MudBlazor;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Contracts;
using DMCockpit_Library.Services;

namespace DMCockpit.Components
{
    public partial class SpotifyDrawer : ComponentBase
    {
        [Inject]
        public ISettingsManager SettingsManager { get; set; } = default!;

        private Dictionary<string, bool> playlistHidden = [];
        private Dictionary<string, string> playlistDictionary = [];

        protected override void OnInitialized()
        {
            foreach(var playlist in SettingsManager.PlaylistDictionary)
            {
                playlistHidden.Add(playlist.Key, true);
            }
        }

        private void TogglePlayList(string playlistName)
        {
            foreach (var key in playlistHidden.Keys)
            {
                if (key == playlistName)
                {
                    playlistHidden[key] = !playlistHidden[key];
                }
                else
                {
                    playlistHidden[key] = true;
                }
            }
        }

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
