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
                    _result.Append(node.OperatorToken.ToFullString());
                    Visit(node.Right);
                    break;
                default:
                    break;
            }
        }

        public static string Convert(CSharpSyntaxNode root)
        {
            var converter = new ConvertingVisitor();
            converter.Visit(root);

            return converter._result.GetCode();
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            _result.Append(node.Identifier.ToFullString());
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            _result.Append(node.Token.ToFullString());
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            if (node.Expression is BinaryExpressionSyntax)
            {
                _result.Append(node.OpenParenToken.ToFullString());
                Visit(node.Expression);
                _result.Append(node.CloseParenToken.ToFullString());
            }
        }
    }
}
