using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.View;
using Android.Os;
using Android.Widget;
using Android.Content;

namespace JGCompanion
{
    public class StationInventoryListFragment : Fragment
    {
        private StationInventoryAdapter mInventory;
        private Activity mParent;

        public StationInventoryListFragment()
            : base()
        {
        }

        public StationInventoryListFragment(StationInventoryAdapter inventory, Activity parent)
            : base()
        {
            mInventory = inventory;
            mParent = parent;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(R.Layouts.StationInventoryListFragment_Layout, container, false);
            var listview = view.FindViewById<ListView>(R.Ids.stationInventoryListView);
            listview.Adapter = mInventory;
            if ((mInventory != null) && (mInventory.Count > 50))
            {
                listview.SetFastScrollEnabled(true);
            }
            listview.ItemClick += new EventHandler<ItemClickEventArgs>(listview_ItemClick);

            return view;
        }

        void listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listview = sender as ListView;
            var t = mInventory.GetItem(e.Position);

            var itemDetails = new Intent(mParent, typeof(ItemDetailActivity));
            itemDetails.PutExtra("itemID", t.ItemID);
            itemDetails.PutExtra("itemName", t.Name);
            StartActivity(itemDetails);
        }
    }
}
