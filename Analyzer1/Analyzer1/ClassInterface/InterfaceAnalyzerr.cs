using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InterfaceAnalyzer : DiagnosticAnalyzer
    {
        #region Descriptor fields
        public const string DiagnosticId = "EA1000";
        private static readonly string Title = "Not correct naming of interface";
        private static readonly string MessageFormat = @"Missing prefix ""I"" or not correct format";
        private static readonly string Description = "Not correct naming of interface";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        #endregion

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as INamedTypeSymbol;
            if (namedTypeSymbol == null)
                return;

            if (namedTypeSymbol.TypeKind == TypeKind.Interface)
            {
                if (namedTypeSymbol.Name[0] != 'I' || char.IsLower(namedTypeSymbol.Name[1])
                    ||namedTypeSymbol.Name.IsUpperCase())
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}