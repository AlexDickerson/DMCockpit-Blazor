using DMCockpit_Library.Managers;

namespace DMCockpit.XAML_Pages;

public partial class CampaignViewPage : ContentPage
{
    private readonly ISettingsManager settingsManager;

    public CampaignViewPage(ISettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;

        InitializeComponent();
    }

    public void CampaignViewUnloaded(object sender, EventArgs e)
    {
        settingsManager.SaveWindowLocation("CampaignView", (int)this.Window.X, (int)this.Window.Y);
    }
}