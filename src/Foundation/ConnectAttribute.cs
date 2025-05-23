//
// Copyright 2009-2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

#nullable enable

namespace Foundation {

	/// <summary>Exposes a property as an Interface Builder Outlet.</summary>
	///     <remarks>
	///       <para>
	/// 	This property must be applied to properties that represent an
	/// 	Interface Builder Outlet on a XIB file to properly support
	/// 	loading XIB files.  The name of the property must match the
	/// 	name of the outlet declared in Interface Builder.
	///       </para>
	///       <para>
	/// 	This attribute and the property that it is applied to are automatically added by Xamarin Studio to any outlets in use that have been exposed in a class.</para>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Property)]
	public sealed class ConnectAttribute : Attribute {
		string? name;

		/// <summary>Default constructor, uses the name of the property as the name of the outlet.</summary>
		///         <remarks>To be added.</remarks>
		public ConnectAttribute () { }
		/// <param name="name">The name on the Inteface Builder file.</param>
		///         <summary>Use this constructor to specify the name of the underlying outlet to map to, instead of defaulting to the name of the property.</summary>
		///         <remarks>To be added.</remarks>
		public ConnectAttribute (string name)
		{
			this.name = name;
		}

		/// <summary>The name of the outlet.</summary>
		///         <value>The name of the outlet if specified, or null if it should default to the property name to which it is applied.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get { return this.name; }
			set { this.name = value; }
		}
	}
}
