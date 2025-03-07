// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	public const string Runtime = "Runtime";
	public const string ClassPtr = "class_ptr";

	/// <summary>
	/// Generates a call to the Runtime.GetNSObject&lt;T&gt; method to create a nsobject from a handle.
	/// </summary>
	/// <param name="nsObjectType">The type of object to use as T</param>
	/// <param name="args">The arguments to pass to the GetNSObject method.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning.</param>
	/// <returns>The expression that calls GetNSObject method.</returns>
	public static ExpressionSyntax GetNSObject (string nsObjectType, ImmutableArray<ArgumentSyntax> args,
		bool suppressNullableWarning = false)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (Runtime, "GetNSObject",
			nsObjectType, argsList, suppressNullableWarning);
	}

	/// <summary>
	/// Generates a call to the method CFArray.ArrayFromHandle&lt;T&gt; to create a collection of NSObjects.
	/// </summary>
	/// <param name="nsObjectType">The type of the object to use as T</param>
	/// <param name="args">The arguments to bass to the ArrayFromHandle method.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning.</param>
	/// <returns>The expression that calls ArrayFromHandle method.</returns>
	public static ExpressionSyntax GetNSArrayFromHandle (string nsObjectType, ImmutableArray<ArgumentSyntax> args,
		bool suppressNullableWarning = false)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression ("CFArray", "ArrayFromHandle",
			nsObjectType, argsList, suppressNullableWarning);
	}

	/// <summary>
	/// Returns the expression to get the handle of a selector.
	/// </summary>
	/// <param name="selector">The selector whose handle we want to retrieve.</param>
	/// <returns>The expression to retrieve a selector handle.</returns>
	public static InvocationExpressionSyntax GetHandle (string selector)
	{
		// (selector)
		var args = ArgumentList (SingletonSeparatedList (
					Argument (LiteralExpression (
						SyntaxKind.StringLiteralExpression,
						Literal (selector))))).NormalizeWhitespace ();

		// Selector.GetHandle (selector)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName ("Selector"),
					IdentifierName ("GetHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (args);
	}

	/// <summary>
	/// Generates the "this.Handle" expression.
	/// </summary>
	/// <returns></returns>
	public static MemberAccessExpressionSyntax ThisHandle ()
	{
		return MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			ThisExpression (),
			IdentifierName ("Handle"));
	}

	/// <summary>
	/// Generates the expression to call the objc_msgSend method.
	/// </summary>
	/// <param name="objcMsgSendMethod">The name of the method in the messaging namespace.</param>
	/// <param name="selector">The selector.</param>
	/// <param name="parameters">An optional argument list.</param>
	/// <returns>The expression needed to call a specific messaging method.</returns>
	public static InvocationExpressionSyntax MessagingInvocation (string objcMsgSendMethod, string selector,
		ImmutableArray<ArgumentSyntax> parameters)
	{
		// the size of the arguments is 2 + the optional arguments
		// [0] = the handle
		// [1] = the selector
		// [2] = the arguments
		// but to be able to use the SeparatedList we need to add a comma for each argument
		// except for the last one, so we need to add parametersCount - 1 commas
		var parametersCount = 2 + parameters.Length;
		var args = new SyntaxNodeOrToken [(2 * parametersCount) - 1];
		// the first two arguments are the selector and the handle, we add those by hand
		args [0] = Argument (ThisHandle ());
		args [1] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
		args [2] = Argument (GetHandle (selector));

		// we need to add the commas and the arguments provided by the user of the api
		if (parameters.Length > 0) {
			// add a comma because we know we added the selector
			args [3] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
			var argsIndex = 4; // start at 4 because we added the first 2 parameters + 2 separators
			var parametersIndex = 0;
			while (argsIndex < args.Length) {
				var currentParameter = parameters [parametersIndex++];
				args [argsIndex++] = currentParameter;
				if (argsIndex < args.Length - 1) {
					args [argsIndex++] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
				}
			}
		}

		// generates: (this.Handle, Selector.GetHandle (selector), args)
		var argumentList = ArgumentList (SeparatedList<ArgumentSyntax> (args));
		// generates: ObjCRuntime.Messaging.objc_msgSend (this.Handle, Selector.GetHandle (selector), args)
		var invocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					AliasQualifiedName (
						IdentifierName (
							Token (SyntaxKind.GlobalKeyword)),
						IdentifierName ("ObjCRuntime")),
					IdentifierName ("Messaging")),
				IdentifierName (objcMsgSendMethod).WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
		return invocation;
	}

	/// <summary>
	/// Generates the expression to call the CFArray.StringArrayFromHandle method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call CFArray.StringArrayFromHandle method with the provided args.</returns>
	internal static InvocationExpressionSyntax StringArrayFromHandle (ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: CFArray.StringArrayFromHandle (arg1, arg2, arg3)
		return InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				IdentifierName ("CFArray"),
				IdentifierName ("StringArrayFromHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the expression to call the CFString.FromHandle method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call the CFString.FromHandle method with the provided args.</returns>
	internal static InvocationExpressionSyntax StringFromHandle (ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: CFString.FromHandle (arg1, arg2, arg3)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName ("CFString"),
					IdentifierName ("FromHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}
	
	/// <summary>
	/// Generates a call to the NSArray.ArrayFromHandleFunc with the given arguments.
	/// </summary>
	/// <param name="returnType">The generic return type of the call.</param>
	/// <param name="arguments">An immutable array of arguments.</param>
	/// <returns>The invocation syntax for the method.</returns>
	internal static InvocationExpressionSyntax NSArrayFromHandleFunc (string returnType,
		ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		// generate <returnType>
		var genericsList = TypeArgumentList (
			SingletonSeparatedList<TypeSyntax> (IdentifierName (returnType)));
		
		// generate NSArray.ArrayFromHandleFunc<returnType> (arg1, arg2, arg3)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName ("NSArray"),
					GenericName ("ArrayFromHandleFunc")
						.WithTypeArgumentList(genericsList)
						.WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}
}
