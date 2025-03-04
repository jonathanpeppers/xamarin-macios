// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.DataModel;
using ObjCBindings;
using ObjCRuntime;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using Property = ObjCBindings.Property;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class ClassBindingTests : BaseGeneratorTestClass {
	readonly BindingEqualityComparer comparer = new ();

	[Fact]
	public void IsThreadSafe ()
	{
		var binding = new Binding (
			bindingInfo: new (new BindingTypeData<Class> (Class.IsThreadSafe)),
			name: "MyClass",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.MyClass",
			symbolAvailability: new ()
		) {
			Base = "object",
			Interfaces = ImmutableArray<string>.Empty,
			Attributes = [
				new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
			],
			UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings" },
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword)
			]
		};
		Assert.True (binding.IsThreadSafe);

		binding = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "MyClass",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.MyClass",
			symbolAvailability: new ()
		) {
			Base = "object",
			Interfaces = ImmutableArray<string>.Empty,
			Attributes = [
				new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
			],
			UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings" },
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword)
			]
		};
		Assert.False (binding.IsThreadSafe);
	}

	class TestDataCodeChangesFromClassDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var builder = SymbolAvailability.CreateBuilder ();
			builder.Add (new SupportedOSPlatformData ("ios17.0"));
			builder.Add (new SupportedOSPlatformData ("tvos17.0"));
			builder.Add (new UnsupportedOSPlatformData ("macos"));

			const string emptyClass = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
}
";

			yield return [
				emptyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					]
				}
			];

			const string emptyClassWithBase = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass : NSObject {
}
";

			yield return [
				emptyClassWithBase,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "Foundation.NSObject",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					]
				}
			];

			const string emptyClassWithBaseWithInterface = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS;

public interface IMyInterface {}

[BindingType<Class>]
public partial class MyClass : NSObject, IMyInterface {
}
";

			yield return [
				emptyClassWithBaseWithInterface,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "Foundation.NSObject",
					Interfaces = ["NS.IMyInterface"],
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					]
				}
			];

			const string internalClass = @"
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS;

[BindingType<Class>]
internal partial class MyClass {
}
";

			yield return [
				internalClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.InternalKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					]
				}
			];

			const string emptyClassAvailability = @"
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS;

[BindingType<Class>]
[SupportedOSPlatform (""ios17.0"")]
[SupportedOSPlatform (""tvos17.0"")]
[UnsupportedOSPlatform (""macos"")]
public partial class MyClass {
}
";

			yield return [
				emptyClassAvailability,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: builder.ToImmutable ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>"),
						new ("System.Runtime.Versioning.UnsupportedOSPlatformAttribute", ["macos"]),
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["tvos17.0"]),
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
					],
					UsingDirectives = new HashSet<string> { "Foundation", "ObjCRuntime", "ObjCBindings", "System.Runtime.Versioning" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					]
				}
			];

			const string singleConstructorClass = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	string name = string.Empty;

	public MyClass () {}
}
";

			yield return [
				singleConstructorClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Constructors = [
						new (
							type: "MyClass",
							symbolAvailability: new (),
							attributes: [],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword)
							],
							parameters: []
						)
					]
				}
			];

			const string multiConstructorClass = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	string name = string.Empty;

	public MyClass () {}

	public MyClass(string name) {
		name = name;
	}
}
";

			yield return [
				multiConstructorClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Constructors = [
						new (
							type: "MyClass",
							symbolAvailability: new (),
							attributes: [],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword)
							],
							parameters: []
						),
						new (
							type: "MyClass",
							symbolAvailability: new (),
							attributes: [],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword)
							],
							parameters: [
								new (
									position: 0,
									type: ReturnTypeForString (),
									name: "name"
								)
							]
						),
					]
				}
			];

			const string singlePropertyClass = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Property> (""name"")]
	public partial string Name { get; set; } = string.Empty;
}
";

			yield return [
				singlePropertyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new ("name")
						}
					]
				}
			];

			const string singlePropertySmartEnumClass = @"
using ObjCBindings;

namespace NS;

[BindingType]
public enum MyEnum {
	None = 0,
}

[BindingType<Class>]
public partial class MyClass {
	[Export<Property> (""name"")]
	public partial MyEnum Name { get; set; }
}
";

			yield return [
				singlePropertySmartEnumClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForEnum ("NS.MyEnum", isSmartEnum: true),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new ("name")
						}
					]
				}
			];

			const string singlePropertyEnumClass = @"
using ObjCBindings;

namespace NS;

public enum MyEnum {
	None = 0,
}

