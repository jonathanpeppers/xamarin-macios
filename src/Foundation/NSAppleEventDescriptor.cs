//
// NSAppleEventDescriptor.cs
//
// Copyright 2015 Xamarin Inc

#if MONOMAC

using System;

using AppKit;

namespace Foundation {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum NSAppleEventDescriptorType {
		/// <summary>To be added.</summary>
		Record,
		/// <summary>To be added.</summary>
		List,
	}

	public partial class NSAppleEventDescriptor {
		/// <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSAppleEventDescriptor (NSAppleEventDescriptorType type)
		{
			switch (type) {
			case NSAppleEventDescriptorType.List:
				InitializeHandle (_InitListDescriptor (), "listDescriptor");
				break;
			case NSAppleEventDescriptorType.Record:
				InitializeHandle (_InitRecordDescriptor (), "recordDescriptor");
				break;
			default:
				throw new ArgumentOutOfRangeException ("type");
			}
		}
	}
}

#endif // MONOMAC
