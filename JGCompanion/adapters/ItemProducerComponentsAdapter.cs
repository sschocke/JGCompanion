using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;
using Android.Graphics;

namespace JGCompanion
{
    public class ItemProducerComponentsAdapter : ArrayAdapter<Component>
    {
        private List<Component> list;

        public ItemProducerComponentsAdapter(Context context, List<Component> items)
            : base(context, R.Layouts.ItemProducersAdapter_Layout)
        {
            this.Update(items);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(Context, R.Layouts.ItemProducerComponentsAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.itemProducerComponentName).Text = item.Name;
            view.FindViewById<TextView>(R.Ids.itemProducerComponentQty).Text = item.Qty.ToString("N0");

            if (item.Qty == 0)
            {
                view.SetBackgroundColor(Color.Rgb(96, 0, 0));
            }
            else if (item.Qty < 10)
            {
                view.SetBackgroundColor(Color.Rgb(165, 63, 0));
            }
            else if (item.Qty < 50)
            {
                view.SetBackgroundColor(Color.Rgb(104, 87, 0));
            }
            else if (item.Qty < 100)
            {
                view.SetBackgroundColor(Color.Rgb(98, 102, 0));
            }
            else
            {
                view.SetBackgroundColor(Color.Rgb(22, 73, 0));
            }

            return view;
        }

        public void Update(List<Component> items)
        {
            var sortedList = from Component item in items
                             orderby item.Name
                             select item;

            this.AddAll(sortedList.ToArray());
            this.NotifyDataSetInvalidated();
        }
    }
}
