// 
// ABUnknownPersonViewController.cs: 
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
//

#nullable enable

using System;

using AddressBook;
using Foundation;
using ObjCRuntime;

namespace AddressBookUI {
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ABUnknownPersonCreatedEventArgs : EventArgs {

		public ABUnknownPersonCreatedEventArgs (ABPerson? person)
		{
			Person = person;
		}

		public ABPerson? Person { get; private set; }
	}

	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	class InternalABUnknownPersonViewControllerDelegate : ABUnknownPersonViewControllerDelegate {
		internal EventHandler<ABPersonViewPerformDefaultActionEventArgs>? performDefaultAction;
		internal EventHandler<ABUnknownPersonCreatedEventArgs>? personCreated;

		public InternalABUnknownPersonViewControllerDelegate ()
		{
			IsDirectBinding = false;
		}

		[Preserve (Conditional = true)]
		public override void DidResolveToPerson (ABUnknownPersonViewController personViewController, ABPerson? person)
		{
			personViewController.OnPersonCreated (new ABUnknownPersonCreatedEventArgs (person));
		}

		[Preserve (Conditional = true)]
		public override bool ShouldPerformDefaultActionForPerson (ABUnknownPersonViewController personViewController, ABPerson person, int propertyId, int identifier)
		{
			ABPersonProperty property = ABPersonPropertyId.ToPersonProperty (propertyId);
			int? id = identifier == ABRecord.InvalidPropertyId ? null : (int?) identifier;

			var e = new ABPersonViewPerformDefaultActionEventArgs (person, property, id);
			personViewController.OnPerformDefaultAction (e);
			return e.ShouldPerformDefaultAction;
		}
	}

	partial class ABUnknownPersonViewController {

		ABPerson? displayedPerson;
		public ABPerson? DisplayedPerson {
			get {
				MarkDirty ();
				return BackingField.Get (ref displayedPerson, _DisplayedPerson, h => new ABPerson (h, AddressBook));
			}
			set {
				_DisplayedPerson = BackingField.Save (ref displayedPerson, value);
				MarkDirty ();
			}
		}

		ABAddressBook? addressBook;
		public ABAddressBook? AddressBook {
			get {
				MarkDirty ();
				return BackingField.Get (ref addressBook, _AddressBook, h => new ABAddressBook (h, false));
			}
			set {
				_AddressBook = BackingField.Save (ref addressBook, value);
				MarkDirty ();
			}
		}

		InternalABUnknownPersonViewControllerDelegate EnsureEventDelegate ()
		{
			var d = WeakDelegate as InternalABUnknownPersonViewControllerDelegate;
			if (d is null) {
				d = new InternalABUnknownPersonViewControllerDelegate ();
				WeakDelegate = d;
			}
			return d;
		}

		protected internal virtual void OnPerformDefaultAction (ABPersonViewPerformDefaultActionEventArgs e)
		{
			var h = EnsureEventDelegate ().performDefaultAction;
			if (h is not null)
				h (this, e);
		}

		protected internal virtual void OnPersonCreated (ABUnknownPersonCreatedEventArgs e)
		{
			var h = EnsureEventDelegate ().personCreated;
			if (h is not null)
				h (this, e);
		}

		public event EventHandler<ABPersonViewPerformDefaultActionEventArgs> PerformDefaultAction {
			add { EnsureEventDelegate ().performDefaultAction += value; }
			remove { EnsureEventDelegate ().performDefaultAction -= value; }
		}

		public event EventHandler<ABUnknownPersonCreatedEventArgs> PersonCreated {
			add { EnsureEventDelegate ().personCreated += value; }
			remove { EnsureEventDelegate ().personCreated -= value; }
		}
	}
}
