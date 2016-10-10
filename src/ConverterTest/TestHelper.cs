using CSToJSConverter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterTest
{
    internal class TestHelper
    {
        /// <summary>
        /// Checks if converted C# script matches given JavaScript code
        /// </summary>
        /// <param name="expected">JavaScript code</param>
        /// <param name="script">C# source script</param>
        /// <remarks>Script allows C# top-level statements, declarations, and optional trailing expression</remarks>
        internal static void AssertScript(string expected, string script)
        {
            CSharpSyntaxNode root = SyntaxBuilder.GetRootFromScript(script);
            string convertedText = ConvertingVisitor.Convert(root);
            Assert.AreEqual(expected, convertedText);
        }

        /// <summary>
        /// Checks if converted C# statement list matches given JavaScript code
        /// </summary>
        /// <param name="expected">JavaScript code</param>
        /// <param name="block">C# statement list</param>
        internal static void AssertBlock(string expected, string block)
        {
            CSharpSyntaxNode root = SyntaxBuilder.GetRootFromProgram(WrapWithMainFunction(block));
            BlockSyntax statements = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .First(method => method.Identifier.Text == "Main")
                .Body;

            string convertedText = ConvertingVisitor.Convert(statements);
            Assert.AreEqual(expected, convertedText);
        }

        /// <summary>
        /// Checks if converted C# declarations matches given JavaScript code
        /// </summary>
        /// <param name="expected">JavaScript code</param>
        /// <param name="block">C# statement list</param>
        internal static void AssertDeclarations(string expected, string block)
        {
            CSharpSyntaxNode root = SyntaxBuilder.GetRootFromProgram(WrapWithMainFunction(block));
            List<MethodDeclarationSyntax> declarations = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(method => method.Identifier.Text != "Main")
                .ToList();

            string convertedText = declarations
                .Select(ConvertingVisitor.Convert)
                .Aggregate(string.Concat);
            Assert.AreEqual(expected, convertedText);
        }

        /// <summary>
        /// Wraps given code with C# entry point
        /// </summary>
        /// <param name="code">C# main function body</param>
        /// <returns></returns>
        internal static string WrapWithMainFunction(string code)
        {
            return
$@"namespace DummyNamespace
{{
    class Program
    {{
        static void Main(string[] args)
        {{
            {code}
        }}
    }}
}}";
        }
    }
}
