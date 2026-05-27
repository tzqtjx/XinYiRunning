using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace RecordTableApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
                           ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
                           ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        EnableImmersiveMode();
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        base.OnWindowFocusChanged(hasFocus);
        if (hasFocus)
        {
            EnableImmersiveMode();
        }
    }

    private void EnableImmersiveMode()
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            Window.SetDecorFitsSystemWindows(false);
            var controller = Window.InsetsController;
            if (controller != null)
            {
                controller.Hide(WindowInsets.Type.NavigationBars());
                controller.SystemBarsBehavior =
                    (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
            }
        }
        else
        {
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                SystemUiFlags.HideNavigation |
                SystemUiFlags.ImmersiveSticky |
                SystemUiFlags.Fullscreen |
                SystemUiFlags.LayoutStable |
                SystemUiFlags.LayoutHideNavigation |
                SystemUiFlags.LayoutFullscreen);
        }
    }
}