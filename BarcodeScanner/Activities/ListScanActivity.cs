using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BarcodeScanner.Activities
{
    [Activity(Label = "ListScanActivity")]
    public class ListScanActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            Toast toast = Toast.MakeText(this, "Ciao sono la nuova activity", ToastLength.Long);
            toast.Show();
        }
    }
}