// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012, 2014-2015 Xamarin Inc.
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
//
using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

#nullable enable

namespace AVFoundation {
	/// <summary>A class that encapsulates the edge-widths used by an <see cref="AVFoundation.AVVideoCompositionRenderContext" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVEdgeWidths {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat /* CGFloat */ Left;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat /* CGFloat */ Top;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat /* CGFloat */ Right;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat /* CGFloat */ Bottom;

		public AVEdgeWidths (nfloat left, nfloat top, nfloat right, nfloat bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return string.Format ("(left={0},top={1},right={2},bottom={3})", Left, Top, Right, Bottom);
		}

		public static bool operator == (AVEdgeWidths left, AVEdgeWidths right)
		{
			return
				left.Left == right.Left &&
				left.Top == right.Top &&
				left.Right == right.Right &&
				left.Bottom == right.Bottom;
		}

		public static bool operator != (AVEdgeWidths left, AVEdgeWidths right)
		{
			return
				left.Left != right.Left ||
				left.Top != right.Top ||
				left.Right != right.Right ||
				left.Bottom != right.Bottom;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Left, Top, Right, Bottom);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? other)
		{
			if (other is AVEdgeWidths) {
				var o = (AVEdgeWidths) other;

				return this == o;
			}
			return false;
		}
	}
}
