using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1.MethodPropertyEvent
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    class MethodParamsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EA1008";
        private static readonly string Title = "Parameter naming fault";
        private static readonly string MessageFormat = @"Not lower camel case";
        private static readonly string Description = "Parameter naming fault";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var method = context.Node as MethodDeclarationSyntax;
            var paramsList = method.ParameterList;
            var parameters = paramsList.Parameters;
            foreach (ParameterSyntax item in parameters)
            {
                if(item.Identifier.Text!=String.Empty)
                {
                    if((!char.IsLower(item.Identifier.Text[0])))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, item.GetLocation()));
                    }
                }
            }
        }
    }
}

