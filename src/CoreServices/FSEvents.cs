//
// FSEvents.cs: binding for CoreService's FSEvents API
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc

#nullable enable

#if MONOMAC

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace CoreServices {
	// FSEvents.h: typedef UInt32                          FSEventStreamCreateFlags;
	[Flags]
	public enum FSEventStreamCreateFlags : uint {
		/// <summary>To be added.</summary>
		None = 0x00000000,
		/*UseCFTypes = 0x00000001,*/
		/// <summary>To be added.</summary>
		NoDefer = 0x00000002,
		/// <summary>To be added.</summary>
		WatchRoot = 0x00000004,
		/// <summary>To be added.</summary>
		IgnoreSelf = 0x00000008,
		/// <summary>To be added.</summary>
		FileEvents = 0x00000010,
#if NET
		[SupportedOSPlatform ("macos")]
#endif
		MarkSelf = 0x00000020,
#if NET
		[SupportedOSPlatform ("macos")]
#endif
		UseExtendedData = 0x00000040,
#if NET
		[SupportedOSPlatform ("macos")]
#endif
		FullHistory = 0x00000080,
	}

	// FSEvents.h: typedef UInt32                          FSEventStreamEventFlags;
	[Flags]
	public enum FSEventStreamEventFlags : uint {
		/// <summary>To be added.</summary>
		None = 0x00000000,
		/// <summary>To be added.</summary>
		MustScanSubDirs = 0x00000001,
		/// <summary>To be added.</summary>
		UserDropped = 0x00000002,
		/// <summary>To be added.</summary>
		KernelDropped = 0x00000004,
		/// <summary>To be added.</summary>
		EventIdsWrapped = 0x00000008,
		/// <summary>To be added.</summary>
		HistoryDone = 0x00000010,
		/// <summary>To be added.</summary>
		RootChanged = 0x00000020,
		/// <summary>To be added.</summary>
		Mount = 0x00000040,
		/// <summary>To be added.</summary>
		Unmount = 0x00000080,
		/// <summary>To be added.</summary>
		ItemCreated = 0x00000100,
		/// <summary>To be added.</summary>
		ItemRemoved = 0x00000200,
		/// <summary>To be added.</summary>
		ItemInodeMetaMod = 0x00000400,
		/// <summary>To be added.</summary>
		ItemRenamed = 0x00000800,
		/// <summary>To be added.</summary>
		ItemModified = 0x00001000,
		/// <summary>To be added.</summary>
		ItemFinderInfoMod = 0x00002000,
		/// <summary>To be added.</summary>
		ItemChangeOwner = 0x00004000,
		/// <summary>To be added.</summary>
		ItemXattrMod = 0x00008000,
		/// <summary>To be added.</summary>
		ItemIsFile = 0x00010000,
		/// <summary>To be added.</summary>
		ItemIsDir = 0x00020000,
		/// <summary>To be added.</summary>
		ItemIsSymlink = 0x00040000,
		/// <summary>To be added.</summary>
		OwnEvent = 0x00080000,
		/// <summary>To be added.</summary>
		ItemIsHardlink = 0x00100000,
		/// <summary>To be added.</summary>
		ItemIsLastHardlink = 0x00200000,
#if NET
		[SupportedOSPlatform ("macos")]
#endif
		ItemCloned = 0x00400000,
	}

#if NET
	[SupportedOSPlatform ("macos")]
#endif
	public struct FSEvent {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ulong Id { get; internal set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Path { get; internal set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public FSEventStreamEventFlags Flags { get; internal set; }
		public ulong FileId { get; internal set; }

		public override string ToString ()
		{
			return String.Format ("[FSEvent: Id={0}, Path={1}, Flags={2}, FileId={3}]", Id, Path, Flags, FileId);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public const ulong SinceNowId = UInt64.MaxValue;

		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr FSEventsCopyUUIDForDevice (ulong device);

		public static Guid GetUuidForDevice (ulong device)
		{
			if (device <= 0) {
				return Guid.Empty;
			}

			var uuidRef = FSEventsCopyUUIDForDevice (device);
			if (uuidRef == IntPtr.Zero) {
				return Guid.Empty;
			}

			return Marshal.PtrToStructure<Guid> (uuidRef)!;
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern ulong FSEventsGetCurrentEventId ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static ulong CurrentEventId {
			get { return FSEventsGetCurrentEventId (); }
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern ulong FSEventsGetLastEventIdForDeviceBeforeTime (
			ulong device, double timeInSecondsSinceEpoch);

		public static ulong GetLastEventIdForDeviceBeforeTime (ulong device, double timeInSecondsSinceEpoch)
		{
			return FSEventsGetLastEventIdForDeviceBeforeTime (device, timeInSecondsSinceEpoch);
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern byte FSEventsPurgeEventsForDeviceUpToEventId (ulong device, ulong eventId);

		public static bool PurgeEventsForDeviceUpToEventId (ulong device, ulong eventId)
		{
			return FSEventsPurgeEventsForDeviceUpToEventId (device, eventId) != 0;
		}
	}

	struct FSEventStreamContext {
		nint version; /* CFIndex: only valid value is zero */
		internal IntPtr Info; /* void * __nullable */
		IntPtr Retain; /* CFAllocatorRetainCallBack __nullable */
#if NET
		internal unsafe delegate* unmanaged<IntPtr, void> Release; /* CFAllocatorReleaseCallBack __nullable */
#else
		internal IntPtr Release; /* CFAllocatorReleaseCallBack __nullable */
#endif
		IntPtr CopyDescription; /* CFAllocatorCopyDescriptionCallBack __nullable */
	}

	public delegate void FSEventStreamEventsHandler (object sender, FSEventStreamEventsArgs args);

#if NET
	[SupportedOSPlatform ("macos")]
#endif
	public sealed class FSEventStreamEventsArgs : EventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public FSEvent [] Events { get; private set; }

		internal FSEventStreamEventsArgs (FSEvent [] events)
		{
			Events = events;
		}
	}

	/// <summary>
	/// Creation options for <see cref="FSEventStream"/>.
	/// </summary>
#if NET
	[SupportedOSPlatform ("macos")]
#endif
	public sealed class FSEventStreamCreateOptions {
		/// <summary>
		/// The allocator to use to allocate memory for the stream. If <c>null</c>, the default
		/// allocator will be used.
		/// </summary>
		public CFAllocator? Allocator { get; set; }

		/// <summary>
		/// A <c>dev_t</c> corresponding to the device which you want to receive notifications from.
		/// The <c>dev_t</c> is the same as the <c>st_dev</c> field from a <c>stat</c> structure of a
		/// file on that device or the <c>f_fsid[0]</c> field of a <c>statfs</c> structure.
		/// </summary>
		public ulong? DeviceToWatch { get; set; }

		/// <summary>
		/// A list of directory paths, signifying the root of a filesystem hierarchy to be watched
		/// for modifications. If <see cref="DeviceToWatch"/> is set, the list of paths should be
		/// relative to the root of the device. For example, if a volume "MyData" is mounted at
		/// "/Volumes/MyData" and you want to watch "/Volumes/MyData/Pictures/July", specify a path
		/// string of "Pictures/July". To watch the root of a volume pass a path of "" (the empty string).
		/// </summary>
		public IReadOnlyList<string>? PathsToWatch { get; set; }

		// NB. to be set only by the FSEventStream .ctors
		internal NSArray? NSPathsToWatch { get; set; }

		/// <summary>
		/// The service will supply events that have happened after the given event ID. To ask for
		/// events "since now," set to <c>null</c> or <see cref="FSEvent.SinceNowId"/>. Often, clients
		/// will supply the highest-numbered event ID they have received in a callback, which they can
		/// obtain via <see cref="FSEventStream.LatestEventId"/>. Do not set to zero, unless you want to
		/// receive events for every directory modified since "the beginning of time" -- an unlikely scenario.
		/// </summary>
		public ulong? SinceWhenId { get; set; }

		/// <summary>
		/// The amount of time the service should wait after hearing about an event from the kernel
		/// before passing it along to the client via its callback. Specifying a larger value may result
		/// in more effective temporal coalescing, resulting in fewer callbacks.
		/// </summary>
		public TimeSpan Latency { get; set; }

		/// <summary>
		/// Flags that modify the behavior of the stream being created.
		/// See <see cref="FSEventStreamCreateFlags"/>.
		/// </summary>
		public FSEventStreamCreateFlags Flags { get; set; }

		public FSEventStreamCreateOptions ()
		{
		}

		public FSEventStreamCreateOptions (FSEventStreamCreateFlags flags, TimeSpan latency,
			params string [] pathsToWatch)
		{
			Flags = flags;
			Latency = latency;
			PathsToWatch = pathsToWatch;
		}

		public FSEventStreamCreateOptions (FSEventStreamCreateFlags flags, TimeSpan latency,
			ulong deviceToWatch, params string [] pathsToWatchRelativeToDevice)
		{
			Flags = flags;
			Latency = latency;
			DeviceToWatch = deviceToWatch;
			PathsToWatch = pathsToWatchRelativeToDevice;
		}

		public FSEventStream CreateStream () => new (this);
	}

#if NET
	[SupportedOSPlatform ("macos")]
#endif
	public class FSEventStream : NativeObject {
		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamRetain (IntPtr handle);

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamRelease (IntPtr handle);

		protected internal override void Retain ()
		{
			FSEventStreamRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			FSEventStreamRelease (GetCheckedHandle ());
		}

		delegate void FSEventStreamCallback (IntPtr handle, IntPtr userData, nint numEvents,
 			IntPtr eventPaths, IntPtr eventFlags, IntPtr eventIds);

		[DllImport (Constants.CoreServicesLibrary)]
		unsafe static extern IntPtr FSEventStreamCreate (IntPtr allocator,
#if NET
			delegate* unmanaged<IntPtr, IntPtr, nint, IntPtr, IntPtr, IntPtr, void> callback,
#else
			IntPtr callback,
#endif
			FSEventStreamContext* context, IntPtr pathsToWatch,
			ulong sinceWhen, double latency, FSEventStreamCreateFlags flags);

		[DllImport (Constants.CoreServicesLibrary)]
		unsafe static extern IntPtr FSEventStreamCreateRelativeToDevice (IntPtr allocator,
#if NET
			delegate* unmanaged<IntPtr, IntPtr, nint, IntPtr, IntPtr, IntPtr, void> callback,
#else
			IntPtr callback,
#endif
			FSEventStreamContext* context, ulong deviceToWatch, IntPtr pathsToWatchRelativeToDevice,
			ulong sinceWhen, double latency, FSEventStreamCreateFlags flags);

		public FSEventStream (FSEventStreamCreateOptions options)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			NSArray pathsToWatch;

			if (options.NSPathsToWatch is not null) {
				pathsToWatch = options.NSPathsToWatch;
			} else if (options.PathsToWatch?.Count == 0) {
				throw new ArgumentException (
					$"must specify at least one path to watch on " +
					$"{nameof (FSEventStreamCreateOptions)}.{nameof (FSEventStreamCreateOptions.PathsToWatch)}",
					nameof (options));
			} else {
				pathsToWatch = NSArray.FromStrings (options.PathsToWatch);
			}

			var gch = GCHandle.Alloc (this);

			var context = default (FSEventStreamContext);
			context.Info = GCHandle.ToIntPtr (gch);
#if NET
			unsafe {
				context.Release = &FreeGCHandle;
			}
#else
			context.Release = Marshal.GetFunctionPointerForDelegate (releaseContextCallback);
#endif

			var allocator = options.Allocator.GetHandle ();
			var sinceWhenId = options.SinceWhenId ?? FSEvent.SinceNowId;
			var latency = options.Latency.TotalSeconds;
			var flags = options.Flags |= (FSEventStreamCreateFlags) 0x1 /* UseCFTypes */;

			IntPtr handle;
			unsafe {
				if (options.DeviceToWatch.HasValue) {
					handle = FSEventStreamCreateRelativeToDevice (
						allocator,
#if NET
						&EventsCallback,
#else
						Marshal.GetFunctionPointerForDelegate (eventsCallback),
#endif
						&context,
						options.DeviceToWatch.Value,
						pathsToWatch.Handle, sinceWhenId, latency, flags);
					GC.KeepAlive (pathsToWatch);
				} else {
					handle = FSEventStreamCreate (
						allocator,
#if NET
						&EventsCallback,
#else
						Marshal.GetFunctionPointerForDelegate (eventsCallback),
#endif
						&context,
						pathsToWatch.Handle, sinceWhenId, latency, flags);
					GC.KeepAlive (pathsToWatch);
				}
			}

			InitializeHandle (handle);
		}

		public FSEventStream (CFAllocator? allocator, NSArray pathsToWatch,
			ulong sinceWhenId, TimeSpan latency, FSEventStreamCreateFlags flags)
			: this (new () {
				Allocator = allocator,
				NSPathsToWatch = pathsToWatch ?? throw new ArgumentNullException (nameof (pathsToWatch)),
				SinceWhenId = sinceWhenId,
				Latency = latency,
				Flags = flags,
			})
		{
		}

		public FSEventStream (string [] pathsToWatch, TimeSpan latency, FSEventStreamCreateFlags flags)
			: this (new () {
				PathsToWatch = pathsToWatch ?? throw new ArgumentNullException (nameof (pathsToWatch)),
				Latency = latency,
				Flags = flags,
			})
		{
		}

#if !NET
		static readonly FSEventStreamCallback eventsCallback = EventsCallback;

		static readonly ReleaseContextCallback releaseContextCallback = FreeGCHandle;
		internal delegate void ReleaseContextCallback (IntPtr info);
#endif

#if NET
		[UnmanagedCallersOnly]
#endif
		static void FreeGCHandle (IntPtr gchandle)
		{
			GCHandle.FromIntPtr (gchandle).Free ();
		}

		static readonly nint CFStringTypeID = CFString.GetTypeID ();
		static readonly nint CFDictionaryTypeID = CFDictionary.GetTypeID ();

		// These constants are defined in FSEvents.h but do not end up exported in any binaries,
		// so we cannot use Dlfcn.GetStringConstant against CoreServices. -abock, 2022-03-04
		static readonly NSString kFSEventStreamEventExtendedDataPathKey = new ("path");
		static readonly NSString kFSEventStreamEventExtendedFileIDKey = new ("fileID");
		static readonly NSString kFSEventStreamEventExtendedDocIDKey = new ("docID");

#if NET
		[UnmanagedCallersOnly]
#endif
		static void EventsCallback (IntPtr handle, IntPtr userData, nint numEvents,
			IntPtr eventPaths, IntPtr eventFlags, IntPtr eventIds)
		{
			if (numEvents == 0) {
				return;
			}

			var events = new FSEvent [numEvents];

			for (int i = 0; i < events.Length; i++) {
				string? path = null;
				long fileId = 0;

				var eventDataHandle = CFArray.CFArrayGetValueAtIndex (eventPaths, i);
				var eventDataType = CFType.GetTypeID (eventDataHandle);

				if (eventDataType == CFStringTypeID) {
					path = CFString.FromHandle (eventDataHandle);
				} else if (eventDataType == CFDictionaryTypeID) {
					path = CFString.FromHandle (CFDictionary.GetValue (
						eventDataHandle,
						kFSEventStreamEventExtendedDataPathKey.Handle));

					var fileIdHandle = CFDictionary.GetValue (
						eventDataHandle,
						kFSEventStreamEventExtendedFileIDKey.Handle);
					if (fileIdHandle != IntPtr.Zero) {
						unsafe {
							CFDictionary.CFNumberGetValue (fileIdHandle, 4 /*kCFNumberSInt64Type*/, &fileId);
						}
					}
				}

				events [i] = new FSEvent {
					Id = (ulong) Marshal.ReadInt64 (eventIds, i * 8),
					Path = path,
					Flags = (FSEventStreamEventFlags) (uint) Marshal.ReadInt32 (eventFlags, i * 4),
					FileId = (ulong) fileId,
				};
			}

			var instance = GCHandle.FromIntPtr (userData).Target as FSEventStream;
			instance?.OnEvents (events);
		}

		public event FSEventStreamEventsHandler? Events;

		protected virtual void OnEvents (FSEvent [] events)
		{
			var handler = Events;
			if (handler is not null) {
				handler (this, new FSEventStreamEventsArgs (events));
			}
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr FSEventStreamCopyDescription (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Description {
			get {
				if (Handle == IntPtr.Zero) {
					return null;
				}

				return CFString.FromHandle (FSEventStreamCopyDescription (Handle), true);
			}
		}

		public override string? ToString ()
		{
			return Description;
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamShow (IntPtr handle);

		public void Show ()
		{
			FSEventStreamShow (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern byte FSEventStreamStart (IntPtr handle);

		public bool Start ()
		{
			return FSEventStreamStart (GetCheckedHandle ()) != 0;
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamStop (IntPtr handle);

		public void Stop ()
		{
			FSEventStreamStop (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamScheduleWithRunLoop (IntPtr handle,
			IntPtr runLoop, IntPtr runLoopMode);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos13.0", "Use 'SetDispatchQueue' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 13,0, message: "Use 'SetDispatchQueue' instead.")]
#endif
		public void ScheduleWithRunLoop (CFRunLoop runLoop, NSString runLoopMode)
		{
			FSEventStreamScheduleWithRunLoop (GetCheckedHandle (), runLoop.Handle, runLoopMode.Handle);
			GC.KeepAlive (runLoop);
			GC.KeepAlive (runLoopMode);
		}

		public void ScheduleWithRunLoop (CFRunLoop runLoop)
		{
			ScheduleWithRunLoop (runLoop, CFRunLoop.ModeDefault);
		}

		public void ScheduleWithRunLoop (NSRunLoop runLoop, NSString runLoopMode)
		{
			ScheduleWithRunLoop (runLoop.GetCFRunLoop (), runLoopMode);
		}

		public void ScheduleWithRunLoop (NSRunLoop runLoop)
		{
			ScheduleWithRunLoop (runLoop.GetCFRunLoop (), CFRunLoop.ModeDefault);
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamUnscheduleFromRunLoop (IntPtr handle,
			IntPtr runLoop, IntPtr runLoopMode);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos13.0", "Use 'SetDispatchQueue' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 13,0, message: "Use 'SetDispatchQueue' instead.")]
#endif
		public void UnscheduleFromRunLoop (CFRunLoop runLoop, NSString runLoopMode)
		{
			FSEventStreamScheduleWithRunLoop (GetCheckedHandle (), runLoop.Handle, runLoopMode.Handle);
			GC.KeepAlive (runLoop);
			GC.KeepAlive (runLoopMode);
		}

		public void UnscheduleFromRunLoop (CFRunLoop runLoop)
		{
			UnscheduleFromRunLoop (runLoop, CFRunLoop.ModeDefault);
		}

		public void UnscheduleFromRunLoop (NSRunLoop runLoop, NSString runLoopMode)
		{
			UnscheduleFromRunLoop (runLoop.GetCFRunLoop (), runLoopMode);
		}

		public void UnscheduleFromRunLoop (NSRunLoop runLoop)
		{
			UnscheduleFromRunLoop (runLoop.GetCFRunLoop (), CFRunLoop.ModeDefault);
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamSetDispatchQueue (IntPtr handle, IntPtr dispatchQueue);

		public void SetDispatchQueue (DispatchQueue? dispatchQueue)
		{
			FSEventStreamSetDispatchQueue (GetCheckedHandle (), dispatchQueue.GetHandle ());
			GC.KeepAlive (dispatchQueue);
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern ulong FSEventStreamGetDeviceBeingWatched (IntPtr handle);

		public ulong DeviceBeingWatched => FSEventStreamGetDeviceBeingWatched (GetCheckedHandle ());

		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr FSEventStreamCopyPathsBeingWatched (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? []? PathsBeingWatched {
			get {
				var cfarray = FSEventStreamCopyPathsBeingWatched (GetCheckedHandle ());
				if (cfarray == IntPtr.Zero)
					return Array.Empty<string> ();
				return CFArray.StringArrayFromHandle (cfarray, true);
			}
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern uint FSEventStreamFlushAsync (IntPtr handle);

		public uint FlushAsync ()
		{
			return FSEventStreamFlushAsync (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamFlushSync (IntPtr handle);

		public void FlushSync ()
		{
			FSEventStreamFlushSync (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern void FSEventStreamInvalidate (IntPtr handle);

		public void Invalidate ()
		{
			FSEventStreamInvalidate (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreServicesLibrary)]
		static extern ulong FSEventStreamGetLatestEventId (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ulong LatestEventId {
			get {
				return FSEventStreamGetLatestEventId (GetCheckedHandle ());
			}
		}
	}
}

#endif // MONOMAC
