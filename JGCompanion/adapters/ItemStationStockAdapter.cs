using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;

namespace JGCompanion
{
    public class ItemStationStockAdapter : ArrayAdapter<ItemStationStock>
    {
        public ItemStationStockAdapter(Context context, List<ItemStationStock> items)
            : base(context, R.Layouts.ItemStationStockAdapter_Layout)
        {
            this.Update(items);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(Context, R.Layouts.ItemStationStockAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.itemStationName).Text = item.Name;
            view.FindViewById<TextView>(R.Ids.itemStationQty).Text = item.Qty.ToString("N0");
            view.FindViewById<TextView>(R.Ids.itemStationPrice).Text = "c" + item.Price.ToString("N0");

            return view;
        }

        public void Update(List<ItemStationStock> items)
        {
            var sortedList = from ItemStationStock item in items
                             orderby item.Name
                             select item;

            this.AddAll(sortedList.ToArray());
            this.NotifyDataSetInvalidated();
        }
    }
}
