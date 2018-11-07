#include "pch.h"
#include "CalcParser.h"
#include "../lexertl/iterator.hpp"

CalcParser::CalcParser()
{
}


void CalcParser::NextToken()
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

	m_tokens = m_lexer.GetTokens(source);

	NextToken();
	if (m_currentToken.id == 0) return result;
	if (m_currentToken.id == TT_SEMICOLON) return result;

	result = Expr(false);
	m_pointerToToken = 0;

	m_tokens.clear();

	return result;
}

double CalcParser::Error(const std::string& message)
{
	std::cout << "error: " << message << std::endl;
	return 1;
}

double CalcParser::Prim(bool get)
{
	if (get) NextToken();

	std::string stringValue;
	switch (m_currentToken.id)
	{
	case TokenType::TT_NUMBER:
	{
		m_numberValue = atof(m_currentToken.value.c_str());
		NextToken();
		return m_numberValue;
	}
	case TokenType::TT_ID:
	{
		stringValue = m_currentToken.value;

		double v1;

		if (m_variables.find(m_currentToken.value) != m_variables.end())
		{
			v1 = m_variables[m_currentToken.value];
		}
		else
		{
			m_variables.emplace(m_currentToken.value, 0);
			v1 = m_variables[m_currentToken.value];
		}

		NextToken();
		if (m_currentToken.id == TokenType::TT_ASSIGN)
		{
			v1 = Expr(true);
			m_variables[stringValue] = v1;
		}
		return v1;
	}
	case TokenType::TT_MINUS:
	{
		return -Prim(true);
	}
	case TokenType::TT_OPENING_PARENTHESIS:
	{
		double e = Expr(true);
		if (m_currentToken.id != TokenType::TT_CLOSING_PARENTHESIS)
			return Error(") expected");
		NextToken();
		return e;
	}
	default:
		return Error("primary expected");		
	}
}

double CalcParser::Expr(bool get)
{
	double left = Term(get);

	for ( ; ; )
	{
		switch (m_currentToken.id)
		{
		case TokenType::TT_PLUS:
			left += Term(true);
			break;

		case TokenType::TT_MINUS:
			left -= Term(true);
			break;

		default:
			return left;
		}
	}
}

double CalcParser::Term(bool get)
{
	double left = Prim(get);

	for ( ; ; )
	{
		switch (m_currentToken.id)
		{
			case TokenType::TT_MULTIPLY:
			{
				left *= Prim(true);
				break;
			}
			case TokenType::TT_DIVIDE:
			{
				double d = Prim(true);
				if (d == 0)
					return Error("divide by 0");

				left /= d;
				break;
			}
			default:
				return left;
		}
	}
}