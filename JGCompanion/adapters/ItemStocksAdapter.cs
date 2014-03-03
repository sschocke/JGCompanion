using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;

namespace JGCompanion
{
    public class ItemStocksAdapter : ArrayAdapter<ItemStocks.StockItem>
    {
        public ItemStocksAdapter(Context context, ItemStocks items)
            : base(context, R.Layouts.ItemStocksAdapter_Layout)
        {
            this.Update(items);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(Context, R.Layouts.ItemStocksAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.itemStockItemName).Text = item.Name;
            view.FindViewById<TextView>(R.Ids.itemStockQty).Text = item.Qty.ToString("N0");

            return view;
        }

        public void Update(ItemStocks items)
        {
            var sortedList = from ItemStocks.StockItem item in items.Items
                             orderby item.Qty
                             select item;

            this.AddAll(sortedList.ToArray());
            this.NotifyDataSetInvalidated();
        }
    }
}
