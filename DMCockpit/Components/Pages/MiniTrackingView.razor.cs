using DMCockpit_Library.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCockpit.Components.Pages
{
    public partial class MiniTrackingView : ComponentBase
    {
        [Inject]
        IMiniTracking miniTracking { get; set; } = default!;

        protected override void OnInitialized()
        {
            miniTracking.InitiateTracking();
        }
    }
}
