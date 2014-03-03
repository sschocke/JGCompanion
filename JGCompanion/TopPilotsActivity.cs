using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;
using Android.View;
using Android.Content;

namespace JGCompanion
{
    [Activity(Label = "Top Pilots", VisibleInLauncher = false)]
    public class TopPilotsActivity : Activity, ActionBar.ITabListener
    {
        PilotStats _savedInstance;
        PilotStatsAdapter confluxKillsAdapter, pilotKillsAdapter;
        PilotStatsAdapter deathsAdapter, creditsAdapter;
        PilotStatsAdapter missionsAdapter, dutyHoursAdapter;
        PilotStatsAdapter experienceAdapter;
        ProgressDialog pd;

        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.TopPilotsActivity_Layout);

            this.ActionBar.NavigationMode = ActionBar.NAVIGATION_MODE_TABS;
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            confluxKillsAdapter = CreateTab("Conflux Kills");
            pilotKillsAdapter = CreateTab("Pilot Kills");
            deathsAdapter = CreateTab("Deaths");
            creditsAdapter = CreateTab("Credits");
            missionsAdapter = CreateTab("Missions Completed");
            dutyHoursAdapter = CreateTab("Duty Hours");
            experienceAdapter = CreateTab("Experience");

            var pilotStats = LastNonConfigurationInstance as PilotStats;
            if (pilotStats != null)
            {
                PopulatePilotStats(pilotStats);
                _savedInstance = pilotStats;
            }
            else
            {
                RefreshPilotStats();
            }
        }

        PilotStatsAdapter CreateTab(string title)
        {
            PilotStatsAdapter adapter = new PilotStatsAdapter(this);
            PilotStatsListFragment fragment = new PilotStatsListFragment(title, adapter, this);

            var tab = this.ActionBar.NewTab();
            tab.SetText(title);
            tab.SetTabListener(this);
            tab.SetTag(fragment);

            this.ActionBar.AddTab(tab);

            return adapter;
        }

        void PopulatePilotStats(PilotStats pilotStats)
        {
            confluxKillsAdapter.Update(pilotStats.FluxKills);
            pilotKillsAdapter.Update(pilotStats.PilotKills);
            deathsAdapter.Update(pilotStats.Deaths);
            creditsAdapter.Update(pilotStats.Credits);
            missionsAdapter.Update(pilotStats.MissionsCompleted);
            dutyHoursAdapter.Update(pilotStats.DutyHours);
            experienceAdapter.Update(pilotStats.Experience);

            if (pd != null)
            {
                pd.Dismiss();
            }
        }

        void RefreshPilotStats()
        {
            string pilotStatsURL = String.Format("{0}/pilotstats.php", MainActivity.BASE_URL);

            pd = new ProgressDialog(this);
            pd.SetTitle("Loading...");
            pd.SetMessage("Please wait while loading...");
            pd.SetCancelable(false);
            pd.SetIndeterminate(true);
            pd.Show();

            var worker = new BackgroundWorker();
            worker.DoWork += OnGetPilotStats;
            worker.RunWorkerAsync(pilotStatsURL);
        }

        private void OnGetPilotStats(object sender, DoWorkEventArgs args)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadString(args.Argument.ToString());
            ParsePilotStats(data);
        }

        void ParsePilotStats(string httpRes)
        {
            var xml = XDocument.Parse(httpRes);
            {
                XElement root = xml.Root;
                PilotStats stats = new PilotStats();
                stats.FluxKills = ParseStatList(root.Element("conflux_leaderboard"));
                stats.PilotKills = ParseStatList(root.Element("pilotskilled_leaderboard"));
                stats.Deaths = ParseStatList(root.Element("deaths_leaderboard"));
                stats.Credits = ParseStatList(root.Element("credits_leaderboard"));
                stats.MissionsCompleted = ParseStatList(root.Element("missionscompleted_leaderboard"));
                stats.DutyHours = ParseStatList(root.Element("dutyhours_leaderboard"));
                stats.Experience = ParseStatList(root.Element("experience_leaderboard"));

                _savedInstance = stats;
            }

            RunOnUiThread(() =>
            {
                PopulatePilotStats(_savedInstance);
            });
        }

        PilotStats.PilotStat[] ParseStatList(XElement xml)
        {
            List<PilotStats.PilotStat> tempList = new List<PilotStats.PilotStat>();
            IEnumerable<XElement> pilots = xml.Elements("pilot");

            foreach (XElement pilot in pilots)
            {
                PilotStats.PilotStat entry = new PilotStats.PilotStat(pilot);
                tempList.Add(entry);
            }

            return tempList.ToArray();
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
        {
            PilotStatsListFragment fragment = tab.Tag as PilotStatsListFragment;
            if (fragment != null)
            {
                ft.Replace(R.Ids.pilotStatsFragmentContainer, fragment);
            }
        }
        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft)
        {}
        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction ft)
        {}

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
