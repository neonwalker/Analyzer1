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
using System.Globalization;
using System.Text;

namespace Analyzer1
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StaticInstanceFieldFixer)), Shared]
    public class StaticInstanceFieldFixer : CodeFixProvider
    {

        private const string title = "Field naming";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(StaticInstanceFieldAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }
        public void Method()
        {
        }
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedSolution: c => RenameInterfaceAsync(context.Document, declaration, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Solution> RenameInterfaceAsync(Document document, FieldDeclarationSyntax fieldDecl, CancellationToken cancellationToken)
        {
            var identifierToken = fieldDecl.Declaration.Variables.First().Identifier;
            StringBuilder stringBuilder = new StringBuilder(identifierToken.Text);
            if (stringBuilder[0] != '_' )
            {
                stringBuilder.Insert(0, '_');
            }
            if (!char.IsLower(stringBuilder[1]))
            {
                stringBuilder[1] = char.ToLower(stringBuilder[1]);
            }
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(fieldDecl.Declaration.Variables.First());
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, stringBuilder.ToString(), optionSet, cancellationToken).ConfigureAwait(false);

            return newSolution;
        }
    }
}