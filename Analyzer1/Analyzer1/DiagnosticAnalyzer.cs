//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using System.Threading;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Diagnostics;
//using System.Globalization;
//namespace Analyzer1
//{
//    [DiagnosticAnalyzer(LanguageNames.CSharp)]
//    public class FirstAnalyzerCSAnalyzer : DiagnosticAnalyzer
//    {
//        public const string DiagnosticId = "MakeConstCS";
//        private const string Title = "Variable can be made constant";
//        private const string MessageFormat = "Can be made constant";
//        private const string Description = "Make Constant";
//        private const string Category = "Usage";

//        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

//        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

//        public override void Initialize(AnalysisContext context)
//        {
//            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
//        }

//        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
//        {
//            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;
//            foreach (var variable in localDeclaration.Declaration.Variables)
//            {
//                if(!variable.Identifier.Text.IsUpperCase())
//                {
//                    return;
//                }
//            }
//            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
//            //    // Only consider local variasble declarations that aren't already const.
//            //    if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
//            //{
//            //    return;
//            //}

//            //// Ensure that all variables in the local declaration have initializers that
//            //// are assigned with constant values.
//            //foreach (var variable in localDeclaration.Declaration.Variables)
//            //{
//            //    var initializer = variable.Initializer;
//            //    if (initializer == null)
//            //    {
//            //        return;
//            //    }

//            //    var constantValue = context.SemanticModel.GetConstantValue(initializer.Value);
//            //    if (!constantValue.HasValue)
//            //    {
//            //        return;
//            //    }
//            //}

//            //// Perform data flow analysis on the local declarartion.
//            //var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

//            //// Retrieve the local symbol for each variable in the local declaration
//            //// and ensure that it is not written outside of the data flow analysis region.
//            //foreach (var variable in localDeclaration.Declaration.Variables)
//            //{
//            //    var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);
//            //    if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
//            //    {
//            //        return;
//            //    }
//            //}
//        }
//    }
//}
