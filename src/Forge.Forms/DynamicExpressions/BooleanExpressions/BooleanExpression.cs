using System;

namespace Forge.Forms.DynamicExpressions.BooleanExpressions
{
    internal abstract class BooleanExpression
    {
        // Source: https://unnikked.ga/how-to-build-a-boolean-expression-evaluator-518e9e068a65
        private class Parser
        {
            private readonly Token[] tokens;
            private int index;

            private BooleanExpression root;

            private Token Current => index < tokens.Length ? tokens[index] : null;

            private Token Move()
            {
                index++;
                return Current;
            }

            public Parser(Token[] tokens)
            {
                this.tokens = tokens;
            }

            public BooleanExpression Parse()
            {
                /*
                 * <expression>::=<term>{<or><term>}
                 * <term>::=<factor>{<and><factor>}
                 * <factor>::=<constant>|<not><factor>|(<expression>)
                 * <constant>::= false|true
                 * <or>::='|'
                 * <and>::='&'
                 * <not>::='!'
                 */
                index = -1;
                Expression();
                return root;
            }

            private void Expression()
            {
                Term();
                while (Current is OrToken)
                {
                    var or = new OrOperator();
                    or.Left = root;
                    Term();
                    or.Right = root;
                    root = or;
                }
            }

            private void Term()
            {
                Factor();
                while (Current is AndToken)
                {
                    var and = new AndOperator();
                    and.Left = root;
                    Factor();
                    and.Right = root;
                    root = and;
                }
            }

            private void Factor()
            {
                var symbol = Move();
                if (symbol is ValueToken token)
                {
                    root = new ValueNode
                    {
                        Index = token.Index
                    };

                    Move();
                }
                else if (symbol is NotToken)
                {
                    var not = new NotOperator();
                    Factor();
                    not.Child = root;
                    root = not;
                }
                else if (symbol is LParenToken)
                {
                    Expression();
                    if (!(Current is RParenToken))
                    {
                        throw new FormatException("Expected closing paranthesis.");
                    }

                    Move();
                }
                else
                {
                    throw new FormatException("Invalid expression.");
                }
            }
        }

        public abstract bool Evaluate(bool[] values);

        public static BooleanExpression Parse(string expression)
        {
            var tokens = Token.Parse(expression);
            return new Parser(tokens).Parse();
        }
    }

    internal class AndOperator : BooleanExpression
    {
        public BooleanExpression Left { get; set; }

        public BooleanExpression Right { get; set; }

        public override bool Evaluate(bool[] values)
        {
            return Left.Evaluate(values) && Right.Evaluate(values);
        }
    }

    internal class OrOperator : BooleanExpression
    {
        public BooleanExpression Left { get; set; }

        public BooleanExpression Right { get; set; }

        public override bool Evaluate(bool[] values)
        {
            return Left.Evaluate(values) || Right.Evaluate(values);
        }
    }

    internal class NotOperator : BooleanExpression
    {
        public BooleanExpression Child { get; set; }

        public override bool Evaluate(bool[] values)
        {
            return !Child.Evaluate(values);
        }
    }

    internal class ValueNode : BooleanExpression
    {
        public int Index { get; set; }

        public override bool Evaluate(bool[] values)
        {
            if (Index >= values.Length)
            {
                return false;
            }

            return values[Index];
        }
    }
}
