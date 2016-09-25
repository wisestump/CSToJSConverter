using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSToJSConverter;
using static ConverterTest.TestHelper;

namespace ConverterTest
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void ArithmeticTest()
        {
            var input = "(a ^ 2.5) * (((1 | 2) + (a & 3.0) / (1e10 / 1e-5)) - 0xff) % g * (1 >> 2) / (1 << 2)";
            AssertScript(input, input);

            input = "(a \n + \n b ) * c";
            AssertScript("(a + b) * c", input);
        }

        [TestMethod]
        public void LogicTest()
        {
            var input = "a && (b || c) && true";
            AssertScript(input, input);
        }
    }
}
