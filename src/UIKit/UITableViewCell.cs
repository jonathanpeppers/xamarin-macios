using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using UIKit;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public partial class UITableViewCell {
		/// <include file="../../docs/api/UIKit/UITableViewCell.xml" path="/Documentation/Docs[@DocId='M:UIKit.UITableViewCell.#ctor(UIKit.UITableViewCellStyle,System.String)']/*" />
		public UITableViewCell (UITableViewCellStyle style, string reuseIdentifier) : this (style, reuseIdentifier is null ? (NSString) null : new NSString (reuseIdentifier))
		{
		}
	} /* class UITableViewCell */
}
