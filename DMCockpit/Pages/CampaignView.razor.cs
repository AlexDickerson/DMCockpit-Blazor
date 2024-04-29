using DMCockpit_Library.Extensions;
using DMCockpit_Library.Managers;
using DMCockpit_Library.Relays;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace DMCockpit.Pages
{
    public partial class CampaignView : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; } = default!;

        [Inject]
        private DndBeyondBrowserRelay hTTPInterceptor { get; set; } = default!;

        [Inject]
        private IHTTPRelay dMCockpitHTTPListener { get; set; } = default!;

        private string imageBase64 = string.Empty;
        private string imageSrc = string.Empty;
        private string rawHTML = string.Empty;

        private bool showMarkup = false;
        private bool showImage = false;

        protected override void OnInitialized()
        {
            DisplayManager.ImageUpdatedEvent += UpdateImageBase64;
            dMCockpitHTTPListener.MessageRecivedEvent += UpdateImageFromURL;
            hTTPInterceptor.DNDBeyondUpdateEvent += DNDBeyondUpdate;
        }

        private async void UpdateImageBase64(string imageBase64)
        {
            await InvokeAsync(() => {
                this.imageBase64 = imageBase64;
                showImage = true;
                StateHasChanged();
            });
        }

        private async void UpdateImageFromURL(RelayMessage message)
        {
            await InvokeAsync(() =>
            {
                this.imageBase64 = message.Body;
                showImage = true;
                StateHasChanged();
            });
        }

        private void DNDBeyondUpdate(string html)
        {
            rawHTML = html;
            StateHasChanged();
        }
    }
}