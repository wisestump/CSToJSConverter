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
    }
}
