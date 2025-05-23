// 
// ImportExport.cs
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright 2011-2014 Xamarin Inc.
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

#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace Security {

	/// <summary>Encapsulates the import and export of identities and certificates.</summary>
	///     <remarks>To be added.</remarks>
	public partial class SecImportExport {

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode SecPKCS12Import (IntPtr pkcs12_data, IntPtr options, IntPtr* items);

		/// <param name="buffer">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="array">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public SecStatusCode ImportPkcs12 (byte [] buffer, NSDictionary options, out NSDictionary [] array)
		{
			using (NSData data = NSData.FromArray (buffer)) {
				return ImportPkcs12 (data, options, out array);
			}
		}

		/// <param name="data">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="array">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public SecStatusCode ImportPkcs12 (NSData data, NSDictionary options, out NSDictionary [] array)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			IntPtr handle;
			SecStatusCode code;
			unsafe {
				code = SecPKCS12Import (data.Handle, options.Handle, &handle);
				GC.KeepAlive (data);
				GC.KeepAlive (options);
			}
			array = NSArray.ArrayFromHandle<NSDictionary> (handle);
			NSObject.DangerousRelease (handle);
			return code;
		}
	}
}
