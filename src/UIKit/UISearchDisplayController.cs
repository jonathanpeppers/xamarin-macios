#if !TVOS && !__MACCATALYST__ // __TVOS_PROHIBITED
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreLocation;
using MapKit;
using UIKit;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UISearchDisplayController {
		/// <summary>The UITableViewSource holding the search results.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public UITableViewSource SearchResultsSource {
			get {
				var d = SearchResultsWeakDelegate as UITableViewSource;
				if (d is not null)
					return d;
				return null;
			}

			set {
				SearchResultsWeakDelegate = value;
				SearchResultsWeakDataSource = value;
			}
		}
	}
}

#endif // !TVOS && !__MACCATALYST__
