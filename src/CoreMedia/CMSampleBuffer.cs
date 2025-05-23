// 
// CMSampleBuffer.cs: Implements the managed CMSampleBuffer
//
// Authors: Mono Team
//			Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2010 Novell, Inc
// Copyright 2012-2014 Xamarin Inc
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

#if !COREBUILD
using AudioToolbox;
using CoreVideo;
#if !MONOMAC
using UIKit;
#endif
#endif

namespace CoreMedia {

	/// <summary>A container of zero-or-more samples of a particular media type.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/AVCaptureFrames/">avcaptureframes</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMSampleBuffer : NativeObject, ICMAttachmentBearer {
#if !COREBUILD
		GCHandle invalidate;

		[Preserve (Conditional = true)]
		internal CMSampleBuffer (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		internal static CMSampleBuffer? Create (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				return null;
			return new CMSampleBuffer (handle, owns);
		}

		/// <include file="../../docs/api/CoreMedia/CMSampleBuffer.xml" path="/Documentation/Docs[@DocId='M:CoreMedia.CMSampleBuffer.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (invalidate.IsAllocated)
				invalidate.Free ();

			base.Dispose (disposing);
		}

		/// <summary>Get this sample buffer's tagged buffer group.</summary>
		/// <returns>The tagged buffer group for this sample buffer, or null in case of failure or if this sample buffer doesn't contain a tagged buffer group.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("tvos17.0")]
		public CMTaggedBufferGroup? TaggedBufferGroup {
			get => CMTaggedBufferGroup.GetTaggedBufferGroup (this);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static CMSampleBufferError CMAudioSampleBufferCreateWithPacketDescriptions (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CMBlockBufferRef */ IntPtr dataBuffer,
			/* Boolean */ byte dataReady,
			/* CMSampleBufferMakeDataReadyCallback */ IntPtr makeDataReadyCallback,
			/* void */ IntPtr makeDataReadyRefcon,
			/* CMFormatDescriptionRef */ IntPtr formatDescription,
			/* CMItemCount */ nint numSamples,
			CMTime sbufPTS,
			/* AudioStreamPacketDescription* */ AudioStreamPacketDescription* packetDescriptions,
			/* CMSampleBufferRef* */ IntPtr* sBufOut);

		/// <param name="dataBuffer">To be added.</param>
		///         <param name="formatDescription">To be added.</param>
		///         <param name="samplesCount">To be added.</param>
		///         <param name="sampleTimestamp">To be added.</param>
		///         <param name="packetDescriptions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMSampleBuffer? CreateWithPacketDescriptions (CMBlockBuffer? dataBuffer, CMFormatDescription formatDescription, int samplesCount,
			CMTime sampleTimestamp, AudioStreamPacketDescription [] packetDescriptions, out CMSampleBufferError error)
		{
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));
			if (samplesCount <= 0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (samplesCount), "Negative");

			IntPtr buffer;
			unsafe {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					error = CMAudioSampleBufferCreateWithPacketDescriptions (
								IntPtr.Zero,
								dataBuffer.GetHandle (),
								1, IntPtr.Zero, IntPtr.Zero,
								formatDescription.Handle,
								samplesCount, sampleTimestamp,
								packetDescriptionsPtr,
								&buffer);
					GC.KeepAlive (dataBuffer);
					GC.KeepAlive (formatDescription);
				}
			}

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern OSStatus CMSampleBufferCreateCopyWithNewTiming (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CMSampleBufferRef */ IntPtr originalSBuf,
			/* CMItemCount */ nint numSampleTimingEntries,
			CMSampleTimingInfo* sampleTimingArray,
			/* CMSampleBufferRef* */ IntPtr* sBufCopyOut
			);

		/// <param name="original">The existing sample buffer providing the default values for the new sample buffer.</param>
		///         <param name="timing">The timing information for the new sample buffer.</param>
		///         <summary>Clones a sample buffer, using the provided timing information.</summary>
		///         <returns>An identical sample buffer as the original one, except with the provided timing information.</returns>
		///         <remarks>To be added.</remarks>
		public static CMSampleBuffer? CreateWithNewTiming (CMSampleBuffer original, CMSampleTimingInfo []? timing)
		{
			OSStatus status;
			return CreateWithNewTiming (original, timing, out status);
		}

