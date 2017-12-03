//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Composition;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CodeFixes;
//using Microsoft.CodeAnalysis.CodeActions;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Rename;
//using Microsoft.CodeAnalysis.Text;
//using System.Globalization;
//using System.Text;

//namespace Analyzer1.MethodPropertyEvent
//{
//    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MethodParamsAnalyzer)), Shared]
//    class MethodParamsFixer : CodeFixProvider
//    {
//        private const string title = "Parameter naming";

//        public sealed override ImmutableArray<string> FixableDiagnosticIds
//        {
//            get { return ImmutableArray.Create(MethodParamsAnalyzer.DiagnosticId); }
//        }

//        public sealed override FixAllProvider GetFixAllProvider()
//        {
//            return WellKnownFixAllProviders.BatchFixer;
//        }
//        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
//        {
//            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
//            var diagnostic = context.Diagnostics.First();
//            var diagnosticSpan = diagnostic.Location.SourceSpan;
//            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

//            context.RegisterCodeFix(
//                CodeAction.Create(
//                    title: title,
//                    createChangedSolution: c => RenameInterfaceAsync(context.Document, declaration, c),
//                    equivalenceKey: title),
//                diagnostic);
//        }

//        private async Task<Solution> RenameInterfaceAsync(Document document, MethodDeclarationSyntax typeDecl, CancellationToken cancellationToken)
//        {
//            var identifierToken = typeDecl.ParameterList;
//            var newName = identifierToken.Text.TitleCaseString();

//            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
//            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);
//            var originalSolution = document.Project.Solution;
//            var optionSet = originalSolution.Workspace.Options;
//            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

//            return newSolution;
//        }
//    }
//}
