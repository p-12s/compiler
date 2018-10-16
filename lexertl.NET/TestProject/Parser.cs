using System;
using System.Collections.Generic;
using lexertl;

namespace TestProject
{
    public class Parser : IParser
    {
        private StateMachine _stateMachine;

        private Token _currentToken;
        private double _numberValue;
        private string _stringValue;
        private Dictionary<string, double> _variables;
        private int _noOfErrors;

        // мои кастомные поля
        private Token[] _tokens;
        private int _pointerToToken;

        public Parser()
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
            _stateMachine = new StateMachine(rules);

            _numberValue = 0;
            _variables = new Dictionary<string, double>();
            _noOfErrors = 0;
            _tokens = null;
            _currentToken = new Token((int)TokenType.TT_SEMICOLON, ";");
        }

        public void Run()
        {
            string input = string.Empty;

            while (true)
            {
                input = Console.ReadLine();
                _tokens = _stateMachine.GetTokens(input);

                GetToken();
                if (_currentToken.Id == 0) break;
                if (_currentToken.Id == (int) TokenType.TT_SEMICOLON) continue;

                Console.WriteLine(Expr(false));

                _pointerToToken = 0;
            }
        }

        private double Expr(bool get)       // add and subtract
        {
            double left = Term(get);

            for ( ; ; )              // ``forever''
            {
                switch (_currentToken.Id)
                {
                    case (int) TokenType.TT_PLUS:
                        left += Term(true);
                        break;

                    case (int) TokenType.TT_MINUS:
                        left -= Term(true);
                        break;

                    default:
                        return left;
                }
            }
        }

        private void GetToken()
        {
            ++_pointerToToken;
            if (_pointerToToken < _tokens.Length)
                _currentToken = _tokens[_pointerToToken];
            else
                _currentToken = new Token(0, "");
        }

        private double Error(string message) // нафифга возвращать число?
        {
            _noOfErrors++;
            Console.WriteLine("error: {0}", message);
            return 1;
        }

        private double Prim(bool get)
        {
            if (get) GetToken();

            switch (_currentToken.Id)
            {
                case (int) TokenType.TT_NUMBER:        // floating-point constant                    
                    double v = _numberValue;
                    GetToken();
                    return v;

                case (int) TokenType.TT_ID:
                    double v1 = _variables[_stringValue];
                    GetToken();
                    if (_currentToken.Id == (int)TokenType.TT_ASSIGN)
                        v1 = Expr(true);
                    return v1;

                case (int) TokenType.TT_MINUS:     // unary minus
                    return -Prim(true);

                case (int) TokenType.TT_OPENING_PARENTHESIS:
                    double e = Expr(true);
                    if (_currentToken.Id == (int) TokenType.TT_CLOSING_PARENTHESIS)
                        return Error(") expected");
                    GetToken();        // eat ')'
                    return e;

                default:
                    return Error("primary expected");
            }
        }

        private double Term(bool get)       // multiply and divide
        {
            double left = Prim(get);

            for ( ; ; )
            {
                switch (_currentToken.Id)
                {
                    case (int) TokenType.TT_MULTIPLY:
                        left *= Prim(true);
                        break;

                    case (int) TokenType.TT_DIVIDE:
                        double d = Prim(true);
                        if (d == 0)
                            return Error("divide by 0");

                        left /= d;
                        break;

                    default:
                        return left;
                }

            }

        }


    }
}
