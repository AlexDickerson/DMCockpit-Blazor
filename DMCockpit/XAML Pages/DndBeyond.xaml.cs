namespace DMCockpit;

public partial class DndBeyond : ContentPage
{
    public DndBeyond()
    {
        InitializeComponent();
    }

    private void DndBeyondUnloaded(object sender, EventArgs e)
    {
        Environment.SetEnvironmentVariable("DMCOCKPIT_OPEN_DNDBEYOND", "false", EnvironmentVariableTarget.User);
    }
}