[BindingType<Class>]
public partial class MyClass {
	[Export<Property> (""name"")]
	public partial MyEnum Name { get; set; }
}
";

			yield return [
				singlePropertyEnumClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForEnum ("NS.MyEnum"),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new ("name")
						}
					]
				}
			];

			const string notificationPropertyClass = @"
using ObjCBindings;

namespace NS;

[BindingType]
public partial class MyClass {
	[Field<Property> (""name"", Property.Notification)]
	public partial string Name { get; set; } = string.Empty;
}
";

			yield return [
				notificationPropertyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.FieldAttribute<ObjCBindings.Property>", ["name", "ObjCBindings.Property.Notification"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {

							ExportFieldData = new (
								fieldData: new (symbolName: "name", flags: Property.Notification),
								libraryName: "NS"),
						}
					]
				}
			];

			const string notificationCenterPropertyClass = @"
using ObjCBindings;

namespace NS;

[BindingType]
public partial class MyClass {
	[Field<Property> (""name"", Flags = Property.Notification, NotificationCenter=""SharedWorkspace.NotificationCenter"")]
	public partial string Name { get; set; } = string.Empty;
}
";

			yield return [
				notificationCenterPropertyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.FieldAttribute<ObjCBindings.Property>", ["name", "ObjCBindings.Property.Notification", "SharedWorkspace.NotificationCenter"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {

							ExportFieldData = new (
								fieldData: new (symbolName: "name", flags: Property.Notification) {
									NotificationCenter = "SharedWorkspace.NotificationCenter",
								},
								libraryName: "NS"),
						}
					]
				}
			];

			const string notificationTypePropertyClass = @"
using ObjCBindings;

namespace NS;

[BindingType]
public partial class MyClass {
	[Field<Property> (""name"", Flags = Property.Notification, Type=typeof (UIApplicationLaunchEventArgs))]
	public partial string Name { get; set; } = string.Empty;
}

public class UIApplicationLaunchEventArgs {}
";

			yield return [
				notificationTypePropertyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.FieldAttribute<ObjCBindings.Property>", ["name", "ObjCBindings.Property.Notification", "NS.UIApplicationLaunchEventArgs"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {

							ExportFieldData = new (
								fieldData: new (symbolName: "name", flags: Property.Notification) {
									Type = "NS.UIApplicationLaunchEventArgs",
								},
								libraryName: "NS"),
						}
					]
				}
			];

			const string fieldPropertyClass = @"
using ObjCBindings;

namespace NS;

[BindingType]
public partial class MyClass {
	[Field<Property> (""CONSTANT"")]
	public static partial string Name { get; set; } = string.Empty;
}
";

			yield return [
				fieldPropertyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.FieldAttribute<ObjCBindings.Property>", ["CONSTANT"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.StaticKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {

							ExportFieldData = new (
								fieldData: new ("CONSTANT"),
								libraryName: "NS"),
						}
					]
				}
			];

			const string multiPropertyClassMissingExport = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Property> (""name"")]
	public partial string Name { get; set; } = string.Empty;

	public partial string Surname { get; set; } = string.Empty;
}
";

			yield return [
				multiPropertyClassMissingExport,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new ("name")
						}
					]
				}
			];

			const string customMarshallingProperty = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Property> (""name"", Flags = Property.CustomMarshalDirective, NativePrefix = ""xamarin_"", Library = ""__Internal"")]
	public partial string Name { get; set; } = string.Empty;
}
";

			yield return [
				customMarshallingProperty,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name", "ObjCBindings.Property.CustomMarshalDirective", "xamarin_", "__Internal"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new (
								selector: "name",
								argumentSemantic: ArgumentSemantic.None,
								flags: Property.Default | Property.CustomMarshalDirective) {
								NativePrefix = "xamarin_",
								Library = "__Internal"
							}
						}
					]
				}
			];

			const string multiPropertyClass = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Property> (""name"")]
	public partial string Name { get; set; } = string.Empty;

	[Export<Property> (""surname"")]
	public partial string Surname { get; set; } = string.Empty;
}
";

			yield return [
				multiPropertyClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Properties = [
						new (
							name: "Name",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new ("name")
						},
						new (
							name: "Surname",
							returnType: ReturnTypeForString (),
							symbolAvailability: new (),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["surname"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Getter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Setter,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
							]
						) {
							ExportPropertyData = new ("surname")
						},
					]
				}
			];

			const string singleMethodClass = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Method> (""withName:"")]
	public partial void SetName (string name);
}
";

			yield return [
				singleMethodClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Methods = [
						new (
							type: "NS.MyClass",
							name: "SetName",
							returnType: ReturnTypeForVoid (),
							symbolAvailability: new (),
							exportMethodData: new ("withName:"),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Method>", ["withName:"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							parameters: [
								new (position: 0, type: ReturnTypeForString (), name: "name")
							]
						),
					]
				}
			];

			const string multiMethodClassMissingExport = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Method> (""withName:"")]
	public partial void SetName (string name);

	public void SetSurname (string inSurname) {}
}
";

			yield return [
				multiMethodClassMissingExport,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Methods = [
						new (
							type: "NS.MyClass",
							name: "SetName",
							returnType: ReturnTypeForVoid (),
							symbolAvailability: new (),
							exportMethodData: new ("withName:"),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Method>", ["withName:"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							parameters: [
								new (position: 0, type: ReturnTypeForString (), name: "name")
							]
						),
					]
				}
			];


			const string multiMethodClass = @"
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {
	[Export<Method> (""withName:"")]
	public partial void SetName (string name);

	[Export<Method> (""withSurname:"")]
	public partial void SetSurname (string inSurname);
}
";
			yield return [
				multiMethodClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Methods = [
						new (
							type: "NS.MyClass",
							name: "SetName",
							returnType: ReturnTypeForVoid (),
							symbolAvailability: new (),
							exportMethodData: new ("withName:"),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Method>", ["withName:"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							parameters: [
								new (position: 0, type: ReturnTypeForString (), name: "name")
							]
						),
						new (
							type: "NS.MyClass",
							name: "SetSurname",
							returnType: ReturnTypeForVoid (),
							symbolAvailability: new (),
							exportMethodData: new ("withSurname:"),
							attributes: [
								new ("ObjCBindings.ExportAttribute<ObjCBindings.Method>", ["withSurname:"])
							],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
								SyntaxFactory.Token (SyntaxKind.PartialKeyword),
							],
							parameters: [
								new (position: 0, type: ReturnTypeForString (), name: "inSurname")
							]
						),
					]
				}
			];

			const string singleEventClass = @"
