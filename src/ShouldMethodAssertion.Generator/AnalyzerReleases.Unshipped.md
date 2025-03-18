; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
SMAssertion0001 | Usage    |  Warning | Inappropriate use of BeNull on value types
SMAssertion0002 | Usage    |  Warning | Inappropriate use of NotBeNull on value types
SMAssertion0003 | Usage    |  Warning | Inappropriate use of BeDefault on reference types (including Nullable<T>)
SMAssertion0004 | Usage    |  Warning | Inappropriate use of BeNotDefault on reference types (including Nullable<T>)
SMAssertion0005 | Usage    |  Warning | An argument has been assigned to a parameter with the CallerArgumentExpression attribute.
