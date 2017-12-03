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
//using Microsoft.CodeAnalysis.Formatting;

//namespace Analyzer1
//{
//    interface ICpare
//    {

//    }

//    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Analyzer1CodeFixProvider)), Shared]
//    public class Analyzer1CodeFixProvider : CodeFixProvider
//    {
//        private const string title = "Make constant";

//        public sealed override ImmutableArray<string> FixableDiagnosticIds
//        {
//            get { return ImmutableArray.Create(FirstAnalyzerCSAnalyzer.DiagnosticId); }
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

//            // Find the type declaration identified by the diagnostic.
//            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().First();

//            //Register a code action that will invoke the fix.
//            context.RegisterCodeFix(
//                CodeAction.Create(
//                   title: title,
//                    createChangedSolution: c => MakeConstAsync(context.Document, declaration, c),
//                    equivalenceKey: title),
//                    diagnostic);
//        }
//        private async Task MakeConstAsync(Document document, LocalDeclarationStatementSyntax localDeclaration, CancellationToken cancellationToken)
//        {
//            var root = await document.GetSyntaxRootAsync(cancellationToken);
//            var token = root.FindToken(localDeclaration.Span.Start);
//            var node = root.FindNode(localDeclaration.Span);
//            if (node.IsKind(SyntaxKind.VariableDeclarator))
//            {
//                if (token.IsKind(SyntaxKind.IdentifierToken))
//                {
//                    var variable = (VariableDeclaratorSyntax)node;
//                    string newName = variable.Identifier.ValueText;
//                    string NameDone = String.Empty;
//                    for (int i = 0; i < newName.Length; i++)
//                    {
//                        NameDone = NameDone.ToString() + char.ToUpper(newName[i]);
//                    }

//                    var leading = variable.Identifier.LeadingTrivia;
//                    var trailing = variable.Identifier.TrailingTrivia;

//                    VariableDeclaratorSyntax newVariable = variable.WithIdentifier(SyntaxFactory.Identifier(leading, NameDone, trailing));

//                    var newRoot = root.ReplaceNode(variable, newVariable);
//                    return new[] { CodeAction.Create("Make upper", document.WithSyntaxRoot(newRoot)) };
//                }
//            }
//            foreach (var variable in localDeclaration.Declaration.Variables)
//            {
//                if (variable.Identifier.Text.IsUpperCase())
//                {
//                    var identifierToken = variable.Identifier.Text;
//                    var newName = "fixed" + identifierToken.TitleCaseString();
//                    // Get the symbol representing the type to be renamed.
//                    var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
//                    var typeSymbol = semanticModel.GetSymbolInfo(localDeclaration, cancellationToken);

//                    // Produce a new solution that has all references to that type renamed, including the declaration.
//                    var originalSolution = document.Project.Solution;
//                    var optionSet = originalSolution.Workspace.Options;
//                    var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol.Symbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

//                    // Return the new solution with the now-uppercase type name.
//                    return newSolution;
//                }
//            }
//            return null;
//        }
//    }
//}