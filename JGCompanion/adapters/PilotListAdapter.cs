using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.View;

namespace JGCompanion
{
    public class PilotListAdapter : ArrayAdapter<Pilot>
    {
        public PilotListAdapter(Context context)
            : base(context, R.Layouts.PilotListAdapter_Layout)
        { }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(Context, R.Layouts.PilotListAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.pilotName).Text = item.PilotName;
            view.FindViewById<ImageView>(R.Ids.pilotFaction).SetImageLevel((int)item.Faction);

            return view;
        }

        public void Update(IEnumerable<Pilot> pilots)
        {
            this.Clear();
            this.AddAll(pilots.ToArray());
            this.NotifyDataSetChanged();
        }
    }
}
