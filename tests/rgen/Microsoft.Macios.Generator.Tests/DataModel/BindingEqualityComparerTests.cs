// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.DataModel;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class BindingEqualityComparerTests : BaseGeneratorTestClass {
	readonly BindingEqualityComparer equalityComparer = new ();

	[Fact]
	public void CompareDifferentFullyQualifiedSymbol ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name1",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name1",
			symbolAvailability: new ());
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name2",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name2",
			symbolAvailability: new ());
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentBindingType ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.SmartEnum, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ());
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ());
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentAttributesLength ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ());
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			Attributes = [
				new AttributeCodeChange (name: "name", arguments: ["arg1", "arg2"])
			]
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentAttributes ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			Attributes = [
				new AttributeCodeChange (name: "name", arguments: ["arg1", "arg2"])
			],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			Attributes = [
				new AttributeCodeChange (name: "name2", arguments: ["arg1", "arg2"])
			],
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentMembersLength ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ());
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [
				new EnumMember (
					name: "name",
					libraryName: "Test",
					libraryPath: "/path/to/library",
					fieldData: new (),
					symbolAvailability: new (),
					attributes: [])
			],
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentMembers ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [
				new EnumMember (
					name: "name",
					libraryName: "Test",
					libraryPath: "/path/to/library",
					fieldData: new (),
					symbolAvailability: new (),
					attributes: [])
			],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [
				new EnumMember (
					name: "name2",
					libraryName: "Test",
					libraryPath: "/path/to/library",
					fieldData: new (),
					symbolAvailability: new (),
					attributes: [])
			],
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentPropertyLength ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = []
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					])
			]
		};

		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSamePropertiesDiffOrder ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			]
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			]
		};
		Assert.True (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentProperties ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			]
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			]
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentConstructorLength ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			],
			Constructors = [],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: [])
			],
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentConstructors ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
							),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: [])
			],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					])
			],
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSameConstructors ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new Constructor (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: []),
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					])
			],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new Constructor (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: []),
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					])
			],
		};
		Assert.True (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSameConstructorsDiffOrder ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					]),
				new (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: []),
			],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: []),
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					]),
			],
		};
		Assert.True (equalityComparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSameDiffModifiers ()
	{
		var changes1 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			Modifiers = [
				SyntaxFactory.Token (kind: SyntaxKind.InternalKeyword),
				SyntaxFactory.Token (kind: SyntaxKind.PartialKeyword)
			],
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					]),
				new (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: []),
			],
		};
		var changes2 = new Binding (
			bindingInfo: new (BindingType.Protocol, new ()),
			name: "name",
			@namespace: ["NS"],
			fullyQualifiedSymbol: "NS.name",
			symbolAvailability: new ()) {
			Modifiers = [
				SyntaxFactory.Token (kind: SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (kind: SyntaxKind.StaticKeyword),
				SyntaxFactory.Token (kind: SyntaxKind.PartialKeyword)
			],
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					returnType: new ("Utils.MyClass"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
							],
							modifiers: []
						),
					]),
				new (
					name: "Surname",
					returnType: new ("string"),
					symbolAvailability: new (),
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [
								new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
							],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: null,
							attributes: [],
							modifiers: []
						),
					]),
			],
			Constructors = [
				new (type: "MyClass", symbolAvailability: new (), attributes: [], modifiers: [], parameters: []),
				new (type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name"),
					]),
			],
		};
		Assert.False (equalityComparer.Equals (changes1, changes2));
	}
}
