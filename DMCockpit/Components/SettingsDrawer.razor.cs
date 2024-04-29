using DMCockpit_Library.Managers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DMCockpit.Components
{
    public partial class SettingsDrawer : ComponentBase
    {
        [Inject]
        public ISettingsManager SettingsManager { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        public bool Dense_Radio { get; set; } = true;

        public string playListName = string.Empty;
        public string playListEmbedUrl = string.Empty;

        public string iframeName = string.Empty;
        public string iframeUrl = string.Empty;

        public void AddPlayList()
        {
            if (string.IsNullOrEmpty(playListName) || string.IsNullOrEmpty(playListEmbedUrl))
            {
                return;
            }

            SettingsManager.PlaylistDictionary.Add(playListName, playListEmbedUrl);
            playListName = string.Empty;
            playListEmbedUrl = string.Empty;

            SettingsManager.SaveSettings();

            StateHasChanged();
        }

        public void AddIframe()
        {
            if (string.IsNullOrEmpty(iframeName) || string.IsNullOrEmpty(iframeUrl))
            {
                return;
            }

            SettingsManager.IFrames.Add(iframeName, new(iframeUrl, Icons.Material.Filled.Book));
            iframeName = string.Empty;
            iframeUrl = string.Empty;

            SettingsManager.SaveSettings();

            StateHasChanged();
        }

        public void DeletePlaylist(string key)
        {
            SettingsManager.PlaylistDictionary.Remove(key);
            SettingsManager.SaveSettings();
            StateHasChanged();
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