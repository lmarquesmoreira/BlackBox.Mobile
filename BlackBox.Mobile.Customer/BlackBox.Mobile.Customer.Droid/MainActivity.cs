using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Android.Support.V7.Widget;

namespace BlackBox.Mobile.Customer.Droid
{
    [Activity(Label = "BlackBox.Mobile.Customer", Icon = "@drawable/icon",
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            SetTheme(Resource.Style.UberBlackBoxTheme);
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
    }


    [Activity(Label = "BlackBox.Mobile.Customer", Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher =true,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SlapshActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(3000); //Let's wait awhile...
            this.StartActivity(typeof(MainActivity));
        }

    }

}

