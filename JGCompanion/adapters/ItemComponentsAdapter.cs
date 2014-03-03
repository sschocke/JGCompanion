using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;

namespace JGCompanion
{
    public class ItemComponentsAdapter : ArrayAdapter<Component>
    {
        bool displayQty;

        public ItemComponentsAdapter(Context context, List<Component> items, bool displayQty)
            : base(context, R.Layouts.ItemComponentsAdapter_Layout)
        {
            this.displayQty = displayQty;
            this.Update(items);
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                if (displayQty == true)
                {
                    view = View.Inflate(Context, R.Layouts.ItemComponentsAdapter_Layout, null);
                }
                else
                {
                    view = View.Inflate(Context, R.Layouts.ItemComponentsAdapter_NoQty_Layout, null);
                }
            }
            view.FindViewById<TextView>(R.Ids.itemComponentName).Text = item.Name;
            if (displayQty == true)
            {
                view.FindViewById<TextView>(R.Ids.itemComponentQty).Text = item.Qty.ToString("N0");
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
