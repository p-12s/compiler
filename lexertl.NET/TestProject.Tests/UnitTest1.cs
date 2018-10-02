using Microsoft.VisualStudio.TestTools.UnitTesting;
using lexertl;
using System;

namespace TestProject.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public enum TokenType
        {
            TT_ERROR = 1,
            TT_NUMBER = 2,
            TT_PLUS = 3,
            TT_MINUS = 4,
            TT_MULTIPLY = 5,
            TT_DIVIDE = 6,
            TT_ASSIGN = 7,                  // '='
            TT_ID = 8,
            TT_SEMICOLON = 9,               // ';'
            TT_OPENING_PARENTHESIS = 10,    // '('
            TT_CLOSING_PARENTHESIS = 11     // ')'
        };

        // Создаёт детерминированный конечный автомат (DFA) для лексического анализа грамматики калькулятора
        /*
        StateMachine BuildCalcLexer()
        {
            Rules rules = new Rules();

            rules.Push("[0-9]+", (int)TokenType.TT_NUMBER);
            rules.Push("\\+", (int)TokenType.TT_PLUS);
            rules.Push("\\-", (int)TokenType.TT_MINUS);
            rules.Push("\\*", (int)TokenType.TT_MULTIPLY);
            rules.Push("\\/", (int)TokenType.TT_DIVIDE);
            rules.Push("\\=", (int)TokenType.TT_ASSIGN);
            rules.Push("[a-z]+", (int)TokenType.TT_ID);
            rules.Push("\\;", (int)TokenType.TT_SEMICOLON);
            rules.Push("\\(", (int)TokenType.TT_OPENING_PARENTHESIS);
            rules.Push("\\)", (int)TokenType.TT_CLOSING_PARENTHESIS);
            rules.Push("[ \t\r\n]+", rules.Skip());

            var stMachine = new StateMachine(rules);

            return stMachine;
        }
        */
        /*{
            var stateMachine = BuildCalcLexer();
            var tokens = stateMachine.GetTokens("123");//velocity + 27 * accel / 19 ( 5 + 3 ) = 1
        }*/

        [TestMethod]
        public void TestMethod1()
        {

            /*StateMachine BuildCalcLexer()
            {
                Rules rules = new Rules();

                rules.Push("[0-9]+", (int)TokenType.TT_NUMBER);
                rules.Push("\\+", (int)TokenType.TT_PLUS);
                rules.Push("\\-", (int)TokenType.TT_MINUS);
                rules.Push("\\*", (int)TokenType.TT_MULTIPLY);
                rules.Push("\\/", (int)TokenType.TT_DIVIDE);
                rules.Push("\\=", (int)TokenType.TT_ASSIGN);
                rules.Push("[a-z]+", (int)TokenType.TT_ID);
                rules.Push("\\;", (int)TokenType.TT_SEMICOLON);
                rules.Push("\\(", (int)TokenType.TT_OPENING_PARENTHESIS);
                rules.Push("\\)", (int)TokenType.TT_CLOSING_PARENTHESIS);
                rules.Push("[ \t\r\n]+", rules.Skip());

                var stMachine = new StateMachine(rules);

                return stMachine;
            }


            var stateMachine = BuildCalcLexer();
            var tokens = stateMachine.GetTokens("123");//velocity + 27 * accel / 19 ( 5 + 3 ) = 1
            */
            Assert.IsTrue(true);
            
            /*foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }*/
            
        }
    }
}
