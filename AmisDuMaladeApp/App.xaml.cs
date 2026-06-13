namespace AmisDuMaladeApp;

public partial class App : Application
{
    private readonly AppShell _shell;

    public App(AppShell shell)
    {
        InitializeComponent();
        _shell = shell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(_shell);
#if WINDOWS
        window.MinimumHeight = 700;
        window.MinimumWidth  = 420;
#endif
        return window;
    }
}
