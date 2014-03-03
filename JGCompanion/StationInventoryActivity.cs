using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using Android.View;
using Android.Content;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;

namespace JGCompanion
{
	[Activity(Label = "Station Inventories", VisibleInLauncher = false)]
	public class StationInventoryActivity : Activity, ActionBar.ITabListener
	{
		StationInventories _savedInstance;
		ProgressDialog pd;

		protected override void OnCreate(Bundle savedInstance)
		{
			base.OnCreate(savedInstance);
			SetContentView(R.Layouts.StationInventoryActivity_Layout);

			this.ActionBar.NavigationMode = ActionBar.NAVIGATION_MODE_TABS;
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			var stationInventories = LastNonConfigurationInstance as StationInventories;
			if (stationInventories != null)
			{
				PopulateStations(stationInventories);
				_savedInstance = stationInventories;
			}
			else
			{
				RefreshStations();
			}
		}

		void PopulateStations (StationInventories stationInventories)
		{
			foreach (StationInventories.StationInventory station in stationInventories.Stations) {
				this.CreateTab (station);
			}
			if (pd != null) {
				pd.Dismiss();
			}
		}

		private void CreateTab(StationInventories.StationInventory station)
		{
			StationInventoryAdapter adapter = new StationInventoryAdapter(this);
			adapter.Update(station);
			StationInventoryListFragment fragment = new StationInventoryListFragment(adapter, this);

			var tab = this.ActionBar.NewTab();
			tab.SetText(station.Name);
			tab.SetTabListener(this);
			tab.SetTag(fragment);
			switch (station.Faction)
			{
				case Factions.SOLRAIN:
					tab.SetIcon(R.Drawables.solrain);
					break;
				case Factions.QUANTAR:
					tab.SetIcon(R.Drawables.quantar);
					break;
				case Factions.OCTAVIUS:
					tab.SetIcon(R.Drawables.octavius);
					break;
				case Factions.HYPERIAL:
					tab.SetIcon(R.Drawables.hyperial);
					break;
				case Factions.AMANANTH:
					tab.SetIcon(R.Drawables.amananth);
					break;
				case Factions.UNREGULATED:
					tab.SetIcon(R.Drawables.tri);
					break;
			}

			this.ActionBar.AddTab(tab);
		}

		void RefreshStations()
		{
			string stationInventoryURL = String.Format("{0}/stationinventory.php", MainActivity.BASE_URL);

			pd = new ProgressDialog(this);
			pd.SetTitle("Loading...");
			pd.SetMessage("Please wait while loading...");
			pd.SetCancelable(false);
			pd.SetIndeterminate(true);
			pd.Show();

			var worker = new BackgroundWorker();
			worker.DoWork += OnGetStationInventory;
			worker.RunWorkerAsync(stationInventoryURL);
		}

		private void OnGetStationInventory(object sender, DoWorkEventArgs args)
		{
			var webClient = new WebClient();
			var data = webClient.DownloadString(args.Argument.ToString());
			ParseStationInventories(data);
		}

		void ParseStationInventories(string httpRes)
		{
			var xml = XDocument.Parse(httpRes);
			{
				XElement root = xml.Root;
				StationInventories data = new StationInventories(root);
				_savedInstance = data;
			}

			RunOnUiThread(() =>
			{
				PopulateStations(_savedInstance);
			});
		}

		public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
		{
			StationInventoryListFragment fragment = tab.Tag as StationInventoryListFragment;
			if (fragment != null)
			{
				ft.Replace(R.Ids.stationInventoryFragmentContainer, fragment);
			}
		}
		public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft)
		{ }
		public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction ft)
		{ }

		public override object OnRetainNonConfigurationInstance()
		{
			base.OnRetainNonConfigurationInstance();
			return _savedInstance;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.GetItemId())
			{
				case Android.R.Id.Home:
					Intent homeIntent = new Intent(this, typeof(MainActivity));
					StartActivity(homeIntent);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}
}
