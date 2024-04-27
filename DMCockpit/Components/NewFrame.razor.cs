using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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