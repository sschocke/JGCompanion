using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.View;
using Android.Os;
using Android.Widget;

namespace JGCompanion
{
    public class ItemStationStockTabFragment : Fragment
    {
        public Item details;

        public ItemStationStockTabFragment(Item details)
        {
            this.details = details;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            if (details == null)
            {
                return inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Blank_Layout, container, false);
            }

            View view = inflater.Inflate(R.Layouts.ItemStationStockTabFragment_Layout, container, false);
            Refresh(view);
            return view;
        }

        private void Refresh(View view)
        {
            var stations = view.FindViewById<ListView>(R.Ids.itemDetailInventoryListView);
            stations.Adapter = new ItemStationStockAdapter(this.Activity, details.StationStocks);
        }
    }
}
