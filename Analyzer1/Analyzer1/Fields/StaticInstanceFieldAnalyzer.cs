using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticInstanceFieldAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EA1004";
        private static readonly string Title = "Field naming";
        private static readonly string MessageFormat = @"Missing underlining or not lowerCamelCase";
        private static readonly string Description = "Not correct naming of field";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as IFieldSymbol;
            if (namedTypeSymbol == null)
                return;

            if (namedTypeSymbol.IsConst||namedTypeSymbol.IsReadOnly)
                return;

            if (namedTypeSymbol.DeclaredAccessibility==Accessibility.Private||
               ( namedTypeSymbol.DeclaredAccessibility == Accessibility.Private&& namedTypeSymbol.IsStatic))
            {
                if (namedTypeSymbol.Name[0] != '_' || !char.IsLower(namedTypeSymbol.Name[1]))
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}