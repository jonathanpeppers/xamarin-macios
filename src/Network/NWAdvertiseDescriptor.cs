//
// NWAdvertiseDescriptor.cs: Bindings the Netowrk nw_advertise_descriptor_t API
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2018 Microsoft Inc
//
#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using nw_advertise_descriptor_t = System.IntPtr;
using OS_nw_advertise_descriptor = System.IntPtr;
using OS_nw_txt_record = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWAdvertiseDescriptor : NativeObject {
		[Preserve (Conditional = true)]
		internal NWAdvertiseDescriptor (NativeHandle handle, bool owns) : base (handle, owns)
		{ }

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_advertise_descriptor nw_advertise_descriptor_create_application_service (IntPtr application_service_name);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		static OS_nw_advertise_descriptor nw_advertise_descriptor_create_application_service (string application_service_name)
		{
			using var namePtr = new TransientString (application_service_name);
			return nw_advertise_descriptor_create_application_service (namePtr);
		}

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public NWAdvertiseDescriptor (string applicationServiceName) : base (nw_advertise_descriptor_create_application_service (applicationServiceName), true) { }

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_advertise_descriptor_get_application_service_name (OS_nw_advertise_descriptor advertise_descriptor);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public string? ApplicationServiceName {
			get {
				var appNamePtr = nw_advertise_descriptor_get_application_service_name (GetCheckedHandle ());
				return Marshal.PtrToStringAnsi (appNamePtr);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_advertise_descriptor_create_bonjour_service (IntPtr name, IntPtr type, IntPtr domain);

		/// <param name="name">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="domain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NWAdvertiseDescriptor? CreateBonjourService (string name, string type, string? domain = null)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			if (type is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (type));

			using var namePtr = new TransientString (name);
			using var typePtr = new TransientString (type);
			using var domainPtr = new TransientString (domain);
			var x = nw_advertise_descriptor_create_bonjour_service (namePtr, typePtr, domainPtr);
			if (x == IntPtr.Zero)
				return null;
			return new NWAdvertiseDescriptor (x, owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_advertise_descriptor_set_txt_record (IntPtr handle, IntPtr txtRecord, nuint txtLen);

		/// <param name="txt">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTxtRecord (string txt)
		{
			if (txt is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (txt));
			var n = System.Text.Encoding.UTF8.GetByteCount (txt);
			using var txtPtr = new TransientString (txt);
			nw_advertise_descriptor_set_txt_record (GetCheckedHandle (), txtPtr, (nuint) n);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_advertise_descriptor_set_no_auto_rename (IntPtr handle, byte no_auto_rename);

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_advertise_descriptor_get_no_auto_rename (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool NoAutoRename {
			set => nw_advertise_descriptor_set_no_auto_rename (GetCheckedHandle (), value.AsByte ());
			get => nw_advertise_descriptor_get_no_auto_rename (GetCheckedHandle ()) != 0;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_txt_record nw_advertise_descriptor_copy_txt_record_object (OS_nw_advertise_descriptor advertise_descriptor);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_advertise_descriptor_set_txt_record_object (OS_nw_advertise_descriptor advertise_descriptor, OS_nw_txt_record txt_record);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public NWTxtRecord TxtRecord {
			get => new NWTxtRecord (nw_advertise_descriptor_copy_txt_record_object (GetCheckedHandle ()), owns: true);
			set {
				nw_advertise_descriptor_set_txt_record_object (GetCheckedHandle (), value.GetHandle ());
				GC.KeepAlive (value);
			}
		}
	}
}
