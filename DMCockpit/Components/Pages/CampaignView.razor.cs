﻿using DMCockpit.Services;
using DMCockpit_Library.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCockpit.Components.Pages
{
    public partial class CampaignView : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; } = default!;

        [Inject]
        private IHTTPInterceptor hTTPInterceptor { get; set; } = default!;

        private string imageBase64 = string.Empty;
        private string rawHTML = string.Empty;

        protected override void OnInitialized()
        {
            DisplayManager.ImageUpdatedEvent += UpdateImage;
            hTTPInterceptor.DNDBeyondUpdateEvent += DNDBeyondUpdate;
        }

        private void UpdateImage(string imageBase64)
        {
            this.imageBase64 = imageBase64;
            StateHasChanged();
        }

        private void DNDBeyondUpdate(string html)
        {
            rawHTML = html;
            StateHasChanged();
        }
    }
}