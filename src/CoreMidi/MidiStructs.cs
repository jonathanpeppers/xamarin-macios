#if !TVOS
#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;

using MidiObjectRef = System.Int32;
using MidiClientRef = System.Int32;
using MidiDeviceRef = System.Int32;
using MidiDeviceListRef = System.Int32;
using MidiPortRef = System.Int32;
using MidiEndpointRef = System.Int32;
using MidiEntityRef = System.Int32;

namespace CoreMidi {
	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	[NativeName ("MIDI2DeviceManufacturer")]
	public struct Midi2DeviceManufacturer {
		// Byte sysExIDByte[3]; // 1-byte SysEx IDs are padded with trailing zeroes
		byte sysExIdByte0;
		byte sysExIdByte1;
		byte sysExIdByte2;

		public byte [] SysExIdByte {
			get {
				return new byte [] { sysExIdByte0, sysExIdByte1, sysExIdByte2 };
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				if (value.Length != 3)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Length must be 3");

				sysExIdByte0 = value [0];
				sysExIdByte1 = value [1];
				sysExIdByte2 = value [2];
			}
		}
	}

	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	[NativeName ("MIDI2DeviceRevisionLevel")]
	public struct Midi2DeviceRevisionLevel {
		// Byte revisionLevel[4];
		byte revisionLevel0;
		byte revisionLevel1;
		byte revisionLevel2;
		byte revisionLevel3;

		public byte [] RevisionLevel {
			get {
				return new byte [] { revisionLevel0, revisionLevel1, revisionLevel2, revisionLevel3 };
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				if (value.Length != 4)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Length must be 4");

				revisionLevel0 = value [0];
				revisionLevel1 = value [1];
				revisionLevel2 = value [2];
				revisionLevel3 = value [3];
			}
		}
	}

	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	[NativeName ("MIDICIProfileIDStandard")]
	public struct MidiCIProfileIdStandard {
		public byte /* MIDIUInteger7 */ ProfileIdByte1;
		public byte /* MIDIUInteger7 */ ProfileBank;
		public byte /* MIDIUInteger7 */ ProfileNumber;
		public byte /* MIDIUInteger7 */ ProfileVersion;
		public byte /* MIDIUInteger7 */ ProfileLevel;
	}

	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	[NativeName ("MIDICIProfileIDManufacturerSpecific")]
	public struct MidiCIProfileIdManufacturerSpecific {
		public byte /* MIDIUInteger7 */ SysExId1;
		public byte /* MIDIUInteger7 */ SysExId2;
		public byte /* MIDIUInteger7 */ SysExId3;
		public byte /* MIDIUInteger7 */ Info1;
		public byte /* MIDIUInteger7 */ Info2;
	}

	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	[NativeName ("MIDICIProfileID")]
	public struct MidiCIProfileId {
		// This is a union between MidiCIProfileIdStandard and MidiCIProfileIdManufacturerSpecific, each with the same size (5 bytes)
		// So declare a struct with 5 bytes, and then do some memory copies to convert to each element of the union.
		byte /* MIDIUInteger7 */ Value0;
		byte /* MIDIUInteger7 */ Value1;
		byte /* MIDIUInteger7 */ Value2;
		byte /* MIDIUInteger7 */ Value3;
		byte /* MIDIUInteger7 */ Value4;

		public unsafe MidiCIProfileIdStandard Standard {
			get {
				fixed (MidiCIProfileId* self = &this) {
					return *(MidiCIProfileIdStandard*) self;
				}
			}
			set {
				fixed (MidiCIProfileId* self = &this) {
					*self = *(MidiCIProfileId*) &value;
				}
			}
		}

		public unsafe MidiCIProfileIdManufacturerSpecific ManufacturerSpecific {
			get {
				fixed (MidiCIProfileId* self = &this) {
					return *(MidiCIProfileIdManufacturerSpecific*) self;
				}
			}
			set {
				fixed (MidiCIProfileId* self = &this) {
					*self = *(MidiCIProfileId*) &value;
				}
			}
		}
	}
}
#endif
