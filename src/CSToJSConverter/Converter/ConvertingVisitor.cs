using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace CSToJSConverter
{
    /// <summary>
    /// Traverses C# syntax tree and generates corresponding JavaScript code
    /// </summary>
    public class ConvertingVisitor : CSharpSyntaxWalker
    {
        private ConverterResult _result = new ConverterResult();

        public static string Convert(CSharpSyntaxNode root)
        {
            var converter = new ConvertingVisitor();
            converter.Visit(root);

            return converter._result.GetCode();
        }

        private void AppendCode(string code) => _result.Append(code);

        private void AppendCodeLine() => _result.Append(Environment.NewLine);

        private void AppendCodeLine(string code) => _result.Append(code + Environment.NewLine);

        //private void AppendLeadingTrivia(CSharpSyntaxNode node)
        //{
        //    Func<SyntaxTrivia, bool> filter =
        //        t =>
        //        {
        //            switch (t.Kind())
        //            {
        //                case SyntaxKind.EndOfLineTrivia:
        //                case SyntaxKind.MultiLineCommentTrivia:
        //                case SyntaxKind.SingleLineCommentTrivia:
        //                    return true;
        //                default:
        //                    return false;
        //            }
        //        };

        //    foreach (var trivia in node.ChildTokens().First().LeadingTrivia.Where(filter))
        //        AppendCode(trivia.ToString());
        //}

        public override void DefaultVisit(SyntaxNode node)
        {
            throw new Exception($"Node {node.GetType()} is not supported");
        }

        public void Traverse(SyntaxNode node)
        {
            base.DefaultVisit(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            AppendCode($"function {node.Identifier.Text}(");
            node.ParameterList.Accept(this);
            AppendCodeLine(")");
            AppendCodeLine("{");
            node.Body.Accept(this);
            AppendCodeLine();
            AppendCode("}");
        }

        public override void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            Traverse(node);
        }

        public override void VisitParameterList(ParameterListSyntax node)
        {
            for (int i = 0; i < node.Parameters.Count - 1; i++)
            {
                node.Parameters[i].Accept(this);
                AppendCode(", ");
            }
            node.Parameters.Last().Accept(this);
        }

        public override void VisitParameter(ParameterSyntax node)
        {
            AppendCode(node.Identifier.Text);
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            AppendCode("return ");
            node.Expression.Accept(this);
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            Traverse(node);
        }

        public override void VisitGlobalStatement(GlobalStatementSyntax node)
        {
            Traverse(node);
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            Traverse(node);
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            // For now we use recursion to process binary expressions, 
            // but in case of large expression it may cause stack overflow
            switch (node.Kind())
            {
                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.ModuloExpression:
                case SyntaxKind.LeftShiftExpression:
                case SyntaxKind.RightShiftExpression:
                case SyntaxKind.BitwiseAndExpression:
                case SyntaxKind.BitwiseOrExpression:
                case SyntaxKind.ExclusiveOrExpression:
                case SyntaxKind.LogicalAndExpression:
                case SyntaxKind.LogicalOrExpression:
                    Visit(node.Left);
                    AppendCode(" " + node.OperatorToken.Text + " ");
                    Visit(node.Right);
                    break;
                default:
                    break;
            }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            AppendCode(node.Identifier.Text);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            AppendCode(node.Token.Text);
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            AppendCode(node.OpenParenToken.Text);
            Visit(node.Expression);
            AppendCode(node.CloseParenToken.Text);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            
            Action<VariableDeclaratorSyntax> DeclaratorProcessor = declarator =>
            {
                string identifier = declarator.Identifier.Text;
                if (declarator.Initializer == null)
                    AppendCode($"var {identifier}");
                else
                {
                    AppendCode($"var {identifier} = ");
                    Visit(declarator.Initializer.Value);
                }
            };

            var variables = node.Declaration.Variables;
            DeclaratorProcessor(variables[0]);
            for (int i = 1; i < variables.Count; i++)
            {
                AppendCodeLine(";");
                DeclaratorProcessor(variables[i]);
            }
        }

        public override void VisitBlock(BlockSyntax node)
        {
            var statements = node.Statements;
            for (int i = 0; i < statements.Count - 1; i++)
            {
                Visit(statements[i]);
                AppendCodeLine(";");
            }
            Visit(statements.Last());
            AppendCode(";");
        }
    }
}
