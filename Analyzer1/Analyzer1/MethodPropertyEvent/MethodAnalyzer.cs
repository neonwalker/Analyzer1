using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    class MethodAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EA1005";
        private static readonly string Title = "Method naming fault";
        private static readonly string MessageFormat = @"Not upper camel case";
        private static readonly string Description = "Method naming fault";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as IMethodSymbol;
            if (namedTypeSymbol == null)
                return;

            if (namedTypeSymbol.Name.IsUpperCase() ||
                char.IsLower(namedTypeSymbol.Name[0]))
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
