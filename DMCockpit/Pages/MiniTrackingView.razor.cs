using DMCockpit_Library;
using Microsoft.AspNetCore.Components;

namespace DMCockpit.Pages
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
