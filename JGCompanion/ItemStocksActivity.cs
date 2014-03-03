using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using Android.View;
using Android.Content;

namespace JGCompanion
{
    [Activity(Label = "Item Stock", VisibleInLauncher = false)]
    public class ItemStocksActivity : Activity
    {
        ItemStocks _savedInstance;
        ProgressDialog pd;

        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.ItemStocksActivity_Layout);

            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            var itemStocks = LastNonConfigurationInstance as ItemStocks;
            if (itemStocks != null)
            {
                PopulateItems(itemStocks);
                _savedInstance = itemStocks;
            }
            else
            {
                RefreshItems();
            }
        }

        void PopulateItems(ItemStocks items)
        {
            var listView = FindViewById<ListView>(R.Ids.itemStocksListView);
            if (listView.Adapter == null)
            {
                listView.Adapter = new ItemStocksAdapter(this, items);
            }
            else
            {
                ((ItemStocksAdapter)listView.Adapter).Update(items);
            }
            listView.ItemClick += new EventHandler<ItemClickEventArgs>(listView_ItemClick);

            if (pd != null)
            {
                pd.Dismiss();
            }
        }

        void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listview = sender as ListView;
            var adapter = listview.Adapter as ItemStocksAdapter;
            var t = adapter.GetItem(e.Position);

            var itemDetails = new Intent(this, typeof(ItemDetailActivity));
            itemDetails.PutExtra("itemID", t.ItemID);
            itemDetails.PutExtra("itemName", t.Name);
            StartActivity(itemDetails);
        }

        void RefreshItems()
        {
            string itemStocksURL = String.Format("{0}/itemstocks.php", MainActivity.BASE_URL);

            pd = new ProgressDialog(this);
            pd.SetTitle("Loading...");
            pd.SetMessage("Please wait while loading...");
            pd.SetCancelable(false);
            pd.SetIndeterminate(true);
            pd.Show();

            var worker = new BackgroundWorker();
            worker.DoWork += OnGetItemStocks;
            worker.RunWorkerAsync(itemStocksURL);
        }

        private void OnGetItemStocks(object sender, DoWorkEventArgs args)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadString(args.Argument.ToString());
            ParseItemStocks(data);
        }

        void ParseItemStocks(string httpRes)
        {
            var xml = XDocument.Parse(httpRes);
            {
                XElement root = xml.Root;
                ItemStocks data = new ItemStocks(root);
                _savedInstance = data;
            }

            RunOnUiThread(() =>
            {
                PopulateItems(_savedInstance);
            });
        }

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
