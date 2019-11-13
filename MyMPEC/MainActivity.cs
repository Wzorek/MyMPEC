using System;
using System.Net.Http;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;

namespace MyMPEC
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        TextView test_text;
        TextView header;
        int mode = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
            test_text = FindViewById<TextView>(Resource.Id.test);
            header = FindViewById<TextView>(Resource.Id.head);
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            FindViewById<Button>(Resource.Id.button1).Click += (e, o) =>
            RefreshDataAsync();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.time)
            {
                header.Text = "TIME";
                mode = 0;
            }
            else if (id == Resource.Id.provider)
            {
                header.Text = "PROVIDER";
                mode = 1;
            }
            else if (id == Resource.Id.controler)
            {
                header.Text = "CONTROLER";
                mode = 2;
            }
            else if (id == Resource.Id.exchanger)
            {
                header.Text = "HEAT EXCHANGER";
                mode = 3;
            }
            else if (id == Resource.Id.b_a)
            {
                header.Text = "BUILDING A";
                mode = 4;
            }
            else if (id == Resource.Id.b_n)
            {
                header.Text = "BUILDING B";
                mode = 5;
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public async void RefreshDataAsync()
        {
            var client = new HttpClient();
            test_text.Text = "Wait";
            HttpResponseMessage response = await client.GetAsync("https://anoldlogcabinforsale.szyszki.de/controler/id/1");
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(json);
            test_text.Text = "status wynosi: " + jsonObject["status"] +
                             "incoming_water_temp_Tzco wynosi:|" + jsonObject["incoming_water_temp_Tzco"] +
                             "set_temp_Tzcoref wynosi:|" + jsonObject["set_temp_Tzcoref"] +
                             "valve wynosi:|" + jsonObject["valve"] +
                             "timestamp wynosi:|" + jsonObject["timestamp"];
        }
    }
}