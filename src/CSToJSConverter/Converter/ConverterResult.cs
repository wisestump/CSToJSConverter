using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSToJSConverter
{
    public class ConverterResult
    {
        private StringBuilder _result = new StringBuilder();

        /// <summary>
        /// Appends specified code to the result
        /// </summary>
        /// <param name="code">JavaScript source code</param>
        public void Append(string code) => _result.Append(code);

        /// <summary>
        /// Returns accumulated source code
        /// </summary>
        /// <returns></returns>
        public string GetCode() => _result.ToString();
    }
}
