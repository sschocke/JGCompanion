using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;

namespace JGCompanion
{
    public class StationInventoryAdapter : ArrayAdapter<StationInventories.InventoryItem>, ISectionIndexer
    {
        Dictionary<string, int> alphaIndex;
        String[] sections;
        object[] sectionsObjects;

        public StationInventoryAdapter(Context context)
            : base(context, R.Layouts.StationInventoryAdapter_Layout)
        {
            this.alphaIndex = new Dictionary<string, int>();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(Context, R.Layouts.StationInventoryAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.inventoryItemName).Text = item.Name;
            view.FindViewById<TextView>(R.Ids.inventoryItemQty).Text = item.Qty.ToString("N0");
            view.FindViewById<TextView>(R.Ids.inventoryItemPrice).Text = "c" + item.Price.ToString("N0");

            return view;
        }

        public void Update(StationInventories.StationInventory station)
        {
            var sortedList = from StationInventories.InventoryItem item in station.Inventory
                             orderby item.Name
                             select item;

            StationInventories.InventoryItem[] items = sortedList.ToArray();
            this.Clear();
            this.alphaIndex.Clear();
            this.AddAll(items);

            for (int i = 0; i < items.Length; i++)
            {
                var key = items[i].Name[0].ToString();
                if (!alphaIndex.ContainsKey(key))
                {
                    alphaIndex.Add(key, i);
                }
            }
            var sortedKeys = from string letter in alphaIndex.Keys
                             orderby letter
                             select letter;

            sections = sortedKeys.ToArray();
            sectionsObjects = new object[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                sectionsObjects[i] = new string(sections[i]);
            }

            this.NotifyDataSetChanged();
        }

        public int GetPositionForSection(int section)
        {
            return alphaIndex[sections[section]];
        }

        public int GetSectionForPosition(int position)
        {
            int prevSection = 0;
            for (int i = 0; i < sections.Length; i++)
            {
                if (GetPositionForSection(i) > position && prevSection <= position)
                {
                    prevSection = i; break;
                }
            }
            return prevSection;
        }

        public object[] GetSections()
        {
            return sectionsObjects;
        }
    }
}
