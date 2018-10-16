using System;
using System.Collections.Generic;
using lexertl;

namespace TestProject
{
    public interface IParser
    {
        void Run();
    }

    public class Parser : IParser
    {
        private StateMachine _stateMachine;
        private Token _currentToken;
        private double _numberValue;
        private Dictionary<string, double> _variables;
        private int _noOfErrors;
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
            _variables = new Dictionary<string, double>
            {
                { "pi", 3.1415926535897932385 },
                { "e", 2.7182818284590452354 }
            };

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

        #region Private members

        private double Expr(bool get)
        {
            double left = Term(get);

            for ( ; ; )
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
            if (_pointerToToken < _tokens.Length)
            {
                _currentToken = _tokens[_pointerToToken];
                ++_pointerToToken;
            }
            else
            {
                _currentToken = new Token(0, "");
            }
        }

        private double Error(string message)
        {
            _noOfErrors++;
            Console.WriteLine("error: {0}", message);
            return 1;
        }

        private double Prim(bool get)
        {
            if (get) GetToken();

            string stringValue;
            switch (_currentToken.Id)
            {
                case (int) TokenType.TT_NUMBER:                  
                    _numberValue = Convert.ToDouble(_currentToken.Value);
                    GetToken();
                    return _numberValue;

                case (int) TokenType.TT_ID:
                    stringValue = _currentToken.Value;
                    
                    double v1;
                    if (_variables.ContainsKey(_currentToken.Value))
                    {
                        v1 = Convert.ToDouble(_variables[_currentToken.Value]);
                    }
                    else
                    {
                        _variables.Add(_currentToken.Value, 0);
                        v1 = Convert.ToDouble(_variables[_currentToken.Value]);
                    }

                    GetToken();
                    if (_currentToken.Id == (int) TokenType.TT_ASSIGN)
                    {
                        v1 = Expr(true);
                        _variables[stringValue] = v1;
                    }
                    return v1;

                case (int) TokenType.TT_MINUS:
                    return -Prim(true);

                case (int) TokenType.TT_OPENING_PARENTHESIS:
                    double e = Expr(true);
                    if (_currentToken.Id != (int) TokenType.TT_CLOSING_PARENTHESIS)
                        return Error(") expected");
                    GetToken();
                    return e;

                default:
                    return Error("primary expected");
            }
        }

        private double Term(bool get)
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

        #endregion

    }
}
