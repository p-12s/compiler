using System.Collections.Generic;
using lexertl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LexertlNetTest
{
    // библиотека взята тут:
    // https://github.com/KonstantinGeist/lexertl.NET/

    [TestClass]
    public class LexertlNetTests
    {
        enum TokenType
        {
            TT_ERROR = 1,
            TT_NUMBER = 2,
            TT_PLUS = 3,
            TT_MINUS = 4,
            TT_MULTIPLY = 5,
            TT_DIVIDE = 6,
            TT_ASSIGN = 7,
            TT_ID = 8,
            TT_SEMICOLON = 9,
            TT_OPENING_PARENTHESIS = 10,
            TT_CLOSING_PARENTHESIS = 11
        };

        StateMachine BuildCalcLexer()
        {
            Rules rules = new Rules();
            
            rules.Push("[1-9]+[\\d]*[.]{1}[\\d]+|0{1}[.]{1}[\\d]+|[1-9]{1}[\\d]+|[\\d]{1}", (int)TokenType.TT_NUMBER);
            rules.Push("\\+", (int)TokenType.TT_PLUS);
            rules.Push("\\-", (int)TokenType.TT_MINUS);
            rules.Push("\\*", (int)TokenType.TT_MULTIPLY);
            rules.Push("\\/", (int)TokenType.TT_DIVIDE);
            rules.Push("\\=", (int)TokenType.TT_ASSIGN);
            rules.Push("[a-zA-Z_]{1}[\\w]*", (int)TokenType.TT_ID);
            rules.Push("\\;", (int)TokenType.TT_SEMICOLON);
            rules.Push("\\(", (int)TokenType.TT_OPENING_PARENTHESIS);
            rules.Push("\\)", (int)TokenType.TT_CLOSING_PARENTHESIS);
            rules.Push("0[\\d]*(\\.[\\d]*)+|0[\\d]+|[\\d]+[\\w]*|[\\d]+[.]+", (int)TokenType.TT_ERROR);
            rules.Push("[ \t\r\n]+", rules.Skip());

            var stMachine = new StateMachine(rules);

            return stMachine;
        }

        StateMachine _stateMachine;

        public void AreTokensEqual(Token[] tokens, List<Token> list)
        {
            for (var i = 0; i < tokens.Length; i++)
            {
                Assert.IsTrue(tokens[i].Equals(list[i]));
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _stateMachine = BuildCalcLexer();
        }

        // TODO юзинг: создавать каждый раз и передавать ему правила и строку
        [TestMethod()]
        public void CanReadOneNumber()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("123"),
                new List<Token>
                {
                    new Token((int)TokenType.TT_NUMBER, "123")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("9876543210"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "9876543210")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("# 1!"),
                new List<Token>
                {
                    new Token(-1, "#"),
                    new Token((int)TokenType.TT_NUMBER, "1"),
                    new Token(-1, "!")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("123.0678 \t "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "123.0678")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("22. \t * 22.22@"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "22."),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "22.22"),
                    new Token(-1, "@"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0 $$$ $"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "0"),
                    new Token(-1, "$"),
                    new Token(-1, "$"),
                    new Token(-1, "$"),
                    new Token(-1, "$")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0 % $"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "0"),
                    new Token(-1, "%"),
                    new Token(-1, "$")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("^2^ 10 % $ & *"),
                new List<Token> {
                    new Token(-1, "^"),
                    new Token((int)TokenType.TT_NUMBER, "2"),
                    new Token(-1, "^"),
                    new Token((int)TokenType.TT_NUMBER, "10"),
                    new Token(-1, "%"),
                    new Token(-1, "$"),
                    new Token(-1, "&"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                }
            );
        }

        [TestMethod()]
        public void CanReadOneOperator()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("+"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("-"),
                new List<Token> {
                    new Token((int)TokenType.TT_MINUS, "-"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("*"),
                new List<Token> {
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("/"),
                new List<Token> {
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                }
            );
        }

        [TestMethod]
        public void CanReadExpressionTokens()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("45+9+28"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "45"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "9"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("45-9-28"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "45"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "9"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("45*9*28"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "45"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "9"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("45/9/28"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "45"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "9"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "28")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("45/9+28-54*4"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "45"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "9"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "28"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "54"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "4")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("5+7.005"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "7.005")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("1.005+43.54+1"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "1.005"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "43.54"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("1940.1851/4.000149*1"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "1940.1851"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "4.000149"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("05-00.2/0.1*0"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "05"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_ERROR, "00.2"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "0.1"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "0")
                }
            );
        }

        [TestMethod]
        public void CanReadOneOperatorWithWhitespaces()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("  +"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\t+"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("   \t\t+"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\n+"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("   \n  +"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\t   \n +"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("+    "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("+  \t\t   "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("+  \n\t   "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("   +   "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("  \t +  \t "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("  \t\t +  \t "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("  +\t 1\t +  \n\t "),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "1"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                }
            );
        }

        [TestMethod]
        public void CanReadOneNumberWithWhitespaces()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("  1"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\t4"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "4")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\t\t3.2"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "3.2")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\n9"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "9")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("   \n  15"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "15")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\t   \n  21.03"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "21.03")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0    "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "0")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("81  \t\t   "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "81")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("4.2  \n\t   "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "4.2")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("   7.9   "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "7.9")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("  \t 3  \t "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "3")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("  \t\t 9.001  \t "),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "9.001")
                }
            );
        }

        [TestMethod]
        public void CanReadExpressionTokensWithWhitespaces()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("2 + 3"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "2"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "3")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\t0.52 * \n4"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "0.52"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "4")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("\n+ \t7.1"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "7.1")
                }
            );
        }


        [TestMethod]
        public void CannotReadNumberWhichStartsWithZero()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("0123456789"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "0123456789"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0.123.456789 0.123..456789 0.123.....456789"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "0.123.456789"),
                    new Token((int)TokenType.TT_ERROR, "0.123..456789"),
                    new Token((int)TokenType.TT_ERROR, "0.123.....456789"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0.123.456789.456789 0.123..456789..456789 0.123.....456789.....456789"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "0.123.456789.456789"),
                    new Token((int)TokenType.TT_ERROR, "0.123..456789..456789"),
                    new Token((int)TokenType.TT_ERROR, "0.123.....456789.....456789"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0..456789"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "0..456789"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("0.. 456789"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "0.."),
                    new Token((int)TokenType.TT_NUMBER, "456789")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("01.25"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "01.25"),
                }
            );
            
            AreTokensEqual(
                _stateMachine.GetTokens("+01"),
                new List<Token> {
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_ERROR, "01"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("-00.32"),
                new List<Token> {
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_ERROR, "00.32"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("4*0521"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "4"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_ERROR, "0521"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("02/21"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "02"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "21")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("02.4+5.3"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "02.4"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "5.3")
                }
            );
        }

        [TestMethod]
        public void CanReadAssigneSemicolonOpeningAndClosingParenthesisTokens()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("5=5"),
                new List<Token> {
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_NUMBER, "5")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("; 5;5;"),
                new List<Token> {
                    new Token((int)TokenType.TT_SEMICOLON, ";"),
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_SEMICOLON, ";"),
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_SEMICOLON, ";")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("(5)"),
                new List<Token> {
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS,")")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("()"),
                new List<Token> {
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")")
                }
            );
        }

        [TestMethod]
        public void CanReadIdentifierToken()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("0123@ 123 @"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "0123"),
                    new Token(-1, "@"),
                    new Token((int)TokenType.TT_NUMBER, "123"),
                    new Token(-1, "@"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("abc%^^f"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "abc"),
                    new Token(-1, "%"),
                    new Token(-1, "^"),
                    new Token(-1, "^"),
                    new Token((int)TokenType.TT_ID, "f"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("1abc abc1"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "1abc"),
                    new Token((int)TokenType.TT_ID, "abc1"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("_1abc abc1"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "_1abc"),
                    new Token((int)TokenType.TT_ID, "abc1"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("var"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "var")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("1abc"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "1abc")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("_abc12"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "_abc12"),
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("_"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "_")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("_1"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "_1")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("1abc + 5"),
                new List<Token> {
                    new Token((int)TokenType.TT_ERROR, "1abc"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "5")
                }
            );
        }

        [TestMethod]
        public void CanReadExpression()
        {
            AreTokensEqual(
                _stateMachine.GetTokens("a=5"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "a"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_NUMBER, "5")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("a=b"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "a"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_ID, "b")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("_=3.2"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "_"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_NUMBER, "3.2")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("variable=0.2222"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "variable"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_NUMBER, "0.2222")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("a= (1+3)-    4 / 5;"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "a"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_NUMBER, "1"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "3"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "4"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_SEMICOLON, ";")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("( 1 + b)"),
                new List<Token> {
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_NUMBER, "1"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_ID, "b"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("b1 1"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "b1"),
                    new Token((int)TokenType.TT_NUMBER, "1")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("_1+="),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "_1"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_ASSIGN, "=")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("var = (1+ b  - (y - 2) / 3) * (i + 5)"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "var"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_NUMBER, "1"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_ID, "b"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_ID, "y"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "2"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_NUMBER, "3"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_ID, "i"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("k = 4 / (8 + 1) - 5 * 9"),
                new List<Token> {
                    new Token((int)TokenType.TT_ID, "k"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_NUMBER, "4"),
                    new Token((int)TokenType.TT_DIVIDE, "/"),
                    new Token((int)TokenType.TT_OPENING_PARENTHESIS, "("),
                    new Token((int)TokenType.TT_NUMBER, "8"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "1"),
                    new Token((int)TokenType.TT_CLOSING_PARENTHESIS, ")"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "5"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_NUMBER, "9")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("~ _ abc%f. y34. =3."),
                new List<Token> {
                    new Token(-1, "~"),
                    new Token((int)TokenType.TT_ID, "_"),
                    new Token((int)TokenType.TT_ID, "abc"),
                    new Token(-1, "%"),
                    new Token((int)TokenType.TT_ID, "f"),
                    new Token(-1, "."),
                    new Token((int)TokenType.TT_ID, "y34"),
                    new Token(-1, "."),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_ERROR, "3.")
                }
            );

            AreTokensEqual(
                _stateMachine.GetTokens("!~&y34 = x * x     + 3 * x - 17;"),
                new List<Token> {
                    new Token(-1, "!"),
                    new Token(-1, "~"),
                    new Token(-1, "&"),
                    new Token((int)TokenType.TT_ID, "y34"),
                    new Token((int)TokenType.TT_ASSIGN, "="),
                    new Token((int)TokenType.TT_ID, "x"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_ID, "x"),
                    new Token((int)TokenType.TT_PLUS, "+"),
                    new Token((int)TokenType.TT_NUMBER, "3"),
                    new Token((int)TokenType.TT_MULTIPLY, "*"),
                    new Token((int)TokenType.TT_ID, "x"),
                    new Token((int)TokenType.TT_MINUS, "-"),
                    new Token((int)TokenType.TT_NUMBER, "17"),
                    new Token((int)TokenType.TT_SEMICOLON, ";")
                }
            );
        }


    }
}
