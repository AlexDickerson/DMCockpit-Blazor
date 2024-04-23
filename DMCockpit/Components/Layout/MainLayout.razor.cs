using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace DMCockpit.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        SpotifyDrawer spotifyDrawer;
        AONDrawer aonDrawer;
        NameGeneratorDrawer nameGeneratorDrawer;

        private void OpenSpotifyDrawer()
        {
            spotifyDrawer.OpenDrawer();
        }

        private void OpenAONDrawer()
        {
            aonDrawer.OpenDrawer();
        }

        private void OpenNameGeneratorDrawer()
        {
            nameGeneratorDrawer.OpenDrawer();
        }
    }
}
