using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Os;
using System.Net;
using Android.Util;
using System.ComponentModel;

namespace JGCompanion
{
    public class AutoUpdateDialog : DialogFragment, IDialogInterface_IOnClickListener
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var builder = new AlertDialog.Builder(this.GetActivity());
            builder.SetTitle("Automatic Update");
            builder.SetIcon(Android.R.Drawable.Ic_dialog_alert);
            builder.SetPositiveButton("Update Now", this);
            builder.SetNegativeButton("Cancel", this);
            builder.SetMessage("A newer version is available... Update now?");

            return builder.Create();
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            switch(which)
            {
                case IDialogInterfaceConstants.BUTTON_POSITIVE:
                    doInBackground();
                    dialog.Dismiss();
                    break;
                default:
                    dialog.Dismiss();
                    break;
            }
        }

        protected void doInBackground()
        {
            string apkURL = String.Format("{0}/JGCompanion.apk", MainActivity.BASE_URL);

            var worker = new BackgroundWorker();
            worker.DoWork += OnDownloadUpdate;
            worker.RunWorkerAsync(apkURL);
        }

        private void OnDownloadUpdate(object sender, DoWorkEventArgs args)
        {
            String path = Android.Os.Environment.GetExternalStoragePublicDirectory(Android.Os.Environment.DIRECTORY_DOWNLOADS) + "/JGCompanion.apk";

            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(args.Argument.ToString(), path);

                Intent i = new Intent();
                i.SetAction(Intent.ACTION_VIEW);
                i.SetDataAndType(Android.Net.Uri.FromFile(new Java.Io.File(path)), "application/vnd.android.package-archive");
                Log.D("JGCompanion", "Starting auto-update");
                MainActivity.StartActivityOnMain(i);
            }
            catch (Exception ex)
            {
                Log.E("JGCompanion", "Error during auto-update!");
                Log.E("JGCompanion", ex.Message);
            }
        }
    }
}
