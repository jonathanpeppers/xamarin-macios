//
// SecProtocolOptions.cs: Bindings the Security sec_protocol_options_t
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

using sec_protocol_options_t = System.IntPtr;
using dispatch_queue_t = System.IntPtr;
using sec_identity_t = System.IntPtr;

namespace Security {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class SecProtocolOptions : NativeObject {
#if !COREBUILD
		// This type is only ever surfaced in response to callbacks in TLS/Network and documented as read-only
		// if this ever changes, make this public
		[Preserve (Conditional = true)]
		internal SecProtocolOptions (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_local_identity (sec_protocol_options_t handle, sec_identity_t identity);

		/// <param name="identity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetLocalIdentity (SecIdentity2 identity)
		{
			if (identity is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (identity));
			sec_protocol_options_set_local_identity (GetCheckedHandle (), identity.GetCheckedHandle ());
			GC.KeepAlive (identity);
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_append_tls_ciphersuite (sec_protocol_options_t options, TlsCipherSuite ciphersuite);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void AddTlsCipherSuite (TlsCipherSuite cipherSuite) => sec_protocol_options_append_tls_ciphersuite (GetCheckedHandle (), cipherSuite);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_add_tls_ciphersuite_group (sec_protocol_options_t handle, SslCipherSuiteGroup cipherSuiteGroup);

		/// <param name="cipherSuiteGroup">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'AddTlsCipherSuiteGroup (TlsCipherSuiteGroup)' instead.")]
		public void AddTlsCipherSuiteGroup (SslCipherSuiteGroup cipherSuiteGroup) => sec_protocol_options_add_tls_ciphersuite_group (GetCheckedHandle (), cipherSuiteGroup);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_append_tls_ciphersuite_group (sec_protocol_options_t options, TlsCipherSuiteGroup group);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void AddTlsCipherSuiteGroup (TlsCipherSuiteGroup cipherSuiteGroup) => sec_protocol_options_append_tls_ciphersuite_group (GetCheckedHandle (), cipherSuiteGroup);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_min_version (sec_protocol_options_t handle, SslProtocol protocol);

		/// <param name="protocol">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'SetTlsMinVersion (TlsProtocolVersion)' instead.")]
		public void SetTlsMinVersion (SslProtocol protocol) => sec_protocol_options_set_tls_min_version (GetCheckedHandle (), protocol);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_min_tls_protocol_version (sec_protocol_options_t handle, TlsProtocolVersion version);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetTlsMinVersion (TlsProtocolVersion protocol) => sec_protocol_options_set_min_tls_protocol_version (GetCheckedHandle (), protocol);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_max_version (sec_protocol_options_t handle, SslProtocol protocol);

		/// <param name="protocol">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'SetTlsMaxVersion (TlsProtocolVersion)' instead.")]
		public void SetTlsMaxVersion (SslProtocol protocol) => sec_protocol_options_set_tls_max_version (GetCheckedHandle (), protocol);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_max_tls_protocol_version (sec_protocol_options_t handle, TlsProtocolVersion version);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetTlsMaxVersion (TlsProtocolVersion protocol) => sec_protocol_options_set_max_tls_protocol_version (GetCheckedHandle (), protocol);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern TlsProtocolVersion sec_protocol_options_get_default_min_dtls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public TlsProtocolVersion DefaultMinDtlsProtocolVersion => sec_protocol_options_get_default_min_dtls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern TlsProtocolVersion sec_protocol_options_get_default_max_dtls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public TlsProtocolVersion DefaultMaxDtlsProtocolVersion => sec_protocol_options_get_default_max_dtls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern TlsProtocolVersion sec_protocol_options_get_default_min_tls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public TlsProtocolVersion DefaultMinTlsProtocolVersion => sec_protocol_options_get_default_min_tls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern TlsProtocolVersion sec_protocol_options_get_default_max_tls_protocol_version ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public TlsProtocolVersion DefaultMaxTlsProtocolVersion => sec_protocol_options_get_default_max_tls_protocol_version ();

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_add_tls_application_protocol (sec_protocol_options_t handle, IntPtr applicationProtocol);

		/// <param name="applicationProtocol">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AddTlsApplicationProtocol (string applicationProtocol)
		{
			if (applicationProtocol is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (applicationProtocol));
			using var applicationProtocolPtr = new TransientString (applicationProtocol);
			sec_protocol_options_add_tls_application_protocol (GetCheckedHandle (), applicationProtocolPtr);
		}

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_server_name (sec_protocol_options_t handle, IntPtr serverName);

		/// <param name="serverName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsServerName (string serverName)
		{
			if (serverName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (serverName));
			using var serverNamePtr = new TransientString (serverName);
			sec_protocol_options_set_tls_server_name (GetCheckedHandle (), serverNamePtr);
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use non-DHE cipher suites instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_diffie_hellman_parameters (IntPtr handle, IntPtr dispatchDataParameter);

		/// <param name="parameters">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use non-DHE cipher suites instead.")]
		public void SetTlsDiffieHellmanParameters (DispatchData parameters)
		{
			if (parameters is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (parameters));
			sec_protocol_options_set_tls_diffie_hellman_parameters (GetCheckedHandle (), parameters.Handle);
			GC.KeepAlive (parameters);
		}

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_add_pre_shared_key (IntPtr handle, IntPtr dispatchDataParameter);

		/// <param name="parameters">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use non-DHE cipher suites instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use non-DHE cipher suites instead.")]
		public void AddPreSharedKey (DispatchData parameters)
		{
			if (parameters is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (parameters));
			sec_protocol_options_set_tls_diffie_hellman_parameters (GetCheckedHandle (), parameters.Handle);
			GC.KeepAlive (parameters);
		}

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_tickets_enabled (IntPtr handle, byte ticketsEnabled);

		/// <param name="ticketsEnabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsTicketsEnabled (bool ticketsEnabled) => sec_protocol_options_set_tls_tickets_enabled (GetCheckedHandle (), (byte) (ticketsEnabled ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_is_fallback_attempt (IntPtr handle, byte isFallbackAttempt);

		/// <param name="isFallbackAttempt">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsIsFallbackAttempt (bool isFallbackAttempt) => sec_protocol_options_set_tls_is_fallback_attempt (GetCheckedHandle (), (byte) (isFallbackAttempt ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_resumption_enabled (IntPtr handle, byte resumptionEnabled);

		/// <param name="resumptionEnabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsResumptionEnabled (bool resumptionEnabled) => sec_protocol_options_set_tls_resumption_enabled (GetCheckedHandle (), (byte) (resumptionEnabled ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_false_start_enabled (IntPtr handle, byte falseStartEnabled);

		/// <param name="falseStartEnabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsFalseStartEnabled (bool falseStartEnabled) => sec_protocol_options_set_tls_false_start_enabled (GetCheckedHandle (), (byte) (falseStartEnabled ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_ocsp_enabled (IntPtr handle, byte ocspEnabled);

		/// <param name="ocspEnabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsOcspEnabled (bool ocspEnabled) => sec_protocol_options_set_tls_ocsp_enabled (GetCheckedHandle (), (byte) (ocspEnabled ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_sct_enabled (IntPtr handle, byte sctEnabled);

		/// <param name="sctEnabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsSignCertificateTimestampEnabled (bool sctEnabled) => sec_protocol_options_set_tls_sct_enabled (GetCheckedHandle (), (byte) (sctEnabled ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_renegotiation_enabled (IntPtr handle, byte renegotiationEnabled);

		/// <param name="renegotiationEnabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetTlsRenegotiationEnabled (bool renegotiationEnabled) => sec_protocol_options_set_tls_renegotiation_enabled (GetCheckedHandle (), (byte) (renegotiationEnabled ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_peer_authentication_required (IntPtr handle, byte peerAuthenticationRequired);

		/// <param name="peerAuthenticationRequired">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetPeerAuthenticationRequired (bool peerAuthenticationRequired) => sec_protocol_options_set_peer_authentication_required (GetCheckedHandle (), (byte) (peerAuthenticationRequired ? 1 : 0));

		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern void sec_protocol_options_set_key_update_block (sec_protocol_options_t options, BlockLiteral* key_update_block, dispatch_queue_t key_update_queue);

		/// <param name="keyUpdate">To be added.</param>
		///         <param name="keyUpdateQueue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void SetKeyUpdateCallback (SecProtocolKeyUpdate keyUpdate, DispatchQueue keyUpdateQueue)
		{
			if (keyUpdate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (keyUpdate));
			if (keyUpdateQueue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (keyUpdateQueue));

			unsafe {
				delegate* unmanaged<IntPtr, NativeHandle, NativeHandle, void> trampoline = &Trampolines.SDSecProtocolKeyUpdate.Invoke;
				using var block = new BlockLiteral (trampoline, keyUpdate, typeof (Trampolines.SDSecProtocolKeyUpdate), nameof (Trampolines.SDSecProtocolKeyUpdate.Invoke));
				sec_protocol_options_set_key_update_block (Handle, &block, keyUpdateQueue.Handle);
				GC.KeepAlive (keyUpdateQueue);
			}
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern byte sec_protocol_options_are_equal (sec_protocol_options_t optionsA, sec_protocol_options_t optionsB);

		// Equatable would be nice but would fail on earlier OS versions
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool IsEqual (SecProtocolOptions other)
		{
			if (other is null)
				return false;
			bool result = sec_protocol_options_are_equal (GetCheckedHandle (), other.Handle) != 0;
			GC.KeepAlive (other);
			return result;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public bool IsEqual (SecProtocolOptions optionsA, SecProtocolOptions optionsB)
		{
			if (optionsA is null)
				return (optionsB is null);
			else if (optionsB is null)
				return false;
			bool result = sec_protocol_options_are_equal (optionsA.Handle, optionsB.Handle) != 0;
			GC.KeepAlive (optionsA);
			GC.KeepAlive (optionsB);
			return result;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		static extern void sec_protocol_options_set_tls_pre_shared_key_identity_hint (sec_protocol_options_t options, IntPtr /* dispatch_data */ psk_identity_hint);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetTlsPreSharedKeyIdentityHint (DispatchData pskIdentityHint)
		{
			if (pskIdentityHint is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (pskIdentityHint));
			sec_protocol_options_set_tls_pre_shared_key_identity_hint (GetCheckedHandle (), pskIdentityHint.Handle);
			GC.KeepAlive (pskIdentityHint);
		}
#endif

#if !COREBUILD
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern void sec_protocol_options_set_challenge_block (sec_protocol_options_t options, BlockLiteral* /* sec_protocol_challenge_t */ challenge_block, IntPtr /* dispatch_queue_t */ challenge_queue);

		/// <summary>Set the challenge callback.</summary>
		/// <param name="challenge">The callback to call during a challenge.</param>
		/// <param name="queue">The queue where the callback is called.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetChallengeBlock (SecProtocolChallenge challenge, DispatchQueue queue)
		{
			unsafe {
				delegate* unmanaged<IntPtr, NativeHandle, NativeHandle, void> trampoline = &Trampolines.SDSecProtocolChallenge.Invoke;
				using var block = new BlockLiteral (trampoline, challenge, typeof (Trampolines.SDSecProtocolChallenge), nameof (Trampolines.SDSecProtocolChallenge.Invoke));
				sec_protocol_options_set_challenge_block (GetCheckedHandle (), &block, queue.GetNonNullHandle (nameof (queue)));
				GC.KeepAlive (queue);
			}
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern void sec_protocol_options_set_verify_block (sec_protocol_options_t options, BlockLiteral* /* sec_protocol_verify_t */ verify_block, IntPtr /* dispatch_queue_t */ verify_block_queue);

		/// <summary>Set the verify callback.</summary>
		/// <param name="verify">The callback to call during verification.</param>
		/// <param name="queue">The queue where the callback is called.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetVerifyBlock (SecProtocolVerify verify, DispatchQueue queue)
		{
			unsafe {
				delegate* unmanaged<IntPtr, NativeHandle, NativeHandle, NativeHandle, void> trampoline = &Trampolines.SDSecProtocolVerify.Invoke;
				using var block = new BlockLiteral (trampoline, verify, typeof (Trampolines.SDSecProtocolVerify), nameof (Trampolines.SDSecProtocolVerify.Invoke));
				sec_protocol_options_set_verify_block (GetCheckedHandle (), &block, queue.GetNonNullHandle (nameof (queue)));
				GC.KeepAlive (queue);
			}
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern void sec_protocol_options_set_pre_shared_key_selection_block (sec_protocol_options_t options, BlockLiteral* /* sec_protocol_pre_shared_key_selection_t */ psk_selection_block, IntPtr /* dispatch_queue_t */ psk_selection_queue);

		/// <summary>Set the pre-shared key selection callback.</summary>
		/// <param name="selection">The callback to call during pre-shared key selection.</param>
		/// <param name="queue">The queue where the callback is called.</param>
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetPreSharedKeySelectionBlock (SecProtocolPreSharedKeySelection selection, DispatchQueue queue)
		{
			unsafe {
				delegate* unmanaged<IntPtr, NativeHandle, NativeHandle, NativeHandle, void> trampoline = &Trampolines.SDSecProtocolPreSharedKeySelection.Invoke;
				using var block = new BlockLiteral (trampoline, selection, typeof (Trampolines.SDSecProtocolPreSharedKeySelection), nameof (Trampolines.SDSecProtocolPreSharedKeySelection.Invoke));
				sec_protocol_options_set_pre_shared_key_selection_block (GetCheckedHandle (), &block, queue.GetNonNullHandle (nameof (queue)));
				GC.KeepAlive (queue);
			}
		}
#endif // !COREBUILD
	}
}
