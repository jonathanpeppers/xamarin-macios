// 
// CTFont.cs: Implements the managed CTFont
//
// Authors: Mono Team
//          Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2010 Novell, Inc
// Copyright 2011 - 2014 Xamarin Inc
// Copyright 2019 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

using ObjCRuntime;
using CoreFoundation;
using CoreGraphics;
using Foundation;

using CGGlyph = System.UInt16;

namespace CoreText {

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontManager.h
	public enum CTFontManagerScope : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Process = 1,
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		Persistent = 2,
		/// <summary>To be added.</summary>
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		Session = 3,
	}

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontManager.h
	public enum CTFontManagerAutoActivation : uint {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Disabled = 1,
		/// <summary>To be added.</summary>
		Enabled = 2,
		/// <summary>Developers should not use this deprecated field. It's now treated as 'Default'.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios", "It's now treated as 'Default'.")]
		[ObsoletedOSPlatform ("maccatalyst", "It's now treated as 'Default'.")]
		[ObsoletedOSPlatform ("tvos", "It's now treated as 'Default'.")]
		[ObsoletedOSPlatform ("macos10.13", "It's now treated as 'Default'.")]
		PromptUser = 3,
	}

	public partial class CTFontManager {

#if MONOMAC
		[DllImport (Constants.CoreTextLibrary)]
		static extern byte CTFontManagerIsSupportedFont (IntPtr url);

		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.6")]
		public static bool IsFontSupported (NSUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			bool result = CTFontManagerIsSupportedFont (url.Handle) != 0;
			GC.KeepAlive (url);
			return result;
		}
#endif

		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern byte CTFontManagerRegisterFontsForURL (IntPtr fontUrl, CTFontManagerScope scope, IntPtr* error);
		public static NSError? RegisterFontsForUrl (NSUrl fontUrl, CTFontManagerScope scope)
		{
			if (fontUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fontUrl));

			IntPtr error = IntPtr.Zero;

			try {
				bool rv;
				unsafe {
					rv = CTFontManagerRegisterFontsForURL (fontUrl.Handle, scope, &error) != 0;
					GC.KeepAlive (fontUrl);
				}
				if (rv)
					return null;
				else
					return Runtime.GetNSObject<NSError> (error);
			} finally {
				if (error != IntPtr.Zero)
					CFObject.CFRelease (error);
			}
		}

		static NSArray EnsureNonNullArray (object [] items, string name)
		{
			if (items is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			foreach (var i in items)
				if (i is null)
					throw new ArgumentException ("Array contains a null entry", name);

			return NSArray.FromObjects (items);
		}

		static T []? ArrayFromHandle<T> (IntPtr handle, bool releaseAfterUse) where T : class, INativeObject
		{
			if (handle == IntPtr.Zero)
				return null;
			try {
				return NSArray.ArrayFromHandle<T> (handle);
			} finally {
				if (releaseAfterUse)
					CFObject.CFRetain (handle);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15")]
		[ObsoletedOSPlatform ("tvos13.0")]
		[ObsoletedOSPlatform ("ios13.0")]
		[ObsoletedOSPlatform ("maccatalyst13.1")]
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern byte CTFontManagerRegisterFontsForURLs (IntPtr arrayRef, CTFontManagerScope scope, IntPtr* error_array);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'RegisterFonts' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'RegisterFonts' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'RegisterFonts' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'RegisterFonts' instead.")]
		public static NSError []? RegisterFontsForUrl (NSUrl [] fontUrls, CTFontManagerScope scope)
		{
			using (var arr = EnsureNonNullArray (fontUrls, nameof (fontUrls))) {
				IntPtr error_array = IntPtr.Zero;
				unsafe {
					if (CTFontManagerRegisterFontsForURLs (arr.Handle, scope, &error_array) != 0)
						return null;
					GC.KeepAlive (arr);
				}
				return ArrayFromHandle<NSError> (error_array, releaseAfterUse: true);
			}
		}

		public delegate bool CTFontRegistrationHandler (NSError [] errors, bool done);

		[UnmanagedCallersOnly]
		static unsafe byte TrampolineRegistrationHandler (IntPtr block, /* NSArray */ IntPtr errors, byte done)
		{
			var del = BlockLiteral.GetTarget<CTFontRegistrationHandler> (block);
			if (del is null)
				return 0;

			var rv = del (NSArray.ArrayFromHandle<NSError> (errors), done == 0 ? false : true);
			return rv ? (byte) 1 : (byte) 0;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern void CTFontManagerRegisterFontURLs (/* CFArrayRef */ IntPtr fontUrls, CTFontManagerScope scope, byte enabled, BlockLiteral* registrationHandler);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void RegisterFonts (NSUrl [] fontUrls, CTFontManagerScope scope, bool enabled, CTFontRegistrationHandler registrationHandler)
		{
			using (var arr = EnsureNonNullArray (fontUrls, nameof (fontUrls))) {
				if (registrationHandler is null) {
					unsafe {
						CTFontManagerRegisterFontURLs (arr.Handle, scope, enabled.AsByte (), null);
						GC.KeepAlive (arr);
					}
				} else {
					unsafe {
						delegate* unmanaged<IntPtr, IntPtr, byte, byte> trampoline = &TrampolineRegistrationHandler;
						using var block = new BlockLiteral (trampoline, registrationHandler, typeof (CTFontManager), nameof (TrampolineRegistrationHandler));
						CTFontManagerRegisterFontURLs (arr.Handle, scope, enabled.AsByte (), &block);
					}
				}
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern byte CTFontManagerUnregisterFontsForURL (IntPtr fotUrl, CTFontManagerScope scope, IntPtr* error);

		public static NSError? UnregisterFontsForUrl (NSUrl fontUrl, CTFontManagerScope scope)
		{
			if (fontUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fontUrl));

			IntPtr error = IntPtr.Zero;

			try {
				bool rv;
				unsafe {
					rv = CTFontManagerUnregisterFontsForURL (fontUrl.Handle, scope, &error) != 0;
					GC.KeepAlive (fontUrl);
				}
				if (rv)
					return null;
				else
					return Runtime.GetNSObject<NSError> (error);
			} finally {
				if (error != IntPtr.Zero)
					CFObject.CFRelease (error);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15")]
		[ObsoletedOSPlatform ("tvos13.0")]
		[ObsoletedOSPlatform ("ios13.0")]
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern byte CTFontManagerUnregisterFontsForURLs (IntPtr arrayRef, CTFontManagerScope scope, IntPtr* error_array);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'UnregisterFonts' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'UnregisterFonts' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'UnregisterFonts' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'UnregisterFonts' instead.")]
		public static NSError []? UnregisterFontsForUrl (NSUrl [] fontUrls, CTFontManagerScope scope)
		{
			IntPtr error_array = IntPtr.Zero;
			using (var arr = EnsureNonNullArray (fontUrls, nameof (fontUrls))) {
				unsafe {
					if (CTFontManagerUnregisterFontsForURLs (arr.Handle, scope, &error_array) != 0)
						return null;
					GC.KeepAlive (arr);
				}
				return ArrayFromHandle<NSError> (error_array, releaseAfterUse: true);
			}
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe void CTFontManagerUnregisterFontURLs (/* CFArrayRef */ IntPtr fontUrls, CTFontManagerScope scope, BlockLiteral* registrationHandler);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static void UnregisterFonts (NSUrl [] fontUrls, CTFontManagerScope scope, CTFontRegistrationHandler registrationHandler)
		{
			using (var arr = EnsureNonNullArray (fontUrls, nameof (fontUrls))) {
				if (registrationHandler is null) {
					CTFontManagerUnregisterFontURLs (arr.Handle, scope, null);
					GC.KeepAlive (arr);
				} else {
					delegate* unmanaged<IntPtr, IntPtr, byte, byte> trampoline = &TrampolineRegistrationHandler;
					using var block = new BlockLiteral (trampoline, registrationHandler, typeof (CTFontManager), nameof (TrampolineRegistrationHandler));
					CTFontManagerUnregisterFontURLs (arr.Handle, scope, &block);
					GC.KeepAlive (arr);
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern /* CFArrayRef */ IntPtr CTFontManagerCreateFontDescriptorsFromURL (/* CFURLRef */ IntPtr fileURL);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public static CTFontDescriptor [] GetFonts (NSUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var arrayPtr = CTFontManagerCreateFontDescriptorsFromURL (url.Handle);
			GC.KeepAlive (url);
			if (arrayPtr == IntPtr.Zero)
				return new CTFontDescriptor [0];

			using (var unmanageFonts = new CFArray (arrayPtr, true)) {
				var managedFonts = new CTFontDescriptor [unmanageFonts.Count];
				for (int index = 0; index < unmanageFonts.Count; index++) {
					managedFonts [index] = new CTFontDescriptor (unmanageFonts.GetValue (index), false);
				}
				return managedFonts;
			}
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos15.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[ObsoletedOSPlatform ("tvos18.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[ObsoletedOSPlatform ("ios18.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[ObsoletedOSPlatform ("maccatalyst18.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern byte CTFontManagerRegisterGraphicsFont (IntPtr cgfont, IntPtr* error);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos15.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[ObsoletedOSPlatform ("tvos18.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[ObsoletedOSPlatform ("ios18.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		[ObsoletedOSPlatform ("maccatalyst18.0", "Use 'CreateFontDescriptors' or 'RegisterFontsForUrl' instead.")]
		public static bool RegisterGraphicsFont (CGFont font, [NotNullWhen (true)] out NSError? error)
		{
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			IntPtr h = IntPtr.Zero;
			bool ret;
			try {
				unsafe {
					ret = CTFontManagerRegisterGraphicsFont (font.Handle, &h) != 0;
					GC.KeepAlive (font);
				}
				if (ret)
					error = null;
				else
					error = new NSError (h);
			} finally {
				if (h != IntPtr.Zero)
					CFObject.CFRelease (h);
			}
			return ret;
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos15.0")]
		[ObsoletedOSPlatform ("tvos18.0")]
		[ObsoletedOSPlatform ("ios18.0")]
		[ObsoletedOSPlatform ("maccatalyst18.0")]
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern byte CTFontManagerUnregisterGraphicsFont (IntPtr cgfont, IntPtr* error);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos15.0")]
		[ObsoletedOSPlatform ("tvos18.0")]
		[ObsoletedOSPlatform ("ios18.0")]
		[ObsoletedOSPlatform ("maccatalyst18.0")]
		public static bool UnregisterGraphicsFont (CGFont font, out NSError? error)
		{
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			IntPtr h = IntPtr.Zero;
			bool ret;
			try {
				unsafe {
					ret = CTFontManagerUnregisterGraphicsFont (font.Handle, &h) != 0;
					GC.KeepAlive (font);
				}
				if (ret)
					error = null;
				else
					error = new NSError (h);
			} finally {
				if (h != IntPtr.Zero)
					CFObject.CFRelease (h);
			}
			return ret;
		}

		static NSString? _RegisteredFontsChangedNotification;

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		static NSString? RegisteredFontsChangedNotification {
			get {
				if (_RegisteredFontsChangedNotification is null)
					_RegisteredFontsChangedNotification = Dlfcn.GetStringConstant (Libraries.CoreText.Handle, "kCTFontManagerRegisteredFontsChangedNotification");
				return _RegisteredFontsChangedNotification;
			}
		}

		public static partial class Notifications {
			public static NSObject ObserveRegisteredFontsChanged (EventHandler<NSNotificationEventArgs> handler)
			{
				return NSNotificationCenter.DefaultCenter.AddObserver (RegisteredFontsChangedNotification, notification => handler (null, new NSNotificationEventArgs (notification)));
			}

		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe void CTFontManagerRegisterFontDescriptors (/* CFArrayRef */ IntPtr fontDescriptors, CTFontManagerScope scope, byte enabled, BlockLiteral* registrationHandler);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static void RegisterFontDescriptors (CTFontDescriptor [] fontDescriptors, CTFontManagerScope scope, bool enabled, CTFontRegistrationHandler registrationHandler)
		{
			using (var arr = EnsureNonNullArray (fontDescriptors, nameof (fontDescriptors))) {
				if (registrationHandler is null) {
					CTFontManagerRegisterFontDescriptors (arr.Handle, scope, enabled.AsByte (), null);
					GC.KeepAlive (arr);
				} else {
					delegate* unmanaged<IntPtr, IntPtr, byte, byte> trampoline = &TrampolineRegistrationHandler;
					using var block = new BlockLiteral (trampoline, registrationHandler, typeof (CTFontManager), nameof (TrampolineRegistrationHandler));
					CTFontManagerRegisterFontDescriptors (arr.Handle, scope, enabled.AsByte (), &block);
					GC.KeepAlive (arr);
				}
			}
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe void CTFontManagerUnregisterFontDescriptors (/* CFArrayRef */ IntPtr fontDescriptors, CTFontManagerScope scope, BlockLiteral* registrationHandler);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static void UnregisterFontDescriptors (CTFontDescriptor [] fontDescriptors, CTFontManagerScope scope, CTFontRegistrationHandler registrationHandler)
		{
			using (var arr = EnsureNonNullArray (fontDescriptors, nameof (fontDescriptors))) {
				if (registrationHandler is null) {
					CTFontManagerUnregisterFontDescriptors (arr.Handle, scope, null);
					GC.KeepAlive (arr);
				} else {
					delegate* unmanaged<IntPtr, IntPtr, byte, byte> trampoline = &TrampolineRegistrationHandler;
					using var block = new BlockLiteral (trampoline, registrationHandler, typeof (CTFontManager), nameof (TrampolineRegistrationHandler));
					CTFontManagerUnregisterFontDescriptors (arr.Handle, scope, &block);
					GC.KeepAlive (arr);
				}
			}
		}

#if __IOS__
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern /* CFArrayRef */ IntPtr CTFontManagerCopyRegisteredFontDescriptors (CTFontManagerScope scope, byte enabled);

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public static CTFontDescriptor []? GetRegisteredFontDescriptors (CTFontManagerScope scope, bool enabled)
		{
			var p = CTFontManagerCopyRegisteredFontDescriptors (scope, enabled.AsByte ());
			// Copy/Create rule - we must release the CFArrayRef
			return ArrayFromHandle<CTFontDescriptor> (p, releaseAfterUse: true);
		}
#endif

		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe /* CTFontDescriptorRef _Nullable */ IntPtr CTFontManagerCreateFontDescriptorFromData (/* CFDataRef */ IntPtr data);

		public static CTFontDescriptor? CreateFontDescriptor (NSData data)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));

			var p = CTFontManagerCreateFontDescriptorFromData (data.Handle);
			GC.KeepAlive (data);
			if (p == IntPtr.Zero)
				return null;
			// Copy/Create rule - dont retain it inside the .ctor
			return new CTFontDescriptor (p, owns: true);
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe /* CFArrayRef */ IntPtr CTFontManagerCreateFontDescriptorsFromData (/* CFDataRef */ IntPtr data);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CTFontDescriptor []? CreateFontDescriptors (NSData data)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));

			var p = CTFontManagerCreateFontDescriptorsFromData (data.Handle);
			GC.KeepAlive (data);
			// Copy/Create rule - we must release the CFArrayRef
			return ArrayFromHandle<CTFontDescriptor> (p, releaseAfterUse: true);
		}

#if __IOS__
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe void CTFontManagerRegisterFontsWithAssetNames (/* CFArrayRef */ IntPtr fontAssetNames, /* CFBundleRef _Nullable */ IntPtr bundle, CTFontManagerScope scope, byte enabled, BlockLiteral* registrationHandler);

		// reminder that NSBundle and CFBundle are NOT toll-free bridged :(
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static void RegisterFonts (string [] assetNames, CFBundle bundle, CTFontManagerScope scope, bool enabled, CTFontRegistrationHandler registrationHandler)
		{
			using (var arr = EnsureNonNullArray (assetNames, nameof (assetNames))) {
				if (registrationHandler is null) {
					CTFontManagerRegisterFontsWithAssetNames (arr.Handle, bundle.GetHandle (), scope, enabled.AsByte (), null);
					GC.KeepAlive (arr);
					GC.KeepAlive (bundle);
				} else {
					delegate* unmanaged<IntPtr, IntPtr, byte, byte> trampoline = &TrampolineRegistrationHandler;
					using var block = new BlockLiteral (trampoline, registrationHandler, typeof (CTFontManager), nameof (TrampolineRegistrationHandler));
					CTFontManagerRegisterFontsWithAssetNames (arr.Handle, bundle.GetHandle (), scope, enabled.AsByte (), &block);
					GC.KeepAlive (arr);
					GC.KeepAlive (bundle);
				}
			}
		}

		public delegate void CTFontManagerRequestFontsHandler (CTFontDescriptor [] unresolvedFontDescriptors);

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern unsafe void CTFontManagerRequestFonts (/* CFArrayRef */ IntPtr fontDescriptors, BlockLiteral* completionHandler);

		[UnmanagedCallersOnly]
		static unsafe void TrampolineRequestFonts (IntPtr block, /* CFArray */ IntPtr fontDescriptors)
		{
			var del = BlockLiteral.GetTarget<CTFontManagerRequestFontsHandler> (block);
			if (del is not null)
				del (NSArray.ArrayFromHandle<CTFontDescriptor> (fontDescriptors));
		}

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void RequestFonts (CTFontDescriptor [] fontDescriptors, CTFontManagerRequestFontsHandler completionHandler)
		{
			if (completionHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (completionHandler));

			using (var arr = EnsureNonNullArray (fontDescriptors, nameof (fontDescriptors))) {
				unsafe {
					delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &TrampolineRequestFonts;
					using var block = new BlockLiteral (trampoline, completionHandler, typeof (CTFontManager), nameof (TrampolineRequestFonts));
					CTFontManagerRequestFonts (arr.Handle, &block);
				}
			}
		}
#endif
	}
}
