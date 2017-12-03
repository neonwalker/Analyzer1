using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Analyzer1
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LocalVariablesFixer)), Shared]
    public class LocalVariablesFixer : CodeFixProvider
    {
        private const string title = "Variable naming";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(LocalVariablesAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().First();
            context.RegisterCodeFix(
                CodeAction.Create(title, c => PrependUnderscore(context.Document, declaration, c), LocalVariablesAnalyzer.DiagnosticId),
                diagnostic);

        }

        async Task<Solution> PrependUnderscore(Document document, LocalDeclarationStatementSyntax declaration, CancellationToken cancellationToken)
        {
            var newName = char.ToLowerInvariant(declaration.Declaration.Variables.First().Identifier.Text[0])
                + declaration.Declaration.Variables.First().Identifier.Text.Substring(1);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var symbol = semanticModel.GetDeclaredSymbol(declaration.Declaration.Variables.First());
            var solution = document.Project.Solution;
            return await Renamer.RenameSymbolAsync(solution, symbol, newName, solution.Workspace.Options, cancellationToken).ConfigureAwait(false);
        }
    }
}