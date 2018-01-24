using System;

namespace Forge.Forms.DynamicExpressions.BooleanExpressions
{
    internal abstract class AstNode
    {
        // Source: https://unnikked.ga/how-to-build-a-boolean-expression-evaluator-518e9e068a65
        class Parser
        {
            private readonly Token[] tokens;
            private int index;

            private AstNode root;

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

            public AstNode Parse()
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
                index = 0;
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
                    symbol = Move();
                    if (!(symbol is RParenToken))
                    {
                        throw new FormatException("Expected closing paranthesis.");
                    }
                }
                else
                {
                    throw new FormatException("Invalid expression.");
                }
            }
        }

        public abstract bool Evaluate(bool[] values);

        public static AstNode Parse(string expression)
        {
            var tokens = Token.Parse(expression);
            return new Parser(tokens).Parse();
        }
    }

    internal class AndOperator : AstNode
    {
        public AstNode Left { get; set; }

        public AstNode Right { get; set; }

        public override bool Evaluate(bool[] values)
        {
            return Left.Evaluate(values) && Right.Evaluate(values);
        }
    }

    internal class OrOperator : AstNode
    {
        public AstNode Left { get; set; }

        public AstNode Right { get; set; }

        public override bool Evaluate(bool[] values)
        {
            return Left.Evaluate(values) || Right.Evaluate(values);
        }
    }

    internal class NotOperator : AstNode
    {
        public AstNode Child { get; set; }

        public override bool Evaluate(bool[] values)
        {
            return !Child.Evaluate(values);
        }
    }

    internal class ValueNode : AstNode
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
