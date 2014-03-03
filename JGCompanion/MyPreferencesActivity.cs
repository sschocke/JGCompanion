using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;

namespace JGCompanion
{
    [Activity(Label = "MyPreferencesActivity", VisibleInLauncher = false)]
    public class MyPreferencesActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);

            FragmentManager.BeginTransaction().Replace(Android.R.Id.Content, new MyPreferencesFragment()).Commit();
        }
    }
}
