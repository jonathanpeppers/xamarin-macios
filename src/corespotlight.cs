//
// CoreSpotlight bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.ComponentModel;
using ObjCRuntime;
using Foundation;
using UniformTypeIdentifiers;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace CoreSpotlight {

	/// <summary>Handler for communication between the application and the index on the device. The app does not need to be running for this communication to occur.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSIndexExtensionRequestHandler_Class/index.html">Apple documentation for <c>CSIndexExtensionRequestHandler</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CSIndexExtensionRequestHandler : NSExtensionRequestHandling, CSSearchableIndexDelegate {

	}

	/// <summary>An author or a recipient stored in a <see cref="T:CoreSpotlight.CSSearchableItemAttributeSet" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSPerson_Class/index.html">Apple documentation for <c>CSPerson</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CSPerson : NSSecureCoding, NSCopying {

		[Export ("initWithDisplayName:handles:handleIdentifier:")]
		NativeHandle Constructor ([NullAllowed] string displayName, string [] handles, NSString handleIdentifier);

		/// <summary>Gets the display name for the person.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("displayName")]
		string DisplayName { get; }

		/// <summary>Gets a list of contact handles for the person.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("handles")]
		string [] Handles { get; }

		/// <summary>Gets the key that represents the resource type that the handle represents.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("handleIdentifier")]
		NSString HandleIdentifier { get; }

		/// <summary>Gets or sets the contact identifier for the CSPerson.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Setting this property to the <see cref="P:Contacts.CNContact.Identifier" /> property for the contact enables direct lookup of the contact.</remarks>
		[NullAllowed]
		[Export ("contactIdentifier")]
		string ContactIdentifier { get; set; }
	}

	/// <summary>A search index used by Spotlight.</summary>
	///     <remarks>
	///       <para>Typically, developers should use <see cref="P:CoreSpotlight.CSSearchableIndex.DefaultSearchableIndex" /> to index their application data.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSSearchableIndex_Class/index.html">Apple documentation for <c>CSSearchableIndex</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CSSearchableIndex {

		/// <summary>Gets or sets the delegate that responds to requests for index-related tasks.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("indexDelegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		ICSSearchableIndexDelegate IndexDelegate { get; set; }

		/// <summary>Gets a Boolean value that tells whether indexing is available.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("isIndexingAvailable")]
		bool IsIndexingAvailable { get; }

		/// <summary>Gets the default search index for the device.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultSearchableIndex")]
		CSSearchableIndex DefaultSearchableIndex { get; }

		[Export ("initWithName:")]
		NativeHandle Constructor (string name);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("initWithName:protectionClass:")]
		NativeHandle Constructor (string name, [NullAllowed] NSString protectionClass);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[iOS (17, 0), Mac (14, 0), MacCatalyst (17, 0), NoTV]
		[Export ("initWithName:protectionClass:bundleIdentifier:options:")]
		NativeHandle Constructor (string name, [NullAllowed] NSString protectionClass, string bundleIdentifier, nint options);

		[Wrap ("this (name, protectionClass.GetConstant (), bundleIdentifier, options)")]
		[iOS (17, 0), Mac (14, 0), MacCatalyst (17, 0), NoTV]
		NativeHandle Constructor (string name, NSFileProtectionType protectionClass, string bundleIdentifier, nint options);

		[Export ("indexSearchableItems:completionHandler:")]
		[Async]
		void Index (CSSearchableItem [] items, [NullAllowed] Action<NSError> completionHandler);

		[Export ("deleteSearchableItemsWithIdentifiers:completionHandler:")]
		[Async]
		void Delete (string [] identifiers, [NullAllowed] Action<NSError> completionHandler);

		[Export ("deleteSearchableItemsWithDomainIdentifiers:completionHandler:")]
		[Async]
		void DeleteWithDomain (string [] domainIdentifiers, [NullAllowed] Action<NSError> completionHandler);

		[Export ("deleteAllSearchableItemsWithCompletionHandler:")]
		[Async]
		void DeleteAll ([NullAllowed] Action<NSError> completionHandler);

		// from interface CSExternalProvider (CSSearchableIndex)

		[Async (ResultTypeName = "CSSearchableIndexBundleDataResult")]
		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Export ("provideDataForBundle:identifier:type:completionHandler:")]
		void ProvideData (string bundle, string identifier, string type, Action<NSData, NSError> completionHandler);

		[Async (ResultTypeName = "CSSearchableIndexBundleDataResult")]
		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Export ("fetchDataForBundleIdentifier:itemIdentifier:contentType:completionHandler:")]
		void FetchData (string bundleIdentifier, string itemIdentifier, UTType contentType, Action<NSData, NSError> completionHandler);
	}

	/// <summary>Completion handler used in <see cref="M:CoreSpotlight.CSSearchableIndex_CSOptionalBatchingExtension.FetchLastClientState(CoreSpotlight.CSSearchableIndex,CoreSpotlight.CSSearchableIndexFetchHandler)" />.</summary>
	delegate void CSSearchableIndexFetchHandler (NSData clientState, NSError error);

	delegate void CSSearchableIndexEndIndexHandler ([NullAllowed] NSError error);

	/// <summary>Extension methods for <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:CoreServices.CSSearchableIndex&amp;scope=Xamarin" title="T:CoreServices.CSSearchableIndex">T:CoreServices.CSSearchableIndex</a></format>.</summary>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (CSSearchableIndex))]
	interface CSSearchableIndex_CSOptionalBatchingExtension {

		[Export ("beginIndexBatch")]
		void BeginIndexBatch ();

		[Export ("endIndexBatchWithClientState:completionHandler:")]
		void EndIndexBatch (NSData clientState, [NullAllowed] Action<NSError> completionHandler);

		[Export ("fetchLastClientStateWithCompletionHandler:")]
		void FetchLastClientState (CSSearchableIndexFetchHandler completionHandler);

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("endIndexBatchWithExpectedClientState:newClientState:completionHandler:")]
		void EndIndexBatch ([NullAllowed] NSData expectedClientState, NSData newClientState, [NullAllowed] CSSearchableIndexEndIndexHandler completionHandler);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:CoreSpotlight.CSSearchableIndexDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:CoreSpotlight.CSSearchableIndexDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:CoreSpotlight.CSSearchableIndexDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:CoreSpotlight.CSSearchableIndexDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICSSearchableIndexDelegate { }

	/// <summary>Delegate object providing members that are called when reindexing the index.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSSearchableIndexDelegate_Protocol/index.html">Apple documentation for <c>CSSearchableIndexDelegate</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CSSearchableIndexDelegate {

		[Abstract]
		[Export ("searchableIndex:reindexAllSearchableItemsWithAcknowledgementHandler:")]
		void ReindexAllSearchableItems (CSSearchableIndex searchableIndex, Action acknowledgementHandler);

		[Abstract]
		[Export ("searchableIndex:reindexSearchableItemsWithIdentifiers:acknowledgementHandler:")]
		void ReindexSearchableItems (CSSearchableIndex searchableIndex, string [] identifiers, Action acknowledgementHandler);

		[Export ("searchableIndexDidThrottle:")]
		void DidThrottle (CSSearchableIndex searchableIndex);

		[Export ("searchableIndexDidFinishThrottle:")]
		void DidFinishThrottle (CSSearchableIndex searchableIndex);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("dataForSearchableIndex:itemIdentifier:typeIdentifier:error:")]
		[return: NullAllowed]
		NSData GetData (CSSearchableIndex searchableIndex, string itemIdentifier, string typeIdentifier, out NSError outError);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("fileURLForSearchableIndex:itemIdentifier:typeIdentifier:inPlace:error:")]
		[return: NullAllowed]
		NSUrl GetFileUrl (CSSearchableIndex searchableIndex, string itemIdentifier, string typeIdentifier, bool inPlace, out NSError outError);
	}

	/// <summary>A uniquely identifiable, searchable object in a <see cref="T:CoreSpotlight.CSSearchableIndex" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSSearchableItem_Class/index.html">Apple documentation for <c>CSSearchableItem</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CSSearchableItem : NSSecureCoding, NSCopying {

		/// <summary>Gets the action type for the searchable item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSSearchableItemActionType")]
		NSString ActionType { get; }

		/// <summary>Gets the key for the item's unique user info dictionary.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSSearchableItemActivityIdentifier")]
		NSString ActivityIdentifier { get; }

		/// <summary>Gets a value that tells whether the continuation action is a search or query.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CSQueryContinuationActionType")]
		NSString ContinuationActionType { get; }

		/// <summary>Gets the user info key for the current query.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CSSearchQueryString")]
		NSString QueryString { get; }

		[Export ("initWithUniqueIdentifier:domainIdentifier:attributeSet:")]
		NativeHandle Constructor ([NullAllowed] string uniqueIdentifier, [NullAllowed] string domainIdentifier, CSSearchableItemAttributeSet attributeSet);

		/// <summary>Gets or sets the value that developers can use to uniquely identify the searchable item within their applications.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("uniqueIdentifier")]
		string UniqueIdentifier { get; set; }

		/// <summary>Gets or sets the optional domain identifier or owner identifier.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("domainIdentifier")]
		string DomainIdentifier { get; set; }

		/// <summary>Gets or sets the searchable item's expiration date.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // <- null_resettable
		[Export ("expirationDate")]
		NSDate ExpirationDate { get; set; }

		/// <summary>Gets or sets the dictionary of attributes for the searchable item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributeSet", ArgumentSemantic.Strong)]
		CSSearchableItemAttributeSet AttributeSet { get; set; }

		[NoTV, iOS (16, 0), MacCatalyst (16, 0), Mac (13, 0)]
		[Export ("compareByRank:")]
		NSComparisonResult CompareByRank (CSSearchableItem other);

		[NoTV]
		[Export ("isUpdate", ArgumentSemantic.Assign)]
		bool IsUpdate { get; set; }
	}

	/// <summary>Represents a string-like object that returns a locale-specific version of a string.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSLocalizedString_Class/index.html">Apple documentation for <c>CSLocalizedString</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSString))]
	// hack: it seems that generator.cs can't track NSCoding correctly ? maybe because the type is named NSString2 at that time
	interface CSLocalizedString : NSCoding {

		[Export ("initWithLocalizedStrings:")]
		NativeHandle Constructor (NSDictionary localizedStrings);

		[Export ("localizedString")]
		string GetLocalizedString ();
	}

	/// <summary>A key that app developers can associate with metadata for an indexable item that can appear in user search results.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSCustomAttributeKey_Class/index.html">Apple documentation for <c>CSCustomAttributeKey</c></related>
	[NoTV] // CS_TVOS_UNAVAILABLE
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: You must call -[CSCustomAttributeKey initWithKeyName...]
	interface CSCustomAttributeKey : NSCopying, NSSecureCoding {

		[Export ("initWithKeyName:")]
		NativeHandle Constructor (string keyName);

		[DesignatedInitializer]
		[Export ("initWithKeyName:searchable:searchableByDefault:unique:multiValued:")]
		NativeHandle Constructor (string keyName, bool searchable, bool searchableByDefault, bool unique, bool multiValued);

		/// <summary>Gets the attribute key name.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("keyName")]
		string KeyName { get; }

		/// <summary>Gets a value that tells whether the attribute can be used as a search word. The default is <see langword="true" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("searchable")]
		bool Searchable { [Bind ("isSearchable")] get; }

		/// <summary>Gets a value that tells whether the attribute is searchable by default. The default is <see langword="false" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("searchableByDefault")]
		bool SearchableByDefault { [Bind ("isSearchableByDefault")] get; }

		/// <summary>Gets a value that tells whether the attribute should be treated as unique in order to save storage space. The default is <see langword="false" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("unique")]
		bool Unique { [Bind ("isUnique")] get; }

		/// <summary>Gets a value that tells whether the attribute will likely be associated with arrays, hashes, or other compound values. The default is <see langword="false" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("multiValued")]
		bool MultiValued { [Bind ("isMultiValued")] get; }
	}

	[MacCatalyst (13, 1)]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	[Static]
	interface CSMailboxKey {

		/// <summary>The key for the Inbox.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSMailboxInbox")]
		NSString Inbox { get; }

		/// <summary>The key for the Drafts mailbox.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSMailboxDrafts")]
		NSString Drafts { get; }

		/// <summary>The key for the Sent mailbox.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSMailboxSent")]
		NSString Sent { get; }

		/// <summary>The key for the Junk mailbox.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSMailboxJunk")]
		NSString Junk { get; }

		/// <summary>The key for the Trash mailbox.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSMailboxTrash")]
		NSString Trash { get; }

		/// <summary>The key for the Archive mailbox.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CSMailboxArchive")]
		NSString Archive { get; }
	}

	/// <summary>Holds the actual content to be indexed for search.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreSpotlight/Reference/CSSearchableItemAttributeSet_Class/index.html">Apple documentation for <c>CSSearchableItemAttributeSet</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CSSearchableItemAttributeSet : NSCopying, NSSecureCoding {

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use '.ctor(UTType)' instead.")]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use '.ctor(UTType)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use '.ctor(UTType)' instead.")]
		[Export ("initWithItemContentType:")]
		NativeHandle Constructor (string itemContentType);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithContentType:")]
		NativeHandle Constructor (UTType contentType);

		// FIXME: Should we keep all the following Categories inline? or should we make them actual [Category] interfaces
		// There are no methods on any of the following categories, just properties

		// CSSearchableItemAttributeSet_Documents.h
		// CSSearchableItemAttributeSet (CSDocuments) Category
		[NullAllowed]
		[Export ("subject")]
		string Subject { get; set; }

		[NullAllowed]
		[Export ("theme")]
		string Theme { get; set; }

		/// <summary>Gets or sets a description for an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentDescription")]
		string ContentDescription { get; set; }

		[NullAllowed]
		[Export ("identifier")]
		string Identifier { get; set; }

		/// <summary>Gets or sets an intended audience descriptor for a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("audiences")]
		string [] Audiences { get; set; }

		[NullAllowed]
		[Export ("fileSize")]
		NSNumber FileSize { get; set; }

		[NullAllowed]
		[Export ("pageCount", ArgumentSemantic.Strong)]
		NSNumber PageCount { get; set; }

		[NullAllowed]
		[Export ("pageWidth", ArgumentSemantic.Strong)]
		NSNumber PageWidth { get; set; }

		[NullAllowed]
		[Export ("pageHeight", ArgumentSemantic.Strong)]
		NSNumber PageHeight { get; set; }

		[NullAllowed]
		[Export ("securityMethod")]
		string SecurityMethod { get; set; }

		/// <summary>The name of the software tool that was used to create an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("creator")]
		string Creator { get; set; }

		[NullAllowed]
		[Export ("encodingApplications")]
		string [] EncodingApplications { get; set; }

		[NullAllowed]
		[Export ("kind")]
		string Kind { get; set; }

		[NullAllowed]
		[Export ("fontNames")]
		string [] FontNames { get; set; }

		// CSSearchableItemAttributeSet_Events.h
		// CSSearchableItemAttributeSet (CSEvents) Category

		/// <summary>Gets or sets the date at which an item is due.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("dueDate", ArgumentSemantic.Strong)]
		NSDate DueDate { get; set; }

		/// <summary>Gets or sets the date on which an item was finished.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("completionDate", ArgumentSemantic.Strong)]
		NSDate CompletionDate { get; set; }

		[NullAllowed]
		[Export ("startDate", ArgumentSemantic.Strong)]
		NSDate StartDate { get; set; }

		[NullAllowed]
		[Export ("endDate", ArgumentSemantic.Strong)]
		NSDate EndDate { get; set; }

		[NullAllowed]
		[Export ("importantDates")]
		NSDate [] ImportantDates { get; set; }

		/// <summary>Gets a value that tells whether an event is an all-day event.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("allDay", ArgumentSemantic.Strong)]
		[NullAllowed]
		NSNumber AllDay { get; set; }

		// CSSearchableItemAttributeSet_General.h
		// CSSearchableItemAttributeSet (CSGeneral) Category

		/// <summary>Gets or sets the display name for an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("displayName")]
		string DisplayName { get; set; }

		/// <summary>Gets or sets an array of localized display names for an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("alternateNames")]
		string [] AlternateNames { get; set; }

		[NullAllowed]
		[Export ("path")]
		string Path { get; set; }

		/// <summary>Gets or sets the URL of indexable content.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentURL", ArgumentSemantic.Strong)]
		NSUrl ContentUrl { get; set; }

		[NullAllowed]
		[Export ("thumbnailURL", ArgumentSemantic.Strong)]
		NSUrl ThumbnailUrl { get; set; }

		[NullAllowed]
		[Export ("thumbnailData", ArgumentSemantic.Copy)]
		NSData ThumbnailData { get; set; }

		[NullAllowed]
		[Export ("relatedUniqueIdentifier")]
		string RelatedUniqueIdentifier { get; set; }

		[NullAllowed]
		[Export ("metadataModificationDate", ArgumentSemantic.Strong)]
		NSDate MetadataModificationDate { get; set; }

		/// <summary>Gets or sets the Uniform Type Identifier (UTI), of an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentType")]
		string ContentType { get; set; }

		/// <summary>Gets or sets the hierarchy of types for an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentTypeTree")]
		string [] ContentTypeTree { get; set; }

		[NullAllowed]
		[Export ("keywords")]
		string [] Keywords { get; set; }

		[NullAllowed]
		[Export ("title")]
		string Title { get; set; }

		[NullAllowed]
		[Export ("version")]
		string Version { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("weakRelatedUniqueIdentifier", ArgumentSemantic.Copy)]
		string WeakRelatedUniqueIdentifier { get; set; }

		/// <summary>Gets or sets a tag that can be used to group items for deletion.</summary>
		///         <value>
		///           <para>Developers can set this value to <see langword="null" /> to remove the item from all groups.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("domainIdentifier")]
		string DomainIdentifier { get; set; }

		// CSSearchableItemAttributeSet_Images.h
		// CSSearchableItemAttributeSet (CSImages) Category

		[NullAllowed]
		[Export ("pixelHeight", ArgumentSemantic.Strong)]
		NSNumber PixelHeight { get; set; }

		[NullAllowed]
		[Export ("pixelWidth", ArgumentSemantic.Strong)]
		NSNumber PixelWidth { get; set; }

		[NullAllowed]
		[Export ("pixelCount", ArgumentSemantic.Strong)]
		NSNumber PixelCount { get; set; }

		/// <summary>Gets or sets a string representation, such as <c>RGB</c>, of a color space model for an image item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("colorSpace")]
		string ColorSpace { get; set; }

		/// <summary>Gets or sets the bit depth of image or audio items.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("bitsPerSample", ArgumentSemantic.Strong)]
		NSNumber BitsPerSample { get; set; }

		[NullAllowed]
		[Export ("flashOn", ArgumentSemantic.Strong)]
		NSNumber FlashOn { [Bind ("isFlashOn")] get; set; }

		[NullAllowed]
		[Export ("focalLength", ArgumentSemantic.Strong)]
		NSNumber FocalLength { get; set; }

		[NullAllowed]
		[Export ("focalLength35mm", ArgumentSemantic.Strong)]
		NSNumber FocalLength35mm { [Bind ("isFocalLength35mm")] get; set; }

		/// <summary>Gets or sets the manufacturer of the device that captured an image.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("acquisitionMake")]
		string AcquisitionMake { get; set; }

		/// <summary>Gets or sets the model of the device that captured an image.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("acquisitionModel")]
		string AcquisitionModel { get; set; }

		/// <summary>Gets or sets the owner of the camera that captured an image.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("cameraOwner")]
		string CameraOwner { get; set; }

		[NullAllowed]
		[Export ("lensModel")]
		string LensModel { get; set; }

		[NullAllowed]
		[Export ("ISOSpeed", ArgumentSemantic.Strong)]
		NSNumber IsoSpeed { get; set; }

		[NullAllowed]
		[Export ("orientation", ArgumentSemantic.Strong)]
		NSNumber Orientation { get; set; }

		[NullAllowed]
		[Export ("layerNames")]
		string [] LayerNames { get; set; }

		[NullAllowed]
		[Export ("whiteBalance", ArgumentSemantic.Strong)]
		NSNumber WhiteBalance { get; set; }

		/// <summary>Gets or sets the logarithmic APEX value that describes the aperture of the capturing device at the time that an image was captured.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("aperture", ArgumentSemantic.Strong)]
		NSNumber Aperture { get; set; }

		[NullAllowed]
		[Export ("profileName")]
		string ProfileName { get; set; }

		[NullAllowed]
		[Export ("resolutionWidthDPI", ArgumentSemantic.Strong)]
		NSNumber ResolutionWidthDpi { get; set; }

		[NullAllowed]
		[Export ("resolutionHeightDPI", ArgumentSemantic.Strong)]
		NSNumber ResolutionHeightDPI { get; set; }

		[NullAllowed]
		[Export ("exposureMode", ArgumentSemantic.Strong)]
		NSNumber ExposureMode { get; set; }

		[NullAllowed]
		[Export ("exposureTime", ArgumentSemantic.Strong)]
		NSNumber ExposureTime { get; set; }

		[NullAllowed]
		[Export ("EXIFVersion")]
		string ExifVersion { get; set; }

		[NullAllowed]
		[Export ("EXIFGPSVersion")]
		string ExifGpsVersion { get; set; }

		[NullAllowed]
		[Export ("hasAlphaChannel", ArgumentSemantic.Strong)]
		NSNumber HasAlphaChannel { get; set; }

		[NullAllowed]
		[Export ("redEyeOn", ArgumentSemantic.Strong)]
		NSNumber RedEyeOn { [Bind ("isRedEyeOn")] get; set; }

		[NullAllowed]
		[Export ("meteringMode")]
		string MeteringMode { get; set; }

		[NullAllowed]
		[Export ("maxAperture", ArgumentSemantic.Strong)]
		NSNumber MaxAperture { get; set; }

		[NullAllowed]
		[Export ("fNumber", ArgumentSemantic.Strong)]
		NSNumber FNumber { get; set; }

		[NullAllowed]
		[Export ("exposureProgram")]
		string ExposureProgram { get; set; }

		[NullAllowed]
		[Export ("exposureTimeString")]
		string ExposureTimeString { get; set; }

		// CSSearchableItemAttributeSet_Media.h
		// CSSearchableItemAttributeSet (CSMedia) Category

		[NullAllowed]
		[Export ("editors")]
		string [] Editors { get; set; }

		[NullAllowed]
		[Export ("participants")]
		string [] Participants { get; set; }

		[NullAllowed]
		[Export ("projects")]
		string [] Projects { get; set; }

		/// <summary>Gets or sets the date on which an item was downloaded.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("downloadedDate", ArgumentSemantic.Strong)]
		NSDate DownloadedDate { get; set; }

		/// <summary>Gets or sets an array of URLs, email subject lines, ore etc., that represent the sources from which a media item was obtained.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentSources")]
		string [] ContentSources { get; set; }

		/// <summary>Gets or sets a comment for a media file.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("comment")]
		string Comment { get; set; }

		/// <summary>Gets or sets the date that a media item was copyrighted.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("copyright")]
		string Copyright { get; set; }

		[NullAllowed]
		[Export ("lastUsedDate", ArgumentSemantic.Strong)]
		NSDate LastUsedDate { get; set; }

		/// <summary>Gets or sets the creation date of an item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentCreationDate", ArgumentSemantic.Strong)]
		NSDate ContentCreationDate { get; set; }

		/// <summary>Gets or sets the date when a content item was most recently modified.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentModificationDate", ArgumentSemantic.Strong)]
		NSDate ContentModificationDate { get; set; }

		/// <summary>Gets or sets the date that a media item was added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("addedDate", ArgumentSemantic.Strong)]
		NSDate AddedDate { get; set; }

		[NullAllowed]
		[Export ("duration", ArgumentSemantic.Strong)]
		NSNumber Duration { get; set; }

		/// <summary>Gets or sets the list of the names of contacts who are associated with an item, but who are not the authors of the item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contactKeywords")]
		string [] ContactKeywords { get; set; }

		/// <summary>Gets or sets the codecs that were used to encode a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("codecs")]
		string [] Codecs { get; set; }

		[NullAllowed]
		[Export ("mediaTypes")]
		string [] MediaTypes { get; set; }

		[NullAllowed]
		[Export ("streamable", ArgumentSemantic.Strong)]
		NSNumber Streamable { [Bind ("isStreamable")] get; set; }

		[NullAllowed]
		[Export ("totalBitRate", ArgumentSemantic.Strong)]
		NSNumber TotalBitRate { get; set; }

		[NullAllowed]
		[Export ("videoBitRate", ArgumentSemantic.Strong)]
		NSNumber VideoBitRate { get; set; }

		/// <summary>Gets or sets the bitrate of the audio portion of a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("audioBitRate", ArgumentSemantic.Strong)]
		NSNumber AudioBitRate { get; set; }

		/// <summary>Gets or sets a value that is <c>0</c> to represent "fast start" and <c>1</c> to represent the Real Time Streaming Protocol.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("deliveryType", ArgumentSemantic.Strong)]
		NSNumber DeliveryType { get; set; }

		[NullAllowed]
		[Export ("organizations")]
		string [] Organizations { get; set; }

		[NullAllowed]
		[Export ("role")]
		string Role { get; set; }

		[NullAllowed]
		[Export ("languages")]
		string [] Languages { get; set; }

		[NullAllowed]
		[Export ("rights")]
		string Rights { get; set; }

		[NullAllowed]
		[Export ("publishers")]
		string [] Publishers { get; set; }

		/// <summary>Gets or sets a list of contributors to the production of a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contributors")]
		string [] Contributors { get; set; }

		/// <summary>Gets or sets an arbitrary string that is intended to represent the geographica or temporal scope of a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("coverage")]
		string [] Coverage { get; set; }

		[NullAllowed]
		[Export ("rating", ArgumentSemantic.Strong)]
		NSNumber Rating { get; set; }

		[NullAllowed]
		[Export ("ratingDescription")]
		NSNumber RatingDescription { get; set; }

		[NullAllowed]
		[Export ("playCount", ArgumentSemantic.Strong)]
		NSNumber PlayCount { get; set; }

		[NullAllowed]
		[Export ("information")]
		string Information { get; set; }

		/// <summary>Gets or sets the director of a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("director")]
		string Director { get; set; }

		[NullAllowed]
		[Export ("producer")]
		string Producer { get; set; }

		[NullAllowed]
		[Export ("genre")]
		string Genre { get; set; }

		[NullAllowed]
		[Export ("performers")]
		string [] Performers { get; set; }

		[NullAllowed]
		[Export ("originalFormat")]
		string OriginalFormat { get; set; }

		[NullAllowed]
		[Export ("originalSource")]
		string OriginalSource { get; set; }

		[NullAllowed]
		[Export ("local", ArgumentSemantic.Strong)]
		NSNumber Local { [Bind ("isLocal")] get; set; }

		/// <summary>Gets or sets a value that represents whether a media item is explicit.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("contentRating", ArgumentSemantic.Strong)]
		NSNumber ContentRating { get; set; }

		[NullAllowed]
		[Export ("URL", ArgumentSemantic.Strong)]
		NSUrl Url { get; set; }

		// CSSearchableItemAttributeSet_Media.h
		// CSSearchableItemAttributeSet (CSMusic) Category

		/// <summary>Gets or sets the frequency at which audio data was sampled when it was recorded, in Hz.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("audioSampleRate", ArgumentSemantic.Strong)]
		NSNumber AudioSampleRate { get; set; }

		/// <summary>Gets or sets the number of channels in an audio data file.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("audioChannelCount", ArgumentSemantic.Strong)]
		NSNumber AudioChannelCount { get; set; }

		[NullAllowed]
		[Export ("tempo", ArgumentSemantic.Strong)]
		NSNumber Tempo { get; set; }

		[NullAllowed]
		[Export ("keySignature")]
		string KeySignature { get; set; }

		[NullAllowed]
		[Export ("timeSignature")]
		string TimeSignature { get; set; }

		/// <summary>Gets or sets the name of the application that encoded the audio data in a file.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("audioEncodingApplication")]
		string AudioEncodingApplication { get; set; }

		/// <summary>Gets or sets the composer for an audio media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("composer")]
		string Composer { get; set; }

		[NullAllowed]
		[Export ("lyricist")]
		string Lyricist { get; set; }

		/// <summary>Gets or sets the album for audio items.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("album")]
		string Album { get; set; }

		/// <summary>Gets or sets the artist for a media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("artist")]
		string Artist { get; set; }

		/// <summary>Gets or sets the track number of an audio media item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("audioTrackNumber", ArgumentSemantic.Strong)]
		NSNumber AudioTrackNumber { get; set; }

		[NullAllowed]
		[Export ("recordingDate", ArgumentSemantic.Strong)]
		NSDate RecordingDate { get; set; }

		[NullAllowed]
		[Export ("musicalGenre")]
		string MusicalGenre { get; set; }

		[NullAllowed]
		[Export ("generalMIDISequence", ArgumentSemantic.Strong)]
		NSNumber GeneralMidiSequence { [Bind ("isGeneralMIDISequence")] get; set; }

		[NullAllowed]
		[Export ("musicalInstrumentCategory")]
		string MusicalInstrumentCategory { get; set; }

		[NullAllowed]
		[Export ("musicalInstrumentName")]
		string MusicalInstrumentName { get; set; }

		// CSSearchableItemAttributeSet_Messaging.h
		// CSSearchableItemAttributeSet (CSMessaging) Category

		/// <summary>Gets or sets the account identifier for a message.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("accountIdentifier")]
		string AccountIdentifier { get; set; }

		/// <summary>Gets or sets an array of account handles that a message is associated with.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("accountHandles")]
		string [] AccountHandles { get; set; }

		[NullAllowed]
		[Export ("HTMLContentData", ArgumentSemantic.Copy)]
		NSData HtmlContentData { get; set; }

		[NullAllowed]
		[Export ("textContent")]
		string TextContent { get; set; }

		/// <summary>Gets or sets an array of <see cref="T:CoreSpotlight.CSPerson" /> objects that represent the authors of a message.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("authors", ArgumentSemantic.Copy)]
		CSPerson [] Authors { get; set; }

		[NullAllowed]
		[Export ("primaryRecipients", ArgumentSemantic.Copy)]
		CSPerson [] PrimaryRecipients { get; set; }

		/// <summary>Gets or sets an array of <see cref="T:CoreSpotlight.CSPerson" /> objects that comprise the recipients on the Cc: field.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("additionalRecipients", ArgumentSemantic.Copy)]
		CSPerson [] AdditionalRecipients { get; set; }

		[NullAllowed]
		[Export ("hiddenAdditionalRecipients", ArgumentSemantic.Copy)]
		CSPerson [] HiddenAdditionalRecipients { get; set; }

		[NullAllowed]
		[Export ("emailHeaders", ArgumentSemantic.Copy)]
		NSDictionary EmailHeaders { get; set; }

		[NullAllowed]
		[Export ("mailboxIdentifiers")]
		string [] MailboxIdentifiers { get; set; }

		/// <summary>Gets or sets the list of names of the people who have authored a message.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("authorNames")]
		string [] AuthorNames { get; set; }

		[NullAllowed]
		[Export ("recipientNames")]
		string [] RecipientNames { get; set; }

		/// <summary>Gets or sets an array of author email addresses.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("authorEmailAddresses")]
		string [] AuthorEmailAddresses { get; set; }

		[NullAllowed]
		[Export ("recipientEmailAddresses")]
		string [] RecipientEmailAddresses { get; set; }

		/// <summary>Gets or sets an array of author addresses.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("authorAddresses")]
		string [] AuthorAddresses { get; set; }

		[NullAllowed]
		[Export ("recipientAddresses")]
		string [] RecipientAddresses { get; set; }

		[NullAllowed]
		[Export ("phoneNumbers")]
		string [] PhoneNumbers { get; set; }

		[NullAllowed]
		[Export ("emailAddresses")]
		string [] EmailAddresses { get; set; }

		[NullAllowed]
		[Export ("instantMessageAddresses")]
		string [] InstantMessageAddresses { get; set; }

		[Export ("likelyJunk", ArgumentSemantic.Strong)]
		NSNumber LikelyJunk { [Bind ("isLikelyJunk")] get; set; }

		// CSSearchableItemAttributeSet_Places.h
		// CSSearchableItemAttributeSet (CSPlaces) Category

		[NullAllowed]
		[Export ("headline")]
		string Headline { get; set; }

		[NullAllowed]
		[Export ("instructions")]
		string Instructions { get; set; }

		/// <summary>Gets or sets the city of an item's origin.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("city")]
		string City { get; set; }

		[NullAllowed]
		[Export ("stateOrProvince")]
		string StateOrProvince { get; set; }

		/// <summary>Gets or sets the name of the country were a media item was created.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("country")]
		string Country { get; set; }

		/// <summary>Gets or sets the altitude, in meters, above the WGS84 datum sea level.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("altitude", ArgumentSemantic.Strong)]
		NSNumber Altitude { get; set; }

		[NullAllowed]
		[Export ("latitude", ArgumentSemantic.Strong)]
		NSNumber Latitude { get; set; }

		[NullAllowed]
		[Export ("longitude", ArgumentSemantic.Strong)]
		NSNumber Longitude { get; set; }

		[NullAllowed]
		[Export ("speed", ArgumentSemantic.Strong)]
		NSNumber Speed { get; set; }

		[NullAllowed]
		[Export ("timestamp", ArgumentSemantic.Strong)]
		NSDate Timestamp { get; set; }

		[NullAllowed]
		[Export ("imageDirection", ArgumentSemantic.Strong)]
		NSNumber ImageDirection { get; set; }

		[NullAllowed]
		[Export ("namedLocation")]
		string NamedLocation { get; set; }

		[NullAllowed]
		[Export ("GPSTrack", ArgumentSemantic.Strong)]
		NSNumber GpsTrack { get; set; }

		[NullAllowed]
		[Export ("GPSStatus")]
		string GpsStatus { get; set; }

		[NullAllowed]
		[Export ("GPSMeasureMode")]
		string GpsMeasureMode { get; set; }

		[NullAllowed]
		[Export ("GPSDOP", ArgumentSemantic.Strong)]
		NSNumber GpsDop { get; set; }

		[NullAllowed]
		[Export ("GPSMapDatum")]
		string GpsMapDatum { get; set; }

		[NullAllowed]
		[Export ("GPSDestLatitude", ArgumentSemantic.Strong)]
		NSNumber GpsDestLatitude { get; set; }

		[NullAllowed]
		[Export ("GPSDestLongitude", ArgumentSemantic.Strong)]
		NSNumber GpsDestLongitude { get; set; }

		[NullAllowed]
		[Export ("GPSDestBearing", ArgumentSemantic.Strong)]
		NSNumber GpsDestBearing { get; set; }

		[NullAllowed]
		[Export ("GPSDestDistance", ArgumentSemantic.Strong)]
		NSNumber GpsDestDistance { get; set; }

		[NullAllowed]
		[Export ("GPSProcessingMethod")]
		string GpsProcessingMethod { get; set; }

		[NullAllowed]
		[Export ("GPSAreaInformation")]
		string GpsAreaInformation { get; set; }

		[NullAllowed]
		[Export ("GPSDateStamp", ArgumentSemantic.Strong)]
		NSDate GpsDateStamp { get; set; }

		[NullAllowed]
		[Export ("GPSDifferental", ArgumentSemantic.Strong)]
		NSNumber GpsDifferental { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("fullyFormattedAddress")]
		string FullyFormattedAddress { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("postalCode")]
		string PostalCode { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("subThoroughfare")]
		string SubThoroughfare { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("thoroughfare")]
		string Thoroughfare { get; set; }

		// CSActionExtras

		[NullAllowed, Export ("supportsPhoneCall", ArgumentSemantic.Strong)]
		NSNumber SupportsPhoneCall { get; set; }

		[NullAllowed, Export ("supportsNavigation", ArgumentSemantic.Strong)]
		NSNumber SupportsNavigation { get; set; }

		[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CSActionIdentifier")]
		NSString ActionIdentifier { get; }

		[NoTV, NoMac, iOS (15, 0)]
		[NoMacCatalyst]
		[Export ("actionIdentifiers", ArgumentSemantic.Copy)]
		string [] ActionIdentifiers { get; set; }

		[NullAllowed]
		[NoTV, NoMac, iOS (15, 0)]
		[NoMacCatalyst]
		[Export ("sharedItemContentType", ArgumentSemantic.Copy)]
		UTType SharedItemContentType { get; set; }

		// CSContainment

		/// <summary>Gets or sets the title of the item's container.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("containerTitle")]
		string ContainerTitle { get; set; }

		/// <summary>Gets or sets the localized container display name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("containerDisplayName")]
		string ContainerDisplayName { get; set; }

		/// <summary>Gets or sets the identifer of the item's container.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("containerIdentifier")]
		string ContainerIdentifier { get; set; }

		/// <summary>Gets or sets the order of the item within its container.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("containerOrder", ArgumentSemantic.Strong)]
		NSNumber ContainerOrder { get; set; }

		// CSCustomAttributes

		[Internal]
		[Export ("setValue:forCustomKey:")]
		void SetValue ([NullAllowed] INSSecureCoding value, CSCustomAttributeKey key);

		[Internal]
		[Export ("valueForCustomKey:")]
		[return: NullAllowed]
		INSSecureCoding ValueForCustomKey (CSCustomAttributeKey key);

		// CSSearchableItemAttributeSet_CSGeneral

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("userCreated", ArgumentSemantic.Strong)]
		[Internal] // We would like to use [BindAs (typeof (bool?))]
		NSNumber _IsUserCreated { [Bind ("isUserCreated")] get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("userOwned", ArgumentSemantic.Strong)]
		[Internal] // We would like to use[BindAs (typeof (bool?))]
		NSNumber _IsUserOwned { [Bind ("isUserOwned")] get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("userCurated", ArgumentSemantic.Strong)]
		[Internal] // We would like to use [BindAs (typeof (bool?))]
		NSNumber _IsUserCurated { [Bind ("isUserCurated")] get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("rankingHint", ArgumentSemantic.Strong)]
		NSNumber RankingHint { get; set; }

		[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("darkThumbnailURL", ArgumentSemantic.Strong)]
		NSUrl DarkThumbnailUrl { get; set; }

		// CSSearchableItemAttributeSet_CSItemProvider

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("providerDataTypeIdentifiers", ArgumentSemantic.Copy)]
		string [] ProviderDataTypeIdentifiers { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("providerFileTypeIdentifiers", ArgumentSemantic.Copy)]
		string [] ProviderFileTypeIdentifiers { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("providerInPlaceFileTypeIdentifiers", ArgumentSemantic.Copy)]
		string [] ProviderInPlaceFileTypeIdentifiers { get; set; }
	}

	/// <include file="../docs/api/CoreSpotlight/CSSearchQuery.xml" path="/Documentation/Docs[@DocId='T:CoreSpotlight.CSSearchQuery']/*" />
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CSSearchQuery {
		[Deprecated (PlatformName.iOS, 16, 0, message: "Use the constructor that takes a 'CSSearchQueryContext' parameter instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use the constructor that takes a 'CSSearchQueryContext' parameter instead.")]
		[Deprecated (PlatformName.MacOSX, 13, 0, message: "Use the constructor that takes a 'CSSearchQueryContext' parameter instead.")]
		[Export ("initWithQueryString:attributes:")]
		NativeHandle Constructor (string queryString, [NullAllowed] string [] attributes);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("initWithQueryString:queryContext:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string queryString, [NullAllowed] CSSearchQueryContext queryContext);

		/// <summary>Gets a value that tells whether the query has been canceled.</summary>
		///         <value>A value that tells whether the query has been canceled.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cancelled")]
		bool Cancelled { [Bind ("isCancelled")] get; }

		/// <summary>Gets the current number of found items.</summary>
		///         <value>The current number of found items.</value>
		///         <remarks>This number will grow as more search results become ready.</remarks>
		[Export ("foundItemCount")]
		nuint FoundItemCount { get; }

		/// <summary>Gets or sets a handler that, if it is not <see langword="null" />, is run when a batch of search results is ready.</summary>
		///         <value>
		///           <para>A a handler that, if it is not <see langword="null" />, is run when a batch of search results is ready.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("foundItemsHandler", ArgumentSemantic.Copy)]
		Action<CSSearchableItem []> FoundItemsHandler { get; set; }

		/// <summary>Gets or sets a handler that, if it is not <see langword="null" />, is run when the search completes.</summary>
		///         <value>
		///           <para>A  handler that, if supplied, is run when the search completes.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("completionHandler", ArgumentSemantic.Copy)]
		Action<NSError> CompletionHandler { get; set; }

		/// <summary>Gets or sets an array of protection classes for the indexed data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("protectionClasses", ArgumentSemantic.Copy)]
		string [] ProtectionClasses { get; set; }

		[Export ("start")]
		void Start ();

		[Export ("cancel")]
		void Cancel ();
	}

	[Abstract]
	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface CSImportExtension : NSExtensionRequestHandling {
		[Export ("updateAttributes:forFileAtURL:error:")]
		bool Update (CSSearchableItemAttributeSet attributes, NSUrl contentUrl, [NullAllowed] out NSError error);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (CSSearchQuery))]
	[DisableDefaultCtor]
	interface CSUserQuery {
		[Export ("initWithUserQueryString:userQueryContext:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] string userQueryString, [NullAllowed] CSUserQueryContext userQueryContext);

		[Export ("foundSuggestionCount")]
		nint FoundSuggestionCount { get; }

		[NullAllowed, Export ("foundSuggestionsHandler", ArgumentSemantic.Copy)]
		Action<NSArray<CSSuggestion>> FoundSuggestionsHandler { get; set; }

		[Export ("start")]
		void Start ();

		[Export ("cancel")]
		void Cancel ();

		[Static]
		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("prepare")]
		void Prepare ();

		[Static]
		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("prepareProtectionClasses:")]
		void Prepare (NSString [] protectionClasses);

		[Static]
		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Wrap ("Prepare (protectionClasses.ToConstantArray ()!)")]
		void Prepare (NSFileProtectionType [] protectionClasses);

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("userEngagedWithItem:visibleItems:userInteractionType:")]
		void UserEngaged (CSSearchableItem item, CSSearchableItem [] visibleItems, CSUserInteraction userInteractionType);

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("userEngagedWithSuggestion:visibleSuggestions:userInteractionType:")]
		void UserEngaged (CSSuggestion suggestion, CSSuggestion [] visibleSuggestions, CSUserInteraction userInteractionType);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (CSSearchQueryContext))]
	[DisableDefaultCtor]
	interface CSUserQueryContext {
		[Static]
		[Export ("userQueryContext")]
		CSUserQueryContext UserQueryContext { get; }

		[Static]
		[Export ("userQueryContextWithCurrentSuggestion:")]
		CSUserQueryContext Create ([NullAllowed] CSSuggestion currentSuggestion);

		[Export ("maxSuggestionCount")]
		nint MaxSuggestionCount { get; set; }

		[Export ("enableRankedResults")]
		bool EnableRankedResults { get; set; }

		[Export ("maxResultCount")]
		nint MaxResultCount { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("disableSemanticSearch")]
		bool DisableSemanticSearch { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("maxRankedResultCount")]
		nint MaxRankedResultCount { get; set; }
	}


	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CSSuggestion : NSSecureCoding, NSCopying {

		[Field ("CSSuggestionHighlightAttributeName")]
		NSString HighlightAttributeName { get; }

		[Export ("localizedAttributedSuggestion")]
		NSAttributedString LocalizedAttributedSuggestion { get; }

		[Export ("suggestionKind")]
		CSSuggestionKind SuggestionKind { get; }

		[Export ("compare:")]
		NSComparisonResult Compare (CSSuggestion other);

		[Export ("compareByRank:")]
		NSComparisonResult CompareByRank (CSSuggestion other);

		[iOS (17, 0), MacCatalyst (17, 0), Mac (14, 0), NoTV]
		[Export ("score")]
		NSNumber Score { get; }

		[iOS (17, 0), MacCatalyst (17, 0), Mac (14, 0), NoTV]
		[Export ("suggestionDataSources")]
		NSObject [] SuggestionDataSources { get; }
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface CSSearchQueryContext : NSSecureCoding, NSCopying {
		[Export ("fetchAttributes", ArgumentSemantic.Strong)]
		string [] FetchAttributes { get; set; }

		[Export ("filterQueries", ArgumentSemantic.Copy)]
		string [] FilterQueries { get; set; }

		[NullAllowed, Export ("keyboardLanguage", ArgumentSemantic.Strong)]
		string KeyboardLanguage { get; set; }

		[Export ("sourceOptions", ArgumentSemantic.Assign)]
		CSSearchQuerySourceOptions SourceOptions { get; set; }
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum CSSearchQuerySourceOptions : long {
		Default = 0,
		AllowMail = 1L << 0,
	}

	[NoTV, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum CSSuggestionKind : long {
		None,
		Custom,
		Default,
	}

}
