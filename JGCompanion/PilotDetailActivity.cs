using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using System.Net;
using System.Xml.Linq;
using System.Threading;
using Android.Net.Http;
using System.ComponentModel;
using System.IO;
using Java.Io;
using Android.Util;

namespace JGCompanion
{
    [Activity(Label = "Pilot Details", VisibleInLauncher = false)]
    public class PilotDetailActivity : Activity
    {
        PilotDetail _savedInstance;
        ProgressDialog pd;

        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.PilotDetailActivity_Layout);

            var pilotID = Intent.GetLongExtra("pilotID", 0);
            var pilotName = Intent.GetStringExtra("pilotName");

            this.ActionBar.Title = pilotName;

            if (pilotID == 0)
            {
                Toast.MakeText(this, "Pilot ID Error!", Toast.LENGTH_LONG).Show();
                return;
            }

            var pilotDetail = LastNonConfigurationInstance as PilotDetail;
            if ((pilotDetail != null) && (pilotDetail.PilotID == pilotID))
            {
                PopulatePilotDetail(pilotDetail);
                _savedInstance = pilotDetail;
            }
            else
            {
                if (pilotID > 0)
                {
                    RefreshPilotDetail(pilotID);
                }
                else
                {
                    RefreshPilotByName(pilotName);
                }
            }
        }

        void PopulatePilotDetail(PilotDetail pilotDetail)
        {
            if (pd != null)
            {
                pd.Dismiss();
            }

            var txtStatus = FindViewById<TextView>(R.Ids.txtPilotDetailOnlineStatus);
            var txtLastOnline = FindViewById<TextView>(R.Ids.txtPilotDetailLastOnline);
            var txtRank = FindViewById<TextView>(R.Ids.txtPilotDetailRank);
            var txtSquad = FindViewById<TextView>(R.Ids.txtPilotDetailSquad);
            var txtExpToNextLevel = FindViewById<TextView>(R.Ids.txtPilotDetailExpToNextLevel);
            var txtRegistry = FindViewById<TextView>(R.Ids.txtPilotDetailRegistry);
            //var imgRegistry = FindViewById<ImageView> (R.Ids.imgPilotDetailRegistry);
            var txtSolKills = FindViewById<TextView>(R.Ids.txtPilotDetailSolKills);
            var txtSolRating = FindViewById<TextView>(R.Ids.txtPilotDetailSolRating);
            var txtOctKills = FindViewById<TextView>(R.Ids.txtPilotDetailOctKills);
            var txtOctRating = FindViewById<TextView>(R.Ids.txtPilotDetailOctRating);
            var txtQuantKills = FindViewById<TextView>(R.Ids.txtPilotDetailQuantKills);
            var txtQuantRating = FindViewById<TextView>(R.Ids.txtPilotDetailQuantRating);
            var txtAmanRating = FindViewById<TextView>(R.Ids.txtPilotDetailAmanRating);
            var txtHypRating = FindViewById<TextView>(R.Ids.txtPilotDetailHypRating);
            var txtTotalKills = FindViewById<TextView>(R.Ids.txtPilotDetailTotalPilotKills);
            var txtBountyCollected = FindViewById<TextView>(R.Ids.txtPilotDetailBountyCollected);
            var txtBountyPerKill = FindViewById<TextView>(R.Ids.txtPilotDetailBountyPerKill);
            var txtDeaths = FindViewById<TextView>(R.Ids.txtPilotDetailDeaths);
            var txtKillRatio = FindViewById<TextView>(R.Ids.txtPilotDetailKillRatio);
            var txtConfluxKills = FindViewById<TextView>(R.Ids.txtPilotDetailConfluxKills);
            var txtShotsFired = FindViewById<TextView>(R.Ids.txtPilotDetailGunshotsFired);
            var txtShotsHit = FindViewById<TextView>(R.Ids.txtPilotDetailGunshotsHit);
            var txtShotAccuracy = FindViewById<TextView>(R.Ids.txtPilotDetailGunAccuracy);
            var txtMissilesFired = FindViewById<TextView>(R.Ids.txtPilotDetailMissilesFired);
            var txtMissilesHit = FindViewById<TextView>(R.Ids.txtPilotDetailMissilesHit);
            var txtMissileAccuracy = FindViewById<TextView>(R.Ids.txtPilotDetailMissileAccuracy);

            var txtExp = FindViewById<TextView>(R.Ids.txtPilotDetailExperience);
            var txtCredits = FindViewById<TextView>(R.Ids.txtPilotDetailCredits);
            var txtLaunches = FindViewById<TextView>(R.Ids.txtPilotDetailLaunches);
            var txtLandings = FindViewById<TextView>(R.Ids.txtPilotDetailLandings);
            var txtDisconnects = FindViewById<TextView>(R.Ids.txtPilotDetailDisconnects);
            var txtDutyHours = FindViewById<TextView>(R.Ids.txtPilotDetailDutyHours);
            var txtMissionsTaken = FindViewById<TextView>(R.Ids.txtPilotDetailMissionsTaken);
            var txtMissionsCompleted = FindViewById<TextView>(R.Ids.txtPilotDetailMissionsCompleted);
            var txtInsuranceRating = FindViewById<TextView>(R.Ids.txtPilotDetailInsuranceRating);

            if ((pilotDetail.Online == true) && (pilotDetail.Docked == false))
            {
                txtStatus.Text = "IN FLIGHT";
                txtStatus.SetBackgroundResource(R.Drawables.PilotDetailTextViewInFlight);
                txtStatus.SetTextColor(Android.Graphics.Color.BLACK);
            }
            else if (pilotDetail.Online == true)
            {
                txtStatus.Text = "ONLINE";
                txtStatus.SetBackgroundResource(R.Drawables.PilotDetailTextViewOnline);
            }
            else
            {
                txtStatus.Text = "offline";
            }
            if (pilotDetail.LastOnline.Year > 2000)
            {
                txtLastOnline.Text = pilotDetail.LastOnline.ToString("d MMM yyyy HH:mm");
            }
            txtRank.Text = PilotDetail.RankMatrix(pilotDetail.Faction, pilotDetail.Rank);
            txtSquad.Text = pilotDetail.Squad;
            txtRegistry.Text = PilotDetail.RegString(pilotDetail.Registration);
            txtExpToNextLevel.Text = "Next level experience: " + pilotDetail.ExperienceNextRank.ToString("N0");
            //imgRegistry.SetImageLevel ((int)pilotDetail.Registration);
            txtSolKills.Text = pilotDetail.KillsSol.ToString("N0");
            txtSolRating.SetText(PilotDetail.PolRating(pilotDetail.RatingSol));
            txtOctKills.Text = pilotDetail.KillsOct.ToString("N0");
            txtOctRating.SetText(PilotDetail.PolRating(pilotDetail.RatingOct));
            txtQuantKills.Text = pilotDetail.KillsQuant.ToString("N0");
            txtQuantRating.SetText(PilotDetail.PolRating(pilotDetail.RatingQuant));
            txtAmanRating.SetText(PilotDetail.PolRating(pilotDetail.RatingAman));
            txtHypRating.SetText(PilotDetail.PolRating(pilotDetail.RatingHyp));
            txtTotalKills.Text = pilotDetail.TotalPilotKills.ToString("N0");
            txtBountyCollected.Text = "c" + pilotDetail.BountyCollected.ToString("N0");
            txtBountyPerKill.Text = "c" + pilotDetail.BountyPerKill.ToString("N3");
            txtDeaths.Text = pilotDetail.Deaths.ToString("N0");
            txtKillRatio.Text = pilotDetail.KillRatio.ToString("N3") + " %";
            txtConfluxKills.Text = pilotDetail.KillsConflux.ToString("N0");
            txtShotsFired.Text = pilotDetail.ShotsFired.ToString("N0");
            txtShotsHit.Text = pilotDetail.ShotsHit.ToString("N0");
            txtShotAccuracy.Text = pilotDetail.GunAccuracy.ToString("N3") + " %";
            txtMissilesFired.Text = pilotDetail.MissilesFired.ToString("N0");
            txtMissilesHit.Text = pilotDetail.MissilesHit.ToString("N0");
            txtMissileAccuracy.Text = pilotDetail.MissileAccuracy.ToString("N3") + " %";

            txtExp.Text = pilotDetail.Experience.ToString("N0");
            txtCredits.Text = "c" + pilotDetail.Credits.ToString("N0");
            txtLaunches.Text = pilotDetail.Launches.ToString("N0");
            txtLandings.Text = pilotDetail.Landings.ToString("N0");
            txtDisconnects.Text = pilotDetail.Disconnects.ToString("N0");
            txtDutyHours.Text = pilotDetail.DutyHours.ToString("N2");
            txtMissionsTaken.Text = pilotDetail.MissionsFlown.ToString("N0");
            txtMissionsCompleted.Text = pilotDetail.MissionsCompleted.ToString("N0");
            txtInsuranceRating.Text = pilotDetail.InsuranceRating.ToString("N3");

            switch (pilotDetail.Faction)
            {
                case Factions.QUANTAR:
                    this.ActionBar.SetLogo(R.Drawables.QuantarIcon);
                    break;
                case Factions.SOLRAIN:
                    this.ActionBar.SetLogo(R.Drawables.SolrainIcon);
                    break;
                case Factions.OCTAVIUS:
                    this.ActionBar.SetLogo(R.Drawables.OctaviusIcon);
                    break;
            }
            this.ActionBar.Title = pilotDetail.PilotName;
        }

        void RefreshPilotDetail(Int64 pilotID)
        {
            //TODO: Remove workaround for Int64.ToString() problem
            string pilotProfileURL = String.Format("{0}/pilotprofile.php?id={1}", MainActivity.BASE_URL, pilotID.ToString().Replace(",", ""));

            pd = new ProgressDialog(this);
            pd.SetTitle("Loading...");
            pd.SetMessage("Please wait while loading...");
            pd.SetCancelable(false);
            pd.SetIndeterminate(true);
            pd.Show();

            var worker = new BackgroundWorker();
            worker.DoWork += OnGetProfile;
            worker.RunWorkerAsync(pilotProfileURL);
        }
        private void RefreshPilotByName(string pilotName)
        {
            string pilotProfileURL = String.Format("{0}/pilotprofile.php?name={1}", MainActivity.BASE_URL, pilotName);

            pd = new ProgressDialog(this);
            pd.SetTitle("Loading...");
            pd.SetMessage("Please wait while loading...");
            pd.SetCancelable(false);
            pd.SetIndeterminate(true);
            pd.Show();

            var worker = new BackgroundWorker();
            worker.DoWork += OnGetProfile;
            worker.RunWorkerAsync(pilotProfileURL);
        }

        private void OnGetProfile(object sender, DoWorkEventArgs args)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadString(args.Argument.ToString());
            ParsePilotProfile(data);
        }

        void ParsePilotProfile(string httpRes)
        {
            var xml = XDocument.Parse(httpRes);
            {
                XElement root = xml.Root;
                PilotDetail pilotdetail = new PilotDetail(root);
                _savedInstance = pilotdetail;
            }

            RunOnUiThread(() =>
            {
                PopulatePilotDetail(_savedInstance);
            });
        }

        public override object OnRetainNonConfigurationInstance()
        {
            base.OnRetainNonConfigurationInstance();
            return _savedInstance;
        }
    }
}
