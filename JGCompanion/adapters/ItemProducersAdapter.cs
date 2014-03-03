using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;

namespace JGCompanion
{
    public class ItemProducersAdapter : ArrayAdapter<ItemProducer>
    {
        public ItemProducersAdapter(Context context, List<ItemProducer> items)
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
                view = View.Inflate(Context, R.Layouts.ItemProducersAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.itemProducerName).Text = item.Name;

            ListView components = view.FindViewById<ListView>(R.Ids.listProducerComponents);
            components.Adapter = new ItemProducerComponentsAdapter(Context, item.Components);
            components.ItemClick += new EventHandler<ItemClickEventArgs>(components_ItemClick);
            MainActivity.setListViewHeightBasedOnChildren(components);

            return view;
        }

        void components_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listview = sender as ListView;
            var adapter = listview.Adapter as ItemProducerComponentsAdapter;
            var t = adapter.GetItem(e.Position);

            var itemDetails = new Intent(this.Context, typeof(ItemDetailActivity));
            itemDetails.PutExtra("itemID", t.ItemID);
            itemDetails.PutExtra("itemName", t.Name);
            this.Context.StartActivity(itemDetails);
        }

        public void Update(List<ItemProducer> items)
        {
            var sortedList = from ItemProducer item in items
                             orderby item.Name
                             select item;

            this.AddAll(sortedList.ToArray());
            this.NotifyDataSetInvalidated();
        }
    }
}
