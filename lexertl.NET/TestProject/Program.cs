using System;
using lexertl;

namespace TestProject
{



    class Program
    {

        static void Main(string[] args)
        {
            // Создаёт детерминированный конечный автомат (DFA) для лексического анализа грамматики калькулятора


            /*Token[] tokens = stateMachine.GetTokens("a=123+256");

            for (var i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine(tokens[i].Id + " " + tokens[i].Value);
            }*/


            // ХОЧУ ПРОГРАММУ-КАЛЬКУЛЯТОР

            Parser parser = new Parser();
            parser.Run(); 

            /*
            while (cin) // 
            {
                get_token();
                if (curr_tok == END) break;
                if (curr_tok == PRINT) continue;
                cout << expr(false) << '\n';
            }
            */

            // собрать дерево по токенам
            // вычислить


            // итак, у меня есть цикл считывания токенов.
            // осталось экспрешен по-кусочку просчитывать


            /*
            string input = string.Empty;
            int pointerToToken = 0;

            while (true)
            {
                input = Console.ReadLine();
                Token[] tokens = stateMachine.GetTokens(input);

                foreach (var token in tokens)
                {
                    if (token.Id == 0) break;
                    if (token.Id == (int) TokenType.TT_PRINT) continue;
                    // считал токен - го в экспрешен

                    // на токен конца не проверяем, т.к. он недоступен для сравнения
                    //if (currToken == TokenType.TT_SEMICOLON) continue;

                    Console.WriteLine(token.Id + " " + token.Value);

                    Console.WriteLine(Expr(false, tokens, ref pointerToToken));

                    //Console.WriteLine(Expr(false)); cout << expr(false) << '\n';


                }
                i = 0;
            }*/

            Console.ReadKey();

        }
    }
}
