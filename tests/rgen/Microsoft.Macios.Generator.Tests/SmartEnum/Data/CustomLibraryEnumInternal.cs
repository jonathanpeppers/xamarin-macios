using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace CustomLibrary;

[BindingType<SmartEnum>]
public enum CustomLibraryEnumInternal {
	[Field<EnumValue> ("None", "__Internal")]
	None,
	[Field<EnumValue> ("Medium", "__Internal")]
	Medium,
	[Field<EnumValue> ("High", "__Internal")]
	High,
}
