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
    public class PilotListCounterFragment : Fragment
    {
        private string mTitle;
        private PilotListAdapter mPilotList;
        private Activity mParent;

        public PilotListCounterFragment()
            : base()
        {
        }

        public PilotListCounterFragment(string title, PilotListAdapter pilotList, Activity parent)
            : base()
        {
            mTitle = title;
            mPilotList = pilotList;
            mParent = parent;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(R.Layouts.PilotListCounterFragment_Layout, container, false);
            var listview = view.FindViewById<ListView>(R.Ids.pilotListCounterView);
            listview.ItemClick += new EventHandler<ItemClickEventArgs>(listview_ItemClick);
            Refresh(view);
            return view;
        }

        public void Refresh()
        {
            Refresh(this.View);
        }
        private void Refresh(View view)
        {
            var counterTitle = view.FindViewById<TextView>(R.Ids.pilotListCounterTitle);
            counterTitle.Text = mTitle + ":" + mPilotList.Count;

            var listview = view.FindViewById<ListView>(R.Ids.pilotListCounterView);
            listview.Adapter = mPilotList;
        }

        void listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listview = sender as ListView;
            var t = mPilotList.GetItem(e.Position);

            var pilotDetails = new Intent(mParent, typeof(PilotDetailActivity));
            pilotDetails.PutExtra("pilotID", t.PilotID);
            pilotDetails.PutExtra("pilotName", t.PilotName);
            StartActivity(pilotDetails);
        }
    }
}
