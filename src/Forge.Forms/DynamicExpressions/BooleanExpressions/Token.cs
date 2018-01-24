using System;
using System.Collections.Generic;
using System.IO;

namespace Forge.Forms.DynamicExpressions.BooleanExpressions
{
    internal abstract class Token
    {
        public Token[] Parse(string input)
        {
            var result = new List<Token>();
            using (var reader = new StringReader(input))
            {
                throw new NotImplementedException();
            }

            return result.ToArray();
        }
    }

    internal class AndToken : Token
    {
    }

    internal class OrToken : Token
    {
    }

    internal class NotToken : Token
    {
    }

    internal class ValueToken : Token
    {
        public int Index { get; set; }
    }
}
