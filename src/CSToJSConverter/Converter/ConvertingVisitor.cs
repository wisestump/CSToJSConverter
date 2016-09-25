using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                    _result.Append(" " + node.OperatorToken.Text + " ");
                    Visit(node.Right);
                    break;
                default:
                    break;
            }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            AppendCode(node.Identifier.ValueText);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            AppendCode(node.Token.Text);
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            if (node.Expression is BinaryExpressionSyntax)
            {
                AppendCode(node.OpenParenToken.Text);
                Visit(node.Expression);
                AppendCode(node.CloseParenToken.Text);
            }
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            //var v = null;
            VariableDeclarationSyntax declaration = node.Declaration;
            //AppendCode(declaration.Type.)
        }
    }
}
