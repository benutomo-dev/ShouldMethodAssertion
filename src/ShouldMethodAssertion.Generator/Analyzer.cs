using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ShouldMethodAssertion.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class Analyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// 値型に対するBeNullの使用は不適当
    /// <para>Using `.Should().BeNull()` on value types is inappropriate. To check if `{0}` is a default value, use `.Should().BeDefault()` instead.</para>
    /// </summary>
    internal static DiagnosticDescriptor s_diagnosticDescriptor_0001 = new DiagnosticDescriptor(
        "SMAssertion0001",
        "Inappropriate use of BeNull on value types",
        "Using `.Should().BeNull()` on value types is inappropriate. To check if `{0}` is a default value, use `.Should().BeDefault()` instead.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    /// <summary>
    /// 値型に対するNotBeNullの使用は不適当
    /// <para>Using `.Should().NotBeNull()` on value types is inappropriate. To check if `{0}` is a default value, use `.Should().NotBeDefault()` instead.</para>
    /// </summary>
    internal static DiagnosticDescriptor s_diagnosticDescriptor_0002 = new DiagnosticDescriptor(
        "SMAssertion0002",
        "Inappropriate use of NotBeNull on value types",
        "Using `.Should().NotBeNull()` on value types is inappropriate. To check if `{0}` is a default value, use `.Should().NotBeDefault()` instead.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    /// <summary>
    /// 参照型(<see cref="Nullable{T}"/>も含む)に対するBeDefaultの使用は不適当
    /// <para>Using `.Should().BeDefault()` on reference types or Nullable&lt;T&gt; is inappropriate. To check if `{0}` is null, use `.Should().BeNull()` instead."</para>
    /// </summary>
    internal static DiagnosticDescriptor s_diagnosticDescriptor_0003 = new DiagnosticDescriptor(
        "SMAssertion0003",
        "Inappropriate use of BeDefault on reference types (including Nullable<T>)",
        "Using `.Should().BeDefault()` on reference types or Nullable<T> is inappropriate. To check if `{0}` is null, use `.Should().BeNull()` instead.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    /// <summary>
    /// 参照型(<see cref="Nullable{T}"/>も含む)に対するBeNotDefaultの使用は不適当
    /// <para>Using `.Should().BeNotDefault()` on reference types or Nullable&lt;T&gt; is inappropriate. To check if `{0}` is null, use `.Should().BeNotNull()` instead.</para>
    /// </summary>
    internal static DiagnosticDescriptor s_diagnosticDescriptor_0004 = new DiagnosticDescriptor(
        "SMAssertion0004",
        "Inappropriate use of BeNotDefault on reference types (including Nullable<T>)",
        "Using `.Should().BeNotDefault()` on reference types or Nullable<T> is inappropriate. To check if `{0}` is null, use `.Should().BeNotNull()` instead.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    /// <summary>
    /// CallerArgumentExpression属性が付与されたパラメータに引数が割り当てられてしまっています
    /// <para>An argument has been assigned to a parameter with the CallerArgumentExpression attribute.</para>
    /// </summary>
    internal static DiagnosticDescriptor s_diagnosticDescriptor_0005 = new DiagnosticDescriptor(
        "SMAssertion0005",
        "An argument has been assigned to a parameter with the CallerArgumentExpression attribute",
        "An argument has been assigned to a parameter with the CallerArgumentExpression attribute",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        s_diagnosticDescriptor_0001,
        s_diagnosticDescriptor_0002,
        s_diagnosticDescriptor_0003,
        s_diagnosticDescriptor_0004,
        s_diagnosticDescriptor_0005
        );

    public override void Initialize(AnalysisContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
            context.EnableConcurrentExecution();
#else
        context.EnableConcurrentExecution();
#endif

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterCompilationStartAction(DetectInvalidArgumentExpressionArgUsingAnalysis);

        context.RegisterSyntaxNodeAction(InappropriateIsNullOrIsDefaultCallAnalysys, SyntaxKind.InvocationExpression);
    }

    private void DetectInvalidArgumentExpressionArgUsingAnalysis(CompilationStartAnalysisContext context)
    {
        var callerArgumentExpressionAttributeSymbol = context.Compilation.GetTypeByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        if (callerArgumentExpressionAttributeSymbol is null)
            return;

        context.RegisterSyntaxNodeAction((context) =>
        {
            if (!TryGetActualValueExpression(context.Node, out var assertMethodInvocationExpressionSyntax, out var assertMethodName, out var actualValueExpressionSyntax))
                return; // 通常のメソッドチェインによるShoud()メソッドからの呼出しでない


            if (assertMethodInvocationExpressionSyntax.ArgumentList.Arguments.Count == 0)
                return;

            var invocationOperation = context.SemanticModel.GetOperation(assertMethodInvocationExpressionSyntax) as IInvocationOperation;

            if (invocationOperation is null)
                return;

            foreach (var argumentOperation in invocationOperation.Arguments)
            {
                if (argumentOperation.Parameter is null)
                    continue;

                if (argumentOperation.IsImplicit)
                    continue;

                if (!argumentOperation.Parameter.GetAttributes().Any(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, callerArgumentExpressionAttributeSymbol)))
                    continue;

                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0005, argumentOperation.Syntax.GetLocation()));
            }

        }, SyntaxKind.InvocationExpression);
    }

    private void InappropriateIsNullOrIsDefaultCallAnalysys(SyntaxNodeAnalysisContext context)
    {
        if (!TryGetActualValueExpression(context.Node, out var assertMethodInvocationExpressionSyntax, out var assertMethodName, out var actualValueExpressionSyntax))
            return; // 通常のメソッドチェインによるShoud()メソッドからの呼出しでない


        if (assertMethodName is AssertMethodNames.BeNull)
        {
            var actualValueType = context.SemanticModel.GetTypeInfo(actualValueExpressionSyntax, context.CancellationToken).Type;

            if (actualValueType is { IsValueType: true, OriginalDefinition.SpecialType: not SpecialType.System_Nullable_T })
            {
                // 値型に対するBeNullは不適当
                var expressionText = actualValueExpressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0001, assertMethodInvocationExpressionSyntax.GetLocation(), [expressionText]));
            }
        }
        else if (assertMethodName is AssertMethodNames.NotBeNull)
        {
            var actualValueType = context.SemanticModel.GetTypeInfo(actualValueExpressionSyntax, context.CancellationToken).Type;

            if (actualValueType is { IsValueType: true, OriginalDefinition.SpecialType: not SpecialType.System_Nullable_T })
            {
                // 値型に対するNotBeNullは不適当
                var expressionText = actualValueExpressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0002, assertMethodInvocationExpressionSyntax.GetLocation(), [expressionText]));
            }
        }
        else if (assertMethodName is AssertMethodNames.BeDefault)
        {
            var actualValueType = context.SemanticModel.GetTypeInfo(actualValueExpressionSyntax, context.CancellationToken).Type;

            if (actualValueType is { IsReferenceType: true } or { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T })
            {
                // 参照型(Nullable<T>も含む)に対するBeDefaultは不適当
                var expressionText = actualValueExpressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0003, assertMethodInvocationExpressionSyntax.GetLocation(), [expressionText]));
            }
        }
        else if (assertMethodName is AssertMethodNames.NotBeDefault)
        {
            var actualValueType = context.SemanticModel.GetTypeInfo(actualValueExpressionSyntax, context.CancellationToken).Type;

            if (actualValueType is { IsReferenceType: true } or { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T })
            {
                // 参照型(Nullable<T>も含む)に対するBeNotDefaultは不適当
                var expressionText = actualValueExpressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0004, assertMethodInvocationExpressionSyntax.GetLocation(), [expressionText]));
            }
        }
    }

    private static bool TryGetActualValueExpression(
        SyntaxNode invocationExpressionSyntax,
        [MaybeNullWhen(false)] out InvocationExpressionSyntax assertMethodInvocationExpressionSyntax,
        [MaybeNullWhen(false)] out string assertMethodName,
        [MaybeNullWhen(false)] out ExpressionSyntax expressionSyntax)
    {
        assertMethodName = null;
        expressionSyntax = default;

        assertMethodInvocationExpressionSyntax = invocationExpressionSyntax as InvocationExpressionSyntax;

        if (assertMethodInvocationExpressionSyntax is null)
            return false;

        if (assertMethodInvocationExpressionSyntax.Expression is not MemberAccessExpressionSyntax assertMethodAccessExpressionSyntax)
            return false;

        if (assertMethodAccessExpressionSyntax.Name is not IdentifierNameSyntax assertMethodIdentifierNameSyntax)
            return false;

        assertMethodName = assertMethodIdentifierNameSyntax.Identifier.Text;

        if (assertMethodAccessExpressionSyntax.Expression is not InvocationExpressionSyntax shouldMethodInvocationExpressionSyntax)
            return false;

        if (shouldMethodInvocationExpressionSyntax.Expression is not MemberAccessExpressionSyntax shouldMethodAccessExpressionSyntax)
            return false;

        if (shouldMethodAccessExpressionSyntax.Name is not IdentifierNameSyntax shouldMethodIdentifierNameSyntax)
            return false;

        if (shouldMethodIdentifierNameSyntax.Identifier.Text != AssertMethodNames.Should)
            return false;

        expressionSyntax = shouldMethodAccessExpressionSyntax.Expression;
        return true;
    }
}
