using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PriceTracker.Services;

namespace PriceTracker
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
#if ANDROID13_0_OR_GREATER
        private OnBackInvokedCallback _backInvokedCallback;
#endif
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(Application);

            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);

            var rootView = Window.DecorView.FindViewById(Android.Resource.Id.Content);
            rootView.ViewTreeObserver.GlobalLayout += (sender, args) =>
            {
                Android.Graphics.Rect r = new Android.Graphics.Rect();
                rootView.GetWindowVisibleDisplayFrame(r);

                int screenHeight = rootView.RootView.Height;
                int keypadHeight = screenHeight - r.Bottom;

                bool isKeyboardOpen = keypadHeight > screenHeight * 0.15;

                // Use a instância do serviço via DI
                var keyboardService = MauiApplication.Current.Services.GetService(typeof(KeyboardStateService)) as KeyboardStateService;
                keyboardService?.SetKeyboardVisible(isKeyboardOpen);
            };
#if ANDROID13_0_OR_GREATER
                    // Android 13+ (API 33+): Use OnBackInvokedCallback
                    _backInvokedCallback = new OnBackInvokedCallback(() =>
                    {
                        var service = MauiApplication.Current.Services.GetService<AppSharedState>();
                        service?.RaiseBackButtonPressed();
                    });
                    OnBackInvokedDispatcher.RegisterOnBackInvokedCallback(
                        OnBackInvokedPriority.Default, _backInvokedCallback);
#endif
        }

#if ANDROID13_0_OR_GREATER
                protected override void OnDestroy()
                {
                    if (_backInvokedCallback != null)
                    {
                        OnBackInvokedDispatcher.UnregisterOnBackInvokedCallback(_backInvokedCallback);
                    }
                    base.OnDestroy();
                }
#endif

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            // Mantém compatibilidade com versões antigas
            if (e.KeyCode == Keycode.Back)
            {
                if (e.Action == KeyEventActions.Down)
                {
                    var service = MauiApplication.Current.Services.GetService<AppSharedState>();
#if ANDROID13_0_OR_GREATER
                            service?.RaiseBackButtonPressed();
#else
                    service?.RaiseBackButtonPressedAsync();
#endif
                }
                return true;
            }
            return base.DispatchKeyEvent(e);
        }
    }
}