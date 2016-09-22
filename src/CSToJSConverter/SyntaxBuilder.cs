using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSToJSConverter
{
    public class SyntaxBuilder
    {
        /// <summary>
        /// Parses C# top-level statements, declarations, and optional trailing expression
        /// </summary>
        /// <param name="script"></param>
        /// <returns>Root node</returns>
        public static CSharpSyntaxNode GetRootFromScript(string script)
            => 
            CSharpSyntaxTree
            .ParseText(
                text: script,
                options: new CSharpParseOptions(
                    LanguageVersion.CSharp6,
                    DocumentationMode.Parse,
                    SourceCodeKind.Script))
            .GetRoot() as CSharpSyntaxNode;

        /// <summary>
        /// Parses regular C# program code
        /// </summary>
        /// <param name="text">Root node</param>
        /// <returns></returns>
        public static CSharpSyntaxNode GetRootFromText(string text) 
            => 
            CSharpSyntaxTree
            .ParseText(text)
            .GetRoot() as CSharpSyntaxNode;
    }
}
