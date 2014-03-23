using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using Android.Graphics.Drawable;

namespace JGCompanion
{
    [Activity(Label = "Item Details", VisibleInLauncher = false)]
    public class ItemDetailActivity : Activity, ActionBar.ITabListener
    {
        Item _savedInstance;
        ProgressDialog pd;
        ActionBar.Tab detailsTab;
        ActionBar.Tab stockTab;
        ActionBar.Tab producersTab;
        ItemDetailsTabFragment detailsFragment;
        ItemStationStockTabFragment stocksFragment;
        ItemProducersTabFragment producersFragment;

        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.ItemDetailActivity_Layout);

            var itemID = Intent.GetLongExtra("itemID", 0);
            var itemName = Intent.GetStringExtra("itemName");

            this.ActionBar.SetTitle(itemName);
            this.ActionBar.NavigationMode = ActionBar.NAVIGATION_MODE_TABS;

            detailsFragment = new ItemDetailsTabFragment(null);
            detailsTab = this.ActionBar.NewTab();
            detailsTab.SetText("Details");
            detailsTab.SetTabListener(this);
            detailsTab.SetTag(detailsFragment);
            this.ActionBar.AddTab(detailsTab);

            stocksFragment = new ItemStationStockTabFragment(null);
            stockTab = this.ActionBar.NewTab();
            stockTab.SetText("Stock");
            stockTab.SetTabListener(this);
            stockTab.SetTag(stocksFragment);
            this.ActionBar.AddTab(stockTab);

            producersFragment = new ItemProducersTabFragment(null);
            producersTab = this.ActionBar.NewTab();
            producersTab.SetText("Production");
            producersTab.SetTabListener(this);
            producersTab.SetTag(producersFragment);
            this.ActionBar.AddTab(producersTab);

            if (itemID == 0)
            {
                Android.Widget.Toast.MakeText(this, "Item ID Error!", Android.Widget.Toast.LENGTH_LONG).Show();
            }

            var itemDetail = LastNonConfigurationInstance as Item;
            if ((itemDetail != null) && (itemDetail.ItemID == itemID))
            {
                _savedInstance = itemDetail;
                PopulateItemDetail(itemDetail);
            }
            else
            {
                RefreshItemDetail(itemID);
            }
        }

        void PopulateItemDetail(Item itemDetail)
        {
            int iconid = Item.GetIconId(itemDetail.ResourceName, this);
            if (iconid > 0)
            {
                this.ActionBar.SetIcon(iconid);
            }
            detailsFragment = new ItemDetailsTabFragment(itemDetail);
            detailsTab.SetTag(detailsFragment);
            var transaction = this.FragmentManager.BeginTransaction();
            transaction.Replace(R.Ids.itemDetailFragmentContainer, detailsFragment);
            transaction.Commit();
            stocksFragment = new ItemStationStockTabFragment(itemDetail);
            stockTab.SetTag(stocksFragment);
            if (itemDetail.Producers.Count > 0)
            {
                producersFragment = new ItemProducersTabFragment(itemDetail);
                producersTab.SetTag(producersFragment);
            }
            else
            {
                this.ActionBar.RemoveTab(producersTab);
            }
            this.ActionBar.Title = itemDetail.Name;
            if (pd != null)
            {
                pd.Dismiss();
            }
        }

        void RefreshItemDetail(Int64 itemID)
        {
            string itemDetailURL = String.Format("{0}/itemdetail.php?id={1}", MainActivity.BASE_URL, itemID.ToString().Replace(",", ""));

            pd = new ProgressDialog(this);
            pd.SetTitle("Loading...");
            pd.SetMessage("Please wait while loading...");
            pd.SetCancelable(false);
            pd.SetIndeterminate(true);
            pd.Show();

            var worker = new BackgroundWorker();
            worker.DoWork += OnGetItemDetail;
            worker.RunWorkerAsync(itemDetailURL);
        }

        private void OnGetItemDetail(object sender, DoWorkEventArgs args)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadString(args.Argument.ToString());
            ParseItemDetail(data);
        }

        void ParseItemDetail(string httpRes)
        {
            var xml = XDocument.Parse(httpRes);
            {
                XElement root = xml.Root;
                XElement detail = root.Element("detail");
                Item itemDetail = Item.FromDetailXML(detail);
                _savedInstance = itemDetail;
            }

            RunOnUiThread(() =>
            {
                PopulateItemDetail(_savedInstance);
            });
        }

        public override object OnRetainNonConfigurationInstance()
        {
            base.OnRetainNonConfigurationInstance();
            return _savedInstance;
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
        {
            Fragment fragment = tab.Tag as Fragment;
            if (fragment != null)
            {
                ft.Replace(R.Ids.itemDetailFragmentContainer, fragment);
            }
        }
        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft)
        { }
        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction ft)
        { }
    }
}
