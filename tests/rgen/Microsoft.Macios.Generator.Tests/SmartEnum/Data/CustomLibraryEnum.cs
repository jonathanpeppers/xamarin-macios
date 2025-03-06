using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace CustomLibrary;

[BindingType<SmartEnum>]
public enum CustomLibraryEnum {
	[Field<EnumValue> ("None", "/path/to/customlibrary.framework")]
	None,
	[Field<EnumValue> ("Medium", "/path/to/customlibrary.framework")]
	Medium,
	[Field<EnumValue> ("High", "/path/to/customlibrary.framework")]
	High,
}
