namespace DMCockpit;

public partial class Spotify : ContentPage
{
	public Spotify()
	{
		InitializeComponent();
	}

	private async void SpotifyUnloaded(object sender, EventArgs e)
    {
		Environment.SetEnvironmentVariable("DMCOCKPIT_OPEN_SPOTIFY", "false", EnvironmentVariableTarget.User);
    }
}