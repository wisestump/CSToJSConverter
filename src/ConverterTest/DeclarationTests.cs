using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ConverterTest.TestHelper;

namespace ConverterTest
{
    [TestClass]
    public class DeclarationTests
    {
        [TestMethod]
        public void VariablesTest()
        {
            var input = "var a;";
            AssertBlock(input, input);

            input = "var a = 4;";
            AssertBlock(input, input);

            input = "var a = 4, b, c = 5;";
            var expected =
@"var a = 4;
var b;
var c = 5;";
            AssertBlock(expected, input);

            input =
@"bool a = true, b = false;
bool c = a | b;";
            expected =
@"var a = true;
var b = false;
var c = a | b;";
            AssertBlock(expected, input);

            input =
@"int a = 1;
float b = 2.5;
var c = (a + b) * 3;";
            expected =
@"var a = 1;
var b = 2.5;
var c = (a + b) * 3;";
            AssertBlock(expected, input);
        }
    }
}
