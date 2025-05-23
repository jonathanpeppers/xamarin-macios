// 
// CVPixelBufferIOSurface.cs
//
// Authors: Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2017 Xamarin Inc.
//

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreVideo {
	public partial class CVPixelBuffer : CVImageBuffer {

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr /* IOSurfaceRef */ CVPixelBufferGetIOSurface (
			/* CVPixelBufferRef CV_NULLABLE */ IntPtr pixelBuffer
		);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public IOSurface.IOSurface? GetIOSurface ()
		{
			if (Handle == IntPtr.Zero)
				throw new ObjectDisposedException ("CVPixelBuffer");

			var ret = CVPixelBufferGetIOSurface (Handle);
			if (ret == IntPtr.Zero)
				return null;

			return Runtime.GetINativeObject<IOSurface.IOSurface> (ret, false);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		unsafe extern static CVReturn /* IOSurfaceRef */ CVPixelBufferCreateWithIOSurface (
			/* CFAllocatorRef CV_NULLABLE */ IntPtr allocator,
			/* IOSurfaceRef CV_NONNULL */ IntPtr surface,
			/* CFDictionaryRef CV_NULLABLE */ IntPtr pixelBufferAttributes,
			/* CVPixelBufferRef CV_NULLABLE * CV_NONNULL */ IntPtr* pixelBufferOut
		);

		/// <param name="surface">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <param name="pixelBufferAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CVPixelBuffer? Create (IOSurface.IOSurface surface, out CVReturn result, CVPixelBufferAttributes? pixelBufferAttributes = null)
		{
			if (surface is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (surface));

			IntPtr pixelBufferPtr;
			unsafe {
				var pixelBufferAttributesDict = pixelBufferAttributes?.Dictionary;
				result = CVPixelBufferCreateWithIOSurface (
					allocator: IntPtr.Zero,
					surface: surface.Handle,
					pixelBufferAttributes: pixelBufferAttributesDict?.Handle ?? IntPtr.Zero,
					pixelBufferOut: &pixelBufferPtr
				);
				GC.KeepAlive (surface);
				GC.KeepAlive (pixelBufferAttributesDict);
			}

			if (result != CVReturn.Success)
				return null;

			return new CVPixelBuffer (pixelBufferPtr, true);
		}

		/// <param name="surface">To be added.</param>
		///         <param name="pixelBufferAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CVPixelBuffer? Create (IOSurface.IOSurface surface, CVPixelBufferAttributes? pixelBufferAttributes = null)
		{
			CVReturn result;
			return Create (surface, out result, pixelBufferAttributes);
		}
	}
}
