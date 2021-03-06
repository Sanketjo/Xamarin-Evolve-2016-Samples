﻿using System;
using Foundation;
using UIKit;
using CoreLocation;
using System.Linq;

namespace EvolveBeaconSample.iOS
{
    public partial class ViewController : UIViewController
	{
		public ViewController(IntPtr handle) : base(handle)	{}

#region Color from distance
		private UIColor ColorFromDistance(double distance)
		{
			if (distance < 0.0d)
				return UIColor.Gray;
			else if (distance < 1.0d)
				return UIColor.Green;
			else if (distance < 5.0d)
				return UIColor.Orange;
			else
				return UIColor.Red;
		}
#endregion

		private static NSUuid _regionUuid => 
			new NSUuid("8F4010DC-D398-4B52-943A-56B31A91C409");

		private CLLocationManager LocationManager => 
			((AppDelegate)UIApplication.SharedApplication.Delegate).LocationManager;

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			LocationManager.DidRangeBeacons += (s, e) =>
			{
				var first = e.Beacons.FirstOrDefault();
				if (first == null)
				{
					View.BackgroundColor = UIColor.White;
					DistanceLabel.Text = "";
				}
				else if (first.Accuracy >= 0)
				{
					View.BackgroundColor = ColorFromDistance(first.Accuracy);
					DistanceLabel.Text = $"Distance to coffee\n{first.Accuracy:N1}m";
				}
			};

			var region = new CLBeaconRegion(_regionUuid, "EvolveRanging");

			LocationManager.StartRangingBeacons(region);
        }
    }
}

