using System;
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreLocation;

#if !MONOMAC
using UIKit;
#endif

namespace NotificationCenter {

	/// <summary>Enumerates values that describe what happened after the application developer attempted to change the state of a widget by using the <see cref="NotificationCenter.NCWidgetProviding.WidgetPerformUpdate(System.Action{NotificationCenter.NCUpdateResult})" /> method.</summary>
	[Deprecated (PlatformName.iOS, 14, 0)]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[Native]
	public enum NCUpdateResult : ulong {
		/// <summary>The widget has new data and may need to update its displayed values, if any.</summary>
		NewData,
		/// <summary>The update succeeded, but no new data resulted.</summary>
		NoData,
		/// <summary>The update failed.</summary>
		Failed,
	}

	/// <summary>Enumerates widget display modes.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 14, 0)]
	[Native]
	public enum NCWidgetDisplayMode : long {
		/// <summary>Indicates that a widget is displayed with compact height.</summary>
		Compact,
		/// <summary>Indicates that a widget is displayed at its full height.</summary>
		Expanded,
	}
}
