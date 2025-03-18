using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ShouldMethodAssertion.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class Analyzer : DiagnosticAnalyzer
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

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        s_diagnosticDescriptor_0001,
        s_diagnosticDescriptor_0002,
        s_diagnosticDescriptor_0003,
        s_diagnosticDescriptor_0004
        );

    public override void Initialize(AnalysisContext context)
    {
#if DEBUG
        if (Debugger.IsAttached)
            context.EnableConcurrentExecution();
#else
        context.EnableConcurrentExecution();
#endif
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(InvocationAction, SyntaxKind.InvocationExpression);
    }

    private void InvocationAction(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not InvocationExpressionSyntax assertMethodInvocationExpressionSyntax)
            return;

        if (assertMethodInvocationExpressionSyntax.Expression is not MemberAccessExpressionSyntax assertMethodAccessExpressionSyntax)
            return;

        if (assertMethodAccessExpressionSyntax.Name is not IdentifierNameSyntax assertMethodIdentifierNameSyntax)
            return;

        var assertMethodName = assertMethodIdentifierNameSyntax.Identifier.Text;

        if (assertMethodName is AssertMethodNames.BeNull)
        {
            if (!tryGetActualValueTypeAndExpression(context, assertMethodAccessExpressionSyntax, out var typeSymbol, out var expressionSyntax))
                return;

            if (typeSymbol.IsValueType && typeSymbol.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T)
            {
                // 値型に対するBeNullは不適当
                var expressionText = expressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0001, assertMethodIdentifierNameSyntax.GetLocation(), [expressionText]));
            }
        }
        else if (assertMethodName is AssertMethodNames.NotBeNull)
        {
            if (!tryGetActualValueTypeAndExpression(context, assertMethodAccessExpressionSyntax, out var typeSymbol, out var expressionSyntax))
                return;

            if (typeSymbol.IsValueType && typeSymbol.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T)
            {
                // 値型に対するNotBeNullは不適当
                var expressionText = expressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0002, assertMethodIdentifierNameSyntax.GetLocation(), [expressionText]));
            }
        }
        else if (assertMethodName is AssertMethodNames.BeDefault)
        {
            if (!tryGetActualValueTypeAndExpression(context, assertMethodAccessExpressionSyntax, out var typeSymbol, out var expressionSyntax))
                return;

            if (typeSymbol.IsReferenceType || typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                // 参照型(Nullable<T>も含む)に対するBeDefaultは不適当
                var expressionText = expressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0003, assertMethodIdentifierNameSyntax.GetLocation(), [expressionText]));
            }
        }
        else if (assertMethodName is AssertMethodNames.NotBeDefault)
        {
            if (!tryGetActualValueTypeAndExpression(context, assertMethodAccessExpressionSyntax, out var typeSymbol, out var expressionSyntax))
                return;

            if (typeSymbol.IsReferenceType || typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                // 参照型(Nullable<T>も含む)に対するBeNotDefaultは不適当
                var expressionText = expressionSyntax.ToString().Replace("\r", "").Replace("\n", "");
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor_0004, assertMethodIdentifierNameSyntax.GetLocation(), [expressionText]));
            }
        }


        static bool tryGetActualValueTypeAndExpression(SyntaxNodeAnalysisContext context, MemberAccessExpressionSyntax assertMethodAccessExpressionSyntax, [MaybeNullWhen(false)] out ITypeSymbol typeSymbol, [MaybeNullWhen(false)] out ExpressionSyntax expressionSyntax)
        {
            typeSymbol = default;
            expressionSyntax = default;

            if (assertMethodAccessExpressionSyntax.Expression is not InvocationExpressionSyntax shouldMethodInvocationExpressionSyntax)
                return false;

            if (shouldMethodInvocationExpressionSyntax.Expression is not MemberAccessExpressionSyntax shouldMethodAccessExpressionSyntax)
                return false;

            if (shouldMethodAccessExpressionSyntax.Name is not IdentifierNameSyntax shouldMethodIdentifierNameSyntax)
                return false;

            if (shouldMethodIdentifierNameSyntax.Identifier.Text != AssertMethodNames.Should)
                return false;

            expressionSyntax = shouldMethodAccessExpressionSyntax.Expression;

            var typeInfo = context.SemanticModel.GetTypeInfo(shouldMethodAccessExpressionSyntax.Expression);

            typeSymbol = typeInfo.Type;

            return typeSymbol is not null;
        }
    }
}
