using System;
using System.Collections.Generic;
using lexertl;
using static TestProject.Program;

namespace TestProject
{

    public class MyGlobalClass
    {
        public TokenType currToken = TokenType.TT_SEMICOLON;
        double numberValue;
        string stringValue;

        public static double Term(bool get)       // multiply and divide
        {
            double left = Prim(get);

            for (; ; )
                switch (curr_tok)
                {
                    case MUL:
                        left *= prim(true);
                        break;
                    case DIV:
                        if (double d = prim(true)) {
                left /= d;
                break;
            }
            return error("divide by 0");
            default:
			    return left;
        }
    

        public static double Expr(bool get)       // add and subtract
        {
            double left = Term(get);

            for (; ; )              // ``forever''
                switch (curr_tok)
                {
                    case PLUS:
                        left += term(true);
                        break;
                    case MINUS:
                        left -= term(true);
                        break;
                    default:
                        return left;
                }
        };

    }

class Program
    {
        public enum TokenType //Token_value
        {
            TT_ERROR = -1,
            TT_NUMBER = 2,
            TT_PLUS = 3,
            TT_MINUS = 4,
            TT_MULTIPLY = 5,
            TT_DIVIDE = 6,
            TT_ASSIGN = 7,                  // '='
            TT_ID = 8,
            TT_SEMICOLON = 9,               // ';' PRINT
            TT_OPENING_PARENTHESIS = 10,    // '(' LP
            TT_CLOSING_PARENTHESIS = 11     // ')' RP
        };

        












        static void Main(string[] args)
        {
            // Создаёт детерминированный конечный автомат (DFA) для лексического анализа грамматики калькулятора

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

            var stateMachine = BuildCalcLexer();


            


            
            /*
            var tokens = stateMachine.GetTokens("velocity + 27 * accel / 19 ( 5 + 3 ) = 1");

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }*/



            /*while (cin)
            {
                get_token();
                if (curr_tok == END) break;
                if (curr_tok == PRINT) continue;
                cout << expr(false) << '\n';
            }*/

            // собрать дерево по токенам
            // вычислить 




            // итак, у меня есть цикл считывания токенов.
            // осталось экспрешен по-кусочку просчитывать

            string input = string.Empty;
            bool isInputComplete = false;
            while (!isInputComplete)
            {
                input = Console.ReadLine();
                var tokens = stateMachine.GetTokens(input);

                foreach (var token in tokens)
                {
                    // на токен конца не проверяем, т.к. он недоступен для сравнения
                    if (currToken == TokenType.TT_SEMICOLON) continue;

                    Console.WriteLine(Expr(false));
                }

            }
            
            Console.ReadKey();

        }
    }
}
