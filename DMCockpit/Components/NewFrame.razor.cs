using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DMCockpit.Components
{
    public partial class NewFrame : ComponentBase
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; } = default!;

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}