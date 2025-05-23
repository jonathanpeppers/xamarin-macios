using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using Security;

using OS_nw_protocol_metadata = System.IntPtr;
using SecProtocolMetadataRef = System.IntPtr;

#nullable enable

namespace Network {
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWQuicMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWQuicMetadata (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_remote_idle_timeout (OS_nw_protocol_metadata metadata);

		public ulong RemoteIdleTimeout
			=> nw_quic_get_remote_idle_timeout (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern ushort nw_quic_get_keepalive_interval (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_keepalive_interval (OS_nw_protocol_metadata metadata, ushort keepaliveInterval);

		public ushort KeepaliveInterval {
			get => nw_quic_get_keepalive_interval (GetCheckedHandle ());
			set => nw_quic_set_keepalive_interval (GetCheckedHandle (), value);
		}

		/// <summary>
		/// Set the default keepalive value. The current default is every 20 seconds, but this is subject to change.
		/// </summary>
		public void SetDefaultKeepAlive ()
		{
			// #define NW_QUIC_CONNECTION_DEFAULT_KEEPALIVE	UINT16_MAX
			KeepaliveInterval = ushort.MaxValue;
		}

		[DllImport (Constants.NetworkLibrary, EntryPoint = "nw_quic_get_application_error_reason")]
		static extern IntPtr nw_quic_get_application_error_reason_ptr (OS_nw_protocol_metadata metadata);

		static string nw_quic_get_application_error_reason (OS_nw_protocol_metadata metadata)
		{
			var ptr = nw_quic_get_application_error_reason_ptr (metadata);
			return TransientString.ToStringAndFree (ptr)!;
		}

		public string? ApplicationErrorReason
			=> nw_quic_get_application_error_reason (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_application_error (OS_nw_protocol_metadata metadata);

		public ulong ApplicationErrorCode =>
			nw_quic_get_application_error (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_application_error (OS_nw_protocol_metadata metadata, ulong application_error, IntPtr reason);

		static void nw_quic_set_application_error (OS_nw_protocol_metadata metadata, ulong application_error, string? reason)
		{
			using var reasonPtr = new TransientString (reason);
			nw_quic_set_application_error (metadata, application_error, reasonPtr);
		}

		public (ulong error, string? reason) ApplicationError {
			get => (ApplicationErrorCode, ApplicationErrorReason);
			set => nw_quic_set_application_error (GetCheckedHandle (), value.error, value.reason);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern SecProtocolMetadataRef nw_quic_copy_sec_protocol_metadata (OS_nw_protocol_metadata metadata);

		public SecProtocolMetadata SecProtocolMetadata
			=> new SecProtocolMetadata (nw_quic_copy_sec_protocol_metadata (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_stream_id (OS_nw_protocol_metadata metadata);

		public ulong StreamId
			=> nw_quic_get_stream_id (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_stream_application_error (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_stream_application_error (OS_nw_protocol_metadata metadata, ulong application_error);

		public ulong StreamApplicationError {
			get => nw_quic_get_stream_application_error (GetCheckedHandle ());
			set => nw_quic_set_stream_application_error (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_local_max_streams_bidirectional (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_local_max_streams_bidirectional (OS_nw_protocol_metadata metadata, ulong max_streams_bidirectional);

		public ulong MaxStreamsBidirectional {
			get => nw_quic_get_local_max_streams_bidirectional (GetCheckedHandle ());
			set => nw_quic_set_local_max_streams_bidirectional (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_local_max_streams_unidirectional (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_local_max_streams_unidirectional (OS_nw_protocol_metadata metadata, ulong max_streams_unidirectional);

		public ulong LocalMaxStreamsUnidirectional {
			get => nw_quic_get_local_max_streams_unidirectional (GetCheckedHandle ());
			set => nw_quic_set_local_max_streams_unidirectional (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_remote_max_streams_bidirectional (OS_nw_protocol_metadata metadata);

		public ulong RemoteMaxStreamsBidirectional
			=> nw_quic_get_remote_max_streams_bidirectional (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_remote_max_streams_unidirectional (OS_nw_protocol_metadata metadata);

		public ulong RemoteMaxStreamsUnidirectional
			=> nw_quic_get_remote_max_streams_unidirectional (GetCheckedHandle ());
	}
}
