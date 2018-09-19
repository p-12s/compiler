using ManualLexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
    /*public string GetTokenName(TokenType type)
    {
        switch (type)
        {
            case TokenType.TT_END:
                return "end";
            case TokenType.TT_ERROR:
                return "error";
            case TokenType.TT_NUMBER:
                return "number";
            case TokenType.TT_PLUS:
                return "+";
            case TokenType.TT_MINUS:
                return "-";
            case TokenType.TT_MULTIPLY:
                return "*";
            case TokenType.TT_DIVIDE:
                return "/";
            case TokenType.TT_ASSIGN:
                return "=";
            case TokenType.TT_ID:
                return "identifier";
            case TokenType.TT_SEMICOLON:
                return ";";
            case TokenType.TT_OPENING_PARENTHESIS:
                return "(";
            case TokenType.TT_CLOSING_PARENTHESIS:
                return ")";
        }
        return "<UNEXPECTED!!!>";
    }*/
}

namespace ManualLexer.Tests
{

    [TestClass]
    public class CalcLexer_Tests
    {
        List<Token> Tokenize(string text)
        {
            List<Token> results = new List<Token>();
            var calLexer = new CalcLexer(text);

            for (Token token = calLexer.Read(); token.type != TokenType.TT_END; token = calLexer.Read())
            {
                results.Add(token);
            }
            return results;
        }

        public void AreTokensEqual(string str, List<Token> list)
        {
            Assert.IsTrue(Tokenize(str).SequenceEqual(list));
        }

        [TestMethod]
        public void CanReadOneNumber()
        {
            AreTokensEqual("1.",
                new List<Token> {
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("# 1!",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_NUMBER, "1"),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("22. \t * 22.22@",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "22.22"),
                    new Token(TokenType.TT_ERROR)
                }
            );
            
            AreTokensEqual("0",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "0")
                }
            );

