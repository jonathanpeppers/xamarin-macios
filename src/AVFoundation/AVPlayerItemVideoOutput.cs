using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using AudioToolbox;

#nullable enable

namespace AVFoundation {
	public partial class AVPlayerItemVideoOutput {

		enum InitMode {
			PixelAttributes,
			OutputSettings,
		}

		AVPlayerItemVideoOutput (NSDictionary? data, AVPlayerItemVideoOutput.InitMode mode) : base (NSObjectFlag.Empty)
		{
			switch (mode) {
			case InitMode.PixelAttributes:
				InitializeHandle (_FromPixelBufferAttributes (data), "initWithPixelBufferAttributes:");
				break;
			case InitMode.OutputSettings:
				InitializeHandle (_FromOutputSettings (data), "initWithOutputSettings:");
				break;
			default:
				throw new ArgumentException (nameof (mode));
			}
		}

		/// <param name="pixelBufferAttributes">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Advice ("Please use the constructor that uses one of the available StrongDictionaries. This constructor expects PixelBuffer attributes.")]
		protected AVPlayerItemVideoOutput (NSDictionary pixelBufferAttributes) : this (pixelBufferAttributes, InitMode.PixelAttributes) { }
	}
}
