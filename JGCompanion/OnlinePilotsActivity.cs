using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using System.Linq;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;
using Android.View;
using Android.Content;

namespace JGCompanion
{
    [Activity(Label = "Online Pilots", VisibleInLauncher = false)]
    public class OnlinePilotsActivity : Activity, ActionBar.ITabListener
    {
        PilotList _savedInstance;
        PilotListAdapter solrainAdapter, quantarAdapter, octaviusAdapter;
        PilotListCounterFragment currentTab;
        ProgressDialog pd;

        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.OnlinePilotsActivity_Layout);

            this.ActionBar.NavigationMode = ActionBar.NAVIGATION_MODE_TABS;
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            solrainAdapter = CreateTab("Solrain", true);
            quantarAdapter = CreateTab("Quantar", false);
            octaviusAdapter = CreateTab("Octavius", false);

            var pilotList = LastNonConfigurationInstance as PilotList;
            if (pilotList != null)
            {
                PopulatePilotLists(pilotList);
                _savedInstance = pilotList;
            }
            else
            {
                RefreshPilotList();
            }
        }

        PilotListAdapter CreateTab(string title, bool makeCurrent)
        {
            PilotListAdapter adapter = new PilotListAdapter(this);
            PilotListCounterFragment fragment = new PilotListCounterFragment(title + " online", adapter, this);

            var tab = this.ActionBar.NewTab();
            tab.SetText(title);
            tab.SetTabListener(this);
            tab.SetTag(fragment);
            this.ActionBar.AddTab(tab);

            if (makeCurrent == true)
            {
                currentTab = fragment;
            }

            return adapter;
        }

        void PopulatePilotLists(PilotList pilots)
        {
            var solrain = from Pilot p in pilots.List
                          where p.Faction == Factions.SOLRAIN
                          select p;
            var quantar = from Pilot p in pilots.List
                          where p.Faction == Factions.QUANTAR
                          select p;
            var oct = from Pilot p in pilots.List
                      where p.Faction == Factions.OCTAVIUS
                      select p;

            solrainAdapter.Update(solrain);
            quantarAdapter.Update(quantar);
            octaviusAdapter.Update(oct);

            currentTab.Refresh();

            if (pd != null)
            {
                pd.Dismiss();
            }
        }

        void RefreshPilotList()
        {
            string pilotsOnlineURL = String.Format("{0}/onlinepilots.php", MainActivity.BASE_URL);

            pd = new ProgressDialog(this);
            pd.SetTitle("Loading...");
            pd.SetMessage("Please wait while loading...");
            pd.SetCancelable(false);
            pd.SetIndeterminate(true);
            pd.Show();

            var worker = new BackgroundWorker();
            worker.DoWork += OnGetOnlinePilots;
            worker.RunWorkerAsync(pilotsOnlineURL);
        }

        private void OnGetOnlinePilots(object sender, DoWorkEventArgs args)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadString(args.Argument.ToString());
            ParseOnlinePilots(data);
        }

        void ParseOnlinePilots(string httpRes)
        {
            var xml = XDocument.Parse(httpRes);
            {
                XElement root = xml.Root;
                List<Pilot> pilots = new List<Pilot>();
                foreach (XElement pilot in root.Elements("pilot"))
                {
                    Pilot entry = new Pilot(pilot);
                    pilots.Add(entry);
                }
                PilotList list = new PilotList();
                list.List = pilots.ToArray();
                _savedInstance = list;
            }

            RunOnUiThread(() =>
            {
                PopulatePilotLists(_savedInstance);
            });
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
        {
            PilotListCounterFragment fragment = tab.Tag as PilotListCounterFragment;
            if (fragment != null)
            {
                ft.Replace(R.Ids.pilotsOnlineFragmentContainer, fragment);
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
