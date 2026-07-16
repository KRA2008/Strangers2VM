using Android;
using Android.App;
using Android.App.Roles;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using CommunityToolkit.Mvvm.Messaging;

namespace CallBlock
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public Activity Instance;

        public MainActivity()
        {
            Instance = this;
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            WeakReferenceMessenger.Default.Register<TurnedOnMessage>(this, (recipient, message) =>
            {
                if (message.Value)
                {
                    ActivityCompat.RequestPermissions(Instance, [Manifest.Permission.ReadContacts], 2);

                    var roleManager = GetSystemService(Context.RoleService) as RoleManager;
                    var intent = roleManager.CreateRequestRoleIntent(RoleManager.RoleCallScreening);
                    StartActivityForResult(intent, 1);
                }
            });
        }
    }
}
