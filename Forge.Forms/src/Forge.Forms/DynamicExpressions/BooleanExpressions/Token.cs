using System;
using System.Collections.Generic;

namespace Forge.Forms.DynamicExpressions.BooleanExpressions
{
    internal abstract class Token
    {
        public static Token[] Parse(string input)
        {
            var chars = input.ToCharArray();
            var tokens = new List<Token>();
            var index = 0;
            char next;
            char EnsureNext()
            {
                if (index >= chars.Length)
                {
                    throw new FormatException("Unexpected end of input.");
                }

                return next = chars[index++];
            }

            while (index < chars.Length)
            {
                next = chars[index++];
                switch (next)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                        continue;
                    case '(':
                        tokens.Add(new LParenToken());
                        break;
                    case ')':
                        tokens.Add(new RParenToken());
                        break;
                    case '&':
                        EnsureNext();
                        if (next != '&')
                        {
                            throw new FormatException("Invalid symbol '&'");
                        }

                        tokens.Add(new AndToken());
                        break;
                    case '|':
                        EnsureNext();
                        if (next != '|')
                        {
                            throw new FormatException("Invalid symbol '|'");
                        }

                        tokens.Add(new OrToken());
                        break;
                    case '!':
                        tokens.Add(new NotToken());
                        break;
                    case '{':
                        var id = "";
                        while (char.IsDigit(EnsureNext()))
                        {
                            id += next;
                        }

                        if (next != '}')
                        {
                            while (EnsureNext() != '}')
                            {
                            }
                        }

                        tokens.Add(new ValueToken
                        {
                            Index = int.Parse(id)
                        });
                        break;
                    default:
                        throw new FormatException("Invalid input sequence.");
                }
            }

            return tokens.ToArray();
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

    internal class LParenToken : Token
    {
    }

    internal class RParenToken : Token
    {
    }
}
