using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSToJSConverter;

namespace ConverterTest
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void ArithmeticTest()
        {
            var input = "(a ^ 2.5) * (((1 | 2) + (a & 3.0) / (1e10 / 1e-5)) - 0xff) % g * (1 >> 2) / (1 << 2)";
            var root = SyntaxBuilder.GetRootFromScript(input);
            var convertedText = ConvertingVisitor.Convert(root);
            Assert.AreEqual(input, convertedText);
        }

        [TestMethod]
        public void LogicTest()
        {
            var input = "a && (b || c) && true";
            var root = SyntaxBuilder.GetRootFromScript(input);
            var convertedText = ConvertingVisitor.Convert(root);
            Assert.AreEqual(input, convertedText);
        }
    }
}