#if !XAMCORE_5_0
		// OSStatus was incorrectly defined as IntPtr in this file, so providing this overload to keep compatibility,
		// while at the same time highly discourage using this overload.
		[EditorBrowsable (EditorBrowsableState.Never)]
		[OverloadResolutionPriorityAttribute (-1)]
		public static CMSampleBuffer? CreateWithNewTiming (CMSampleBuffer original, CMSampleTimingInfo []? timing, out nint status)
		{
			var rv = CreateWithNewTiming (original, timing, out var actualStatus);
			status = (nint) actualStatus;
			return rv;
		}
#endif // XAMCORE_5_0

		public unsafe static CMSampleBuffer? CreateWithNewTiming (CMSampleBuffer original, CMSampleTimingInfo []? timing, out OSStatus status)
		{
			if (original is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (original));

			nint count = timing is null ? 0 : timing.Length;
			IntPtr handle;

			fixed (CMSampleTimingInfo* t = timing) {
				status = CMSampleBufferCreateCopyWithNewTiming (IntPtr.Zero, original.Handle, count, t, &handle);
				GC.KeepAlive (original);
				if (status != (OSStatus) 0)
					return null;
			}

			return new CMSampleBuffer (handle, true);
		}

		// CMSampleBufferCallBlockForEachSample is not bound because it's signature is problematic wrt AOT and
		// we can provide the same managed API using CMSampleBufferCallForEachSample

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern CMSampleBufferError CMSampleBufferCallForEachSample (
			/* CMSampleBufferRef */ IntPtr sbuf,
			delegate* unmanaged<IntPtr, int, IntPtr, CMSampleBufferError> callback,
		   /* void* */ IntPtr refcon);

		[UnmanagedCallersOnly]
		static CMSampleBufferError ForEachSampleHandler (IntPtr sbuf, int index, IntPtr refCon)
		{
			GCHandle gch = GCHandle.FromIntPtr (refCon);
			var obj = gch.Target as Tuple<Func<CMSampleBuffer, int, CMSampleBufferError>, CMSampleBuffer>;
			if (obj is null)
				return CMSampleBufferError.RequiredParameterMissing;
			return obj.Item1 (obj.Item2, index);
		}

		/// <param name="callback">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError CallForEachSample (Func<CMSampleBuffer, int, CMSampleBufferError> callback)
		{
			// it makes no sense not to provide a callback - and it also crash the app
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));

			GCHandle h = GCHandle.Alloc (Tuple.Create (callback, this));
			try {
				unsafe {
					return CMSampleBufferCallForEachSample (Handle, &ForEachSampleHandler, (IntPtr) h);
				}
			} finally {
				h.Free ();
			}
		}

		/*
				[DllImport(Constants.CoreMediaLibrary)]
				int CMSampleBufferCopySampleBufferForRange (
				   CFAllocatorRef allocator,
				   CMSampleBufferRef sbuf,
				   CFRange sampleRange,
				   CMSampleBufferRef *sBufOut
				);

				[DllImport(Constants.CoreMediaLibrary)]
				int CMSampleBufferCreate (
				   CFAllocatorRef allocator,
				   CMBlockBufferRef dataBuffer,
				   Boolean dataReady,
				   CMSampleBufferMakeDataReadyCallback makeDataReadyCallback,
				   void *makeDataReadyRefcon,
				   CMFormatDescriptionRef formatDescription,
				   int numSamples,
				   int numSampleTimingEntries,
				   const CMSampleTimingInfo *sampleTimingArray,
				   int numSampleSizeEntries,
				   const uint *sampleSizeArray,
				   CMSampleBufferRef *sBufOut
				);

				[DllImport(Constants.CoreMediaLibrary)]
				int CMSampleBufferCreateCopy (
				   CFAllocatorRef allocator,
				   CMSampleBufferRef sbuf,
				   CMSampleBufferRef *sbufCopyOut
				);
				*/

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMSampleBufferError CMSampleBufferCreateForImageBuffer (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CVImageBufferRef */ IntPtr imageBuffer,
			/* Boolean */ byte dataReady,
			/* CMSampleBufferMakeDataReadyCallback */ IntPtr makeDataReadyCallback,
			/* void* */ IntPtr makeDataReadyRefcon,
			/* CMVideoFormatDescriptionRef */ IntPtr formatDescription,
			/* const CMSampleTimingInfo* */ CMSampleTimingInfo* sampleTiming,
			/* CMSampleBufferRef* */ IntPtr* bufOut
		);

		/// <param name="imageBuffer">To be added.</param>
		///         <param name="dataReady">To be added.</param>
		///         <param name="formatDescription">To be added.</param>
		///         <param name="sampleTiming">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMSampleBuffer? CreateForImageBuffer (CVImageBuffer imageBuffer, bool dataReady, CMVideoFormatDescription formatDescription, CMSampleTimingInfo sampleTiming, out CMSampleBufferError error)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));

			IntPtr buffer;
			unsafe {
				error = CMSampleBufferCreateForImageBuffer (
							IntPtr.Zero,
							imageBuffer.Handle, dataReady.AsByte (),
							IntPtr.Zero, IntPtr.Zero,
							formatDescription.Handle,
							&sampleTiming,
							&buffer);
				GC.KeepAlive (imageBuffer);
				GC.KeepAlive (formatDescription);
			}

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Boolean */ byte CMSampleBufferDataIsReady (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool DataIsReady {
			get {
				return CMSampleBufferDataIsReady (Handle) != 0;
			}
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioBufferListWithRetainedBlockBuffer (
		   CMSampleBufferRef sbuf,
		   uint *bufferListSizeNeededOut,
		   AudioBufferList *bufferListOut,
		   uint bufferListSize,
		   CFAllocatorRef bbufStructAllocator,
		   CFAllocatorRef bbufMemoryAllocator,
		   uint32_t flags,
		   CMBlockBufferRef *blockBufferOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioStreamPacketDescriptions (
		   CMSampleBufferRef sbuf,
		   uint packetDescriptionsSize,
		   AudioStreamPacketDescription *packetDescriptionsOut,
		   uint *packetDescriptionsSizeNeededOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioStreamPacketDescriptionsPtr (
		   CMSampleBufferRef sbuf,
		   const AudioStreamPacketDescription **packetDescriptionsPtrOut,
		   uint *packetDescriptionsSizeOut
		);*/

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMBlockBufferRef */ IntPtr CMSampleBufferGetDataBuffer (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMBlockBuffer? GetDataBuffer ()
		{
			var blockHandle = CMSampleBufferGetDataBuffer (Handle);
			if (blockHandle == IntPtr.Zero) {
				return null;
			} else {
				return new CMBlockBuffer (blockHandle, false);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetDecodeTimeStamp (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime DecodeTimeStamp {
			get {
				return CMSampleBufferGetDecodeTimeStamp (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetDuration (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime Duration {
			get {
				return CMSampleBufferGetDuration (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMFormatDescriptionRef */ IntPtr CMSampleBufferGetFormatDescription (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMAudioFormatDescription? GetAudioFormatDescription ()
		{
			var descHandle = CMSampleBufferGetFormatDescription (Handle);
			if (descHandle == IntPtr.Zero)
				return null;

			return new CMAudioFormatDescription (descHandle, false);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMVideoFormatDescription? GetVideoFormatDescription ()
		{
			var descHandle = CMSampleBufferGetFormatDescription (Handle);
			if (descHandle == IntPtr.Zero)
				return null;

			return new CMVideoFormatDescription (descHandle, false);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CVImageBufferRef */ IntPtr CMSampleBufferGetImageBuffer (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CVImageBuffer? GetImageBuffer ()
		{
			IntPtr ib = CMSampleBufferGetImageBuffer (Handle);
			if (ib == IntPtr.Zero)
				return null;

			var ibt = CFType.GetTypeID (ib);
			if (ibt == CVPixelBuffer.GetTypeID ())
				return new CVPixelBuffer (ib, false);
			return new CVImageBuffer (ib, false);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMItemCount */ nint CMSampleBufferGetNumSamples (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint NumSamples {
			get {
				return CMSampleBufferGetNumSamples (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputDecodeTimeStamp (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime OutputDecodeTimeStamp {
			get {
				return CMSampleBufferGetOutputDecodeTimeStamp (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputDuration (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime OutputDuration {
			get {
				return CMSampleBufferGetOutputDuration (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputPresentationTimeStamp (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime OutputPresentationTimeStamp {
			get {
				return CMSampleBufferGetOutputPresentationTimeStamp (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferSetOutputPresentationTimeStamp (/* CMSampleBufferRef */ IntPtr sbuf, CMTime outputPresentationTimeStamp);

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetOutputSampleTimingInfoArray (
		   CMSampleBufferRef sbuf,
		   int timingArrayEntries,
		   CMSampleTimingInfo *timingArrayOut,
		   int *timingArrayEntriesNeededOut
		);*/

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetPresentationTimeStamp (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime PresentationTimeStamp {
			get {
				return CMSampleBufferGetPresentationTimeStamp (Handle);
			}
			set {
				var result = CMSampleBufferSetOutputPresentationTimeStamp (Handle, value);
				if (result != 0)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (result.ToString ());
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFArrayRef */ IntPtr CMSampleBufferGetSampleAttachmentsArray (/* CMSampleBufferRef */ IntPtr sbuf, /* Boolean */ byte createIfNecessary);

		/// <param name="createIfNecessary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferAttachmentSettings? [] GetSampleAttachments (bool createIfNecessary)
		{
			var cfArrayRef = CMSampleBufferGetSampleAttachmentsArray (Handle, createIfNecessary.AsByte ());
			if (cfArrayRef == IntPtr.Zero) {
				return Array.Empty<CMSampleBufferAttachmentSettings> ();
			} else {
				return NSArray.ArrayFromHandle (cfArrayRef, h => new CMSampleBufferAttachmentSettings ((NSMutableDictionary) Runtime.GetNSObject (h)!))!;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* size_t */ nuint CMSampleBufferGetSampleSize (/* CMSampleBufferRef */ IntPtr sbuf, /* CMItemIndex */ nint sampleIndex);

		public nuint GetSampleSize (nint sampleIndex)
		{
			return CMSampleBufferGetSampleSize (Handle, sampleIndex);
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleSizeArray (
		   CMSampleBufferRef sbuf,
		   int sizeArrayEntries,
		   uint *sizeArrayOut,
		   int *sizeArrayEntriesNeededOut
		);
		
		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleTimingInfo (
		   CMSampleBufferRef sbuf,
		   int sampleIndex,
		   CMSampleTimingInfo *timingInfoOut
		);
		*/

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern OSStatus CMSampleBufferGetSampleTimingInfoArray (
			/* CMSampleBufferRef */ IntPtr sbuf,
			/* CMItemCount */ nint timingArrayEntries,
			CMSampleTimingInfo* timingArrayOut,
			/* CMItemCount* */ nint* timingArrayEntriesNeededOut
		);

		/// <summary>Fetches the timing information for the sample buffer.</summary>
		///         <returns>An array of CMSampleTimingInfo.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleTimingInfo []? GetSampleTimingInfo ()
		{
			OSStatus status;
			return GetSampleTimingInfo (out status);
		}

#if !XAMCORE_5_0
		// OSStatus was incorrectly defined as IntPtr in this file, so providing this overload to keep compatibility,
		// while at the same time highly discourage using this overload.
		[EditorBrowsable (EditorBrowsableState.Never)]
		[OverloadResolutionPriorityAttribute (-1)]
		public CMSampleTimingInfo []? GetSampleTimingInfo (out nint status)
		{
			var rv = GetSampleTimingInfo (out OSStatus actualStatus);
			status = actualStatus;
			return rv;
		}
#endif // XAMCORE_5_0

		public unsafe CMSampleTimingInfo []? GetSampleTimingInfo (out OSStatus status)
		{
			nint count;

			status = default (OSStatus);

			if (Handle == IntPtr.Zero)
				return null;

			status = CMSampleBufferGetSampleTimingInfoArray (Handle, 0, null, &count);
			if (status != (OSStatus) 0)
				return null;

			CMSampleTimingInfo [] pInfo = new CMSampleTimingInfo [count];

			if (count == 0)
				return pInfo;

			fixed (CMSampleTimingInfo* info = pInfo) {
				status = CMSampleBufferGetSampleTimingInfoArray (Handle, count, info, &count);
				if (status != (OSStatus) 0)
					return null;
			}

			return pInfo;
		}

		static string OSStatusToString (OSStatus status)
		{
			return new NSError (NSError.OsStatusErrorDomain, (nint) status).LocalizedDescription;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* size_t */ nuint CMSampleBufferGetTotalSampleSize (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint TotalSampleSize {
			get {
				return CMSampleBufferGetTotalSampleSize (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFTypeID */ nint CMSampleBufferGetTypeID ();

		/// <summary>Type identifier for the CoreMedia.CMSampleBuffer type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isCMSampleBuffer = (CFType.GetTypeID (foo.Handle) == CMSampleBuffer.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		public static nint GetTypeID ()
		{
			return CMSampleBufferGetTypeID ();
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferInvalidate (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError Invalidate ()
		{
			return CMSampleBufferInvalidate (Handle);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Boolean */ byte CMSampleBufferIsValid (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsValid {
			get {
				return CMSampleBufferIsValid (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferMakeDataReady (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError MakeDataReady ()
		{
			return CMSampleBufferMakeDataReady (Handle);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferSetDataBuffer (IntPtr handle, IntPtr dataBufferHandle);

		/// <param name="dataBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError SetDataBuffer (CMBlockBuffer dataBuffer)
		{
			CMSampleBufferError result = CMSampleBufferSetDataBuffer (Handle, dataBuffer.GetHandle ());
			GC.KeepAlive (dataBuffer);
			return result;
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferSetDataBufferFromAudioBufferList (
		   CMSampleBufferRef sbuf,
		   CFAllocatorRef bbufStructAllocator,
		   CFAllocatorRef bbufMemoryAllocator,
		   uint32_t flags,
		   const AudioBufferList *bufferList
		);*/

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferSetDataReady (/* CMSampleBufferRef */ IntPtr sbuf);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError SetDataReady ()
		{
			return CMSampleBufferSetDataReady (Handle);
		}

#if false
		// new in iOS 8 beta 5 - but the signature is not easy to bind with the AOT limitation, i.e. MonoPInvokeCallback
		[DllImport(Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferSetInvalidateHandler (
			/* CMSampleBufferRef */ IntPtr sbuf,
			/* CMSampleBufferInvalidateHandler */ IntPtr invalidateHandler);
#endif
		// however there was already a similar call that we did not bound (not sure why) 
		// and can provide the same feature (since iOS 4 not 8.0)
		[DllImport (Constants.CoreMediaLibrary)]
		extern unsafe static /* OSStatus */ CMSampleBufferError CMSampleBufferSetInvalidateCallback (
			/* CMSampleBufferRef */ IntPtr sbuf,
			delegate* unmanaged<IntPtr, ulong, void> invalidateCallback,
			/* uint64_t */ ulong invalidateRefCon);

		[UnmanagedCallersOnly]
		static void InvalidateHandler (IntPtr sbuf, ulong invalidateRefCon)
		{
			GCHandle gch = GCHandle.FromIntPtr ((IntPtr) invalidateRefCon);
			var obj = gch.Target as Tuple<Action<CMSampleBuffer>, CMSampleBuffer>;
			if (obj is not null)
				obj.Item1 (obj.Item2);
		}

		/// <param name="invalidateHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError SetInvalidateCallback (Action<CMSampleBuffer> invalidateHandler)
		{
			if (invalidateHandler is null) {
				if (invalidate.IsAllocated)
					invalidate.Free ();
				unsafe {
					return CMSampleBufferSetInvalidateCallback (Handle, null, 0);
				}
			}

			// only one callback can be assigned - and ObjC does not let you re-assign a different one,
			// i.e. it returns RequiredParameterMissing, we could fake this but that would only complexify 
			// porting apps (different behavior)
			if (invalidate.IsAllocated)
				return CMSampleBufferError.RequiredParameterMissing;

			invalidate = GCHandle.Alloc (Tuple.Create (invalidateHandler, this));
			unsafe {
				return CMSampleBufferSetInvalidateCallback (Handle, &InvalidateHandler, (ulong) (IntPtr) invalidate);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferTrackDataReadiness (/* CMSampleBufferRef */ IntPtr sbuf, /* CMSampleBufferRef */ IntPtr sbufToTrack);

		/// <param name="bufferToTrack">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMSampleBufferError TrackDataReadiness (CMSampleBuffer bufferToTrack)
		{
			CMSampleBufferError result = CMSampleBufferTrackDataReadiness (Handle, bufferToTrack.GetHandle ());
			GC.KeepAlive (bufferToTrack);
			return result;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMSampleBufferError CMSampleBufferCopyPCMDataIntoAudioBufferList (/* CMSampleBufferRef */ IntPtr sbuf, /* int32_t */ int frameOffset, /* int32_t */ int numFrames, /* AudioBufferList* */ IntPtr bufferList);

		/// <param name="frameOffset">To be added.</param>
		///         <param name="numFrames">To be added.</param>
		///         <param name="bufferList">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public CMSampleBufferError CopyPCMDataIntoAudioBufferList (int frameOffset, int numFrames, AudioBuffers bufferList)
		{
			if (bufferList is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bufferList));

			return CMSampleBufferCopyPCMDataIntoAudioBufferList (Handle, frameOffset, numFrames, (IntPtr) bufferList);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMSampleBufferError CMAudioSampleBufferCreateReadyWithPacketDescriptions (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CMBlockBufferRef */ IntPtr dataBuffer,
			/* CMFormatDescriptionRef */ IntPtr formatDescription,
			/* CMItemCount */ nint numSamples,
			CMTime sbufPTS,
			/* AudioStreamPacketDescription* */ AudioStreamPacketDescription* packetDescriptions,
			/* CMSampleBufferRef* */ IntPtr* sBufOut);

		/// <param name="dataBuffer">To be added.</param>
		///         <param name="formatDescription">To be added.</param>
		///         <param name="samplesCount">To be added.</param>
		///         <param name="sampleTimestamp">To be added.</param>
		///         <param name="packetDescriptions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMSampleBuffer? CreateReadyWithPacketDescriptions (CMBlockBuffer dataBuffer, CMFormatDescription formatDescription, int samplesCount,
			CMTime sampleTimestamp, AudioStreamPacketDescription []? packetDescriptions, out CMSampleBufferError error)
		{
			if (dataBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataBuffer));
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));
			if (samplesCount <= 0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (samplesCount), "smaller than 0");

			IntPtr buffer;
			unsafe {
				fixed (AudioStreamPacketDescription* packetDescriptionsPtr = packetDescriptions) {
					error = CMAudioSampleBufferCreateReadyWithPacketDescriptions (
								IntPtr.Zero,
								dataBuffer.Handle,
								formatDescription.Handle,
								samplesCount,
								sampleTimestamp,
								packetDescriptionsPtr,
								&buffer);
					GC.KeepAlive (dataBuffer);
					GC.KeepAlive (formatDescription);
				}
			}

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMSampleBufferError CMSampleBufferCreateReady (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CMBlockBufferRef */ IntPtr dataBuffer,
			/* CMFormatDescriptionRef */ IntPtr formatDescription,  // can be null
			/* CMItemCount */ nint numSamples,                      // can be 0
			/* CMItemCount */ nint numSampleTimingEntries,          // 0, 1 or numSamples
			CMSampleTimingInfo* sampleTimingArray,                   // can be null
			/* CMItemCount */ nint numSampleSizeEntries,            // 0, 1 or numSamples
			/* size_t* */ nuint* sampleSizeArray,                    // can be null
			/* CMSampleBufferRef* */ IntPtr* sBufOut);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMSampleBuffer? CreateReady (CMBlockBuffer dataBuffer, CMFormatDescription? formatDescription,
			int samplesCount, CMSampleTimingInfo []? sampleTimingArray, nuint []? sampleSizeArray,
			out CMSampleBufferError error)
		{
			if (dataBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataBuffer));
			if (samplesCount < 0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (samplesCount), "Negative");

			IntPtr buffer;
			var fdh = formatDescription.GetHandle ();
			var timingCount = sampleTimingArray is null ? 0 : sampleTimingArray.Length;
			var sizeCount = sampleSizeArray is null ? 0 : sampleSizeArray.Length;
			unsafe {
				fixed (CMSampleTimingInfo* sampleTimingArrayPtr = sampleTimingArray) {
					fixed (nuint* sampleSizeArrayPtr = sampleSizeArray) {
						error = CMSampleBufferCreateReady (
									IntPtr.Zero,
									dataBuffer.Handle,
									fdh,
									samplesCount,
									timingCount,
									sampleTimingArrayPtr,
									sizeCount,
									sampleSizeArrayPtr,
									&buffer);
						GC.KeepAlive (dataBuffer);
					}
				}
			}
			GC.KeepAlive (formatDescription);

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMSampleBufferError CMSampleBufferCreateReadyWithImageBuffer (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CVImageBufferRef */ IntPtr imageBuffer,
			/* CMFormatDescriptionRef */ IntPtr formatDescription,  // not null
			/* const CMSampleTimingInfo * CM_NONNULL */ CMSampleTimingInfo* sampleTiming,
			/* CMSampleBufferRef* */ IntPtr* sBufOut);

		/// <param name="imageBuffer">To be added.</param>
		///         <param name="formatDescription">To be added.</param>
		///         <param name="sampleTiming">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMSampleBuffer? CreateReadyWithImageBuffer (CVImageBuffer imageBuffer,
			CMFormatDescription formatDescription, ref CMSampleTimingInfo sampleTiming, out CMSampleBufferError error)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));
			if (formatDescription is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (formatDescription));

			IntPtr buffer;
			unsafe {
				error = CMSampleBufferCreateReadyWithImageBuffer (
							IntPtr.Zero,
							imageBuffer.Handle,
							formatDescription.Handle,
							(CMSampleTimingInfo*) Unsafe.AsPointer<CMSampleTimingInfo> (ref sampleTiming),
							&buffer);
				GC.KeepAlive (imageBuffer);
				GC.KeepAlive (formatDescription);
			}

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}
#endif // !COREBUILD
	}

#if !COREBUILD
	public partial class CMSampleBufferAttachmentSettings : DictionaryContainer {

		internal CMSampleBufferAttachmentSettings (NSMutableDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? NotSync {
			get {
				return GetBoolValue (CMSampleAttachmentKey.NotSync);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.NotSync, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? PartialSync {
			get {
				return GetBoolValue (CMSampleAttachmentKey.PartialSync);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.PartialSync, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? RedundantCoding {
			get {
				return GetBoolValue (CMSampleAttachmentKey.HasRedundantCoding);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.HasRedundantCoding, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? DependedOnByOthers {
			get {
				return GetBoolValue (CMSampleAttachmentKey.IsDependedOnByOthers);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.IsDependedOnByOthers, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? DependsOnOthers {
			get {
				return GetBoolValue (CMSampleAttachmentKey.DependsOnOthers);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.DependsOnOthers, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? EarlierDisplayTimesAllowed {
			get {
				return GetBoolValue (CMSampleAttachmentKey.EarlierDisplayTimesAllowed);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.EarlierDisplayTimesAllowed, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? DisplayImmediately {
			get {
				return GetBoolValue (CMSampleAttachmentKey.DisplayImmediately);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.DisplayImmediately, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? DoNotDisplay {
			get {
				return GetBoolValue (CMSampleAttachmentKey.DoNotDisplay);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.DoNotDisplay, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? ResetDecoderBeforeDecoding {
			get {
				return GetBoolValue (CMSampleAttachmentKey.ResetDecoderBeforeDecoding);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.ResetDecoderBeforeDecoding, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? DrainAfterDecoding {
			get {
				return GetBoolValue (CMSampleAttachmentKey.DrainAfterDecoding);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.DrainAfterDecoding, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? Reverse {
			get {
				return GetBoolValue (CMSampleAttachmentKey.Reverse);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.Reverse, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? FillDiscontinuitiesWithSilence {
			get {
				return GetBoolValue (CMSampleAttachmentKey.FillDiscontinuitiesWithSilence);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.FillDiscontinuitiesWithSilence, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? EmptyMedia {
			get {
				return GetBoolValue (CMSampleAttachmentKey.EmptyMedia);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.EmptyMedia, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? PermanentEmptyMedia {
			get {
				return GetBoolValue (CMSampleAttachmentKey.PermanentEmptyMedia);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.PermanentEmptyMedia, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? DisplayEmptyMediaImmediately {
			get {
				return GetBoolValue (CMSampleAttachmentKey.DisplayEmptyMediaImmediately);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.DisplayEmptyMediaImmediately, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? EndsPreviousSampleDuration {
			get {
				return GetBoolValue (CMSampleAttachmentKey.EndsPreviousSampleDuration);
			}
			set {
				SetBooleanValue (CMSampleAttachmentKey.EndsPreviousSampleDuration, value);
			}
		}

#if !MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public string? DroppedFrameReason {
			get {
				return GetStringValue (CMSampleAttachmentKey.DroppedFrameReason);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public LensStabilizationStatus StillImageLensStabilizationStatus {
			get {
				var reason = GetStringValue (CMSampleAttachmentKey.StillImageLensStabilizationInfo);
				if (reason is null)
					return LensStabilizationStatus.None;

				if (reason == CMSampleAttachmentKey.BufferLensStabilizationInfo_Active)
					return LensStabilizationStatus.Active;
				if (reason == CMSampleAttachmentKey.BufferLensStabilizationInfo_OutOfRange)
					return LensStabilizationStatus.OutOfRange;
				if (reason == CMSampleAttachmentKey.BufferLensStabilizationInfo_Unavailable)
					return LensStabilizationStatus.Unavailable;
				if (reason == CMSampleAttachmentKey.BufferLensStabilizationInfo_Off)
					return LensStabilizationStatus.Off;

				return LensStabilizationStatus.None;
			}
		}
#endif // !MONOMAC
	}
#endif
}
