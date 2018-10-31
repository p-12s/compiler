#include "pch.h"
#include "CalcParser.h"
#include "../lexertl/iterator.hpp"

CalcParser::CalcParser()
{
	lexertl::rules rules;

	rules.push("[1-9]+[\\d]*[.]{1}[\\d]+|0{1}[.]{1}[\\d]+|[1-9]{1}[\\d]+|[\\d]{1}", TokenType::TT_NUMBER);
	rules.push("\\+", TokenType::TT_PLUS);
	rules.push("\\-", TokenType::TT_MINUS);
	rules.push("\\*", TokenType::TT_MULTIPLY);
	rules.push("\\/", TokenType::TT_DIVIDE);
	rules.push("\\=", TokenType::TT_ASSIGN);
	rules.push("[a-zA-Z_]{1}[\\w]*", TokenType::TT_ID);
	rules.push("\\;", TokenType::TT_SEMICOLON);
	rules.push("\\(", TokenType::TT_OPENING_PARENTHESIS);
	rules.push("\\)", TokenType::TT_CLOSING_PARENTHESIS);
	rules.push("[ \t\r\n]+", rules.skip());

	auto lexer = std::make_unique<lexertl::state_machine>();
	lexertl::generator::build(rules, *lexer);

	_stateMachine = std::move(lexer);
}


void CalcParser::GetToken()
{
	if (m_pointerToToken < m_tokens.size())
	{
		m_currentToken = m_tokens[m_pointerToToken];
		++m_pointerToToken;
	}
	else
	{
		m_currentToken = Token(0, "");
	}
}

double CalcParser::Calculate(const std::string& source)
{
	double result = 0;

	//_tokens = _stateMachine.GetTokens(source);
	lexertl::siterator begin(source.begin(), source.end(), *_stateMachine);
	lexertl::siterator end;
	for (auto& it = begin; it != end; ++it)
	{
		Token token{ (int)it->id, it->str() };
		m_tokens.emplace_back(token);
	}

	GetToken();
	if (m_currentToken.id == 0) return result;
	if (m_currentToken.id == TT_SEMICOLON) return result;

	result = Expr(false);
	m_pointerToToken = 0;
	return result;
}

double CalcParser::Expr(bool get)
{
	return 0.0f;
}

/*double Error(std::string message)
	{
		m_noOfErrors++;
		std"error: {0}", message);
		return 1;
	}


		private double Expr(bool get)
	{
		double left = Term(get);

		for (; ; )
		{
			switch (_currentToken.Id)
			{
			case (int)TokenType.TT_PLUS:
				left += Term(true);
				break;

			case (int)TokenType.TT_MINUS:
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
		case (int)TokenType.TT_NUMBER:
			_numberValue = Convert.ToDouble(_currentToken.Value);
			GetToken();
			return _numberValue;

		case (int)TokenType.TT_ID:
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
			if (_currentToken.Id == (int)TokenType.TT_ASSIGN)
			{
				v1 = Expr(true);
				_variables[stringValue] = v1;
			}
			return v1;

		case (int)TokenType.TT_MINUS:
			return -Prim(true);

		case (int)TokenType.TT_OPENING_PARENTHESIS:
			double e = Expr(true);
			if (_currentToken.Id != (int)TokenType.TT_CLOSING_PARENTHESIS)
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

		for (; ; )
		{
			switch (_currentToken.Id)
			{
			case (int)TokenType.TT_MULTIPLY:
				left *= Prim(true);
				break;

			case (int)TokenType.TT_DIVIDE:
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

*/