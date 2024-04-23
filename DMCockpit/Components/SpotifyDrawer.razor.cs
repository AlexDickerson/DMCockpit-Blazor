using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace DMCockpit.Components
{
    public partial class SpotifyDrawer : ComponentBase
    {
        private Dictionary<string, bool> playlistHidden = [];
        private Dictionary<string, string> playlistDictionary = [];

        private readonly List<string> spotifyEmbedURLs = [
            "https://open.spotify.com/embed/playlist/5OfeOCp481xOMkF4qBAdlW?utm_source=generator",
            "https://open.spotify.com/embed/playlist/3h7mjadevdr4tJFXVvH6Cu?utm_source=generator",
            "https://open.spotify.com/embed/playlist/3nNhKHeLppJl9x7NpGf0l7?utm_source=generator"];

        private readonly List<string> spotifyPlaylistNames = [
            "Combat",
            "Dungeon",
            "Campfire"];

        protected override void OnInitialized()
        {
            for (int i = 0; i < spotifyEmbedURLs.Count; i++)
            {
                playlistDictionary.Add(spotifyPlaylistNames[i], spotifyEmbedURLs[i]);
                playlistHidden.Add(spotifyPlaylistNames[i], true);
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
    }
}
