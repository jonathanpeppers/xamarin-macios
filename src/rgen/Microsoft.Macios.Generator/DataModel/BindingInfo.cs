// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// This struct works as a union to store the possible BindingTypeData that can be present in the bindings.
/// </summary>
[StructLayout (LayoutKind.Explicit)]
readonly struct BindingInfo : IEquatable<BindingInfo> {
	// make the struct smaller by making all wrapped structs be at index 0
	[FieldOffset (0)] readonly BindingType bindingType;
	[FieldOffset (8)] readonly BindingTypeData bindingTypeData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.Class> classData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.Protocol> protocolData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.Category> categoryData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.CoreImageFilter> coreImageFilterData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.SmartEnum> smartEnumData;

	public BindingType BindingType => bindingType;

	/// <summary>
	/// Returns if the binding has been declared to be thread safe.
	/// </summary>
	public bool IsThreadSafe {
		get {
			return bindingType switch {
				BindingType.Category => categoryData.Flags.HasFlag (Category.IsThreadSafe),
				BindingType.Class => classData.Flags.HasFlag (Class.IsThreadSafe),
				BindingType.Protocol => protocolData.Flags.HasFlag (Protocol.IsThreadSafe),
				_ => false
			};
		}
	}

	public BindingInfo (BindingType type, BindingTypeData data)
	{
		bindingType = type;
		bindingTypeData = data;
	}

	public BindingInfo (BindingTypeData<ObjCBindings.Class> data)
	{
		bindingType = BindingType.Class;
		classData = data;
	}

	public BindingInfo (BindingTypeData<ObjCBindings.Protocol> data)
	{
		bindingType = BindingType.Protocol;
		protocolData = data;
	}

	public BindingInfo (BindingTypeData<ObjCBindings.Category> data)
	{
		bindingType = BindingType.Category;
		categoryData = data;
	}

	public BindingInfo (BindingTypeData<ObjCBindings.CoreImageFilter> data)
	{
		bindingType = BindingType.CoreImageFilter;
		coreImageFilterData = data;
	}

	public BindingInfo (BindingTypeData<ObjCBindings.SmartEnum> data)
	{
		bindingType = BindingType.SmartEnum;
		smartEnumData = data;
	}

	public static implicit operator BindingTypeData (BindingInfo info) => info.bindingTypeData;

	public static implicit operator BindingTypeData<ObjCBindings.Class> (BindingInfo info)
	{
		if (info.BindingType != BindingType.Class)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.Class for binding type {info.BindingType}");
		return info.classData;
	}

	public static implicit operator BindingTypeData<ObjCBindings.Protocol> (BindingInfo info)
	{
		if (info.BindingType != BindingType.Protocol)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.Protocol for binding type {info.BindingType}");
		return info.protocolData;
	}

	public static implicit operator BindingTypeData<ObjCBindings.Category> (BindingInfo info)
	{
		if (info.BindingType != BindingType.Category)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.Category for binding type {info.BindingType}");
		return info.categoryData;
	}

	public static implicit operator BindingTypeData<ObjCBindings.CoreImageFilter> (BindingInfo info)
	{
		if (info.BindingType != BindingType.CoreImageFilter)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.SmartEnum for binding type {info.BindingType}");
		return info.coreImageFilterData;
	}

	public static implicit operator BindingTypeData<ObjCBindings.SmartEnum> (BindingInfo info)
	{
		if (info.BindingType != BindingType.SmartEnum)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.SmartEnum for binding type {info.BindingType}");
		return info.smartEnumData;
	}

	public static implicit operator BindingInfo (BindingTypeData data) => new (BindingType.Unknown, data);
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.Class> data) => new (data);
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.Protocol> data) => new (data);
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.Category> data) => new (data);
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.CoreImageFilter> data) => new (data);
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.SmartEnum> data) => new (data);

	/// <inheritdoc />
	public bool Equals (BindingInfo other)
	{
		if (bindingType != other.bindingType)
			return false;
		switch (bindingType) {
		case BindingType.Unknown:
			return bindingTypeData == other.bindingTypeData;
		case BindingType.SmartEnum:
			return bindingTypeData == other.bindingTypeData;
		case BindingType.Class:
			return classData == other.classData;
		case BindingType.Protocol:
			return protocolData == other.protocolData;
		case BindingType.Category:
			return categoryData == other.categoryData;
		}
		return false;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindingInfo other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode () => bindingType switch {
		BindingType.SmartEnum => HashCode.Combine (bindingType, bindingTypeData),
		BindingType.Class => HashCode.Combine (bindingType, classData),
		BindingType.Protocol => HashCode.Combine (bindingType, protocolData),
		BindingType.Category => HashCode.Combine (bindingType, categoryData),
		_ => HashCode.Combine (bindingType, bindingTypeData)
	};

	public static bool operator == (BindingInfo x, BindingInfo y)
	{
		return x.Equals (y);
	}

	public static bool operator != (BindingInfo x, BindingInfo y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString () => bindingType switch {
		BindingType.Class => $"{{ BindingType: {bindingType}, BindingData: {classData} }}",
		BindingType.Protocol => $"{{ BindingType: {bindingType}, BindingData: {protocolData} }}",
		BindingType.Category => $"{{ BindingType: {bindingType}, BindingData: {categoryData} }}",
		BindingType.CoreImageFilter => $"{{ BindingType: {bindingType}, BindingData: {coreImageFilterData} }}",
		BindingType.SmartEnum => $"{{ BindingType: {bindingType}, BindingData: {smartEnumData} }}",
		_ => throw new NotImplementedException ()
	};
}
