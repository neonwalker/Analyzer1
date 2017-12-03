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
    public class LocalVariablesAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EA1003";
        private const string Title = @"Variable ""{0}"" naming fault";
        private const string MessageFormat = "Not lower camel case";
        private const string Description = "Not correct naming of varianle";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;
            foreach (VariableDeclaratorSyntax variable in localDeclaration.Declaration.Variables)
            {
                if(variable.Identifier.Text==String.Empty)
                    return;

                if (char.IsUpper(variable.Identifier.Text[0]))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
                }
            }
        }
    }
}