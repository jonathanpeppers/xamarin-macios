// Copyright 2016 Xamarin Inc. All rights reserved.

using System;
using System.ComponentModel;
using Foundation;
using ObjCRuntime;

#nullable enable

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace GameKit {
#if !NET
	public partial class GKGameSession {

		[Obsolete ("Empty stub (GKGameSessionEventListenerPrivate category members are not public API).")]
		public static void DidAddPlayer (GKGameSession session, GKCloudPlayer player) { }

		[Obsolete ("Empty stub (GKGameSessionEventListenerPrivate category members are not public API).")]
		public static void DidChangeConnectionState (GKGameSession session, GKCloudPlayer player, GKConnectionState newState) { }

		[Obsolete ("Empty stub (GKGameSessionEventListenerPrivate category members are not public API).")]
		public static void DidReceiveData (GKGameSession session, Foundation.NSData data, GKCloudPlayer player) { }

		[Obsolete ("Empty stub (GKGameSessionEventListenerPrivate category members are not public API).")]
		public static void DidReceiveMessage (GKGameSession session, string message, Foundation.NSData data, GKCloudPlayer player) { }

		[Obsolete ("Empty stub (GKGameSessionEventListenerPrivate category members are not public API).")]
		public static void DidRemovePlayer (GKGameSession session, GKCloudPlayer player) { }

		[Obsolete ("Empty stub (GKGameSessionEventListenerPrivate category members are not public API).")]
		public static void DidSaveData (GKGameSession session, GKCloudPlayer player, Foundation.NSData data) { }
	}
#endif

#if !XAMCORE_5_0
#if __IOS__ || __MACCATALYST__
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
#if NET
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#else
	[Unavailable (PlatformName.MacOSX)]
	[Unavailable (PlatformName.TvOS)]
#endif
	public interface IGKPeerPickerControllerDelegate : INativeObject, IDisposable {
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
#if NET
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#else
	[Unavailable (PlatformName.MacOSX)]
	[Unavailable (PlatformName.TvOS)]
#endif
	public static class GKPeerPickerControllerDelegate_Extensions {
		public static void ConnectionTypeSelected (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker, GKPeerPickerConnectionType type)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public static GKSession GetSession (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker, GKPeerPickerConnectionType forType)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public static void PeerConnected (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker, string peerId, GKSession toSession)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public static void ControllerCancelled (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
#if NET
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#else
	[Unavailable (PlatformName.MacOSX)]
	[Unavailable (PlatformName.TvOS)]
#endif
	public unsafe class GKPeerPickerControllerDelegate : NSObject, IGKPeerPickerControllerDelegate {
		public GKPeerPickerControllerDelegate () : base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		protected GKPeerPickerControllerDelegate (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		protected internal GKPeerPickerControllerDelegate (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public virtual void ConnectionTypeSelected (GKPeerPickerController picker, GKPeerPickerConnectionType type)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public virtual void ControllerCancelled (GKPeerPickerController picker)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public virtual GKSession GetSession (GKPeerPickerController picker, GKPeerPickerConnectionType forType)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public virtual void PeerConnected (GKPeerPickerController picker, string peerId, GKSession toSession)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	} /* class GKPeerPickerControllerDelegate */

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
#if NET
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#else
	[Unavailable (PlatformName.MacOSX)]
	[Unavailable (PlatformName.TvOS)]
#endif
	public class GKPeerPickerController : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new PlatformNotSupportedException (Constants.TypeUnavailable); } }

		public GKPeerPickerController () : base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		protected GKPeerPickerController (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		protected internal GKPeerPickerController (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public virtual void Dismiss ()
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		public virtual void Show ()
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual GKPeerPickerConnectionType ConnectionTypesMask {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <summary>An instance of the GameKit.IGKPeerPickerControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the GameKit.IGKPeerPickerControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		public IGKPeerPickerControllerDelegate Delegate {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual bool Visible {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		public virtual NSObject? WeakDelegate {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		protected override void Dispose (bool disposing)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	} /* class GKPeerPickerController */
#endif //  __IOS__ || __MACCATALYST__
#endif // !XAMCORE_5_0
}
