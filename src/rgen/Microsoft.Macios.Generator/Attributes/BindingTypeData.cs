// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Generator.Attributes;

readonly struct BindingTypeData : IEquatable<BindingTypeData> {

	/// <summary>
	/// Original name of the ObjC class or protocol.
	/// </summary>
	public string? Name { get; }

	public BindingTypeData (string? name)
	{
		Name = name;
	}

	/// <summary>
	/// Try to parse the attribute data to retrieve the information of an ExportAttribute&lt;T&gt;.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="data">The parsed data. Null if we could not parse the attribute data.</param>
	/// <returns>True if the data was parsed.</returns>
	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out BindingTypeData? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		string? name;
		switch (count) {
		case 0:
			name = null;
			break;
		case 1:
			name = (string?) attributeData.ConstructorArguments [0].Value!;
			break;
		default:
			// 0 should not be an option..
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = new (name);
			return true;
		}

		foreach (var (paramName, value) in attributeData.NamedArguments) {
			switch (paramName) {
			case "Name":
				name = (string?) value.Value!;
				break;
			default:
				data = null;
				return false;
			}
		}

		data = new (name);
		return true;
	}

	/// <inheritdoc />
	public bool Equals (BindingTypeData other) => Name == other.Name;

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindingTypeData other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Name);
	}

	public static bool operator == (BindingTypeData x, BindingTypeData y)
	{
		return x.Equals (y);
	}

	public static bool operator != (BindingTypeData x, BindingTypeData y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ Name: '{Name}' }}";
	}
}

readonly struct BindingTypeData<T> : IEquatable<BindingTypeData<T>> where T : Enum {

	/// <summary>
	/// Original name of the ObjC class or protocol.
	/// </summary>
	public string? Name { get; }

	/// <summary>
	/// The domain of an error enumerator. This has to be used with the SmartEnum flag.
	/// </summary>
	public string? ErrorDomain { get; init; }

	/// <summary>
	/// The library name of an error/smart enum.
	/// </summary>
	public string? LibraryName { get; init; }

	/// <summary>
	/// The configuration flags used on the exported class/interface.
	/// </summary>
	public T? Flags { get; } = default;

	/// <summary>
	/// The visibility of the default constructor for a core image filter.
	/// </summary>
	public MethodAttributes DefaultCtorVisibility { get; init; } = MethodAttributes.Public;

	/// <summary>
	/// The visibility of the IntPtr constructor for a core image filter.
	/// </summary>
	public MethodAttributes IntPtrCtorVisibility { get; init; } = MethodAttributes.PrivateScope;

	/// <summary>
	/// The visibility of the string constructor for a core image filter.
	/// </summary>
	public MethodAttributes StringCtorVisibility { get; init; } = MethodAttributes.PrivateScope;

	public BindingTypeData (string? name)
	{
		Name = name;
	}

	public BindingTypeData (T? flags)
	{
		Name = null;
		Flags = flags;
	}

	public BindingTypeData (string? name, T? flags)
	{
		Name = name;
		Flags = flags;
	}

	/// <summary>
	/// Try to parse the attribute data to retrieve the information of an ExportAttribute&lt;T&gt;.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="data">The parsed data. Null if we could not parse the attribute data.</param>
	/// <returns>True if the data was parsed.</returns>
	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out BindingTypeData<T>? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		string? name = null;
		T? flags = default;
		string? errorDomain = null;
		string? libraryName = null;
		var defaultCtorVisibility = MethodAttributes.Public;
		var intPtrCtorVisibility = MethodAttributes.PrivateScope;
		var stringCtorVisibility = MethodAttributes.PrivateScope;

		switch (count) {
		case 0:
			// use the defaults
			name = null;
			flags = default;
			break;
		case 1:
			if (attributeData.ConstructorArguments [0].Value is string) {
				name = (string?) attributeData.ConstructorArguments [0].Value!;
			} else {
				flags = (T) attributeData.ConstructorArguments [0].Value!;
			}
			break;
		case 2:
			// we have the name and the config flags present
			name = (string?) attributeData.ConstructorArguments [0].Value!;
			flags = (T) attributeData.ConstructorArguments [1].Value!;
			break;
		default:
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = flags is not null ?
				new (name, flags) : new (name);
			return true;
		}

		foreach (var (paramName, value) in attributeData.NamedArguments) {
			switch (paramName) {
			case "Name":
				name = (string?) value.Value!;
				break;
			case "Flags":
				flags = (T) value.Value!;
				break;
			case "ErrorDomain":
				errorDomain = (string?) value.Value!;
				break;
			case "LibraryName":
				libraryName = (string?) value.Value!;
				break;
			case "DefaultCtorVisibility":
				defaultCtorVisibility = (MethodAttributes) Convert.ToSingle ((int) value.Value!);
				break;
			case "IntPtrCtorVisibility":
				intPtrCtorVisibility = (MethodAttributes) Convert.ToSingle ((int) value.Value!);
				break;
			case "StringCtorVisibility":
				stringCtorVisibility = (MethodAttributes) Convert.ToSingle ((int) value.Value!);
				break;
			default:
				data = null;
				return false;
			}
		}

		data = flags is not null
			? new (name, flags) {
				ErrorDomain = errorDomain,
				LibraryName = libraryName,
				DefaultCtorVisibility = defaultCtorVisibility,
				IntPtrCtorVisibility = intPtrCtorVisibility,
				StringCtorVisibility = stringCtorVisibility,
			}
			: new (name) {
				ErrorDomain = errorDomain,
				LibraryName = libraryName,
				DefaultCtorVisibility = defaultCtorVisibility,
				IntPtrCtorVisibility = intPtrCtorVisibility,
				StringCtorVisibility = stringCtorVisibility,
			};
		return true;
	}

	/// <inheritdoc />
	public bool Equals (BindingTypeData<T> other)
	{
		if (Name != other.Name)
			return false;
		if (Flags is not null && other.Flags is not null) {
			return Flags.Equals (other.Flags);
		}
		return false;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindingTypeData<T> other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Name, Flags);
	}

	public static bool operator == (BindingTypeData<T> x, BindingTypeData<T> y)
	{
		return x.Equals (y);
	}

	public static bool operator != (BindingTypeData<T> x, BindingTypeData<T> y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ Name: '{Name}', Flags: '{Flags}' }}";
	}
}
