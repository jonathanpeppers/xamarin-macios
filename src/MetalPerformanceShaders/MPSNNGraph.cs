#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalPerformanceShaders {
	public partial class MPSNNGraph {
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe static MPSNNGraph? Create (IMTLDevice device, MPSNNImageNode [] resultImages, bool [] resultsAreNeeded)
		{
			fixed (void* resultsAreNeededHandle = resultsAreNeeded)
				return Create (device, resultImages, (IntPtr) resultsAreNeededHandle);
		}
	}
}
