// 
// CMTime.cs: API for creating and manipulating CMTime structs
//
// Authors: Mono Team
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreMedia {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public partial struct CMTime {
		// CMTimeFlags -> uint32_t -> CMTime.h
		[Flags]
		public enum Flags : uint {
			/// <summary>To be added.</summary>
			Valid = 1,
			/// <summary>To be added.</summary>
			HasBeenRounded = 2,
			/// <summary>To be added.</summary>
			PositiveInfinity = 4,
			/// <summary>To be added.</summary>
			NegativeInfinity = 8,
			/// <summary>To be added.</summary>
			Indefinite = 16,
			/// <summary>To be added.</summary>
			ImpliedValueFlagsMask = PositiveInfinity | NegativeInfinity | Indefinite,
		}
#if !COREBUILD

		/// <summary>Constant that contains an invalid CMTime.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static CMTime Invalid = new CMTime (0);

		const Flags kIndefinite = Flags.Valid | Flags.Indefinite;
		/// <summary>Constant that contains an indefinite CMTime.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static CMTime Indefinite = new CMTime (kIndefinite);

		const Flags kPositive = Flags.Valid | Flags.PositiveInfinity;
		/// <summary>Constant that contains a positive infinity CMTime.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static CMTime PositiveInfinity = new CMTime (kPositive);

		const Flags kNegative = Flags.Valid | Flags.NegativeInfinity;
		/// <summary>Constant that contains a negative infinity CMTime.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static CMTime NegativeInfinity = new CMTime (kNegative);

		/// <summary>Constant that contains a zero CMTime.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static CMTime Zero = new CMTime (Flags.Valid, 1);

		/// <summary>Maximum supported time scale.</summary>
		///         <remarks>To be added.</remarks>
		public const int MaxTimeScale = 0x7fffffff;

#endif // !COREBUILD

		// CMTimeValue -> int64_t -> CMTime.h
		/// <summary>Value component of a CMTime (the numerator).</summary>
		///         <remarks>To be added.</remarks>
		public long Value;

		// CMTimeScale -> int32_t -> CMTime.h
		/// <summary>Timescale for the CMTime (the denominator).</summary>
		///         <remarks>To be added.</remarks>
		public int TimeScale;

		// CMTimeFlags -> uint32_t -> CMTime.h
		/// <summary>Flags set on the CMTime</summary>
		///         <remarks>To be added.</remarks>
		public Flags TimeFlags;

		// CMTimeEpoch -> int64_t -> CMTime.h
		/// <summary>The time epoch.</summary>
		///         <remarks>To be added.</remarks>
		public long TimeEpoch;

#if !COREBUILD
		CMTime (Flags f)
		{
			Value = 0;
			TimeScale = 0;
			TimeEpoch = 0;
			TimeFlags = f;
		}

		CMTime (Flags f, int timescale)
		{
			Value = 0;
			TimeScale = timescale;
			TimeEpoch = 0;
			TimeFlags = f;
		}

		public CMTime (long value, int timescale)
		{
			Value = value;
			TimeScale = timescale;
			TimeFlags = Flags.Valid;
			TimeEpoch = 0;
		}

		public CMTime (long value, int timescale, long epoch)
		{
			Value = value;
			TimeScale = timescale;
			TimeFlags = Flags.Valid;
			TimeEpoch = epoch;
		}

		/// <summary>Indicates that the CMTime value stored is invalid.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsInvalid {
			get {
				return (TimeFlags & Flags.Valid) == 0;
			}
		}

		/// <summary>Indicates if the CMTime is numeric.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsNumeric {
			get {
				return ((TimeFlags & (Flags.Valid | Flags.ImpliedValueFlagsMask)) == Flags.Valid);
			}

		}

		/// <summary>Indicates that the CMTime was rounded.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool HasBeenRounded {
			get {
				return IsNumeric && ((TimeFlags & Flags.HasBeenRounded) != 0);
			}
		}

		/// <summary>Indicates that the CMTime is indefinite.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsIndefinite {
			get {
				return (TimeFlags & kIndefinite) == kIndefinite;
			}
		}

		/// <summary>Indicates if the CMTime represents positive infinity.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsPositiveInfinity {
			get {
				return (TimeFlags & kPositive) == kPositive;
			}
		}

		/// <summary>Indicates if the CMTime represents negative infinity.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsNegativeInfinity {
			get {
				return (TimeFlags & kNegative) == kNegative;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeAbsoluteValue (CMTime time);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime AbsoluteValue {
			get {
				return CMTimeAbsoluteValue (this);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* int32_t */ int CMTimeCompare (CMTime time1, CMTime time2);

		public static int Compare (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2);
		}

		public static bool operator == (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2) == 0;
		}

		public static bool operator != (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2) != 0;
		}

		public static bool operator < (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2) == -1;
		}

		public static bool operator <= (CMTime time1, CMTime time2)
		{
			var comp = CMTimeCompare (time1, time2);
			return comp <= 0;
		}

		public static bool operator > (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2) == 1;
		}

		public static bool operator >= (CMTime time1, CMTime time2)
		{
			var comp = CMTimeCompare (time1, time2);
			return comp >= 0;
		}

		public override bool Equals (object? obj)
		{
			if (obj is CMTime time)
				return CMTimeCompare (this, time) == 0;
			return false;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (Value, TimeScale, TimeFlags, TimeEpoch);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeAdd (CMTime addend1, CMTime addend2);

		public static CMTime Add (CMTime time1, CMTime time2)
		{
			return CMTimeAdd (time1, time2);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeSubtract (CMTime minuend, CMTime subtrahend);

		public static CMTime Subtract (CMTime minuend, CMTime subtraend)
		{
			return CMTimeSubtract (minuend, subtraend);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiply (CMTime time, /* int32_t */ int multiplier);

		public static CMTime Multiply (CMTime time, int multiplier)
		{
			return CMTimeMultiply (time, multiplier);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiplyByFloat64 (CMTime time, /* Float64 */ double multiplier);

		public static CMTime Multiply (CMTime time, double multiplier)
		{
			return CMTimeMultiplyByFloat64 (time, multiplier);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiplyByRatio (CMTime time, /* int32_t */ int multiplier, /* int32_t */ int divisor);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMTime Multiply (CMTime time, int multiplier, int divisor)
		{
			return CMTimeMultiplyByRatio (time, multiplier, divisor);
		}

		public static CMTime operator + (CMTime time1, CMTime time2)
		{
			return Add (time1, time2);
		}

		public static CMTime operator - (CMTime minuend, CMTime subtraend)
		{
			return Subtract (minuend, subtraend);
		}

		public static CMTime operator * (CMTime time, int multiplier)
		{
			return Multiply (time, multiplier);
		}

		public static CMTime operator * (CMTime time, double multiplier)
		{
			return Multiply (time, multiplier);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeConvertScale (CMTime time, /* int32_t */ int newScale, CMTimeRoundingMethod method);

		public CMTime ConvertScale (int newScale, CMTimeRoundingMethod method)
		{
			return CMTimeConvertScale (this, newScale, method);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Float64 */ double CMTimeGetSeconds (CMTime time);

		/// <summary>Number of seconds</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Seconds {
			get {
				return CMTimeGetSeconds (this);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMakeWithSeconds (/* Float64 */ double seconds, /* int32_t */ int preferredTimeScale);

		public static CMTime FromSeconds (double seconds, int preferredTimeScale)
		{
			return CMTimeMakeWithSeconds (seconds, preferredTimeScale);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMaximum (CMTime time1, CMTime time2);

		public static CMTime GetMaximum (CMTime time1, CMTime time2)
		{
			return CMTimeMaximum (time1, time2);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMinimum (CMTime time1, CMTime time2);

		public static CMTime GetMinimum (CMTime time1, CMTime time2)
		{
			return CMTimeMinimum (time1, time2);
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeFoldIntoRange (CMTime time, CMTimeRange foldRange);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CMTime Fold (CMTime time, CMTimeRange foldRange)
		{
			return CMTimeFoldIntoRange (time, foldRange);
		}

		// FIXME: generated will need some changes to emit [Field] in partial struct (not class)
		/// <summary>Key that can be used in NSDictionary objects returned by CMTime to extract the Value property.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static NSString? ValueKey;
		/// <summary>Key that can be used in NSDictionary objects returned by CMTime to extract the Scale property.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static NSString? ScaleKey;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static NSString? EpochKey;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static NSString? FlagsKey;

		static CMTime ()
		{
			var lib = Libraries.CoreMedia.Handle;
			ValueKey = Dlfcn.GetStringConstant (lib, "kCMTimeValueKey");
			ScaleKey = Dlfcn.GetStringConstant (lib, "kCMTimeScaleKey");
			EpochKey = Dlfcn.GetStringConstant (lib, "kCMTimeEpochKey");
			FlagsKey = Dlfcn.GetStringConstant (lib, "kCMTimeFlagsKey");
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFDictionaryRef */ IntPtr CMTimeCopyAsDictionary (CMTime time, /* CFAllocatorRef */ IntPtr allocator);

		public NSDictionary ToDictionary ()
		{
			return new NSDictionary (CMTimeCopyAsDictionary (this, IntPtr.Zero), true);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFStringRef */ IntPtr CMTimeCopyDescription (/* CFAllocatorRef */ IntPtr allocator, CMTime time);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Description {
			get {
				return CFString.FromHandle (CMTimeCopyDescription (IntPtr.Zero, this));
			}
		}

		public override string? ToString ()
		{
			return Description;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMakeFromDictionary (/* CFDictionaryRef */ IntPtr dict);

		public static CMTime FromDictionary (NSDictionary dict)
		{
			if (dict is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dict));
			return CMTimeMakeFromDictionary (dict.Handle);
		}
#endif // !COREBUILD
	}
}
