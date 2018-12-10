/* 
 * GeoSensor (https://github.com/nicolabiancolini/Samples/tree/master/GeoSensor)
 * Copyright © 2017 Nicola Biancolini. All rights reserved.
 * Licensed under MIT (https://github.com/nicolabiancolini/Samples/blob/master/LICENSE)
 */

using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace GeoSensor
{
    [Activity(Label = "GeoSensor", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        // Define the LocationManager variable.
        private LocationManager manager;

        // Define the UI elements used in the activity.
        private TextView latitudeTextView;
        private TextView longitudeTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            // Recover y the UI elements.
            latitudeTextView = FindViewById<TextView>(Resource.Id.latitudeTextView);
            longitudeTextView = FindViewById<TextView>(Resource.Id.longitudeTextView);
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (manager == null) manager = GetSystemService(LocationService) as LocationManager;

            // Verify if GPS provider is enable.
            if (manager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                // Define the criteria for retrive location from GPS.
                Criteria locationCriteria = new Criteria()
                {
                    Accuracy = Accuracy.Coarse,
                    PowerRequirement = Power.Medium
                };

                // Refresh position.
                manager.RequestSingleUpdate(LocationManager.GpsProvider, this, null);
            }
        }

        public void OnLocationChanged(Location location)
        {
            // Set latitude and longitude on UI.
            latitudeTextView.Text = location.Latitude.ToString();
            longitudeTextView.Text = location.Longitude.ToString();
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}

