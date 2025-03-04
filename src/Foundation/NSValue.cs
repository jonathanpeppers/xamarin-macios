//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012, 2013 Xamarin Inc
// Copyright 2019 Microsoft Corporation
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
#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif
using System.Runtime.InteropServices;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	public partial class NSValue : NSObject {
#if !COREBUILD
		public string ObjCType {
			get {
				return Marshal.PtrToStringAnsi (ObjCTypePtr ());
			}
		}

#if !NO_SYSTEM_DRAWING
		public static NSValue FromRectangleF (RectangleF rect)
		{
			return FromCGRect (rect);
		}

		public static NSValue FromSizeF (SizeF size)
		{
			return FromCGSize (size);
		}

		public static NSValue FromPointF (PointF point)
		{
			return FromCGPoint (point);
		}

		public RectangleF RectangleFValue {
			get { return (RectangleF) CGRectValue; }
		}

		public SizeF SizeFValue {
			get { return (SizeF) CGSizeValue; }
		}

		public PointF PointFValue {
			get { return (PointF) CGPointValue; }
		}
#endif

#endif
	}
}
