//
// SecCertificate2.cs: Bindings the Security's sec_trust_t
//
// The difference between SecCertificate2 and SecCertificate is that the
// SecCertificate2 is a binding for the new sec_trust_t API that was
// introduced on iOS 12/OSX Mojave, while SecCertificate is the older API
// that binds SecCertificateRef.
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2018 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace Security {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class SecCertificate2 : NativeObject {
		[Preserve (Conditional = true)]
		internal SecCertificate2 (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr sec_certificate_create (IntPtr seccertificateHandle);

		/// <param name="certificate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecCertificate2 (SecCertificate certificate)
		{
			if (certificate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificate));
			InitializeHandle (sec_certificate_create (certificate.Handle));
			GC.KeepAlive (certificate);
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static /* SecCertificateRef */ IntPtr sec_certificate_copy_ref (/* OS_sec_certificate */ IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecCertificate Certificate => new SecCertificate (sec_certificate_copy_ref (GetCheckedHandle ()), owns: true);
	}
}
