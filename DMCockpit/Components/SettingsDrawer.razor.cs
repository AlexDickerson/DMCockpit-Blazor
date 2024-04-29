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

        private void OpenDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            DialogService.Show<NewFrame>("New IFrame", options);
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