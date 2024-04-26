using DMCockpit_Library.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCockpit.Components
{
    public partial class  SettingsDrawer: ComponentBase
    {
        [Inject]
        public ISettingsManager SettingsManager { get; set; }


    }
}
