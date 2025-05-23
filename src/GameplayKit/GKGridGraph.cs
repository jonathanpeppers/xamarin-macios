//
// GKGridGraph.cs: Implements some nicer methods for GKGridGraph
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using ObjCRuntime;
#if NET
using Vector2i = global::CoreGraphics.NVector2i;
#else
using Vector2i = global::OpenTK.Vector2i;
#endif // NET

namespace GameplayKit {
	public partial class GKGridGraph {

#if !NET
		public virtual GKGridGraphNode? GetNodeAt (Vector2i position)
		{
			return GetNodeAt<GKGridGraphNode> (position);
		}
#endif
		/// <typeparam name="NodeType">To be added.</typeparam>
		/// <param name="position">To be added.</param>
		/// <summary>Gets the node at the specified <paramref name="position" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public NodeType? GetNodeAt<NodeType> (Vector2i position) where NodeType : GKGridGraphNode
		{
			return Runtime.GetNSObject<NodeType> (_GetNodeAt (position));
		}
	}
}
