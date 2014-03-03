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
    public class PilotStatsListFragment : Fragment
    {
        private string mTitle;
        private PilotStatsAdapter mStats;
        private Activity mParent;

        public PilotStatsListFragment()
            : base()
        {
        }

        public PilotStatsListFragment(string title, PilotStatsAdapter stats, Activity parent)
            : base()
        {
            mTitle = title;
            mStats = stats;
            mParent = parent;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(R.Layouts.PilotsStatsListFragment_Layout, container, false);
            var textTitle = view.FindViewById<TextView>(R.Ids.pilotStatsTextTitle);
            textTitle.Text = mTitle;

            var listview = view.FindViewById<ListView>(R.Ids.pilotStatsListView);
            listview.Adapter = mStats;
            listview.ItemClick += new EventHandler<ItemClickEventArgs>(listview_ItemClick);

            return view;
        }

        void listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listview = sender as ListView;
            var t = mStats.GetItem(e.Position);

            var pilotDetails = new Intent(mParent, typeof(PilotDetailActivity));
            pilotDetails.PutExtra("pilotID", t.PilotID);
            pilotDetails.PutExtra("pilotName", t.PilotName);
            StartActivity(pilotDetails);
        }
    }
}
