using System;

#if HAS_APPKIT
using AppKit;
#endif

using Foundation;
using ObjCRuntime;
using Security;

#if HAS_UIKIT
using UIKit;
#endif

namespace SecurityUI {
	[TV (18, 4), MacCatalyst (18, 4), Mac (15, 4), iOS (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SFCertificatePresentation {
		[Export ("initWithTrust:")]
		[DesignatedInitializer]
		NativeHandle Constructor (SecTrust trust);

#if __MACOS__
		[NoTV, NoiOS, NoMacCatalyst]
		[Export ("presentSheetInWindow:dismissHandler:")]
		void PresentSheet (NSWindow window, [NullAllowed] Action dismissHandler);
#endif

#if !__MACOS__
		[NoMac]
		[Export ("presentSheetInViewController:dismissHandler:")]
		void PresentSheet (UIViewController viewController, [NullAllowed] Action dismissHandler);
#endif

		[Export ("dismissSheet")]
		void DismissSheet ();

		[Export ("trust", ArgumentSemantic.Assign)]
		SecTrust Trust { get; }

		[NullAllowed, Export ("title")]
		string Title { get; set; }

		[NullAllowed, Export ("message")]
		string Message { get; set; }

		[NullAllowed, Export ("helpURL", ArgumentSemantic.Copy)]
		NSUrl HelpUrl { get; set; }
	}
}
