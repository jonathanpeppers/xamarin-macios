//
// social.cs: API definition for Apple's Social Framework
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012-2013 Xamarin Inc
//

using System;
using ObjCRuntime;
using Foundation;
using Accounts;

#if !MONOMAC
using UIKit;
using SocialImage = UIKit.UIImage;
using SocialTextView = UIKit.UITextView;
using SocialTextViewDelegate = UIKit.UITextViewDelegate;
using SocialView = UIKit.UIView;
using SocialViewController = UIKit.UIViewController;
#endif
#if MONOMAC
using AppKit;
using SocialImage = AppKit.NSImage;
using SocialTextView = AppKit.NSTextView;
using SocialTextViewDelegate = AppKit.NSTextViewDelegate;
using SocialView = AppKit.NSView;
using SocialViewController = AppKit.NSViewController;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Social {
	/// <summary>NSString constants with the various service types supported by the Social framework</summary>
	///     <remarks>These constants are used typically when interacting with low-level Objective-C APIs.   In general, you can just use the higher level APIs that use strongly typed enumerations of type <see cref="Social.SLServiceKind" />.</remarks>
	[Static]
	interface SLServiceType {
		/// <summary>Developers should not use this deprecated property. Developers should use Facebook SDK instead.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Facebook SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Facebook SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Facebook SDK instead.")]
		[Field ("SLServiceTypeFacebook")]
		NSString Facebook { get; }

		/// <summary>Represents the value associated with the constant SLServiceTypeTwitter</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Twitter SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Twitter SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Twitter SDK instead.")]
		[Field ("SLServiceTypeTwitter")]
		NSString Twitter { get; }

		/// <summary>Represents the value associated with the constant SLServiceTypeSinaWeibo</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Sina Weibo SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Sina Weibo SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Sina Weibo SDK instead.")]
		[Field ("SLServiceTypeSinaWeibo")]
		NSString SinaWeibo { get; }

		/// <summary>Represents the value associated with the constant SLServiceTypeTencentWeibo</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Tencent Weibo SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Tencent Weibo SDK instead.")]
		[Field ("SLServiceTypeTencentWeibo")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Tencent Weibo SDK instead.")]
		NSString TencentWeibo { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use LinkedIn SDK instead.")]
		[Field ("SLServiceTypeLinkedIn")]
		[NoiOS]
		[NoMacCatalyst]
		NSString LinkedIn { get; }
	}

	/// <summary>Enumeration with the various kinds of social services that can be used.</summary>
	/// <remarks>This enumeration is used to map into the underlying set of services offered by the social framework.   It is intended to assist code completion while developing and take the gueswork out of using the framework in some entry points that take an NSString as a parameter.</remarks>
	enum SLServiceKind {
		/// <summary>Facebook services</summary>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Facebook SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Facebook SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Facebook SDK instead.")]
		[Field ("SLServiceTypeFacebook")]
		Facebook,

		/// <summary>Twitter service.</summary>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Twitter SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Twitter SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Twitter SDK instead.")]
		[Field ("SLServiceTypeTwitter")]
		Twitter,

		/// <summary>SinaWeibo service</summary>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Sina Weibo SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Sina Weibo SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Sina Weibo SDK instead.")]
		[Field ("SLServiceTypeSinaWeibo")]
		SinaWeibo,

		/// <summary>TencentWeibo service.</summary>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Tencent Weibo SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Tencent Weibo SDK instead.")]
		[Field ("SLServiceTypeTencentWeibo")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Tencent Weibo SDK instead.")]
		TencentWeibo,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use LinkedIn SDK instead.")]
		[Field ("SLServiceTypeLinkedIn")]
		[NoiOS]
		[NoMacCatalyst]
		LinkedIn,
	}

	/// <summary>A request made to a social service.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Social/Reference/SLRequest_Class/index.html">Apple documentation for <c>SLRequest</c></related>
	[BaseType (typeof (NSObject))]
	// init -> Objective-C exception thrown.  Name: NSInternalInconsistencyException Reason: SLRequestMultiPart must be obtained through!
	[DisableDefaultCtor]
	interface SLRequest {
		[Static]
		[Export ("requestForServiceType:requestMethod:URL:parameters:")]
		SLRequest Create (NSString serviceType, SLRequestMethod requestMethod, NSUrl url, [NullAllowed] NSDictionary parameters);

		/// <param name="serviceKind">To be added.</param>
		///         <param name="method">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="parameters">To be added.</param>
		///         <summary>Creates a new request object with the specified values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("Create (serviceKind.GetConstant ()!, method, url, parameters)")]
		SLRequest Create (SLServiceKind serviceKind, SLRequestMethod method, NSUrl url, [NullAllowed] NSDictionary parameters);

		[Deprecated (PlatformName.iOS, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
		[Export ("account", ArgumentSemantic.Retain), NullAllowed]
		ACAccount Account { get; set; }

		[Export ("requestMethod")]
		SLRequestMethod RequestMethod { get; }

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("parameters")]
		NSDictionary Parameters { get; }

		[NoiOS] // just macOS
		[NoMacCatalyst]
		[Export ("addMultipartData:withName:type:")]
		void AddMultipartData (NSData data, string partName, string partType);

		[Export ("addMultipartData:withName:type:filename:")]
		void AddMultipartData (NSData data, string partName, string partType, string filename);

		[Export ("preparedURLRequest")]
		NSUrlRequest GetPreparedUrlRequest ();

		// async 
		[Export ("performRequestWithHandler:")]
		[Async (ResultTypeName = "SLRequestResult", XmlDocs = """
			<summary>Asynchronously makes the request.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous PerformRequest operation.  The value of the TResult parameter is of type System.Action&lt;Foundation.NSData,Foundation.NSHttpUrlResponse,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para copied="true">The PerformRequestAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void PerformRequest (Action<NSData, NSHttpUrlResponse, NSError> handler);
	}

	/// <summary>A <see cref="UIKit.UIViewController" /> that manages the user experience for the composition of a post for a social service.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/SLComposeViewController_Class/index.html">Apple documentation for <c>SLComposeViewController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (SocialViewController))]
	[DisableDefaultCtor] // see note on 'composeViewControllerForServiceType:'
	interface SLComposeViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new compose view controller from the named NIB in the specified <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("serviceType")]
		NSString ServiceType { get; }

		[Export ("completionHandler", ArgumentSemantic.Copy)]
		Action<SLComposeViewControllerResult> CompletionHandler { get; set; }

		[Static]
		[Export ("composeViewControllerForServiceType:")]
		// note: Use this method to create a social compose view controller. Do not use any other methods.
		SLComposeViewController FromService (NSString serviceType);

		[Static]
		[Export ("isAvailableForServiceType:")]
		bool IsAvailable (NSString serviceType);

		[Export ("setInitialText:")]
		bool SetInitialText (string text);

		[Export ("addImage:")]
		bool AddImage (SocialImage image);

		[Export ("removeAllImages")]
		bool RemoveAllImages ();

		[Export ("addURL:")]
		bool AddUrl (NSUrl url);

		[Export ("removeAllURLs")]
		bool RemoveAllUrls ();
	}

	/// <summary>A standard UIViewController for composing data for social sharing.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Social/Reference/SLComposeServiceViewController_Class/index.html">Apple documentation for <c>SLComposeServiceViewController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (SocialViewController))]
	interface SLComposeServiceViewController : SocialTextViewDelegate {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new view controller from a named NIB in the provided bundle.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("presentationAnimationDidFinish")]
		void PresentationAnimationDidFinish ();

		[Export ("textView")]
		SocialTextView TextView { get; }

		[Export ("contentText")]
		string ContentText { get; }

		[NullAllowed] // by default this property is null
		[Export ("placeholder")]
		string Placeholder { get; set; }

		[Export ("didSelectPost")]
		void DidSelectPost ();

		[Export ("didSelectCancel")]
		void DidSelectCancel ();

		[Export ("cancel")]
		void Cancel ();

		[Export ("isContentValid")]
		bool IsContentValid ();

		[Export ("validateContent")]
		void ValidateContent ();

		[NullAllowed] // by default this property is null
		[Export ("charactersRemaining", ArgumentSemantic.Strong)]
		NSNumber CharactersRemaining { get; set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("configurationItems")]
		SLComposeSheetConfigurationItem [] GetConfigurationItems ();

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("reloadConfigurationItems")]
		void ReloadConfigurationItems ();

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("pushConfigurationViewController:")]
		void PushConfigurationViewController (SocialViewController viewController);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("popConfigurationViewController")]
		void PopConfigurationViewController ();

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("loadPreviewView")]
		SocialView LoadPreviewView ();

		[NoMac]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("autoCompletionViewController", ArgumentSemantic.Strong)]
		SocialViewController AutoCompletionViewController { get; set; }

#if NET
		// Inlined manually from UITextViewDelegate/NSTextViewDelegate, because the one from the *Delegate type
		// has different availability attributes depending on the platform.
		[iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0), NoTV]
		[Export ("textViewWritingToolsWillBegin:")]
		new void WritingToolsWillBegin (SocialTextView textView);

		// Inlined manually from UITextViewDelegate/NSTextViewDelegate, because the one from the *Delegate type
		// has different availability attributes depending on the platform.
		[iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0), NoTV]
		[Export ("textViewWritingToolsDidEnd:")]
		new void WritingToolsDidEnd (SocialTextView textView);

		// Inlined manually from UITextViewDelegate/NSTextViewDelegate, because the one from the *Delegate type
		// has different availability attributes depending on the platform.
		[iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0), NoTV]
		[Export ("textView:writingToolsIgnoredRangesInEnclosingRange:")]
		new NSValue [] GetWritingToolsIgnoredRangesInEnclosingRange (SocialTextView textView, NSRange enclosingRange);
#endif
	}


	/// <summary>Allows users to configure properties of a post for social sharing.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Social/Reference/SLComposeSheetConfigurationItem_Class/index.html">Apple documentation for <c>SLComposeSheetConfigurationItem</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // designated
	interface SLComposeSheetConfigurationItem {

		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[NullAllowed] // by default this property is null
		[Export ("title")]
		string Title { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("value")]
		string Value { get; set; }

		[Export ("valuePending", ArgumentSemantic.Assign)]
		bool ValuePending { get; set; }

		[Export ("tapHandler", ArgumentSemantic.Copy)]
		Action TapHandler { get; set; }
	}
}
