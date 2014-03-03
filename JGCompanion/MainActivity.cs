using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using Android.Content;
using Android.Preference;
using Android.View;

[assembly: Application("JGCompanion", Icon="drawable/Icon", Label="Jumpgate Companion")]
[assembly: UsesPermission(Android.Manifest.Permission.INTERNET)]
[assembly: UsesPermission(Android.Manifest.Permission.ACCESS_NETWORK_STATE)]
[assembly: Package(VersionName="2.0.0", VersionCode=10)]

namespace JGCompanion
{
    [Activity(Label="Jumpgate Companion", VisibleInLauncher=true)]
    public class MainActivity : Activity, ISharedPreferences_IOnSharedPreferenceChangeListener, IDialogInterface_IOnClickListener
    {
#if DEBUG
        public const string BASE_URL = "http://192.168.1.110/jgcompanion";
#else
        public const string BASE_URL = "http://www.protofactory.co.za/jgcompanion";
#endif
        private static MainActivity main;

        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.MainLayout);
            main = this;

            PreferenceManager.SetDefaultValues(this, R.Xmls.preferences, false);
            ISharedPreferences sharedPrefs = PreferenceManager.GetDefaultSharedPreferences(this);
            sharedPrefs.RegisterOnSharedPreferenceChangeListener(this);

            var btnTopPilots = FindViewById<Button>(R.Ids.btnPilotStats);
            btnTopPilots.Click += delegate
            {
                var topPilotsIntent = new Intent(this, typeof(TopPilotsActivity));
                StartActivity(topPilotsIntent);
            };
            var btnOnlinePilots = FindViewById<Button>(R.Ids.btnOnlinePilots);
            btnOnlinePilots.Click += delegate
            {
                var onlinePilotsIntent = new Intent(this, typeof(OnlinePilotsActivity));
                StartActivity(onlinePilotsIntent);
            };
            var btnStationInventories = FindViewById<Button>(R.Ids.btnStationInventories);
            btnStationInventories.Click += delegate
            {
                var stationInventoriesIntent = new Intent(this, typeof(StationInventoryActivity));
                StartActivity(stationInventoriesIntent);
            };
            var btnItemStocks = FindViewById<Button>(R.Ids.btnItemStocks);
            btnItemStocks.Click += delegate
            {
                var itemStocksIntent = new Intent(this, typeof(ItemStocksActivity));
                StartActivity(itemStocksIntent);
            };

            var btnMyPilotStats = FindViewById<Button>(R.Ids.btnMyPilotStats);
            string pilotName = sharedPrefs.GetString("pref_pilotname", "");
            OnSharedPreferenceChanged(sharedPrefs, "pref_pilotname");
            btnMyPilotStats.Click += delegate
            {
                var pilotDetails = new Intent(this, typeof(PilotDetailActivity));
                string curPilotName = sharedPrefs.GetString("pref_pilotname", "");
                pilotDetails.PutExtra("pilotID", long.MinValue);
                pilotDetails.PutExtra("pilotName", curPilotName);
                StartActivity(pilotDetails);
            };
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            if (key == "pref_pilotname")
            {
                string pilotName = sharedPreferences.GetString("pref_pilotname", "");
                var btnMyPilotStats = FindViewById<Button>(R.Ids.btnMyPilotStats);
                if (pilotName == "")
                {
                    btnMyPilotStats.SetEnabled(false);
                    btnMyPilotStats.Text = "Your Pilot Stats";
                }
                else
                {
                    btnMyPilotStats.SetEnabled(true);
                    btnMyPilotStats.Text = pilotName + " Stats";
                }
            }
        }

        public static void ShowExceptionAlert(Activity source, Exception ex)
        {
            var builder = new AlertDialog.Builder(source);
            AlertDialog alert = builder.Create();
            alert.SetTitle("Exception Caught");
            alert.SetIcon(Android.R.Drawable.Ic_dialog_alert);
            alert.SetButton("OK", main);
            alert.Show();
        }

        public static void setListViewHeightBasedOnChildren(ListView listView)
        {
            IListAdapter listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                // pre-condition
                return;
            }

            int totalHeight = 0;
            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView.Width, View.MeasureSpec.AT_MOST);
            for (int i = 0; i < listAdapter.GetCount(); i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                listItem.Measure(1000, 0);
                totalHeight += listItem.MeasuredHeight;
            }

            ViewGroup.LayoutParams parameters = listView.GetLayoutParams();
            parameters.Height = totalHeight + (listView.DividerHeight * (listAdapter.GetCount() - 1)) + 10;
            listView.SetLayoutParams(parameters);
            listView.RequestLayout();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(R.Menus.MainActivityMenu, menu);
            var settingMenuItem = menu.FindItem(R.Ids.settingMenuItem);
            settingMenuItem.SetIcon(Android.R.Drawable.Ic_menu_preferences);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.GetItemId())
            {
                case R.Ids.settingMenuItem:
                    var settingsIntent = new Intent(this, typeof(MyPreferencesActivity));
                    StartActivity(settingsIntent);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();
        }
    }
}
