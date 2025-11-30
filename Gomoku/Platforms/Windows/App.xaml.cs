using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Gomoku.WinUI;

public partial class App : MauiWinUIApplication
{
    public App() : base()
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}