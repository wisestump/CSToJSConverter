using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ConverterTest.TestHelper;

namespace ConverterTest
{
    [TestClass]
    public class FunctionTests
    {

        [TestMethod]
        public void FunctionDeclarationTest()
        {
            var input = @"
public int sum(int a, int b)
{
  var result = a + b;
  return result;
}";
            var expected =
@"function sum(a, b)
{
var result = a + b;
return result;
}";
            AssertDeclarations(expected, input);
        }
    }
}
