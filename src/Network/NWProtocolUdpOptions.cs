//
// NWProtocolTls: Bindings the Netowrk nw_protocol_options API focus on Udp options.
//
// Authors:
//   Manuel de la Pena <mandel@microsoft.com>
//
// Copyrigh 2019 Microsoft Inc
//

#nullable enable

using System;
using ObjCRuntime;
using Foundation;
using CoreFoundation;
using Security;

namespace Network {
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWProtocolUdpOptions : NWProtocolOptions {
		[Preserve (Conditional = true)]
		internal NWProtocolUdpOptions (NativeHandle handle, bool owns) : base (handle, owns) { }

		public NWProtocolUdpOptions () : this (nw_udp_create_options (), owns: true) { }

		public void SetPreferNoChecksum (bool preferNoChecksum) => nw_udp_options_set_prefer_no_checksum (GetCheckedHandle (), preferNoChecksum.AsByte ());
	}
}
