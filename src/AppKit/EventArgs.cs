//
// Copyright 2013 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// EventArgs.cs: augment generated Notification EventArgs classes with
//               better C#isms/strong typing.
//
// Enums defined here are not actual ObjC enum and exist only to map
// NSString keys to an enum value for better API. An 'Unknown' value
// exists on these enums for when a key cannot be mapped.

#if !__MACCATALYST__

using Foundation;

#nullable enable

namespace AppKit {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum NSFontCollectionAction {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Shown,
		/// <summary>To be added.</summary>
		Hidden,
		/// <summary>To be added.</summary>
		Renamed,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class NSFontCollectionChangedEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSFontCollectionAction Action {
			get {
				if (_Action == NSFontCollection.ActionWasShown) {
					return NSFontCollectionAction.Shown;
				} else if (_Action == NSFontCollection.ActionWasHidden) {
					return NSFontCollectionAction.Hidden;
				} else if (_Action == NSFontCollection.ActionWasRenamed) {
					return NSFontCollectionAction.Renamed;
				} else {
					return NSFontCollectionAction.Unknown;
				}
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSFontCollectionVisibility Visibility {
			get { return (NSFontCollectionVisibility) (int) _Visibility; }
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum NSPopoverCloseReason {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Standard,
		/// <summary>To be added.</summary>
		DetachToWindow,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class NSPopoverCloseEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSPopoverCloseReason Reason {
			get {
				if (_Reason == NSPopover.CloseReasonStandard) {
					return NSPopoverCloseReason.Standard;
				} else if (_Reason == NSPopover.CloseReasonDetachToWindow) {
					return NSPopoverCloseReason.DetachToWindow;
				} else {
					return NSPopoverCloseReason.Unknown;
				}
			}
		}
	}
}
#endif // !__MACCATALYST__
