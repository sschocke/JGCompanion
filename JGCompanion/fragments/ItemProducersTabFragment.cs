using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.View;
using Android.Os;
using Android.Widget;

namespace JGCompanion
{
    public class ItemProducersTabFragment : Fragment
    {
        public Item details;

        public ItemProducersTabFragment(Item details)
        {
            this.details = details;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            if (details == null)
            {
                return inflater.Inflate(R.Layouts.ItemDetailsTabFragment_Blank_Layout, container, false);
            }

            View view = inflater.Inflate(R.Layouts.ItemProducersTabFragment_Layout, container, false);
            Refresh(view);
            return view;
        }

        private void Refresh(View view)
        {
            var producers = view.FindViewById<ListView>(R.Ids.itemDetailProducersListView);
            producers.Adapter = new ItemProducersAdapter(this.Activity, details.Producers);
        }
    }
}
