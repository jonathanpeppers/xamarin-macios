//
// NWUdpMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API that is an Udp.
//
// Authors:
//   Manuel de la Pena <mandel@microsoft.com>
//
// Copyrigh 2019 Microsoft
//

#nullable enable

using System;
using ObjCRuntime;
using Foundation;
using Security;
using CoreFoundation;

namespace Network {
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWUdpMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWUdpMetadata (NativeHandle handle, bool owns) : base (handle, owns) { }

		public NWUdpMetadata () : this (nw_udp_create_metadata (), owns: true) { }
	}
}
