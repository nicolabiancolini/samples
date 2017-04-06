/* 
 * HelloWorldLocalized (https://github.com/nicolabiancolini/Samples/tree/master/HelloWorldLocalized)
 * Copyright © 2017 Nicola Biancolini. All rights reserved.
 * Licensed under MIT (https://github.com/nicolabiancolini/Samples/blob/master/LICENSE)
 */

using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Widget;
using Java.Util;

namespace HelloWorldLocalized
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.button);

            button.Click += delegate
            {
                Locale locale;
                Configuration config = new Configuration();

                // Verify the current language of the application.
                if (Resources.Configuration.Locale.Language != Locale.Italian.Language)
                    config.Locale = Locale.Italian;
                else
                    config.Locale = Locale.Default;

                // Set the new configuration and recreate Activity.
                BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
                Recreate();
            };
        }
    }
}