            AreTokensEqual("1",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual("9876543210",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "9876543210")
                }
            );

            AreTokensEqual("0 $$$ $",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "0"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("0 % $",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "0"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("^2^ 10 % $ & *",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_NUMBER, "2"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_NUMBER, "10"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_MULTIPLY)
                }
            );
        }

        [TestMethod]
        public void CanReadOneOperator()
        {
            AreTokensEqual("+",
                new List<Token> { new Token(TokenType.TT_PLUS) });

            AreTokensEqual("-",
                new List<Token> { new Token(TokenType.TT_MINUS) });

            AreTokensEqual("*",
                new List<Token> { new Token(TokenType.TT_MULTIPLY) });

            AreTokensEqual("/",
                new List<Token> { new Token(TokenType.TT_DIVIDE) });
        }

        [TestMethod]
        public void CanReadExpressionTokens()
        {
            AreTokensEqual("45+9+28",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "45"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "9"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual("45-9-28",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "45"),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "9"),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual("45*9*28",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "45"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "9"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual("45/9/28",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "45"),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "9"),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual("45/9+28-54*4",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "45"),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "9"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "28"),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "54"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "4")
                }
            );

            AreTokensEqual("5+7.005",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "7.005")
                }
            );

            AreTokensEqual("1.005+43.54+1",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "1.005"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "43.54"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual("1940.1851/4.000149*1",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "1940.1851"),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "4.000149"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual("05-00.2/0.1*0",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "0.1"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "0")
                }
            );
        }

        [TestMethod]
        public void CanReadOneOperatorWithWhitespaces()
        {
            AreTokensEqual("  +",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("\t+",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("   \t\t+",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("\n+",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("   \n  +",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("\t   \n +",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("+    ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("+  \t\t   ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("+  \n\t   ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("   +   ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("  \t +  \t ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("  \t\t +  \t ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS)
                }
            );

            AreTokensEqual("  +\t 1\t +  \n\t ",
                new List<Token> {
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "1"),
                    new Token(TokenType.TT_PLUS)
                }
            );
        }

        [TestMethod]
        public void CanReadOneNumberWithWhitespaces()
        {
            AreTokensEqual("  1",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual("\t4",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "4")
                }
            );

            AreTokensEqual("\t\t3.2",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "3.2")
                }
            );

            AreTokensEqual("\n9",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "9")
                }
            );

            AreTokensEqual("   \n  15",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "15")
                }
            );

            AreTokensEqual("\t   \n  21.03",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "21.03")
                }
            );

            AreTokensEqual("0    ",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "0")
                }
            );

            AreTokensEqual("81  \t\t   ",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "81")
                }
            );

            AreTokensEqual("4.2  \n\t   ",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "4.2")
                }
            );

            AreTokensEqual("   7.9   ",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "7.9")
                }
            );

            AreTokensEqual("  \t 3  \t ",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "3")
                }
            );

            AreTokensEqual("  \t\t 9.001  \t ",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "9.001")
                }
            );
        }

        [TestMethod]
        public void CanReadExpressionTokensWithWhitespaces()
        {
            AreTokensEqual("2 + 3",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "2"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "3")
                }
            );

            AreTokensEqual("\t0.52 * \n4",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "0.52"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "4")
                }
            );

            AreTokensEqual("\n+ \t7.1",
                new List<Token> {
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "7.1")
                }
            );
        }

        [TestMethod]
        public void CannotReadNumberWhichStartsWithZero()
        {
            AreTokensEqual("0123456789",
                new List<Token> {
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("0.123.456789",
                new List<Token> {
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("0..456789",
                new List<Token> {
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("0.. 456789",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_NUMBER, "456789")
                }
            );

            AreTokensEqual("01.25",
                new List<Token> {
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("+01",
                new List<Token> {
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("-00.32",
                new List<Token> {
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("4*0521",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "4"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("02/21",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "21")
                }
            );

            AreTokensEqual("02.4+5.3",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "5.3")
                }
            );
        }

        [TestMethod]
        public void CanReadAssigneSemicolonOpeningAndClosingParenthesisTokens()
        {
            AreTokensEqual("5=5",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_NUMBER, "5")
                }
            );

            AreTokensEqual("; 5;5;",
                new List<Token> {
                    new Token(TokenType.TT_SEMICOLON),
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_SEMICOLON),
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_SEMICOLON)
                }
            );

            AreTokensEqual("(5)",
                new List<Token> {
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS)
                }
            );

            AreTokensEqual("()",
                new List<Token> {
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS)
                }
            );
        }

        [TestMethod]
        public void CanReadIdentifierToken()
        {
            AreTokensEqual("0123@ 123 @",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_NUMBER, "123"),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("123@ 123 @",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "123"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_NUMBER, "123"),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("abc%^^f",
                new List<Token> {
                    new Token(TokenType.TT_ID, "abc"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ID, "f"),
                }
            );

            AreTokensEqual("1abc abc1",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ID, "abc1"),
                }
            );

            AreTokensEqual("_1abc abc1",
                new List<Token> {
                    new Token(TokenType.TT_ID, "_1abc"),
                    new Token(TokenType.TT_ID, "abc1"),
                }
            );

            AreTokensEqual("var",
                new List<Token> {
                    new Token(TokenType.TT_ID, "var")
                }
            );

            AreTokensEqual("1abc",
                new List<Token> {
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("_abc12",
                new List<Token> {
                    new Token(TokenType.TT_ID, "_abc12"),
                }
            );

            AreTokensEqual("_",
                new List<Token> {
                    new Token(TokenType.TT_ID, "_")
                }
            );

            AreTokensEqual("_1",
                new List<Token> {
                    new Token(TokenType.TT_ID, "_1")
                }
            );

            AreTokensEqual("1abc + 5",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "5")
                }
            );
        }

        [TestMethod]
        public void CanReadExpression()
        {
            AreTokensEqual("a=5",
                new List<Token> {
                    new Token(TokenType.TT_ID, "a"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_NUMBER, "5")
                }
            );

            AreTokensEqual("a=b",
                new List<Token> {
                    new Token(TokenType.TT_ID, "a"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_ID, "b")
                }
            );

            AreTokensEqual("_=3.2",
                new List<Token> {
                    new Token(TokenType.TT_ID, "_"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_NUMBER, "3.2")
                }
            );

            AreTokensEqual("variable=0.2222",
                new List<Token> {
                    new Token(TokenType.TT_ID, "variable"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_NUMBER, "0.2222")
                }
            );

            AreTokensEqual("a= (1+3)-    4 / 5;",
                new List<Token> {
                    new Token(TokenType.TT_ID, "a"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_NUMBER, "1"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "3"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "4"),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_SEMICOLON)
                }
            );

            AreTokensEqual("( 1 + b)",
                new List<Token> {
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_NUMBER, "1"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_ID, "b"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS)
                }
            );

            AreTokensEqual("b1 1",
                new List<Token> {
                    new Token(TokenType.TT_ID, "b1"),
                    new Token(TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual("_1+=",
                new List<Token> {
                    new Token(TokenType.TT_ID, "_1"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_ASSIGN)
                }
            );

            AreTokensEqual("var = (1+ b  - (y - 2) / 3) * (i + 5)",
                new List<Token> {
                    new Token(TokenType.TT_ID, "var"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_NUMBER, "1"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_ID, "b"),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_ID, "y"),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "2"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_NUMBER, "3"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_ID, "i"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS)
                }
            );

            AreTokensEqual("k = 4 / (8 + 1) - 5 * 9",
                new List<Token> {
                    new Token(TokenType.TT_ID, "k"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_NUMBER, "4"),
                    new Token(TokenType.TT_DIVIDE),
                    new Token(TokenType.TT_OPENING_PARENTHESIS),
                    new Token(TokenType.TT_NUMBER, "8"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "1"),
                    new Token(TokenType.TT_CLOSING_PARENTHESIS),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "5"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_NUMBER, "9")
                }
            );

            AreTokensEqual("~ _ abc%f. y34. =  3.",
                new List<Token> {
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ID, "_"),
                    new Token(TokenType.TT_ID, "abc"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ID, "f"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ID, "y34"),
                    new Token(TokenType.TT_ERROR),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_ERROR)
                }
            );

            AreTokensEqual("y34 = x * x     + 3 * x - 17;",
                new List<Token> {
                    new Token(TokenType.TT_ID, "y34"),
                    new Token(TokenType.TT_ASSIGN),
                    new Token(TokenType.TT_ID, "x"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_ID, "x"),
                    new Token(TokenType.TT_PLUS),
                    new Token(TokenType.TT_NUMBER, "3"),
                    new Token(TokenType.TT_MULTIPLY),
                    new Token(TokenType.TT_ID, "x"),
                    new Token(TokenType.TT_MINUS),
                    new Token(TokenType.TT_NUMBER, "17"),
                    new Token(TokenType.TT_SEMICOLON)
                }
            );
        }

        [TestMethod]
        public void Test()
        {/*
            AreTokensEqual("EEEEEEE",
                new List<Token> {
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE"),
                    new Token(TokenType.TT_NUMBER, "EEEEEE")
                }
            );
            */
        }
    }
}
