//
// VNBarcodeSymbologyExtensions.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;

namespace Vision {
	public static partial class VNBarcodeSymbologyExtensions {
		/// <param name="self">The instance on which this method operates.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSString [] GetConstants (this VNBarcodeSymbology [] self)
		{
			if (self is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (self));

			var array = new NSString [self.Length];
			for (int n = 0; n < self.Length; n++)
				array [n] = self [n].GetConstant ()!;
			return array;
		}

		/// <param name="constants">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static VNBarcodeSymbology [] GetValues (NSString []? constants)
		{
			if (constants is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (constants));

			var array = new VNBarcodeSymbology [constants.Length];
			for (int n = 0; n < constants.Length; n++)
				array [n] = GetValue (constants [n]);
			return array;
		}
	}
}
