using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Preference;
using Android.Content;
using Android.Os;

namespace JGCompanion
{
    public class MyPreferencesFragment : PreferenceFragment, ISharedPreferences_IOnSharedPreferenceChangeListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddPreferencesFromResource(R.Xmls.preferences);
            ISharedPreferences sharedPreferences = this.PreferenceManager.SharedPreferences;

            DisplayCurrentPreferenceValues(sharedPreferences);
            sharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            DisplayCurrentPreferenceValues(sharedPreferences);
        }

        void DisplayCurrentPreferenceValues(ISharedPreferences sharedPreferences)
        {
            Preference pilotName = FindPreference("pref_pilotname");
            string curPilotName = sharedPreferences.GetString("pref_pilotname", "");
            if (curPilotName == "")
            {
                curPilotName = "Enter your pilot name";
            }
            pilotName.Summary = curPilotName;
        }
    }
}
