using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ConverterTest.TestHelper;

namespace ConverterTest
{
    [TestClass]
    public class StatementTests
    {
        [TestMethod]
        public void OperatorIfTest()
        {
            var input =
@"if (age > 14)
{
access = true;
}";
            AssertBlock(input, input);

            input =
@"if (age > 14)
{
access = true;
}
else
{
access = false;
}";
            AssertBlock(input, input);

            input =
@"if (value > 0)
{
sign = 1;
}
else if (value < 0)
{
sign = -1;
}
else
{
sign = 0;
}";
            AssertBlock(input, input);



        }

        [TestMethod]
        public void CycleWhileTest()
        {
            var input =
@"int i = 0;
while(i < 10)
i += 1;";
            var expected =
@"var i = 0;
while (i < 10)
{
i += 1;
}";

            AssertBlock(expected, input);
            input =
@"int i = 1;
double fact = 1;
while(i < 10)
{
d *= i;
i += 1;
}";
            expected =
@"var i = 1;
var fact = 1;
while (i < 10)
{
d *= i;
i += 1;
}";
            AssertBlock(expected, input);

        }
    }
}
