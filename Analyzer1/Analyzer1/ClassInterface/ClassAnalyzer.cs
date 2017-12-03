using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EA1002";

        private static readonly string Title = "Class naming fault";
        private static readonly string MessageFormat = @"Not upper camel case";
        private static readonly string Description = "Class naming fault";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
                return;
            if (namedTypeSymbol.TypeKind == TypeKind.Class)
            {
                if (namedTypeSymbol.Name.IsUpperCase() ||
                   char.IsLower(namedTypeSymbol.Name[0]))
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