using System;
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {

	public event EventHandler Changed { add; remove; }
}
";

			yield return [
				singleEventClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings", "System" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Events = [
						new (
							name: "Changed",
							type: "System.EventHandler",
							symbolAvailability: new (),
							attributes: [],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Add,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Remove,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								)
							])
					],
				}
			];

			const string multiEventClass = @"
using System;
using ObjCBindings;

namespace NS;

[BindingType<Class>]
public partial class MyClass {

	public event EventHandler Changed { add; remove; }

	public event EventHandler Removed { add; remove; }
}
";

			yield return [
				multiEventClass,
				new Binding (
					bindingInfo: new (new BindingTypeData<Class> ()),
					name: "MyClass",
					@namespace: ["NS"],
					fullyQualifiedSymbol: "NS.MyClass",
					symbolAvailability: new ()
				) {
					Base = "object",
					Interfaces = ImmutableArray<string>.Empty,
					Attributes = [
						new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
					],
					UsingDirectives = new HashSet<string> { "ObjCBindings", "System" },
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword)
					],
					Events = [
						new (
							name: "Changed",
							type: "System.EventHandler",
							symbolAvailability: new (),
							attributes: [],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Add,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Remove,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								)
							]
						),
						new (
							name: "Removed",
							type: "System.EventHandler",
							symbolAvailability: new (),
							attributes: [],
							modifiers: [
								SyntaxFactory.Token (SyntaxKind.PublicKeyword),
							],
							accessors: [
								new (
									accessorKind: AccessorKind.Add,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								),
								new (
									accessorKind: AccessorKind.Remove,
									symbolAvailability: new (),
									exportPropertyData: null,
									attributes: [],
									modifiers: []
								)
							]
						),
					],
				}
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataCodeChangesFromClassDeclaration>]
	void CodeChangesFromClassDeclaration (ApplePlatform platform, string inputText, Binding expected)
	{
		var (compilation, sourceTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<ClassDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		var changes = Binding.FromDeclaration (node, semanticModel);
		Assert.NotNull (changes);
		Assert.Equal (expected, changes.Value, comparer);
	}

	[Fact]
	public void IsStaticPropertyTest ()
	{
		var changes = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ());

		Assert.False (changes.IsStatic);

		changes = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ()) {
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.StaticKeyword),
			]
		};

		Assert.True (changes.IsStatic);
	}

	[Fact]
	public void IsPartialPropertyTest ()
	{
		var changes = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ());

		Assert.False (changes.IsPartial);

		changes = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ()) {
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword),
			]
		};

		Assert.True (changes.IsPartial);
	}

	[Fact]
	public void IsAbstractPropertyTest ()
	{
		var changes = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ());

		Assert.False (changes.IsAbstract);

		changes = new Binding (
			bindingInfo: new (new BindingTypeData<Class> ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ()) {
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.AbstractKeyword),
			]
		};

		Assert.True (changes.IsAbstract);
	}
}
