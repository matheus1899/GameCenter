using System;
using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Java.Util;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using Android.Util;

namespace SQLITE_CRUD.Droid
{
    [Activity(Label = "Game Center", Icon = "@mipmap/logo", Theme = "@style/MainTheme", MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const BuildVersionCodes m = BuildVersionCodes.M;
        public string TAG { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            IsStoragePermissionGranted();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Window.AddFlags(WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public bool IsStoragePermissionGranted() {
            if (Build.VERSION.SdkInt >= m) {
                if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == (int)Android.Content.PM.Permission.Granted) {
                    Log.Verbose(TAG, "Permission is granted");
                    return true;
                }
                else {
                    Log.Verbose(TAG, "Permission is revoked");
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage }, 1);
                    return false;
                }
            }
            else
            {
                Log.Verbose(TAG, "Permission is granted");
                return true;
            }
        }
    }
}