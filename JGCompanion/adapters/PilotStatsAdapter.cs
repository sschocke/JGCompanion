using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.View;
using Android.Content;

namespace JGCompanion
{
    public class PilotStatsAdapter : ArrayAdapter<PilotStats.PilotStat>
    {
        public PilotStatsAdapter(Context context)
            : base(context, R.Layouts.PilotStatsAdapter_Layout)
        { }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(Context, R.Layouts.PilotStatsAdapter_Layout, null);
            }
            view.FindViewById<TextView>(R.Ids.pilotStatName).Text = item.PilotName;
            view.FindViewById<ImageView>(R.Ids.pilotStatFaction).SetImageLevel((int)item.Faction);
            view.FindViewById<TextView>(R.Ids.pilotStatValue).Text = item.Value.ToString("N0");

            return view;
        }

        public void Update(PilotStats.PilotStat[] stats)
        {
            this.Clear();
            this.AddAll(stats);
            this.NotifyDataSetChanged();
        }
    }
}
